using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CCN.Modules.Rewards.BusinessEntity;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Dapper;

namespace CCN.Modules.Rewards.DataAccess
{
    public class RewardsDataAccess : DataAccessBase
    {
        #region 会员积分

        /// <summary>
        /// 会员积分变更
        /// </summary>
        /// <param name="model">变更信息</param>
        /// <returns></returns>
        public int ChangePoint(CustPointModel model)
        {

            const string sql =
                @"insert into point_record(innerid, custid, type, sourceid, point, remark, validtime, createdtime) values (@innerid, @custid, @type, @sourceid, @point, @remark, @validtime, @createdtime);";
            const string sqlUCustCurr =
                "update cust_total_info set currpoint=currpoint+@changenum where custid=@custid;";
            const string sqlUCustInfo =
                "update cust_info set totalpoints=totalpoints+@changenum where innerid=@innerid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    //插入积分变更记录
                    conn.Execute(sql, model, tran);

                    switch (model.Type)
                    {
                        case 1: //加积分 同时要累积到会员的基本信息中，用于会员升级用等
                            conn.Execute(sqlUCustInfo, new {changenum = model.Point, innerid = model.Custid}, tran);
                            break;
                        case 2: //减积分 将积分变成负数
                            model.Point = -Math.Abs(model.Point);
                            break;
                    }

                    //变更会员的当前积分数
                    conn.Execute(sqlUCustCurr, new {changenum = model.Point, custid = model.Custid}, tran);

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取会员积分记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustPointViewModel> GetCustPointLogPageList(CustPointQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName =
                @"point_record as a left join base_code as b on a.sourceid=b.codevalue and b.typekey='point_source'";
            const string fields =
                "a.innerid, a.custid, a.type, a.point, a.remark, a.validtime, a.createdtime,b.codename as source";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder($"a.custid='{query.Custid}'");

            if (query.Type > 0)
            {
                sqlWhere.Append($" and a.type={query.Type}");
            }

            if (query.Sourceid > 0)
            {
                sqlWhere.Append($" and a.sourceid={query.Sourceid}");
            }

            if (query.MinPoint > 0)
            {
                sqlWhere.Append($" and a.point>={query.MinPoint}");
            }

            if (query.MaxPoint > 0)
            {
                sqlWhere.Append($" and a.point<={query.MaxPoint}");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<CustPointViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 积分兑换礼券
        /// </summary>
        /// <param name="model">兑换相关信息</param>
        /// <returns></returns>
        public int PointExchangeCoupon(CustPointExChangeCouponModel model)
        {
            var guid = Guid.NewGuid().ToString();
            const string sqlISent =
                @"insert into coupon_sent(innerid, cardid, custid, isreceive, createdtime, receivetime, sourceid) values (uuid(), @cardid, @custid, 1, @createdtime, @receivetime, @sourceid);";
            const string sqlIRecord =
                @"insert into point_record (innerid, custid, `type`, sourceid, `point`, remark, validtime, createdtime) values (@innerid, @custid, 100, @sourceid, @point, @remark, null, @createdtime);";
            const string sqlIExChange =
                @"insert into point_exchange (innerid, custid, recordid, `point`, `code`, createdtime) values (uuid(), @custid, @recordid, @point, @code, @createdtime);";
            const string sqlICode =
                @"insert into coupon_code (innerid, cardid, `code`, custid, gettime, sourceid, qrcode) values (uuid(), @cardid, @code, @custid, @gettime, @sourceid, @qrcode);";
            const string sqlUCoupon = "update coupon_card set count=count-1 where innerid=@cardid;";
            const string sqlUPoint = "update cust_total_info set currpoint=currpoint-@point,currpouponnum=currpouponnum+1 where custid=@custid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    //插入领取通知
                    conn.Execute(sqlISent, new
                    {
                        cardid = model.Cardid,
                        custid = model.Custid,
                        createdtime = model.Createdtime,
                        receivetime = model.Createdtime,
                        sourceid = model.Sourceid
                    }, tran);

                    //插入积分变更记录
                    conn.Execute(sqlIRecord, new
                    {
                        innerid = guid,
                        custid = model.Custid,
                        sourceid = model.Sourceid,
                        point = model.Point,
                        createdtime = model.Createdtime,
                        remark = model.Remark
                    }, tran);

                    //插入兑换礼券记录
                    conn.Execute(sqlIExChange, new
                    {
                        custid = model.Custid,
                        recordid = guid,
                        point = model.Point,
                        code = model.Code,
                        createdtime = model.Createdtime
                    }, tran);

                    //插入礼券code
                    conn.Execute(sqlICode, new
                    {
                        cardid = model.Cardid,
                        custid = model.Custid,
                        code = model.Code,
                        gettime = model.Createdtime,
                        sourceid = model.Sourceid,
                        qrcode = model.QrCode
                    }, tran);

                    //更新卡券库存
                    conn.Execute(sqlUCoupon, new {cardid = model.Cardid}, tran);

                    //更新会员的积分
                    conn.Execute(sqlUPoint, new {custid = model.Custid, point = model.Point}, tran);

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("购买礼券异常：", TraceEventType.Information, ex);
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 登录奖励验证
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public IEnumerable<CustPointModel> GetLoginPointRecord(string custid)
        {
            var date = DateTime.Now.AddDays(-22);
            var sql =
                $"select * from point_record where custid=@custid and `type`=1 and sourceid=2 and createdtime>='{date}' order by createdtime desc;";

            try
            {
                return Helper.Query<CustPointModel>(sql, new {custid});  
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取认证积分记录
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public IEnumerable<CustPointModel> GetAuthPointRecord(string custid)
        {
            const string sql =
                @"select * from point_record where custid=@custid and `type`=1 and sourceid=3;";

            try
            {
                return Helper.Query<CustPointModel>(sql, new { custid });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取今天分享获得积分记录
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public int GetSharePointRecord(string custid)
        {
            const string sql =
                @"select sum(point) as totalpoint from point_record where custid=@custid and `type`=1 and sourceid=5 and date(createdtime)=curdate();";

            try
            {
                return Helper.ExecuteScalar<int>(sql, new { custid });
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        #region 会员礼券

        /// <summary>
        /// 获取获取礼券列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CouponInfoModel> GetCouponPageList(CouponQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"coupon_card";
            const string fields =
                "innerid, title, titlesub, amount, logourl, vtype, vstart, vend, value1, value2, maxcount, count, codetype, createdtime, modifiedtime, isenabled";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            if (query.IsEnabled.HasValue)
            {
                sqlWhere.Append($" and isenabled={query.IsEnabled}");
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                sqlWhere.Append($" and title like '%{query.Title}%'");
            }

            if (!string.IsNullOrWhiteSpace(query.Titlesub))
            {
                sqlWhere.Append($" and titlesub like '%{query.Titlesub}%'");
            }

            if (query.MinAmount > 0)
            {
                sqlWhere.Append($" and amount>={query.MinAmount}");
            }

            if (query.MaxAmount > 0)
            {
                sqlWhere.Append($" and amount<={query.MaxAmount}");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<CouponInfoModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 添加礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public int AddCoupon(CouponInfoModel model)
        {
            const string sql = @"INSERT INTO coupon_card
                                (innerid, title, titlesub, amount, logourl, vtype, vstart, vend, value1, value2, maxcount, count, codetype, createdtime, modifiedtime, isenabled)
                                VALUES
                                (@innerid,@title,@titlesub,@amount,@logourl,@vtype,@vstart,@vend,@value1,@value2,@maxcount,@count,@codetype,@createdtime,@modifiedtime,@isenabled);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 修改礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public int UpdateCoupon(CouponInfoModel model)
        {
            //var sql = "update coupon_card set title=@title,titlesub=@titlesub,amount=@amount,logourl=@logourl, where innerid = @innerid";
            var sqlStr = new StringBuilder("update coupon_card set ");
            sqlStr.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sqlStr.Append(" where innerid = @innerid");
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlStr.ToString(), model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取礼券信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public CouponInfoModel GetCouponById(string innerid)
        {
            const string sql = "select a.*,b.productid from coupon_card as a left join coupon_card_product as b on a.innerid=b.cardid where a.innerid=@innerid;";

            try
            {
                var couponModel = Helper.Query<CouponInfoModel>(sql, new {innerid}).FirstOrDefault();
                return couponModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 更新礼券状态
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateStatus(string cardid,int status)
        {
            const string sql = "update coupon_card set isenabled=@isenabled where innerid=@innerid";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = cardid, isenabled = status }, tran);
                    //需要考虑已发出去的礼券这么处理
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 修改礼券库存
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public int UpdateStock(CouponInfoModel model)
        {
            const string sql =
                "update coupon_card set maxcount=maxcount+@count,count=count+@count where innerid=@innerid";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new {innerid = model.Innerid, count = model.Count}, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 修改礼券有效期
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public int UpdateValidity(CouponInfoModel model)
        {

            try
            {
                if (model.Vtype == 1)
                {
                    const string sql = "update coupon_card set vstart=@vstart,vend=@vend where innerid=@innerid";
                    Helper.Execute(sql, new
                    {
                        innerid = model.Innerid,
                        vstart = model.Vstart,
                        vend = model.Vend
                    });
                }
                else
                {
                    const string sql = "update coupon_card set value1=@value1,value2=@value2 where innerid=@innerid";
                    Helper.Execute(sql, new
                    {
                        innerid = model.Innerid,
                        value1 = model.Value1,
                        value2 = model.Value2
                    });
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 礼券与微信小店产品绑定
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public int ValidatedBindRepeat(string productid)
        {
            const string sql = @"select count(1) from coupon_card_product where productid=@productid;";

            try
            {
                var count = Helper.ExecuteScalar<int>(sql, new { productid });
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 礼券与微信小店产品绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int BindWechatProduct(CouponCardProduct model)
        {
            const string sql = @"INSERT INTO coupon_card_product (innerid, cardid, productid, createdtime)
                                VALUES (@innerid, @cardid, @productid, @createdtime);";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 礼券与微信小店产品解除绑定
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public int UnBindWechatProduct(string cardid)
        {
            const string sql = @"delete from coupon_card_product where cardid=@cardid;";

            try
            {
                Helper.Execute(sql, new { cardid});
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        #region 礼券对外接口

        /// <summary>
        /// 批量发放礼券给会员
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public int CouponToCustomer(CouponSendModel model)
        {
            const string sqlISent =
                @"insert into coupon_sent(innerid, cardid, custid, isreceive, createdtime, receivetime, sourceid) values (uuid(), @cardid, @custid, 1, @createdtime, @receivetime, @sourceid);";
            const string sqlICode =
                @"insert into coupon_code (innerid, cardid, `code`, custid, gettime, sourceid, qrcode) values (uuid(), @cardid, @code, @custid, @gettime, @sourceid, @qrcode);";
            const string sqlUCoupon = "update coupon_card set count=count-@number where innerid=@cardid;";
            const string sqlUCouponTotal = "update cust_total_info set currpouponnum=currpouponnum+@number where custid=@custid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    //插入领取通知
                    conn.Execute(sqlISent, new
                    {
                        cardid = model.Cardid,
                        custid = model.Custid,
                        createdtime = model.Createdtime,
                        receivetime = model.Createdtime,
                        sourceid = model.Sourceid
                    }, tran);

                    foreach (var item in model.ListCode)
                    {
                        //插入礼券code
                        conn.Execute(sqlICode, new
                        {
                            cardid = model.Cardid,
                            custid = model.Custid,
                            code = item.Code,
                            gettime = model.Createdtime,
                            sourceid = model.Sourceid,
                            qrcode = item.QrCode
                        }, tran);

                    }
                    
                    //更新卡券库存
                    conn.Execute(sqlUCoupon, new { cardid = model.Cardid, number = model.Number }, tran);

                    //更新会员的积分
                    conn.Execute(sqlUCouponTotal, new { custid = model.Custid, number = model.Number }, tran);

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("购买礼券异常：", TraceEventType.Error, ex);
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 根据商品id获取礼券信息
        /// </summary>
        /// <param name="productid">商品id</param>
        /// <returns></returns>
        public CouponInfoModel GetCouponByProductId(string productid)
        {
            const string sql = @"select a.* from coupon_card as a left join coupon_card_product as b on a.innerid=b.cardid where b.productid=@productid;";
            try
            {
                var couponModel = Helper.Query<CouponInfoModel>(sql, new { productid }).FirstOrDefault();
                return couponModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据openid获取custid
        /// </summary>
        /// <param name="openid">商品id</param>
        /// <returns></returns>
        public string GetCustidByOpenid(string openid)
        {
            const string sql = @"select custid from cust_wechat where openid=@openid;";
            try
            {
                var custid = Helper.Query<string>(sql, new { openid }).FirstOrDefault();
                return custid;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int CancelCoupon(string code)
        {

            return 0;
        }

        #endregion
    }
}

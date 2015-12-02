using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
                @"insert into point_record (innerid, custid, `type`, sourceid, `point`, remark, validtime, createdtime) values (@innerid, @custid, 2, 100, @point, @remark, null, @createdtime);";
            const string sqlIExChange =
                @"insert into point_exchange (innerid, custid, recordid, `point`, `code`, createdtime) values (uuid(), @custid, @recordid, @point, @code, @createdtime);";
            const string sqlICode =
                @"insert into coupon_code (innerid, cardid, `code`, custid, gettime, sourceid, qrcode,vstart,vend) values (uuid(), @cardid, @code, @custid, @gettime, @sourceid, @qrcode,@vstart,@vend);";
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
                        qrcode = model.QrCode,
                        vstart = model.Vstart?.ToShortDateString(),
                        vend = model.Vend?.ToString("yyyy-MM-dd 23:23:59")
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

        /// <summary>
        /// 获取会员统计信息
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public CustTotalModel GetCustTotalInfo(string custid)
        {
            const string sql =
                @"SELECT * FROM cust_total_info where custid=@custid;";

            try
            {
                return Helper.Query<CustTotalModel>(sql, new { custid }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 会员礼券

        /// <summary>
        /// 获取获取礼券列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CouponViewModel> GetCouponPageList(CouponQueryModel query)
        {
            /*
            礼券类型多选时的SQL（后面做）
            select 
            a.innerid, a.title, a.titlesub, a.amount, a.logourl, a.vtype, a.vstart, a.vend, a.value1, a.value2, a.maxcount, a.count,a.codetype, a.createdtime,a.modifiedtime, a.isenabled,
            (select group_concat(bc.codename) from base_code as bc inner join coupon_type_relation as tr on bc.codevalue=tr.cardtype and bc.typekey='coupon_type' where tr.cardid=a.innerid) as cardtypename,
            (select group_concat(bc.remark) from base_code as bc inner join coupon_type_relation as tr on bc.codevalue=tr.cardtype and bc.typekey='coupon_type' where tr.cardid=a.innerid) as cardtyperemark
            from
            coupon_card as a 
            where 1=1 
            and innerid in (select cardid from coupon_type_relation where cardtype in(1,2))
            order by a.createdtime desc;
            */

            const string spName = "sp_common_pager";
            const string tableName = @"coupon_card as a left join base_code as bc on a.cardtype=bc.codevalue and bc.typekey='coupon_type'";
            const string fields =
                "a.innerid, a.title, a.titlesub, a.amount, a.logourl, a.vtype, a.vstart, a.vend, a.value1, a.value2, a.maxcount, a.count,a.codetype, a.createdtime,a.modifiedtime, a.isenabled,bc.codename as cardtypename,bc.remark as cardtyperemark";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            if (query.IsEnabled.HasValue)
            {
                sqlWhere.Append($" and a.isenabled={query.IsEnabled}");
            }

            if (!string.IsNullOrWhiteSpace(query.Shopid))
            {
                sqlWhere.Append($" and a.shopid='{query.Shopid}'");
            }

            if (query.Cardtype != null)
            {
                sqlWhere.Append($" and a.cardtype={query.Cardtype}");
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                sqlWhere.Append($" and a.title like '%{query.Title}%'");
            }

            if (!string.IsNullOrWhiteSpace(query.Titlesub))
            {
                sqlWhere.Append($" and a.titlesub like '%{query.Titlesub}%'");
            }

            if (query.MinAmount > 0)
            {
                sqlWhere.Append($" and a.amount>={query.MinAmount}");
            }

            if (query.MaxAmount > 0)
            {
                sqlWhere.Append($" and a.amount<={query.MaxAmount}");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CouponViewModel>(model, query.Echo);
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
                                (innerid,shopid, title, titlesub, amount,buyprice, costprice, logourl, vtype, vstart, vend, value1, value2, maxcount, count,cardtype, codetype,needpoint,usedesc, createdtime, modifiedtime, isenabled)
                                VALUES
                                (@innerid,@shopid,@title,@titlesub,@amount,@buyprice, @costprice,@logourl,@vtype,@vstart,@vend,@value1,@value2,@maxcount,@count,@cardtype,@codetype,@needpoint,@usedesc,@createdtime,@modifiedtime,@isenabled);";
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
        public CouponViewModel GetCouponById(string innerid)
        {
            var sql = @"select a.*,b.productid,bc.codename as cardtypename,bc.remark as cardtyperemark from coupon_card as a 
                        left join coupon_card_product as b on a.innerid=b.cardid 
                        left join base_code as bc on a.cardtype=bc.codevalue and bc.typekey='coupon_type'
                        where a.innerid=@innerid;";

            try
            {
                var couponModel = Helper.Query<CouponViewModel>(sql, new {innerid}).FirstOrDefault();
                return couponModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        ///// <summary>
        ///// 获取礼券信息 by code
        ///// </summary>
        ///// <param name="code">id</param>
        ///// <returns></returns>
        //public CouponCodeInfo GetCouponByCode(string code)
        //{
        //    const string sql = "select b.*,a.isused,a.gettime from coupon_code as a left join coupon_card as b on a.cardid=b.innerid where a.code=@code;";

        //    try
        //    {
        //        var couponModel = Helper.Query<CouponCodeInfo>(sql, new { code }).FirstOrDefault();
        //        return couponModel;

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

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

        /// <summary>
        /// 获取礼券奖励规则
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CouponRuleModel> GetCouponRule()
        {
            const string sql = "select * from coupon_rule where isenabled=1;";
            try
            {
                return Helper.Query<CouponRuleModel>(sql);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取认证获得的礼券
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <param name="sourceid"></param>
        /// <returns></returns>
        public IEnumerable<CouponCodeModel> GetCouponRecord(string custid,int sourceid)
        {
            const string sql = @"select code,qrcode from coupon_code where custid=@custid and sourceid=@sourceid;";

            try
            {
                return Helper.Query<CouponCodeModel>(sql, new { custid, sourceid });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 保存购买订单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveOrder(CouponBuyModel model)
        {
            const string sql = @"INSERT INTO coupon_order
                                (innerid, order_id, order_status, order_total_price, order_create_time, order_express_price, buyer_openid, buyer_nick, receiver_name, receiver_province, receiver_city, receiver_address, receiver_mobile, receiver_phone, product_id, product_name, product_price, product_sku, product_count, product_img, delivery_id, delivery_company, trans_id, result,resultdesc,createdtime)
                                VALUES
                                (@innerid, @order_id, @order_status, @order_total_price, @order_create_time, @order_express_price, @buyer_openid, @buyer_nick, @receiver_name, @receiver_province, @receiver_city,@receiver_address, @receiver_mobile, @receiver_phone, @product_id, @product_name, @product_price, @product_sku, @product_count, @product_img, @delivery_id, @delivery_company, @trans_id, @result,@resultdesc,@createdtime);";
            try
            {
                Helper.Execute(sql, new
                {
                    model.innerid,
                    model.Order.order_id,
                    model.Order.order_status,
                    model.Order.order_total_price,
                    model.Order.order_create_time,
                    model.Order.order_express_price,
                    model.Order.buyer_openid,
                    model.Order.buyer_nick,
                    model.Order.receiver_name,
                    model.Order.receiver_province,
                    model.Order.receiver_city,
                    model.Order.receiver_address,
                    model.Order.receiver_mobile,
                    model.Order.receiver_phone,
                    model.Order.product_id,
                    model.Order.product_name,
                    model.Order.product_price,
                    model.Order.product_sku,
                    model.Order.product_count,
                    model.Order.product_img,
                    model.Order.delivery_id,
                    model.Order.delivery_company,
                    model.Order.trans_id,
                    model.result,
                    model.resultdesc,
                    createdtime = DateTime.Now
                });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改购买订单处理结果
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public OrderViewList GetOrderInfo(string innerid)
        {
            const string sql = @"select * from coupon_order where innerid=@innerid;";
            try
            {
                return Helper.Query<OrderViewList>(sql, new
                {
                    innerid
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 修改购买订单处理结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateOrderResult(CouponBuyModel model)
        {
            const string sql = @"update coupon_order set result=@result,resultdesc=@resultdesc,modifiedtime=@modifiedtime where innerid=@innerid;";
            try
            {
                Helper.Execute(sql, new
                {
                    model.innerid,
                    model.result,
                    model.resultdesc,
                    modifiedtime = DateTime.Now
                });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取发送礼券失败的订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<OrderViewList> GetOrderList(OrderQuery query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"coupon_order as a 
                                    left join cust_wechat as b on a.buyer_openid=b.openid 
                                    left join cust_info as c on b.custid=c.innerid";
            const string fields = "a.*,c.custname,c.mobile";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            switch (query.Status)
            {
                case 1:
                    sqlWhere.Append(" and a.result<>0");
                    break;
                case 2:                    
                    sqlWhere.Append(" and a.result=0");
                    break;
            }

            if (!string.IsNullOrWhiteSpace(query.ProductName))
            {
                sqlWhere.Append($" and a.product_name like '%{query.ProductName}%'");
            }

            if (!string.IsNullOrWhiteSpace(query.Mobile))
            {
                sqlWhere.Append($" and c.mobile like '%{query.Mobile}%'");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<OrderViewList>(model, query.Echo);
            return list;
        }

        #endregion

        #region 礼券Code

        /// <summary>
        /// 获取我的礼券
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<MyCodeViewListModel> GetMyCodeList(MyCodeQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"coupon_code as a 
                                        inner join coupon_card as b on a.cardid=b.innerid
                                        inner join coupon_shop as c on b.shopid=c.innerid 
                                        left join base_code as bc on b.cardtype=bc.codevalue and bc.typekey='coupon_type'";
            const string fields = "a.innerid, a.code, a.gettime,a.isused, b.shopid,b.title, b.amount,b.logourl,a.vstart, a.vend,c.shopname,bc.codename as cardtypename,bc.remark as cardtyperemark";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.gettime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder();

            sqlWhere.Append(query.Status == 1 
                ? "a.isused=0 and a.vend>=now()" 
                : "(a.isused=1 or a.vend<now())");

            if (!string.IsNullOrWhiteSpace(query.Custid))
            {
                sqlWhere.Append($" and a.custid='{query.Custid}'");
            }

            if (!string.IsNullOrWhiteSpace(query.Shopid))
            {
                sqlWhere.Append($" and b.shopid='{query.Shopid}'");
            }

            if (query.CardType != null)
            {
                sqlWhere.Append($" and b.cardtype={query.CardType}");
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                sqlWhere.Append($" and b.title like '%{query.Title}%'");
            }

            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                sqlWhere.Append($" and a.code like '%{query.Code}%'");
            }

            if (query.StartTime != null)
            {
                sqlWhere.Append($" and a.gettime>='{query.StartTime?.ToString("yyyy-MM-dd 00:00:00")}'");
            }

            if (query.EndTime != null)
            {
                sqlWhere.Append($" and a.gettime<='{query.EndTime?.ToString("yyyy-MM-dd 23:59:59")}'");
            }

            if (query.IsExpire == 1)
            {
                sqlWhere.Append($" and a.vend<='{DateTime.Now.AddDays(7).ToShortDateString()}'");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<MyCodeViewListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 我的礼券详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MyCodeViewModel GetCodeInfo(string code)
        {
            const string sql = @"select a.innerid,a.code,a.cardid,a.gettime,a.qrcode,a.vstart,a.vend,a.isused,b.shopid,b.title,b.logourl,b.amount,b.buyprice,b.isenabled,b.usedesc, c.shopname,c.area,c.address,bc.codename as cardtypename,bc.remark as cardtyperemark from coupon_code as a 
                                        inner join coupon_card as b on a.cardid=b.innerid
                                        inner join coupon_shop as c on b.shopid=c.innerid
                                        left join base_code as bc on b.cardtype=bc.codevalue and bc.typekey='coupon_type' where a.code=@code;";
            var codeModel = Helper.Query<MyCodeViewModel>(sql, new { code }).FirstOrDefault();
            return codeModel;
        }

        ///// <summary>
        ///// 根据id更新qrcode
        ///// </summary>
        ///// <param name="innerid"></param>
        ///// <param name="qrcode"></param>
        ///// <returns></returns>
        //public int UpdateQrcode(string innerid,string qrcode)
        //{
        //    const string sql = @"update coupon_code set qrcode=@qrcode where innerid=@innerid";
        //    var result = Helper.Execute(sql, new { innerid, qrcode });
        //    return result;
        //}

        /// <summary>
        /// 根据code更新qrcode
        /// </summary>
        /// <param name="code"></param>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public int UpdateQrcode(string code, string qrcode)
        {
            const string sql = @"update coupon_code set qrcode=@qrcode where code=@code";
            var result = Helper.Execute(sql, new { code, qrcode });
            return result;
        }

        /// <summary>
        /// 我的可用礼券总数
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public int GetMyUsableCodeTotal(string custid)
        {
            const string sql = @"select count(1) as count from coupon_code where custid=@custid and isused=0 and vend>=now();";
            var total = Helper.ExecuteScalar<int>(sql, new { custid });
            return total;
        }
        
        /// <summary>
        /// 根据code查询详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public CodeCancelQueryModel GetCode(string code)
        {
            const string sql = @"select a.*,b.shopid from coupon_code as a inner join coupon_card as b on a.cardid=b.innerid where code=@code;";
            var codeModel = Helper.Query<CodeCancelQueryModel>(sql, new { code }).FirstOrDefault();
            return codeModel;
        }

        /// <summary>
        /// 车商核销记录列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CodeViewListModel> GetCodeRecord(CodeQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"coupon_code as a inner join coupon_card as b on a.cardid=b.innerid";
            const string fields = "a.innerid, a.code, a.usedtime, b.title, b.amount, b.buyprice, b.costprice";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.usedtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("a.isused=1");

            if (!string.IsNullOrWhiteSpace(query.Shopid))
            {
                sqlWhere.Append($" and b.shopid='{query.Shopid}'");
            }
            
            if (query.CardType != null)
            {
                sqlWhere.Append($" and b.cardtype={query.CardType}");
            }

            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                sqlWhere.Append($" and a.code like '%{query.Code}%'");
            }
            
            if (query.StartTime != null)
            {
                sqlWhere.Append($" and a.usedtime>='{query.StartTime?.ToString("yyyy-MM-dd 00:00:00")}'");
            }

            if (query.EndTime != null)
            {
                sqlWhere.Append($" and a.usedtime<='{query.EndTime?.ToString("yyyy-MM-dd 23:59:59")}'");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CodeViewListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 车商核销记录列表合计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public CodeListSummaryModel GetCodeRecordTotal(CodeQueryModel query)
        {
            //查询条件 
            var sqlWhere = new StringBuilder("a.isused=1");

            if (!string.IsNullOrWhiteSpace(query.Shopid))
            {
                sqlWhere.Append($" and b.shopid='{query.Shopid}'");
            }

            if (query.CardType != null)
            {
                sqlWhere.Append($" and b.cardtype={query.CardType}");
            }

            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                sqlWhere.Append($" and a.code like '%{query.Code}%'");
            }

            if (query.StartTime != null)
            {
                sqlWhere.Append($" and a.usedtime>='{query.StartTime?.ToString("yyyy-MM-dd 00:00:00")}'");
            }

            if (query.EndTime != null)
            {
                sqlWhere.Append($" and a.usedtime<='{query.EndTime?.ToString("yyyy-MM-dd 23:59:59")}'");
            }
            
            var sql =
                @"select count(1) as TotalNumber,sum(b.costprice) as TotalPrice from coupon_code as a inner join coupon_card as b on a.cardid=b.innerid where " +
                sqlWhere;
            var summaryModel = Helper.Query<CodeListSummaryModel>(sql).FirstOrDefault();
            return summaryModel;
        }

        #endregion

        #region 礼券商城

        /// <summary>
        /// 获取礼券列表（购买）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CouponViewModel> GetMallCouponPageList(CouponMallQuery query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"coupon_card as a 
                                        left join coupon_shop as s on a.shopid=s.innerid
                                        left join base_code as bc on a.cardtype=bc.codevalue and bc.typekey='coupon_type'
                                        left join coupon_card_product as p on a.innerid=p.cardid";
            const string fields =
                @"a.innerid, a.title, a.titlesub, a.amount,a.buyprice, a.logourl, a.vtype, a.vstart, a.vend, a.value1, a.value2, a.maxcount, a.count,a.codetype, a.createdtime,s.area as ShopArea, s.address as ShopAddress,s.shopname as Shopname,bc.codename as cardtypename,bc.remark as cardtyperemark,p.productid as ProductUrl,
                    (select count(1) from coupon_code where cardid=a.innerid and sourceid = 1) as SoldedNum";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("a.isenabled=1");

            if (!string.IsNullOrWhiteSpace(query.Shopid))
            {
                sqlWhere.Append($" and a.shopid='{query.Shopid}'");
            }

            if (!string.IsNullOrWhiteSpace(query.CardTypes))
            {
                sqlWhere.Append($" and a.cardtype in ({query.CardTypes})");
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                sqlWhere.Append($" and a.title like '%{query.Title}%'");
            }
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CouponViewModel>(model, query.Echo);
            return list;
        }
        
        /// <summary>
        /// 商城搜索商户列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<ShopMallViewList> GetMallShopPageList(ShopMallQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"coupon_shop as a";
            const string fields =
                @"innerid,shopname,area,address,
                (select count(1) from coupon_code as cc inner join coupon_card as ccard on cc.cardid=ccard.innerid where ccard.shopid=a.innerid and cc.sourceid=1) as SoldedNum,
                (select group_concat(codename) from base_code where typekey='coupon_type' and codevalue in (select cardtype from coupon_card where shopid=a.innerid )) as CardTypeNames,
                (select count(1) from base_code where typekey='coupon_type' and codevalue in (select cardtype from coupon_card where shopid=a.innerid )) as sort";

            //select count(1) from coupon_code where sourceid = 1 and cardid in (select innerid from coupon_card where shopid='2550607b-6c12-48b6-a72a-c1fe4ec354c5') as SoldedNum;

            query.Order = "sort desc,a.createdtime desc";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("a.status=1");
            
            if (!string.IsNullOrWhiteSpace(query.Shopname))
            {
                sqlWhere.Append($" and a.shopname like '%{query.Shopname}%'");
            }
            
            if (!string.IsNullOrWhiteSpace(query.Area))
            {
                sqlWhere.Append($" and a.area='{query.Area}'");
            }

            if (!string.IsNullOrWhiteSpace(query.CardTypes))
            {
                sqlWhere.Append($" and a.innerid in (select shopid from coupon_card where cardtype in ({query.CardTypes}))");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<ShopMallViewList>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 查询该商户下有多少种券
        /// </summary>
        /// <param name="shopid">查询条件</param>
        /// <returns></returns>
        public IEnumerable<string> GetMallShopCardTypeNameList(string shopid)
        {
            const string sql = "select distinct bc.codename from coupon_card as a inner join base_code as bc on a.cardtype=bc.codevalue and bc.typekey='coupon_type' where shopid = @shopid;";
            return Helper.Query<string>(sql,new { shopid });
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
                @"insert into coupon_code (innerid, cardid, `code`, custid, gettime, sourceid, qrcode,vstart,vend) values (uuid(), @cardid, @code, @custid, @gettime, @sourceid, @qrcode,@vstart,@vend);";
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
                            qrcode = item.QrCode,
                            vstart = item.Vstart?.ToShortDateString(),
                            vend = item.Vend?.ToString("yyyy-MM-dd 23:23:59")
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
                    LoggerFactories.CreateLogger().Write("批量发放礼券给会员异常：", TraceEventType.Error, ex);
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

        /// <summary>
        /// 礼券核销
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int CancelCoupon(string code)
        {
            const string sqlUCode = "update coupon_code set isused=1,usedtime=@usedtime where `code`=@code;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    var nowDate = DateTime.Now;

                    //更新礼券code
                    conn.Execute(sqlUCode, new {usedtime = nowDate, code}, tran);

                    //更新会员的礼券数
                    var sqlUCust = "update cust_total_info set currpouponnum=currpouponnum-1 where custid=(select custid from coupon_code where `code`=@code);";
                    conn.Execute(sqlUCust, new { code }, tran);
                    
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("核销礼券异常：", TraceEventType.Information, ex);
                    tran.Rollback();
                    return 0;
                }
            }
        }
        
        /// <summary>
        /// 查询已核销的礼券
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<CardCancelSummaryModel> GetCoupon(CardCancelSummaryQueryModel query)
        {
            const string sqlSelect =
                @"select a.innerid as Cardid, a.title, a.titlesub, a.logourl, a.amount, a.buyprice, a.costprice, a.maxcount, a.count, a.cardtype, a.codetype, a.createdtime, 
                (select count(1)
                from coupon_code where isused = 1 and usedtime > @starttime and usedtime < @endtime
                and cardid=a.innerid 
                and `code` not in (select `code` from coupon_settcoderecord where cardid = a.innerid)) as CanedCount
                from coupon_card as a where a.shopid=@shopid;";
            
            //更新礼券code
            return Helper.Query<CardCancelSummaryModel>(sqlSelect,
                new {
                    starttime = query.StartTime?.ToString("yyyy-MM-dd 00:00:00"),
                    endtime = query.EndTime?.ToString("yyyy-MM-dd 23:59:59"),
                    shopid = query.Shopid});
        }
        
        #endregion

        #region 商户管理

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        public ShopModel GetShopModel(string shopcode, string password)
        {
            const string sqlSelect =
                "select innerid, shopname, shopcode, password, telephone, email, headportrait, status, provid, cityid, area, address, qq, signature, qrcode, createdtime, modifiedtime from coupon_shop where shopcode=@shopcode and password=@password";

            //更新礼券code
            return Helper.Query<ShopModel>(sqlSelect, new { shopcode, password }).FirstOrDefault();
        }

        /// <summary>
        /// 根据id获取商户信息
        /// </summary>
        /// <returns></returns>
        public ShopModel GetShopById(string innerid)
        {
            const string sqlSelect =
                "select innerid, shopname, shopcode, password, telephone, email, headportrait, status, provid, cityid, area, address, qq, signature, qrcode, createdtime, modifiedtime from coupon_shop where innerid=@innerid";

            //更新礼券code
            return Helper.Query<ShopModel>(sqlSelect, new { innerid }).FirstOrDefault();
        }

        /// <summary>
        /// 根据id获取商户信息（包含关联信息）
        /// </summary>
        /// <returns></returns>
        public ShopInfoViewModel GetShopViewById(string innerid)
        {
            const string sqlSelect =
                @"select a.innerid, a.shopname, a.shopcode, a.telephone, a.email, a.headportrait, a.status, pr.provname, ct.cityname, a.area, a.address, a.qq, a.signature, a.qrcode, a.createdtime from coupon_shop as a 
                left join base_province as pr on a.provid=pr.innerid
                left join base_city as ct on a.cityid=ct.innerid 
                where a.innerid=@innerid";

            //更新礼券code
            return Helper.Query<ShopInfoViewModel>(sqlSelect, new { innerid }).FirstOrDefault();
        }

        /// <summary>
        /// 验证商户名重复
        /// </summary>
        /// <returns></returns>
        public int CheckShopName(string shopname)
        {
            const string sql = "select count(1) as count from coupon_shop where shopname=@shopname;";
            try
            {
                return Helper.ExecuteScalar<int>(sql, new {shopname});
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("验证商户名重复异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取最大
        /// </summary>
        /// <returns></returns>
        public int GetMaxCode()
        {
            const string sql = "select max(code) as maxcode from coupon_shop";
            try
            {
                return Helper.ExecuteScalar<int>(sql, new {  });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("验证商户登录名重复异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <returns></returns>
        public int AddShop(ShopModel model)
        {
            const string sql = "insert into coupon_shop (innerid, shopname, shopcode, password, telephone, email, headportrait, status, provid, cityid, area,address, qq, signature, qrcode, createdtime, modifiedtime,code) " +
                               "values (@innerid, @shopname, @shopcode, @password, @telephone, @email, @headportrait, @status, @provid, @cityid, @area,@address, @qq, @signature, @qrcode, @createdtime, @modifiedtime,@code);";
            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("添加商户异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 更新商户
        /// </summary>
        /// <returns></returns>
        public int UpdateShop(ShopModel model)
        {
            var sqlStr = new StringBuilder("update coupon_shop set ");
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
                    LoggerFactories.CreateLogger().Write("更新商户异常：", TraceEventType.Error, ex);
                    tran.Rollback();
                    return 0;
                }
            }
        }
        
        /// <summary>
        /// 修改商户状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateShopStatus(string innerid, int status)
        {
            const string sql = "update coupon_shop set status=@status where innerid=@innerid;";
            var result = Helper.Execute(sql, new
            {
                innerid,
                status
            });
            return result;
        }

        /// <summary>
        /// 删除商户
        /// </summary>
        /// <returns></returns>
        public int DeleteShop(string innerid)
        {
            const string sql = "delete from coupon_shop where innerid=@innerid;";
            try
            {
                Helper.Execute(sql, new { innerid });
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("删除商户异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 商户列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<ShopViewModel> GetShopPageList(ShopQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName =
                @"coupon_shop as a 
                left join base_province as pr on a.provid=pr.innerid
                left join base_city as ct on a.cityid=ct.innerid ";
            const string fields =
                "a.innerid, a.shopname, a.shopcode, a.telephone, a.email, a.headportrait, a.createdtime,a.status,pr.provname,ct.cityname,a.area,a.address";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            sqlWhere.Append(query.Status != null
                ? $" and a.status={query.Status}"
                : "");

            if (!string.IsNullOrWhiteSpace(query.Shopname))
            {
                sqlWhere.Append($" and shopname like '%{query.Shopname}%'");
            }
            
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<ShopViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 根据id获取商户信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemShop> GetShopList()
        {
            const string sqlSelect =
                "select innerid as Value, shopname as Text from coupon_shop where status=1";

            //更新礼券code
            return Helper.Query<ItemShop>(sqlSelect);
        }

        #endregion

        #region 商户职员管理
        
        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        public ShopStaffInfo GetShopStaffModel(string loginname, string password)
        {
            const string sqlSelectStaff =
                "select innerid,shopid, loginname, password, staffname, sex, mobile, email, status, createdtime, modifiedtime from coupon_shop_staff where loginname=@loginname and password=@password";

            const string sqlSelectShop =
                "select innerid, shopname, shopcode, password, telephone, email, headportrait, status, provid, cityid, area,address, qq, signature, qrcode, createdtime, modifiedtime from coupon_shop where shopid=@shopid";

            var shopStaffInfo = new ShopStaffInfo();
            using (var conn = Helper.GetConnection())
            {
                try
                {
                    shopStaffInfo.StaffModel = conn.Query<ShopStaffModel>(sqlSelectStaff, new { loginname , password }).FirstOrDefault();
                    if (shopStaffInfo.StaffModel != null)
                    {
                        shopStaffInfo.ShopModel = conn.Query<ShopModel>(sqlSelectShop, new { shopid = shopStaffInfo.StaffModel.Shopid }).FirstOrDefault();
                    }
                    return shopStaffInfo;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("更新商户异常：", TraceEventType.Error, ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// 根据id获取商户职员信息
        /// </summary>
        /// <returns></returns>
        public ShopStaffModel GetShopStaffById(string innerid)
        {
            const string sqlSelect =
                "select innerid,shopid, loginname, staffname, sex, mobile, email, status, createdtime, modifiedtime from coupon_shop_staff where innerid=@innerid";

            //更新礼券code
            return Helper.Query<ShopStaffModel>(sqlSelect, new { innerid }).FirstOrDefault();
        }

        /// <summary>
        /// 验证商户职员名重复
        /// </summary>
        /// <returns></returns>
        public int CheckShopStaffName(string staffname)
        {
            const string sql = "select count(1) as count from coupon_shop_staff where staffname=@staffname;";
            try
            {
                return Helper.ExecuteScalar<int>(sql, new { staffname });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("验证商户职员名重复异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 验证商户职员登录名重复
        /// </summary>
        /// <returns></returns>
        public int CheckShopStaffLoginName(string staffname)
        {
            const string sql = "select count(1) as count from coupon_shop_staff where loginname=@loginname;";
            try
            {
                return Helper.ExecuteScalar<int>(sql, new { staffname });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("验证商户职员登录名重复异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 添加职员
        /// </summary>
        /// <returns></returns>
        public int AddShopStaff(ShopStaffModel model)
        {
            const string sql = "insert into coupon_shop_staff (innerid,shopid, loginname, staffname, sex, mobile, email, status, createdtime, modifiedtime) " +
                               "values (@innerid,@shopid, @loginname, @staffname, @sex, @mobile, @email, @status, @createdtime, @modifiedtime);";
            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("添加职员异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 更新商户Staff
        /// </summary>
        /// <returns></returns>
        public int UpdateShopStaff(ShopStaffModel model)
        {
            var sqlStr = new StringBuilder("update coupon_shop_staff set ");
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
                    LoggerFactories.CreateLogger().Write("更新商户Staff异常：", TraceEventType.Error, ex);
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 修改商户Staff状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateShopStaffStatus(string innerid, int status)
        {
            const string sql = "update coupon_shop_staff set status=@status where innerid=@innerid;";
            var result = Helper.Execute(sql, new
            {
                innerid,
                status
            });
            return result;
        }

        /// <summary>
        /// 删除商户Staff
        /// </summary>
        /// <returns></returns>
        public int DeleteShopStaff(string innerid)
        {
            const string sql = "delete from coupon_shop_staff where innerid=@innerid;";
            try
            {
                Helper.Execute(sql, new { innerid });
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("删除商户Staff异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 商户职员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<ShopStaffViewModel> GetShopStaffPageList(ShopStaffQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName =
                @"coupon_shop_staff as a 
                inner join coupon_shop as b on a.shopid=b.innerid";
            const string fields =
                "a.innerid,a.shopid, a.loginname, a.staffname, a.sex, a.mobile, a.email, a.status, a.createdtime, a.modifiedtime,b.shopname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            sqlWhere.Append(query.Status != null
                ? $" and a.status={query.Status}"
                : "");

            if (!string.IsNullOrWhiteSpace(query.Staffname))
            {
                sqlWhere.Append($" and a.staffname like '%{query.Staffname}%'");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<ShopStaffViewModel>(model, query.Echo);
            return list;
        }

        #endregion

        #region 结算记录

        /// <summary>
        /// 添加结算记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddSettLog(SettlementLogModel model)
        {
            const string sql = "insert into coupon_settlement (innerid, shopid, orderid, setttime, setttotal, settcyclestart, settcycleend, settserialnum, settaccount,pictures) values (@innerid, @shopid, @orderid, @setttime, @setttotal, @settcyclestart, @settcycleend, @settserialnum, @settaccount,@pictures);";
            const string sqlRecord = @"insert into coupon_settcoderecord (innerid,shopid,cardid, settid, `code`, createdtime, creater) 

                                        select uuid(), @shopid, cardid, @settid,`code`, @nowtime, ''
                                        from coupon_code where isused = 1 and usedtime > @starttime and usedtime < @endtime
                                        and cardid in (select innerid from coupon_card where shopid = @shopid) 
                                        and `code` not in (select `code` from coupon_settcoderecord where shopid = @shopid); ";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql,model, tran);
                    conn.Execute(sqlRecord, new
                    {
                        shopid = model.Shopid,
                        settid = model.Innerid,
                        nowtime = DateTime.Now,
                        starttime = model.SettCycleStart?.ToString("yyyy-MM-dd 00:00:00"),
                        endtime = model.SettCycleEnd?.ToString("yyyy-MM-dd 23:59:59")
                    }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("add结算记录异常：", TraceEventType.Error, ex);
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 修改结算记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateSettLog(SettlementLogModel model)
        {
            var sqlStr = new StringBuilder("update coupon_settlement set ");
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
                    LoggerFactories.CreateLogger().Write("更新结算记录异常：", TraceEventType.Error, ex);
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 删除结算记录
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DelSettLog(string innerid)
        {
            const string sql = "delete from coupon_settlement where innerid=@innerid;";
            const string sqlRecord = "delete from coupon_settcoderecord where settid=@settid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid },tran);
                    conn.Execute(sqlRecord, new {settid = innerid}, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("删除结算记录异常：", TraceEventType.Error, ex);
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 更新结算记录图片
        /// </summary>
        /// <param name="innerid">记录id</param>
        /// <param name="pictures"></param>
        /// <returns></returns>
        public int UpdateSettLogPic(string innerid,string pictures)
        {
            const string sql = "update coupon_settlement set pictures=@pictures where innerid=@innerid;";
            try
            {
                Helper.Execute(sql, new { innerid, pictures });
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("更新结算记录图片异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 根据id获取结算记录信息
        /// </summary>
        /// <returns></returns>
        public SettlementLogModel GetSettLogById(string innerid)
        {
            const string sqlSelect =
                "select * from coupon_settlement where innerid=@innerid";

            //更新礼券code
            return Helper.Query<SettlementLogModel>(sqlSelect, new { innerid }).FirstOrDefault();
        }

        /// <summary>
        /// 结算记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<SettlementLogViewModel> GetSettLogPageList(SettlementLogQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"coupon_settlement as a inner join coupon_shop as b on a.shopid=b.innerid";
            const string fields = "a.*,b.shopname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.setttime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            if (!string.IsNullOrWhiteSpace(query.Shopid))
            {
                sqlWhere.Append($" and a.shopid='{query.Shopid}'");
            }

            if (!string.IsNullOrWhiteSpace(query.OrderidOrNumber))
            {
                sqlWhere.Append(
                    $" and (a.orderid like '%{query.OrderidOrNumber}%' or a.settserialnum like '%{query.OrderidOrNumber}%')");
            }
            
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<SettlementLogViewModel>(model, query.Echo);
            return list;
        }


        /// <summary>
        /// 根据cardid获取已核销的code列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CodeViewListModel> GetUsedCodePageList(UsedCodeQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"coupon_code";
            const string fields = "code,usedtime";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "usedtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            if (!string.IsNullOrWhiteSpace(query.Cardid))
            {
                sqlWhere.Append($" and cardid='{query.Cardid}'");
            }

            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                sqlWhere.Append($" and code like '%{query.Code}%'");
            }
            
            if (query.StartTime != null)
            {
                sqlWhere.Append($" and usedtime>='{query.StartTime?.ToString("yyyy-MM-dd 00:00:00")}'");
            }

            if (query.EndTime != null)
            {
                sqlWhere.Append($" and usedtime<='{query.EndTime?.ToString("yyyy-MM-dd 23:59:59")}'");
            }
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<CodeViewListModel>(model, query.Echo);
            return list;
        }


        /// <summary>
        /// 根据settid获取已结算的code列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<SettedCodeViewListModel> GetSettedCodePageList(SettedCodeQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"coupon_settcoderecord as a 
                                    left join coupon_card as b on a.cardid=b.innerid
                                    left join coupon_code as c on a.`code`=c.`code`
                                    left join cust_info as ci on ci.innerid=c.custid";
            const string fields = "a.`code`,c.usedtime,b.title,ci.custname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "b.innerid,c.usedtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            if (!string.IsNullOrWhiteSpace(query.Settid))
            {
                sqlWhere.Append($" and a.settid='{query.Settid}'");
            }
            
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<SettedCodeViewListModel>(model, query.Echo);
            return list;
        }

        #endregion

        #region 商户区处理

        /// <summary>
        /// 根据城市id获取区列表
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public IEnumerable<string> GetShopAreaByCityid(string cityid)
        {
            const string sqlSelect = "select area from coupon_shop where cityid=@cityid group by area order by area desc;";
            return Helper.Query<string>(sqlSelect, new { cityid });
        }

        /// <summary>
        /// 根据区获取商户列表
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public IEnumerable<ItemShop> GetShopByArea(string area)
        {
            const string sqlSelect = "select innerid as Value, shopname as Text from coupon_shop where area=@area order by shopname desc;";
            return Helper.Query<ItemShop>(sqlSelect, new { area });
        }

        #endregion

        #region 可能存在并发问题


        /// <summary>
        /// 积分兑换礼券
        /// </summary>
        /// <param name="model">兑换相关信息</param>
        /// <returns></returns>
        public int PointToCoupon(CustPointToCouponModel model)
        {
            using (var conn = Helper.GetConnection())
            {
                //参数
                var obj = new
                {
                    p_custid = model.Custid,
                    p_cardid = model.Cardid,
                    p_code = model.Code,
                    p_remark = model.Remark
                };

                var args = new DynamicParameters(obj);
                args.Add("p_value", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (conn.QueryMultiple("ccnsp_pointtocoupon", args, commandType: CommandType.StoredProcedure))
                {
                    //获取结果集
                    //var data = result.Read<T>();
                }

                return args.Get<int>("p_value");
            }
        }

        #endregion
    }
}

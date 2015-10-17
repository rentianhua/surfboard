#region

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CCN.Modules.Customer.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
using Cedar.Framework.Common.BaseClasses;
using Dapper;
using MySql.Data.MySqlClient;

#endregion

namespace CCN.Modules.Customer.DataAccess
{
    /// <summary>
    /// </summary>
    public class CustomerDA : CustomerDataAccessBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomerDA()
        {

        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetALlCustomers()
        {
            var d = Helper.Query("select * from base_carbrand where id=@id", new { id = "" }).ToList();
            return d;
        }


        #region 用户模块

        /// <summary>
        /// 会员注册检查Email是否被注册
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>0：未被注册，非0：Email被注册</returns>
        public int CheckEmail(string email)
        {
            const string sql = @"select count(1) as count from cust_info where email=@email;";
            var result = Helper.ExecuteScalar<int>(sql, new {email});
            return result;
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：被注册</returns>
        public int CheckMobile(string mobile)
        {
            const string sql = @"select count(1) from `cust_info` where mobile=@mobile;";
            var result = Helper.ExecuteScalar<int>(sql, new { mobile });
            return result;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public int CustRegister(CustModel userInfo)
        {
            //插入账户基本信息
            const string sql = @"INSERT INTO `cust_info`(`innerid`,`custname`,`password`,`mobile`,`telephone`,`email`,`headportrait`,qrcode,`status`,authstatus,`type`,`realname`,`totalpoints`,`level`,`createdtime`,`modifiedtime`)
                        VALUES (@innerid,@custname,@password,@mobile,@telephone,@email,@headportrait,@qrcode,@status,@authstatus,@type,@realname,@totalpoints,@level,@createdtime,@modifiedtime);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, userInfo ,tran);

                    //插入会员的总数信息
                    const string sqlTotal = "insert into cust_total_info (innerid, custid) values (uuid(),@custid);";
                    conn.Execute(sqlTotal, new { custid  = userInfo.Innerid }, tran);

                    //插入微信信息
                    if (userInfo.Wechat != null)
                    {
                        const string sqlwechat =
                            @"INSERT INTO cust_wechat(`innerid`,`custid`,`accountid`,`openid`,`nickname`,`headportrait`,`sex`,`country`,`province`,`city`,`createdtime`,`modifiedtime`)
                        VALUES(uuid(),@custid,@accountid,@openid,@nickname,@headportrait,@sex,@country,@province,@city,@createdtime,@modifiedtime);";
                        conn.Execute(sqlwechat, userInfo.Wechat, tran);
                    }
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
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        public CustModel CustLogin(CustLoginInfo loginInfo)
        {
            const string sql = "select * from `cust_info` where mobile=@mobile and password=@password;";
            var custModel = Helper.Query<CustModel>(sql, new
            {
                mobile = loginInfo.Mobile,
                password = loginInfo.Password
            }).FirstOrDefault();

            //获取微信信息
            if (custModel != null)
            {
                custModel.Wechat = Helper.Query<CustWechat>("select * from cust_wechat where custid=@custid;", new
                {
                    custid = custModel.Innerid
                }).FirstOrDefault();
            }

            return custModel;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="qrcode"></param>
        /// <returns>用户信息</returns>
        public int UpdateQrCode(string innerid,string qrcode)
        {
            const string sql = "update cust_info set qrcode=@qrcode where innerid=@innerid;";
            var custModel = Helper.Execute(sql, new {qrcode, innerid});
            return custModel;
        }

        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        public CustModel GetCustById(string innerid)
        {
            const string sql = "select * from cust_info where innerid=@innerid;";

            try
            {
                var custModel = Helper.Query<CustModel>(sql, new {innerid}).FirstOrDefault();
                //获取微信信息
                if (custModel != null)
                {
                    custModel.Wechat = Helper.Query<CustWechat>("select * from cust_wechat where custid=@custid;", new
                    {
                        custid = innerid
                    }).FirstOrDefault();
                }
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustModel> GetCustPageList(CustQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"cust_info";
            const string fields = "innerid, custname, mobile, telephone, email, headportrait, status, authstatus, autherid, authtime, authdesc, type, realname, totalpoints, level, createdtime, modifiedtime";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            sqlWhere.Append(query.Status != null
                ? $" and status={query.Status}"
                : "");

            if (!string.IsNullOrWhiteSpace(query.Mobile))
            {
                sqlWhere.Append($" and mobile like '%{query.Mobile}%'");
            }
            
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CustModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="mRetrievePassword"></param>
        /// <returns></returns>
        public int UpdatePassword(CustRetrievePassword mRetrievePassword)
        {
            const string sql = "update cust_info set password=@password where mobile=@mobile;";
            var custModel = Helper.Execute(sql, new {
                password = mRetrievePassword.NewPassword,
                mobile = mRetrievePassword.Mobile
            });
            return custModel;
        }

        #endregion

        #region 用户认证

        /// <summary>
        /// 用户添加认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public int AddAuthentication(CustAuthenticationModel model)
        {
            const string sqlU = "update cust_info set authstatus=1 where innerid=@innerid;";
            const string sqlI = @"INSERT INTO `cust_authentication`
                                (`innerid`,`custid`,`idcard`,`idname`,`idpicture`,`company`,`legalperson`,`organizationcode`,`organizationpicture`,`createdtime`,`modifiedtime`)
                                VALUES
                                (uuid(),@custid,@idcard,@idname,@idpicture,@company,@legalperson,@organizationcode,@organizationpicture,@createdtime,@modifiedtime);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlU, new {innerid = model.Custid}, tran);
                    conn.Execute(sqlI, model, tran);
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
        /// 用户修改认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public int UpdateAuthentication(CustAuthenticationModel model)
        {
            var sqlStr = new StringBuilder("update `cust_authentication` set ");
            sqlStr.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sqlStr.Append(" where innerid = @innerid");

            const string sqlU = "update cust_info set authstatus=1 where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlU, new { innerid = model.Custid }, tran);
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
        /// 审核认证信息
        /// </summary>
        /// <param name="info">会员相关信息</param>
        /// <param name="operid">操作人id</param>
        /// <returns></returns>
        public int AuditAuthentication(CustModel info,string operid)
        {
            const string sql = "update cust_info set authstatus=@authstatus,autherid=@autherid,authdesc=@authdesc,authtime=@authtime where innerid=@innerid;";

            try
            {
                Helper.Execute(sql, new
                {
                    innerid = info.Innerid,
                    authstatus = info.AuthStatus,
                    autherid = operid,
                    authtime = info.AuthTime,
                    authdesc = info.AuthDesc
                });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取会员认证信息 by innerid
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public CustAuthenticationModel GetCustAuthById(string innerid)
        {
            const string sql = "select * from cust_authentication where innerid=@innerid;";

            try
            {
                var custModel = Helper.Query<CustAuthenticationModel>(sql, new { innerid }).FirstOrDefault();
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取会员认证信息 by custid
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public CustAuthenticationModel GetCustAuthByCustid(string custid)
        {
            const string sql = "select * from cust_authentication where custid=@custid;";

            try
            {
                var custModel = Helper.Query<CustAuthenticationModel>(sql, new { custid }).FirstOrDefault();
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 会员标签


        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        public int AddTag(CustTagModel model)
        {
            const string sql = @"INSERT INTO cust_tag(innerid, tagname, hotcount, isenabled, createdtime, modifiedtime) VALUES (uuid(), @tagname, @hotcount, @isenabled, @createdtime, @modifiedtime);";

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
        /// 修改标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        public int UpdateTag(CustTagModel model)
        {
            const string sql = @"UPDATE cust_tag SET tagname = @tagname,modifiedtime = @modifiedtime WHERE innerid=@innerid;";

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
        /// 修改标签状态
        /// </summary>
        /// <param name="innerid">标签ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateTagStatus(string innerid,int status)
        {
            const string sql = @"UPDATE cust_tag SET isenabled = @isenabled WHERE innerid = @innerid;";

            try
            {
                Helper.Execute(sql, new { innerid, isenabled = status });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        public int DeleteTag(string innerid)
        {
            const string sql = @"delete from cust_tag WHERE innerid=@innerid;";

            try
            {
                Helper.Execute(sql, new { innerid });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取标签详情
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        public CustTagModel GetTagById(string innerid)
        {
            const string sql = @"select innerid, tagname, hotcount, isenabled, createdtime, modifiedtime from cust_tag WHERE innerid=@innerid;";

            try
            {
                return Helper.Query<CustTagModel>(sql, new { innerid }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustTagModel> GetTagPageList(CustTagQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"cust_tag";
            const string fields = "innerid, tagname, hotcount, isenabled, createdtime, modifiedtime";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            sqlWhere.Append(query.Isenabled != null
                ? $" and status={query.Isenabled}"
                : "");

            if (!string.IsNullOrWhiteSpace(query.Tagname))
            {
                sqlWhere.Append($" and tagname like '%{query.Tagname}%'");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CustTagModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 打标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DoTagRelation(CustTagRelation model)
        {
            const string sql = @"INSERT INTO cust_tag_relation(innerid,tagid,fromid,toid,createdtime) VALUES (@innerid,@tagid,@fromid,@toid,@createdtime);";
            const string sqlH = @"UPDATE cust_tag SET hotcount=hotcount+1 WHERE innerid = @innerid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    //插入关系
                    conn.Execute(sql, model, tran);
                    //更新热度
                    conn.Execute(sqlH, new {innerid = model.Tagid}, tran);

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
        /// 删除标签关系
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DelTagRelation(string innerid)
        {
            const string sql = @"delete from cust_tag_relation WHERE innerid=@innerid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    //删除关系
                    conn.Execute(sql, new { innerid }, tran);

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
        /// 获取会员拥有的标签
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTagRelation(string custid)
        {
            const string sql = @"select tagid,tagname from cust_tag_relation as a
                                left join cust_tag as b on a.tagid=b.innerid 
                                WHERE toid=@custid group by tagid;";

            var list = Helper.Query<dynamic>(sql, new {custid});
            return list;
        }

        /// <summary>
        /// 获取会员该标签的操作者
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="tagid"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTagRelationWithOper(string custid ,string tagid)
        {
            const string sql = @"select b.custname, a.createdtime from cust_tag_relation as a 
                                left join cust_info as b on a.fromid=b.innerid 
                                where tagid=@tagid and toid=@custid;";

            var list = Helper.Query<dynamic>(sql, new {tagid, custid});
            return list;
        }

        #endregion

        #region 会员积分

        /// <summary>
        /// 会员积分变更
        /// </summary>
        /// <param name="model">变更信息</param>
        /// <returns></returns>
        public int ChangePoint(CustPointModel model)
        {
            
            const string sql = @"insert into point_record(innerid, custid, type, sourceid, point, remark, validtime, createdtime) values (@innerid, @custid, @type, @sourceid, @point, @remark, @validtime, @createdtime);";            
            const string sqlUCustCurr = "update cust_total_info set currpoint=currpoint+@changenum where custid=@custid;";
            const string sqlUCustInfo = "update cust_info set totalpoints=totalpoints+@changenum where innerid=@innerid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    //插入积分变更记录
                    conn.Execute(sql, model,tran);
                                       
                    switch (model.Type)
                    {
                        case 1: //加积分 同时要累积到会员的基本信息中，用于会员升级用等
                            conn.Execute(sqlUCustInfo, new {changenum = model.Point, innerid = model.Custid}, tran);
                            break;
                        case 2: //减积分 将积分变成负数
                            model.Point = - Math.Abs(model.Point);
                            break;
                    }
                    
                    //变更会员的当前积分数
                    conn.Execute(sqlUCustCurr, new { changenum = model.Point, custid = model.Custid }, tran);

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
            const string tableName = @"point_record as a left join base_code as b on a.sourceid=b.codevalue and b.typekey='point_source'";
            const string fields = "a.innerid, a.custid, a.type, a.point, a.remark, a.validtime, a.createdtime,b.codename as source";
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

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
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
            const string sqlISent = @"insert into coupon_sent(innerid, cardid, custid, isreceive, createdtime, receivetime, sourceid) values (uuid(), @cardid, @custid, 1, @createdtime, @receivetime, @sourceid);";
            const string sqlIRecord = @"insert into point_record (innerid, custid, `type`, sourceid, `point`, remark, validtime, createdtime) values (@innerid, @custid, 2, @sourceid, @point, @remark, null, @createdtime);";
            const string sqlIExChange = @"insert into point_exchange (innerid, custid, recordid, `point`, `code`, createdtime) values (uuid(), @custid, @recordid, @point, @code, @createdtime);";
            const string sqlICode = @"insert into coupon_code (innerid, cardid, `code`, custid, gettime, sourceid, qrcode) values (uuid(), @cardid, @code, @custid, @gettime, @sourceid, @qrcode);";
            const string sqlUCoupon = "update coupon_card set count=count-1 where innerid=@cardid;";
            const string sqlUPoint = "update cust_total_info set currpoint=currpoint-@point where custid=@custid;";

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
                        point = model.Point,
                        qrcode = model.QrCode
                    }, tran);

                    //更新卡券库存
                    conn.Execute(sqlUCoupon, new { cardid = model.Cardid }, tran);

                    //更新会员的积分
                    conn.Execute(sqlUPoint, new {custid = model.Custid, point = model.Point}, tran);

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
            const string fields = "innerid, title, titlesub, amount, logourl, vtype, vstart, vend, value1, value2, maxcount, count, codetype, createdtime, modifiedtime, isenabled";
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

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
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
            const string sql = "select * from coupon_card where innerid=@innerid;";

            try
            {
                var custModel = Helper.Query<CouponInfoModel>(sql, new { innerid }).FirstOrDefault();
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion
    }
}
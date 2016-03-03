#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using CCN.Modules.Customer.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
using Cedar.Core.EntLib.Logging;
using Cedar.Core.Logging;
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

        #region 会员基础

        /// <summary>
        /// 会员注册检查Email是否被注册
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>0：未被注册，非0：Email被注册</returns>
        public int CheckEmail(string email)
        {
            const string sql = @"select count(1) as count from cust_info where email=@email;";
            var result = Helper.ExecuteScalar<int>(sql, new { email });
            return result;
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：被注册</returns>
        public int CheckMobile(string mobile)
        {
            const string sql = @"select count(1) from cust_info where mobile=@mobile;";
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
            const string sql = @"INSERT INTO cust_info(innerid, custname, password, mobile, telephone, email, headportrait, status, authstatus, provid, cityid, area, sex, brithday, qq, signature, totalpoints, level, type, recommendedid, createdtime)
                        VALUES (@innerid, @custname, @password, @mobile, @telephone, @email, @headportrait, @status, @authstatus, @provid, @cityid, @area, @sex, @brithday, @qq, @signature, @totalpoints, @level, @type, @recommendedid, @createdtime);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, userInfo, tran);

                    //插入会员的总数信息
                    const string sqlTotal = "insert into cust_total_info (innerid, custid) values (uuid(),@custid);";
                    conn.Execute(sqlTotal, new { custid = userInfo.Innerid }, tran);

                    //插入关联
                    if (userInfo.Wechat != null)
                    {
                        const string sqlwechat =
                            @"INSERT INTO cust_wechat(innerid,custid,openid,createdtime) VALUES(uuid(),@custid,@openid,@createdtime);";
                        conn.Execute(sqlwechat, new
                        {
                            custid = userInfo.Innerid,
                            openid = userInfo.Wechat.Openid,
                            createdtime = userInfo.Createdtime
                        }, tran);
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="custid"></param>
        ///// <param name="openid"></param>
        ///// <returns></returns>
        //public int RelationFans(string custid,string openid) {

        //    var sql = "select * from wechat_friend where openid=@openid;";

        //    return 1;
        //}

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
                custModel.Wechat = Helper.Query<CustWechat>("select a.* from wechat_friend as a inner join cust_wechat as b on a.openid=b.openid where b.custid=@custid;", new
                {
                    custid = custModel.Innerid
                }).FirstOrDefault();
            }

            return custModel;
        }

        /// <summary>
        /// 根据openid获取会员信息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        public CustModel GetCustByOpenid(string openid)
        {
            const string sql = "select b.* from cust_wechat as a inner join cust_info as b on a.custid=b.innerid where a.openid=@openid;";
            var custModel = Helper.Query<CustModel>(sql, new { openid }).FirstOrDefault();
            return custModel;
        }

        /// <summary>
        /// 用户登录(openid登录)
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        public CustModel CustLoginByOpenid(string openid)
        {
            const string sql = "select b.* from cust_wechat as a left join cust_info as b on a.custid=b.innerid where a.openid=@openid;";
            var custModel = Helper.Query<CustModel>(sql, new { openid }).FirstOrDefault();

            //获取微信信息
            if (custModel != null)
            {
                custModel.Wechat = Helper.Query<CustWechat>("select * from wechat_friend where openid=@openid;", new
                {
                    openid
                }).FirstOrDefault();
            }

            return custModel;
        }

        /// <summary>
        /// 根据openid获取会员基本信息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        public CustModel CustInfoByOpenid(string openid)
        {
            const string sql = "select b.* from cust_wechat as a inner join cust_info as b on a.custid=b.innerid where a.openid=@openid;";
            var custModel = Helper.Query<CustModel>(sql, new { openid }).FirstOrDefault();
            return custModel;
        }

        /// <summary>
        /// 根据carid获取会员基本信息
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>用户信息</returns>
        public CustModel CustInfoByCarid(string carid)
        {
            const string sql =
                "select a.* from cust_info as a inner join car_info as b on a.innerid=b.custid where b.innerid=@carid;";
            var custModel = Helper.Query<CustModel>(sql, new { carid }).FirstOrDefault();
            return custModel;
        }

        /// <summary>
        /// 获取微信信息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        public CustWechat CustWechatByOpenid(string openid)
        {
            CustWechat wechat = Helper.Query<CustWechat>("select * from wechat_friend where openid=@openid;", new
            {
                openid
            }).FirstOrDefault();

            return wechat;
        }

        /// <summary>
        /// 更新会员二维码
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="qrcode"></param>
        /// <returns>用户信息</returns>
        public int UpdateQrCode(string innerid, string qrcode)
        {
            const string sql = "update cust_info set qrcode=@qrcode where innerid=@innerid;";
            var custModel = Helper.Execute(sql, new { qrcode, innerid });
            return custModel;
        }

        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        public CustViewModel GetCustById(string innerid)
        {
            const string sql = @"select a.*,b.provname,c.cityname from cust_info as a 
                left join base_province as b on a.provid=b.innerid 
                left join base_city as c on a.cityid=c.innerid where a.innerid=@innerid;";

            try
            {
                var custModel = Helper.Query<CustViewModel>(sql, new { innerid }).FirstOrDefault();
                //获取微信信息
                if (custModel != null)
                {
                    custModel.Wechat = Helper.Query<CustWechat>("select a.* from wechat_friend as a inner join cust_wechat as b on a.openid=b.openid where b.custid=@custid;", new
                    {
                        custid = innerid
                    }).FirstOrDefault();

                    custModel.TotalInfo = Helper.Query<CustTotalModel>("select * from cust_total_info where custid=@custid;", new { custid = innerid }).FirstOrDefault();
                }
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取会员详情（根据手机号）
        /// </summary>
        /// <param name="mobile">会员手机号</param>
        /// <returns></returns>
        public CustModel GetCustByMobile(string mobile)
        {
            const string sql = "select * from cust_info where mobile=@mobile;";

            try
            {
                var custModel = Helper.Query<CustModel>(sql, new { mobile }).FirstOrDefault();
                //获取微信信息
                if (custModel != null)
                {
                    custModel.Wechat = Helper.Query<CustWechat>("select a.* from wechat_friend as a inner join cust_wechat as b on a.openid=b.openid where b.custid=@custid;", new
                    {
                        custid = custModel.Innerid
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
            const string fields = "innerid, custname, password, mobile, telephone, email, headportrait, status, authstatus, provid, cityid, area, sex, brithday, qq, totalpoints, level, qrcode, type, createdtime, modifiedtime,(select count(1) from car_info where custid=cust_info.innerid and status<>0) as carnum";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            sqlWhere.Append(query.Status != null
                ? $" and status={query.Status}"
                : "");
            //会员ID
            //if (!string.IsNullOrWhiteSpace(query.innerid))
            //{
            //    sqlWhere.Append($" and cityid in (select cityid from sys_user_city where userid='{query.innerid}')");
            //}
            //手机号
            if (!string.IsNullOrWhiteSpace(query.Mobile))
            {
                sqlWhere.Append($" and mobile like '%{query.Mobile}%'");
            }
            //昵称
            if (!string.IsNullOrWhiteSpace(query.Custname))
            {
                sqlWhere.Append($" and custname like '%{query.Custname}%'");
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
            var custModel = Helper.Execute(sql, new
            {
                password = mRetrievePassword.NewPassword,
                mobile = mRetrievePassword.Mobile
            });
            return custModel;
        }

        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCustInfo(CustModel model)
        {
            var sqlStr = new StringBuilder("update cust_info set ");
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
        /// 修改会员状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateCustStatus(string innerid, int status)
        {
            const string sql = "update cust_info set status=@status where innerid=@innerid;";
            var result = Helper.Execute(sql, new
            {
                innerid,
                status
            });
            return result;
        }

        /// <summary>
        /// 修改会员类型(修改成功后返回会员信息)
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CustModel UpdateCustType(string innerid)
        {
            const string sqlSCust = "select * from cust_info where innerid=@innerid;";
            const string sqlSNum = "select count(1) from car_info where custid=@custid and status<>0;";
            const string sql = "update cust_info set type=1 where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var custModel = conn.Query<CustModel>(sqlSCust, new { innerid }).FirstOrDefault();
                var carNum = conn.Query<int>(sqlSNum, new { custid = innerid }).FirstOrDefault();
                if (custModel == null)
                    return null;
                if (custModel.Type == 1 || carNum <= 1)
                    return null;
                try
                {
                    conn.Execute(sql, new
                    {
                        innerid
                    });

                    return custModel;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("修改会员类型异常：", TraceEventType.Error, ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// 升级为精品车商
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        public int UpgradeBoutique(string innerid)
        {
            const string sqlSCust = "select * from cust_info where innerid=@innerid;";
            const string sql = "update cust_info set type=3 where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var custModel = conn.Query<CustModel>(sqlSCust, new { innerid }).FirstOrDefault();
                if (custModel == null)
                    return 0;
                if (custModel.Type == 3)
                {
                    return 1;
                }
                try
                {
                    conn.Execute(sql, new
                    {
                        innerid
                    });

                    return 1;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("升级为精品车商异常：", TraceEventType.Error, ex);
                    return 0;
                }
            }
        }

        #endregion

        #region 会员Total

        /// <summary>
        /// 更新会员的刷新次数
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="oper">1+ 2-</param>
        /// <returns>用户信息</returns>
        public int UpdateCustTotalCount(string custid, int type, int count, int oper = 1)
        {
            var sql = "";
            var o = oper == 1 ? "+" : "-";
            //刷新
            if (type == 1)
            {
                sql = $"update cust_total_info set refreshnum=refreshnum{o}@count where custid=@custid;";
            }
            //置顶
            else if (type == 2)
            {
                sql = $"update cust_total_info set topnum=topnum{o}@count where custid=@custid;";
            }
            //积分
            else if (type == 3)
            {
                sql = $"update cust_total_info set currpoint=currpoint{o}@count where custid=@custid;";
            }

            var result = Helper.Execute(sql, new { count, custid });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveTotalRecord(CustTotalRecordModel model)
        {
            const string sql = "insert into cust_total_record (innerid, custid, count,type, remark, spare1, createrid, createdtime) values (@innerid, @custid, @count,@type, @remark, @spare1, @createrid, @createdtime);";
            var result = Helper.Execute(sql, model);
            return result;
        }

        /// <summary>
        /// 发福利
        /// </summary>
        /// <returns></returns>
        public int SendWelfare(int refreshnum, int topnum)
        {
            //try
            //{
            //    var dddd = Convert.ToInt32("qqqqq");
            //}
            //catch (Exception ex)
            //{
            //    LoggerFactories.CreateLogger().Write("dao test", TraceEventType.Error, ex);
            //}

            //return 1;
            const string sql =
                "select custid from cust_total_info where custid not in (select custid from cust_total_record where `type`=100 and date_format(createdtime,'%Y-%m')=date_format(now(),'%Y-%m'));";
            const string u = "update cust_total_info set refreshnum=refreshnum+@refreshcount,topnum=topnum+@topcount where custid=@custid;";
            const string i = "insert into cust_total_record (innerid, custid, count, type, remark, spare1, createrid, createdtime) values (@innerid, @custid, @count, @type, @remark, @spare1, @createrid, @createdtime);";

            using (var conn = Helper.GetConnection())
            {
                var list = conn.Query<string>(sql).ToList();
                var tran = conn.BeginTransaction();
                try
                {
                    foreach (var item in list)
                    {
                        //更新刷新和置顶次数
                        conn.Execute(u, new { refreshcount = refreshnum, topcount = topnum, custid = item }, tran);
                        //保存更新记录
                        conn.Execute(i, new
                        {
                            innerid = Guid.NewGuid().ToString(),
                            count = 0,
                            custid = item,
                            type = 100,
                            remark = $"发福利记录：刷新次数增加{refreshnum}次，置顶次数增加{topnum}次",
                            createdtime = DateTime.Now
                        }, tran);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("发福利异常：" + ex.Message, TraceEventType.Information);
                }
            }

            return 1;
        }

        #endregion

        #region 会员认证

        /// <summary>
        /// 用户添加认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public int AddAuthentication(CustAuthenticationModel model)
        {
            const string sqlU = "update cust_info set authstatus=1 where innerid=@innerid;";
            const string sqlI = @"INSERT INTO `cust_authentication`
                                (innerid, custid, realname, idcard, enterprisename, licencecode, licencearea, organizationcode, taxcode, relevantpicture, createdtime)
                                VALUES
                                (uuid(), @custid, @realname, @idcard, @enterprisename, @licencecode, @licencearea, @organizationcode, @taxcode, @relevantpicture, @createdtime);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlU, new { innerid = model.Custid }, tran);
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

            const string sqlU = "update cust_info set authstatus=4 where innerid=@innerid;";
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
        /// <param name="model">会员相关信息</param>
        /// <returns></returns>
        public int AuditAuthentication(CustAuthenticationModel model)
        {
            const string sql = "update cust_info set authstatus=@authstatus where innerid=@innerid;";
            const string sqlau = "update cust_authentication set auditper=@auditper,auditdesc=@auditdesc,audittime=@audittime where custid=@custid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new
                    {
                        authstatus = model.AuditResult == 1 ? 2 : 3,
                        innerid = model.Custid
                    }, tran);
                    conn.Execute(sqlau, new
                    {
                        auditper = model.AuditPer,
                        auditdesc = model.AuditDesc,
                        audittime = model.AuditTime,
                        custid = model.Custid
                    }, tran);

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
        /// 撤销审核
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public int CancelAuditAuthentication(string custid)
        {
            const string sql = "update cust_info set authstatus=1 where innerid=@innerid;";
            const string sqlau = "update cust_authentication set auditper='',auditdesc='',audittime=null where custid=@custid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = custid });
                    conn.Execute(sqlau, new { custid });

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

        #region 会员点赞

        /// <summary>
        /// 保存点赞人信息
        /// </summary>
        /// <param name="model">点赞人信息</param>
        /// <returns></returns>
        public int AddLaudator(CustLaudator model)
        {
            const string sql = @"INSERT INTO cust_laudator
                                (innerid, accountid, nickname, photo, openid, remarkname, area, sex, subscribe_time, subscribe, country, province, city, createdtime)
                                VALUES (@innerid, @accountid, @nickname, @photo, @openid, @remarkname, @area, @sex, @subscribe_time, @subscribe, @country, @province, @city, @createdtime);";
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
        /// 用户修改认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public int UpdateLaudator(CustLaudator model)
        {
            var sqlStr = new StringBuilder("update cust_laudator set ");
            sqlStr.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sqlStr.Append(" where openid=@openid;");

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
        /// 保存关系表
        /// </summary>
        /// <param name="relModel">关系对象</param>
        /// <returns></returns>
        public int SaveLaudatorRelation(CustLaudatorRelation relModel)
        {
            const string sql = "insert into cust_laudator_relation (laudatorid, tocustid, carid, createdtime) values (@laudatorid, @tocustid, @carid, @createdtime);";

            try
            {
                var result = Helper.Execute(sql, relModel);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取点赞人信息 by openid
        /// </summary>
        /// <param name="openid">点赞者的openid</param>
        /// <param name="tocustid">被点赞者的会员id</param>
        /// <returns></returns>
        public CustLaudator GetCustLaudatorByOpenid(string openid, string tocustid)
        {
            const string sql = "select a.*,(select count(1) from cust_laudator_relation where laudatorid=a.innerid and tocustid=@tocustid) as ispraise from cust_laudator as a where openid=@openid;";

            try
            {
                var custModel = Helper.Query<CustLaudator>(sql, new { openid, tocustid }).FirstOrDefault();
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据会员id获取所有点赞人列表
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public IEnumerable<CustLaudator> GetLaudatorListByCustid(string custid)
        {
            const string sql = "select a.createdtime,b.openid,b.nickname,b.photo from cust_laudator_relation as a inner join cust_laudator as b on a.laudatorid=b.innerid where a.tocustid=@tocustid;";

            try
            {
                var list = Helper.Query<CustLaudator>(sql, new { tocustid = custid });
                return list;
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
        public int UpdateTagStatus(string innerid, int status)
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
                    conn.Execute(sqlH, new { innerid = model.Tagid }, tran);

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

            var list = Helper.Query<dynamic>(sql, new { custid });
            return list;
        }

        /// <summary>
        /// 获取会员该标签的操作者
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="tagid"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTagRelationWithOper(string custid, string tagid)
        {
            const string sql = @"select b.custname, a.createdtime from cust_tag_relation as a 
                                left join cust_info as b on a.fromid=b.innerid 
                                where tagid=@tagid and toid=@custid;";

            var list = Helper.Query<dynamic>(sql, new { tagid, custid });
            return list;
        }

        #endregion

        #region 数据清理

        /// <summary>
        /// 清除所有数据(除基础数据)
        /// </summary>
        /// <returns></returns>
        public int DeleteAll()
        {
            using (var conn = Helper.GetConnection())
            {
                var args = new DynamicParameters();
                args.Add("p_values", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var result = conn.QueryMultiple("ccnsp_clearall", args, commandType: CommandType.StoredProcedure))
                {
                    //获取结果集
                    //var data = result.Read<T>();
                }

                return args.Get<int>("p_values");
            }
        }

        /// <summary>
        /// 删除会员所有信息
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public int DeleteCustomer(string mobile)
        {
            using (var conn = Helper.GetConnection())
            {
                //参数
                var obj = new
                {
                    p_mobile = mobile
                };

                var args = new DynamicParameters(obj);
                args.Add("p_values", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var result = conn.QueryMultiple("ccnsp_clearData", args, commandType: CommandType.StoredProcedure))
                {
                    //获取结果集
                    //var data = result.Read<T>();
                }

                return args.Get<int>("p_values");
            }
        }

        /// <summary>
        /// 删除会员所有信息
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public DeleteCustAllPic GetCustomerAllPicture(string mobile)
        {
            var sqlCust = "select innerid,qrcode from cust_info where mobile=@mobile;";
            var sqlCustAuth = "select relevantpicture from cust_authentication where custid=@custid;";
            var sqlCarPic = "select path from car_picture where carid in (select innerid from car_info where custid=@custid);";
            var sqlCarCode = "select qrcode from coupon_code where custid=@custid;";

            using (var conn = Helper.GetConnection())
            {
                var custModel = conn.Query<dynamic>(sqlCust, new { mobile }).FirstOrDefault();
                if (custModel == null)
                {
                    return null;
                }
                var custid = custModel.innerid.ToString();

                var model = new DeleteCustAllPic
                {
                    Qrcode = custModel.qrcode.ToString(),
                    AuthPic = conn.Query<string>(sqlCustAuth, new { custid }).FirstOrDefault(),
                    CarPicList = conn.Query<string>(sqlCarPic, new { custid }).ToList(),
                    CodeList = conn.Query<string>(sqlCarCode, new { custid }).ToList(),
                };

                //model.qrcode = custModel.qrcode.ToString();

                ////获取认证信息图片
                //model.authPic = conn.Query<string>(sqlCustAuth, new { custid }).FirstOrDefault();

                ////获取车辆图片
                //model.carPicList = conn.Query<string>(sqlCarPic, new { custid }).ToList();

                ////获取礼券二维码图片
                //model.codeList = conn.Query<string>(sqlCarCode, new { custid }).ToList();

                return model;
            }

        }

        #endregion

        #region 微信信息
        /// <summary>
        /// 获取cust_wechat信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CustWeChatViewModel> GetCustWeChatList(CustWeChatQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @" wechat_friend ";
            const string fields = @"ifnull(accountid,'') accountid,ifnull(nickname,'') nickname,ifnull(openid,'') openid,ifnull(remarkname,'') remarkname,sex,ifnull(createdtime,current_date()) createdtime 
                                    ,innerid, nickname, photo, area, isdel, subscribe_time, subscribe, country, province, city";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? " createdtime desc" : query.Order;
            //查詢條件
            var sqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(query.nickname))
            {
                sqlWhere.Append($" and nickname like '%{query.nickname}%'");
            }
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CustWeChatViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 更新绑定openid
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int UpdateOpenid(string custid, string openid)
        {
            using (var conn = Helper.GetConnection())
            {
                int result;
                const string sqlSelect = "select count(1) from cust_wechat where custid=@custid;";
                var i = conn.Query<int>(sqlSelect, new { custid }).FirstOrDefault();
                if (i == 0)
                {
                    const string sql = "INSERT INTO cust_wechat(innerid,custid,openid,createdtime) VALUES(uuid(),@custid,@openid,@createdtime);";
                    result = Helper.Execute(sql, new
                    {
                        custid,
                        openid,
                        createdtime = DateTime.Now
                    });
                }
                else
                {
                    const string sql = "update cust_wechat set openid=@openid,modifiedtime=@modifiedtime where custid=@custid;";
                    result = Helper.Execute(sql, new
                    {
                        custid,
                        openid,
                        modifiedtime = DateTime.Now
                    });
                }

                return result;
            }
        }

        #endregion

        #region 车信评

        /// <summary>
        /// 导入公司
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int ImportCompany(DataTable dt)
        {
            const string sql = @"INSERT INTO settled_info
                                (innerid, companyname, address, opername, originalregistcapi, scope, companystatus, officephone, spare1, spare2, createrid, createdtime, modifierid, modifiedtime)
                                VALUES (@innerid, @companyname, @address, @opername, @originalregistcapi, @scope, @companystatus, @officephone, @spare1, @spare2, @createrid, @createdtime, @modifierid, @modifiedtime);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    var model = new CompanyModel();
                    foreach (DataRow row in dt.Rows)
                    {
                        model.CompanyName = row["CompanyName"].ToString();
                        model.Address = row["Address"].ToString();
                        model.OperName = row["OperName"].ToString();
                        model.OriginalRegistCapi = row["OriginalRegistCapi"].ToString();
                        model.Scope = row["Scope"].ToString();
                        model.CompanyStatus = row["CompanyStatus"].ToString();
                        model.OfficePhone = row["OfficePhone"].ToString();
                        model.Innerid = Guid.NewGuid().ToString();
                        model.Createdtime = DateTime.Now;
                        model.Createrid = "";
                        conn.Execute(sql, model, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("导入公司数据失败：" + ex.Message, TraceEventType.Information);
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 公司列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CompanyListModel> GetCompanyPageList(CompanyQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @" settled_info as a  left join (select settid,count(1) as setttotal,createdtime from settled_info_applyupdate where status=2 group by settid) as b on b.settid=a.innerid";
            const string fields = @"innerid, companyname, address, opername, originalregistcapi, companystatus, ifnull(officephone,'') as officephone, picurl, companytitle, ancestryids, categoryids, customdesc, boutiqueurl, spare1, spare2, createrid, a.createdtime, modifierid, modifiedtime,b.setttotal,
(select count(innerid) from settled_praiselog where companyid=a.innerid) as PraiseNum,
(select count(innerid) from settled_comment where companyid=a.innerid) as CommentNum,
(select avg(score) from settled_comment where companyid=a.innerid) as ScoreNum,
(select group_concat(codename) from base_code where typekey='car_ancestry' and FIND_IN_SET(codevalue,a.ancestryids)) as ancestryname,
(select group_concat(codename) from base_code where typekey='car_category' and FIND_IN_SET(codevalue,a.categoryids)) as categoryname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime,b.setttotal desc,b.createdtime asc " : query.Order;
            //查詢條件
            var sqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                sqlWhere.Append($" and companyname like '%{query.CompanyName}%'");
            }
            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                sqlWhere.Append($" and address like '%{query.Address}%'");
            }

            if (!string.IsNullOrWhiteSpace(query.City))
            {
                sqlWhere.Append($" and (address like '%{query.City}%' or companyname like '%{query.City}%')");
            }

            if (!string.IsNullOrWhiteSpace(query.County))
            {
                sqlWhere.Append($" and address like '%{query.County}%'");
            }

            if (!string.IsNullOrWhiteSpace(query.Ancestryids))
            {
                string str = "";
                foreach (var item in query.Ancestryids.Split(','))
                {
                    str += " locate('" + item + "',ancestryids)>0 or";
                }
                str = str.Substring(0, str.Length - 2);
                sqlWhere.Append(" and (" + str + ")");
            }

            if (!string.IsNullOrWhiteSpace(query.Categoryids))
            {
                string str = "";
                foreach (var item in query.Categoryids.Split(','))
                {
                    str += " locate('" + item + "',categoryids)>0 or";
                }
                str = str.Substring(0, str.Length - 2);
                sqlWhere.Append(" and (" + str + ")");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CompanyListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取公司model
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CompanyModel GetCompanyModelById(string innerid)
        {
            const string sql = @"select * from settled_info as a where innerid=@innerid;";
            var model = Helper.Query<CompanyModel>(sql, new { innerid }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 获取公司详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CompanyViewModel GetCompanyById(string innerid)
        {
            const string sql = @"select innerid, companyname, address, opername, originalregistcapi, scope, companystatus, officephone, picurl, companytitle, ancestryids, categoryids, customdesc, boutiqueurl, introduction,spare1, spare2, createrid, createdtime, modifierid, modifiedtime,
(select count(innerid) from settled_praiselog where companyid=a.innerid) as PraiseNum,
(select count(innerid) from settled_comment where companyid=a.innerid) as CommentNum,
(select avg(score) from settled_comment where companyid=a.innerid) as ScoreNum,
(select count(1) from settled_info_applyupdate where settid=a.innerid and status=2) as Status,
(select group_concat(codename) from base_code where typekey='car_ancestry' and FIND_IN_SET(codevalue,a.ancestryids)) as ancestryname,
(select group_concat(codename) from base_code where typekey='car_category' and FIND_IN_SET(codevalue,a.categoryids)) as categoryname
from settled_info as a where innerid=@innerid;";
            var model = Helper.Query<CompanyViewModel>(sql, new { innerid }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 申请企业信息修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCompanyApplyUpdate(CompanyApplyUpdateModel model)
        {
            const string sql = @"INSERT INTO settled_info_applyupdate(innerid, settid, contactmobile,pictures, companyname, address, opername, originalregistcapi, scope, companystatus, officephone, picurl, companytitle, ancestryids, categoryids, customdesc, boutiqueurl,introduction,status, contactsphone,remark,spare1, spare2, createrid, createdtime, modifierid, modifiedtime)
                                                              VALUES (@innerid, @settid, @contactmobile, @pictures, @companyname, @address, @opername, @originalregistcapi, @scope, @companystatus, @officephone, @picurl, @companytitle, @ancestryids, @categoryids, @customdesc, @boutiqueurl,@introduction,2,@contactsphone,@remark, @spare1, @spare2, @createrid, now(), @modifierid, @modifiedtime);";
            using (var conn = Helper.GetConnection())
            {
                try
                {
                    conn.Execute(sql, model);
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 修改申请列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CompanyUpdateApplyListModel> GetUpdateApplyPageList(CompanyUpdateApplyQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @" settled_info_applyupdate as a left join settled_info as b on b.innerid=a.settid left join sys_user as c on c.innerid=a.modifierid ";
            const string fields = @"a.innerid, b.companyname, a.address, b.opername,  b.originalregistcapi, b.companystatus, a.officephone, a.picurl, a.companytitle, a.customdesc, a.boutiqueurl,a.status,ifnull(a.contactsphone,'') as contactsphone, a.createrid, a.createdtime, ifnull(c.username,'') as modifierid, a.modifiedtime,
(select group_concat(codename) from base_code where typekey='car_ancestry' and FIND_IN_SET(codevalue,a.ancestryids)) as ancestryname,
(select group_concat(codename) from base_code where typekey='car_category' and FIND_IN_SET(codevalue,a.categoryids)) as categoryname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime desc" : query.Order;
            //查詢條件
            var sqlWhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrWhiteSpace(query.Settid))
            {
                sqlWhere.Append($" and settid='{query.Settid}'");
            }

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                sqlWhere.Append($" and companyname like '%{query.CompanyName}%'");
            }

            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                sqlWhere.Append($" and address like '%{query.Address}%'");
            }

            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                sqlWhere.Append($" and officephone like '%{query.OfficePhone}%'");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CompanyUpdateApplyListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 更新申请表状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateApplyStatus(CompanyApplyUpdateModel model)
        {
            const string sql = @"update settled_info_applyupdate set status=@status,remark=@remark,modifierid=@modifierid,modifiedtime=now() where innerid=@innerid;";

            using (var conn = Helper.GetConnection())
            {
                try
                {
                    conn.Execute(sql, model);
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取申请的信息view
        /// </summary>
        /// <param name="applyid"></param>
        /// <returns></returns>
        public CompanyApplyUpdateViewModel GetUpdateApplyById(string applyid)
        {
            const string sqlS = @"select a.innerid,a.settid,a.ContactMobile,a.Pictures, b.companyname, a.address, b.opername, b.originalregistcapi, b.scope, b.companystatus, a.officephone, a.picurl, a.companytitle, a.customdesc, a.boutiqueurl,a.status, a.spare1, a.spare2, a.createrid, a.createdtime, a.modifierid, a.modifiedtime,a.introduction,a.remark,
(select group_concat(codename) from base_code where typekey = 'car_ancestry' and FIND_IN_SET(codevalue, a.ancestryids)) as ancestryname,
(select group_concat(codename) from base_code where typekey = 'car_category' and FIND_IN_SET(codevalue, a.categoryids)) as categoryname
from settled_info_applyupdate as a left join settled_info as b on b.innerid=a.settid where a.innerid = @innerid; ";
            return Helper.Query<CompanyApplyUpdateViewModel>(sqlS, new { innerid = applyid }).FirstOrDefault();
        }

        /// <summary>
        /// 获取申请的信息
        /// </summary>
        /// <param name="applyid"></param>
        /// <returns></returns>
        public CompanyApplyUpdateModel GetApplyModel(string applyid)
        {
            const string sqlS = "select * from settled_info_applyupdate where innerid=@innerid and status=2;";
            return Helper.Query<CompanyApplyUpdateModel>(sqlS, new { innerid = applyid }).FirstOrDefault();
        }

        /// <summary>
        /// 更新企业信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pictures"></param>
        /// <returns></returns>
        public int UpdateCompanyModel(CompanyModel model, string pictures)
        {
            var sqlStr = new StringBuilder("update `settled_info` set ");
            sqlStr.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sqlStr.Append(" where innerid = @innerid");

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlStr.ToString(), model, tran);

                    const string delPic = "delete from settled_picture where settid=@settid;";
                    conn.Execute(delPic, new { settid = model.Innerid }, tran);

                    const string addPic = "insert into settled_picture (innerid, settid, typeid, path, sort, createdtime) values (@innerid, @settid, @typeid, @path, @sort, @createdtime);";
                    var i = 1;
                    foreach (var item in pictures.Split(','))
                    {
                        conn.Execute(addPic, new
                        {
                            innerid = Guid.NewGuid().ToString(),
                            settid = model.Innerid,
                            typeid = 0,
                            path = item,
                            sort = i,
                            createdtime = DateTime.Now
                        }, tran);
                        i++;
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
        /// 更新企业信息（后台系统）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCompanyModel(CompanyModel model)
        {
            var sqlStr = new StringBuilder("update `settled_info` set ");
            sqlStr.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sqlStr.Append(" where innerid = @innerid");

            using (var conn = Helper.GetConnection())
            {
                try
                {
                    conn.Execute(sqlStr.ToString(), model);
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 验证重复评论
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public int CheckComment(string ip, string companyid)
        {
            const string sql = @"select count(1) from settled_comment where ip=@ip and companyid=@companyid;";
            try
            {
                return Helper.ExecuteScalar<int>(sql, new { ip, companyid });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("企业评论失败：" + ex.Message, TraceEventType.Information);
                return 0;
            }
        }

        /// <summary>
        /// 验证重复点赞
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public int CheckPraise(string ip, string companyid)
        {
            const string sql = @"select count(1) from settled_praiselog where ip=@ip and companyid=@companyid and date(createdtime)=@shortdate;";
            try
            {
                return Helper.ExecuteScalar<int>(sql, new { ip, companyid, shortdate = DateTime.Now.ToShortDateString() });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("企业评论失败：" + ex.Message, TraceEventType.Information);
                return 0;
            }
        }

        /// <summary>
        /// 企业评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DoComment(CommentModel model)
        {
            const string sql = @"insert into settled_comment (innerid, companyid, mobile, headportrait, score, ip, commentdesc, pictures, createdtime, isdelete, deletedtime, deletedesc) 
                                values (@innerid, @companyid, @mobile, @headportrait, @score, @ip, @commentdesc, @pictures, @createdtime, @isdelete, @deletedtime, @deletedesc);";
            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("企业评论失败：" + ex.Message, TraceEventType.Information);
                return 0;
            }
        }

        /// <summary>
        /// 企业点赞
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DoPraise(PraiseModel model)
        {
            const string sql = @"insert into settled_praiselog (innerid, companyid, ip, address, spare1, spare2, createdtime) values (@innerid, @companyid, @ip, @address, @spare1, @spare2, @createdtime);";
            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("企业点赞失败：" + ex.Message, TraceEventType.Information);
                return 0;
            }
        }

        /// <summary>
        /// 评论列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CommentListModel> GetCommentPageList(CommentQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @" settled_comment as a left join settled_info as b on b.innerid =a.companyid";
            const string fields = @"a.innerid, a.companyid, ifnull(a.mobile,'') as mobile, a.headportrait, a.score, a.ip, ifnull(a.commentdesc,'') as commentdesc, a.createdtime,b.companyname,a.pictures";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? " createdtime desc" : query.Order;
            //查詢條件
            var sqlWhere = new StringBuilder(" 1=1 and (isdelete <>1 or isdelete is null) ");
            if (!string.IsNullOrWhiteSpace(query.Companyid))
            {
                sqlWhere.Append($" and companyid='{query.Companyid}'");
            }
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                sqlWhere.Append($" and companyname like '%{query.CompanyName}%'");
            }

            if (query.OnlyLow)
            {
                sqlWhere.Append(" and score<=2");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CommentListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 评分列表
        /// </summary>
        /// <param name="settid"></param>
        /// <returns></returns>
        public ScoreListModel GetScoreList(string settid)
        {
            const string sql = @"select count(1) as count from settled_comment where companyid=@companyid and score=@num;";
            var model = new ScoreListModel();
            using (var conn = Helper.GetConnection())
            {
                model.Score1 = conn.Query<int>(sql, new { companyid = settid, num = 1 }).FirstOrDefault();
                model.Score2 = conn.Query<int>(sql, new { companyid = settid, num = 2 }).FirstOrDefault();
                model.Score3 = conn.Query<int>(sql, new { companyid = settid, num = 3 }).FirstOrDefault();
                model.Score4 = conn.Query<int>(sql, new { companyid = settid, num = 4 }).FirstOrDefault();
                model.Score5 = conn.Query<int>(sql, new { companyid = settid, num = 5 }).FirstOrDefault();
            }

            return model;
        }

        /// <summary>
        /// 删除评论（逻辑删除）
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteComment(string innerid)
        {
            var sqlStr = new StringBuilder("update `settled_comment` set `isdelete` =1 where innerid = @innerid ");

            using (var conn = Helper.GetConnection())
            {
                try
                {
                    Helper.ExecuteScalar<int>(sqlStr.ToString(), new { innerid });
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CommentListModel GetCommentViewByID(string innerid)
        {
            const string sqlS = "select a.*,b.companyname from settled_comment as a left join settled_info as b on b.innerid =a.companyid where a.innerid=@innerid;";
            return Helper.Query<CommentListModel>(sqlS, new { innerid = innerid }).FirstOrDefault();
        }


        #region 图片处理

        /// <summary>
        /// 获取公司图片
        /// </summary>
        /// <param name="settid"></param>
        /// <returns></returns>
        public IEnumerable<string> GetCompanyPictureListById(string settid)
        {
            const string sql = @"select path from settled_picture where settid=@settid;";
            var list = Helper.Query<string>(sql, new { settid });
            return list;
        }

        /// <summary>
        /// 获取公司图片
        /// </summary>
        /// <param name="settid"></param>
        /// <returns></returns>
        public IEnumerable<CompanyPictureModel> GetCompanyPictureById(string settid)
        {
            const string sql = @"select path,innerid from settled_picture where settid=@settid;";
            var list = Helper.Query<CompanyPictureModel>(sql, new { settid });
            return list;
        }

        /// <summary>
        /// 获取需要删除的图片列表
        /// </summary>
        /// <param name="idList">车辆ids</param>
        /// <returns></returns>
        public IEnumerable<CompanyPictureModel> GetCompanyPictureByIds(List<string> idList)
        {
            var ids = idList.Aggregate("", (current, it) => current + $"'{it}',").TrimEnd(',');
            var sql = $"select innerid, settid, typeid, path, sort, createdtime from settled_picture where innerid in ({ids});";
            try
            {
                var list = Helper.Query<CompanyPictureModel>(sql);
                return list;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取需要删除的图片列表：", TraceEventType.Information, ex);
                return null;
            }
        }

        /// <summary>
        /// 批量保存图片(添加)
        /// </summary>
        /// <param name="pathList"></param>
        /// <param name="settid"></param>
        /// <returns></returns>
        public int AddCompanyPictureList(List<string> pathList, string settid)
        {
            const string sqlSCarPic = "select innerid, settid, typeid, path, sort, createdtime from settled_picture where settid=@settid order by sort;";//查询企业图片
            const string sqlSMaxSort = "select ifnull(max(sort),0) as maxsort from settled_picture where settid=@settid;";                              //查询企业所有图片的最大排序
            const string sqlIPic = @"insert into settled_picture (innerid, settid, typeid, path, sort, createdtime) values (@innerid, @settid, @typeid, @path, @sort, @createdtime);";
            const string sqlUCover = @"update settled_info set pic_url=(select path from settled_picture where settid=@settid order by sort limit 1) where innerid=@settid;";

            using (var conn = Helper.GetConnection())
            {
                //获取图片
                var picedList = conn.Query<CompanyPictureModel>(sqlSCarPic, new { settid }).ToList();
                var number = picedList.Count + pathList.Count;
                if (number > 9)
                {
                    //图片数量控制在>=3 and <=9
                    return 402;
                }

                var maxsort = conn.ExecuteScalar<int>(sqlSMaxSort, new { settid });
                var tran = conn.BeginTransaction();
                try
                {
                    foreach (var path in pathList)
                    {
                        conn.Execute(sqlIPic, new CompanyPictureModel
                        {
                            Settid = settid,
                            Createdtime = DateTime.Now,
                            Path = path,
                            Innerid = Guid.NewGuid().ToString(),
                            Sort = ++maxsort
                        }, tran); //插入图片
                    }

                    //表示添加首批图片
                    if (maxsort == pathList.Count)
                    {
                        conn.Execute(sqlUCover, new { settid }, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("批量添加图片异常：" + ex.Message, TraceEventType.Warning);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 批量保存图片(删除)
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="settid"></param>
        /// <returns></returns>
        public int DelCompanyPictureList(List<string> idList, string settid)
        {
            const string sqlSCarPic = "select innerid, settid, typeid, path, sort, createdtime from settled_picture where settid=@settid order by sort;";//查询企业图片
            const string sqlDPic = @"delete from settled_picture where innerid=@innerid;";
            const string sqlUCover = @"update settled_info set pic_url=(select path from auction_car_picture where settid=@settid order by sort limit 1) where innerid=@settid;";

            using (var conn = Helper.GetConnection())
            {
                //获取车辆图片
                var picedList = conn.Query<CompanyPictureModel>(sqlSCarPic, new { settid }).ToList();
                var number = picedList.Count - idList.Count;
                //if (number < 3)
                //{
                //    //图片数量控制在>=3 and <=9
                //    return 402;
                //}

                var tran = conn.BeginTransaction();
                try
                {
                    //标示是否修改封面
                    var isUCover = false;
                    //获取封面图片
                    var coverid = picedList.First().Innerid;
                    foreach (var id in idList)
                    {
                        if (id.Equals(coverid))
                        {
                            isUCover = true;
                        }
                        conn.Execute(sqlDPic, new { innerid = id }, tran);
                    }

                    if (isUCover)
                    {
                        conn.Execute(sqlUCover, new { settid }, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("批量删除图片异常：" + ex.Message, TraceEventType.Warning);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveCompanyPicture(CompanyPictureListModel model)
        {
            const string sqlSCarPic = "select innerid, settid, typeid, path, sort, createdtime from settled_picture where settid=@settid order by sort;";//查询车辆图片
            const string sqlSMaxSort = "select ifnull(max(sort),0) as maxsort from settled_picture where carid=@carid;";//查询车辆所有图片的最大排序
            const string sqlIPic = @"insert into settled_picture (innerid, carid, typeid, path, sort, createdtime) values (@innerid, @settid, @typeid, @path, @sort, @createdtime);";
            const string sqlDPic = @"delete from settled_picture where innerid=@innerid;";
            const string sqlUCover = @"update settled_info set pic_url=(select path from settled_picture where settid=@settid order by sort limit 1) where innerid=@settid;";

            using (var conn = Helper.GetConnection())
            {
                //获取车辆图片
                var picList = conn.Query<CompanyPictureModel>(sqlSCarPic, new { carid = model.Settid }).ToList();
                var number = picList.Count + model.AddPaths.Count - model.DelIds.Count;
                if (number < 3 || number > 9)
                {
                    //图片数量控制在>=3 and <=9
                    return 402;
                }

                var maxsort = conn.ExecuteScalar<int>(sqlSMaxSort, new { carid = model.Settid });
                var tran = conn.BeginTransaction();
                try
                {
                    foreach (var path in model.AddPaths)
                    {
                        conn.Execute(sqlIPic, new CompanyPictureModel
                        {
                            Settid = model.Settid,
                            Createdtime = DateTime.Now,
                            Path = path,
                            Innerid = Guid.NewGuid().ToString(),
                            Sort = ++maxsort
                        }, tran); //插入图片
                    }

                    //标示是否修改封面
                    var isUCover = false;
                    //获取封面图片
                    var coverid = picList.First().Innerid;
                    foreach (var id in model.DelIds)
                    {
                        if (id.Equals(coverid))
                        {
                            isUCover = true;
                        }
                        conn.Execute(sqlDPic, new { innerid = id }, tran);
                    }

                    if (isUCover)
                    {
                        conn.Execute(sqlUCover, new { carid = model.Settid }, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("批量保存图片异常：" + ex.Message, TraceEventType.Warning);
                    return 0;
                }
            }
        }

        #endregion

        #endregion

    }
}
#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
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
            if (!string.IsNullOrWhiteSpace(query.innerid))
            {
                sqlWhere.Append($" and cityid in (select cityid from sys_user_city where userid='{query.innerid}')");
            }
            //手机号
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
                var i = conn.Query<int>(sqlSelect,new { custid }).FirstOrDefault();
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
    }
}
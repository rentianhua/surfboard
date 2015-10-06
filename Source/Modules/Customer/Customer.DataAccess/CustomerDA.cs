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
        /// 会员注册检查用户名是否被注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        public int CheckUserName(string username)
        {
            const string sql = @"select count(1) as count from `cust_info` where username=@username;";
            var result = Helper.Execute<int>(sql, new { username });
            return result;
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        public int CheckMobile(string mobile)
        {
            const string sql = @"select count(1) as count from `cust_info` where mobile=@mobile;";
            var result = Helper.Execute<int>(sql, new { mobile });
            return result;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public int CustRegister(CustModel userInfo)
        {
            var result = 0;
            //插入账户基本信息
            const string sql = @"INSERT INTO `cust_info`(`innerid`,`username`,`password`,`mobile`,`telephone`,`email`,`headportrait`,`status`,authstatus,`type`,`realname`,`totalpoints`,`level`,`createdtime`,`modifiedtime`)
                        VALUES (@innerid,@username,@password,@mobile,@telephone,@email,@headportrait,@status,@authstatus,@type,@realname,@totalpoints,@level,@createdtime,@modifiedtime);";
            
            try
            {
                Helper.Execute(sql, userInfo);

                //插入微信信息
                if (userInfo.Wechat != null)
                {
                    const string sqlwechat = @"INSERT INTO cust_wechat(`innerid`,`custid`,`accountid`,`openid`,`nickname`,`headportrait`,`sex`,`country`,`province`,`city`,`createdtime`,`modifiedtime`)
                        VALUES(uuid(),@custid,@accountid,@openid,@nickname,@headportrait,@sex,@country,@province,@city,@createdtime,@modifiedtime);";
                    Helper.Execute(sqlwechat, userInfo.Wechat);
                }
            }
            catch (Exception ex)
            {
                result = 401;
            }

            return result;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        public CustModel CustLogin(CustLoginInfo loginInfo)
        {
            const string sql = "select * from `cust_info` where (username=@username or mobile=@mobile) and password=@password;";
            var custModel = Helper.Query<CustModel>(sql, new
            {
                username = loginInfo.Username,
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

        #endregion
    }
}
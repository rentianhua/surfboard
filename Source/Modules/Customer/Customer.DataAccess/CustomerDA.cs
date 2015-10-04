#region

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using CCN.Modules.Customer.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
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
            const string sql = @"INSERT INTO `cust_info`(`innerid`,`username`,`password`,`mobile`,`telephone`,`email`,`headportrait`,`status`,`type`,`realname`,`totalpoints`,`level`,`createdtime`,`modifiedtime`)
                        VALUES (@innerid,@username,@password,@mobile,@telephone,@email,@headportrait,@status,@type,@realname,@totalpoints,@level,@createdtime,@modifiedtime);";
            
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
    }
}
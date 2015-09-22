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
        private readonly Database _factoy;

        /// <summary>
        /// 
        /// </summary>
        public CustomerDA()
        {
            _factoy = new DatabaseWrapperFactory().GetDatabase("mysqldb");
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetALlCustomers()
        {
            var d = _factoy.Query("select * from base_carbrand where id=@id",new {id=""}).ToList();
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
            var result = _factoy.Query<int>(sql, new { username }).FirstOrDefault();
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
            var result = _factoy.Query<int>(sql, new { mobile } ).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public int CustRegister(CustModel userInfo)
        {
            var result = 1;
            //插入账户基本信息
            const string sql = @"INSERT INTO `cust_info`(`innerid`,`username`,`password`,`mobile`,`telephone`,`email`,`headportrait`,`status`,`type`,`realname`,`totalpoints`,`level`,`createdtime`,`modifiedtime`)
                        VALUES (@innerid,@username,@password,@mobile,@telephone,@email,@headportrait,@status,@type,@realname,@totalpoints,@level,@createdtime,@modifiedtime);";
            
            userInfo.Type = 1;
            try
            {
                _factoy.Execute(sql, userInfo);

                //插入微信信息
                if (userInfo.Wechat != null)
                {
                    const string sqlwechat = @"INSERT INTO cust_wechat(`innerid`,`custid`,`accountid`,`openid`,`nickname`,`headportrait`,`sex`,`country`,`province`,`city`,`createdtime`,`modifiedtime`)
                        VALUES(@innerid,@custid,@accountid,@openid,@nickname,@headportrait,@sex,@country,@province,@city,@createdtime,@modifiedtime);";
                    _factoy.Execute(sqlwechat, userInfo.Wechat);
                }
            }
            catch (Exception)
            {
                var s = @"INSERT INTO `car_info`
                        (`innerid`,`carid`,`title`,`pic_url`,`provid`,`cityid`,`brand_id`,`series_id`,`model_id`,`price`,`mileageid`,`register_date`,`reg_year`,`gearid`,`carageid`,`literid`,`colorid`,`carshructid`,`dischargeid`,`tel`,`contactor`,`dealer_id`,`seller_type`,`status`,`remark`,`createdtime`,`modifiedtime`,`post_time`,`audit_time`,`sold_time`,`keep_time`,`eval_price`,`next_year_eval_price`,`vpr`,`tlci_date`,`audit_date`,`mile_age`,`gear_type`,`color`,`liter`,`url`)
                        VALUES
                        (@innerid,@carid,@title,@pic_url,@provid,@cityid,@brand_id,@series_id,@model_id,@price,@mileageid,@register_date,@reg_year,@gearid,@carageid,@literid,@colorid,@carshructid,@dischargeid,@tel,@contactor,@dealer_id,@seller_type,@status,@remark,@createdtime,@modifiedtime,@post_time,@audit_time,@sold_time,@keep_time,@eval_price,@next_year_eval_price,@vpr,@tlci_date,@audit_date,@mile_age,@gear_type,@color,@liter,@url);";
                result = 0;
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
            return _factoy.Query<CustModel>(sql ,new
            {
                username = loginInfo.Username,
                mobile = loginInfo.Mobile,
                password = loginInfo.Password
            }).FirstOrDefault();
        }

        #endregion
    }
}
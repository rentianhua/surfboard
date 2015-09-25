#region

using System.Collections.Generic;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.DataAccess;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

#endregion

namespace CCN.Modules.Customer.BusinessComponent
{
    /// <summary>
    /// </summary>
    public class CustomerBC : BusinessComponentBase<CustomerDA>
    {
        /// <summary>
        /// </summary>
        /// <param name="da"></param>
        public CustomerBC(CustomerDA da)
            : base(da)
        {
            
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        //[AuditTrailCallHandler("GetALlCustomers")]
        public List<dynamic> GetALlCustomers()
        {
            return DataAccess.GetALlCustomers();
        }

        #region 用户模块

        /// <summary>
        /// 会员注册检查用户名是否被注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        public int CheckUserName(string username)
        {
            return DataAccess.CheckUserName(username);
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        public int CheckMobile(string mobile)
        {
            return DataAccess.CheckMobile(mobile);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public int CustRegister(CustModel userInfo)
        {
            //密码加密
            var en = new Encryptor();
            userInfo.Password = en.EncryptMd5(userInfo.Password);

            userInfo.Type = 1; //这版只有车商
            return DataAccess.CustRegister(userInfo);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        public CustResult CustLogin(CustLoginInfo loginInfo)
        {
            var en = new Encryptor();
            loginInfo.Password = en.EncryptMd5(loginInfo.Password);

            var result = new CustResult();
            var userInfo = DataAccess.CustLogin(loginInfo);
            if (userInfo == null)
            {
                result.errcode = 401;
                result.errmsg = "帐户名或登录密码不正确";
            }
            else if (userInfo.Status == 2)
            {
                result.errcode = 402;
                result.errmsg = "帐户被冻结";
            }
            else
            {
                result.errcode = 0;
                result.errmsg = userInfo;
            }
            return result;
        }

        #endregion
    }
}
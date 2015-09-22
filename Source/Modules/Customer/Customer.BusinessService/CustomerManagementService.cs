#region

using System.Collections.Generic;
using CCN.Modules.Customer.BusinessComponent;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.AuditTrail.Interception;

#endregion

namespace CCN.Modules.Customer.BusinessService
{
    /// <summary>
    /// </summary>
    public class CustomerManagementService : ServiceBase<CustomerBC>, ICustomerManagementService
    {
        /// <summary>
        /// </summary>
        public CustomerManagementService(CustomerBC bc)
            : base(bc)
        {
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [AuditTrailCallHandler("CustomerManagementService.GetALlCustomers")]
        public List<dynamic> GetALlCustomers()
        {
            return BusinessComponent.GetALlCustomers();
        }

        #region 用户模块

        /// <summary>
        /// 会员注册检查用户名是否被注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        [AuditTrailCallHandler("CheckUserName")]
        public int CheckUserName(string username)
        {
            return BusinessComponent.CheckUserName(username);
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        [AuditTrailCallHandler("CheckMobile")]
        public int CheckMobile(string mobile)
        {
            return BusinessComponent.CheckMobile(mobile);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CustRegister")]
        public int CustRegister(CustModel userInfo)
        {
            return BusinessComponent.CustRegister(userInfo);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        [AuditTrailCallHandler("CustLogin")]
        public CustResult CustLogin(CustLoginInfo loginInfo)
        {
            return BusinessComponent.CustLogin(loginInfo);
        }

        #endregion
    }
}
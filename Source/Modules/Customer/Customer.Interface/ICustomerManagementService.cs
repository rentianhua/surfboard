#region

using System.Collections.Generic;
using CCN.Modules.Customer.BusinessEntity;

#endregion

namespace CCN.Modules.Customer.Interface
{
    /// <summary>
    /// </summary>
    public interface ICustomerManagementService
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetALlCustomers();

        #region 用户模块
        
        /// <summary>
        /// 会员注册检查用户名是否被注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        int CheckUserName(string username);

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        int CheckMobile(string mobile);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        int CustRegister(CustModel userInfo);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        CustResult CustLogin(CustLoginInfo loginInfo);

        #endregion
    }
}
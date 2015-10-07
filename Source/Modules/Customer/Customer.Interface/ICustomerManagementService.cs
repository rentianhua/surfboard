#region

using System.Collections.Generic;
using CCN.Modules.Customer.BusinessEntity;
using Cedar.Framework.Common.BaseClasses;

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
        JResult CustLogin(CustLoginInfo loginInfo);
        
        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        JResult GetCustById(string innerid);

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CustModel> GetCustPageList(CustQueryModel query);

        #endregion

        #region 用户认证

        /// <summary>
        /// 用户添加认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        JResult AddAuthentication(CustAuthenticationModel model);

        /// <summary>
        /// 用户修改认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        JResult UpdateAuthentication(CustAuthenticationModel model);

        /// <summary>
        /// 审核认证信息
        /// </summary>
        /// <param name="info">会员相关信息</param>
        /// <returns></returns>
        JResult AuditAuthentication(CustModel info);


        /// <summary>
        /// 获取会员认证信息 by innerid
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        JResult GetCustAuthById(string innerid);

        /// <summary>
        /// 获取会员认证信息 by custid
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        JResult GetCustAuthByCustid(string custid);


        #endregion
    }
}
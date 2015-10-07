#region

using System.Collections.Generic;
using CCN.Modules.Customer.BusinessComponent;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.AuditTrail.Interception;
using Cedar.Framework.Common.BaseClasses;

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
        public JResult CustLogin(CustLoginInfo loginInfo)
        {
            return BusinessComponent.CustLogin(loginInfo);
        }


        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        public JResult GetCustById(string innerid)
        {
            return BusinessComponent.GetCustById(innerid);
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustModel> GetCustPageList(CustQueryModel query)
        {
            return BusinessComponent.GetCustPageList(query);
        }

        #endregion

        #region 用户认证

        /// <summary>
        /// 用户添加认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public JResult AddAuthentication(CustAuthenticationModel model)
        {
            return BusinessComponent.AddAuthentication(model);
        }

        /// <summary>
        /// 用户修改认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public JResult UpdateAuthentication(CustAuthenticationModel model)
        {
            return BusinessComponent.UpdateAuthentication(model);
        }

        /// <summary>
        /// 审核认证信息
        /// </summary>
        /// <param name="info">会员相关信息</param>
        /// <returns></returns>
        public JResult AuditAuthentication(CustModel info)
        {
            return BusinessComponent.AuditAuthentication(info);
        }



        /// <summary>
        /// 获取会员认证信息 by innerid
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public JResult GetCustAuthById(string innerid)
        {
            return BusinessComponent.GetCustAuthById(innerid);
        }

        /// <summary>
        /// 获取会员认证信息 by custid
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public JResult GetCustAuthByCustid(string custid)
        {
            return BusinessComponent.GetCustAuthByCustid(custid);
        }


        #endregion
    }
}
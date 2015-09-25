using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using CCN.Modules.Base.Interface;
using Cedar.Core.IoC;

namespace CCN.WebAPI.ApiControllers
{
    /// <summary>
    /// 会员模块
    /// </summary>
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        private readonly ICustomerManagementService _custservice;

        public CustomerController()
        {
            _custservice = ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();
        }

        #region 用户模块

        /// <summary>
        /// 会员注册检查用户名是否被注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        [Route("CheckUserName")]
        [HttpGet]
        public int CheckUserName(string username)
        {
            return _custservice.CheckUserName(username);
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        [Route("CheckMobile")]
        [HttpGet]
        public int CheckMobile(string mobile)
        {
            return _custservice.CheckMobile(mobile);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        [Route("CustRegister")]
        [HttpPost]
        public int CustRegister([FromBody] CustModel userInfo)
        {
            var _baseservice = ServiceLocatorFactory.GetServiceLocator().GetService<IBaseManagementService>();
            
            //检查验证码
            var cresult = _baseservice.CheckVerification(userInfo.Mobile, userInfo.VCode);
            if (cresult != 1)
            {
                //验证码错误
                return -1;
            }

            return _custservice.CustRegister(userInfo);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        [Route("CustLogin")]
        [HttpPost]
        public CustResult CustLogin([FromBody] CustLoginInfo loginInfo)
        {
            return _custservice.CustLogin(loginInfo);
        }

        #endregion
    }
}

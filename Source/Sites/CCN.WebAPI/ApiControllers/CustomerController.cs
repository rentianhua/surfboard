using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using Cedar.Core.IoC;

namespace CCN.WebAPI.ApiControllers
{
    /// <summary>
    /// 会员模块
    /// </summary>
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        private readonly ICustomerManagementService _baseservice;

        public CustomerController()
        {
            _baseservice = ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();
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
            return _baseservice.CheckUserName(username);
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
            return _baseservice.CheckMobile(mobile);
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
            return _baseservice.CustRegister(userInfo);
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
            return _baseservice.CustLogin(loginInfo);
        }

        #endregion
    }
}

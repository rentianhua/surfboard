using System.Web.Http;
using CCN.Modules.Base.Interface;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Client.DelegationHandler;

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
        public JResult CheckUserName(string username)
        {
            var result = _custservice.CheckUserName(username);
            return new JResult
            {
                errcode = result,
                errmsg = ""
            };
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        [Route("CheckMobile")]
        [HttpGet]
        public JResult CheckMobile(string mobile)
        {
            var result = _custservice.CheckMobile(mobile);
            return new JResult
            {
                errcode = result,
                errmsg = ""
            };
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>
        /// errcode,0.成功，400.验证码错误，401.异常
        /// </returns>
        [Route("CustRegister")]
        [HttpPost]
        public JResult CustRegister([FromBody] CustModel userInfo)
        {
            var baseservice = ServiceLocatorFactory.GetServiceLocator().GetService<IBaseManagementService>();
            
            //检查验证码
            var cresult = baseservice.CheckVerification(userInfo.Mobile, userInfo.VCode);
            if (cresult.errcode != 0)
            {
                //验证码错误
                //return cresult;
            }

            var result = _custservice.CustRegister(userInfo);

            return new JResult
            {
                errcode = result,
                errmsg = ""
            };
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        [Route("CustLogin")]
        [HttpPost]
        public JResult CustLogin([FromBody] CustLoginInfo loginInfo)
        {

            return _custservice.CustLogin(loginInfo);
        }
        
        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        [Route("GetCustById")]
        [HttpGet]
        public JResult GetCustById(string innerid)
        {
            return _custservice.GetCustById(innerid);
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetCustPageList")]
        [HttpPost]
        public BasePageList<CustModel> GetCustPageList([FromBody]CustQueryModel query)
        {
            return _custservice.GetCustPageList(query);
        }

        #endregion

        #region 用户认证

        /// <summary>
        /// 用户添加认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        [Route("AddAuthentication")]
        [HttpPost]
        public JResult AddAuthentication([FromBody] CustAuthenticationModel model)
        {
            return _custservice.AddAuthentication(model);
        }

        /// <summary>
        /// 用户修改认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        [Route("UpdateAuthentication")]
        [HttpPost]
        public JResult UpdateAuthentication([FromBody] CustAuthenticationModel model)
        {
            return _custservice.UpdateAuthentication(model);
        }

        /// <summary>
        /// 审核认证信息
        /// </summary>
        /// <param name="info">会员相关信息</param>
        /// <returns></returns>
        [Route("AuditAuthentication")]
        [HttpPost]
        [ApplicationContextFilter]
        public JResult AuditAuthentication([FromBody] CustModel info)
        {
            return _custservice.AuditAuthentication(info);
        }

        /// <summary>
        /// 获取会员认证信息 by innerid
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        [Route("GetCustAuthById")]
        [HttpGet]
        public JResult GetCustAuthById(string innerid)
        {
            return _custservice.GetCustAuthById(innerid);
        }

        /// <summary>
        /// 获取会员认证信息 by custid
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        [Route("GetCustAuthByCustid")]
        [HttpGet]
        public JResult GetCustAuthByCustid(string custid)
        {
            return _custservice.GetCustAuthByCustid(custid);
        }
        #endregion
    }
}

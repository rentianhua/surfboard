using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Cedar.Core.EntLib.Logging;
using Cedar.Core.Logging;

namespace CCN.WebAPI.Common
{
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0) // 允许匿名访问
                {
                    base.OnActionExecuting(actionContext);
                    return;
                }

                var token = actionContext.Request.Headers.Authorization?.ToString();
                if (string.IsNullOrEmpty(token))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    return;
                }

                var apiToken = ConfigurationManager.AppSettings["apiToken"] ?? "";

                if (apiToken.Equals(token))
                    return;

                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("接口请求异常:", TraceEventType.Error, ex);
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}

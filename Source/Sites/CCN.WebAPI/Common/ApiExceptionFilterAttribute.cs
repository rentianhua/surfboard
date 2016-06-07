using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Newtonsoft.Json;

namespace CCN.WebAPI.Common
{
    public class ApiExceptionFilterAttribute: ExceptionFilterAttribute
    {
        /// <summary>
        /// Called when [exception].
        /// log exception
        /// </summary>
        /// <param name="context">The context.</param>
        /// { Created At Time:[ 2015/1/30 15:08], By User:Administrator, On Machine:APP-DEV-SAMWANG }
        public override void OnException(HttpActionExecutedContext context)
        {
            //request method
            var method = context.ActionContext.Request.Method.Method;
            //request absoluteuri
            var url = context.ActionContext.Request.RequestUri.AbsoluteUri;
            //request route
            var route = context.ActionContext.Request.GetRouteData().Route.RouteTemplate;
            //request controller
            var controllerName = context.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //request action
            var actionName = context.ActionContext.ActionDescriptor.ActionName;
            //exception message
            var exception = context.Exception.GetBaseException();
            var message = exception.Message;

            //return context response
            const string jResult = "{\"errcode\":500,\"errmsg\":\"内部服务器错误\"}";
            context.Response = new HttpResponseMessage { Content = new StringContent(jResult,Encoding.UTF8, "application/json") };
            var log = $"{method} {url}, route: {route}, controller:{controllerName}, action:{actionName}, exception:{message}";

            LoggerFactories.CreateLogger().Write("Api Error:" + log, TraceEventType.Error);
            base.OnException(context);
        }
    }
}
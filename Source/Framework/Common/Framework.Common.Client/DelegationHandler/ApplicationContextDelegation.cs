using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Cedar.Core.ApplicationContexts;
using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;

namespace Cedar.Framework.Common.Client.DelegationHandler
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationContextFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //ApplicationContext.Current.UserId= filterContext.HttpContext.Request.Headers.
        }
    }
}

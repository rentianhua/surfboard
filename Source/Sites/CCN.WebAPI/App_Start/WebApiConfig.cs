using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CCN.WebAPI.Common;
using Cedar.Framework.Common.Client.DelegationHandler;
using Cedar.Framework.Common.Client.WebAPI;

namespace CCN.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Web API configuration and services
            config.DependencyResolver = new ServiceLocatableDependencyResolver();

            //add ApplicationContextFilterAttribute
            config.Filters.Add(new ApplicationContextFilterAttribute());
            config.Filters.Add(new ApiExceptionFilterAttribute());
            //config.Filters.Add(new AuthorizeFilterAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

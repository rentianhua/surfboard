using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCN.Resource.Main.Common;

namespace CCN.Resource.Common
{
    public class LoginCheckFilterAttribute : ActionFilterAttribute
    {
        //表示是否检查登录
        public bool IsCheck { get; set; }

        //Action方法执行之前执行此方法
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var a = filterContext.RouteData.DataTokens["area"];
            if (IsCheck)
            {
                //校验用户是否已经登录
                if (filterContext.HttpContext.Session["UserInfo"] == null && filterContext.HttpContext.Session["CustModel"] == null)
                {
                    if (filterContext.HttpContext.Request.Cookies["type"] == null)
                    {
                        //跳转到登陆页
                        filterContext.HttpContext.Response.Redirect("/Home/BusinessLogin");
                    }
                    else
                    {
                        if (filterContext.HttpContext.Request.Cookies["type"].Value.ToString() == "1")
                        {
                            //跳转到登陆页
                            filterContext.HttpContext.Response.Redirect("/Home/Login");
                        }
                        else
                        {
                            //跳转到商户登入
                            filterContext.HttpContext.Response.Redirect("/Home/BusinessLogin");
                        }
                    }
                }

                
            }
        }
    }
}
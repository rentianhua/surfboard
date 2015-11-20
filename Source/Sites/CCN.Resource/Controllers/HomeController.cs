using CCN.Modules.Base.BusinessEntity;
using CCN.Modules.Base.Interface;
using CCN.Resource.Main.Common;
using Cedar.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CCN.Resource.Controllers
{
    public class HomeController : DefaultController
    {
        /// <summary>
        /// 基础模块数据
        /// </summary>
        private readonly IBaseManagementService _baseservice;

        /// <summary>
        /// 构造函数
        /// </summary>
        public HomeController()
        {
            _baseservice = ServiceLocatorFactory.GetServiceLocator().GetService<IBaseManagementService>();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "苏州车信网";
            ViewBag.UserInfo = (BaseUserInfo)Session["UserInfo"];
            return View();
        }

        /// <summary>
        /// 左边菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult LeftMenu()
        {
            return View();
        }

        /// <summary>
        /// 登入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 验证登入信息
        /// </summary>
        /// <param name="loginname">登入账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public ActionResult CheckLogin(string loginname, string password)
        {
            Session.Timeout = 30;
            var user = _baseservice.GetUserInfo(loginname, password);
            if (user.errcode == 0)
            {
                UserInfo = (BaseUserInfo)user.errmsg;
                Session["UserInfo"] = user.errmsg;
                return Json(new { code = 1, message = "登录成功" });
            }
            else
            {
                return Json(new { code = 0, message = "登录失败" });
            }
        }

        public static XmlReader CreateReader(string filename, bool ignorecomments = true)
        {
            var settings = new XmlReaderSettings();
            settings.IgnoreComments = ignorecomments;
            return XmlReader.Create(filename, settings);
        }
    }
}

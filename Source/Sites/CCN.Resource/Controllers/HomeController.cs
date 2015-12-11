using CCN.Modules.Base.BusinessEntity;
using CCN.Modules.Base.Interface;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Resource.Common;
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
        HttpCookie cookie = new HttpCookie("type");
        public ActionResult Index()
        {
            var UserInfosdfd = UserInfo;
            ViewBag.Title = "苏州车信网";
            ViewBag.UserInfo = (BaseUserModel)Session["UserInfo"];
            if (ADMIN == UserInfo.innerid)
            {
                ViewBag.Admin = "1";
            }

            return View();
        }

        /// <summary>
        /// 左边菜单
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult LeftMenu()
        {
            ////获取所有菜单
            //BaseMenuModel model = new BaseMenuModel();
            //model.isenabled = 1;
            //var allmenu = (IEnumerable<BaseMenuModel>)_baseservice.GetAllMenu(model).errmsg;
            //获取该用户的菜单权限
            var usermenu = _baseservice.GetMenuByUerid(UserInfo.innerid).errmsg;
            ViewBag.menu = usermenu;
            return PartialView(usermenu);
        }

        /// <summary>
        /// 登入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginCheckFilterAttribute(IsCheck = false)]
        public ActionResult Login(string type)
        {
            cookie.Value = "1";
            if (!string.IsNullOrWhiteSpace(type))
            {
                ViewBag.type = "1";
            }
            Response.AppendCookie(cookie);
            return View();
        }

        /// <summary>
        /// 验证登入信息
        /// </summary>
        /// <param name="loginname">登入账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [LoginCheckFilterAttribute(IsCheck = false)]
        public ActionResult CheckLogin(string loginname, string password)
        {
            Session.Timeout = 120;
            var user = _baseservice.GetUserInfo(loginname, password);
            if (user.errcode == 0)
            {
                UserInfo = (BaseUserModel)user.errmsg;
                Session["UserInfo"] = user.errmsg;
                return Json(new { code = 1, message = "登录成功" });
            }
            else
            {
                return Json(new { code = 0, message = "登录失败" });
            }
        }

        /// <summary>
        /// 车商登入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginCheckFilterAttribute(IsCheck = false)]
        public ActionResult BusinessLogin(string type)
        {
            cookie.Value = "0";
            if (!string.IsNullOrWhiteSpace(type))
            {
                ViewBag.type = "1";
            }
            Response.AppendCookie(cookie);
            return View();
        }

        public ActionResult BusinessIndex()
        {
            ViewBag.Title = "苏州车信网";
            ViewBag.UserInfo = (CustModel)Session["CustModel"];
            return View();
        }

        /// <summary>
        /// 记录车商登入信息
        /// </summary>
        /// <param name="customerinfo"></param>
        /// <returns></returns>
        [LoginCheckFilterAttribute(IsCheck = false)]
        public ActionResult CheckBusinessLogin(CustModel customerinfo)
        {
            Session.Timeout = 120;
            Session["CustModel"] = customerinfo;
            return Json(new { code = 1, message = "登录成功" });
        }

        public static XmlReader CreateReader(string filename, bool ignorecomments = true)
        {
            var settings = new XmlReaderSettings();
            settings.IgnoreComments = ignorecomments;
            return XmlReader.Create(filename, settings);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CCN.Resource.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "苏州车信网";
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

        public static XmlReader CreateReader(string filename, bool ignorecomments = true)
        {
            var settings = new XmlReaderSettings();
            settings.IgnoreComments = ignorecomments;
            return XmlReader.Create(filename, settings);
        }
    }
}

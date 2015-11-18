using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCN.Resource.Areas.System.Controllers
{
    public class SystemController : Controller
    {
        // GET: System/System
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuList()
        {
            return View();
        }
        /// <summary>
        /// 获取基础数据代码类型
        /// </summary>
        /// <returns></returns>
        public ActionResult BaseCodeTypeList() {
            return View();
        }

    }
}
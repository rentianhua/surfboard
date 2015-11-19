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
        #region 基础数据代码类型
        /// <summary>
        /// 获取基础数据代码类型
        /// </summary>
        /// <returns></returns>
        public ActionResult BaseCodeTypeList() {
            return View();
        }
        /// <summary>
        /// 展示基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public ActionResult BaseCodeTypeView(string innerid) {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }
        /// <summary>
        /// 编辑基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public ActionResult BaseCodeTypeEdit(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }
        #endregion
        #region 基础数据代码值
        /// <summary>
        /// 获取基础数据代码值
        /// </summary>
        /// <returns></returns>
        public ActionResult BaseCodeList() {
            return View();
        }
        /// <summary>
        /// 编辑基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public ActionResult BaseCodeEdit(string innerid) {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }
        /// <summary>
        /// 展示基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public ActionResult BaseCodeView(string innerid) {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }
        #endregion

    }
}
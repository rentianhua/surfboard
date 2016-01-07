using CCN.Resource.Main.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCN.Resource.Areas.System.Controllers
{
    public class SystemController : DefaultController
    {
        // GET: System/System
        public ActionResult Index()
        {
            return View();
        }

        #region 基础数据代码类型
        /// <summary>
        /// 获取基础数据代码类型
        /// </summary>
        /// <returns></returns>
        public ActionResult BaseCodeTypeList()
        {
            return View();
        }
        /// <summary>
        /// 展示基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public ActionResult BaseCodeTypeView(string innerid)
        {
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
        public ActionResult BaseCodeList()
        {
            return View();
        }
        /// <summary>
        /// 编辑基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public ActionResult BaseCodeEdit(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }
        /// <summary>
        /// 展示基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public ActionResult BaseCodeView(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }
        #endregion

        #region 系统管理

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SysUserList()
        {
            if (ADMIN.ToString().ToLower()==UserInfo.innerid.ToLower())
            {
                ViewBag.isadmin = 1;
            }
            ViewBag.adminid = ADMIN;
            return View();
        }

        /// <summary>
        /// 用户编辑/新增
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult SysUserEdit(string innerid)
        {
            if (!string.IsNullOrWhiteSpace(innerid))
            {
                ViewBag.innerid = innerid;
            }
            return View();
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SysRoleList()
        {
            if (ADMIN.ToString().ToLower() == UserInfo.innerid.ToLower())
            {
                ViewBag.isadmin = 1;
            }
            ViewBag.adminid = ADMIN;
            return View();
        }

        /// <summary>
        /// 角色编辑/新增
        /// </summary>
        /// <returns></returns>
        public ActionResult SysRoleEdit(string innerid)
        {
            if (!string.IsNullOrWhiteSpace(innerid))
            {
                ViewBag.innerid = innerid;
            }
            return View();
        }

        /// <summary>
        /// 权限列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SysRightList()
        {
            return View();
        }

        #region 菜单管理

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuList()
        {
            return View();
        }

        /// <summary>
        /// 菜单编辑/新增页面
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public ActionResult SysMenuEdit(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }
        #endregion

        #region 城市管理

        /// <summary>
        /// 城市管理列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult SysManageCityList()
        {
            return View();
        }

        /// <summary>
        /// 城市管理编辑页
        /// </summary>
        /// <returns></returns>
        public ActionResult SysManageCityEdit(string innerid)
        {
            if (!string.IsNullOrWhiteSpace(innerid))
            {
                ViewBag.innerid = innerid;
            }
            return View();
        }
        #endregion

        #endregion

    }
}
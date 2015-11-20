using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCN.Resource.Areas.Rewards.Controllers
{
    public class RewardsController : Controller
    {
        public ActionResult CouponList()
        {
            return View();
        }

        public ActionResult CouponEdit(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }

        public ActionResult CouponView(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }

        /// <summary>
        /// 商品id
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductList()
        {
            return View();
        }

        /// <summary>
        /// 商铺列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopList()
        {
            return View();
        }

        /// <summary>
        /// 商铺编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopEdit(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }

        #region 结算记录

        /// <summary>
        /// 已核销记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchCancelList()
        {
            return View();
        }

        /// <summary>
        /// 结算记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SettlementList()
        {
            return View();
        }

        /// <summary>
        /// 结算记录编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult SettlementEdit(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }

        #endregion

    }
}
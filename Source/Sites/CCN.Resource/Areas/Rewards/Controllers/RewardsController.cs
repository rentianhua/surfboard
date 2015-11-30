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

        /// <summary>
        /// 商铺职员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopStaffList()
        {
            return View();
        }

        /// <summary>
        /// 商铺职员编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopStaffEdit(string innerid)
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


        /// <summary>
        /// 结算页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SettlementAdd(string shopid, string setttotal, string scstart,string scend)
        {
            ViewBag.shopid = string.IsNullOrWhiteSpace(shopid) ? "" : shopid;
            ViewBag.setttotal = string.IsNullOrWhiteSpace(setttotal) ? "0" : setttotal;
            ViewBag.scstart = string.IsNullOrWhiteSpace(scstart) ? "" : scstart;
            ViewBag.scend = string.IsNullOrWhiteSpace(scend) ? "" : scend;
            return View();
        }

        /// <summary>
        /// 结算详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SettCodeRecord(string settid)
        {
            ViewBag.settid = string.IsNullOrWhiteSpace(settid) ? "" : settid;
            return View();
        }
        #endregion

    }
}
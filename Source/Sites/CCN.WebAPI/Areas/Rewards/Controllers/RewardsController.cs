using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCN.WebAPI.Areas.Rewards.Controllers
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
        /// 商铺id
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopList()
        {
            return View();
        }
    }
}
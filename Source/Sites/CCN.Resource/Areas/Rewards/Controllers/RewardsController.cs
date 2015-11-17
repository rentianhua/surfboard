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
    }
}
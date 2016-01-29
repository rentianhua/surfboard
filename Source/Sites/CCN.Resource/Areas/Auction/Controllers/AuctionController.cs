using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCN.Resource.Main.Common;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Resource.Areas.Auction.Controllers
{
    public class AuctionController : DefaultController
    {
        public ActionResult AuctionCarList()
        {
            return View();
        }

        public ActionResult AuctionCarEdit(string id)
        {
            ViewBag.carid = string.IsNullOrWhiteSpace(id) ? "" : id;
            ViewBag.uptoken = QiniuUtility.GetToken();
            return View();
        }

        public ActionResult AuctionCarView(string id)
        {
            ViewBag.carid = string.IsNullOrWhiteSpace(id) ? "" : id;
            return View();
        }

        public ActionResult AuctionParticipantList(string auctionid)
        {
            ViewBag.auctionid = string.IsNullOrWhiteSpace(auctionid) ? "" : auctionid;
            return View();
        }

        //交押金人员列表
        public ActionResult AuctionDepositList(string auctionid)
        {
            ViewBag.auctionid = string.IsNullOrWhiteSpace(auctionid) ? "" : auctionid;
            return View();
        }

        //交押金信息修改页面
        public ActionResult AuctionDepositEdit(string auctionid, string depid)
        {
            ViewBag.auctionid = string.IsNullOrWhiteSpace(auctionid) ? "" : auctionid;
            ViewBag.depid = string.IsNullOrWhiteSpace(depid) ? "" : depid;
            return View();
        }
    }
}
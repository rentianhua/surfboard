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

        public ActionResult AuctionCarEdit(string carid,string id)
        {
            ViewBag.carid = string.IsNullOrWhiteSpace(carid) ? "" : carid;
            ViewBag.id = string.IsNullOrWhiteSpace(id) ? "" : id;
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

        /// <summary>
        /// 检测报告页面
        /// </summary>
        /// <param name="carid">车辆ID</param>
        /// <param name="auctionid">拍卖ID</param>
        /// <returns></returns>
        public ActionResult AuctionEvaluationPics(string carid,string auctionid)
        {
            ViewBag.Carid = string.IsNullOrWhiteSpace(carid) ? "" : carid; ;
            ViewBag.Auctionid = string.IsNullOrWhiteSpace(auctionid) ? "" : auctionid; ;
            return View();
        }
    }
}
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
            ViewBag.UserLevel = UserInfo.level;
            ViewBag.DepId = UserInfo.depid;
            ViewBag.UserId = UserInfo.innerid;
            return View();
        }

        /// <summary>
        /// 拍卖车辆编辑
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AuctionCarEdit(string carid,string id)
        {
            ViewBag.carid = string.IsNullOrWhiteSpace(carid) ? "" : carid;
            ViewBag.id = string.IsNullOrWhiteSpace(id) ? "" : id;
            ViewBag.UserId = UserInfo.innerid;
            ViewBag.uptoken = QiniuUtility.GetToken();
            return View();
        }

        /// <summary>
        /// 拍卖车辆新增
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AuctionCarAdd(string carid, string id)
        {
            ViewBag.carid = string.IsNullOrWhiteSpace(carid) ? "" : carid;
            ViewBag.id = string.IsNullOrWhiteSpace(id) ? "" : id;
            ViewBag.uptoken = QiniuUtility.GetToken();
            ViewBag.UserId = UserInfo.innerid;
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
            ViewBag.Auctionid = string.IsNullOrWhiteSpace(auctionid) ? "" : auctionid; 
            return View();
        }

        /// <summary>
        /// 竞拍记录
        /// </summary>
        /// <param name="auctionid"></param>
        /// <returns></returns>
        public ActionResult AuctionRecordList(string auctionid)
        {
            ViewBag.Auctionid = string.IsNullOrWhiteSpace(auctionid) ? "" : auctionid;
            ViewBag.UserLevel = UserInfo.level;
            ViewBag.UserId = UserInfo.innerid;
            return View();
        }

        /// <summary>
        /// 竞拍记录详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="auctionid"></param>
        /// <returns></returns>
        public ActionResult AuctionRecordEdit(string id,string auctionid)
        {
            ViewBag.participantid= string.IsNullOrWhiteSpace(id) ? "" : id;
            ViewBag.auctionid = string.IsNullOrWhiteSpace(auctionid) ? "" : auctionid;
            ViewBag.UserName = UserInfo.username;
            ViewBag.UserNo = UserInfo.no;
            return View();
        }
    }
}
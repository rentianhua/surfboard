using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCN.Resource.Main.Common;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Resource.Areas.Activity.Controllers
{
    public class ActivityController : DefaultController
    {
        #region 投票活动
        
        /// <summary>
        /// 投票活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult VoteList()
        {
            return View();
        }

        /// <summary>
        /// 投票活动编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult VoteEdit(string voteid)
        {
            ViewBag.voteid = string.IsNullOrWhiteSpace(voteid) ? "" : voteid;
            return View();
        }

        /// <summary>
        /// 投票活动详情
        /// </summary>
        /// <returns></returns>
        public ActionResult VoteView(string voteid)
        {
            ViewBag.voteid = string.IsNullOrWhiteSpace(voteid) ? "" : voteid;
            return View();
        }

        /// <summary>
        /// 投票活动参赛人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult VotePerList(string voteid)
        {
            ViewBag.voteid = string.IsNullOrWhiteSpace(voteid) ? "" : voteid;
            return View();
        }

        /// <summary>
        /// 投票活动参赛人员编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult VotePerEdit(string perid)
        {
            ViewBag.perid = string.IsNullOrWhiteSpace(perid) ? "" : perid;
            return View();
        }

        /// <summary>
        /// 投票活动参赛人员详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult VotePerView(string perid)
        {
            ViewBag.perid = string.IsNullOrWhiteSpace(perid) ? "" : perid;
            return View();
        }

        /// <summary>
        /// 投票活动参赛人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult VoteLogList(string voteid, string perid)
        {
            ViewBag.voteid = string.IsNullOrWhiteSpace(voteid) ? "" : voteid;
            ViewBag.perid = string.IsNullOrWhiteSpace(perid) ? "" : perid;
            return View();
        }

        #endregion

        #region 众筹活动

        /// <summary>
        /// 活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CrowdList()
        {
            return View();
        }

        /// <summary>
        /// 活动编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult CrowdEdit(string activityid)
        {
            ViewBag.uptoken = QiniuUtility.GetToken();
            ViewBag.activityid = string.IsNullOrWhiteSpace(activityid) ? "" : activityid;
            return View();
        }

        /// <summary>
        /// 活动详情
        /// </summary>
        /// <returns></returns>
        public ActionResult CrowdView(string flagcode)
        {
            ViewBag.flagcode = string.IsNullOrWhiteSpace(flagcode) ? "" : flagcode;
            return View();
        }

        /// <summary>
        /// 活动档次列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CrowdGradeList(string activityid)
        {
            ViewBag.activityid = string.IsNullOrWhiteSpace(activityid) ? "" : activityid;
            return View();
        }

        /// <summary>
        /// 活动档次编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult CrowdGradeEdit(string activityid,string gradeid)
        {
            ViewBag.activityid = string.IsNullOrWhiteSpace(activityid) ? "" : activityid;
            ViewBag.gradeid = string.IsNullOrWhiteSpace(gradeid) ? "" : gradeid;
            return View();
        }

        /// <summary>
        /// 活动参与人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CrowdPlayerList(string flagcode)
        {
            ViewBag.flagcode = string.IsNullOrWhiteSpace(flagcode) ? "" : flagcode;
            return View();
        }

        /// <summary>
        /// 活动参与人员详情
        /// </summary>
        /// <returns></returns>
        public ActionResult CrowdPlayerView(string flagcode, string playerid)
        {
            ViewBag.flagcode = string.IsNullOrWhiteSpace(flagcode) ? "" : flagcode;
            ViewBag.playerid = string.IsNullOrWhiteSpace(playerid) ? "" : playerid;
            return View();
        }

        #endregion
    }
}
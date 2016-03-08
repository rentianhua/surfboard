using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCN.Resource.Main.Common;

namespace CCN.Resource.Areas.Activity.Controllers
{
    public class ActivityController : DefaultController
    {
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
    }
}
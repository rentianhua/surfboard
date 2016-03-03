#region

using CCN.Resource.Main.Common;
using Cedar.Framework.Common.BaseClasses;
using System.Web.Mvc;

#endregion

namespace CCN.Resource.Areas.Customer.Controllers
{
    public class CustomerController : DefaultController
    {
        //private readonly ICustomerManagementService _service;

        #region 用户认证

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerList()
        {
            if (ADMIN != UserInfo.innerid)
            {
                ViewBag.userid = UserInfo.innerid;
            }
            return View();
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerView(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }

        /// <summary>
        /// 用户编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerAdd(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }
        #endregion

        #region 粉丝信息
        /// <summary>
        /// 粉丝列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CustWeChatList()
        {
            return View();
        }
        #endregion

        #region 车信评论

        /// <summary>
        /// 企业列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyList()
        {
            return View();
        }

        /// <summary>
        /// 企业修改
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyEdit(string innerid)
        {
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            new QiniuUtility();
            ViewBag.uptoken = QiniuUtility.GetToken();
            return View();
        }

        /// <summary>
        /// 企业修改申请列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyUpdateList(string companyid)
        {
            ViewBag.companyid = string.IsNullOrWhiteSpace(companyid) ? "" : companyid;
            return View();
        }

        /// <summary>
        /// 企业修改申请详情查看页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyUpdateView(string applyid)
        {
            ViewBag.UserId = UserInfo.innerid;
            ViewBag.applyid = string.IsNullOrWhiteSpace(applyid) ? "" : applyid;
            return View();
        }

        /// <summary>
        /// 评论列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CommentList()
        {
            return View();
        }

        /// <summary>
        /// 评论详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public ActionResult CommentView(string innerid)
        {
            ViewBag.Innerid= string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            return View();
        }

        #endregion
    }
}
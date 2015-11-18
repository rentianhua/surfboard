#region

using System.Web.Mvc;

#endregion

namespace CCN.WebAPI.Areas.Customer.Controllers
{
    public class CustomerController : Controller
    {
        //private readonly ICustomerManagementService _service;

        #region 用户认证

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerList()
        {
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
        /// <summary>
        /// 粉丝列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CustWeChatList() {
            return View();
        }
    }
}
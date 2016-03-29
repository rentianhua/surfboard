using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using CCN.Resource.Main.Common;

namespace CCN.Resource.Areas.Car.Controllers
{
    public class CarController : DefaultController
    {
        // GET: Car/Car
        public ActionResult CarList(string custid)
        {
            if ((CustModel)Session["CustModel"] !=null)
            {
                custid = ((CustModel)Session["CustModel"]).Innerid;
            }
            if (string.IsNullOrWhiteSpace(custid))
            {
                return Redirect("/Customer/Customer/CustomerList");
            }

            var custservice = ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();
            var jresult = custservice.GetCustById(custid);
            if (jresult != null && jresult.errcode == 0)
            {
                var custModel = (CustViewModel)jresult.errmsg;
                ViewBag.custname = "[" + custModel.Custname + "]";
            }

            ViewBag.custid = custid;
            return View();
        }

        public ActionResult CarEdit(string custid, string carid)
        {
            if (string.IsNullOrWhiteSpace(custid))
            {
                return Redirect("/Customer/Customer/CustomerList");
            }

            ViewBag.custid = custid;
            ViewBag.carid = string.IsNullOrWhiteSpace(carid) ? "" : carid;
            new QiniuUtility();
            ViewBag.uptoken = QiniuUtility.GetToken();

            return View();
        }

        /// <summary>
        /// 车辆列表（没有录车功能）
        /// </summary>
        /// <returns></returns>
        public ActionResult CarShowList()
        {
            if (ADMIN != UserInfo.innerid)
            {
                ViewBag.userid = UserInfo.innerid;
            }
            return View();
        }

        /// <summary>
        /// 车辆详情
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        public ActionResult CarView(string carid)
        {
            ViewBag.carid = string.IsNullOrWhiteSpace(carid) ? "" : carid;
            return View();
        }

        /// <summary>
        /// 车贷列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CarLoanList()
        {
            return View();
        }
    }
}
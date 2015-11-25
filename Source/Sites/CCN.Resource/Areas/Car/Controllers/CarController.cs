using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using Cedar.Core.IoC;

namespace CCN.Resource.Areas.Car.Controllers
{
    public class CarController : Controller
    {
        // GET: Car/Car
        public ActionResult CarList(string custid)
        {
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
            return View();
        }

        /// <summary>
        /// 车辆列表（没有录车功能）
        /// </summary>
        /// <returns></returns>
        public ActionResult CarShowList()
        {
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
    }
}
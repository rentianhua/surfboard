using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCN.WebAPI.Areas.Car.Controllers
{
    public class CarController : Controller
    {
        // GET: Car/Car
        public ActionResult CarList(string custid)
        {
            if (string.IsNullOrWhiteSpace(custid))
            {
                return RedirectToAction("CustomerList", "Customer");
            }



            return View();
        }

        public ActionResult CarEdit()
        {
            return View();
        }
    }
}
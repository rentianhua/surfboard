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
        public ActionResult CarList()
        {
            return View();
        }

        public ActionResult CarEdit()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCN.WebAPI.Areas.DataAnalysis.Controllers
{
    public class DataAnalysisController : Controller
    {
        /// <summary>
        /// 增长量统计
        /// </summary>
        /// <returns></returns>
        public ActionResult CustGrowthList()
        {
            return View();
        }
    }
}
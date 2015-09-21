using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCN.Modules.Base.BusinessEntity;
using CCN.Modules.Base.Interface;
using Cedar.Core.IoC;

namespace CCN.WebAPI.Areas.Base.Controllers
{
    public class BaseController : Controller
    {
        private readonly IBaseManagementService _baseservice;

        public BaseController()
        {
            _baseservice = ServiceLocatorFactory.GetServiceLocator().GetService<IBaseManagementService>();
        }

        // GET: Base/Base
        public ActionResult Index()
        {
            var provList = _baseservice.GetProvList("");
            return View(provList);
        }


    }
}
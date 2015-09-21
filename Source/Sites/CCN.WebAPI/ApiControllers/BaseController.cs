using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCN.Modules.Base.BusinessEntity;
using CCN.Modules.Base.Interface;
using Cedar.Core.IoC;

namespace CCN.WebAPI.ApiControllers
{
    public class BaseController : ApiController
    {
        private readonly IBaseManagementService _baseservice;

        public BaseController()
        {
            _baseservice = ServiceLocatorFactory.GetServiceLocator().GetService<IBaseManagementService>();
        }

        [Route("/api/Base/GetProvList/{initial}")]
        [HttpGet]
        public IEnumerable<BaseProvince> GetProvList(string initial)
        {
            return _baseservice.GetProvList(initial);
        }
    }
}

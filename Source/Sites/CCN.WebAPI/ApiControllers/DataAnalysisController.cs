using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCN.Modules.DataAnalysis.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;

using System.Threading.Tasks;
using Cedar.Core.ApplicationContexts;

namespace CCN.WebAPI.ApiControllers
{
    /// <summary>
    /// 数据接口
    /// </summary>
    [RoutePrefix("api/DataAnalysis")]
    public class DataAnalysisController : ApiController
    {
        private readonly IDataAnalysisManagementService _dataanalysisservice;

        public DataAnalysisController()
        {
            _dataanalysisservice = ServiceLocatorFactory.GetServiceLocator().GetService<IDataAnalysisManagementService>();
        }

        /// <summary>
        /// 本地市场按月的持有量TOP10
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("GetLocalByMonthTop10")]
        [HttpGet]
        public JResult GetLocalByMonthTop10()
        {
            var result = _dataanalysisservice.GetLocalByMonthTop10();
            return result;
        }
    }
}

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
    /// <summary>
    /// 基础模块
    /// </summary>
    [RoutePrefix("api/Base")]
    public class BaseController : ApiController
    {
        private readonly IBaseManagementService _baseservice;

        public BaseController()
        {
            _baseservice = ServiceLocatorFactory.GetServiceLocator().GetService<IBaseManagementService>();
        }

        #region 区域

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial"></param>
        /// <returns></returns>
        [Route("GetProvList")]
        [HttpGet]
        public IEnumerable<BaseProvince> GetProvList(string initial = null)
        {
            return _baseservice.GetProvList(initial);
        }

        /// <summary>
        /// 根据省份id获取城市
        /// </summary>
        /// <param name="provid">省份id</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        [Route("GetCityList/{provid}")]
        [HttpGet]
        public IEnumerable<BaseCity> GetCityList(int provid, string initial = null)
        {
            return _baseservice.GetCityList(provid, initial);
        }

        #endregion

        #region 品牌/车系/车型

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        [Route("GetCarBrand")]
        [HttpGet]
        public IEnumerable<BaseCarBrandModel> GetCarBrand(string initial = null)
        {
            return _baseservice.GetCarBrand(initial);
        }

        /// <summary>
        /// 根据品牌id获取车系
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <returns></returns>
        [Route("GetCarSeries/{brandId}")]
        [HttpGet]
        public IEnumerable<BaseCarSeriesModel> GetCarSeries(int brandId)
        {
            return _baseservice.GetCarSeries(brandId);
        }

        /// <summary>
        /// 根据车系ID获取车型
        /// </summary>
        /// <param name="seriesId">车系id</param>
        /// <returns></returns>
        [Route("GetCarModel/{seriesId}")]
        [HttpGet]
        public IEnumerable<BaseCarModelModel> GetCarModel(int seriesId)
        {
            return _baseservice.GetCarModel(seriesId);
        }

        #endregion
    }
}

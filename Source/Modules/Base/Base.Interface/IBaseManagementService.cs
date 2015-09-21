﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Base.BusinessEntity;

namespace CCN.Modules.Base.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseManagementService
    {
        #region 区域

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        IEnumerable<BaseProvince> GetProvList(string initial);

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="provId">省份ID</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        IEnumerable<BaseCity> GetCityList(int provId, string initial);

        #endregion

        #region 品牌/车系/车型

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        IEnumerable<BaseCarBrandModel> GetCarBrand(string initial);

        /// <summary>
        /// 根据品牌id获取车系
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <returns></returns>
        IEnumerable<BaseCarSeriesModel> GetCarSeries(int brandId);

        /// <summary>
        /// 根据车系ID获取车型
        /// </summary>
        /// <param name="seriesId">车系id</param>
        /// <returns></returns>
        IEnumerable<BaseCarModelModel> GetCarModel(int seriesId);

        #endregion
    }
}

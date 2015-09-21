using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Base.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Base.DataAccess
{
    /// <summary>
    /// 基础模块
    /// </summary>
    public class BaseDA  : DataAccessBase
    {
        private readonly Database _factoy;

        /// <summary>
        /// 
        /// </summary>
        public BaseDA() 
        {
            _factoy = new DatabaseWrapperFactory().GetDatabase("mysqldb");
        }

        #region 区域

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseProvince> GetProvList(string initial)
        {
            string where = " isenabled=1";
            if (!string.IsNullOrWhiteSpace(initial))
            {
                where += $" and initial='{initial.ToUpper()}'";
            }
            var sql = $"select innerid, provname, initial, isenabled, remark from base_province where {where}";
            var provList = _factoy.Query<BaseProvince>(sql);
            return provList;
        }

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="provId">省份ID</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseCity> GetCityList(int provId,string initial)
        {
            string where = " isenabled=1 and provid=" + provId;
            if (!string.IsNullOrWhiteSpace(initial))
            {
                where += $" and initial='{initial.ToUpper()}'";
            }
            var sql = $"select innerid, cityname, initial, provid, isenabled, remark from base_city where {where}";
            var cityList = _factoy.Query<BaseCity>(sql);
            return cityList;
        }

        #endregion

        #region 品牌/车系/车型

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseCarBrandModel> GetCarBrand(string initial)
        {
            var where = " isenabled=1";
            if (!string.IsNullOrWhiteSpace(initial))
            {
                where += $" and initial='{initial.ToUpper()}'";
            }
            var sql = $"select innerid, brandname, initial, isenabled, remark, logurl from base_carbrand where {where}";
            var brandList = _factoy.Query<BaseCarBrandModel>(sql);
            return brandList;
        }

        /// <summary>
        /// 根据品牌id获取车系
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <returns></returns>
        public IEnumerable<BaseCarSeriesModel> GetCarSeries(int brandId)
        {
            var sql = $"select innerid, seriesname, seriesgroupname, brandid, isenabled, remark from base_carseries where isenabled=1 and brandid={brandId}";
            var seriesList = _factoy.Query<BaseCarSeriesModel>(sql);
            return seriesList;
        }

        /// <summary>
        /// 根据车系ID获取车型
        /// </summary>
        /// <param name="seriesId">车系id</param>
        /// <returns></returns>
        public IEnumerable<BaseCarModelModel> GetCarModel(int seriesId)
        {
            var sql = $"select innerid, modelname, modelprice, modelyear, minregyear, maxregyear, liter, geartype, dischargestandard, seriesid, isenabled, remark from base_carmodel where isenabled=1 and seriesid={seriesId}";
            var modelList = _factoy.Query<BaseCarModelModel>(sql);
            return modelList;
        }

        #endregion
    }
}

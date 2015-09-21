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

        #region Area
        
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
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseProvince> GetCarBrand(string initial)
        {
            string where = " isenabled=1";
            if (!string.IsNullOrWhiteSpace(initial))
            {
                where += $" and initial='{initial.ToUpper()}'";
            }
            var sql = $"select innerid, name, initial, parentid, level, isenabled, remark, logurl from base_carbrand  where {where}";
            var provList = _factoy.Query<BaseProvince>(sql);
            return provList;
        }

        #endregion
    }
}

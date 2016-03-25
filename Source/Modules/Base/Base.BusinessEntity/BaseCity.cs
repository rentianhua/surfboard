using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseCity
    {
        /// <summary>
        /// id
        /// </summary>
        public int Innerid { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        public string Initial { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseCounty
    {
        /// <summary>
        /// id
        /// </summary>
        public int Innerid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 区县名称
        /// </summary>
        public string Countyname { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int Cityid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseProvinceAll
    {
        /// <summary>
        /// id
        /// </summary>
        public int provid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string provname { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        public string initial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BaseCityAll> citylist { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseCityAll
    {
        /// <summary>
        /// id
        /// </summary>
        public int cityid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string cityname { get; set; }
        /// <summary>
        /// 首字母
        /// </summary>
        public string initial { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public int provid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BaseCountyAll> countylist { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseCountyAll
    {
        /// <summary>
        /// id
        /// </summary>
        public int countyid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string countyname { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int cityid { get; set; }
    }
}

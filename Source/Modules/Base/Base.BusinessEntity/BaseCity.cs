﻿using System;
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
}

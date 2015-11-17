using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseCarBrandModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int? Innerid { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        public string Initial { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 品牌图标
        /// </summary>
        public string Logurl { get; set; }

        /// <summary>
        /// 热度
        /// </summary>
        public int? Hot { get; set; }
        /// <summary>
        /// ID最大值
        /// </summary>
        public int? MaxId { get; set; }
    }
    /// <summary>
    /// 品牌列表显示字段
    /// </summary>
    public class BaseCarBrandListViewModel
    {

        /// <summary>
        /// id
        /// </summary>
        public int? Innerid { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        public string Initial { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 品牌图标
        /// </summary>
        public string Logurl { get; set; }

        /// <summary>
        /// 热度
        /// </summary>
        public int? Hot { get; set; }
    }
    /// <summary>
    /// 品牌查询条件
    /// </summary>
    public class BaseCarBrandQueryModel : QueryModel
    {
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }
    }
}

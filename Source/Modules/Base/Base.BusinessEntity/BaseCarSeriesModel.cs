using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 车系
    /// </summary>
    public class BaseCarSeriesModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int? Innerid { get; set; }

        /// <summary>
        /// 车系名称
        /// </summary>
        public string SeriesName { get; set; }

        /// <summary>
        /// 车系组 进口/国产
        /// </summary>
        public string SeriesGroupName { get; set; }

        /// <summary>
        /// 品牌id
        /// </summary>
        public int? Brandid { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// ID最大值
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int? MaxId { get; set; }
    }
    /// <summary>
    /// 车系列表显示字段
    /// </summary>
    public class BaseCarSeriesListViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int? Innerid { get; set; }

        /// <summary>
        /// 车系名称
        /// </summary>
        public string SeriesName { get; set; }

        /// <summary>
        /// 车系组 进口/国产
        /// </summary>
        public string SeriesGroupName { get; set; }

        /// <summary>
        /// 品牌id
        /// </summary>
        public int? Brandid { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
    /// <summary>
    /// 查询条件
    /// </summary>
    public class BaseCarSeriesQueryModel : QueryModel
    {
        /// <summary>
        /// 车系名称
        /// </summary>
        public string SeriesName { get; set; }
    }
}

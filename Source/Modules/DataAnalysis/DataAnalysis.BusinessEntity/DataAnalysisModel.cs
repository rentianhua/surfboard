using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.DataAnalysis.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class DataAnalysisModel
    {
        /// <summary>
        /// key
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// value
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// value2
        /// </summary>
        public string value2 { get; set; }

        /// <summary>
        /// value3
        /// </summary>
        public List<DataAnalysisModel> value3 { get; set; }

        /// <summary>
        /// value4
        /// </summary>
        public decimal value4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<dynamic> value5 { get; set; }

        /// <summary>
        /// value6
        /// </summary>
        public decimal value6 { get; set; }
    }

    /// <summary>
    /// 数据分析查询实体
    /// </summary>
    public class DataQueryModel
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime starttime{get;set;}

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endtime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 车辆估值
    /// </summary>
    public class CarEvaluateModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 车型ID
        /// </summary>
        public int? model_id { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public int? cityid { get; set; }

        /// <summary>
        /// 上牌时间
        /// </summary>
        public DateTime register_date { get; set; }

        /// <summary>
        /// 里程
        /// </summary>
        public decimal mileage { get; set; }

        /// <summary>
        /// 估价
        /// </summary>
        public decimal estimateprice {get;set;}

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdtime { get; set; }

        /// <summary>
        /// 指导价
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string openid { get; set; }

    }
}

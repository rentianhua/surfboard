using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class CarMysteriousListModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 供应商id
        /// </summary>
        public string supplierid { get; set; }

        /// <summary>
        /// 车辆编号
        /// </summary>
        public string carno { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string pic_url { get; set; }
        
        /// <summary>
        /// 待售价格（万元）
        /// </summary>
        public decimal? price { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public string mileage { get; set; }

        /// <summary>
        /// 车辆上牌日期
        /// </summary>
        public DateTime? register_date { get; set; }

        /// <summary>
        /// 状态状态[1.在售，2.已售，0.已删除]
        /// </summary>
        public int? status { get; set; }

        #region code name

        /// <summary>
        /// 省份
        /// </summary>
        public string provname { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string cityname { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string brand_name { get; set; }

        /// <summary>
        /// 车系
        /// </summary>
        public string series_name { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string model_name { get; set; }
        
        #endregion
    }

    /// <summary>
    /// 查看车源下单model
    /// </summary>
    public class PayRecordModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; } 

        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string Orderno { get; set; }

        /// <summary>
        /// 是否支付
        /// </summary>
        public short? Ispay { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }
    }

    /// <summary>
    /// 查看车源下单model
    /// </summary>
    public class PayModel
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public string custid { get; set; }

        /// <summary>
        /// 车辆id
        /// </summary>
        public string carid { get; set; }

        /// <summary>
        /// 交易类型JSAPI，NATIVE，APP
        /// </summary>
        public string tradeType { get; set; } = "NATIVE";
    }
}

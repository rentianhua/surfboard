using System;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 劲爆车源信息
    /// </summary>
    public class CarMaddenModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 神秘车源
        /// </summary>
        public string supplierid { get; set; }

        /// <summary>
        /// 车辆编号
        /// </summary>
        public string carno { get; set; }

        /// <summary>
        /// 车辆标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 车源图片集合
        /// </summary>
        public string pictures { get; set; }

        /// <summary>
        /// 省份id
        /// </summary>
        public int? provid { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int? cityid { get; set; }

        /// <summary>
        /// 品牌id
        /// </summary>
        public int? brand_id { get; set; }

        /// <summary>
        /// 车系id
        /// </summary>
        public int? series_id { get; set; }

        /// <summary>
        /// 车型id
        /// </summary>
        public int? model_id { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int? colorid { get; set; }

        /// <summary>
        /// 购车方案(如闪电落地、全款裸车)
        /// </summary>
        public string programme { get; set; }

        /// <summary>
        /// 方案说明
        /// </summary>
        public string programmedesc { get; set; }

        /// <summary>
        /// 厂方指导价(万元)
        /// </summary>
        public decimal? guideprice { get; set; }

        /// <summary>
        /// 现售价（万元）
        /// </summary>
        public decimal? price { get; set; }

        /// <summary>
        /// 成交价格（万元）
        /// </summary>
        public decimal? dealprice { get; set; }

        /// <summary>
        /// 结案时间
        /// </summary>
        public DateTime? dealtime { get; set; }

        /// <summary>
        /// 结案说明
        /// </summary>
        public string dealdesc { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? deletedtime { get; set; }

        /// <summary>
        /// 删除备注
        /// </summary>
        public string deletedesc { get; set; }
        
        /// <summary>
        /// 状态状态[1.在售，2.已售]
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 估价
        /// </summary>
        public string estimateprice { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public short? isdeleted { get; set; }

        /// <summary>
        /// createdtime
        /// </summary>
        public DateTime? createdtime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? modifiedtime { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public long? refreshtime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        public string operatedid { get; set; }
    }

    /// <summary>
    /// 劲爆车源详情model
    /// </summary>
    public class CarMaddenViewModel : CarMaddenModel
    {
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

        /// <summary>
        /// 变速箱类型
        /// </summary>
        public string geartype { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        public string liter { get; set; }

        /// <summary>
        /// 排放标准
        /// </summary>
        public string dischargeName { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class CarMaddenListModel
    {

        /// <summary>
        /// 主键id
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 神秘车源
        /// </summary>
        public string supplierid { get; set; }

        /// <summary>
        /// 车辆编号
        /// </summary>
        public string carno { get; set; }
        
        /// <summary>
        /// 车源图片集合
        /// </summary>
        public string pictures { get; set; }
        
        /// <summary>
        /// 购车方案(如闪电落地、全款裸车)
        /// </summary>
        public string programme { get; set; }

        /// <summary>
        /// 厂方指导价(万元)
        /// </summary>
        public decimal? guideprice { get; set; }

        /// <summary>
        /// 现售价（万元）
        /// </summary>
        public decimal? price { get; set; }
        
        /// <summary>
        /// 状态状态[1.在售，2.已售]
        /// </summary>
        public int? status { get; set; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        public short? isdeleted { get; set; }

        /// <summary>
        /// createdtime
        /// </summary>
        public DateTime? createdtime { get; set; }
        
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

        /// <summary>
        /// 变速箱类型
        /// </summary>
        public string geartype { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        public string liter { get; set; }

        /// <summary>
        /// 排放标准
        /// </summary>
        public string dischargeName { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class CarMaddenQueryModel : QueryModel
    {
        /// <summary>
        /// 省份id
        /// </summary>
        public int? provid { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int? cityid { get; set; }

        /// <summary>
        /// 品牌id
        /// </summary>
        public int? brand_id { get; set; }

        /// <summary>
        /// 车系id
        /// </summary>
        public int? series_id { get; set; }

        /// <summary>
        /// 车型id
        /// </summary>
        public int? model_id { get; set; }
        
        /// <summary>
        /// 状态[1.在售，2.已售]
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int? isdeleted { get; set; }
    }
}

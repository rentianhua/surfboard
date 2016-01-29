using System;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Auction.BusinessEntity
{
    /// <summary>
    /// 拍卖车辆基本信息表
    /// </summary>
    public class AuctionCarInfoModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }
        
        /// <summary>
        /// 会员ID
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 车辆标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string pic_url { get; set; }

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
        /// 行驶里程（万元）
        /// </summary>
        public decimal? mileage { get; set; }

        /// <summary>
        /// 车辆上牌日期
        /// </summary>
        public DateTime? register_date { get; set; }
        
        /// <summary>
        /// 收购时间
        /// </summary>
        public DateTime? buytime { get; set; }

        /// <summary>
        /// 收购价格(万元)
        /// </summary>
        public decimal? buyprice { get; set; }

        /// <summary>
        /// 最低价（万元）
        /// </summary>
        public decimal? lowestprice { get; set; }
        
        /// <summary>
        /// 重大事故/水浸/火烧
        /// </summary>
        public short? isproblem { get; set; }

        /// <summary>
        /// 是否定期保养
        /// </summary>
        public short? istain { get; set; }
        
        /// <summary>
        /// 是否含过户费
        /// </summary>
        public short? istransferfee { get; set; }

        /// <summary>
        /// 年检到期时间
        /// </summary>
        public DateTime? ckyear_date { get; set; }

        /// <summary>
        /// 交强险到期时间
        /// </summary>
        public DateTime? tlci_date { get; set; }

        /// <summary>
        /// 商业险到期时间
        /// </summary>
        public DateTime? audit_date { get; set; }

        /// <summary>
        /// 车况备注/优势
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 转让原因
        /// </summary>
        public string sellreason { get; set; }

        /// <summary>
        /// 原车主信息
        /// </summary>
        public string masterdesc { get; set; }

        /// <summary>
        /// 车源估值情况
        /// </summary>
        public string estimateprice { get; set; }
        /// <summary>
        /// 成交奖励
        /// </summary>
        public string dealrewards { get; set; }
        /// <summary>
        /// 过户风险与责任
        /// </summary>
        public string transferrisk { get; set; }
        /// <summary>
        /// 购买提醒
        /// </summary>
        public string remind { get; set; }
        /// <summary>
        /// 重要提示
        /// </summary>
        public string tips { get; set; }
        /// <summary>
        /// 状态[0.已删除,1.待拍,2.在拍,3.成交,4.流拍]
        /// </summary>
        public int? status { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        public string createrid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdtime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string modifierid { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? modifiedtime { get; set; }

        /// <summary>
        /// 删除操作人
        /// </summary>
        public string deleterid { get; set; }

        /// <summary>
        /// 删除时间[status:0]
        /// </summary>
        public DateTime? deletedtime { get; set; }

        /// <summary>
        /// 删除操作人
        /// </summary>
        public string deletedesc { get; set; }
        
        /// <summary>
        /// 发拍操作人
        /// </summary>
        public string publisherid { get; set; }

        /// <summary>
        /// 发拍时间[status:2]
        /// </summary>
        public DateTime? publishedtime { get; set; }
        
        /// <summary>
        /// 成交操作人
        /// </summary>
        public string dealerid { get; set; }

        /// <summary>
        /// 成交时间[status:3]
        /// </summary>
        public DateTime? dealedtime { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        public decimal? dealedprice { get; set; }

        /// <summary>
        /// 成交说明
        /// </summary>
        public string dealdesc { get; set; }

        /// <summary>
        /// 成交人手机号
        /// </summary>
        public string dealmobile { get; set; }
        
        /// <summary>
        /// 拍卖有效期[过后status:4]
        /// </summary>
        public DateTime? validtime { get; set; }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuctionCarInfoViewModel : AuctionCarInfoModel
    {
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
    public class AuctionCarInfoQueryModel : QueryModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string mobile { get; set; }

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
        /// 状态
        /// </summary>
        public int? status { get; set; }

    }
}

using System;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Auction.BusinessEntity
{
    /// <summary>
    /// 拍卖参与者
    /// </summary>
    public class AuctionCarParticipantModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 拍卖车辆id
        /// </summary>
        public string Auctionid { get; set; }

        /// <summary>
        /// 竞拍人
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 竞拍人ID
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 报价
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        public string Createrid { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifierid { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 竞价修改记录
        /// </summary>
        public string recordlist { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string orderno { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public AuctionCarParticipantModel()
        {
            Createdtime = DateTime.Now;
        }
    }
    
    /// <summary>
    /// 拍卖参与者view
    /// </summary>
    public class AuctionCarParticipantViewModel: AuctionCarParticipantModel
    {
        /// <summary>
        /// 拍品编号
        /// </summary>
        public string auctionno { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string model_name { get; set; }

        /// <summary>
        /// 出价
        /// </summary>
        public decimal? lowestprice { get; set; }

        /// <summary>
        /// 上牌时间
        /// </summary>
        public DateTime? register_date { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public decimal mileage { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string cityname { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        public string provname { get; set; }

        /// <summary>
        /// 新车价
        /// </summary>
        public decimal? price { get; set; }

        /// <summary>
        /// 出价次数
        /// </summary>
        public int? pricecount { get; set; }
    }

    /// <summary>
    /// 拍卖参与者query
    /// </summary>
    public class AuctionCarParticipantQueryModel : QueryModel
    {
        /// <summary>
        /// 拍卖车辆id
        /// </summary>
        public string Auctionid { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 出价人ID
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 报价
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 拍品编号
        /// </summary>
        public string auctionno { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string userno { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 业务员ID
        /// </summary>
        public string operatedid { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public decimal? minmileage { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public decimal? maxmileage { get; set; }

        /// <summary>
        /// 车辆上牌日期
        /// </summary>
        public DateTime? register_date { get; set; }

        /// <summary>
        /// 城市id(所在地)
        /// </summary>
        public int? cityid { get; set; }
    }
}

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
        public int status { get; set; }

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
    }
}

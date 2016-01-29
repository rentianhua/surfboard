using System;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Auction.BusinessEntity
{
    /// <summary>
    /// 押金model
    /// </summary>
    public class AuctionDepositModel
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
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 支付联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 类型 1收，2退
        /// </summary>
        public short? Type { get; set; }

        /// <summary>
        /// 买卖方,1卖家,2买家
        /// </summary>
        public short? Payer { get; set; }

        /// <summary>
        /// 押金金额
        /// </summary>
        public decimal? Dpsamount { get; set; }

        /// <summary>
        /// 银行电子回单号
        /// </summary>
        public string Dpsserialnum { get; set; }

        /// <summary>
        /// 流入银行账户号
        /// </summary>
        public string Dpsaccount { get; set; }

        /// <summary>
        /// 截图
        /// </summary>
        public string Pictures { get; set; }

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
        public AuctionDepositModel()
        {
            Createdtime = DateTime.Now;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class AuctionDepositViewModel : AuctionDepositModel
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class AuctionDepositQueryModel : QueryModel
    {
        /// <summary>
        /// 类型 1收，2退
        /// </summary>
        public short? Type { get; set; }

        /// <summary>
        /// 买卖方,1卖家,2买家
        /// </summary>
        public short? Payer { get; set; }
    }
}

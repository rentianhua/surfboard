using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Rewards.BusinessEntity
{
    /// <summary>
    /// 结算记录
    /// </summary>
    public class SettlementLogModel
    {
        /// <summary>
        /// 内部id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 商户id
        /// </summary>
        public string Shopid { get; set; }

        /// <summary>
        /// 结算单号
        /// </summary>
        public string Orderid { get; set; }

        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime? SettTime { get; set; }

        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal? SettTotal { get; set; }

        /// <summary>
        /// 对应核销周期开始时间
        /// </summary>
        public DateTime? SettCycleStart { get; set; }

        /// <summary>
        /// 对应核销周期结束时间
        /// </summary>
        public DateTime? SettCycleEnd { get; set; }

        /// <summary>
        /// 银行电子回单号
        /// </summary>
        public string SettSerialNum { get; set; }

        /// <summary>
        /// 流入银行账户号
        /// </summary>
        public string SettAccount { get; set; }
        
        /// <summary>
        /// 截图
        /// </summary>
        public string Pictures { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SettlementLogViewModel : SettlementLogModel
    {
        /// <summary>
        /// 截图
        /// </summary>
        public string Shopname { get; set; }
    }

    /// <summary>
    /// 结算记录query
    /// </summary>
    public class SettlementLogQueryModel : QueryModel
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string Shopid { get; set; }

        /// <summary>
        /// 结算单号 or 银行电子回单号
        /// </summary>
        public string OrderidOrNumber { get; set; }

        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal? SettTotal { get; set; }

        /// <summary>
        /// 结算时间-开始时间
        /// </summary>
        public DateTime? SettTimeStart { get; set; }

        /// <summary>
        /// 结算时间-结束时间
        /// </summary>
        public DateTime? SettTimeEnd { get; set; }

        /// <summary>
        /// 流入银行账户号
        /// </summary>
        public string SettAccount { get; set; }
        
    }

    /// <summary>
    /// 已核销的code查询条件
    /// </summary>
    public class UsedCodeQueryModel : QueryModel
    {
        /// <summary>
        /// 卡券id
        /// </summary>
        public string Cardid { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 核销开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 核销结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 已核销的code查询条件
    /// </summary>
    public class SettedCodeQueryModel : QueryModel
    {
        /// <summary>
        /// 结算记录id
        /// </summary>
        public string Settid { get; set; }
    }

    /// <summary>
    /// 已核销的code查询条件
    /// </summary>
    public class SettedCodeViewListModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Usedtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Custname { get; set; }
    }
}

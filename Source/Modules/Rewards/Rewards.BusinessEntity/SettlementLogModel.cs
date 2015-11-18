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
        /// 银行交易流水号
        /// </summary>
        public string SettSerialNum { get; set; }

        /// <summary>
        /// 流入银行账户号
        /// </summary>
        public string SettAccount { get; set; }
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
        /// 结算单号
        /// </summary>
        public string Orderid { get; set; }

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
        /// 银行交易流水号
        /// </summary>
        public string SettSerialNum { get; set; }

        /// <summary>
        /// 流入银行账户号
        /// </summary>
        public string SettAccount { get; set; }
    }
}

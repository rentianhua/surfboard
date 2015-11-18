using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Rewards.BusinessEntity
{
    /// <summary>
    /// code model
    /// </summary>
    public class CodeModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 礼券id
        /// </summary>
        public string Cardid { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// custid
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 获得时间
        /// </summary>
        public DateTime? Gettime { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? Usedtime { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        public int Sourceid { get; set; }

        /// <summary>
        /// 一/二维码的图片
        /// </summary>
        public string Qrcode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CardCancelSummaryModel
    {
        /// <summary>
        /// 礼券id
        /// </summary>
        public string Cardid { get; set; }
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 子标题
        /// </summary>
        public string Titlesub { get; set; }

        /// <summary>
        /// 面额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 购买价
        /// </summary>
        public decimal BuyPrice { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Logourl { get; set; }
        
        /// <summary>
        /// 起始库存
        /// </summary>
        public int? Maxcount { get; set; }

        /// <summary>
        /// 当前库存
        /// </summary>
        public int? Count { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 已核销数
        /// </summary>
        public int CanedCount { get; set; }

        /// <summary>
        /// 已核销数
        /// </summary>
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// 核销查询条件model
    /// </summary>
    public class CardCancelSummaryQueryModel
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string Shopid { get; set; }

        /// <summary>
        /// 周期开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 周期结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}

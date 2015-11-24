using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs.MerChant;

namespace CCN.Modules.Rewards.BusinessEntity
{
    /// <summary>
    /// 购买礼券model
    /// </summary>
    public class CouponBuyModel
    {
        ///// <summary>
        ///// 商品id
        ///// </summary>
        //public string ProductId { get; set; }

        ///// <summary>
        ///// 订单id
        ///// </summary>
        //public string OrderId { get; set; }

        ///// <summary>
        ///// 购买数量
        ///// </summary>
        //public int Number { get; set; }

        ///// <summary>
        ///// openid
        ///// </summary>
        //public string Openid { get; set; }

        ///// <summary>
        ///// 公众号原始id
        ///// </summary>
        //public string Accountid { get; set; }

        ///// <summary>
        ///// 初始化
        ///// </summary>
        //public CouponBuyModel()
        //{
        //    Number = 1;
        //}

        /// <summary>
        /// 订单信息
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int result { get; set; }
    }


    /// <summary>
    /// 发放礼券model
    /// </summary>
    public class CouponSendModel
    {
        /// <summary>
        /// Cardid
        /// </summary>
        public string Cardid { get; set; }

        /// <summary>
        /// Custid
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 发放数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 礼券来源
        /// </summary>
        public int Sourceid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CouponCodeModel> ListCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CouponCodeModel
    {
        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QrCode { get; set; }
    }
}

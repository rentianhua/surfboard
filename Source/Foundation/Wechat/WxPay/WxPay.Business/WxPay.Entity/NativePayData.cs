using System;
using Cedar.Foundation.WeChat.WxPay.Lib;
using Newtonsoft.Json;

namespace Cedar.Foundation.WeChat.WxPay.Business.WxPay.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class NativePayData
    {
        /// <summary>
        /// 商品描述
        /// </summary>
        [JsonProperty("body")]
        public string Body { set; get; }

        /// <summary>
        /// 附加数据
        /// </summary>
        [JsonProperty("attach")]
        public string Attach { set; get; }

        /// <summary>
        /// 随机字符串 （订单号）
        /// </summary>
        [JsonProperty("out_trade_no")]
        public string OutTradeNo { set; get; }

        /// <summary>
        /// 支付总金额
        /// </summary>
        [JsonProperty("total_fee")]
        public int TotalFee { set; get; }

        /// <summary>
        /// 交易起始时间
        /// </summary>
        [JsonProperty("time_start")]
        public string TimeStart { set; get; }

        /// <summary>
        /// 交易结束时间
        /// </summary>
        [JsonProperty("time_expire")]
        public string TimeExpire { set; get; }

        /// <summary>
        /// 商品标记
        /// </summary>
        [JsonProperty("goods_tag")]
        public string GoodsTag { set; get; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [JsonProperty("product_id")]
        public string ProductId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public NativePayData ()
        {
            OutTradeNo = WxPayApi.GenerateOutTradeNo();
            TimeStart = DateTime.Now.ToString("yyyyMMddHHmmss");
            TimeExpire = DateTime.Now.AddMinutes(5).ToString("yyyyMMddHHmmss");
        }
    }
}

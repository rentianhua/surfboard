﻿using System.Collections.Generic;
using Cedar.Foundation.WeChat.WxPay.Business.WxPay.Entity;
using Cedar.Foundation.WeChat.WxPay.Lib;
using Cedar.Framework.Common.BaseClasses;

namespace Cedar.Foundation.WeChat.WxPay.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class NativePay
    {
        /// <summary>
        /// 
        /// </summary>
        public NativePay()
        {
            WxPayConfig.Init();
        }

        /**
        * 生成扫描支付模式一URL
        * @param productId 商品ID
        * @return 模式一URL
        */
        public string GetPrePayUrl(string productId)
        {
            //Log.Info(this.GetType().ToString(), "Native pay mode 1 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("appid", WxPayConfig.APPID);//公众帐号id
            data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            data.SetValue("time_stamp", WxPayApi.GenerateTimeStamp());//时间戳
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("sign", data.MakeSign());//签名
            string str = ToUrlParams(data.GetValues());//转换为URL串
            string url = "weixin://wxpay/bizpayurl?" + str;

            //Log.Info(this.GetType().ToString(), "Get native pay mode 1 url : " + url);
            return url;
        }

        /// <summary>
        /// 生成直接支付url，支付url有效期为2小时,模式二
        /// </summary>
        /// <param name="payData"></param>
        /// <returns></returns>
        public JResult GetPayUrl(NativePayData payData)
        {
            var data = new WxPayData();
            data.SetValue("body", payData.Body);//商品描述
            data.SetValue("attach", payData.Attach);//附加数据
            data.SetValue("out_trade_no", payData.OutTradeNo);//随机字符串
            data.SetValue("total_fee", payData.TotalFee);//总金额
            data.SetValue("time_start", payData.TimeStart);//交易起始时间
            data.SetValue("time_expire", payData.TimeExpire);//交易结束时间
            data.SetValue("goods_tag", payData.GoodsTag);//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", payData.ProductId);//商品ID

            var result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            if (result.IsSet("return_code") 
                && result.IsSet("result_code") 
                && result.GetValue("return_code").ToString().Equals("SUCCESS") 
                && result.GetValue("result_code").ToString().Equals("SUCCESS"))
            {
                //获得统一下单接口返回的二维码链接
                return JResult._jResult(0, result.GetValue("code_url").ToString());
            }
            
            return JResult._jResult(400, result.ToJson());
        }

        /**
        * 参数数组转换为url格式
        * @param map 参数名与参数值的映射表
        * @return URL字符串
        */
        private string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }
    }
}
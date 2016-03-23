using System;
using Cedar.Foundation.WeChat.WxPay.Business.WxPay.Entity;
using Cedar.Foundation.WeChat.WxPay.Lib;
using Cedar.Framework.Common.BaseClasses;
using Newtonsoft.Json;

namespace Cedar.Foundation.WeChat.WxPay.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class WxPayAPIs
    {
        /// <summary>
        /// 
        /// </summary>
        public WxPayAPIs()
        {
            WxPayConfig.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JResult GetNativePayQrCode(NativePayData data2)
        {
            var nativePay = new NativePay();

            var data = new NativePayData
            {
                Body = "快拍立信拍车定金",//商品描述
                Attach = "testAttach",//附加数据
                TotalFee = 1,//总金额
                ProductId = "prodid",//商品ID
                OutTradeNo = WxPayApi.GenerateOutTradeNo(),//订单编号
                GoodsTag = "testgood"
            };



            var result = nativePay.GetPayUrl(data);

            if (result.errcode != 0)
            {
                return result;
            }

            //生成二维码
            var bitmap = BarCodeUtility.CreateBarcode(result.errmsg.ToString(), 240, 240);
            
            var ran = new Random();
            var key = string.Concat("wxpay", DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
            var stream = BarCodeUtility.BitmapToStream(bitmap);
            //上传图片到七牛云
            var qinniu = new QiniuUtility();
            var qrcode = qinniu.Put(stream, "", key);
            return JResult._jResult(0, qrcode);
        }
    }
}

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
        public static string GetNativePayQrCode(NativePayData data)
        {
            NativePay nativePay = new NativePay();
            string url2 = nativePay.GetPayUrl(data);
            
            //生成二维码
            var bitmap = BarCodeUtility.CreateBarcode(url2, 240, 240);
            
            var ran = new Random();
            var key = string.Concat("wxpay", DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));            
            var stream = BarCodeUtility.BitmapToStream(bitmap);
            //上传图片到七牛云
            var qinniu = new QiniuUtility();
            var qrcode = qinniu.Put(stream, "", key);
            return qrcode;
        }
    }
}

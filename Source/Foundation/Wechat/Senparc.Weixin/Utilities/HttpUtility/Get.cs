﻿/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
    
    文件名：Get.cs
    文件功能描述：Get
    
    
    创建标识：Senparc - 20150211
    
    修改标识：Senparc - 20150303
    修改描述：整理接口
----------------------------------------------------------------*/

using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Exceptions;

namespace Senparc.Weixin.HttpUtility
{
    public static class Get
    {
        #region 同步方法

        public static T GetJson<T>(string url, Encoding encoding = null)
        {
            var returnText = RequestUtility.HttpGet(url, encoding);

            var js = new JavaScriptSerializer();

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
                var errorResult = js.Deserialize<WxJsonResult>(returnText);
                if (errorResult.errcode != ReturnCode.请求成功)
                {
                    //发生错误
                    throw new ErrorJsonResultException(
                        string.Format("微信请求发生错误！错误代码：{0}，说明：{1}",
                            (int) errorResult.errcode,
                            errorResult.errmsg),
                        null, errorResult);
                }
            }

            var result = js.Deserialize<T>(returnText);

            return result;
        }

        public static void Download(string url, Stream stream)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);  

            var wc = new WebClient();
            var data = wc.DownloadData(url);
            foreach (var b in data)
            {
                stream.WriteByte(b);
            }
        }

        public static byte[] Download(string url)
        {
            var wc = new WebClient();
            var data = wc.DownloadData(url);
            return data;
        }

        #endregion

        #region 异步方法

        public static async Task<T> GetJsonAsync<T>(string url, Encoding encoding = null)
        {
            var returnText = await RequestUtility.HttpGetAsync(url, encoding);

            var js = new JavaScriptSerializer();

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
                var errorResult = js.Deserialize<WxJsonResult>(returnText);
                if (errorResult.errcode != ReturnCode.请求成功)
                {
                    //发生错误
                    throw new ErrorJsonResultException(
                        string.Format("微信请求发生错误！错误代码：{0}，说明：{1}",
                            (int) errorResult.errcode,
                            errorResult.errmsg),
                        null, errorResult);
                }
            }

            var result = js.Deserialize<T>(returnText);

            return result;
        }

        public static async Task DownloadAsync(string url, Stream stream)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);  

            var wc = new WebClient();
            var data = await wc.DownloadDataTaskAsync(url);
            await stream.WriteAsync(data, 0, data.Length);
            //foreach (var b in data)
            //{
            //    stream.WriteAsync(b);
            //}
        }

        #endregion
    }
}
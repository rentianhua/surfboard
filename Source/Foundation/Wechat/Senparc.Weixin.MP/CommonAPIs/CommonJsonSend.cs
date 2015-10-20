﻿/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
    
    文件名：CommonJsonSend.cs
    文件功能描述：向需要AccessToken的API发送消息的公共方法
    
    
    创建标识：Senparc - 20150211
    
    修改标识：Senparc - 20150303
    修改描述：整理接口
 
    修改标识：Senparc - 20150312
    修改描述：开放代理请求超时时间
----------------------------------------------------------------*/

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.HttpUtility;

namespace Senparc.Weixin.MP.CommonAPIs
{
    public enum CommonJsonSendType
    {
        GET,
        POST
    }

    public static class CommonJsonSend
    {
        #region 同步请求

        /// <summary>
        ///     向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static WxJsonResult Send(string accessToken, string urlFormat, object data,
            CommonJsonSendType sendType = CommonJsonSendType.POST, int timeOut = Config.TIME_OUT)
        {
            return Send<WxJsonResult>(accessToken, urlFormat, data, sendType, timeOut);
        }

        /// <summary>
        ///     向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null。在POST方式中将被转为JSON字符串提交</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="checkValidationResult">验证服务器证书回调自动验证</param>
        /// <param name="jsonSetting">JSON字符串生成设置</param>
        /// <returns></returns>
        public static T Send<T>(string accessToken, string urlFormat, object data,
            CommonJsonSendType sendType = CommonJsonSendType.POST, int timeOut = Config.TIME_OUT,
            bool checkValidationResult = false,
            JsonSetting jsonSetting = null
            )
        {
            var url = string.IsNullOrEmpty(accessToken) ? urlFormat : string.Format(urlFormat, accessToken);
            switch (sendType)
            {
                case CommonJsonSendType.GET:
                    return Get.GetJson<T>(url);
                case CommonJsonSendType.POST:
                    var serializerHelper = new SerializerHelper();
                    var jsonString = serializerHelper.GetJsonString(data, jsonSetting);
                    using (var ms = new MemoryStream())
                    {
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);

                        return Post.PostGetJson<T>(url, null, ms, timeOut: timeOut,
                            checkValidationResult: checkValidationResult);
                    }
                default:
                    throw new ArgumentOutOfRangeException("sendType");
            }
        }

        #endregion

        #region 异步请求

        /// <summary>
        ///     向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendAsync(string accessToken, string urlFormat, object data,
            CommonJsonSendType sendType = CommonJsonSendType.POST, int timeOut = Config.TIME_OUT)
        {
            return await SendAsync<WxJsonResult>(accessToken, urlFormat, data, sendType, timeOut);
        }

        /// <summary>
        ///     向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null。在POST方式中将被转为JSON字符串提交</param>
        /// <param name="sendType">发送类型，POST或GET，默认为POST</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="checkValidationResult">验证服务器证书回调自动验证</param>
        /// <param name="jsonSetting">JSON字符串生成设置</param>
        /// <returns></returns>
        public static async Task<T> SendAsync<T>(string accessToken, string urlFormat, object data,
            CommonJsonSendType sendType = CommonJsonSendType.POST, int timeOut = Config.TIME_OUT,
            bool checkValidationResult = false,
            JsonSetting jsonSetting = null
            )
        {
            var url = string.IsNullOrEmpty(accessToken) ? urlFormat : string.Format(urlFormat, accessToken);

            switch (sendType)
            {
                case CommonJsonSendType.GET:
                    return await Get.GetJsonAsync<T>(url);
                case CommonJsonSendType.POST:
                    var serializerHelper = new SerializerHelper();
                    var jsonString = serializerHelper.GetJsonString(data, jsonSetting);
                    using (var ms = new MemoryStream())
                    {
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        await ms.WriteAsync(bytes, 0, bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);

                        return
                            await
                                Post.PostGetJsonAsync<T>(url, null, ms, timeOut: timeOut,
                                    checkValidationResult: checkValidationResult);
                    }
                default:
                    throw new ArgumentOutOfRangeException("sendType");
            }
        }

        #endregion
    }
}
﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    /// </summary>
    public class DynamicWebService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="isjson"></param>
        /// <returns></returns>
        public static string ExeApiMethod(string url, string type, string data, bool isjson = true)
        {
            var handler = new WebRequestHandler
            {
                AllowAutoRedirect = false,
                UseProxy = false
            };

            var json = isjson ? (data != null ? JsonConvert.DeserializeObject(data) : (string) null) : data;

            var client = new HttpClient(handler);
            var website = ConfigurationManager.AppSettings["localapi"];
            client.BaseAddress = new Uri(website);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response;
                switch (type)
                {
                    case "get":
                        response = client.GetAsync(url).Result;
                        break;
                    case "post":
                        response = client.PostAsJsonAsync(url, json).Result;
                        break;
                    case "put":
                        response = client.PutAsJsonAsync(url, json).Result;
                        break;
                    case "delete":
                        response = client.DeleteAsync(url).Result;
                        break;
                    default:
                        response = client.GetAsync(url).Result;
                        break;
                }
                return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : null;
            }
            catch (AggregateException)
            {
                return "";
            }
        }

        /// <summary>
        ///     Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        public static string SendPost(string url, IDictionary<string, string> parameters, string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebResponse rsp = null;
                Stream reqStream = null;
                try
                {
                    var req = (HttpWebRequest) WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    var postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse) req.GetResponse();
                    var encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    reqStream?.Close();
                    rsp?.Close();
                }
            }

            //创建请求
            var request = (HttpWebRequest) WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

            //GET请求
            request.Method = "GET";
            request.ReadWriteTimeout = 5000;
            request.ContentType = "text/html;charset=UTF-8";
            var response = (HttpWebResponse) request.GetResponse();
            var myResponseStream = response.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

            //返回内容
            var retString = myStreamReader.ReadToEnd();
            return retString;
        }

        /// <summary>
        ///     Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        public static string SendJson(string url, IDictionary<string, string> parameters, string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebResponse rsp = null;
                Stream reqStream = null;
                try
                {
                    var req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    var postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    var encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    reqStream?.Close();
                    rsp?.Close();
                }
            }

            //创建请求
            var request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

            //GET请求
            request.Method = "GET";
            request.ReadWriteTimeout = 5000;
            request.ContentType = "text/html;charset=UTF-8";
            var response = (HttpWebResponse)request.GetResponse();
            var myResponseStream = response.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

            //返回内容
            var retString = myStreamReader.ReadToEnd();
            return retString;
        }

        /// <summary>
        ///     组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <param name="encode"></param>
        /// <returns>URL编码后的请求数据</returns>
        private static string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            var postData = new StringBuilder();
            var hasParam = false;
            var dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                var name = dem.Current.Key;
                var value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name)) //&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        ///     把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                reader?.Close();
                stream?.Close();
                rsp?.Close();
            }
        }
    }
}
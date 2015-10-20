/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
 
    文件名：RequestHandler.cs
    文件功能描述：微信支付 请求处理
    
    
    创建标识：Senparc - 20150722
----------------------------------------------------------------*/

using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Senparc.Weixin.QY.Helpers;

namespace Senparc.Weixin.QY.TenPayLib
{
    /**
    '签名工具类
     ============================================================================/// <summary>
    'api说明：
    'Init();
    '初始化函数，默认给一些参数赋值。
    'SetKey(key_)'设置商户密钥
    'CreateMd5Sign(signParams);字典生成Md5签名
    'GenPackage(packageParams);获取package包
    'CreateSHA1Sign(signParams);创建签名SHA1
    'ParseXML();输出xml
    'GetDebugInfo(),获取debug信息
     * 
     * ============================================================================
     */

    public class RequestHandler
    {
        /// <summary>
        ///     debug信息
        /// </summary>
        private string DebugInfo;

        protected HttpContext HttpContext;

        /// <summary>
        ///     密钥
        /// </summary>
        private string Key;

        /// <summary>
        ///     请求的参数
        /// </summary>
        protected Hashtable Parameters;

        public RequestHandler(HttpContext httpContext)
        {
            Parameters = new Hashtable();

            HttpContext = httpContext ?? HttpContext.Current;
        }

        /// <summary>
        ///     初始化函数
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        ///     获取debug信息
        /// </summary>
        /// <returns></returns>
        public string GetDebugInfo()
        {
            return DebugInfo;
        }

        /// <summary>
        ///     获取密钥
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            return Key;
        }

        /// <summary>
        ///     设置密钥
        /// </summary>
        /// <param name="key"></param>
        public void SetKey(string key)
        {
            Key = key;
        }

        /// <summary>
        ///     设置参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        public void SetParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (Parameters.Contains(parameter))
                {
                    Parameters.Remove(parameter);
                }

                Parameters.Add(parameter, parameterValue);
            }
        }


        /// <summary>
        ///     创建md5摘要,规则是:按参数名称a-z排序,遇到空值的参数不参加签名
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        /// key和value通常用于填充最后一组参数
        /// <returns></returns>
        public virtual string CreateMd5Sign(string key, string value)
        {
            var sb = new StringBuilder();

            var akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                var v = (string) Parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append(key + "=" + value);
            var sign = MD5UtilHelper.GetMD5(sb.ToString(), GetCharset()).ToUpper();

            return sign;
        }

        /// <summary>
        ///     输出XML
        /// </summary>
        /// <returns></returns>
        public string ParseXML()
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (string k in Parameters.Keys)
            {
                var v = (string) Parameters[k];
                if (Regex.IsMatch(v, @"^[0-9.]$"))
                {
                    sb.Append("<" + k + ">" + v + "</" + k + ">");
                }
                else
                {
                    sb.Append("<" + k + "><![CDATA[" + v + "]]></" + k + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }


        /// <summary>
        ///     设置debug信息
        /// </summary>
        /// <param name="debugInfo"></param>
        public void SetDebugInfo(string debugInfo)
        {
            DebugInfo = debugInfo;
        }

        public Hashtable GetAllParameters()
        {
            return Parameters;
        }

        protected virtual string GetCharset()
        {
            return HttpContext.Request.ContentEncoding.BodyName;
        }
    }
}
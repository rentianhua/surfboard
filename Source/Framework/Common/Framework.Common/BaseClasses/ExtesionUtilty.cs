using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    /// 字符串处理
    /// </summary>
    public static class StringExtesion
    {
        /// <summary>
        /// Html编码
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string theString)
        {
            return theString.Replace(">", "&gt;")
                .Replace("<", "&lt;")
                .Replace(" ", "&nbsp;")
                .Replace("\"", "&quot;")
                .Replace("\'", "&#39;")
                .Replace("&", "&amp;")
                .Replace("'", "&apos;");
        }

        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string theString)
        {
            return theString.Replace("&amp;", "&")
                .Replace("&gt;", ">")
                .Replace("&lt;", "<")
                .Replace("&nbsp;", " ")
                .Replace("&quot;", "\"")
                .Replace("&#39;", "\'")
                .Replace("<br/>", "\n");
        }

        /// <summary>
        ///     字符串处理，将字符串中 #{str}/@{str} 处理成 str
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string StringFilter(this string source)
        {
            const string pattern1 = @"#{(?![^}]*#{)[^}]*}"; //保存关键字
            const string pattern2 = @"@{(?![^}]*@{)[^}]*}"; //保存关键字并打上标签

            var mList = Regex.Matches(source, pattern1);
            source = mList.Cast<Match>()
                .Aggregate(source, (current, m) => current.Replace(m.Value, m.Value.Substring(2, m.Value.Length - 3)));
            var mList2 = Regex.Matches(source, pattern2);
            return mList2.Cast<Match>()
                .Aggregate(source, (current, m) => current.Replace(m.Value, m.Value.Substring(2, m.Value.Length - 3)));
        }

        /// <summary>
        /// 转换成mysql值
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public static string ToDbValue(this string userInput)
        {
            return userInput?.Replace("%", "\\%").Replace("'", "\\'") ?? "";
        }

        /// <summary>
        ///     将值转换成图片显示
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="value">转换值</param>
        /// <param name="imageSrc">可访问图片源地址</param>
        /// <returns>转换结果</returns>
        public static bool ToImageSrc(HttpRequestMessage request, string value, out string imageSrc)
        {
            imageSrc = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (0 == value.IndexOf("http://", StringComparison.InvariantCultureIgnoreCase))
                    {
                        imageSrc = value;
                    }
                    else
                    {
                        Guid guid;
                        if (Guid.TryParse(value, out guid))
                        {
                            if (request != null)
                            {
                                var protocal = request.RequestUri.Scheme;
                                var host = request.RequestUri.Authority;
                                imageSrc = protocal + @"://" + host + @"/ShowImg/" + value;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                imageSrc = ex.Message;
                return false;
            }
        }

        /// <summary>
        ///     序列化MQModel实体信息
        /// </summary>
        /// <param name="model">MQModel实体</param>
        /// <returns>序列化Json</returns>
        public static string ToJson(this object model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        /// <summary>
        /// 字符串截取
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string SubStr(this string str, int length)
        {
            const string pat = "[\u4e00-\u9fa5]";
            var temp = str;
            if (Regex.Replace(temp, pat, "zz", RegexOptions.IgnoreCase).Length <= length)
            {
                return temp;
            }
            for (var i = temp.Length; i >= 0; i--)
            {
                temp = temp.Substring(0, i);
                if (Regex.Replace(temp, pat, "zz", RegexOptions.IgnoreCase).Length <= length)
                {
                    return temp;
                }
            }
            return "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cedar.Foundation.SMS.Common
{
    public class SMSSDK
    {
        private string _smsapi = ConfigurationManager.AppSettings["smsapi"];

        /// <summary>
        /// 
        /// </summary>
        public static readonly Dictionary<string, string> DicSmsResultInfo = new Dictionary<string, string>()
        {
            {"0", "操作成功"},
            {"-1", "签权失败"},
            {"-2", "未检索到被叫号码"},
            {"-3", "被叫号码过多"},
            {"-4", "内容未签名"},
            {"-5", "内容过长"},
            {"-6", "余额不足"},
            {"-7", "暂停发送"},
            {"-8", "保留"},
            {"-9", "定时发送时间格式错误"},
            {"-10", "下发内容为空"},
            {"-11", "账户无效"},
            {"-12", "Ip地址非法"},
            {"-13", "操作频率快"},
            {"-14", "操作失败"},
            {"-15", "拓展码无效(1-99999)"}
        };

        /// <summary>
        /// 请求URL（以GET方式请求）
        /// </summary>
        /// <param name="postUrl">请求地址</param>
        /// <returns>请求结果</returns>
        public static string GetWebRequest(string postUrl)
        {
            string ret;
            try
            {
                var webReq = (HttpWebRequest) WebRequest.Create(new Uri(postUrl));
                webReq.Method = "Get";
                webReq.ContentType = "application/x-www-form-urlencoded";
                var response = (HttpWebResponse) webReq.GetResponse();
                // ReSharper disable once AssignNullToNotNullAttribute
                var sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                return "-1";
            }
            return ret;
        }

        /// <summary>
        /// 请求URL（以POST方式请求）
        /// </summary>
        /// <param name="postUrl">请求地址</param>
        /// <param name="param"></param>
        /// <returns>请求结果</returns>
        public static string PostWebRequest(string postUrl, string param)
        {
            var req = (HttpWebRequest) WebRequest.Create(postUrl);
            Encoding encoding = Encoding.UTF8;
            byte[] bs = Encoding.ASCII.GetBytes(param);
            string responseData;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
            }
            using (var response = (HttpWebResponse) req.GetResponse())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    responseData = reader.ReadToEnd();
                }
            }

            return responseData;
        }
    }
}

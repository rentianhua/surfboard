using System;
using System.Configuration;
using System.Text;
using System.Web;

namespace Cedar.Foundation.SMS.Common
{
    public class SMSMSG
    {
        private readonly string _smsapi = ConfigurationManager.AppSettings["smsapi"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public SendResult SendSms(string to, string content)
        {
            var newurl = string.Format(_smsapi, to, HttpUtility.UrlEncode(content, Encoding.GetEncoding("GBK")));
            var sendResult = SMSSDK.GetWebRequest(newurl);
            try
            {
                return new SendResult
                {
                    errcode = sendResult.Split(',')[0],
                    errmsg = SMSSDK.DicSmsResultInfo[sendResult.Split(',')[0]]
                };
            }
            catch (Exception)
            {
                return new SendResult {errcode = "-100", errmsg = "未知错误"};
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SendResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errmsg { get; set; }
    }
}

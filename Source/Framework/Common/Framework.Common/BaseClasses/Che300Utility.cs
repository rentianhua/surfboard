using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    /// 
    /// </summary>
    public class Che300Utility
    {
        private readonly string _appkey = "89f5c616f242348a894728b73becfd64";
        private readonly string _url = "http://api.che300.com/service/getUsedCarPrice";

        /// <summary>
        ///     初始化
        /// </summary>
        public Che300Utility()
        {
            if (ConfigurationManager.AppSettings["che300_appkey"] != null)
            {
                _appkey = ConfigurationManager.AppSettings["che300_appkey"];
            }
            if (ConfigurationManager.AppSettings["che300_url"] != null)
            {
                _url = ConfigurationManager.AppSettings["che300_url"];
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string GetUsedCarPrice(Dictionary<string, string> paramList)
        {
            var paramBase = new Dictionary<string, string>
            {
                {"token", _appkey}
            };

            //合并参数
            var parameters = paramBase.Concat(paramList).ToDictionary(k => k.Key, v => v.Value);

            var result = DynamicWebService.SendPost(_url, parameters, "get");

            var obj = JObject.Parse(result);
            var errorCode = obj["status"].ToString();
            return errorCode != "1" ? "0" : result;
        }
    }
}

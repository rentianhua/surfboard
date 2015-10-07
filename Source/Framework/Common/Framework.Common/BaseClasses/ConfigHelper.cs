using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 根据key获取value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettings(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key] ?? "";            
        }
    }
}

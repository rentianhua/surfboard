using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using Cedar.Core.Logging;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using Senparc.Weixin.MP.CommonAPIs;

namespace CCN.Midware.Wechat
{
    class Program
    {
        static void Main(string[] args)
        {
            string _appid = ConfigurationManager.AppSettings["APPID"];
            string _appSecret = ConfigurationManager.AppSettings["AppSecret"];
            string host = $"http://{ConfigurationManager.AppSettings["hostip"]}";
            var strhost = $"Service start and linsent on {host}...";
            try
            {
                using (WebApp.Start<Startup>(host))
                {
                    Console.WriteLine(strhost);
                    LoggerFactories.CreateLogger().Write(strhost, TraceEventType.Information);
                    if (!AccessTokenRedisContainer.CheckRegistered(_appid))
                        AccessTokenRedisContainer.Register(_appid, _appSecret);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException.Message);
                
            }
            
        }
    }
}

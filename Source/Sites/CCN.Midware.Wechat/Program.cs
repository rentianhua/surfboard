using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;

namespace CCN.Midware.Wechat
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = $"http://{ConfigurationManager.AppSettings["hostip"]}";
            using (WebApp.Start<Startup>(host))
            {
                Console.WriteLine($"Service start and linsent on {host}...");
                Console.ReadLine();
            }
        }
    }
}

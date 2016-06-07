using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Cedar.Core.Logging;

namespace Cedar.Foundation.WeChat.WxPay.Lib
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg)
        {
            LoggerFactories.CreateLogger().Write("WxPay post ：" + msg, TraceEventType.Error);
        }
     }
}
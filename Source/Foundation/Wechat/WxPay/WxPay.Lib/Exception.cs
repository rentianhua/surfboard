using System;
using System.Collections.Generic;
using System.Web;

namespace Cedar.Foundation.WeChat.WxPay.Lib
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}
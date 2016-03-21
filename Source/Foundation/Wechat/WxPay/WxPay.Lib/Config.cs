namespace Cedar.Foundation.WeChat.WxPay.Lib
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public static string APPID = "wx8237977d5ac3d164";
        public static string MCHID = "1270879801";
        public static string KEY = "2257013F03EA762AB1559A00F22D2F86";
        public static string APPSECRET = "9dde5c8a66b2c712b6d4b2ab8c7c5173";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public static string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public static string SSLCERT_PASSWORD = "1233410002";



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public static string NOTIFY_URL = "http://wechat.chexinwang.cn/api/WeChatCallback/DataDispatcher";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public static string IP = "8.8.8.8";
        
        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public static string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public static int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public static int LOG_LEVENL = 0;

        public WxPayConfig()
        {
            Init();
        }

        /// <summary>
        /// 初始化七牛帐户、请求地址等信息，不应在客户端调用。
        /// </summary>
        public static void Init()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["APPID"] != null)
            {
                APPID = System.Configuration.ConfigurationManager.AppSettings["APPID"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["MCHID"] != null)
            {
                MCHID = System.Configuration.ConfigurationManager.AppSettings["MCHID"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["PAYKEY"] != null)
            {
                KEY = System.Configuration.ConfigurationManager.AppSettings["PAYKEY"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["APPSECRET"] != null)
            {
                APPSECRET = System.Configuration.ConfigurationManager.AppSettings["APPSECRET"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["SSLCERT_PATH"] != null)
            {
                SSLCERT_PATH = System.Configuration.ConfigurationManager.AppSettings["SSLCERT_PATH"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["SSLCERT_PASSWORD"] != null)
            {
                SSLCERT_PASSWORD = System.Configuration.ConfigurationManager.AppSettings["SSLCERT_PASSWORD"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["NOTIFY_URL"] != null)
            {
                NOTIFY_URL = System.Configuration.ConfigurationManager.AppSettings["NOTIFY_URL"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["PAYIP"] != null)
            {
                IP = System.Configuration.ConfigurationManager.AppSettings["PAYIP"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["PROXY_URL"] != null)
            {
                PROXY_URL = System.Configuration.ConfigurationManager.AppSettings["PROXY_URL"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["REPORT_LEVENL"] != null)
            {
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["REPORT_LEVENL"],out REPORT_LEVENL);
            }
            if (System.Configuration.ConfigurationManager.AppSettings["LOG_LEVENL"] != null)
            {
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["LOG_LEVENL"], out LOG_LEVENL);
            }
        }
    }
}
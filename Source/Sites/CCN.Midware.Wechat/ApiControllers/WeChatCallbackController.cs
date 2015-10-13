using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.CommonAPIs;

namespace CCN.Midware.Wechat.Controllers
{
    [RoutePrefix("api/WeChatCallback")]
    public class WeChatCallbackController : ApiController
    {
        private readonly HttpResponseMessage _response;
        private readonly string _appid = ConfigurationManager.AppSettings["APPID"];
        private readonly string _appSecret = ConfigurationManager.AppSettings["AppSecret"];

        public WeChatCallbackController()
        {
            _response = new HttpResponseMessage { Content = new StringContent("") };
            var appid = ConfigurationManager.AppSettings["APPID"];
            if (!AccessTokenContainer.CheckRegistered(appid))
                AccessTokenContainer.Register(appid, ConfigurationManager.AppSettings["AppSecret"]);
        }

        [HttpGet]
        public HttpResponseMessage DataDispatcher(string signature, string timestamp, string nonce, string echostr)
        {
            Console.WriteLine($"----------------get Start {DateTime.Now}----------------");
            Console.WriteLine($"----------------{signature}_{timestamp}_{nonce}_{echostr}----------------");
            Console.WriteLine($"----------------pass:{signature}----------------");
            Console.WriteLine($"----------------{CheckSignature.Check(signature, timestamp, nonce, "weixin")}----------------");
            _response.Content = CheckSignature.Check(signature, timestamp, nonce, !string.IsNullOrEmpty(ConfigurationManager.AppSettings["wechattoken"]) ?
                ConfigurationManager.AppSettings["wechattoken"] : null)
                ? new StringContent(echostr) : _response.Content;
            return _response;
        }

        /// <summary>
        /// 最简化的处理流程（不加密）
        /// </summary>
        [HttpPost]
        public HttpResponseMessage DataDispatcher()
        {
            var stream = Request.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"----------------Start {DateTime.Now}----------------");
            Console.WriteLine(stream);
            Console.WriteLine($"----------------End {DateTime.Now}----------------");
            return _response;
        }

        [HttpGet]
        [Route("GetTicket/{appid}")]
        public string GetTicket(string appid)
        {
            return AccessTokenContainer.TryGetAccessToken(_appid, _appSecret);
        }

        [HttpGet]
        [Route("GetToken/{appid}")]
        public string GetToken(string appid)
        {
            return AccessTokenContainer.TryGetJsApiTicket(_appid, _appSecret);
        }
    }
}

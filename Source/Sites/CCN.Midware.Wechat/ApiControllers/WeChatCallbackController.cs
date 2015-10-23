using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CCN.Midware.Wechat.Business;
using Cedar.Core.IoC;
using Cedar.Core.Logging;
using Cedar.Foundation.WeChat.Interface;
using Cedar.Framework.Common.BaseClasses;
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
        private readonly IWeChatManagementService _service;

        public WeChatCallbackController()
        {
            _service = ServiceLocatorFactory.GetServiceLocator().GetService<IWeChatManagementService>();
            _response = new HttpResponseMessage { Content = new StringContent("") };
            if (!AccessTokenRedisContainer.CheckRegistered(_appid))
                AccessTokenRedisContainer.Register(_appid, _appSecret);
        }

        [HttpGet]
        [Route("DataDispatcher")]
        public HttpResponseMessage DataDispatcher(string signature, string timestamp, string nonce, string echostr)
        {
            Console.WriteLine($"----------------get Start {DateTime.Now}----------------");
            Console.WriteLine($"----------------{CheckSignature.Check(signature, timestamp, nonce, "weixin")}----------------");
            LoggerFactories.CreateLogger().Write($"{CheckSignature.Check(signature, timestamp, nonce, "weixin")}", TraceEventType.Information);
            _response.Content = CheckSignature.Check(signature, timestamp, nonce, !string.IsNullOrEmpty(ConfigurationManager.AppSettings["wechattoken"]) ?
                ConfigurationManager.AppSettings["wechattoken"] : null)
                ? new StringContent(echostr) : _response.Content;
            return _response;
        }

        /// <summary>
        /// 最简化的处理流程（不加密）
        /// </summary>
        [HttpPost]
        [Route("DataDispatcher")]
        public HttpResponseMessage DataDispatcher()
        {
            var stream = Request.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"----------------Start {DateTime.Now}----------------");
            LoggerFactories.CreateLogger().Write(stream, TraceEventType.Information);
            Console.WriteLine(stream);
            try
            {
                Task.Run(() => RequestMessageFactory.GetRequestEntity(_service, stream));
            }
            catch (Exception e)
            {
                LoggerFactories.CreateLogger().Write(e.Message, TraceEventType.Error, e);
            }
            Console.WriteLine($"----------------End {DateTime.Now}----------------");
            return _response;
        }

        [HttpGet]
        [Route("GetTicket/{appid}")]
        public JResult GetTicket(string appid)
        {
            var ticket = AccessTokenRedisContainer.TryGetJsApiTicket(_appid, _appSecret);
            return new JResult
            {
                errcode = 0,
                errmsg = new
                {
                    ticket
                }
            };
        }

        [HttpGet]
        [Route("GetToken/{appid}")]
        public JResult GetToken(string appid)
        {
            var token = AccessTokenRedisContainer.TryGetAccessToken(_appid, _appSecret);
            return new JResult
            {
                errcode = 0,
                errmsg = new
                {
                    token
                }
            };
        }

        [HttpGet]
        [Route("GenerateWechatFriends/{appid}")]
        public JResult GenerateWechatFriends(string appid)
        {
            var result = _service.GenerateWechatFriends(appid);
            return result;
        }
    }
}

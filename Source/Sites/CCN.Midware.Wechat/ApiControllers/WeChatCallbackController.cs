using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Http;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MvcExtension;

namespace CCN.Midware.Wechat.Controllers
{
    public class WeChatCallbackController : ApiController
    {
        public WeChatCallbackController()
        {
        }

        [HttpGet]
        public WeixinResult DataDispatcher(string signature, string timestamp, string nonce, string echostr)
        {
            return !CheckSignature.Check(signature, timestamp, nonce, null) ? new WeixinResult("") : new WeixinResult(echostr);
        }

        /// <summary>
        /// 最简化的处理流程（不加密）
        /// </summary>
        [HttpPost]
        public string DataDispatcher([FromBody] string data)
        {
            Console.WriteLine(data);
            return "";
        }
    }


}

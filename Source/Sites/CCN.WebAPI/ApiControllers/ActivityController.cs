using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using CCN.Modules.Activity.BusinessEntity;
using CCN.Modules.Activity.Interface;
using CCN.Modules.Auction.BusinessEntity;
using Cedar.Core.IoC;
using Cedar.Core.Logging;
using Cedar.Foundation.WeChat.WxPay.Business;
using Cedar.Foundation.WeChat.WxPay.Business.WxPay.Entity;
using Cedar.Framework.Common.BaseClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.Helpers;

namespace CCN.WebAPI.ApiControllers
{
    /// <summary>
    /// 基础模块
    /// </summary>
    [RoutePrefix("api/Activity")]
    public class ActivityController : ApiController
    {
        private readonly IActivityManagementService _activityservice;

        public ActivityController()
        {
            _activityservice = ServiceLocatorFactory.GetServiceLocator().GetService<IActivityManagementService>();
        }

        #region 投票活动

        /// <summary>
        /// 获取投票活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVoteList")]
        public BasePageList<VoteListModel> GetVoteList([FromBody]VoteQueryModel query)
        {
            return _activityservice.GetVoteList(query);
        }

        /// <summary>
        /// 获取投票活动详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetVoteViewById")]
        public JResult GetVoteViewById(string id)
        {
            return _activityservice.GetVoteViewById(id);
        }

        #endregion

        #region 投票活动参赛人员
        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVotePerList")]
        public BasePageList<VotePerListModel> GetVotePerList([FromBody]VotePerQueryModel query)
        {
            return _activityservice.GetVotePerList(query);
        }

        /// <summary>
        /// 获取投票活动的参赛人员详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetVotePerViewById")]
        public JResult GetVotePerViewById(string id)
        {
            return _activityservice.GetVotePerViewById(id);
        }

        /// <summary>
        /// 参赛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddVotePer")]
        public JResult AddVotePer([FromBody]VotePerModel model)
        {
            return _activityservice.AddVotePer(model);
        }

        #endregion

        #region 投票日志

        /// <summary>
        /// 获取参赛人的投票列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVoteLogList")]
        public BasePageList<VoteLogListModel> GetVoteLogList([FromBody]VoteLogQueryModel query)
        {
            return _activityservice.GetVoteLogList(query);
        }

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddVoteLog")]
        public JResult AddVoteLog([FromBody]VoteLogModel model)
        {
            return _activityservice.AddVoteLog(model);
        }

        #endregion
        
        [HttpPost]
        [Route("TestPay")]
        public JResult TestPay()
        {
            var ran = new Random();
            var outTradeNo = $"{"WXPAY"}{DateTime.Now.ToString("yyyyMMddHHmmss")}{ran.Next(999)}";

            var data = new NativePayData
            {
                Body = "快拍立信定金",//商品描述
                Attach = "快拍立信看车费",//附加数据
                TotalFee = 1,//总金额
                ProductId = "productid",//商品ID
                OutTradeNo = outTradeNo,
                GoodsTag = ""
            };
            
            var jresult = WxPayAPIs.GetNativePayQrCode(data);
            return jresult;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("TestPayBack")]
        public HttpResponseMessage TestPayBack()
        {
            var stream = Request.Content.ReadAsStringAsync().Result;
            LoggerFactories.CreateLogger().Write($"WxPay Result: {stream}", TraceEventType.Information);
            
            try
            {
                var doc = XDocument.Parse(stream);
                var model = new AuctionPaymentRecordModel();
                model.FillEntityWithXml(doc);
                //var result = _auctionservice.AddPaymentRecord(model);
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write($"WxPay Result Ex: {ex.Message}", TraceEventType.Information);
            }

            //调用nodejs 通知前端
            return new HttpResponseMessage { Content = new StringContent("") };
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("TestPu")]
        public void TestPu()
        {
            //调用nodejs 通知前端
            var localapi = ConfigHelper.GetAppSettings("localapi") + "api/Auction/UnifiedOrder";

            //IDictionary<string, string> parameters = new Dictionary<string, string>()
            //{
            //    {"Body","快拍立信拍车定金" },
            //    {"Attach","kply" },
            //    {"TotalFee","1"},
            //    {"ProductId","P160323133517"},
            //    {"OutTradeNo","P160323133517" },
            //    {"GoodsTag","" }
            //};

            var json = "{\"Body\":\"快拍立信拍车定金\",\"Attach\":\"kply\",\"TotalFee\":\"1\",\"ProductId\":\"P160323133517\",\"OutTradeNo\":\"P160323133517\",\"GoodsTag\":\"\"}";
            var orderresult = DynamicWebService.ExeApiMethod(localapi, "post", json);
            //var orderresult = DynamicWebService.SendPost(nodejs, parameters, "post");
        }
    }    
}

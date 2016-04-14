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

        #region 众筹活动

        [HttpPost]
        [Route("CrowdUnifiedOrder")]
        public JResult CrowdUnifiedOrder([FromBody]CrowdUnifiedOrderModel model)
        {
            return _activityservice.CrowdUnifiedOrder(model);
        }
        
        #endregion

        [HttpPost]
        [Route("TestPay")]
        public JResult TestPay()
        {

            return null;
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
        public void TestPu(string mobile)
        {
            var b = System.Text.RegularExpressions.Regex.IsMatch(mobile, @"^1[3|4|5|8][0-9]\d{8}$");
            if (b)
            {
                LoggerFactories.CreateLogger().Write("TestPu true：" + mobile, TraceEventType.Information);
            }
            else
            {
                LoggerFactories.CreateLogger().Write("TestPu false：" + mobile, TraceEventType.Information);
            }

            //var url = ConfigHelper.GetAppSettings("nodejssiteurl") + "auction/largeTransaction";
            //var param = new Dictionary<string, string>
            //    {
            //        {"innerid", "40805cab-36c0-4395-8811-7a71a97280f0"}
            //    };
            //var nodeRes = DynamicWebService.SendPost(url, param, "post");
        }
    }    
}

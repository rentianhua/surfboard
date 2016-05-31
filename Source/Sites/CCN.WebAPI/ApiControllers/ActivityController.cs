using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Http;
using System.Xml.Linq;
using CCN.Modules.Activity.BusinessEntity;
using CCN.Modules.Activity.Interface;
using CCN.Modules.Auction.BusinessEntity;
using Cedar.Core.IoC;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
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
        /// 验证是否粉丝
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckIsFans")]
        public JResult CheckIsFans(dynamic dc)
        {
            LoggerFactories.CreateLogger().Write("CheckIsFans参数：" + JsonConvert.SerializeObject(dc), TraceEventType.Information);
            try
            {
                if (string.IsNullOrEmpty(dc.appid.ToString()))
                {
                    dc.appid = ConfigHelper.GetAppSettings("APPID");
                }

                dynamic backmeg = UserApi.Info(dc.appid.ToString(), dc.openid.ToString());
                return string.IsNullOrEmpty(backmeg.nickname)
                    ? JResult._jResult(400, "非粉丝")
                    : JResult._jResult(0, "粉丝");
            }
            catch (Exception)
            {
                return JResult._jResult(400, "非粉丝");
            }
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

        #endregion
        #region 众筹活动

        #region 活动管理

        /// <summary>
        /// 开始抽奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("StartDraw")]
        public JResult StartDraw([FromBody] StartDrawModel model)
        {
            return _activityservice.StartDraw(model);
        }

        /// <summary>
        /// 结束抽奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EndDraw")]
        public JResult EndDraw([FromBody] StartDrawModel model)
        {
            return _activityservice.EndDraw(model);
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCrowdActivityPageList")]
        public BasePageList<CrowdInfoListModel> GetCrowdActivityPageList([FromBody]CrowdInfoQueryModel query)
        {
            return _activityservice.GetCrowdActivityPageList(query);
        }

        /// <summary>
        /// 获取活动详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCrowdInfoById")]
        public JResult GetCrowdInfoById(string innerid)
        {
            return _activityservice.GetCrowdInfoById(innerid);
        }

        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCrowdViewById")]
        public JResult GetCrowdViewById(string flagcode)
        {
            return _activityservice.GetCrowdViewById(flagcode);
        }

        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCrowdProgressByFlagcode")]
        public JResult GetCrowdProgressByFlagcode(string flagcode)
        {
            return _activityservice.GetCrowdProgressByFlagcode(flagcode);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCrowdInfo")]
        public JResult AddCrowdInfo([FromBody]CrowdInfoModel model)
        {
            return _activityservice.AddCrowdInfo(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCrowdInfo")]
        public JResult UpdateCrowdInfo([FromBody]CrowdInfoModel model)
        {
            return _activityservice.UpdateCrowdInfo(model);
        }

        /// <summary>
        /// 获取活动的信息及档次list信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCrowdActivityTotal")]
        public JResult GetCrowdActivityTotal(string flagcode)
        {
            return _activityservice.GetCrowdActivityTotal(flagcode);
        }

        #endregion

        #region 档次管理
        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetGradePageList")]
        public BasePageList<CrowdGradeModel> GetGradePageList([FromBody]QueryModel query)
        {
            return _activityservice.GetGradePageList(query);
        }

        /// <summary>
        /// 获取档次列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGradeListByFlagcode")]
        public JResult GetGradeListByFlagcode(string flagcode)
        {
            return _activityservice.GetGradeListByFlagcode(flagcode);
        }

        /// <summary>
        /// 获取档次详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGradeInfoById")]
        public JResult GetGradeInfoById(string innerid)
        {
            return _activityservice.GetGradeInfoById(innerid);
        }

        /// <summary>
        /// 添加档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddGrade")]
        public JResult AddGrade([FromBody]CrowdGradeModel model)
        {
            return _activityservice.AddGrade(model);
        }

        /// <summary>
        /// 修改档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateGrade")]
        public JResult UpdateGrade([FromBody]CrowdGradeModel model)
        {
            return _activityservice.UpdateGrade(model);
        }

        #endregion

        #region 参与人管理
        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPlayerPageList")]
        public BasePageList<CrowdPlayerListModel> GetPlayerPageList([FromBody]CrowdPlayerQueryModel query)
        {
            return _activityservice.GetPlayerPageList(query);
        }

        /// <summary>
        /// 获取Player列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPlayerListByFlagcode")]
        public JResult GetPlayerListByFlagcode(string flagcode)
        {
            return _activityservice.GetPlayerListByFlagcode(flagcode);
        }

        /// <summary>
        /// 获取Player详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPlayerInfoById")]
        public JResult GetPlayerInfoById(string innerid)
        {
            return _activityservice.GetPlayerInfoById(innerid);
        }

        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPlayer")]
        public JResult AddPlayer([FromBody]CrowdPlayerModel model)
        {
            return _activityservice.AddPlayer(model);
        }
        /// <summary>
        /// UpdatePlayer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdatePlayer")]
        public JResult UpdatePlayer([FromBody]CrowdPlayerModel model)
        {
            return _activityservice.UpdatePlayer(model);
        }

        /// <summary>
        /// 获取活动信息及用户已支付总金额
        /// </summary>
        /// <param name="flagcode">活动码</param>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetActivityAndPaidTotal")]
        public JResult GetActivityAndPaidTotal(string flagcode, string openid)
        {
            return _activityservice.GetActivityAndPaidTotal(flagcode, openid);
        }

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DoPay")]
        public JResult DoPay(string orderNo)
        {
            return _activityservice.DoPay(orderNo);
        }
        #endregion

        [HttpGet]
        [Route("CrowdGenerateQrCode")]
        public JResult CrowdGenerateQrCode(string flagcode)
        {
            return _activityservice.CrowdGenerateQrCode(flagcode);
        }

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
            var b = Regex.IsMatch(mobile, @"^1[3|4|5|8][0-9]\d{8}$");
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

    public abstract class AbdClass
    {
        public void GetValue()
        {

            short s1 = 1;
            s1 = (short)(s1 + 1);

            string str = " i am        a student. ";
            str = Regex.Replace(str.Trim(), " *", " ");
        }

    }
}

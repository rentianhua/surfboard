using System.Web.Http;
using CCN.Modules.Auction.BusinessEntity;
using CCN.Modules.Auction.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using Cedar.Foundation.WeChat.WxPay.Business.WxPay.Entity;
using Cedar.Foundation.WeChat.WxPay.Business;
using System.Xml;
using System.Data;
using System.Linq;
using Cedar.Core.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Xml.Linq;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.Helpers;

namespace CCN.WebAPI.ApiControllers
{
    /// <summary>
    /// 基础模块
    /// </summary>
    [RoutePrefix("api/Auction")]
    public class AuctionController : ApiController
    {
        private readonly IAuctionManagementService _auctionservice;

        public AuctionController()
        {
            _auctionservice = ServiceLocatorFactory.GetServiceLocator().GetService<IAuctionManagementService>();
        }

        #region 拍卖车辆基本信息

        /// <summary>
        /// 获取正在拍卖的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAuctioningList")]
        public BasePageList<AuctionCarInfoViewModel> GetAuctioningList([FromBody] AuctionCarInfoQueryModel query)
        {
            return _auctionservice.GetAuctioningList(query);
        }

        /// <summary>
        /// 获取正在拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctioningViewById")]
        public JResult GetAuctioningViewById(string id)
        {
            return _auctionservice.GetAuctioningViewById(id);
        }

        /// <summary>
        /// 添加拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddAuctionCar")]
        public JResult AddAuctionCar([FromBody] AuctionCarInfoModel model)
        {
            return _auctionservice.AddAuctionCar(model);
        }

        /// <summary>
        /// 更新拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateAuctionCar")]
        public JResult UpdateAuctionCar([FromBody] AuctionCarInfoModel model)
        {
            return _auctionservice.UpdateAuctionCar(model);
        }

        /// <summary>
        /// 获取拍卖车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctionInfoById")]
        public JResult GetAuctionInfoById(string id)
        {
            return _auctionservice.GetAuctionInfoById(id);
        }

        #endregion

        #region 竞拍

        /// <summary>
        /// 添加拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddParticipant")]
        public JResult AddParticipant([FromBody]AuctionCarParticipantModel model)
        {
            return _auctionservice.AddParticipant(model);
        }

        /// <summary>
        ///  获取所有竞拍记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllAuctionParticipantList")]
        public JResult GetAllAuctionParticipantList(AuctionCarParticipantQueryModel query)
        {
            return _auctionservice.GetAllAuctionParticipantList(query);
        }

        /// <summary>
        ///  获取所有竞拍记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAuctionParticipantList")]
        public BasePageList<AuctionCarParticipantViewModel> GetAuctionParticipantList(AuctionCarParticipantQueryModel query)
        {
            return _auctionservice.GetAuctionParticipantList(query);
        }


        /// <summary>
        /// 根据拍卖ID 获取竞拍记录
        /// </summary>
        /// <param name="auctionid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPriceCount")]
        public JResult GetPriceCount(string auctionid)
        {
            return _auctionservice.GetPriceCount(auctionid);
        }

        /// <summary>
        /// 支付完成更新出价状态
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateStatusForPay")]
        public JResult UpdateStatusForPay(string orderno)
        {
            return _auctionservice.UpdateStatusForPay(orderno);
        }

        #endregion

        #region 拍卖时间 

        /// <summary>
        /// 获取拍卖时间列表 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctionTimeList")]
        public JResult GetAuctionTimeList()
        {
            return _auctionservice.GetAuctionTimeList();
        }

        #endregion

        #region 获取认证报告

        /// <summary>
        /// 获取认证项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AuctionCarInspectionItem")]
        public JResult AuctionCarInspectionItem()
        {
            return _auctionservice.AuctionCarInspectionItem();
        }

        /// <summary>
        /// 获取认证报告内容
        /// </summary>
        /// <param name="id">拍卖ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctionCarInspectionResult")]
        public JResult GetAuctionCarInspectionResult(string id)
        {
            return _auctionservice.GetAuctionCarInspectionResult(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInspectionResultForHtml")]
        public JResult GetInspectionResultForHtml(string id)
        {
            return _auctionservice.GetInspectionResultForHtml(id);
        }

        #endregion

        #region 关注

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Follow")]
        public JResult Follow([FromBody]AuctionFollowModel model)
        {
            return _auctionservice.Follow(model);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="usrid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Unfollow")]
        public JResult Unfollow(string auctionid, string userid)
        {
            return _auctionservice.Unfollow(auctionid, userid);
        }

        /// <summary>
        /// 判断用户是否关注了该拍卖车辆
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsFollow")]
        public JResult IsFollow(string auctionid, string userid)
        {
            return _auctionservice.IsFollow(auctionid, userid);
        }

        /// <summary>
        /// 获取关注的拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFollowPageList")]
        public BasePageList<AuctionCarInfoViewModel> GetFollowPageList([FromBody]AuctionFollowQueryModel query)
        {
            return _auctionservice.GetFollowPageList(query);
        }

        #endregion

        #region 支付

        /// <summary>
        /// 微信定金支付
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("WeChatPayForAuction")]
        public JResult WeChatPayForAuction(string innerid)
        {
            return _auctionservice.WeChatPayForAuction(innerid);
        }

        ///// <summary>
        ///// 微信支付结果回调
        ///// </summary>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("UnifiedOrder")]
        //public JResult UnifiedOrder()
        //{
        //    var stream = Request.Content.ReadAsStringAsync().Result;
        //    LoggerFactories.CreateLogger().Write($"WxPay Result: {stream}", TraceEventType.Information);
        //    if (string.IsNullOrWhiteSpace(stream))
        //    {
        //        return JResult._jResult(402,"参数不正确");
        //    }
        //    try
        //    {
        //        var jobj = JObject.Parse(stream);
        //        NativePayData data = new NativePayData();
        //        data.Body = jobj["Body"].ToString();
        //        data.Attach = jobj["Attach"].ToString();
        //        data.ProductId = jobj["ProductId"].ToString();
        //        data.OutTradeNo = jobj["OutTradeNo"].ToString();
        //        data.GoodsTag = jobj["GoodsTag"].ToString();
        //        int fee;
        //        int.TryParse(jobj["TotalFee"].ToString(), out fee);
        //        data.TotalFee = fee;

        //        var qrcodeResult = WxPayAPIs.GetNativePayQrCode(data);
        //        return qrcodeResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerFactories.CreateLogger().Write($"WxPay Result Ex: {ex.Message}", TraceEventType.Information);
        //        return JResult._jResult(500, ex.Message);
        //    }
        //}

        /// <summary>
        /// 微信支付结果回调
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("WxPayBack")]
        public HttpResponseMessage WxPayBack()
        {
            var stream = Request.Content.ReadAsStringAsync().Result;
            LoggerFactories.CreateLogger().Write($"WxPay Result: {stream}", TraceEventType.Information);
            try
            {
                var doc = XDocument.Parse(stream);
                var model = new AuctionPaymentRecordModel();
                model.FillEntityWithXml(doc);

                //记录支付结果
                var result = _auctionservice.AddPaymentRecord(model);
                LoggerFactories.CreateLogger().Write($"WxPay Saved Result: {result.errcode}", TraceEventType.Information);

                if (result.errcode != 0)
                {
                    return new HttpResponseMessage { Content = new StringContent("ERROR") };
                }

                string innerid;
                if (model.attach.Equals("kplx_auction"))
                {
                    var resultPay = _auctionservice.GetAuctionParticipantByOrderNo(model.out_trade_no);
                    var modelPay = (AuctionCarParticipantModel)resultPay.errmsg;
                    innerid = modelPay.Innerid;
                    //更新竞价
                    var upstatus = _auctionservice.UpdateStatusForPay(model.out_trade_no).errcode;
                    if (upstatus != 0)
                    {
                        LoggerFactories.CreateLogger().Write("socket result ： 支付定金完成，更新竞价状态出错！", TraceEventType.Information);
                    }
                }
                else //获取会员ID
                {
                    var custservice = ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();
                    var custPayModel = (CustWxPayModel)custservice.CustWeChatPayByorderno(model.out_trade_no).errmsg;
                    innerid = custPayModel.Innerid;
                    //更新会员状态
                    var ucstatus = custservice.CustWxPayVipBack(model.out_trade_no).errcode;
                    if (ucstatus != 0)
                    {
                        LoggerFactories.CreateLogger().Write("socket result ： 会员支付完成，更新状态出错！", TraceEventType.Information);
                    }
                }

                var url = ConfigHelper.GetAppSettings("nodejssiteurl") + "auction/largeTransaction";
                var param = new Dictionary<string, string>
                {
                    {"innerid", innerid}
                };
                var nodeRes = DynamicWebService.SendPost(url, param, "post");
                LoggerFactories.CreateLogger().Write("socket result ： " + nodeRes, TraceEventType.Information);
                return new HttpResponseMessage { Content = new StringContent("SUCCESS") };
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write($"WxPay Result Ex: {ex.Message}", TraceEventType.Information);
                return new HttpResponseMessage { Content = new StringContent("Exception") };
            }
        }

        #endregion

    }
}
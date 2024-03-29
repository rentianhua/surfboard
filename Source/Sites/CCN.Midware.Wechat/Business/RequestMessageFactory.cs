﻿
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CCN.Modules.Activity.Interface;
using CCN.Modules.Auction.BusinessEntity;
using CCN.Modules.Auction.Interface;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using CCN.Modules.Rewards.BusinessEntity;
using CCN.Modules.Rewards.Interface;
using Cedar.Core.IoC;
using Cedar.Core.Logging;
using Cedar.Foundation.WeChat.Interface;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.MerChant;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Helpers;

namespace CCN.Midware.Wechat.Business
{
    public static class RequestMessageFactory
    {
        //<?xml version="1.0" encoding="utf-8"?>
        //<xml>
        //  <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
        //  <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
        //  <CreateTime>1357986928</CreateTime>
        //  <MsgType><![CDATA[text]]></MsgType>
        //  <Content><![CDATA[中文]]></Content>
        //  <MsgId>5832509444155992350</MsgId>
        //</xml>

        private static readonly string AppID = ConfigurationManager.AppSettings["APPID"];
        private static readonly string _slogan = ConfigurationManager.AppSettings["SLOGAN"];
        private static readonly string _urlVote = ConfigurationManager.AppSettings["UrlVote"];   //投票大赛的地址

        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(IWeChatManagementService service, XDocument doc,
            PostModel postModel = null)
        {
            RequestMessageBase requestMessage = null;
            RequestMsgType msgType;

            try
            {
                msgType = MsgTypeHelper.GetRequestMsgType(doc);
                switch (msgType)
                {
                    case RequestMsgType.Text:
                        requestMessage = new RequestMessageText();
                        requestMessage.FillEntityWithXml(doc);

                        /*tim update by 2016-03-31*/
                        var rMessage = (RequestMessageText) requestMessage;
                        //回复手机号重新绑定到会员
                        var b = System.Text.RegularExpressions.Regex.IsMatch(rMessage.Content.Trim(),
                            @"^1[3|4|5|8][0-9]\d{8}$");
                        if (b)
                        {
                            var customerService =
                                ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();
                            var result = customerService.RebindFans(new CustRebindFansModel
                            {
                                Openid = rMessage.FromUserName,
                                Mobile = rMessage.Content
                            });
                            LoggerFactories.CreateLogger()
                                .Write($"rebind result：{result.errcode}", TraceEventType.Information);
                        }
                        else
                        {
                            //处理投票活动消息
                            var result = HandleVote(rMessage.Content, requestMessage.FromUserName);
                            if (result)
                            {
                                return requestMessage;
                            }
                            CustomApi.SendText(AppID, requestMessage.FromUserName, "感谢您的回复，玖伍淘车会尽快回复您。");
                        }

                        break;
                    case RequestMsgType.Location:
                        requestMessage = new RequestMessageLocation();
                        break;
                    case RequestMsgType.Image:
                        requestMessage = new RequestMessageImage();
                        break;
                    case RequestMsgType.Voice:
                        requestMessage = new RequestMessageVoice();
                        break;
                    case RequestMsgType.Video:
                        requestMessage = new RequestMessageVideo();
                        break;
                    case RequestMsgType.Link:
                        requestMessage = new RequestMessageLink();
                        break;
                    case RequestMsgType.ShortVideo:
                        requestMessage = new RequestMessageShortVideo();
                        break;
                    case RequestMsgType.Event:
                        //判断Event类型
                        switch (doc.Root.Element("Event").Value.ToUpper())
                        {
                            case "ENTER": //进入会话
                                requestMessage = new RequestMessageEvent_Enter();
                                break;
                            case "LOCATION": //地理位置
                                requestMessage = new RequestMessageEvent_Location();
                                break;
                            case "SUBSCRIBE": //订阅（关注）
                                requestMessage = new RequestMessageEvent_Subscribe();
                                EntityHelper.FillEntityWithXml(requestMessage, doc);
                                var taskSubscribe = new Task(() =>
                                {
                                    service.GenerateWechatFriend(AppID, requestMessage.FromUserName, true);
                                });
                                taskSubscribe.Start();
                                
                                var slogan = _slogan.Replace("\\n", Environment.NewLine);

                                //"\n\n&lt;a src=''&gt;点击参加玖伍淘车首届“/强绝代车王/强”微信自拍投票大赛&lt;a/&gt;";
                                var btnContent = ConfigurationManager.AppSettings["BtnContent"];

                                var tourl = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={AppID}&redirect_uri={_urlVote}&response_type=code&scope=snsapi_base&state=vote&connect_redirect=1#wechat_redirect";

                                btnContent = $"<a href=\"{tourl}\">{btnContent}</a>";
                                slogan = string.Concat(slogan, Environment.NewLine, btnContent);

                                CustomApi.SendText(AppID, requestMessage.FromUserName, slogan);

                                //判断是否扫描场景二维码关注
                                var xElement = doc.Root.Element("EventKey");
                                if (xElement != null && xElement.Value.Contains("qrscene_"))
                                {
                                    var code = xElement.Value.Replace("qrscene_", "");
                                    service.UpdateWechatFriendQrScene(requestMessage.FromUserName, code, AppID);
                                }
                                break;
                            case "UNSUBSCRIBE": //取消订阅（关注）
                                requestMessage = new RequestMessageEvent_Unsubscribe();
                                EntityHelper.FillEntityWithXml(requestMessage, doc);
                                service.UpdateWechatFriendUnSubscribe(AppID, requestMessage.FromUserName);

                                var activityservice = ServiceLocatorFactory.GetServiceLocator().GetService<IActivityManagementService>();
                                activityservice.UnSubscribe(AppID, requestMessage.FromUserName);

                                break;
                            case "CLICK": //菜单点击
                                requestMessage = new RequestMessageEvent_Click();
                                break;
                            case "SCAN": //二维码扫描
                                requestMessage = new RequestMessageEvent_Scan();
                                break;
                            case "VIEW": //URL跳转
                                requestMessage = new RequestMessageEvent_View();
                                break;
                            case "MASSSENDJOBFINISH":
                                requestMessage = new RequestMessageEvent_MassSendJobFinish();
                                break;
                            case "TEMPLATESENDJOBFINISH": //模板信息
                                requestMessage = new RequestMessageEvent_TemplateSendJobFinish();
                                break;
                            case "SCANCODE_PUSH": //扫码推事件(scancode_push)
                                requestMessage = new RequestMessageEvent_Scancode_Push();
                                break;
                            case "SCANCODE_WAITMSG": //扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
                                requestMessage = new RequestMessageEvent_Scancode_Waitmsg();
                                break;
                            case "PIC_SYSPHOTO": //弹出系统拍照发图(pic_sysphoto)
                                requestMessage = new RequestMessageEvent_Pic_Sysphoto();
                                break;
                            case "PIC_PHOTO_OR_ALBUM": //弹出拍照或者相册发图（pic_photo_or_album）
                                requestMessage = new RequestMessageEvent_Pic_Photo_Or_Album();
                                break;
                            case "PIC_WEIXIN": //弹出微信相册发图器(pic_weixin)
                                requestMessage = new RequestMessageEvent_Pic_Weixin();
                                break;
                            case "LOCATION_SELECT": //弹出地理位置选择器（location_select）
                                requestMessage = new RequestMessageEvent_Location_Select();
                                break;
                            case "CARD_PASS_CHECK": //卡券通过审核
                                requestMessage = new RequestMessageEvent_Card_Pass_Check();
                                break;
                            case "CARD_NOT_PASS_CHECK": //卡券未通过审核
                                requestMessage = new RequestMessageEvent_Card_Not_Pass_Check();
                                break;
                            case "USER_GET_CARD": //领取卡券
                                requestMessage = new RequestMessageEvent_User_Get_Card();
                                break;
                            case "USER_DEL_CARD": //删除卡券
                                requestMessage = new RequestMessageEvent_User_Del_Card();
                                break;
                            case "KF_CREATE_SESSION": //多客服接入会话
                                requestMessage = new RequestMessageEvent_Kf_Create_Session();
                                break;
                            case "KF_CLOSE_SESSION": //多客服关闭会话
                                requestMessage = new RequestMessageEvent_Kf_Close_Session();
                                break;
                            case "KF_SWITCH_SESSION": //多客服转接会话
                                requestMessage = new RequestMessageEvent_Kf_Switch_Session();
                                break;
                            case "POI_CHECK_NOTIFY": //审核结果事件推送
                                requestMessage = new RequestMessageEvent_Poi_Check_Notify();
                                break;
                            case "WIFICONNECTED": //Wi-Fi连网成功事件
                                requestMessage = new RequestMessageEvent_WifiConnected();
                                break;
                            case "USER_CONSUME_CARD": //卡券核销
                                requestMessage = new RequestMessageEvent_User_Consume_Card();
                                break;
                            case "USER_ENTER_SESSION_FROM_CARD": //从卡券进入公众号会话
                                requestMessage = new RequestMessageEvent_User_Enter_Session_From_Card();
                                break;
                            case "USER_VIEW_CARD": //进入会员卡
                                requestMessage = new RequestMessageEvent_User_View_Card();
                                break;
                            case "MERCHANT_ORDER": //微小店订单付款通知
                                requestMessage = new RequestMessageEvent_Merchant_Order();
                                var merchantOrderresult = (RequestMessageEvent_Merchant_Order) requestMessage;
                                EntityHelper.FillEntityWithXml(requestMessage, doc);
                                var orderresult = OrderApi.GetByIdOrder(AppID, merchantOrderresult.OrderId);
                                if (orderresult.errcode == ReturnCode.请求成功)
                                {
                                    var rewardsManagementService =
                                        ServiceLocatorFactory.GetServiceLocator()
                                            .GetService<IRewardsManagementService>();
                                    var wholesaleCouponresult =
                                        rewardsManagementService.WholesaleCoupon(new CouponBuyModel()
                                        {
                                            //ProductId = orderresult.order.product_id,
                                            //OrderId = orderresult.order.order_id,
                                            //Accountid = AppID,
                                            //Number = orderresult.order.product_count,
                                            //Openid = orderresult.order.buyer_openid
                                            Order = orderresult.order
                                        });
                                    var text = JsonConvert.SerializeObject(orderresult.order);
                                    var logresult =
                                        $"MERCHANT_ORDER:{text}    result:{JsonConvert.SerializeObject(wholesaleCouponresult)}";
                                    LoggerFactories.CreateLogger().Write(logresult, TraceEventType.Information);
                                }
                                break;
                            case "SUBMIT_MEMBERCARD_USER_INFO": //接收会员信息事件通知
                                requestMessage = new RequestMessageEvent_Submit_Membercard_User_Info();
                                break;
                            case "SHAKEAROUNDUSERSHAKE": //摇一摇事件通知
                                requestMessage = new RequestMessageEvent_ShakearoundUserShake();
                                break;
                            default: //其他意外类型（也可以选择抛出异常）
                                requestMessage = new RequestMessageEventBase();
                                break;
                        }
                        break;
                    default:
                        throw new UnknownRequestMsgTypeException(
                            $"MsgType：{msgType} 在RequestMessageFactory中没有对应的处理程序！", new ArgumentOutOfRangeException());
                    //为了能够对类型变动最大程度容错（如微信目前还可以对公众账号suscribe等未知类型，但API没有开放），建议在使用的时候catch这个异常
                }
            }
            catch (ArgumentException ex)
            {
                //throw new WeixinException(string.Format("RequestMessage转换出错！可能是MsgType不存在！，XML：{0}", doc.ToString()), ex);
                LoggerFactories.CreateLogger()
                    .Write($"RequestMessage转换出错！可能是MsgType不存在！，XML：{doc}", TraceEventType.Error, ex);
            }
            return requestMessage;
        }


        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(IWeChatManagementService service, string xml)
        {
            return GetRequestEntity(service, XDocument.Parse(xml));
        }

        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <param name="stream">如Request.InputStream</param>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(IWeChatManagementService service, Stream stream)
        {
            using (XmlReader xr = XmlReader.Create(stream))
            {
                var doc = XDocument.Load(xr);

                //

                return GetRequestEntity(service, doc);
            }
        }

        /// <summary>
        /// 处理微信支付结果信息
        /// </summary>
        /// <param name="xml">结果信息</param>
        /// <returns></returns>
        public static void HandlePayMessage(string xml)
        {
            var doc = XDocument.Parse(xml);
            var model = new AuctionPaymentRecordModel();
            model.FillEntityWithXml(doc);

            //待实现
        }

        /// <summary>
        /// 处理投票活动消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="openid"></param>
        /// <returns>返回是否正常处理</returns>
        public static bool HandleVote(string msg, string openid)
        {
            if (!msg.Trim().Equals("报名"))
                return false;

            
            var title = ConfigurationManager.AppSettings["Title"];
            var picUrl = ConfigurationManager.AppSettings["PicUrl"];
            var tourl = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={AppID}&redirect_uri={_urlVote}&response_type=code&scope=snsapi_base&state=vote&connect_redirect=1#wechat_redirect";
            var description = ConfigurationManager.AppSettings["Description"];
            CustomApi.SendNews(AppID, openid, new List<Article>
            {
                new Article
                {
                    Description = description,
                    PicUrl =picUrl,
                    Title = title,
                    Url = tourl
                }
            });
            return true;
        }
    }
}

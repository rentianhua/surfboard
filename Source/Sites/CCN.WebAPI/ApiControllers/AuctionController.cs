using System.Web.Http;
using CCN.Modules.Auction.BusinessEntity;
using CCN.Modules.Auction.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using System;
using Cedar.Foundation.WeChat.WxPay.Business.WxPay.Entity;
using Cedar.Foundation.WeChat.WxPay.Business;
using System.Xml;
using System.Data;
using System.Linq;


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
        /// <param name="orderno"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("WeChatPayForAuction")]
        public JResult WeChatPayForAuction(string innerid, string orderno)
        {
            var ran = new Random();
            var modelname = string.Empty;
            //获取定金金额
            var deposit = Convert.ToInt32(ConfigHelper.GetAppSettings("depositauction"));

            var data = new NativePayData
            {
                Body = "快拍立信拍车定金",//商品描述
                Attach = "【kply】",//附加数据
                TotalFee = deposit,//总金额
                ProductId = orderno,//商品ID
                OutTradeNo = orderno,//订单编号
                GoodsTag = ""
            };
            //获取竞拍详情
            var auctionParticipant = _auctionservice.GetAuctionParticipantByID(innerid);
            if (auctionParticipant.errcode == 0)
            {
                if (auctionParticipant.errmsg != null)
                {
                    var auctionParticipantModel = (AuctionCarParticipantViewModel)auctionParticipant.errmsg;
                    modelname = auctionParticipantModel.model_name;
                }
            }
            var qrcode = WxPayAPIs.GetNativePayQrCode(data);
            var result = "{\"qrcode\": \"" + qrcode + "\",\"modelname\": \"" + modelname + "\",\"deposit\": " + deposit + "}";
            return JResult._jResult(0, result);
        }

        /// <summary>
        /// 支付回调
        /// </summary>
        [HttpGet]
        [Route("WeChatPayForAuction")]
        public void GetResult()
        {
            var a = @"<xml><appid><![CDATA[wx8237977d5ac3d164]]></appid>
                        <attach><![CDATA[快拍立信看车费]]></attach>
                        <bank_type><![CDATA[CFT]]></bank_type>
                        <cash_fee><![CDATA[1]]></cash_fee>
                        <fee_type><![CDATA[CNY]]></fee_type>
                        <is_subscribe><![CDATA[Y]]></is_subscribe>
                        <mch_id><![CDATA[1270879801]]></mch_id>
                        <nonce_str><![CDATA[a6c6d5decae74ee0b484669491e48f6b]]></nonce_str>
                        <openid><![CDATA[oRpPlwYU_dfedweCJwMgHlUaSbNA]]></openid>
                        <out_trade_no><![CDATA[WXPAY20160318145827707]]></out_trade_no>
                        <result_code><![CDATA[SUCCESS]]></result_code>
                        <return_code><![CDATA[SUCCESS]]></return_code>
                        <sign><![CDATA[A33680B7A3FA2525D147E3E52D21C067]]></sign>
                        <time_end><![CDATA[20160318145921]]></time_end>
                        <total_fee>1</total_fee>
                        <trade_type><![CDATA[NATIVE]]></trade_type>
                        <transaction_id><![CDATA[1009200583201603184079405726]]></transaction_id>
                        </xml>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(a);

        
            const string itemNode = "xml";
            var root = xmlDoc.SelectSingleNode(itemNode);
            var roots = root.ChildNodes;

            foreach (var items in from XmlNode rows in roots select rows.ChildNodes)
            {
                //没有子节点的时候
                if (items != null)
                {
                    foreach (XmlNode row in items)
                    {
                        var innertext = row.InnerText;
                        var innerxml = row.ParentNode.LocalName;
                    }
                }
            }
        }

        #endregion

    }
}

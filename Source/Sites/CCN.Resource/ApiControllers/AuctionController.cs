using System.Web.Http;
using CCN.Modules.Auction.BusinessEntity;
using CCN.Modules.Auction.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using System.Collections.Generic;
using System;

namespace CCN.Resource.ApiControllers
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
        /// 获取拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAuctionList")]
        public BasePageList<AuctionCarInfoViewModel> GetAuctionList([FromBody] AuctionCarInfoQueryModel query)
        {
            return _auctionservice.GetAuctionList(query);
        }

        /// <summary>
        /// 获取拍卖车辆详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctionInfoById")]
        public JResult GetAuctionInfoById(string id)
        {
            return _auctionservice.GetAuctionInfoById(id);
        }

        /// <summary>
        /// 获取拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctionViewById")]
        public JResult GetAuctionViewById(string id)
        {
            return _auctionservice.GetAuctionViewById(id);
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
        public JResult AddAuctionCar([FromBody]AuctionCarInfoModel model)
        {
            return _auctionservice.AddAuctionCar(model);
        }

        /// <summary>
        /// 修改拍卖车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateAuctionCar")]
        public JResult UpdateAuctionCar([FromBody]AuctionCarInfoModel model)
        {
            return _auctionservice.UpdateAuctionCar(model);
        }

        /// <summary>
        /// 删除拍卖车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        [HttpPost]
        [Route("DeleteAuctionCar")]
        public JResult DeleteAuctionCar([FromBody]AuctionCarInfoModel model)
        {
            return _auctionservice.DeleteAuctionCar(model);
        }

        /// <summary>
        /// 发布拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        [HttpPost]
        [Route("PublishAuctionCar")]
        public JResult PublishAuctionCar([FromBody]AuctionCarInfoModel model)
        {
            return _auctionservice.PublishAuctionCar(model);
        }

        /// <summary>
        /// 成交拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        [HttpPost]
        [Route("DealAuctionCar")]
        public JResult DealAuctionCar([FromBody]AuctionCarInfoModel model)
        {
            return _auctionservice.DealAuctionCar(model);
        }
        
        #endregion
        
        #region 图片处理

        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCarPictureByCarid")]
        public JResult GetCarPictureByCarid(string carid)
        {
            return _auctionservice.GetCarPictureByCarid(carid);
        }

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveAuctionCarPicture")]
        public JResult SaveAuctionCarPicture([FromBody]AuctionPictureListModel model)
        {
            return _auctionservice.SaveAuctionCarPicture(model);
        }

        #endregion

        #region 竞拍

        /// <summary>
        /// 获取竞拍人列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAuctionParticipantList")]
        public BasePageList<AuctionCarParticipantViewModel> GetAuctionParticipantList([FromBody]AuctionCarParticipantQueryModel query)
        {
            return _auctionservice.GetAuctionParticipantList(query);
        }


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
        /// 更新竞价信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateParticipant")]
        public JResult UpdateParticipant([FromBody]AuctionCarParticipantModel model)
        {
            return _auctionservice.UpdateParticipant(model);
        }

        /// <summary>
        /// 中标拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BidParticipant")]
        public JResult BidParticipant([FromBody]AuctionCarParticipantModel model)
        {
            return _auctionservice.BidParticipant(model);
        }

        /// <summary>
        /// 根据ID获取出价详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctionParticipantByID")]
        public JResult GetAuctionParticipantByID(string innerid)
        {
            return _auctionservice.GetAuctionParticipantByID(innerid);
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

        #region 押金

        /// <summary>
        /// 获取押金列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAuctionDepositList")]
        public BasePageList<AuctionDepositViewModel> GetAuctionDepositList([FromBody]AuctionDepositQueryModel query)
        {
            return _auctionservice.GetAuctionDepositList(query);
        }

        /// <summary>
        /// 添加押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDeposit")]
        public JResult AddDeposit([FromBody]AuctionDepositModel model)
        {
            return _auctionservice.AddDeposit(model);
        }

        /// <summary>
        /// 修改押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateDeposit")]
        public JResult UpdateDeposit([FromBody]AuctionDepositModel model)
        {
            return _auctionservice.UpdateDeposit(model);
        }

        /// <summary>
        /// 获取押金详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDepositInfoById")]
        public JResult GetDepositInfoById(string innerid)
        {
            return _auctionservice.GetDepositInfoById(innerid);
        }

        /// <summary>
        /// 删除押金
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteDeposit")]
        public JResult DeleteDeposit(string innerid)
        {
            return _auctionservice.DeleteDeposit(innerid);
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
        /// 
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// 添加认证报告信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCarInspection")]
        public JResult AddCarInspection([FromBody]List<AuctionSaveCarInspectionModel> model)
        {
            return _auctionservice.AddCarInspection(model);
        }

        /// <summary>
        /// 修改认证报告信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("EditCarInspection")]
        public JResult EditCarInspection([FromBody]List<AuctionSaveCarInspectionModel> model)
        {
            return _auctionservice.EditCarInspection(model);
        }

        #endregion

    }
}

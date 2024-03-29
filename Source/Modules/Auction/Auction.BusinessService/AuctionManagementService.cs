﻿using System;
using CCN.Modules.Auction.BusinessComponent;
using CCN.Modules.Auction.BusinessEntity;
using CCN.Modules.Auction.Interface;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Cedar.Core.Logging;
using Newtonsoft.Json.Linq;

namespace CCN.Modules.Auction.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionManagementService : ServiceBase<AuctionBusinessComponent>, IAuctionManagementService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bc"></param>
        public AuctionManagementService(AuctionBusinessComponent bc) : base(bc)
        {

        }

        #region 拍卖车辆基本信息

        /// <summary>
        /// 获取正在拍卖的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetAuctioningList(AuctionCarInfoQueryModel query)
        {
            return BusinessComponent.GetAuctioningList(query);
        }

        /// <summary>
        /// 获取拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetAuctionList(AuctionCarInfoQueryModel query)
        {
            return BusinessComponent.GetAuctionList(query);
        }

        /// <summary>
        /// 获取拍卖车辆详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctionInfoById(string id)
        {
            return BusinessComponent.GetAuctionInfoById(id);
        }

        /// <summary>
        /// 获取拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctionViewById(string id)
        {
            return BusinessComponent.GetAuctionViewById(id);
        }

        /// <summary>
        /// 获取正在拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctioningViewById(string id)
        {
            return BusinessComponent.GetAuctioningViewById(id);
        }

        /// <summary>
        /// 添加拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddAuctionCar(AuctionCarInfoModel model)
        {
            return BusinessComponent.AddAuctionCar(model);
        }

        /// <summary>
        /// 修改拍卖车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult UpdateAuctionCar(AuctionCarInfoModel model)
        {
            return BusinessComponent.UpdateAuctionCar(model);
        }

        /// <summary>
        /// 修改拍卖车辆(状态)
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult UpdateAuctionCarStatus(AuctionCarInfoModel model)
        {
            return BusinessComponent.UpdateAuctionCarStatus(model);
        }

        /// <summary>
        /// 删除拍卖车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        public JResult DeleteAuctionCar(AuctionCarInfoModel model)
        {
            return BusinessComponent.DeleteAuctionCar(model);
        }

        /// <summary>
        /// 发布拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        public JResult PublishAuctionCar(AuctionCarInfoModel model)
        {
            return BusinessComponent.PublishAuctionCar(model);
        }

        /// <summary>
        /// 成交拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        public JResult DealAuctionCar(AuctionCarInfoModel model)
        {
            return BusinessComponent.DealAuctionCar(model);
        }

        #endregion

        #region 图片处理

        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        public JResult GetCarPictureByCarid(string carid)
        {
            return BusinessComponent.GetCarPictureByCarid(carid);
        }

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult SaveAuctionCarPicture(AuctionPictureListModel model)
        {
            return BusinessComponent.SaveAuctionCarPicture(model);
        }

        #endregion

        #region 竞拍

        /// <summary>
        /// 获取竞拍人列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarParticipantViewModel> GetAuctionParticipantList(AuctionCarParticipantQueryModel query)
        {
            return BusinessComponent.GetAuctionParticipantList(query);
        }


        /// <summary>
        /// 添加拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddParticipant(AuctionCarParticipantModel model)
        {
            return BusinessComponent.AddParticipant(model);
        }

        /// <summary>
        /// 更新竞价信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateParticipant(AuctionCarParticipantModel model)
        {
            return BusinessComponent.UpdateParticipant(model);
        }

        /// <summary>
        /// 根据ID获取出价详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetAuctionParticipantByID(string innerid)
        {
            return BusinessComponent.GetAuctionParticipantByID(innerid);
        }

        /// <summary>
        /// 中标拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult BidParticipant(AuctionCarParticipantModel model)
        {
            return BusinessComponent.BidParticipant(model);
        }

        /// <summary>
        ///  获取所有竞拍记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JResult GetAllAuctionParticipantList(AuctionCarParticipantQueryModel query)
        {
            return BusinessComponent.GetAllAuctionParticipantList(query);
        }

        /// <summary>
        /// 根据拍卖ID 获取竞拍记录
        /// </summary>
        /// <param name="auctionid"></param>
        /// <returns></returns>
        public JResult GetPriceCount(string auctionid)
        {
            return BusinessComponent.GetPriceCount(auctionid);
        }

        /// <summary>
        /// 支付完成更新出价状态
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public JResult UpdateStatusForPay(string orderno)
        {
            return BusinessComponent.UpdateStatusForPay(orderno);
        }

        #endregion

        #region 押金

        /// <summary>
        /// 获取押金列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionDepositViewModel> GetAuctionDepositList(AuctionDepositQueryModel query)
        {
            return BusinessComponent.GetAuctionDepositList(query);
        }

        /// <summary>
        /// 添加押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddDeposit(AuctionDepositModel model)
        {
            return BusinessComponent.AddDeposit(model);
        }

        /// <summary>
        /// 修改押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateDeposit(AuctionDepositModel model)
        {
            return BusinessComponent.UpdateDeposit(model);
        }

        /// <summary>
        /// 获取押金详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetDepositInfoById(string innerid)
        {
            return BusinessComponent.GetDepositInfoById(innerid);
        }

        /// <summary>
        /// 删除押金
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteDeposit(string innerid)
        {
            return BusinessComponent.DeleteDeposit(innerid);
        }

        #endregion

        #region 拍卖时间区间

        /// <summary>
        /// 获取拍卖时间列表
        /// </summary>
        /// <returns></returns>
        public JResult GetAuctionTimeList()
        {
            return BusinessComponent.GetAuctionTimeList();
        }

        #endregion

        #region 认证报告
        /// <summary>
        /// 获取认证项
        /// </summary>
        /// <returns></returns>
        public JResult AuctionCarInspectionItem()
        {
            return BusinessComponent.AuctionCarInspectionItem();
        }

        /// <summary>
        /// 根据拍卖ID获取拍卖报告内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctionCarInspectionResult(string id)
        {
            return BusinessComponent.GetAuctionCarInspectionResult(id);
        }

        /// <summary>
        /// 根据拍卖ID获取拍卖报告内容（前台使用）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetInspectionResultForHtml(string id)
        {
            return BusinessComponent.GetInspectionResultForHtml(id);
        }

        /// <summary>
        /// 添加认证报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarInspection(List<AuctionSaveCarInspectionModel> model)
        {
            return BusinessComponent.AddCarInspection(model);
        }

        /// <summary>
        /// 修改认证报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult EditCarInspection(List<AuctionSaveCarInspectionModel> model)
        {
            return BusinessComponent.EditCarInspection(model);
        }

        #endregion

        #region 关注

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult Follow(AuctionFollowModel model)
        {
            return BusinessComponent.Follow(model);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JResult Unfollow(string auctionid, string userid)
        {
            return BusinessComponent.Unfollow(auctionid, userid);
        }

        /// <summary>
        /// 判断用户是否关注了该拍卖车辆
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JResult IsFollow(string auctionid, string userid)
        {
            return BusinessComponent.IsFollow(auctionid, userid);
        }

        /// <summary>
        /// 获取关注的拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetFollowPageList(AuctionFollowQueryModel query)
        {
            return BusinessComponent.GetFollowPageList(query);
        }

        #endregion

        #region 支付相关

        /// <summary>
        /// 根据订单号获取出价详情
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public JResult GetAuctionParticipantByOrderNo(string orderno)
        {
            var result = BusinessComponent.GetAuctionParticipantByOrderNo(orderno);
            return result;
        }

        /// <summary>
        /// 添加定金拍卖定金支付记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddPaymentRecord(AuctionPaymentRecordModel model)
        {
            var result = BusinessComponent.AddPaymentRecord(model);
            return result;
        }

        /// <summary>
        /// 微信定金支付
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        public JResult WeChatPayForAuction(string innerid, string tradeType = "NATIVE")
        {
            return BusinessComponent.WeChatPayForAuction(innerid, tradeType);
        }

        #endregion
    }
}

using CCN.Modules.Auction.BusinessEntity;
using Cedar.Framework.Common.BaseClasses;
using System.Collections.Generic;

namespace CCN.Modules.Auction.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuctionManagementService
    {
        #region 拍卖车辆基本信息

        /// <summary>
        /// 获取正在拍卖的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<AuctionCarInfoViewModel> GetAuctioningList(AuctionCarInfoQueryModel query);

        /// <summary>
        /// 获取拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<AuctionCarInfoViewModel> GetAuctionList(AuctionCarInfoQueryModel query);

        /// <summary>
        /// 获取拍卖车辆详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JResult GetAuctionInfoById(string id);

        /// <summary>
        /// 获取拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JResult GetAuctionViewById(string id);

        /// <summary>
        /// 获取正在拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JResult GetAuctioningViewById(string id);

        /// <summary>
        /// 添加拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddAuctionCar(AuctionCarInfoModel model);

        /// <summary>
        /// 修改拍卖车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        JResult UpdateAuctionCar(AuctionCarInfoModel model);

        /// <summary>
        /// 删除拍卖车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        JResult DeleteAuctionCar(AuctionCarInfoModel model);

        /// <summary>
        /// 发布拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        JResult PublishAuctionCar(AuctionCarInfoModel model);

        /// <summary>
        /// 成交拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        JResult DealAuctionCar(AuctionCarInfoModel model);

        #endregion


        #region 图片处理

        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        JResult GetCarPictureByCarid(string carid);

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult SaveAuctionCarPicture(AuctionPictureListModel model);

        #endregion

        #region 竞拍

        /// <summary>
        /// 获取竞拍人列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<AuctionCarParticipantViewModel> GetAuctionParticipantList(AuctionCarParticipantQueryModel query);


        /// <summary>
        /// 添加拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddParticipant(AuctionCarParticipantModel model);

        /// <summary>
        /// 中标拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult BidParticipant(AuctionCarParticipantModel model);

        /// <summary>
        ///  获取所有竞拍记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        JResult GetAllAuctionParticipantList(AuctionCarParticipantQueryModel query);

        /// <summary>
        /// 根据拍卖ID 获取竞拍记录
        /// </summary>
        /// <param name="auctionid"></param>
        /// <returns></returns>
        JResult GetPriceCount(string auctionid);

        #endregion

        #region 押金

        /// <summary>
        /// 获取押金列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<AuctionDepositViewModel> GetAuctionDepositList(AuctionDepositQueryModel query);

        /// <summary>
        /// 添加押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddDeposit(AuctionDepositModel model);

        /// <summary>
        /// 修改押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateDeposit(AuctionDepositModel model);
        
        /// <summary>
        /// 获取押金详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetDepositInfoById(string innerid);

        /// <summary>
        /// 删除押金
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DeleteDeposit(string innerid);

        #endregion

        #region 拍卖时间区间

        /// <summary>
        /// 获取拍卖时间列表
        /// </summary>
        /// <returns></returns>
        JResult GetAuctionTimeList();

        #endregion

        #region 认证报告

        /// <summary>
        /// 获取认证项
        /// </summary>
        /// <returns></returns>
        JResult AuctionCarInspectionItem();

        /// <summary>
        /// 拍卖ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JResult GetAuctionCarInspectionResult(string id);

        /// <summary>
        /// 获取认证报告页面(前台使用)
        /// </summary>
        /// <param name="id">拍卖ID</param>
        /// <returns></returns>
        JResult GetInspectionResultForHtml(string id);

        /// <summary>
        /// 添加认证报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCarInspection(List<AuctionSaveCarInspectionModel> model);

        /// <summary>
        /// 修改认证报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult EditCarInspection(List<AuctionSaveCarInspectionModel> model);

        #endregion

        #region 关注

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult Follow(AuctionFollowModel model);

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        JResult Unfollow(string auctionid, string userid);

        /// <summary>
        /// 获取关注的拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<AuctionCarInfoViewModel> GetFollowPageList(AuctionFollowQueryModel query);
        
        #endregion
    }
}

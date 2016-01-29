using CCN.Modules.Auction.BusinessEntity;
using Cedar.Framework.Common.BaseClasses;

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
    }
}

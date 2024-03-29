﻿using System.Collections.Generic;
using CCN.Modules.Car.BusinessEntity;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Car.Interface
{
    /// <summary>
    /// 车辆接口
    /// </summary>
    public interface ICarManagementService
    {
        #region 车辆

        /// <summary>
        /// 全城搜车(官网页面)（查询到置顶车辆）
        /// </summary>
        /// <param name="query">查询条件
        /// query.Echo 用于第几次进入最后一页，补齐的时候就代表第几页
        /// </param>
        /// <returns></returns>
        BasePageList<CarInfoListViewModel> SearchCarPageListTop(CarGlobalQueryModel query);

        /// <summary>
        /// 全城搜车(官网页面)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarInfoListViewModel> SearchCarPageListEx(CarGlobalQueryModel query);
        
        /// <summary>
        /// 全城搜车列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarInfoListViewModel> SearchCarPageList(CarGlobalQueryModel query);

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarInfoListViewModel> GetCarPageList(CarQueryModel query);

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarInfoListViewModel> GetAllCarPageList(CarQueryModel query);

        /// <summary>
        /// 获取车辆详细信息(info)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        JResult GetCarInfoById(string id);

        /// <summary>
        /// 获取车辆详情(view)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        JResult GetCarViewById(string id);

        /// <summary>
        /// 根据车辆获取车辆信息
        /// </summary>
        /// <param name="carno"></param>
        /// <returns></returns>
        JResult GetCarInfoByNo(string carno);

        #region 感兴趣

        /// <summary>
        /// 获取感兴趣的车列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CarInfoListViewModel> GetInterestList(CarInterestQueryModel query);

        #endregion

        /// <summary>
        /// 车辆估值（根据城市，车型，时间）
        /// </summary>
        /// <param name="carInfo"></param>
        /// <returns></returns>
        JResult GetCarEvaluateByCar(CarEvaluateModel carInfo);

        /// <summary>
        /// 车辆估值
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        JResult GetCarEvaluateById(string id);

        /// <summary>
        /// 获取本月本车型成交数量
        /// </summary>
        /// <param name="modelid">车型id</param>
        /// <returns></returns>
        JResult GetCarSales(string modelid);

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        JResult AddCar(CarInfoModel model);

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        JResult UpdateCar(CarInfoModel model);

        /// <summary>
        /// 删除车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        JResult DeleteCar(CarInfoModel model);

        /// <summary>
        /// 回复车辆
        /// </summary>
        /// <param name="model">回复成交model</param>
        /// <returns>1.操作成功</returns>
        JResult RecoveryCar(CarInfoModel model);

        /// <summary>
        /// 车辆成交
        /// </summary>
        /// <param name="model">车辆成交model</param>
        /// <returns>1.操作成功</returns>
        JResult DealCar(CarInfoModel model);

        /// <summary>
        /// 删除车辆(物理删除，暂不用)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.删除成功</returns>
        JResult DeleteCar(string id);

        /// <summary>
        /// 车辆状态更新
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="status"></param>
        /// <returns>1.操作成功</returns>
        JResult UpdateCarStatus(string carid, int status);

        /// <summary>
        /// 车辆分享
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        JResult ShareCar(string id);

        /// <summary>
        /// 累计车辆查看次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="count">新增次数</param>
        /// <returns>1.累计成功</returns>
        JResult UpSeeCount(string id, int count);

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.累计成功</returns>
        JResult UpPraiseCount(string id);

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="content">评论内容</param>
        /// <returns>1.累计成功</returns>
        JResult CommentCar(string id, string content);

        /// <summary>
        /// 审核车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="status">审核状态</param>
        /// <returns>1.操作成功</returns>
        int AuditCar(string id, int status);

        /// <summary>
        /// 核销车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        int CancelCar(string id);

        /// <summary>
        /// 获取车辆 分享/查看次数
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        JResult GetCarShareInfo(string carid);

        /// <summary>
        /// 刷新车辆
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>1.操作成功</returns>
        JResult RefreshCar(string carid);

        /// <summary>
        /// 置顶车辆
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>1.操作成功</returns>
        JResult PushUpCar(string carid);

        /// <summary>
        /// 置顶或取消置顶
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="istop">1.置顶 0取消置顶</param>
        /// <returns>1.操作成功</returns>
        JResult DoTopCar(string carid, int istop);

        #endregion

        #region 车辆图片

        /// <summary>
        /// 添加车辆图片
        /// </summary>
        /// <param name="model">车辆图片信息</param>
        /// <returns></returns>
        JResult AddCarPicture(CarPictureModel model);
        
        /// <summary>
        /// 添加车辆图片
        /// </summary>
        /// <param name="innerid">车辆图片id</param>
        /// <returns></returns>
        JResult DeleteCarPicture(string innerid);

        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        JResult GetCarPictureByCarid(string carid);

        /// <summary>
        /// 图片调换位置
        /// </summary>
        /// <param name="listPicture">车辆图片列表</param>
        /// <returns></returns>
        JResult ExchangePictureSort(List<CarPictureModel> listPicture);

        /// <summary>
        /// 批量保存图片(删除)
        /// </summary>
        /// <param name="picModel"></param>
        /// <returns></returns>
        JResult DelCarPictureList(PictureDelListModel picModel);

        /// <summary>
        /// 批量添加车辆图片(添加)(后台)
        /// </summary>
        /// <param name="picModel">车辆图片信息</param>
        /// <returns></returns>
        JResult AddCarPictureList(PictureListModel picModel);

        /// <summary>
        /// 批量添加车辆图片(添加)(微信端使用)
        /// </summary>
        /// <param name="picModel">车辆图片信息</param>
        /// <returns></returns>
        JResult AddCarPictureList(WechatPictureModel picModel);

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult SaveCarPicture(BatchPictureListWeichatModel model);

        /// <summary>
        /// 批量保存图片(wechat webapp)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult BatchSaveCarPictureWechat(WechatPictureExModel model);

        /// <summary>
        /// 批量保存图片(通用，除微信端)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult BatchSaveCarPicture(BatchPictureListModel model);
        #endregion

        #region 车辆收藏

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCollection(CarCollectionModel model);

        /// <summary>
        /// 删除收藏 by innerid
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DeleteCollection(string innerid);

        /// <summary>
        /// 删除收藏 by carid
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        JResult DeleteCollectionByCarid(string carid);

        /// <summary>
        /// 获取收藏的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CarCollectionViewListModel> GetCollectionList(CarCollectionQueryModel query);

        #endregion

        #region 车辆举报

        /// <summary>
        /// 获取举报信息
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        JResult GetTipOffListByCarId(string carid);

        /// <summary>
        /// 添加举报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddTipOff(CarTipOffModel model);

        /// <summary>
        /// 获取举报
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CarTipOffModel> GetTipOffPageList(CarTipQueryModel query);

        /// <summary>
        /// 举报处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult HandleTipOff(CarTipHandleModel model);

        #endregion

        #region 车辆悬赏

        /// <summary>
        /// 车辆悬赏列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CarRewardViewModel> CarRewardPageList(CarRewardQueryModel query);

        /// <summary>
        /// 添加车辆悬赏信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCarReward(CarReward model);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="innerid">主键</param>
        /// <returns></returns>
        JResult UpdateCarRewardStatus(int status, string innerid);

        /// <summary>
        /// 车辆悬赏推荐
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarInfoListViewModel> GetCarRewardPageList(CarRewardQueryModel query);

        #endregion

        #region 会员车辆

        /// <summary>
        /// 根据手机号获取会员拥有的车辆
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        JResult GetCarInfoByMobile(string mobile);

        #endregion

        #region 车贷相关

        /// <summary>
        /// 获取贷款列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CarLoanViewModel> GetCarLoanList(CarLoanQueryModel query);

        /// <summary>
        /// 车贷申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCarLoan(CarLoanModel model);

        /// <summary>
        /// 车贷修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateCarLoan(CarLoanModel model);

        /// <summary>
        /// 根据ID获取贷款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JResult CarLoanInfo(string id);

        /// <summary>
        /// 添加贷款图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddLoanPicture(CarLoanPicture model);

        /// <summary>
        /// 根据贷款ID获取对应的图片
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        JResult GetLoanPictureByloanid(string id);

        /// <summary>
        /// 删除贷款图片
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DeleteLoanPicture(string innerid);

        #endregion

        #region 金融方案

        /// <summary>
        /// 获取金融方案列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<FinanceProgrammeViewModel> GetFinanceProgrammeList(FinanceProgrammeQueryModel query);

        /// <summary>
        /// 金融方案新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddFinanceProgramme(FinanceProgrammeModel model);

        /// <summary>
        /// 金融方案修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateFinanceProgramme(FinanceProgrammeModel model);

        /// <summary>
        /// 根据id获取金融方案详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetFinanceProgrammeById(string innerid);

        /// <summary>
        /// 经融方案明细新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddFinanceProgrammeDetail(FinanceProgrammeDetailModel model);

        /// <summary>
        /// 金融方案明细修改 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateFinanceProgrammeDetail(FinanceProgrammeDetailModel model);

        /// <summary>
        /// 根据id获取金融方案明细详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetFinanceProgrammeDetailById(string innerid);

        #endregion

        #region 供应商管理

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="model">供应商信息</param>
        /// <returns></returns>
        JResult AddSupplier(CarSupplierModel model);

        /// <summary>
        /// 修改供应商
        /// </summary>
        /// <param name="model">供应商信息</param>
        /// <returns></returns>
        JResult UpdateSupplier(CarSupplierModel model);

        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="innerid">供应商model</param>
        /// <returns>1.操作成功</returns>
        JResult DeleteSupplier(string innerid);

        /// <summary>
        /// 供应商列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarSupplierModel> GetSupplierCarPageList(CarSupplierQueryModel query);
        /// <summary>
        /// 获取会员所有供应商列表
        /// </summary>
        /// <returns></returns>
        JResult GetSupplierAll();

        /// <summary>
        /// 根据id获取供应商的信息
        /// </summary>
        /// <returns></returns>
        JResult GetSupplierInfoById(string innerid);

        #endregion

        #region 神秘车源

        /// <summary>
        /// 查询神秘车源列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarMysteriousListModel> GetMysteriousCarPageList(CarGlobalQueryModel query);

        /// <summary>
        /// 后台查询神秘车源列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarMysteriousListModel> GetMysteriousBackCarPageList(CarGlobalQueryModel query);

        /// <summary>
        /// 顶神秘车源
        /// </summary>
        /// <param name="innerid">车辆id</param>
        /// <returns>1.操作成功</returns>
        JResult PushUpMysteriousCar(string innerid);

        /// <summary>
        /// 微信定金支付
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult MysteriousUnifiedOrder(PayModel model);

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        JResult DoPay(string orderNo);

        /// <summary>
        /// 获取支付记录
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="custid"></param>
        /// <returns></returns>
        JResult GetPayRecordById(string carid, string custid);
        #endregion

        #region 劲爆车源

        /// <summary>
        /// 添加劲爆车源
        /// </summary>
        /// <param name="model">车源信息</param>
        /// <returns></returns>
        JResult AddMaddenCar(CarMaddenModel model);

        /// <summary>
        /// 修改劲爆车源
        /// </summary>
        /// <param name="model">车源信息</param>
        /// <returns></returns>
        JResult UpdateMaddenCar(CarMaddenModel model);

        /// <summary>
        /// 删除劲爆车源
        /// </summary>
        /// <param name="model">删除model</param>
        /// <returns>1.操作成功</returns>
        JResult DeleteMaddenCar(CarMaddenModel model);

        /// <summary>
        /// 回复劲爆车辆
        /// </summary>
        /// <param name="model">回复model</param>
        /// <returns>1.操作成功</returns>
        JResult RecoveryMaddenCar(CarMaddenModel model);

        /// <summary>
        /// 车辆劲爆成交
        /// </summary>
        /// <param name="model">车辆model</param>
        /// <returns>1.操作成功</returns>
        JResult DealMaddenCar(CarMaddenModel model);

        /// <summary>
        /// 顶劲爆车源
        /// </summary>
        /// <param name="innerid">车辆id</param>
        /// <returns>1.操作成功</returns>
        JResult PushUpMaddenCar(string innerid);

        /// <summary>
        /// 根据id获取劲爆车源的信息
        /// </summary>
        /// <returns></returns>
        JResult GetMaddenCarInfoById(string innerid);

        /// <summary>
        /// 根据id获取劲爆车源的信息
        /// </summary>
        /// <returns></returns>
        JResult GetMaddenCarViewById(string innerid);

        /// <summary>
        /// 查询劲爆车源列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarMaddenListModel> GetMaddenCarPageList(CarMaddenQueryModel query);

        /// <summary>
        /// 后台查询劲爆车源列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CarMaddenListModel> GetMaddenCarBackPageList(CarMaddenQueryModel query);

        #endregion
    }
}

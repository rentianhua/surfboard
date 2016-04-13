using System;
using System.Collections.Generic;
using System.Linq;
using CCN.Modules.Car.BusinessComponent;
using CCN.Modules.Car.BusinessEntity;
using CCN.Modules.Car.Interface;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Car.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class CarManagementService : ServiceBase<CarBC>, ICarManagementService
    {
        /// <summary>
        /// </summary>
        public CarManagementService(CarBC bc)
            : base(bc)
        {
        }

        #region 车辆

        /// <summary>
        /// 全城搜车(官网页面)（查询到置顶车辆）
        /// </summary>
        /// <param name="query">查询条件
        /// query.Echo 用于第几次进入最后一页，补齐的时候就代表第几页
        /// </param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageListTop(CarGlobalQueryModel query)
        {
            return BusinessComponent.SearchCarPageListTop(query);
        }

        /// <summary>
        /// 全城搜车(官网页面)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageListEx(CarGlobalQueryModel query)
        {
            return BusinessComponent.SearchCarPageListEx(query);
        }

        /// <summary>
        /// 全城搜车列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageList(CarGlobalQueryModel query)
        {
            return BusinessComponent.SearchCarPageList(query);
        }

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> GetCarPageList(CarQueryModel query)
        {
            return BusinessComponent.GetCarPageList(query);
        }

        /// <summary>
        /// 获取车辆详细信息(info)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        public JResult GetCarInfoById(string id)
        {
            return BusinessComponent.GetCarInfoById(id);
        }

        /// <summary>
        /// 获取车辆详情(view)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        public JResult GetCarViewById(string id)
        {
            return BusinessComponent.GetCarViewById(id);
        }

        #region 感兴趣

        /// <summary>
        /// 获取感兴趣的车列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> GetInterestList(CarInterestQueryModel query)
        {
            return BusinessComponent.GetInterestList(query);
        }

        #endregion

        /// <summary>
        /// 车辆估值（根据城市，车型，时间）
        /// </summary>
        /// <param name="carInfo">车辆id</param>
        /// <returns></returns>
        public JResult GetCarEvaluateByCar(CarEvaluateModel carInfo)
        {
            return BusinessComponent.GetCarEvaluateByCar(carInfo);
        }

        /// <summary>
        /// 车辆估值
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        public JResult GetCarEvaluateById(string id)
        {
            return BusinessComponent.GetCarEvaluateById(id);
        }

        /// <summary>
        /// 获取本月本车型成交数量
        /// </summary>
        /// <param name="modelid">车型id</param>
        /// <returns></returns>
        public JResult GetCarSales(string modelid)
        {
            return BusinessComponent.GetCarSales(modelid);
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult AddCar(CarInfoModel model)
        {
            return BusinessComponent.AddCar(model);
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult UpdateCar(CarInfoModel model)
        {
            return BusinessComponent.UpdateCar(model);
        }

        /// <summary>
        /// 删除车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        public JResult DeleteCar(CarInfoModel model)
        {
            return BusinessComponent.DeleteCar(model);
        }

        /// <summary>
        /// 车辆成交
        /// </summary>
        /// <param name="model">车辆成交model</param>
        /// <returns>1.操作成功</returns>
        public JResult DealCar(CarInfoModel model)
        {
            return BusinessComponent.DealCar(model);
        }

        /// <summary>
        /// 删除车辆(物理删除，暂不用)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.删除成功</returns>
        public JResult DeleteCar(string id)
        {
            return BusinessComponent.DeleteCar(id);
        }

        /// <summary>
        /// 车辆状态更新
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="status"></param>
        /// <returns>1.操作成功</returns>
        public JResult UpdateCarStatus(string carid, int status)
        {
            return BusinessComponent.UpdateCarStatus(carid, status);
        }

        /// <summary>
        /// 车辆分享
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public JResult ShareCar(string id)
        {
            return BusinessComponent.ShareCar(id);
        }

        /// <summary>
        /// 累计车辆查看次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="count">新增次数</param>
        /// <returns>1.累计成功</returns>
        public JResult UpSeeCount(string id, int count)
        {

            return BusinessComponent.UpSeeCount(id, count);
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.累计成功</returns>
        public JResult UpPraiseCount(string id)
        {
            return BusinessComponent.UpPraiseCount(id);
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="content">评论内容</param>
        /// <returns>1.累计成功</returns>
        public JResult CommentCar(string id, string content)
        {
            return BusinessComponent.CommentCar(id, content);
        }

        /// <summary>
        /// 审核车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="status">审核状态</param>
        /// <returns>1.操作成功</returns>
        public int AuditCar(string id, int status)
        {
            return BusinessComponent.AuditCar(id, status);
        }

        /// <summary>
        /// 核销车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public int CancelCar(string id)
        {
            return BusinessComponent.CancelCar(id);
        }

        /// <summary>
        /// 获取车辆 分享/查看次数
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        public JResult GetCarShareInfo(string carid)
        {
            return BusinessComponent.GetCarShareInfo(carid);
        }

        /// <summary>
        /// 刷新车辆
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>1.操作成功</returns>
        public JResult RefreshCar(string carid)
        {
            return BusinessComponent.RefreshCar(carid);
        }

        /// <summary>
        /// 置顶车辆
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>1.操作成功</returns>
        public JResult PushUpCar(string carid)
        {
            return BusinessComponent.PushUpCar(carid);
        }

        /// <summary>
        /// 置顶或取消置顶
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="istop">1.置顶 0取消置顶</param>
        /// <returns>1.操作成功</returns>
        public JResult DoTopCar(string carid, int istop)
        {
            return BusinessComponent.DoTopCar(carid, istop);
        }
        #endregion

        #region 车辆图片

        /// <summary>
        /// 添加车辆图片
        /// </summary>
        /// <param name="model">车辆图片信息</param>
        /// <returns></returns>
        public JResult AddCarPicture(CarPictureModel model)
        {
            //return BusinessComponent.AddCarPicture(model);
            return BusinessComponent.AddCarPictureEx(model);
        }

        /// <summary>
        /// 刪除车辆图片
        /// </summary>
        /// <param name="innerid">车辆图片id</param>
        /// <returns></returns>
        public JResult DeleteCarPicture(string innerid)
        {
            return BusinessComponent.DeleteCarPicture(innerid);
        }

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
        /// 图片调换位置
        /// </summary>
        /// <param name="listPicture">车辆图片列表</param>
        /// <returns></returns>
        public JResult ExchangePictureSort(List<CarPictureModel> listPicture)
        {
            return BusinessComponent.ExchangePictureSort(listPicture);
        }

        /// <summary>
        /// 批量保存图片(删除)
        /// </summary>
        /// <param name="picModel"></param>
        /// <returns></returns>
        public JResult DelCarPictureList(PictureDelListModel picModel)
        {
            return BusinessComponent.DelCarPictureList(picModel);
        }

        /// <summary>
        /// 批量添加车辆图片(添加)(后台)
        /// </summary>
        /// <param name="picModel">车辆图片信息</param>
        /// <returns></returns>
        public JResult AddCarPictureList(PictureListModel picModel)
        {
            return BusinessComponent.AddCarPictureList(picModel);
        }

        /// <summary>
        /// 批量添加车辆图片(添加)(微信端使用)
        /// </summary>
        /// <param name="picModel">车辆图片信息</param>
        /// <returns></returns>
        public JResult AddCarPictureList(WechatPictureModel picModel)
        {
            return BusinessComponent.AddCarPictureList(picModel);
        }

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult SaveCarPicture(BatchPictureListWeichatModel model)
        {
            return BusinessComponent.SaveCarPicture(model);
        }

        /// <summary>
        /// 批量保存图片(wechat webapp)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult BatchSaveCarPictureWechat(WechatPictureExModel model)
        {
            return BusinessComponent.BatchSaveCarPictureWechat(model);
        }

        /// <summary>
        /// 批量保存图片(通用，除微信端)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult BatchSaveCarPicture(BatchPictureListModel model)
        {
            return BusinessComponent.BatchSaveCarPicture(model);
        }

        #endregion

        #region 车辆收藏

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCollection(CarCollectionModel model)
        {
            return BusinessComponent.AddCollection(model);
        }

        /// <summary>
        /// 删除收藏 by innerid
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCollection(string innerid)
        {
            return BusinessComponent.DeleteCollection(innerid);
        }

        /// <summary>
        /// 删除收藏 by carid
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        public JResult DeleteCollectionByCarid(string carid)
        {
            return BusinessComponent.DeleteCollectionByCarid(carid);
        }

        /// <summary>
        /// 获取收藏的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarCollectionViewListModel> GetCollectionList(CarCollectionQueryModel query)
        {
            return BusinessComponent.GetCollectionList(query);
        }
        #endregion

        #region 车辆悬赏

        /// <summary>
        /// 车辆悬赏列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarRewardViewModel> CarRewardPageList(CarRewardQueryModel query)
        {
            return BusinessComponent.CarRewardPageList(query);
        }

        /// <summary>
        /// 添加车辆悬赏信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarReward(CarReward model)
        {
            return BusinessComponent.AddCarReward(model);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="innerid">主键</param>
        /// <returns></returns>
        public JResult UpdateCarRewardStatus(int status, string innerid)
        {
            return BusinessComponent.UpdateCarRewardStatus(status, innerid);
        }

        /// <summary>
        /// 车辆悬赏推荐
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> GetCarRewardPageList(CarRewardQueryModel query)
        {
            return BusinessComponent.GetCarRewardPageList(query);
        }

        #endregion

        #region 会员车辆

        /// <summary>
        /// 根据手机号获取会员拥有的车辆
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public JResult GetCarInfoByMobile(string mobile)
        {
            return BusinessComponent.GetCarInfoByMobile(mobile);
        }

        #endregion

        #region 车贷相关

        /// <summary>
        /// 获取贷款列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarLoanViewModel> GetCarLoanList(CarLoanQueryModel query)
        {
            return BusinessComponent.GetCarLoanList(query);
        }


        /// <summary>
        /// 车贷申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarLoan(CarLoanModel model)
        {
            return BusinessComponent.AddCarLoan(model);
        }

        /// <summary>
        /// 车贷修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCarLoan(CarLoanModel model)
        {
            return BusinessComponent.UpdateCarLoan(model);
        }

        /// <summary>
        /// 根据ID获取贷款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult CarLoanInfo(string id)
        {
            return BusinessComponent.CarLoanInfo(id);
        }

        /// <summary>
        /// 添加贷款图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddLoanPicture(CarLoanPicture model)
        {
            return BusinessComponent.AddLoanPicture(model);
        }

        /// <summary>
        /// 根据贷款ID获取对应的图片
        /// </summary>
        /// <param name="loanid">loanid</param>
        /// <returns></returns>
        public JResult GetLoanPictureByloanid(string loanid)
        {
            return BusinessComponent.GetLoanPictureByloanid(loanid);
        }

        /// <summary>
        /// 删除贷款图片
        /// </summary>
        /// <param name="innerid">贷款图片id</param>
        /// <returns></returns>
        public JResult DeleteLoanPicture(string innerid)
        {
            return BusinessComponent.GetLoanPictureByloanid(innerid);
        }

        #endregion

        #region 金融方案

        /// <summary>
        /// 获取金融方案列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<FinanceProgrammeViewModel> GetFinanceProgrammeList(FinanceProgrammeQueryModel query)
        {
            return BusinessComponent.GetFinanceProgrammeList(query);
        }

        /// <summary>
        /// 金融方案新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddFinanceProgramme(FinanceProgrammeModel model)
        {
            return BusinessComponent.AddFinanceProgramme(model);
        }

        /// <summary>
        /// 金融方案修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateFinanceProgramme(FinanceProgrammeModel model)
        {
            return BusinessComponent.UpdateFinanceProgramme(model);
        }

        /// <summary>
        /// 根据id获取金融方案详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetFinanceProgrammeById(string innerid)
        {
            return BusinessComponent.GetFinanceProgrammeById(innerid);
        }

        /// <summary>
        /// 经融方案明细新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddFinanceProgrammeDetail(FinanceProgrammeDetailModel model)
        {
            return BusinessComponent.AddFinanceProgrammeDetail(model);
        }

        /// <summary>
        /// 金融方案明细修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateFinanceProgrammeDetail(FinanceProgrammeDetailModel model)
        {
            return BusinessComponent.UpdateFinanceProgrammeDetail(model);
        }

        /// <summary>
        /// 根据id获取金融方案明细详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetFinanceProgrammeDetailById(string innerid)
        {
            return BusinessComponent.GetFinanceProgrammeDetailById(innerid);
        }

        #endregion

			

        #region 供应商管理

        /// <summary>
        /// 获取会员所有供应商列表
        /// </summary>
        /// <returns></returns>
        public JResult GetSupplierAll()
        {
            return BusinessComponent.GetSupplierAll();
        }

        /// <summary>
        /// 根据id获取供应商的信息
        /// </summary>
        /// <returns></returns>
        public JResult GetSupplierInfoById(string innerid)
        {
            return BusinessComponent.GetSupplierInfoById(innerid);
        }

        #endregion

        #region 神秘车源

        /// <summary>
        /// 查询神秘车源列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarMysteriousListModel> GetMysteriousCarPageList(CarGlobalQueryModel query)
        {
            return BusinessComponent.GetMysteriousCarPageList(query);
        }

        /// <summary>
        /// 后台查询神秘车源列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarMysteriousListModel> GetMysteriousBackCarPageList(CarGlobalQueryModel query)
        {
            return BusinessComponent.GetMysteriousBackCarPageList(query);
        }


        #endregion
    }
}

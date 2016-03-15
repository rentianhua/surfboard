using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Auction.BusinessEntity;
using CCN.Modules.Auction.DataAccess;
using Cedar.Core.ApplicationContexts;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Auction.BusinessComponent
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionBusinessComponent : BusinessComponentBase<AuctionDataAccess>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="da"></param>
        public AuctionBusinessComponent(AuctionDataAccess da) : base(da)
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
            return DataAccess.GetAuctioningList(query);
        }

        /// <summary>
        /// 获取拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetAuctionList(AuctionCarInfoQueryModel query)
        {
            return DataAccess.GetAuctionList(query);
        }

        /// <summary>
        /// 获取拍卖车辆详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctionInfoById(string id)
        {
            var model = DataAccess.GetAuctionInfoById(id);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 获取拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctionViewById(string id)
        {
            var model = DataAccess.GetAuctionViewById(id);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 获取正在拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctioningViewById(string id)
        {
            var model = DataAccess.GetAuctionViewById(id);
            if (model == null)
            {
                return JResult._jResult(400, "暂无数据");
            }
            model.mobile = null;
            return JResult._jResult(model);
        }

        /// <summary>
        /// 添加拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddAuctionCar(AuctionCarInfoModel model)
        {

            model.createrid = ApplicationContext.Current.UserId;
            model.createdtime = DateTime.Now;
            //model.modifierid = "";
            //model.modifiedtime = null;
            //model.publisherid = "";
            //model.publishedtime = null;
            //model.dealerid = "";
            //model.deletedtime = null;
            model.Innerid = Guid.NewGuid().ToString();
            //model.status = 1;
            var result = DataAccess.AddAuctionCar(model);
            return JResult._jResult
            (
                result > 0 ? 0 : 400,
                model.Innerid
            );
        }

        /// <summary>
        /// 修改拍卖车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult UpdateAuctionCar(AuctionCarInfoModel model)
        {
            //if (string.IsNullOrWhiteSpace(model?.Innerid) || model.model_id == null || model.colorid == null ||
            //    model.register_date == null || model.cityid == null || model.mileage == null)
            //{
            //    return JResult._jResult(401, "参数不完整");
            //}

            if (string.IsNullOrWhiteSpace(model?.Innerid))
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.createrid = "";
            model.createdtime = null;
            model.modifierid = ApplicationContext.Current.UserId;
            model.modifiedtime = DateTime.Now;
            model.publisherid = "";
            model.publishedtime = null;
            model.dealerid = "";
            model.deletedtime = null;
            var result = DataAccess.UpdateAuctionCar(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 删除拍卖车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        public JResult DeleteAuctionCar(AuctionCarInfoModel model)
        {
            model.deleterid = ApplicationContext.Current.UserId;
            model.deletedtime = DateTime.Now;
            var result = DataAccess.DeleteAuctionCar(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 发布拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        public JResult PublishAuctionCar(AuctionCarInfoModel model)
        {
            model.publisherid = ApplicationContext.Current.UserId;
            model.publishedtime = DateTime.Now;
            var result = DataAccess.PublishAuctionCar(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 成交拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        public JResult DealAuctionCar(AuctionCarInfoModel model)
        {
            if (model?.dealedprice == null)
            {
                return JResult._jResult(401, "参数不完整");
            }
            model.dealerid = ApplicationContext.Current.UserId;
            model.dealedtime = DateTime.Now;
            var result = DataAccess.DealAuctionCar(model);
            return JResult._jResult(result);
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
            var list = DataAccess.GetCarPictureByCarid(carid);
            return JResult._jResult(list);
        }

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult SaveAuctionCarPicture(AuctionPictureListModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Carid) || (model.DelIds.Count == 0 && model.AddPaths.Count == 0))
            {
                return JResult._jResult(401, "参数不完整");
            }

            var result = 0;
            //获取即将删除的图片
            List<AuctionCarPictureModel> picedList = null;

            //only delete
            if (model.DelIds.Count > 0 && model.AddPaths.Count == 0)
            {
                picedList = DataAccess.GetCarPictureByIds(model.DelIds).ToList();
                result = DataAccess.DelCarPictureList(model.DelIds, model.Carid);
            }
            //only add
            else if (model.DelIds.Count == 0 && model.AddPaths.Count > 0)
            {
                result = DataAccess.AddAuctionPictureList(model.AddPaths, model.Carid);
            }
            else if (model.DelIds.Count > 0 && model.AddPaths.Count > 0)
            {
                picedList = DataAccess.GetCarPictureByIds(model.DelIds).ToList();
                result = DataAccess.SaveCarPicture(model);
            }

            switch (result)
            {
                case 402:
                    return JResult._jResult(402, "图片数量不对");
                case 0:
                    return JResult._jResult(400, "批量删除图片失败");
            }

            //异步删除七牛上的图片
            Task.Run(() =>
            {
                if (picedList == null || !picedList.Any())
                    return;

                var qiniu = new QiniuUtility();
                foreach (var item in picedList.Where(item => !string.IsNullOrWhiteSpace(item?.Path)))
                {
                    qiniu.DeleteFile(item.Path);
                }
            });

            return JResult._jResult(0, "批量操作图片成功");
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
            return DataAccess.GetAuctionParticipantList(query);
        }


        /// <summary>
        /// 添加拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddParticipant(AuctionCarParticipantModel model)
        {
            if (model?.Mobile == null || model.Amount == null || model.Auctionid == null)
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Createrid = ApplicationContext.Current.UserId;
            model.Createdtime = DateTime.Now;
            model.Modifierid = "";
            model.Modifiedtime = null;
            model.Innerid = Guid.NewGuid().ToString();
            var result = DataAccess.AddParticipant(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 中标拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult BidParticipant(AuctionCarParticipantModel model)
        {
            if (model.Isbid == 1)
            {
                var chkRes = DataAccess.CheckBidParticipant(model.Auctionid);
                if (chkRes > 0)
                {
                    return JResult._jResult(401, "只能一人中标");
                }
            }

            var bidModel = new AuctionCarParticipantModel
            {
                Innerid = model.Innerid,
                Modifierid = ApplicationContext.Current.UserId,
                Modifiedtime = DateTime.Now,
                Isbid = model.Isbid
            };
            var result = DataAccess.BidParticipant(bidModel);
            return JResult._jResult(result);
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
            return DataAccess.GetAuctionDepositList(query);
        }

        /// <summary>
        /// 添加押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddDeposit(AuctionDepositModel model)
        {
            if (model?.Mobile == null || model.Dpsamount == null || model.Auctionid == null)
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Createrid = ApplicationContext.Current.UserId;
            model.Createdtime = DateTime.Now;
            model.Modifierid = "";
            model.Modifiedtime = null;
            model.Innerid = Guid.NewGuid().ToString();
            var result = DataAccess.AddDeposit(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateDeposit(AuctionDepositModel model)
        {
            if (model?.Innerid == null)
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Modifierid = ApplicationContext.Current.UserId;
            model.Modifiedtime = DateTime.Now;
            model.Createrid = "";
            model.Createdtime = null;
            var result = DataAccess.UpdateDeposit(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 获取押金详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetDepositInfoById(string innerid)
        {
            var result = DataAccess.GetDepositInfoById(innerid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 删除押金
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteDeposit(string innerid)
        {
            var result = DataAccess.DeleteDeposit(innerid);
            return JResult._jResult(result);
        }

        #endregion

        #region 拍卖时间区间

        /// <summary>
        /// 获取拍卖时间列表
        /// </summary>
        /// <returns></returns>
        public JResult GetAuctionTimeList()
        {
            var result = DataAccess.GetAuctionTimeList();
            return JResult._jResult(result);
        }

        #endregion

        #region 认证报告

        /// <summary>
        /// 获取认证项
        /// </summary>
        /// <returns></returns>
        public JResult AuctionCarInspectionItem()
        {
            var result = DataAccess.AuctionCarInspectionItem();
            return JResult._jResult(result);
        }

        #endregion

    }
}

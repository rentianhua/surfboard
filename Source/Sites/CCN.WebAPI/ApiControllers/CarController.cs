using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CCN.Modules.Car.BusinessEntity;
using CCN.Modules.Car.Interface;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using CCN.Modules.Rewards.BusinessEntity;
using CCN.Modules.Rewards.Interface;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.IoC;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Newtonsoft.Json;
using WebGrease;

namespace CCN.WebAPI.ApiControllers
{
    /// <summary>
    /// 车辆管理
    /// </summary>
    [RoutePrefix("api/Car")]
    public class CarController : ApiController
    {
        private readonly ICarManagementService _baseservice;

        public CarController()
        {
            _baseservice = ServiceLocatorFactory.GetServiceLocator().GetService<ICarManagementService>();
        }

        #region 车辆基本信息

        /// <summary>
        /// 全城搜车列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("SearchCarPageList")]
        [HttpPost]
        public BasePageList<CarInfoListViewModel> SearchCarPageList([FromBody] CarGlobalQueryModel query)
        {
            return _baseservice.SearchCarPageList(query);
        }

        /// <summary>
        /// 分页查询车辆
        /// </summary>
        /// <param name="query">车辆信息</param>
        [Route("GetCarPageList")]
        [HttpPost]
        public BasePageList<CarInfoListViewModel> GetCarPageList([FromBody] CarQueryModel query)
        {
            return _baseservice.GetCarPageList(query);
        }

        /// <summary>
        /// 获取车辆详细信息(info)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        [Route("GetCarInfoById")]
        [HttpGet]
        public JResult GetCarInfoById(string id)
        {
            return _baseservice.GetCarInfoById(id);
        }

        /// <summary>
        /// 获取车辆详情(view)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        [Route("GetCarViewById")]
        [HttpGet]
        public JResult GetCarViewById(string id)
        {
            return _baseservice.GetCarViewById(id);
        }

        /// <summary>
        /// 车辆估值 （根据城市，车型，时间）
        /// </summary>
        /// <param name="carinfo">车辆信息</param>
        /// <returns></returns>
        [Route("GetCarEvaluateByCar")]
        [HttpPost]
        public JResult GetCarEvaluateByCar([FromBody] CarEvaluateModel carinfo)
        {
            return _baseservice.GetCarEvaluateByCar(carinfo); ;
        }

        /// <summary>
        /// 车辆估值
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        [Route("GetCarEvaluateById")]
        [HttpGet]
        public JResult GetCarEvaluateById(string id)
        {
            return _baseservice.GetCarEvaluateById(id);
        }

        /// <summary>
        /// 获取本月本车型成交数量
        /// </summary>
        /// <param name="modelid">车型id</param>
        /// <returns></returns>
        [Route("GetCarSales")]
        [HttpGet]
        public JResult GetCarSales(string modelid)
        {
            return _baseservice.GetCarSales(modelid);
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        [Route("AddCar")]
        [HttpPost]
        public JResult AddCar([FromBody] CarInfoModel model)
        {
            var result = _baseservice.AddCar(model);

            #region 车辆录入送积分

            if (result.errcode == 0)
            {
                Task.Run(() =>
                {
                    var rewardsservice = ServiceLocatorFactory.GetServiceLocator().GetService<IRewardsManagementService>();
                    rewardsservice.ChangePoint(new CustPointModel
                    {
                        Custid = model.custid,
                        Type = 1,
                        Point = 10,
                        Remark = "车辆录入送积分",
                        Sourceid = 4
                    });
                });
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 修改车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        [Route("UpdateCar")]
        [HttpPost]
        public JResult UpdateCar([FromBody] CarInfoModel model)
        {
            return _baseservice.UpdateCar(model);
        }

        /// <summary>
        /// 删除车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        [Route("DeleteCar")]
        [HttpPost]
        public JResult DeleteCar([FromBody] CarInfoModel model)
        {
            var result = _baseservice.DeleteCar(model);

            #region 车辆删除扣除积分

            if (result.errcode == 0)
            {
                Task.Run(() =>
                {
                    //获取会员id
                    var custid = ApplicationContext.Current.UserId;
                    if (string.IsNullOrWhiteSpace(custid))
                    {
                        LoggerFactories.CreateLogger().Write("车辆删除扣除积分:会员id空", TraceEventType.Warning);
                        return;
                    }
                    
                    var rewardsservice = ServiceLocatorFactory.GetServiceLocator().GetService<IRewardsManagementService>();
                    rewardsservice.ChangePoint(new CustPointModel
                    {
                        Custid = custid,
                        Type = 2, //扣除
                        Point = 10,
                        Remark = "车辆删除扣除积分",
                        Sourceid = 101
                    });
                });
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 车辆成交
        /// </summary>
        /// <param name="model">车辆成交model</param>
        /// <returns>1.操作成功</returns>
        [Route("DealCar")]
        [HttpPost]
        public JResult DealCar([FromBody] CarInfoModel model)
        {
            var result = _baseservice.DealCar(model);

            #region 车辆结案送积分

            if (result.errcode == 0)
            {
                Task.Run(() =>
                {
                    //获取会员id
                    var custid = ApplicationContext.Current.UserId;
                    if (string.IsNullOrWhiteSpace(custid))
                    {
                        LoggerFactories.CreateLogger().Write("车辆结案送积分:会员id空", TraceEventType.Warning);
                        return;
                    }

                    var rewardsservice = ServiceLocatorFactory.GetServiceLocator().GetService<IRewardsManagementService>();
                    
                    rewardsservice.ChangePoint(new CustPointModel
                    {
                        Custid = custid,
                        Type = 1,
                        Point = 1000,
                        Remark = "车辆结案送积分",
                        Sourceid = 6
                    });

                    //结案送礼券
                    //rewardsservice.SendCoupon(new SendCouponModel()
                    //{
                    //    Custid = custid,
                    //    ActionType = 2,
                    //    Sourceid = 4
                    //});
                });
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 删除车辆(物理删除，暂不用)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.删除成功</returns>
        [Route("DeleteCar")]
        [HttpDelete]
        [NonAction]
        public JResult DeleteCar(string id)
        {
            return _baseservice.DeleteCar(id);
        }

        /// <summary>
        /// 车辆状态更新(废弃)
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="status"></param>
        /// <returns>1.操作成功</returns>
        [Route("UpdateCarStatus")]
        [HttpGet]
        [NonAction]
        public JResult UpdateCarStatus(string carid, int status)
        {
            return _baseservice.UpdateCarStatus(carid, status);
        }

        /// <summary>
        /// 车辆分享
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        [Route("ShareCar")]
        [HttpGet]
        public JResult ShareCar(string id)
        {
            var result = _baseservice.ShareCar(id);

            #region 分享送积分

            if (result.errcode == 0)
            {
                Task.Run(() =>
                {
                    //获取会员id
                    var custid = ApplicationContext.Current.UserId;

                    if (string.IsNullOrWhiteSpace(custid))
                    {
                        LoggerFactories.CreateLogger().Write("分享送积分:会员id空", TraceEventType.Warning);
                        return;
                    }

                    var rewardsservice = ServiceLocatorFactory.GetServiceLocator().GetService<IRewardsManagementService>();

                    //会员每分享一次有效车辆信息至朋友圈奖励10积分，每个会员每天最多由此获得150积分
                    rewardsservice.GetSharePointRecord(custid);

                    rewardsservice.ChangePoint(new CustPointModel
                    {
                        Custid = custid,
                        Type = 1,
                        Point = 10,
                        Remark = "分享送积分",
                        Sourceid = 5
                    });
                });
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 累计车辆查看次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="count">新增次数</param>
        /// <returns>1.累计成功</returns>
        [Route("UpSeeCount")]
        [HttpGet]
        public JResult UpSeeCount(string id, int count = 1)
        {
            return _baseservice.UpSeeCount(id, count);
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.累计成功</returns>
        [Route("UpPraiseCount")]
        [HttpGet]
        public JResult UpPraiseCount(string id)
        {
            return _baseservice.UpPraiseCount(id);
        }

        /// <summary>
        /// 累计评论次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="content">评论内容</param>
        /// <returns>1.累计成功</returns>
        [Route("CommentCar")]
        [HttpGet]
        public JResult CommentCar(string id, string content = "")
        {
            return _baseservice.CommentCar(id, content);
        }

        /// <summary>
        /// 获取车辆 分享/查看次数
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        [Route("GetCarShareInfo")]
        [HttpGet]
        public JResult GetCarShareInfo(string carid)
        {
            return _baseservice.GetCarShareInfo(carid);
        }
        #endregion

        #region 车辆图片

        /// <summary>
        /// 添加车辆图片
        /// </summary>
        /// <param name="model">车辆图片信息</param>
        /// <returns></returns>
        [Route("AddCarPicture")]
        [HttpPost]
        public JResult AddCarPicture([FromBody] CarPictureModel model)
        {
            return _baseservice.AddCarPicture(model);
        }

        [Route("AddCarPictureList")]
        [HttpPost]
        public JResult AddCarPictureList([FromBody] WeichatPictureModel picModel)
        {
            Task.Run(() =>
            {
                try
                {
                    var jreult = _baseservice.AddCarPictureList(picModel);
                    //LoggerFactories.CreateLogger().Write("批量添加微信图片：" + jreult.errcode, TraceEventType.Information);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("批量添加微信图片异常：" + ex.Message, TraceEventType.Information);
                }
            });

            return JResult._jResult(0,"");
        }

        [Route("AddCarPictureKeyList")]
        [HttpPost]
        public JResult AddCarPictureList([FromBody] PictureListModel picModel)
        {
            return _baseservice.AddCarPictureList(picModel);
        }

        /// <summary>
        /// 添加车辆图片
        /// </summary>
        /// <param name="innerid">车辆图片id</param>
        /// <returns></returns>
        [Route("DeleteCarPicture")]
        [HttpDelete]
        public JResult DeleteCarPicture(string innerid)
        {
            return _baseservice.DeleteCarPicture(innerid);
        }

        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        [Route("GetCarPictureByCarid")]
        [HttpGet]
        public JResult GetCarPictureByCarid(string carid)
        {
            return _baseservice.GetCarPictureByCarid(carid);
        }

        /// <summary>
        /// 图片调换位置
        /// </summary>
        /// <param name="listPicture">车辆图片列表</param>
        /// <returns></returns>
        [Route("ExchangePictureSort")]
        [HttpPost]
        public JResult ExchangePictureSort([FromBody] List<CarPictureModel> listPicture)
        {
            return _baseservice.ExchangePictureSort(listPicture);
        }

        #endregion

        #region 车辆收藏

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddCollection")]
        [HttpPost]
        public JResult AddCollection([FromBody] CarCollectionModel model)
        {
            return _baseservice.AddCollection(model);
        }

        /// <summary>
        /// 删除收藏 by innerid
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("DeleteCollection")]
        [HttpDelete]
        public JResult DeleteCollection(string innerid)
        {
            return _baseservice.DeleteCollection(innerid);
        }

        /// <summary>
        /// 删除收藏 by carid
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        [Route("DeleteCollectionByCarid")]
        [HttpDelete]
        public JResult DeleteCollectionByCarid(string carid)
        {
            return _baseservice.DeleteCollectionByCarid(carid);
        }

        /// <summary>
        /// 获取收藏的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetCollectionList")]
        [HttpPost]
        public BasePageList<CarCollectionViewListModel> GetCollectionList(CarCollectionQueryModel query)
        {
            return _baseservice.GetCollectionList(query);
        }


        #endregion
    }
}

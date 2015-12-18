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

namespace CCN.Resource.ApiControllers
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
            return _baseservice.AddCar(model);
        }

        /// <summary>
        /// 后台添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        [Route("AddCarBack")]
        [HttpPost]
        public JResult AddCarBack([FromBody] CarInfoModel model)
        {
            var jresult = _baseservice.AddCar(model);

            if (jresult.errcode == 0)
            {
                Task.Factory.StartNew(() =>
                {
                    ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>().UpdateCustType(model.custid);
                });
            }

            return jresult;
        }

        /// <summary>
        /// 快速录车
        /// </summary>
        /// <param name="model">车辆信息</param>
        [Route("AddCarFast")]
        [HttpPost]
        public JResult AddCarFast([FromBody] CarInfoFastModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.mobile))
            {
                return JResult._jResult(401, "参数不完整");
            }

            //判断是否登录
            if (string.IsNullOrWhiteSpace(model.custid)) //没有登录
            {
                var custservice = ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();

                //用手机号获取会员信息
                var custinfo = (CustModel) custservice.GetCustByMobile(model.mobile).errmsg;
                if (custinfo == null) //会员不存在
                {
                    //自动注册
                    custinfo = (CustModel) custservice.CustRegister(new CustModel
                    {
                        Mobile = model.mobile,
                        Password = string.Concat("ccn", model.mobile),
                        Type = 2 //快速录车时自动注册也默认个人
                    }).errmsg;

                    if (custinfo == null)
                    {
                        return JResult._jResult(500, "自动注册失败");
                    }
                    model.custid = custinfo.Innerid;
                    return _baseservice.AddCar(model);
                }

                model.custid = custinfo.Innerid;
            }

            //添加车辆
            var jresult = _baseservice.AddCar(model);

            if (jresult.errcode == 0)
            {
                Task.Factory.StartNew(() =>
                {
                    var custservice = ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();
                    custservice.UpdateCustType(model.custid);
                });
            }

            return jresult;
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
            return _baseservice.DeleteCar(model);
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
            return _baseservice.DealCar(model);
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

            #region 注册送积分

            if (result.errcode == 0)
            {
                Task.Factory.StartNew(() =>
                {
                    var rewardsservice = ServiceLocatorFactory.GetServiceLocator().GetService<IRewardsManagementService>();

                    //获取会员id
                    var custid = ApplicationContext.Current.UserId;

                    //会员每分享一次有效车辆信息至朋友圈奖励10积分，每个会员每天最多由此获得150积分
                    //////////////////////////
                    rewardsservice.ChangePoint(new CustPointModel
                    {
                        Custid = custid,
                        Createdtime = DateTime.Now,
                        Type = 1,
                        Innerid = Guid.NewGuid().ToString(),
                        Point = 10,
                        Remark = "",
                        Sourceid = 5,
                        Validtime = null
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

        /// <summary>
        /// 批量保存图片(删除)
        /// </summary>
        /// <param name="picModel"></param>
        /// <returns></returns>
        [Route("DelCarPictureList")]
        [HttpPost]
        public JResult DelCarPictureList([FromBody]PictureDelListModel picModel)
        {
            LoggerFactories.CreateLogger().Write("批量删除图片参数：" + JsonConvert.SerializeObject(picModel), TraceEventType.Information);
            return _baseservice.DelCarPictureList(picModel);
        }

        /// <summary>
        /// 批量添加车辆图片(添加)(后台)
        /// </summary>
        /// <param name="picModel">车辆图片信息</param>
        /// <returns></returns>
        [Route("AddCarPictureKeyList")]
        [HttpPost]
        public JResult AddCarPictureList([FromBody]PictureListModel picModel)
        {
            return _baseservice.AddCarPictureList(picModel);
        }

        /// <summary>
        /// 批量添加车辆图片(添加)(微信端使用)
        /// </summary>
        /// <param name="picModel">车辆图片信息</param>
        /// <returns></returns>
        [Route("AddCarPictureList")]
        [HttpPost]
        public JResult AddCarPictureList([FromBody]WechatPictureModel picModel)
        {
            LoggerFactories.CreateLogger().Write("批量添加图片参数：" + JsonConvert.SerializeObject(picModel), TraceEventType.Information);
            return _baseservice.AddCarPictureList(picModel);
        }

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="picModel"></param>
        /// <returns></returns>
        [Route("SaveCarPicture")]
        [HttpPost]
        public JResult SaveCarPicture([FromBody] BatchPictureListWeichatModel picModel)
        {
            LoggerFactories.CreateLogger().Write("批量添加+删除图片参数：" + JsonConvert.SerializeObject(picModel), TraceEventType.Information);
            return _baseservice.SaveCarPicture(picModel);
        }
        #endregion
    }
}

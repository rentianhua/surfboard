using System;
using System.Collections.Generic;
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
using Cedar.Framework.Common.BaseClasses;

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
        public JResult GetCarEvaluateByCar([FromBody] CarInfoModel carinfo)
        {
            var jResult = new JResult();
            //获取5万公里估价
            carinfo.mileage = 5;
            var jResult5 = _baseservice.GetCarEvaluateByCar(carinfo);
            //获取10万公里估价
            carinfo.mileage = 10;
            var jResult10 = _baseservice.GetCarEvaluateByCar(carinfo);
            if (jResult5.errcode == 0 || jResult10.errcode == 0)
            {
                jResult.errcode = 0;
                if (jResult5.errcode == 0 && jResult10.errcode == 0)
                {
                    jResult.errmsg = Math.Round((Convert.ToDouble(jResult5.errmsg.ToString()) + Convert.ToDouble(jResult10.errmsg.ToString())) / 2, 2).ToString();
                }
                else if (jResult5.errcode == 0 && jResult10.errcode > 0)
                {
                    jResult.errmsg = Math.Round(Convert.ToDouble(jResult5.errmsg.ToString()), 2).ToString();
                }
                else if (jResult5.errcode > 0 && jResult10.errcode == 0)
                {
                    jResult.errmsg = Math.Round(Convert.ToDouble(jResult10.errmsg.ToString()), 2).ToString();
                }
            }
            else
            {
                jResult.errcode = 400;
                jResult.errmsg = "没有车辆评估信息";
            }
            return jResult;
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

                    rewardsservice.ChangePoint(new CustPointModel
                    {
                        Custid = custid,
                        Createdtime = DateTime.Now,
                        Type = 1,
                        Innerid = Guid.NewGuid().ToString(),
                        Point = 10,
                        Remark = "",
                        Sourceid = 1,
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
        /// <returns>1.累计成功</returns>
        [Route("UpSeeCount")]
        [HttpGet]
        public JResult UpSeeCount(string id)
        {
            return _baseservice.UpSeeCount(id);
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

        #endregion
    }
}

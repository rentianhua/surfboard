﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCN.Modules.Car.BusinessEntity;
using CCN.Modules.Car.Interface;
using CCN.Modules.Customer.Interface;
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
        public BasePageList<CarInfoModel> GetCarPageList([FromBody] CarQueryModel query)
        {
            return _baseservice.GetCarPageList(query);
        }

        /// <summary>
        /// 获取车辆详情
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
        /// 删除车辆(物理删除，暂不用)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.删除成功</returns>
        [Route("DeleteCar")]
        [HttpDelete]
        public JResult DeleteCar(string id)
        {
            return _baseservice.DeleteCar(id);
        }

        /// <summary>
        /// 车辆状态更新
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="status"></param>
        /// <returns>1.操作成功</returns>
        [Route("UpdateCarStatus")]
        [HttpGet]
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
            return _baseservice.ShareCar(id);
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
        
        #endregion
    }
}

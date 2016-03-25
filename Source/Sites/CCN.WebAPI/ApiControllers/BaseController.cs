using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CCN.Modules.Base.BusinessEntity;
using CCN.Modules.Base.Interface;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.IoC;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Client.DelegationHandler;
using Newtonsoft.Json;
using WebGrease;

namespace CCN.WebAPI.ApiControllers
{
    /// <summary>
    /// 基础模块
    /// </summary>
    [RoutePrefix("api/Base")]
    public class BaseController : ApiController
    {
        private readonly IBaseManagementService _baseservice;

        public BaseController()
        {
            _baseservice = ServiceLocatorFactory.GetServiceLocator().GetService<IBaseManagementService>();
        }

        #region Code

        /// <summary>
        /// 获取代码值列表
        /// </summary>
        /// <param name="typekey">代码类型key</param>
        /// <returns></returns>
        [Route("GetCodeByTypeKey")]
        [HttpGet]
        public JResult GetCodeByTypeKey(string typekey)
        {
            return _baseservice.GetCodeByTypeKey(typekey);
        }
       
        #endregion

        #region 验证码

        /// <summary>
        /// 手机获取验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("SendVerification")]
        [HttpPost]
        public JResult SendVerification([FromBody] BaseVerification model)
        {
            return _baseservice.SendVerification(model);
        }

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CheckVerification")]
        [HttpPost]
        public JResult CheckVerification([FromBody] BaseVerification model)
        {
            return _baseservice.CheckVerification(model.Target, model.Vcode, model.UType);
        }

        #endregion

        #region 区域

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial"></param>
        /// <returns></returns>
        [Route("GetProvList")]
        [HttpGet]
        public JResult GetProvList(string initial = null)
        {
            var list = _baseservice.GetProvList(initial);
            if (list.Any())
            {
                return new JResult
                {
                    errcode = 0,
                    errmsg = list
                };
            }
            return new JResult
            {
                errcode = 401,
                errmsg = "No Data"
            };
        }

        /// <summary>
        /// 根据省份id获取城市
        /// </summary>
        /// <param name="provid">省份id</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        [Route("GetCityList")]
        [HttpGet]
        public JResult GetCityList(int provid = 0, string initial = null)
        {
            var list = _baseservice.GetCityList(provid, initial);
            if (list.Any())
            {
                return new JResult
                {
                    errcode = 0,
                    errmsg = list
                };
            }
            return new JResult
            {
                errcode = 401,
                errmsg = "No Data"
            };
        }

        /// <summary>
        /// 根据省份获取区县
        /// </summary>
        /// <param name="cityId"> 城市ID</param>
        /// <returns></returns>
        [Route("GetCountyList")]
        [HttpGet]
        public JResult GetCountyList(int cityId = 0)
        {
            var list = _baseservice.GetCountyList(cityId);
            if (list.Any())
            {
                return new JResult
                {
                    errcode = 0,
                    errmsg = list
                };
            }
            return new JResult
            {
                errcode = 401,
                errmsg = "No Data"
            };
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial"></param>
        /// <returns></returns>
        [Route("GetProvListEx")]
        [HttpGet]
        public JResult GetProvListEx(string initial = null)
        {
            return _baseservice.GetProvListEx(initial);
        }

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        [Route("GetAllCity")]
        [HttpGet]
        public JResult GetAllCity()
        {
            BaseDepartmentModel model = new BaseDepartmentModel();
            return _baseservice.GetAllDepartment(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetTotalAreaList")]
        [HttpGet]
        public List<BaseProvinceAll> GetTotalAreaList()
        {
            return _baseservice.GetTotalAreaList();
        }
        #endregion

        #region 品牌/车系/车型

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        [Route("GetCarBrand")]
        [HttpGet]
        public JResult GetCarBrand(string initial = null)
        {
            var list = _baseservice.GetCarBrand(initial);
            if (list.Any())
            {
                return new JResult
                {
                    errcode = 0,
                    errmsg = list
                };
            }
            return new JResult
            {
                errcode = 401,
                errmsg = "No Data"
            };
        }

        /// <summary>
        /// 获取品牌热度Top n
        /// </summary>
        /// <returns></returns>
        [Route("GetCarBrandHotTop")]
        [HttpGet]
        public JResult GetCarBrandHotTop(int top = 5)
        {
            return _baseservice.GetCarBrandHotTop(top);
        }

        /// <summary>
        /// 根据品牌id获取车系
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <returns></returns>
        [Route("GetCarSeries")]
        [HttpGet]
        public JResult GetCarSeries(int brandId = 0)
        {
            var list = _baseservice.GetCarSeries(brandId);
            if (list.Any())
            {
                return new JResult
                {
                    errcode = 0,
                    errmsg = list
                };
            }
            return new JResult
            {
                errcode = 401,
                errmsg = "No Data"
            };
        }
        
        /// <summary>
        /// 获取热门车系Top n
        /// </summary>
        /// <returns></returns>
        [Route("GetCarSeriesHotTop")]
        [HttpGet]
        public JResult GetCarSeriesHotTop(int top = 10)
        {
            return _baseservice.GetCarSeriesHotTop(top);
        }


        /// <summary>
        /// 根据车系ID获取车型
        /// </summary>
        /// <param name="seriesId">车系id</param>
        /// <returns></returns>
        [Route("GetCarModel")]
        [HttpGet]
        public JResult GetCarModel(int seriesId = 0)
        {
            var list = _baseservice.GetCarModel(seriesId);
            if (list.Any())
            {
                return new JResult
                {
                    errcode = 0,
                    errmsg = list
                };
            }
            return new JResult
            {
                errcode = 401,
                errmsg = "No Data"
            };
        }

        /// <summary>
        /// 根据ID获取车型信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        [Route("GetCarModelById")]
        [HttpGet]
        public JResult GetCarModelById(int innerid = 0)
        {
            return _baseservice.GetCarModelById(innerid);
        }

        #endregion

        #region 品牌
        /// <summary>
        /// 分页查询品牌
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetCarBrandList")]
        [HttpPost]
        public BasePageList<BaseCarBrandListViewModel> GetCarBrandList([FromBody] BaseCarBrandQueryModel query)
        {
            return _baseservice.GetCarBrandList(query);
        }
        /// <summary>
        /// 获取品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("GetCarBrandById")]
        [HttpGet]
        public JResult GetCarBrandById(string innerid)
        {
            return _baseservice.GetCarBrandById(innerid);
        }
        /// <summary>
        /// 更新品牌状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("UpdateBrandStatus")]
        [HttpPost]
        public JResult UpdateBrandStatus(string carid, int status)
        {
            return _baseservice.UpdateBrandStatus(carid, status);
        }
        /// <summary>
        /// 添加品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddCarBrand")]
        [HttpPost]
        public JResult AddCarBrand([FromBody] BaseCarBrandModel model)
        {
            return _baseservice.AddCarBrand(model);
        }
        /// <summary>
        /// 删除品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("DeleteCarBrand")]
        [HttpPost]
        public JResult DeleteCarBrand(string innerid)
        {
            return _baseservice.DeleteCarBrand(innerid);
        }
        /// <summary>
        /// 更新品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateCarBrand")]
        [HttpPost]
        public JResult UpdateCarBrand([FromBody] BaseCarBrandModel model)
        {
            return _baseservice.UpdateCarBrand(model);
        }
        #endregion

        #region 车系
        /// <summary>
        /// 分页查询车系
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetCarSeriesList")]
        [HttpPost]
        public BasePageList<BaseCarSeriesListViewModel> GetCarSeriesList([FromBody] BaseCarSeriesQueryModel query)
        {
            return _baseservice.GetCarSeriesList(query);
        }
        /// <summary>
        /// 获取车系信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("GetCarSeriesById")]
        [HttpGet]
        public JResult GetCarSeriesById(string innerid)
        {
            return _baseservice.GetCarSeriesById(innerid);
        }
        /// <summary>
        /// 更新车系状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("UpdateSeriesStatus")]
        [HttpPost]
        public JResult UpdateSeriesStatus(string carid, int status)
        {
            return _baseservice.UpdateSeriesStatus(carid, status);
        }
        /// <summary>
        /// 添加车系信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddCarSeries")]
        [HttpPost]
        public JResult AddCarSeries([FromBody] BaseCarSeriesModel model)
        {
            return _baseservice.AddCarSeries(model);
        }
        /// <summary>
        /// 删除车系信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("DeleteCarSeries")]
        [HttpPost]
        public JResult DeleteCarSeries(string innerid)
        {
            return _baseservice.DeleteCarSeries(innerid);
        }
        /// <summary>
        /// 更新车系信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateCarSeries")]
        [HttpPost]
        public JResult UpdateCarSeries([FromBody] BaseCarSeriesModel model)
        {
            return _baseservice.UpdateCarSeries(model);
        }
        #endregion

        #region 车型
        /// <summary>
        /// 分页查询车型
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetCarModelList")]
        [HttpPost]
        public BasePageList<BaseCarModelListViewModel> GetCarModelList([FromBody] BaseCarModelQueryModel query)
        {
            return _baseservice.GetCarModelList(query);
        }
        /// <summary>
        /// 获取车型信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("GetBaseCarModelById")]
        [HttpGet]
        public JResult GetBaseCarModelById(string innerid)
        {
            return _baseservice.GetBaseCarModelById(innerid);
        }
        /// <summary>
        /// 更新车型信息
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("UpdateModelStatus")]
        [HttpPost]
        public JResult UpdateModelStatus(string carid, int status)
        {
            return _baseservice.UpdateModelStatus(carid, status);
        }
        /// <summary>
        /// 添加车型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddCarModel")]
        [HttpPost]
        public JResult AddCarModel([FromBody] BaseCarModelModel model)
        {
            return _baseservice.AddCarModel(model);
        }
        /// <summary>
        /// 删除车型信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("DeleteCarModel")]
        [HttpPost]
        public JResult DeleteCarModel(string innerid)
        {
            return _baseservice.DeleteCarModel(innerid);
        }
        /// <summary>
        /// 更新车型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateCarModel")]
        [HttpPost]
        public JResult UpdateCarModel([FromBody] BaseCarModelModel model)
        {
            return _baseservice.UpdateCarModel(model);
        }
        #endregion

        #region 广告管理
        
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <returns></returns>
        [Route("GetBannerList")]
        [HttpGet]
        public JResult GetBannerList()
        {
            return _baseservice.GetBannerList();
        }
        
        #endregion

        /// <summary>
        ///     上传文件
        /// </summary>
        /// <returns>图片主键</returns>
        [HttpPost]
        [Route("FileUpload")]
        public string FileUpload(string type)
        {
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
            {
                return "0";
            }

            try
            {
                var keys = "";
                for (var i = 0;i < files.Count; i++)
                {
                    var filename = string.Concat(type + "_", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ".jpg");
                    var filepath = QiniuUtility.GetFilePath(filename);
                    
                    files[i].SaveAs(filepath);

                    //上传图片到七牛云
                    var qinniu = new QiniuUtility();
                    var qrcodeKey = qinniu.PutFile(filepath, "", filename);

                    //删除本地临时文件
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                    }

                    keys += qrcodeKey + ",";
                }

                return keys.TrimEnd(',');
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("上傳文件異常：", TraceEventType.Error, ex);
                return "-2";
            }
        }

        /// <summary>
        /// 获取七牛Token值
        /// </summary>
        /// <returns></returns>
        [Route("GetUpToken")]
        [HttpGet]
        public JResult GetUpToken()
        {
            var uptoken = QiniuUtility.GetToken();
            return JResult._jResult(0, uptoken);
        }

        /// <summary>
        /// 获取七牛url
        /// </summary>
        /// <returns></returns>
        [Route("GetQiniuUrl")]
        [HttpGet]
        public JResult GetQiniuUrl()
        {
            var url = ConfigHelper.GetAppSettings("GETURL");
            return JResult._jResult(0, url);
        }
    }
}

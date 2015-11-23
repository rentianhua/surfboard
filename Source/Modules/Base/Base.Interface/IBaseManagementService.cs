using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Base.BusinessEntity;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Base.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseManagementService
    {
        #region Code
        #region 基础数据代码类型
        /// <summary>
        /// 获取基础数据代码类型列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<BaseCodeTypeListModel> GetCodeTypeList(BaseCodeTypeQueryModel query);
        /// <summary>
        /// 更新基础数据代码类型状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateCodeTypeStatus(string id, int status);
        /// <summary>
        /// 删除基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DeleteCodeType(string innerid);
        /// <summary>
        /// 获取基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetCodeTypeById(string innerid);
        /// <summary>
        /// 添加基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCodeType(BaseCodeTypeModel model);
        /// <summary>
        /// 更新基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateCodeType(BaseCodeTypeModel model);
        #endregion

        /// <summary>
        /// 获取基础数据代码值列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<BaseCodeSelectModel> GetCodeList(BaseCodeQueryModel query);
        /// <summary>
        /// 获取基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        IEnumerable<BaseCodeTypeModel> GetCodeType(string innerid);
        /// <summary>
        /// 获取基础数据代码值状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateCodeStatus(string id, int status);
        /// <summary>
        /// 删除基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DeleteCode(string innerid);
        /// <summary>
        /// 获取基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetCodeById(string innerid);
        /// <summary>
        /// 添加基础数据代码值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCode(BaseCodeModel model);
        /// <summary>
        /// 更新基础数据代码值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateCode(BaseCodeModel model);

        /// <summary>
        /// 获取代码值列表
        /// </summary>
        /// <param name="typekey">代码类型key</param>
        /// <returns></returns>
        JResult GetCodeByTypeKey(string typekey);
      
        #endregion

        #region 验证码

        /// <summary>
        /// 会员注册获取验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult SendVerification(BaseVerification model);

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vcode">验证码</param>
        /// <param name="utype">用处类型[1注册,2登录,3,其他]</param>
        /// <returns>返回结果。1.正确，0不正确</returns>
        JResult CheckVerification(string target, string vcode, int utype);

        #endregion

        #region 区域

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        IEnumerable<BaseProvince> GetProvList(string initial);

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="provId">省份ID</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        IEnumerable<BaseCity> GetCityList(int provId, string initial);

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        JResult GetProvListEx(string initial);

        #endregion

        #region 品牌/车系/车型

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        IEnumerable<BaseCarBrandModel> GetCarBrand(string initial);

        /// <summary>
        /// 获取品牌热度Top n
        /// </summary>
        /// <returns></returns>
        JResult GetCarBrandHotTop(int top);

        /// <summary>
        /// 根据品牌id获取车系
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <returns></returns>
        IEnumerable<BaseCarSeriesModel> GetCarSeries(int brandId);

        /// <summary>
        /// 根据车系ID获取车型
        /// </summary>
        /// <param name="seriesId">车系id</param>
        /// <returns></returns>
        IEnumerable<BaseCarModelModel> GetCarModel(int seriesId);

        /// <summary>
        /// 根据ID获取车型信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        JResult GetCarModelById(int innerid);
        #endregion

        #region 品牌信息
        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<BaseCarBrandListViewModel> GetCarBrandList(BaseCarBrandQueryModel query);
        /// <summary>
        /// 获取品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetCarBrandById(string innerid);
        /// <summary>
        /// 更新品牌状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateBrandStatus(string carid, int status);
        /// <summary>
        /// 添加品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCarBrand(BaseCarBrandModel model);
        /// <summary>
        /// 删除品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DeleteCarBrand(string innerid);
        /// <summary>
        /// 更新品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateCarBrand(BaseCarBrandModel model);
        #endregion
        #region 车系信息
        /// <summary>
        /// 获取车系列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<BaseCarSeriesListViewModel> GetCarSeriesList(BaseCarSeriesQueryModel query);
        /// <summary>
        /// 获取车系信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetCarSeriesById(string innerid);
        /// <summary>
        /// 更新车系状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateSeriesStatus(string carid, int status);
        /// <summary>
        /// 添加车系信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCarSeries(BaseCarSeriesModel model);
        /// <summary>
        /// 删除车系信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DeleteCarSeries(string innerid);
        /// <summary>
        /// 更新车系信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateCarSeries(BaseCarSeriesModel model);
        #endregion
        #region 车型信息
        /// <summary>
        /// 获取车型列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<BaseCarModelListViewModel> GetCarModelList(BaseCarModelQueryModel query);
        /// <summary>
        /// 获取车型信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetBaseCarModelById(string innerid);
        /// <summary>
        /// 更新车型状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateModelStatus(string carid, int status);
        /// <summary>
        /// 添加车型欣喜
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCarModel(BaseCarModelModel model);
        /// <summary>
        /// 删除车型信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DeleteCarModel(string innerid);
        /// <summary>
        /// 更新车型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateCarModel(BaseCarModelModel model);
        #endregion

        #region 获取系统后台基础信息
        JResult GetUserInfo(string loginname, string password);
        #endregion
    }
}

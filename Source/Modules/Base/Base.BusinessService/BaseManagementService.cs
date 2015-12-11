using System.Collections.Generic;
using CCN.Modules.Base.BusinessComponent;
using CCN.Modules.Base.BusinessEntity;
using CCN.Modules.Base.Interface;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framework.AuditTrail.Interception;

namespace CCN.Modules.Base.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseManagementService : ServiceBase<BaseBC>, IBaseManagementService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bc"></param>
        public BaseManagementService(BaseBC bc) : base(bc)
        {

        }

        #region Code
        #region 基础数据代码类型
        /// <summary>
        /// 获取基础数据代码类型列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseCodeTypeListModel> GetCodeTypeList(BaseCodeTypeQueryModel query)
        {
            return BusinessComponent.GetCodeTypeList(query);
        }
        /// <summary>
        /// 更新基础数据代码类型状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateCodeTypeStatus(string id, int status)
        {
            return BusinessComponent.UpdateCodeTypeStatus(id, status);
        }
        /// <summary>
        /// 删除基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCodeType(string innerid)
        {
            return BusinessComponent.DeleteCodeType(innerid);
        }
        /// <summary>
        /// 获取基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCodeTypeById(string innerid)
        {
            return BusinessComponent.GetCodeTypeById(innerid);
        }
        /// <summary>
        /// 添加基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCodeType(BaseCodeTypeModel model)
        {
            return BusinessComponent.AddCodeType(model);
        }
        /// <summary>
        /// 更新基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCodeType(BaseCodeTypeModel model)
        {
            return BusinessComponent.UpdateCodeType(model);
        }
        #endregion

        /// <summary>
        /// 获取基础数据代码值列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseCodeSelectModel> GetCodeList(BaseCodeQueryModel query)
        {
            return BusinessComponent.GetCodeList(query);
        }
        /// <summary>
        /// 获取基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public IEnumerable<BaseCodeTypeModel> GetCodeType(string innerid)
        {
            return BusinessComponent.GetCodeType(innerid);
        }
        /// <summary>
        /// 获取基础数据代码值状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateCodeStatus(string id, int status)
        {
            return BusinessComponent.UpdateCodeStatus(id, status);
        }
        /// <summary>
        /// 获取基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCodeById(string innerid)
        {
            return BusinessComponent.GetCodeById(innerid);
        }
        /// <summary>
        /// 删除基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCode(string innerid)
        {
            return BusinessComponent.DeleteCode(innerid);
        }
        /// <summary>
        /// 添加基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCode(BaseCodeModel model)
        {
            return BusinessComponent.AddCode(model);
        }
        /// <summary>
        /// 更新基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCode(BaseCodeModel model)
        {
            return BusinessComponent.UpdateCode(model);
        }


        /// <summary>
        /// 获取代码值列表
        /// </summary>
        /// <param name="typekey">代码类型key</param>
        /// <returns></returns>
        public JResult GetCodeByTypeKey(string typekey)
        {
            return BusinessComponent.GetCodeByTypeKey(typekey);
        }
        #endregion

        #region 验证码

        /// <summary>
        /// 会员注册获取验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult SendVerification(BaseVerification model)
        {
            return BusinessComponent.SendVerification(model);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vcode">验证码</param>
        /// <param name="utype">用处类型[1注册,2登录,3,其他]</param>
        /// <returns>返回结果。1.正确，0不正确</returns>
        public JResult CheckVerification(string target, string vcode, int utype)
        {
            return BusinessComponent.CheckVerification(target, vcode, utype);
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public JResult GetProvListEx(string initial)
        {
            return BusinessComponent.GetProvListEx(initial);
        }

        #endregion

        #region 区域

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        [AuditTrailCallHandler("GetProvList")]
        public IEnumerable<BaseProvince> GetProvList(string initial)
        {
            return BusinessComponent.GetProvList(initial);
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="provId">省份id</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        [AuditTrailCallHandler("GetCityList")]
        public IEnumerable<BaseCity> GetCityList(int provId, string initial)
        {
            return BusinessComponent.GetCityList(provId, initial);
        }

        #endregion

        #region 品牌/车系/车型

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseCarBrandModel> GetCarBrand(string initial)
        {
            return BusinessComponent.GetCarBrand(initial);
        }

        /// <summary>
        /// 获取品牌热度Top n
        /// </summary>
        /// <returns></returns>
        public JResult GetCarBrandHotTop(int top)
        {
            return BusinessComponent.GetCarBrandHotTop(top);
        }

        /// <summary>
        /// 根据品牌id获取车系
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <returns></returns>
        public IEnumerable<BaseCarSeriesModel> GetCarSeries(int brandId)
        {
            return BusinessComponent.GetCarSeries(brandId);
        }

        /// <summary>
        /// 获取热门车系Top n
        /// </summary>
        /// <returns></returns>
        public JResult GetCarSeriesHotTop(int top)
        {
            return BusinessComponent.GetCarSeriesHotTop(top);
        }

        /// <summary>
        /// 根据车系ID获取车型
        /// </summary>
        /// <param name="seriesId">车系id</param>
        /// <returns></returns>
        public IEnumerable<BaseCarModelModel> GetCarModel(int seriesId)
        {
            return BusinessComponent.GetCarModel(seriesId);
        }

        /// <summary>
        /// 根据ID获取车型信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public JResult GetCarModelById(int innerid)
        {
            return BusinessComponent.GetCarModelById(innerid);
        }

        #endregion

        #region 品牌信息
        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseCarBrandListViewModel> GetCarBrandList(BaseCarBrandQueryModel query)
        {
            return BusinessComponent.GetCarBrandList(query);
        }
        /// <summary>
        /// 获取品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCarBrandById(string innerid)
        {
            return BusinessComponent.GetCarBrandById(innerid);
        }
        /// <summary>
        /// 更新品牌信息
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateBrandStatus(string carid, int status)
        {
            return BusinessComponent.UpdateBrandStatus(carid, status);
        }
        /// <summary>
        /// 添加品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarBrand(BaseCarBrandModel model)
        {
            return BusinessComponent.AddCarBrand(model);
        }
        /// <summary>
        /// 删除品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCarBrand(string innerid)
        {
            return BusinessComponent.DeleteCarBrand(innerid);
        }
        /// <summary>
        /// 更新品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCarBrand(BaseCarBrandModel model)
        {
            return BusinessComponent.UpdateCarBrand(model);
        }
        #endregion
        #region 车系信息
        /// <summary>
        /// 获取车系列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<BaseCarSeriesListViewModel> GetCarSeriesList(BaseCarSeriesQueryModel query)
        {
            return BusinessComponent.GetCarSeriesList(query);
        }
        /// <summary>
        /// 获取车系信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCarSeriesById(string innerid)
        {
            return BusinessComponent.GetCarSeriesById(innerid);
        }
        /// <summary>
        /// 更新车系状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateSeriesStatus(string carid, int status)
        {
            return BusinessComponent.UpdateSeriesStatus(carid, status);
        }
        /// <summary>
        /// 添加车系信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarSeries(BaseCarSeriesModel model)
        {
            return BusinessComponent.AddCarSeries(model);
        }
        /// <summary>
        /// 删除车系信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCarSeries(string innerid)
        {
            return BusinessComponent.DeleteCarSeries(innerid);
        }
        /// <summary>
        /// 更新车系信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCarSeries(BaseCarSeriesModel model)
        {
            return BusinessComponent.UpdateCarSeries(model);
        }
        #endregion
        #region 车型信息
        /// <summary>
        /// 获取车型列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseCarModelListViewModel> GetCarModelList(BaseCarModelQueryModel query)
        {
            return BusinessComponent.GetCarModelList(query);
        }
        /// <summary>
        /// 获取车型信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetBaseCarModelById(string innerid)
        {
            return BusinessComponent.GetBaseCarModelById(innerid);
        }
        /// <summary>
        /// 更新车型状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateModelStatus(string carid, int status)
        {
            return BusinessComponent.UpdateModelStatus(carid, status);
        }
        /// <summary>
        /// 添加车型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarModel(BaseCarModelModel model)
        {
            return BusinessComponent.AddCarModel(model);
        }
        /// <summary>
        /// 删除车型信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCarModel(string innerid)
        {
            return BusinessComponent.DeleteCarModel(innerid);
        }
        /// <summary>
        /// 更新车型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCarModel(BaseCarModelModel model)
        {
            return BusinessComponent.UpdateCarModel(model);
        }
        #endregion

        #region 获取系统后台基础信息

        #region 用户管理
        /// <summary>
        /// 获取登录人信息
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JResult GetUserInfo(string loginname, string password)
        {
            return BusinessComponent.GetUserInfo(loginname, password);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseUserModel> GetUserList(BaseUserQueryModel query)
        {
            return BusinessComponent.GetUserList(query);
        }

        /// <summary>
        /// 添加用户 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddUser(BaseUserModel model)
        {
            return BusinessComponent.AddUser(model);
        }

        /// <summary>
        /// 更新用户 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateUser(BaseUserModel model)
        {
            return BusinessComponent.UpdateUser(model);
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateUserStatus(string innerid,int status)
        {
            return BusinessComponent.UpdateUserStatus(innerid,status);
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetUserInfoByID(string innerid)
        {
            return BusinessComponent.GetUserInfoByID(innerid);
        }

        /// <summary>
        /// 获取用户对应权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JResult GetRoleByUerid(string userid)
        {
            return BusinessComponent.GetRoleByUerid(userid);
        }

        /// <summary>
        /// 获取用户对应菜单
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JResult GetMenuByUerid(string userid)
        {
            return BusinessComponent.GetMenuByUerid(userid);
        }

        #endregion

        #region 角色管理

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseRoleViewModel> GetRoleList(BaseRoleQueryModel query)
        {
            return BusinessComponent.GetRoleList(query);
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public JResult GetAllRole(BaseRoleModel model)
        {
            return BusinessComponent.GetAllRole(model);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddRole(BaseRoleModel model)
        {
            return BusinessComponent.AddRole(model);
        }

        /// <summary>
        /// 更新角色 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateRole(BaseRoleModel model)
        {
            return BusinessComponent.UpdateRole(model);
        }

        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="isenabled"></param>
        /// <returns></returns>
        public JResult UpdateRoleStatus(string innerid, int isenabled)
        {
            return BusinessComponent.UpdateRoleStatus(innerid, isenabled);
        }

        /// <summary>
        /// 根据ID获取角色信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetRoleInfoByID(string innerid)
        {
            return BusinessComponent.GetRoleInfoByID(innerid);
        }

        #endregion

        #region 菜单管理

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<MenuViewMode> GetMenuList(MenuQueryModel query)
        {
            return BusinessComponent.GetMenuList(query);
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        public JResult GetAllMenu(BaseMenuModel model)
        {
            return BusinessComponent.GetAllMenu(model);
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddMenu(BaseMenuModel model)
        {
            return BusinessComponent.AddMenu(model);
        }

        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateMenu(BaseMenuModel model)
        {
            return BusinessComponent.UpdateMenu(model);
        }

        /// <summary>
        /// 删除菜单（物理删除）
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteMenu(string innerid)
        {
            return BusinessComponent.DeleteMenu(innerid);
        }

        /// <summary>
        /// 获取菜单详细信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetMenuInfo(string innerid)
        {
            return BusinessComponent.GetMenuInfo(innerid);
        }

        /// <summary>
        /// 给角色赋相应的权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddRoleMenu(BaseRoleMenuModel model)
        {
            return BusinessComponent.AddRoleMenu(model);
        }

        /// <summary>
        /// 保存职员对应角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddUserRole(BaseRoleUserModel model)
        {
            return BusinessComponent.AddUserRole(model);
        }

        /// <summary>
        /// 获取角色对应所有的菜单
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public JResult GetRoleToMenu(string roleid)
        {
            return BusinessComponent.GetRoleToMenu(roleid);
        }

        #endregion

        #endregion
    }
}

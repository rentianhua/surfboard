﻿#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using CCN.Modules.Customer.BusinessComponent;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using Cedar.Core.Logging;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.AuditTrail.Interception;
using Microsoft.Practices.Unity.InterceptionExtension;

#endregion

namespace CCN.Modules.Customer.BusinessService
{
    /// <summary>
    /// </summary>
    public class CustomerManagementService : ServiceBase<CustomerBC>, ICustomerManagementService
    {
        private readonly object _obj = new object();
        /// <summary>
        /// </summary>
        public CustomerManagementService(CustomerBC bc)
            : base(bc)
        {
        }
        
        #region 用户模块

        /// <summary>
        /// 会员注册检查Email是否被注册
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>0：未被注册，非0：Email被注册</returns>
        [AuditTrailCallHandler("CustomerManagementService.CheckEmail")]
        public int CheckEmail(string email)
        {
            return BusinessComponent.CheckEmail(email);
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：被注册</returns>
        [AuditTrailCallHandler("CheckMobile")]
        public int CheckMobile(string mobile)
        {
            return BusinessComponent.CheckMobile(mobile);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CustRegister")]
        public JResult CustRegister(CustModel userInfo)
        {
            return BusinessComponent.CustRegister(userInfo);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        [AuditTrailCallHandler("CustLogin")]
        public JResult CustLogin(CustLoginInfo loginInfo)
        {
            return BusinessComponent.CustLogin(loginInfo);
        }

        /// <summary>
        /// 手机+验证码登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public JResult GetCustLoginByMobile(string mobile)
        {
            return BusinessComponent.GetCustLoginByMobile(mobile);
        }

        /// <summary>
        /// 用户登录(openid登录)
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        public JResult CustLoginByOpenid(string openid)
        {
            return BusinessComponent.CustLoginByOpenid(openid);
        }

        /// <summary>
        /// 判断是否会员
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        public JResult IsCustByOpenid(string openid)
        {
            return BusinessComponent.IsCustByOpenid(openid);
        }


        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        public JResult GetCustById(string innerid)
        {
            return BusinessComponent.GetCustById(innerid);
        }

        /// <summary>
        /// 获取会员详情（根据手机号）
        /// </summary>
        /// <param name="mobile">会员手机号</param>
        /// <returns></returns>
        public JResult GetCustByMobile(string mobile)
        {
            return BusinessComponent.GetCustByMobile(mobile);
        }

        /// <summary>
        /// 根据carid获取会员基本信息
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>用户信息</returns>
        public JResult CustInfoByCarid(string carid)
        {
            return BusinessComponent.CustInfoByCarid(carid);
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustModel> GetCustPageList(CustQueryModel query)
        {
            return BusinessComponent.GetCustPageList(query);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="mRetrievePassword"></param>
        /// <returns></returns>
        public JResult UpdatePassword(CustRetrievePassword mRetrievePassword)
        {
            return BusinessComponent.UpdatePassword(mRetrievePassword);
        }

        /// <summary>
        /// 修改密码（根据手机号和密码修改）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdatePassword(CustModifyPassword model)
        {
            return BusinessComponent.UpdatePassword(model);
        }

        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCustInfo(CustModel model)
        {
            return BusinessComponent.UpdateCustInfo(model);
        }

        /// <summary>
        /// 修改会员状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateCustStatus(string innerid, int status)
        {
            return BusinessComponent.UpdateCustStatus(innerid, status);
        }

        /// <summary>
        /// 修改会员类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult UpdateCustType(string innerid)
        {
            return BusinessComponent.UpdateCustType(innerid);
        }

        #endregion

        #region 会员Total

        /// <summary>
        /// 更新会员的刷新次数
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="oper">1+ 2-</param>
        /// <returns>用户信息</returns>
        public JResult UpdateCustTotalCount(string custid, int type, int count, int oper = 1)
        {
            return BusinessComponent.UpdateCustTotalCount(custid, type, count, oper);            
        }

        /// <summary>
        /// 发福利
        /// </summary>
        /// <returns></returns>
        public JResult SendWelfare(int refreshnum, int topnum)
        {
            lock (_obj)
            {
                var mu = new Mutex(false, "MyMutex");
                mu.WaitOne();
                try
                {
                    return BusinessComponent.SendWelfare(refreshnum, topnum);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("发福利异常BusinessService", TraceEventType.Error, ex);
                    return JResult._jResult(400, "发福利异常");
                }
                finally
                {
                    mu.ReleaseMutex();
                }
            }
        }

        #endregion

        #region 用户认证

        /// <summary>
        /// 用户添加认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public JResult AddAuthentication(CustAuthenticationModel model)
        {
            return BusinessComponent.AddAuthentication(model);
        }

        /// <summary>
        /// 用户修改认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public JResult UpdateAuthentication(CustAuthenticationModel model)
        {
            return BusinessComponent.UpdateAuthentication(model);
        }

        /// <summary>
        /// 审核认证信息
        /// </summary>
        /// <param name="model">会员相关信息</param>
        /// <returns></returns>
        public JResult AuditAuthentication(CustAuthenticationModel model)
        {
            return BusinessComponent.AuditAuthentication(model);
        }

        /// <summary>
        /// 撤销审核
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public JResult CancelAuditAuthentication(string custid)
        {
            return BusinessComponent.CancelAuditAuthentication(custid);
        }

        /// <summary>
        /// 获取会员认证信息 by innerid
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public JResult GetCustAuthById(string innerid)
        {
            return BusinessComponent.GetCustAuthById(innerid);
        }

        /// <summary>
        /// 获取会员认证信息 by custid
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public JResult GetCustAuthByCustid(string custid)
        {
            return BusinessComponent.GetCustAuthByCustid(custid);
        }


        #endregion

        #region 会员点赞

        /// <summary>
        /// 给会员点赞
        /// </summary>
        /// <param name="model">粉丝信息</param>
        /// <returns></returns>
        public JResult CustPraise(CustLaudator model)
        {
            return BusinessComponent.CustPraise(model);
        }

        /// <summary>
        /// 根据会员id获取所有点赞人列表
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public JResult GetLaudatorListByCustid(string custid)
        {
            return BusinessComponent.GetLaudatorListByCustid(custid);
        }

        /// <summary>
        /// 判断是否点赞
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public JResult RepeatPraise(string custid, string openid)
        {
            return BusinessComponent.RepeatPraise(custid, openid);
        }
        #endregion

        #region 会员标签


        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        public JResult AddTag(CustTagModel model)
        {
            return BusinessComponent.AddTag(model);
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        public JResult UpdateTag(CustTagModel model)
        {
            return BusinessComponent.UpdateTag(model);
        }

        /// <summary>
        /// 修改标签状态
        /// </summary>
        /// <param name="innerid">标签ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateTagStatus(string innerid, int status)
        {
            return BusinessComponent.UpdateTagStatus(innerid, status);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        public JResult DeleteTag(string innerid)
        {
            return BusinessComponent.DeleteTag(innerid);
        }

        /// <summary>
        /// 获取标签详情
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        public JResult GetTagById(string innerid)
        {
            return BusinessComponent.GetTagById(innerid);
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustTagModel> GetTagPageList(CustTagQueryModel query)
        {
            return BusinessComponent.GetTagPageList(query);
        }

        /// <summary>
        /// 打标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult DoTagRelation(CustTagRelation model)
        {
            return BusinessComponent.DoTagRelation(model);
        }

        /// <summary>
        /// 删除标签关系
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DelTagRelation(string innerid)
        {
            return BusinessComponent.DelTagRelation(innerid);
        }

        /// <summary>
        /// 获取会员拥有的标签
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public JResult GetTagRelation(string custid)
        {
            return BusinessComponent.GetTagRelation(custid);
        }

        /// <summary>
        /// 获取会员该标签的操作者
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="tagid"></param>
        /// <returns></returns>
        public JResult GetTagRelationWithOper(string custid, string tagid)
        {
            return BusinessComponent.GetTagRelationWithOper(custid,tagid);
        }

        #endregion

        #region 数据清理

        /// <summary>
        /// 清除所有数据(除基础数据)
        /// </summary>
        /// <returns></returns>
        public JResult DeleteAll()
        {
            return BusinessComponent.DeleteAll();
        }

        /// <summary>
        /// 删除会员所有信息
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public JResult DeleteCustomer(string mobile)
        {
            return BusinessComponent.DeleteCustomer(mobile);
        }

        #endregion

        #region 微信信息
        /// <summary>
        /// 获取cust_wechat信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CustWeChatViewModel> GetCustWeChatList(CustWeChatQueryModel query)
        {
            return BusinessComponent.GetCustWeChatList(query);
        }

        /// <summary>
        /// 更新绑定openid
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public JResult BindOpenid(string custid, string openid)
        {
            return BusinessComponent.BindOpenid(custid, openid);
        }

        #endregion

        #region 车信评
        
        /// <summary>
        /// 公司列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CompanyListModel> GetCompanyPageList(CompanyQueryModel query)
        {
            return BusinessComponent.GetCompanyPageList(query);
        }
        /// <summary>
        /// 获取公司model
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCompanyModelById(string innerid)
        {
            return BusinessComponent.GetCompanyModelById(innerid);
        }
        /// <summary>
        /// 获取公司详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCompanyById(string innerid)
        {
            return BusinessComponent.GetCompanyById(innerid);
        }

        /// <summary>
        /// 更新企业信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCompanyModel(CompanyModel model)
        {
            return BusinessComponent.UpdateCompanyModel(model);
        }

        /// <summary>
        /// 申请企业信息修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCompanyApplyUpdate(CompanyApplyUpdateModel model)
        {
            return BusinessComponent.AddCompanyApplyUpdate(model);
        }

        /// <summary>
        /// 修改申请列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CompanyUpdateApplyListModel> GetUpdateApplyPageList(CompanyUpdateApplyQueryModel query)
        {
            return BusinessComponent.GetUpdateApplyPageList(query);
        }

        /// <summary>
        /// 获取申请的信息view
        /// </summary>
        /// <param name="applyid"></param>
        /// <returns></returns>
        public JResult GetUpdateApplyById(string applyid)
        {
            return BusinessComponent.GetUpdateApplyById(applyid);
        }

        /// <summary>
        /// 处理修改申请
        /// </summary>
        ///<param name="model"></param>
        /// <returns></returns>
        public JResult HandleApply(CompanyApplyUpdateModel model)
        {
            return BusinessComponent.HandleApply(model);
        }

        /// <summary>
        /// 获取公司图片
        /// </summary>
        /// <param name="settid"></param>
        /// <returns></returns>
        public JResult GetCompanyPictureListById(string settid)
        {
            return BusinessComponent.GetCompanyPictureListById(settid);
        }

        /// <summary>
        /// 企业评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult DoComment(CommentModel model)
        {
            return BusinessComponent.DoComment(model);
        }

        /// <summary>
        /// 企业点赞
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult DoPraise(PraiseModel model)
        {
            return BusinessComponent.DoPraise(model);
        }

        /// <summary>
        /// 评论列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CommentListModel> GetCommentPageList(CommentQueryModel query)
        {
            return BusinessComponent.GetCommentPageList(query);
        }

        /// <summary>
        /// 评分列表
        /// </summary>
        /// <param name="settid"></param>
        /// <returns></returns>
        public JResult GetScoreList(string settid)
        {
            return BusinessComponent.GetScoreList(settid);
        }

        /// <summary>
        /// 导入公司
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public JResult ImportCompany(string file)
        {
            return BusinessComponent.ImportCompany(file);
        }

        /// <summary>
        /// 更新申请表状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateApplyStatus(CompanyApplyUpdateModel model)
        {
            return BusinessComponent.UpdateApplyStatus(model);
        }

        /// <summary>
        /// 删除评论（逻辑删除）
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteComment(string innerid)
        {
            return BusinessComponent.DeleteComment(innerid);
        }

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCommentViewByID(string innerid)
        {
            return BusinessComponent.GetCommentViewByID(innerid);
        }

        #region 图片处理

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settid"></param>
        /// <returns></returns>
        public JResult GetCompanyPictureById(string settid)
        {
            return BusinessComponent.GetCompanyPictureById(settid);
        }

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult SaveCompanyPicture(CompanyPictureListModel model)
        {
            return BusinessComponent.SaveCompanyCarPicture(model);
        }

        #endregion

        #endregion

        #region C用户管理

        /// <summary>
        /// C用户 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public JResult UserRegister(UserModel userInfo)
        {
            return BusinessComponent.UserRegister(userInfo);
        }

        /// <summary>
        /// C用户 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        public JResult UserLogin(UserLoginInfo loginInfo)
        {
            return BusinessComponent.UserLogin(loginInfo);
        }

        /// <summary>
        /// C用户 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        public JResult GetUserInfoById(string innerid)
        {
            return BusinessComponent.GetUserInfoById(innerid);
        }

        /// <summary>
        /// C用户 获取会员详情（根据手机号）
        /// </summary>
        /// <param name="mobile">会员手机号</param>
        /// <returns></returns>
        public JResult GetUserInfoByMobile(string mobile)
        {
            return BusinessComponent.GetUserInfoByMobile(mobile);
        }

        /// <summary>
        /// C用户 手机+验证码登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public JResult UserLoginByMobile(string mobile)
        {
            return BusinessComponent.UserLoginByMobile(mobile);
        }

        /// <summary>
        /// C用户 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<UserListModel> GetUserPageList(UserQueryModel query)
        {
            return BusinessComponent.GetUserPageList(query);
        }

        /// <summary>
        /// C用户 修改密码
        /// </summary>
        /// <param name="mRetrievePassword"></param>
        /// <returns></returns>
        public JResult UpdateUserPassword(UserRetrievePassword mRetrievePassword)
        {
            return BusinessComponent.UpdateUserPassword(mRetrievePassword);
        }

        /// <summary>
        /// C用户 修改会员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateUserInfo(UserModel model)
        {
            return BusinessComponent.UpdateUserInfo(model);
        }

        /// <summary>
        /// C用户 修改会员状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateUserStatus(string innerid, int status)
        {
            return BusinessComponent.UpdateUserStatus(innerid, status);
        }

        #endregion

        #region 会员升级

        /// <summary>
        /// 微信会员费支付
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <param name="type"></param>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        public JResult CustWxPayVip(string custid,string type, string tradeType = "NATIVE")
        {
            return BusinessComponent.CustWxPayVip(custid, type, tradeType);
        }

        /// <summary>
        /// 微信会员支付回调
        /// </summary>
        /// <param name="orderno">会员id</param>
        /// <returns></returns>
        public JResult CustWxPayVipBack(string orderno)
        {
            return BusinessComponent.CustWxPayVipBack(orderno);
        }

        /// <summary>
        /// 根据订单号获取订单信息
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public JResult CustWeChatPayByorderno(string orderno)
        {
            return BusinessComponent.CustWeChatPayByorderno(orderno);
        }

        #endregion

        #region 投诉建议

        /// <summary>
        /// 保存投诉建议
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddSiteAdvice(SiteAdviceModel model)
        {

            return BusinessComponent.AddSiteAdvice(model);
        }


        #endregion

        #region 粉丝绑定
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult RebindFans(CustRebindFansModel model)
        {
            return BusinessComponent.RebindFans(model);
        }

        #endregion
    }
}
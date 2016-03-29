#region

using System.Collections.Generic;
using CCN.Modules.Customer.BusinessEntity;
using Cedar.Framework.Common.BaseClasses;

#endregion

namespace CCN.Modules.Customer.Interface
{
    /// <summary>
    /// </summary>
    public interface ICustomerManagementService
    {
        #region 用户模块

        /// <summary>
        /// 会员注册检查Email是否被注册
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>0：未被注册，非0：Email被注册</returns>
        int CheckEmail(string email);

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：被注册</returns>
        int CheckMobile(string mobile);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        JResult CustRegister(CustModel userInfo);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        JResult CustLogin(CustLoginInfo loginInfo);

        /// <summary>
        /// 手机+验证码登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        JResult GetCustLoginByMobile(string mobile);

        /// <summary>
        /// 用户登录(openid登录)
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        JResult CustLoginByOpenid(string openid);

        /// <summary>
        /// 判断是否会员
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        JResult IsCustByOpenid(string openid);

        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        JResult GetCustById(string innerid);

        /// <summary>
        /// 获取会员详情（根据手机号）
        /// </summary>
        /// <param name="mobile">会员手机号</param>
        /// <returns></returns>
        JResult GetCustByMobile(string mobile);

        /// <summary>
        /// 根据carid获取会员基本信息
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>用户信息</returns>
        JResult CustInfoByCarid(string carid);

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CustModel> GetCustPageList(CustQueryModel query);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="mRetrievePassword"></param>
        /// <returns></returns>
        JResult UpdatePassword(CustRetrievePassword mRetrievePassword);

        /// <summary>
        /// 修改密码（根据手机号和密码修改）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdatePassword(CustModifyPassword model);

        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateCustInfo(CustModel model);

        /// <summary>
        /// 修改会员状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateCustStatus(string innerid, int status);

        /// <summary>
        /// 修改会员类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult UpdateCustType(string innerid);

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
        JResult UpdateCustTotalCount(string custid, int type, int count, int oper = 1);

        /// <summary>
        /// 发福利
        /// </summary>
        /// <returns></returns>
        JResult SendWelfare(int refreshnum, int topnum);

        #endregion

        #region 用户认证

        /// <summary>
        /// 用户添加认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        JResult AddAuthentication(CustAuthenticationModel model);

        /// <summary>
        /// 用户修改认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        JResult UpdateAuthentication(CustAuthenticationModel model);

        /// <summary>
        /// 审核认证信息
        /// </summary>
        /// <param name="model">会员相关信息</param>
        /// <returns></returns>
        JResult AuditAuthentication(CustAuthenticationModel model);

        /// <summary>
        /// 撤销审核
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        JResult CancelAuditAuthentication(string custid);

        /// <summary>
        /// 获取会员认证信息 by innerid
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        JResult GetCustAuthById(string innerid);

        /// <summary>
        /// 获取会员认证信息 by custid
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        JResult GetCustAuthByCustid(string custid);


        #endregion

        #region 会员点赞

        /// <summary>
        /// 给会员点赞
        /// </summary>
        /// <param name="model">粉丝信息</param>
        /// <returns></returns>
        JResult CustPraise(CustLaudator model);

        /// <summary>
        /// 根据会员id获取所有点赞人列表
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        JResult GetLaudatorListByCustid(string custid);

        /// <summary>
        /// 判断是否点赞
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        JResult RepeatPraise(string custid, string openid);
        #endregion

        #region 会员标签


        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        JResult AddTag(CustTagModel model);

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        JResult UpdateTag(CustTagModel model);

        /// <summary>
        /// 修改标签状态
        /// </summary>
        /// <param name="innerid">标签ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateTagStatus(string innerid, int status);

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        JResult DeleteTag(string innerid);

        /// <summary>
        /// 获取标签详情
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        JResult GetTagById(string innerid);

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CustTagModel> GetTagPageList(CustTagQueryModel query);

        /// <summary>
        /// 打标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult DoTagRelation(CustTagRelation model);

        /// <summary>
        /// 删除标签关系
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DelTagRelation(string innerid);

        /// <summary>
        /// 获取会员拥有的标签
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        JResult GetTagRelation(string custid);

        /// <summary>
        /// 获取会员该标签的操作者
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="tagid"></param>
        /// <returns></returns>
        JResult GetTagRelationWithOper(string custid, string tagid);

        #endregion

        #region 数据清理

        /// <summary>
        /// 清除所有数据(除基础数据)
        /// </summary>
        /// <returns></returns>
        JResult DeleteAll();

        /// <summary>
        /// 删除会员所有信息
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        JResult DeleteCustomer(string mobile);

        #endregion

        #region 微信信息
        /// <summary>
        /// 获取cust_wechat信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CustWeChatViewModel> GetCustWeChatList(CustWeChatQueryModel query);

        /// <summary>
        /// 更新绑定openid
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        JResult BindOpenid(string custid, string openid);

        #endregion

        #region 车信评


        /// <summary>
        /// 公司列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CompanyListModel> GetCompanyPageList(CompanyQueryModel query);

        /// <summary>
        /// 获取公司model
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetCompanyModelById(string innerid);

        /// <summary>
        /// 获取公司详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetCompanyById(string innerid);

        /// <summary>
        /// 更新企业信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateCompanyModel(CompanyModel model);

        /// <summary>
        /// 申请企业信息修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCompanyApplyUpdate(CompanyApplyUpdateModel model);

        /// <summary>
        /// 修改申请列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CompanyUpdateApplyListModel> GetUpdateApplyPageList(CompanyUpdateApplyQueryModel query);

        /// <summary>
        /// 获取申请的信息view
        /// </summary>
        /// <param name="applyid"></param>
        /// <returns></returns>
        JResult GetUpdateApplyById(string applyid);

        /// <summary>
        /// 处理修改申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult HandleApply(CompanyApplyUpdateModel model);

        /// <summary>
        /// 获取公司图片
        /// </summary>
        /// <param name="settid"></param>
        /// <returns></returns>
        JResult GetCompanyPictureListById(string settid);

        /// <summary>
        /// 企业评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult DoComment(CommentModel model);

        /// <summary>
        /// 企业点赞
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult DoPraise(PraiseModel model);

        /// <summary>
        /// 评论列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CommentListModel> GetCommentPageList(CommentQueryModel query);

        /// <summary>
        /// 评分列表
        /// </summary>
        /// <param name="settid"></param>
        /// <returns></returns>
        JResult GetScoreList(string settid);

        /// <summary>
        /// 导入公司
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        JResult ImportCompany(string file);

        /// <summary>
        /// 更新申请表状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateApplyStatus(CompanyApplyUpdateModel model);

        /// <summary>
        /// 删除评论（逻辑删除）
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DeleteComment(string innerid);

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetCommentViewByID(string innerid);

        #region 图片处理

        /// <summary>
        /// 获取企业图片
        /// </summary>
        /// <param name="settid"></param>
        /// <returns></returns>
        JResult GetCompanyPictureById(string settid);

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult SaveCompanyPicture(CompanyPictureListModel model);

        #endregion

        #endregion

        #region C用户管理

        /// <summary>
        /// C用户 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>

        JResult UserRegister(UserModel userInfo);

        /// <summary>
        /// C用户 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        JResult UserLogin(UserLoginInfo loginInfo);

        /// <summary>
        /// C用户 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        JResult GetUserInfoById(string innerid);

        /// <summary>
        /// C用户 获取会员详情（根据手机号）
        /// </summary>
        /// <param name="mobile">会员手机号</param>
        /// <returns></returns>
        JResult GetUserInfoByMobile(string mobile);

        /// <summary>
        /// C用户 手机+验证码登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        JResult UserLoginByMobile(string mobile);

        /// <summary>
        /// C用户 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<UserListModel> GetUserPageList(UserQueryModel query);

        /// <summary>
        /// C用户 修改密码
        /// </summary>
        /// <param name="mRetrievePassword"></param>
        /// <returns></returns>
        JResult UpdateUserPassword(UserRetrievePassword mRetrievePassword);

        /// <summary>
        /// C用户 修改会员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateUserInfo(UserModel model);

        /// <summary>
        /// C用户 修改会员状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateUserStatus(string innerid, int status);

        #endregion

        #region 会员升级
        
        /// <summary>
        /// 微信会员费支付
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        JResult CustWxPayVip(string custid);

        /// <summary>
        /// 微信会员支付回调
        /// </summary>
        /// <param name="orderno">会员id</param>
        /// <returns></returns>
        JResult CustWxPayVipBack(string orderno);

        #endregion

        #region 投诉建议

        /// <summary>
        /// 保存投诉建议
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddSiteAdvice(SiteAdviceModel model);


        #endregion

        #region 粉丝绑定

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult RebindFansModel(CustRebindFansModel model);

        #endregion
    }
}
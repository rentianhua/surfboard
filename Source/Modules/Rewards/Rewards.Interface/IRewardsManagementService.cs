using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Rewards.BusinessEntity;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Rewards.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRewardsManagementService
    {

        #region 会员积分

        /// <summary>
        /// 会员积分变更
        /// </summary>
        /// <param name="model">变更信息</param>
        /// <returns></returns>
        JResult ChangePoint(CustPointModel model);

        /// <summary>
        /// 获取会员积分记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CustPointViewModel> GetCustPointLogPageList(CustPointQueryModel query);

        /// <summary>
        /// 积分兑换礼券
        /// </summary>
        /// <param name="model">兑换相关信息</param>
        /// <returns></returns>
        JResult PointExchangeCoupon(CustPointExChangeCouponModel model);

        /// <summary>
        /// 登录奖励积分算法
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        int LoginAlgorithm(string custid);

        /// <summary>
        /// 获取认证积分记录
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        IEnumerable<CustPointModel> GetAuthPointRecord(string custid);

        /// <summary>
        /// 获取今天分享获得积分记录
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        int GetSharePointRecord(string custid);

        #endregion

        #region 会员礼券

        /// <summary>
        /// 获取获取礼券列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CouponViewModel> GetCouponPageList(CouponQueryModel query);

        /// <summary>
        /// 添加礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        JResult AddCoupon(CouponInfoModel model);

        /// <summary>
        /// 修改礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        JResult UpdateCoupon(CouponInfoModel model);

        /// <summary>
        /// 获取礼券信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        JResult GetCouponById(string innerid);
        
        /// <summary>
        /// 获取礼券信息 by code
        /// </summary>
        /// <param name="code">id</param>
        /// <returns></returns>
        JResult GetCouponByCode(string code);

        /// <summary>
        /// 更新礼券状态
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateStatus(string cardid, int status);

        /// <summary>
        /// 修改礼券库存
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        JResult UpdateStock(CouponInfoModel model);

        /// <summary>
        /// 修改礼券有效期
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        JResult UpdateValidity(CouponInfoModel model);


        /// <summary>
        /// 礼券与微信小店产品绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult BindWechatProduct(CouponCardProduct model);

        /// <summary>
        /// 礼券与微信小店产品解除绑定
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        JResult UnBindWechatProduct(string cardid);

        /// <summary>
        /// 核销记录查询列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CodeViewListModel> GetCodeRecord(CodeQueryModel query);

        /// <summary>
        /// 核销记录查询列表-汇总
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        JResult GetCodeRecordTotal(CodeQueryModel query);
        #endregion

        #region 我的Code

        /// <summary>
        /// 获取我的礼券
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<MyCodeViewListModel> GetMyCodeList(MyCodeQueryModel query);

        /// <summary>
        /// 我的礼券详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        JResult GetCodeInfo(string code);

        #endregion

        #region 礼券商城

        /// <summary>
        /// 获取礼券列表（购买）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CouponViewModel> GetMallCouponPageList(CouponMallQuery query);

        /// <summary>
        /// 商城搜索商户列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<ShopMallViewList> GetMallShopPageList(ShopMallQueryModel query);

        #endregion

        #region 礼券对外接口

        /// <summary>
        /// 根据规则发放礼券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult SendCoupon(SendCouponModel model);
        
        /// <summary>
        /// 批量购买礼券
        /// </summary>
        /// <param name="model">购买信息</param>
        /// <returns></returns>
        JResult WholesaleCoupon(CouponBuyModel model);

        /// <summary>
        /// 获取发送礼券失败的订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<OrderViewList> GetOrderList(OrderQuery query);

        /// <summary>
        /// 处理购买失败的订单
        /// </summary>
        /// <param name="innerid">订单内部id</param>
        /// <returns></returns>
        JResult HandlOrder(string innerid);

        /// <summary>
        /// 礼券核销
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult CancelCoupon(CancelModel model);

        /// <summary>
        /// 查询已核销的礼券
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        JResult GetCoupon(CardCancelSummaryQueryModel query);
        #endregion

        #region 商户管理

        /// <summary>
        /// 根据id获取商户信息
        /// </summary>
        /// <returns></returns>
        JResult GetShopById(string innerid);

        /// <summary>
        /// 根据id获取商户信息（包含关联信息）
        /// </summary>
        /// <returns></returns>
        JResult GetShopViewById(string innerid);

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        JResult ShopLogin(ShopLoginInfo model);

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <returns></returns>
        JResult AddShop(ShopModel model);

        /// <summary>
        /// 更新商户
        /// </summary>
        /// <returns></returns>
        JResult UpdateShop(ShopModel model);

        /// <summary>
        /// 修改商户状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateShopStatus(string innerid, int status);

        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        JResult UpdateShopPassword(string innerid, string password);

        /// <summary>
        /// 删除商户
        /// </summary>
        /// <returns></returns>
        JResult DeleteShop(string innerid);

        /// <summary>
        /// 商户列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<ShopViewModel> GetShopPageList(ShopQueryModel query);

        /// <summary>
        /// 获取商户list 下拉
        /// </summary>
        /// <returns></returns>
        IEnumerable<ItemShop> GetShopList();
        #endregion

        #region 商户职员管理

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        JResult GetShopStaffModel(StaffLoginInfo model);

        /// <summary>
        /// 根据id获取商户职员信息
        /// </summary>
        /// <returns></returns>
        JResult GetShopStaffById(string innerid);

        /// <summary>
        /// 添加职员
        /// </summary>
        /// <returns></returns>
        JResult AddShopStaff(ShopStaffModel model);

        /// <summary>
        /// 更新商户Staff
        /// </summary>
        /// <returns></returns>
        JResult UpdateShopStaff(ShopStaffModel model);

        /// <summary>
        /// 修改商户Staff状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        JResult UpdateShopStaffStatus(string innerid, int status);

        /// <summary>
        /// 删除商户Staff
        /// </summary>
        /// <returns></returns>
        JResult DeleteShopStaff(string innerid);

        /// <summary>
        /// 商户职员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<ShopStaffViewModel> GetShopStaffPageList(ShopStaffQueryModel query);

        #endregion

        #region 结算记录

        /// <summary>
        /// 添加结算记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddSettLog(SettlementLogModel model);

        /// <summary>
        /// 修改结算记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateSettLog(SettlementLogModel model);

        /// <summary>
        /// 删除结算记录
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult DelSettLog(string innerid);

        /// <summary>
        /// 删除结算记录中的一张图片
        /// </summary>
        /// <param name="innerid">记录id</param>
        /// <param name="pic"></param>
        /// <returns></returns>
        JResult DeleteSettPicture(string innerid, string pic);

        /// <summary>
        /// 根据id获取结算记录信息
        /// </summary>
        /// <returns></returns>
        JResult GetSettLogById(string innerid);

        /// <summary>
        /// 结算记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<SettlementLogViewModel> GetSettLogPageList(SettlementLogQueryModel query);

        /// <summary>
        /// 根据settid获取已结算的code列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<SettedCodeViewListModel> GetSettedCodePageList(SettedCodeQueryModel query);

        #endregion

        #region 商户区处理

        /// <summary>
        /// 根据城市id获取区列表
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        JResult GetShopAreaByCityid(string cityid);

        /// <summary>
        /// 根据区获取商户列表
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        JResult GetShopByArea(string area);

        #endregion

        #region 可能存在并发问题

        /// <summary>
        /// 积分兑换礼券
        /// </summary>
        /// <param name="model">兑换相关信息</param>
        /// <returns></returns>
        JResult PointToCoupon(CustPointToCouponModel model);

        #endregion

        /// <summary>
        /// 获取礼券实例
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CouponCodeListModel> GetCouponCode(CodeQueryModel query);
    }
}

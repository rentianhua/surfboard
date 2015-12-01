using System.Collections.Generic;
using System.Threading.Tasks;
using CCN.Modules.Rewards.BusinessComponent;
using CCN.Modules.Rewards.BusinessEntity;
using CCN.Modules.Rewards.Interface;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Rewards.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class RewardsManagementService: ServiceBase<RewardsBC>,IRewardsManagementService
    {
        /// <summary>
        /// </summary>
        public RewardsManagementService(RewardsBC bc) : base(bc)
        {
        }

        #region 会员积分

        /// <summary>
        /// 会员积分变更
        /// </summary>
        /// <param name="model">变更信息</param>
        /// <returns></returns>
        public JResult ChangePoint(CustPointModel model)
        {
            return BusinessComponent.ChangePoint(model);
        }

        /// <summary>
        /// 获取会员积分记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustPointViewModel> GetCustPointLogPageList(CustPointQueryModel query)
        {
            return BusinessComponent.GetCustPointLogPageList(query);
        }

        /// <summary>
        /// 积分兑换礼券
        /// </summary>
        /// <param name="model">兑换相关信息</param>
        /// <returns></returns>
        public JResult PointExchangeCoupon(CustPointExChangeCouponModel model)
        {
            return BusinessComponent.PointExchangeCoupon(model);
        }

        /// <summary>
        /// 登录奖励积分算法
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public int LoginAlgorithm(string custid)
        {
            return BusinessComponent.LoginAlgorithm(custid);
        }

        /// <summary>
        /// 获取认证积分记录
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public IEnumerable<CustPointModel> GetAuthPointRecord(string custid)
        {
            return BusinessComponent.GetAuthPointRecord(custid);
        }

        /// <summary>
        /// 获取今天分享获得积分记录
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public int GetSharePointRecord(string custid)
        {
            return BusinessComponent.GetSharePointRecord(custid);
        }

        #endregion

        #region 会员礼券

        /// <summary>
        /// 获取获取礼券列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CouponViewModel> GetCouponPageList(CouponQueryModel query)
        {
            return BusinessComponent.GetCouponPageList(query);
        }

        /// <summary>
        /// 添加礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public JResult AddCoupon(CouponInfoModel model)
        {
            return BusinessComponent.AddCoupon(model);
        }

        /// <summary>
        /// 修改礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public JResult UpdateCoupon(CouponInfoModel model)
        {
            return BusinessComponent.UpdateCoupon(model);
        }

        /// <summary>
        /// 获取礼券信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public JResult GetCouponById(string innerid)
        {
            return BusinessComponent.GetCouponById(innerid);
        }

        /// <summary>
        /// 获取礼券信息 by code
        /// </summary>
        /// <param name="code">id</param>
        /// <returns></returns>
        public JResult GetCouponByCode(string code)
        {
            return BusinessComponent.GetCouponByCode(code);
        }

        /// <summary>
        /// 更新礼券状态
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateStatus(string cardid, int status)
        {
            return BusinessComponent.UpdateStatus(cardid, status);
        }

        /// <summary>
        /// 修改礼券库存
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public JResult UpdateStock(CouponInfoModel model)
        {
            return BusinessComponent.UpdateStock(model);
        }

        /// <summary>
        /// 修改礼券有效期
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public JResult UpdateValidity(CouponInfoModel model)
        {
            return BusinessComponent.UpdateValidity(model);
        }
        
        /// <summary>
        /// 礼券与微信小店产品绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult BindWechatProduct(CouponCardProduct model)
        {
            return BusinessComponent.BindWechatProduct(model);
        }

        /// <summary>
        /// 礼券与微信小店产品解除绑定
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public JResult UnBindWechatProduct(string cardid)
        {
            return BusinessComponent.UnBindWechatProduct(cardid);
        }

        /// <summary>
        /// 核销记录查询列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CodeViewListModel> GetCodeRecord(CodeQueryModel query)
        {
            return BusinessComponent.GetCodeRecord(query);
        }

        /// <summary>
        /// 核销记录查询列表-汇总
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JResult GetCodeRecordTotal(CodeQueryModel query)
        {
            return BusinessComponent.GetCodeRecordTotal(query);
        }
        #endregion

        #region 我的Code

        /// <summary>
        /// 获取我的礼券
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<MyCodeViewListModel> GetMyCodeList(MyCodeQueryModel query)
        {
            return BusinessComponent.GetMyCodeList(query);
        }

        /// <summary>
        /// 我的礼券详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JResult GetCodeInfo(string code)
        {
            return BusinessComponent.GetCodeInfo(code);
        }

        #endregion

        #region 礼券商城

        /// <summary>
        /// 获取礼券列表（购买）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CouponViewModel> GetMallCouponPageList(CouponMallQuery query)
        {
            return BusinessComponent.GetMallCouponPageList(query);
        }

        /// <summary>
        /// 商城搜索商户列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<ShopMallViewList> GetMallShopPageList(ShopMallQueryModel query)
        {
            return BusinessComponent.GetMallShopPageList(query);
        }
        #endregion

        #region 礼券对外接口

        /// <summary>
        /// 根据规则发放礼券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult SendCoupon(SendCouponModel model)
        {
            return BusinessComponent.SendCoupon(model);
        }


        /// <summary>
        /// 批量购买礼券
        /// </summary>
        /// <param name="model">购买信息</param>
        /// <returns></returns>
        public JResult WholesaleCoupon(CouponBuyModel model)
        {
            var jresult = BusinessComponent.WholesaleCoupon(model);

            Task.Run(() =>
            {
                model.result = jresult.errcode;
                BusinessComponent.SaveOrder(model);
            });

            return jresult;
        }

        /// <summary>
        /// 礼券核销
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult CancelCoupon(CancelModel model)
        {
            return BusinessComponent.CancelCoupon(model);
        }

        /// <summary>
        /// 查询已核销的礼券
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JResult GetCoupon(CardCancelSummaryQueryModel query)
        {
            return BusinessComponent.GetCoupon(query);
        }
        #endregion

        #region 商户管理

        /// <summary>
        /// 根据id获取商户信息
        /// </summary>
        /// <returns></returns>
        public JResult GetShopById(string innerid)
        {
            return BusinessComponent.GetShopById(innerid);
        }
        
        /// <summary>
        /// 根据id获取商户信息（包含关联信息）
        /// </summary>
        /// <returns></returns>
        public JResult GetShopViewById(string innerid)
        {
            return BusinessComponent.GetShopViewById(innerid);
        }

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        public JResult ShopLogin(ShopLoginInfo model)
        {
            return BusinessComponent.ShopLogin(model);
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <returns></returns>
        public JResult AddShop(ShopModel model)
        {
            return BusinessComponent.AddShop(model);
        }

        /// <summary>
        /// 更新商户
        /// </summary>
        /// <returns></returns>
        public JResult UpdateShop(ShopModel model)
        {
            return BusinessComponent.UpdateShop(model);
        }

        /// <summary>
        /// 修改商户状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateShopStatus(string innerid, int status)
        {
            return BusinessComponent.UpdateShopStatus(innerid, status);
        }

        /// <summary>
        /// 删除商户
        /// </summary>
        /// <returns></returns>
        public JResult DeleteShop(string innerid)
        {
            return BusinessComponent.DeleteShop(innerid);
        }

        /// <summary>
        /// 商户列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<ShopViewModel> GetShopPageList(ShopQueryModel query)
        {
            return BusinessComponent.GetShopPageList(query);
        }

        /// <summary>
        /// 获取商户list 下拉
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemShop> GetShopList()
        {
            return BusinessComponent.GetShopList();
        }

        #endregion

        #region 商户职员管理

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        public JResult GetShopStaffModel(StaffLoginInfo model)
        {
            return BusinessComponent.GetShopStaffModel(model);
        }

        /// <summary>
        /// 根据id获取商户职员信息
        /// </summary>
        /// <returns></returns>
        public JResult GetShopStaffById(string innerid)
        {
            return BusinessComponent.GetShopStaffById(innerid);
        }

        /// <summary>
        /// 添加职员
        /// </summary>
        /// <returns></returns>
        public JResult AddShopStaff(ShopStaffModel model)
        {
            return BusinessComponent.AddShopStaff(model);
        }

        /// <summary>
        /// 更新商户Staff
        /// </summary>
        /// <returns></returns>
        public JResult UpdateShopStaff(ShopStaffModel model)
        {
            return BusinessComponent.UpdateShopStaff(model);
        }

        /// <summary>
        /// 修改商户Staff状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateShopStaffStatus(string innerid, int status)
        {
            return BusinessComponent.UpdateShopStaffStatus(innerid, status);
        }

        /// <summary>
        /// 删除商户Staff
        /// </summary>
        /// <returns></returns>
        public JResult DeleteShopStaff(string innerid)
        {
            return BusinessComponent.DeleteShopStaff(innerid);
        }

        /// <summary>
        /// 商户职员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<ShopStaffViewModel> GetShopStaffPageList(ShopStaffQueryModel query)
        {
            return BusinessComponent.GetShopStaffPageList(query);
        }

        #endregion

        #region 结算记录

        /// <summary>
        /// 添加结算记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddSettLog(SettlementLogModel model)
        {
            return BusinessComponent.AddSettLog(model);
        }

        /// <summary>
        /// 修改结算记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateSettLog(SettlementLogModel model)
        {
            return BusinessComponent.UpdateSettLog(model);
        }

        /// <summary>
        /// 删除结算记录
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DelSettLog(string innerid)
        {
            return BusinessComponent.DelSettLog(innerid);
        }

        /// <summary>
        /// 删除结算记录中的一张图片
        /// </summary>
        /// <param name="innerid">记录id</param>
        /// <param name="pic"></param>
        /// <returns></returns>
        public JResult DeleteSettPicture(string innerid, string pic)
        {
            return BusinessComponent.DeleteSettPicture(innerid, pic);
        }

        /// <summary>
        /// 根据id获取结算记录信息
        /// </summary>
        /// <returns></returns>
        public JResult GetSettLogById(string innerid)
        {
            return BusinessComponent.GetSettLogById(innerid);
        }

        /// <summary>
        /// 结算记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<SettlementLogViewModel> GetSettLogPageList(SettlementLogQueryModel query)
        {
            return BusinessComponent.GetSettLogPageList(query);
        }

        /// <summary>
        /// 根据settid获取已结算的code列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<SettedCodeViewListModel> GetSettedCodePageList(SettedCodeQueryModel query)
        {
            return BusinessComponent.GetSettedCodePageList(query);
        }
        
        #endregion

        #region 商户区处理

        /// <summary>
        /// 根据城市id获取区列表
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public JResult GetShopAreaByCityid(string cityid)
        {
            return BusinessComponent.GetShopAreaByCityid(cityid);
        }

        /// <summary>
        /// 根据区获取商户列表
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public JResult GetShopByArea(string area)
        {
            return BusinessComponent.GetShopByArea(area);
        }

        #endregion

        #region 可能存在并发问题

        /// <summary>
        /// 积分兑换礼券
        /// </summary>
        /// <param name="model">兑换相关信息</param>
        /// <returns></returns>
        public JResult PointToCoupon(CustPointToCouponModel model)
        {
            return BusinessComponent.PointToCoupon(model);
        }

        #endregion
    }
}

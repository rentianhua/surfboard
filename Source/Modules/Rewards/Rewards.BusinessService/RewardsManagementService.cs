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
        
        #endregion

        #region 会员礼券

        /// <summary>
        /// 获取获取礼券列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CouponInfoModel> GetCouponPageList(CouponQueryModel query)
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

        #endregion
    }
}

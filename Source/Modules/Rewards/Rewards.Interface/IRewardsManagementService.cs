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

        #endregion

        #region 会员礼券

        /// <summary>
        /// 获取获取礼券列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        BasePageList<CouponInfoModel> GetCouponPageList(CouponQueryModel query);

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
        /// 修改礼券库存
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        JResult UpdateStock(CouponInfoModel model);

        #endregion
    }
}

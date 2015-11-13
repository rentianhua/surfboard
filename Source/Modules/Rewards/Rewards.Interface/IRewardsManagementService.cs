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

        #endregion

        #region 礼券对外接口

        /// <summary>
        /// 批量购买礼券
        /// </summary>
        /// <param name="model">购买信息</param>
        /// <returns></returns>
        JResult WholesaleCoupon(CouponBuyModel model);
        
        #endregion
    }
}

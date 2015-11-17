using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Rewards.BusinessEntity;
using CCN.Modules.Rewards.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using Senparc.Weixin.MP.AdvancedAPIs.MerChant;

namespace CCN.WebAPI.ApiControllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/Rewards")]
    public class RewardsController : ApiController
    {
        private readonly IRewardsManagementService _rewardsservice;

        public RewardsController()
        {
            _rewardsservice = ServiceLocatorFactory.GetServiceLocator().GetService<IRewardsManagementService>();
        }
        
        #region 会员积分

        /// <summary>
        /// 会员积分变更
        /// </summary>
        /// <param name="model">变更信息</param>
        /// <returns></returns>
        [Route("ChangePoint")]
        [HttpPost]
        public JResult ChangePoint([FromBody]CustPointModel model)
        {
            return _rewardsservice.ChangePoint(model);
        }

        /// <summary>
        /// 获取会员积分记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetCustPointLogPageList")]
        [HttpPost]
        public BasePageList<CustPointViewModel> GetCustPointLogPageList([FromBody]CustPointQueryModel query)
        {
            return _rewardsservice.GetCustPointLogPageList(query);
        }

        /// <summary>
        /// 积分兑换礼券
        /// </summary>
        /// <param name="model">兑换相关信息</param>
        /// <returns></returns>
        [Route("PointExchangeCoupon")]
        [HttpPost]
        public JResult PointExchangeCoupon([FromBody] CustPointExChangeCouponModel model)
        {
            return _rewardsservice.PointExchangeCoupon(model);
        }

        #endregion

        #region 会员礼券

        /// <summary>
        /// 获取获取礼券列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetCouponPageList")]
        [HttpPost]
        public BasePageList<CouponInfoModel> GetCouponPageList([FromBody] CouponQueryModel query)
        {
            return _rewardsservice.GetCouponPageList(query);
        }

        /// <summary>
        /// 添加礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        [Route("AddCoupon")]
        [HttpPost]
        public JResult AddCoupon([FromBody] CouponInfoModel model)
        {
            return _rewardsservice.AddCoupon(model);
        }

        /// <summary>
        /// 修改礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        [Route("UpdateCoupon")]
        [HttpPut]
        public JResult UpdateCoupon([FromBody] CouponInfoModel model)
        {
            return _rewardsservice.UpdateCoupon(model);
        }

        /// <summary>
        /// 获取礼券信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        [Route("GetCouponById")]
        [HttpGet]
        public JResult GetCouponById(string innerid)
        {
            return _rewardsservice.GetCouponById(innerid);
        }

        /// <summary>
        /// 更新礼券状态
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("UpdateStatus")]
        [HttpPost]
        public JResult UpdateStatus(string cardid, int status)
        {
            return _rewardsservice.UpdateStatus( cardid,  status);
        }
        
        /// <summary>
        /// 修改礼券库存
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        [Route("UpdateStock")]
        [HttpPost]
        public JResult UpdateStock([FromBody]CouponInfoModel model)
        {
            return _rewardsservice.UpdateStock(model);
        }

        /// <summary>
        /// 修改礼券有效期
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        [Route("UpdateValidity")]
        [HttpPost]
        public JResult UpdateValidity([FromBody]CouponInfoModel model)
        {
            return _rewardsservice.UpdateValidity(model);
        }

        /// <summary>
        /// 礼券与微信小店产品绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("BindWechatProduct")]
        [HttpPost]
        public JResult BindWechatProduct(CouponCardProduct model)
        {
            return _rewardsservice.BindWechatProduct(model);
        }

        /// <summary>
        /// 礼券与微信小店产品解除绑定
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        [Route("UnBindWechatProduct")]
        [HttpGet]
        public JResult UnBindWechatProduct(string cardid)
        {
            return _rewardsservice.UnBindWechatProduct(cardid);
        }
        #endregion

        #region 礼券对外接口

        /// <summary>
        /// 批量购买礼券
        /// </summary>
        /// <param name="model">购买信息</param>
        /// <returns></returns>
        [Route("WholesaleCoupon")]
        [HttpPost]
        //[NonAction]
        public JResult WholesaleCoupon(CouponBuyModel model)
        {
            return _rewardsservice.WholesaleCoupon(model);
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        [Route("GetProductList")]
        [HttpGet]
        public GetByStatusResult GetProductList()
        {
            //var appid = ConfigHelper.GetAppSettings("APPID");
            //var result = ProductApi.GetByStatus(appid, 0);

            var accessToken = "ezVvo70UTaiCn8e22uRW7KkP82R45QekZwTbLm7_OjPcJpZryGnD_Gap5t0stBxvnKx9jm7XKHt_QSSzKbaaWyT2lkQ6WCf8A7jIqRUco-0ZENaAJASXG";
            var result = ProductApi.GetByStatus(accessToken, 0);
            
            return result;
        }

        #endregion
    }
}

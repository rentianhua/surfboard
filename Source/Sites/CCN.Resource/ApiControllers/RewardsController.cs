﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Rewards.BusinessEntity;
using CCN.Modules.Rewards.Interface;
using Cedar.Core.IoC;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Senparc.Weixin.MP.AdvancedAPIs.MerChant;

namespace CCN.Resource.ApiControllers
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
        public JResult ChangePoint([FromBody] CustPointModel model)
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
        public BasePageList<CustPointViewModel> GetCustPointLogPageList([FromBody] CustPointQueryModel query)
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
        public BasePageList<CouponViewModel> GetCouponPageList([FromBody] CouponQueryModel query)
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
        /// 获取礼券信息 by code
        /// </summary>
        /// <param name="code">id</param>
        /// <returns></returns>
        [Route("GetCouponByCode")]
        [HttpGet]
        public JResult GetCouponByCode(string code)
        {
            return _rewardsservice.GetCouponByCode(code);
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
            return _rewardsservice.UpdateStatus(cardid, status);
        }

        /// <summary>
        /// 修改礼券库存
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        [Route("UpdateStock")]
        [HttpPost]
        public JResult UpdateStock([FromBody] CouponInfoModel model)
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
        public JResult UpdateValidity([FromBody] CouponInfoModel model)
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


        /// <summary>
        /// 核销记录查询列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetCodeRecord")]
        [HttpPost]
        public BasePageList<CodeViewListModel> GetCodeRecord([FromBody]CodeQueryModel query)
        {
            return _rewardsservice.GetCodeRecord(query);
        }

        /// <summary>
        /// 核销记录查询列表-汇总
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetCodeRecordTotal")]
        [HttpPost]
        public JResult GetCodeRecordTotal([FromBody]CodeQueryModel query)
        {
            return _rewardsservice.GetCodeRecordTotal(query);
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
        public JResult WholesaleCoupon([FromBody]CouponBuyModel model)
        {
            return _rewardsservice.WholesaleCoupon(model);
        }

        /// <summary>
        /// 获取发送礼券失败的订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetOrderList")]
        [HttpPost]
        public BasePageList<OrderViewList> GetOrderList([FromBody]OrderQuery query)
        {
            return _rewardsservice.GetOrderList(query);
        }

        /// <summary>
        /// 处理购买失败的订单
        /// </summary>
        /// <param name="innerid">订单内部id</param>
        /// <returns></returns>
        [Route("HandlOrder")]
        [HttpGet]
        public JResult HandlOrder(string innerid)
        {
            return _rewardsservice.HandlOrder(innerid);
        }

        /// <summary>
        /// 礼券核销
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CancelCoupon")]
        [HttpPost]
        public JResult CancelCoupon([FromBody]CancelModel model)
        {
            return _rewardsservice.CancelCoupon(model);
        }

        /// <summary>
        /// 查询已核销的礼券
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetCoupon")]
        [HttpPost]
        public JResult GetCoupon([FromBody]CardCancelSummaryQueryModel query)
        {
            return _rewardsservice.GetCoupon(query);
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        [Route("GetProductList")]
        [HttpGet]
        public GetByStatusResult GetProductList(string token = "")
        {
            var accessTokenOrAppId = string.IsNullOrWhiteSpace(token) ? ConfigHelper.GetAppSettings("APPID") : token;
            GetByStatusResult result;
            try
            {
                result = ProductApi.GetByStatus(accessTokenOrAppId, 0);
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取商品列表：", TraceEventType.Error, ex);
                throw;
            }
            
            return result;
        }

        #endregion

        #region 商户管理

        /// <summary>
        /// 根据id获取商户信息
        /// </summary>
        /// <returns></returns>
        [Route("GetShopById")]
        [HttpGet]
        public JResult GetShopById(string innerid)
        {
            return _rewardsservice.GetShopById(innerid);
        }
        
        /// <summary>
        /// 根据id获取商户信息（包含关联信息）
        /// </summary>
        /// <returns></returns>
        [Route("GetShopViewById")]
        [HttpGet]
        public JResult GetShopViewById(string innerid)
        {
            return _rewardsservice.GetShopViewById(innerid);
        }

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        [Route("ShopLogin")]
        [HttpPost]
        public JResult ShopLogin([FromBody] ShopLoginInfo model)
        {
            return _rewardsservice.ShopLogin(model);
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <returns></returns>
        [Route("AddShop")]
        [HttpPost]
        public JResult AddShop([FromBody] ShopModel model)
        {
            return _rewardsservice.AddShop(model);
        }

        /// <summary>
        /// 更新商户
        /// </summary>
        /// <returns></returns>
        [Route("UpdateShop")]
        [HttpPut]
        public JResult UpdateShop([FromBody] ShopModel model)
        {
            return _rewardsservice.UpdateShop(model);
        }

        /// <summary>
        /// 修改商户状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("UpdateShopStatus")]
        [HttpPost]
        public JResult UpdateShopStatus(string innerid, int status)
        {
            return _rewardsservice.UpdateShopStatus(innerid, status);
        }
        
        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Route("UpdateShopPassword")]
        [HttpPut]
        public JResult UpdateShopPassword(string innerid, string password)
        {
            return _rewardsservice.UpdateShopPassword(innerid, password);
        }

        /// <summary>
        /// 删除商户
        /// </summary>
        /// <returns></returns>
        [Route("DeleteShop")]
        [HttpDelete]
        public JResult DeleteShop(string innerid)
        {
            return _rewardsservice.DeleteShop(innerid);
        }

        /// <summary>
        /// 商户列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetShopPageList")]
        [HttpPost]
        public BasePageList<ShopViewModel> GetShopPageList([FromBody] ShopQueryModel query)
        {
            return _rewardsservice.GetShopPageList(query);
        }

        /// <summary>
        /// 获取商户list 下拉
        /// </summary>
        /// <returns></returns>
        [Route("GetShopList")]
        [HttpGet]
        public IEnumerable<ItemShop> GetShopList()
        {
            return _rewardsservice.GetShopList();
        }

        #endregion

        #region 商户职员管理

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        [Route("GetShopStaffModel")]
        [HttpPost]
        public JResult GetShopStaffModel([FromBody]StaffLoginInfo model)
        {
            return _rewardsservice.GetShopStaffModel(model);
        }

        /// <summary>
        /// 根据id获取商户职员信息
        /// </summary>
        /// <returns></returns>
        [Route("GetShopStaffById")]
        [HttpGet]
        public JResult GetShopStaffById(string innerid)
        {
            return _rewardsservice.GetShopStaffById(innerid);
        }

        /// <summary>
        /// 添加职员
        /// </summary>
        /// <returns></returns>
        [Route("AddShopStaff")]
        [HttpPost]
        public JResult AddShopStaff([FromBody]ShopStaffModel model)
        {
            return _rewardsservice.AddShopStaff(model);
        }

        /// <summary>
        /// 更新商户Staff
        /// </summary>
        /// <returns></returns>
        [Route("UpdateShopStaff")]
        [HttpPut]
        public JResult UpdateShopStaff([FromBody]ShopStaffModel model)
        {
            return _rewardsservice.UpdateShopStaff(model);
        }

        /// <summary>
        /// 修改商户Staff状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("UpdateShopStaffStatus")]
        [HttpPost]
        public JResult UpdateShopStaffStatus(string innerid, int status)
        {
            return _rewardsservice.UpdateShopStaffStatus(innerid, status);
        }

        /// <summary>
        /// 删除商户Staff
        /// </summary>
        /// <returns></returns>
        [Route("DeleteShopStaff")]
        [HttpDelete]
        public JResult DeleteShopStaff(string innerid)
        {
            return _rewardsservice.DeleteShopStaff(innerid);
        }

        /// <summary>
        /// 商户职员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetShopStaffPageList")]
        [HttpPost]
        public BasePageList<ShopStaffViewModel> GetShopStaffPageList([FromBody]ShopStaffQueryModel query)
        {
            return _rewardsservice.GetShopStaffPageList(query);
        }

        #endregion

        #region 结算记录

        /// <summary>
        /// 添加结算记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddSettLog")]
        [HttpPost]
        public JResult AddSettLog([FromBody] SettlementLogModel model)
        {
            return _rewardsservice.AddSettLog(model);
        }

        /// <summary>
        /// 修改结算记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateSettLog")]
        [HttpPut]
        public JResult UpdateSettLog([FromBody] SettlementLogModel model)
        {
            return _rewardsservice.UpdateSettLog(model);
        }

        /// <summary>
        /// 删除结算记录
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("DelSettLog")]
        [HttpDelete]
        public JResult DelSettLog(string innerid)
        {
            return _rewardsservice.DelSettLog(innerid);
        }

        /// <summary>
        /// 删除结算记录中的一张图片
        /// </summary>
        /// <param name="innerid">记录id</param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [Route("DeleteSettPicture")]
        [HttpDelete]
        public JResult DeleteSettPicture(string innerid, string pic)
        {
            return _rewardsservice.DeleteSettPicture(innerid, pic);
        }

        /// <summary>
        /// 根据id获取结算记录信息
        /// </summary>
        /// <returns></returns>
        [Route("GetSettLogById")]
        [HttpGet]
        public JResult GetSettLogById(string innerid)
        {
            return _rewardsservice.GetSettLogById(innerid);
        }

        /// <summary>
        /// 结算记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetSettLogPageList")]
        [HttpPost]
        public BasePageList<SettlementLogViewModel> GetSettLogPageList([FromBody] SettlementLogQueryModel query)
        {
            return _rewardsservice.GetSettLogPageList(query);
        }

        /// <summary>
        /// 根据settid获取已结算的code列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetSettedCodePageList")]
        [HttpPost]
        public BasePageList<SettedCodeViewListModel> GetSettedCodePageList(SettedCodeQueryModel query)
        {
            return _rewardsservice.GetSettedCodePageList(query);
        }
        #endregion

        /// <summary>
        /// 获取礼券实例
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetCouponCode")]
        [HttpPost]
        public BasePageList<CouponCodeListModel> GetCouponCode([FromBody] CodeQueryModel query)
        {
            return _rewardsservice.GetCouponCode(query);
        }
        
    }
}

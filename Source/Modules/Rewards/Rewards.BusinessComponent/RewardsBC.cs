using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CCN.Modules.Rewards.BusinessEntity;
using CCN.Modules.Rewards.DataAccess;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Microsoft.Practices.ObjectBuilder2;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs.MerChant;

namespace CCN.Modules.Rewards.BusinessComponent
{
    /// <summary>
    /// 
    /// </summary>
    public class RewardsBC: BusinessComponentBase<RewardsDataAccess>
    {
        /// <summary>
        /// </summary>
        /// <param name="da"></param>
        public RewardsBC(RewardsDataAccess da)
            : base(da)
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
            if (model.Point == 0)
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "变更的积分不能为0"
                };
            }
            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;

            if (model.Type == 0)
            {
                model.Type = 1;
            }
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                model.Remark = "";
            }

            var result = DataAccess.ChangePoint(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "添加成功" : "添加失败"
            };
        }

        /// <summary>
        /// 获取会员积分记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustPointViewModel> GetCustPointLogPageList(CustPointQueryModel query)
        {
            var list = DataAccess.GetCustPointLogPageList(query);
            return list;
        }

        /// <summary>
        /// 积分兑换礼券
        /// </summary>
        /// <param name="model">兑换相关信息</param>
        /// <returns></returns>
        public JResult PointExchangeCoupon(CustPointExChangeCouponModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Custid) || string.IsNullOrWhiteSpace(model.Cardid))
            {
                return JResult._jResult(401, "参数不完整");
            }

            var couponModel = DataAccess.GetCouponById(model.Cardid);
            if (couponModel == null)
            {
                return JResult._jResult(402, "礼券不存在");
            }

            if (couponModel.Needpoint == null || couponModel.Needpoint == 0)
            {
                return JResult._jResult(403, "该礼券不可兑换");
            }

            if (couponModel.Count == 0)
            {
                return JResult._jResult(407, "库存不够");
            }

            //固定时间范围
            if (couponModel.Vtype.HasValue && couponModel.Vtype.Value == 1)
            {
                if (DateTime.Now > couponModel.Vend)
                {
                    return JResult._jResult(404, "礼券已过期");
                }
                model.Vstart = couponModel.Vstart;
                model.Vend = couponModel.Vend;
            }
            else if (couponModel.Value1 != null && couponModel.Value2 != null)
            {
                model.Vstart = DateTime.Now.AddDays(couponModel.Value1.Value);
                model.Vend = DateTime.Now.AddDays(couponModel.Value1.Value).AddDays(couponModel.Value2.Value);
            }

            var custTotalInfo = DataAccess.GetCustTotalInfo(model.Custid);
            if (custTotalInfo == null)
            {
                return JResult._jResult(405, "会员不存在");
            }

            if (custTotalInfo.Currpoint < couponModel.Needpoint)
            {
                return JResult._jResult(406, "积分不够");
            }

            //生成随机数
            model.Code = RandomUtility.GetRandomCode();
            //生成二维码位图
            var bitmap = BarCodeUtility.CreateBarcode(model.Code, 240, 240);
            var filename = QiniuUtility.GetFileName(Picture.card_qrcode);
            var stream = BarCodeUtility.BitmapToStream(bitmap);

            //上传图片到七牛云
            var qinniu = new QiniuUtility();
            model.QrCode = qinniu.Put(stream, "", filename);
            stream.Dispose();

            //开始兑换
            model.Createdtime = DateTime.Now;
            model.Sourceid = 2; //礼券来源  兑换
            model.Point = couponModel.Needpoint.Value;
            model.Remark = "积分兑换";

            //return JResult._jResult(10000,"测试");

            var result = DataAccess.PointExchangeCoupon(model);
            return JResult._jResult(
                result > 0 ? 0 : 400,
                result > 0 ? "兑换成功" : "兑换失败");
        }

        /// <summary>
        /// 登录奖励积分算法
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public int LoginAlgorithm(string custid)
        {
            var list = DataAccess.GetLoginPointRecord(custid).ToList();
            if (!list.Any())
            {
                return 10; //表示第一次登录，奖励10个积分
            }

            var nDate = DateTime.Now;

            if (list.Count(x => x.Createdtime != null && x.Createdtime.Value.ToString("yyyyMMdd") == nDate.ToString("yyyyMMdd")) > 0)
            {
                return 0; //今天奖励过了，不奖励了
            }

            var yesModel = list.FirstOrDefault(x => x.Createdtime != null && x.Createdtime.Value.ToString("yyyyMMdd") == nDate.AddDays(-1).ToString("yyyyMMdd"));
            if (yesModel != null) //表示昨天有登录奖励过，今天奖励的积分要在昨天的基础+N
            {
                list.RemoveAt(0); //删除昨天的记录
                var days = 1;  //记录已经连续登录days天，昨天的记录有，表示初始化连续1天
                foreach (var it in list)
                {
                    
                    //第一条数据是昨天的
                    if (it.Createdtime != null &&
                        it.Createdtime.Value.ToShortDateString()
                            .Equals(nDate.AddDays(-(days+1)).ToShortDateString()))
                    {
                        days ++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (days < 10)  //10天以内连续登录 ，在昨天的基础上+5
                {
                    return yesModel.Point + 5;
                }

                if (days >= 10 && days < 20) //10天以上 20天以内连续登录 ，在昨天的基础上+10
                {
                    return yesModel.Point + 10;
                }

                if (days == 20) //连续第21天登录
                {
                    return 180;
                }

                if (days > 20) //连续21天以上登录
                {
                    return 200;
                }
            }
            else //昨天没有登录，断了重新开始累计
            {
                return 10;
            }

            return 0;
        }

        /// <summary>
        /// 获取认证积分记录
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public IEnumerable<CustPointModel> GetAuthPointRecord(string custid)
        {
            return DataAccess.GetAuthPointRecord(custid);
        }

        /// <summary>
        /// 获取今天分享获得积分记录
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public int GetSharePointRecord(string custid)
        {
            return DataAccess.GetSharePointRecord(custid);
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
            return DataAccess.GetCouponPageList(query);
        }

        /// <summary>
        /// 添加礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public JResult AddCoupon(CouponInfoModel model)
        {
            model.Innerid = Guid.NewGuid().ToString();
            model.Count = model.Maxcount;
            model.Createdtime = DateTime.Now;
            model.IsEnabled = 1;
            var result = DataAccess.AddCoupon(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改礼券
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public JResult UpdateCoupon(CouponInfoModel model)
        {
            model.Count = null;
            model.Maxcount = null;
            model.Createdtime = null;
            model.Vtype = null;
            model.Vstart = null;
            model.Vend = null;
            model.Value1 = null;
            model.Value2 = null;
            model.IsEnabled = null;
            model.Modifiedtime = DateTime.Now;

            var result = DataAccess.UpdateCoupon(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 获取礼券信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public JResult GetCouponById(string innerid)
        {
            var model = DataAccess.GetCouponById(innerid);

            if (string.IsNullOrWhiteSpace(model?.ProductUrl))
                return JResult._jResult(model);

            var producturl = ConfigHelper.GetAppSettings("wechat_producturl");
            model.ProductUrl = string.Format(producturl, model.ProductUrl);

            return JResult._jResult(model);
        }

        /// <summary>
        /// 获取礼券信息 by code
        /// </summary>
        /// <param name="code">id</param>
        /// <returns></returns>
        public JResult GetCouponByCode(string code)
        {
            var model = DataAccess.GetCodeInfo(code);
            if (model != null && string.IsNullOrWhiteSpace(model.QrCode))
            {
                model.QrCode = UpdateQrcode(model.Code);
            }
            return JResult._jResult(model);
        }

        /// <summary>
        /// 更新礼券状态
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateStatus(string cardid, int status)
        {
            if (string.IsNullOrWhiteSpace(cardid) || (status != 0 && status !=1))
            {
               return  JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.UpdateStatus(cardid, status);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 修改礼券库存
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public JResult UpdateStock(CouponInfoModel model)
        {
            if (model?.Count == null || model.Count == 0)
            {
                return JResult._jResult(401,"参数无效");
            }
            var result = DataAccess.UpdateStock(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改礼券有效期
        /// </summary>
        /// <param name="model">礼券信息</param>
        /// <returns></returns>
        public JResult UpdateValidity(CouponInfoModel model)
        {
            var m = DataAccess.GetCouponById(model.Innerid);
            if (m == null)
            {
                return JResult._jResult(401, "礼券不存在");
            }
            if (m.Vtype == 1)
            {
                if (model.Vend < m.Vend)
                {
                    return JResult._jResult(402, "有效期只能延长");
                }
            }
            else
            {
                if (model.Value2 < m.Value2)
                {
                    return JResult._jResult(402, "有效期只能延长");
                }
            }
            var result = DataAccess.UpdateValidity(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 礼券与微信小店产品绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult BindWechatProduct(CouponCardProduct model)
        {
            if (string.IsNullOrWhiteSpace(model?.Cardid) || string.IsNullOrWhiteSpace(model.ProductId))
            {
                return JResult._jResult(402, "参数不完整！");
            }
            var count = DataAccess.ValidatedBindRepeat(model.ProductId);
            if (count > 0)
            {
                return JResult._jResult(401,"该商品ID已绑定其他礼券！");
            }
            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            var result = DataAccess.BindWechatProduct(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 礼券与微信小店产品解除绑定
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public JResult UnBindWechatProduct(string cardid)
        {
            var result = DataAccess.UnBindWechatProduct(cardid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 核销记录查询列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CodeViewListModel> GetCodeRecord(CodeQueryModel query)
        {
            return DataAccess.GetCodeRecord(query);
        }

        /// <summary>
        /// 核销记录查询列表-汇总
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JResult GetCodeRecordTotal(CodeQueryModel query)
        {
            var model = DataAccess.GetCodeRecordTotal(query);
            return JResult._jResult(model);
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
            if (query?.Status == null || string.IsNullOrWhiteSpace(query.Custid))
            {
                return new BasePageList<MyCodeViewListModel>();
            }

            var list = DataAccess.GetMyCodeList(query);

            ////查询不可用券，将可用的券的总数一起带过
            //if (query.Status == 2)
            //{
            //    list.iTotalDisplayRecords = DataAccess.GetMyUsableCodeTotal(query.Custid);
            //}

            return list;
        }

        /// <summary>
        /// 我的礼券详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JResult GetCodeInfo(string code)
        {
            var model = DataAccess.GetCodeInfo(code);
            if (model != null && string.IsNullOrWhiteSpace(model.QrCode))
            {
                model.QrCode = UpdateQrcode(model.Code);
            }
            return JResult._jResult(model);
        }

        public string UpdateQrcode(string code)
        {
            //生成二维码位图
            var bitmap = BarCodeUtility.CreateBarcode(code, 240, 240);
            var filename = QiniuUtility.GetFileName(Picture.card_qrcode);
            var stream = BarCodeUtility.BitmapToStream(bitmap);
            //上传图片到七牛云
            var qinniu = new QiniuUtility();
            var qrcode = qinniu.Put(stream, "", filename);
            stream.Dispose();

            Task.Run(() =>
            {
                DataAccess.UpdateQrcode(code, qrcode);
            });
            return qrcode;
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
            var list = DataAccess.GetMallCouponPageList(query);

            if (list.aaData == null || !list.aaData.Any()) return list;

            var producturl = ConfigHelper.GetAppSettings("wechat_producturl");

            foreach (var item in list.aaData.Where(item => !string.IsNullOrWhiteSpace(item.ProductUrl)))
            {
                item.ProductUrl = string.Format(producturl, item.ProductUrl);
            }

            return list;
        }

        /// <summary>
        /// 商城搜索商户列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<ShopMallViewList> GetMallShopPageList(ShopMallQueryModel query)
        {
            var list = DataAccess.GetMallShopPageList(query);
            return list;
        }

        #endregion

        #region 礼券对外接口

        /// <summary>
        /// 根据规则发放礼券
        /// </summary>
        /// <returns></returns>
        public JResult SendCoupon(SendCouponModel model)
        {
            var recordList = DataAccess.GetCouponRecord(model.Custid, model.Sourceid);
            if (recordList.Any())
            {
                return JResult._jResult(401, "重复奖励");
            }

            var ruleList = DataAccess.GetCouponRule().ToList();
            if (!ruleList.Any())
            {
                return JResult._jResult(402, "没有规则");
            }

            var ableRuleList = ruleList.Where(x => x.Actiontype == model.ActionType);

            foreach (var rModel in ableRuleList)
            {
                var couponModel = DataAccess.GetCouponById(rModel.Cardid);

                if (couponModel.Vtype == 2 && couponModel.Value1 != null && couponModel.Value2 != null)
                {
                    couponModel.Vstart = DateTime.Now.AddDays(couponModel.Value1.Value);
                    couponModel.Vend = DateTime.Now.AddDays(couponModel.Value1.Value).AddDays(couponModel.Value2.Value);
                }

                var codeList = new List<CouponCodeModel>();

                for (var i = 0; i < rModel.Num; i++)
                {
                    //生成随机数
                    var code = RandomUtility.GetRandomCode();
                    //生成二维码位图
                    var bitmap = BarCodeUtility.CreateBarcode(code, 240, 240);
                    var filename = QiniuUtility.GetFileName(Picture.card_qrcode);
                    var stream = BarCodeUtility.BitmapToStream(bitmap);
                    //上传图片到七牛云
                    var qinniu = new QiniuUtility();
                    var qrcode = qinniu.Put(stream, "", filename);
                    stream.Dispose();
                    codeList.Add(new CouponCodeModel
                    {
                        Code = code,
                        QrCode = qrcode,
                        Vstart = couponModel.Vstart,
                        Vend = couponModel.Vend
                    });
                }

                var sendModel = new CouponSendModel
                {
                    Cardid = rModel.Cardid,
                    Createdtime = DateTime.Now,
                    Number = rModel.Num,
                    Custid = model.Custid,
                    Sourceid = model.Sourceid,
                    ListCode = codeList
                };

                DataAccess.CouponToCustomer(sendModel);
            }

            return JResult._jResult(0, "发放成功");
        }

        /// <summary>
        /// 批量购买礼券
        /// </summary>
        /// <param name="model">购买信息</param>
        /// <returns></returns>
        public JResult WholesaleCoupon(CouponBuyModel model)
        {
            LoggerFactories.CreateLogger().Write("购买礼券参数：" + JsonConvert.SerializeObject(model), TraceEventType.Information);
            if (string.IsNullOrWhiteSpace(model.Order?.buyer_openid) || string.IsNullOrWhiteSpace(model.Order.product_id))
            {
                return JResult._jResult(401, "参数不正确");
            }

            var couponModel = DataAccess.GetCouponByProductId(model.Order.product_id);
            if (couponModel == null)
            {
                return JResult._jResult(402, "该商品ID没有绑定礼券");
            }

            var custid = DataAccess.GetCustidByOpenid(model.Order.buyer_openid);
            if (string.IsNullOrWhiteSpace(custid))
            {
                return JResult._jResult(403, "会员不存在");
            }

            if (couponModel.Count < model.Order.product_count)
            {
                return JResult._jResult(407, "库存不够");
            }

            //固定时间范围
            if (couponModel.Vtype.HasValue && couponModel.Vtype.Value == 1)
            {
                if (DateTime.Now > couponModel.Vend)
                {
                    return JResult._jResult(404, "礼券已过期");
                }
            }
            else if (couponModel.Value1 != null && couponModel.Value2 != null)
            {
                couponModel.Vstart = DateTime.Now.AddDays(couponModel.Value1.Value);
                couponModel.Vend = DateTime.Now.AddDays(couponModel.Value1.Value).AddDays(couponModel.Value2.Value);
            }
            
            var codeList = new List<CouponCodeModel>();

            for (var i = 0; i < model.Order.product_count; i++)
            {
                try
                {
                    //生成随机数
                    var code = RandomUtility.GetRandomCode();
                    //生成二维码位图
                    var bitmap = BarCodeUtility.CreateBarcode(code, 240, 240);
                    var filename = QiniuUtility.GetFileName(Picture.card_qrcode);
                    var stream = BarCodeUtility.BitmapToStream(bitmap);
                    //上传图片到七牛云
                    var qinniu = new QiniuUtility();
                    var qrcode = qinniu.Put(stream, "", filename) ?? Path.GetFileName(filename);
                    stream.Dispose();
                    codeList.Add(new CouponCodeModel
                    {
                        Code = code,
                        QrCode = qrcode,
                        Vstart = couponModel.Vstart,
                        Vend = couponModel.Vend
                    });
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("购买礼券生成失败" + i, TraceEventType.Error, ex);
                }
            }
            
            var sendModel = new CouponSendModel
            {
                Cardid = couponModel.Innerid,
                Createdtime = DateTime.Now,
                Number = model.Order.product_count,
                Custid = custid,
                ListCode = codeList
            };

            sendModel.Sourceid = 1;//购买
            var result = DataAccess.CouponToCustomer(sendModel);

            Task.Run(() =>
            {
                if (result == 1)
                {
                    OrderApi.SetdeliveryOrder(ConfigHelper.GetAppSettings("APPID"), model.Order.order_id, "", "", 0);
                }
            });

            return JResult._jResult(
                result > 0 ? 0 : 400,
                result > 0 ? "购买成功" : "购买失败");
        }

        /// <summary>
        /// 保存购买订单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveOrder(CouponBuyModel model)
        {
            model.innerid = Guid.NewGuid().ToString();
            return DataAccess.SaveOrder(model);
        }

        /// <summary>
        /// 修改购买订单处理结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateOrderResult(CouponBuyModel model)
        {
            return DataAccess.UpdateOrderResult(model);
        }

        /// <summary>
        /// 获取发送礼券失败的订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<OrderViewList> GetOrderList(OrderQuery query)
        {
            return DataAccess.GetOrderList(query);
        }

        /// <summary>
        /// 处理购买失败的订单
        /// </summary>
        /// <param name="innerid">订单内部id</param>
        /// <returns></returns>
        public JResult HandlOrder(string innerid)
        {
            var model = DataAccess.GetOrderInfo(innerid);
            if (model == null)
            {
                return  JResult._jResult(500, "订单不存在");
            }

            if (model.result == 0)
            {
                return JResult._jResult(501, "订单已处理");
            }

            var orderModel = new Order
            {
                product_count = model.product_count,
                product_id = model.product_id,
                buyer_openid = model.buyer_openid,
                order_id = model.order_id
            };

            var bugModel = new CouponBuyModel
            {
                Order = orderModel,
                innerid = model.innerid
            };

            var jresult = WholesaleCoupon(bugModel);
            if (jresult.errcode == 0)
            {
                //处理成功

                bugModel.result = 0;
                bugModel.resultdesc = model.resultdesc + "[" + model.result + "]-->购买成功[后期处理]";
                DataAccess.UpdateOrderResult(bugModel);
                return JResult._jResult(0, "处理成功");
            }
            
            bugModel.result = jresult.errcode;
            bugModel.resultdesc = jresult.errmsg.ToString();
            DataAccess.UpdateOrderResult(bugModel);
            return JResult._jResult(jresult.errcode, jresult.errmsg);
        }

        /// <summary>
        /// 礼券核销
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult CancelCoupon(CancelModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Shopid) || string.IsNullOrWhiteSpace(model.Code))
            {
                return JResult._jResult(401,"参数不完整");
            }

            var codeModel = DataAccess.GetCode(model.Code);
            if (codeModel == null)
            {
                return JResult._jResult(402, "code不存在");
            }

            if (!codeModel.Shopid.Equals(model.Shopid))
            {
                return JResult._jResult(403, "非本店礼券");
            }

            if (codeModel.Gettime == null)
            {
                return JResult._jResult(500, "code数据异常");
            }

            if (codeModel.IsUsed == 1)
            {
                return JResult._jResult(404, "该券已被使用");
            }

            var cardModel = DataAccess.GetCouponById(codeModel.Cardid);
            if (cardModel == null)
            {
                return JResult._jResult(405, "礼券不存在");
            }

            if (cardModel.IsEnabled != 1)
            {
                return JResult._jResult(406, "礼券被禁用");
            }

            var nowDate = DateTime.Now;

            if (cardModel.Vtype.HasValue && cardModel.Vtype.Value == 1 )
            {
                if (cardModel.Vstart > nowDate && cardModel.Vend < nowDate)
                {
                    return JResult._jResult(407, "不在有效期范围内");
                }
            }
            else if (cardModel.Vtype.HasValue && cardModel.Vtype.Value == 2)
            {
                if (cardModel.Value1 == null || cardModel.Value2 == null)
                {
                    return JResult._jResult(407, "不在有效期范围内");
                }

                var startTime = codeModel.Gettime.Value.AddDays(cardModel.Value1.Value);
                var endTime = codeModel.Gettime.Value.AddDays(cardModel.Value1.Value).AddDays(cardModel.Value2.Value);
                if (startTime > nowDate && endTime < nowDate)
                {
                    return JResult._jResult(407, "不在有效期范围内");
                }
            }

            var result = DataAccess.CancelCoupon(model.Code);

            return JResult._jResult(
                result > 0 ? 0 : 400,
                result > 0 ? "核销成功" : "核销失败");
        }

        /// <summary>
        /// 查询已核销的礼券
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JResult GetCoupon(CardCancelSummaryQueryModel query)
        {
            var list = DataAccess.GetCoupon(query).ToList();

            if (!list.Any()) return JResult._jResult(0, list);

            foreach (var item in list)
            {
                //item.CanedCount = item.CanedCount - item.SettedCount;
                item.TotalPrice = item.CostPrice*item.CanedCount;
            }

            return JResult._jResult(0, list);
        }

        #endregion

        #region 商户管理

        /// <summary>
        /// 根据id获取商户信息
        /// </summary>
        /// <returns></returns>
        public JResult GetShopById(string innerid)
        {
            var model = DataAccess.GetShopById(innerid);
            if (model != null)
            {
                model.Password = null;
            }
            return JResult._jResult(model);
        }

        /// <summary>
        /// 根据id获取商户信息（包含关联信息）
        /// </summary>
        /// <returns></returns>
        public JResult GetShopViewById(string innerid)
        {
            var model = DataAccess.GetShopViewById(innerid);
            if (model == null)
            {
                return JResult._jResult(400, "");
            }
            var typelist = DataAccess.GetMallShopCardTypeNameList(innerid).ToList();
            if (typelist.Any())
            {
                model.CardTypeNames = typelist.JoinStrings(",");
            }
            
            return JResult._jResult(0, model);
        }

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        public JResult ShopLogin(ShopLoginInfo model)
        {
            model.Password = Encryptor.EncryptAes(model.Password);
            var shopModel = DataAccess.GetShopModel(model.Shopcode, model.Password);

            if (shopModel == null)
            {
                return JResult._jResult(400, "登录名或密码错误");
            }

            if (shopModel.Status == 2)
            {
                return JResult._jResult(401, "商户被禁用");
            }

            shopModel.Password = "";

            return JResult._jResult(0, shopModel);
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <returns></returns>
        public JResult AddShop(ShopModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Shopname) 
                || string.IsNullOrWhiteSpace(model.Password))
            {
                return JResult._jResult(401, "参数不完整");
            }

            if (DataAccess.CheckShopName(model.Shopname) > 0)
            {
                return JResult._jResult(402,"商户名称重复");
            }

            model.Code = DataAccess.GetMaxCode() + 1;

            if (model.Code < 10)
            {
                model.Shopcode = "SP000" + model.Code;
            }
            else if (model.Code >= 10 && model.Code < 100)
            {
                model.Shopcode = "SP00" + model.Code;
            }
            else if (model.Code >= 100 && model.Code < 1000)
            {
                model.Shopcode = "SP0" + model.Code;
            }
            else
            {
                model.Shopcode = "SP" + model.Code;
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            model.Status = 1;
            model.Password = Encryptor.EncryptAes(model.Password);
            var result = DataAccess.AddShop(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 更新商户
        /// </summary>
        /// <returns></returns>
        public JResult UpdateShop(ShopModel model)
        {
            model.Createdtime = null;
            model.Modifiedtime = DateTime.Now;
            model.Password = null;
            var result = DataAccess.UpdateShop(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JResult UpdateShopPassword(string innerid, string password)
        {
            password = Encryptor.EncryptAes(password);
            var result = DataAccess.UpdateShopPassword(innerid, password);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改商户状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateShopStatus(string innerid, int status)
        {
            var result = DataAccess.UpdateShopStatus(innerid, status);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 删除商户
        /// </summary>
        /// <returns></returns>
        public JResult DeleteShop(string innerid)
        {
            var result = DataAccess.DeleteShop(innerid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 商户列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<ShopViewModel> GetShopPageList(ShopQueryModel query)
        {
            return DataAccess.GetShopPageList(query);
        }

        /// <summary>
        /// 获取商户list 下拉
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemShop> GetShopList()
        {
            return DataAccess.GetShopList();
        }
        #endregion

        #region 商户职员管理

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <returns></returns>
        public JResult GetShopStaffModel(StaffLoginInfo model)
        {
            model.Password = Encryptor.EncryptAes(model.Password);
            var shopstaffModel = DataAccess.GetShopStaffModel(model.Loginname, model.Password);

            if (shopstaffModel.StaffModel == null)
            {
                return JResult._jResult(400, "登录名或密码错误");
            }

            if (shopstaffModel.StaffModel.Status == 2)
            {
                return JResult._jResult(401, "该职员被禁用");
            }
            
            if (shopstaffModel.ShopModel == null)
            {
                return JResult._jResult(402, "您职员所在商户不存在");
            }
            if (shopstaffModel.ShopModel.Status == 2)
            {
                return JResult._jResult(403, "您职员所在商户被禁用");
            }

            shopstaffModel.StaffModel.Password = "";
            shopstaffModel.ShopModel.Password = "";

            return JResult._jResult(0, shopstaffModel);
        }

        /// <summary>
        /// 根据id获取商户职员信息
        /// </summary>
        /// <returns></returns>
        public JResult GetShopStaffById(string innerid)
        {
            var model = DataAccess.GetShopStaffById(innerid);
            return JResult._jResult(model);
        }
        
        /// <summary>
        /// 添加职员
        /// </summary>
        /// <returns></returns>
        public JResult AddShopStaff(ShopStaffModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Staffname)||
                string.IsNullOrWhiteSpace(model.Loginname)
                || string.IsNullOrWhiteSpace(model.Password))
            {
                return JResult._jResult(401, "参数不完整");
            }

            //if (DataAccess.CheckShopStaffName(model.Staffname) > 0)
            //{
            //    //return JResult._jResult(402, "商户职员名称重复");
            //}

            if (DataAccess.CheckShopStaffLoginName(model.Loginname) > 0)
            {
                return JResult._jResult(402, "商户职员登录名重复");
            }
            
            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            model.Status = 1;
            model.Password = Encryptor.EncryptAes(model.Password);
            var result = DataAccess.AddShopStaff(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 更新商户Staff
        /// </summary>
        /// <returns></returns>
        public JResult UpdateShopStaff(ShopStaffModel model)
        {
            model.Createdtime = null;
            model.Modifiedtime = DateTime.Now;
            var result = DataAccess.UpdateShopStaff(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改商户Staff状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateShopStaffStatus(string innerid, int status)
        {
            var result = DataAccess.UpdateShopStaffStatus(innerid, status);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 删除商户Staff
        /// </summary>
        /// <returns></returns>
        public JResult DeleteShopStaff(string innerid)
        {
            var result = DataAccess.DeleteShopStaff(innerid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 商户职员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<ShopStaffViewModel> GetShopStaffPageList(ShopStaffQueryModel query)
        {
            return DataAccess.GetShopStaffPageList(query);
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
            model.Innerid = Guid.NewGuid().ToString();
            if (!model.SettTime.HasValue)
            {
                model.SettTime = DateTime.Now;
            }

            var shopModel = DataAccess.GetShopById(model.Shopid);
            if (shopModel == null)
            {
                return JResult._jResult(401, "商户不存在");
            }
            model.Orderid = shopModel.Shopcode + DateTime.Now.ToString("yyMMddHHmmss");
            var result = DataAccess.AddSettLog(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改结算记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateSettLog(SettlementLogModel model)
        {
            model.Orderid = null;
            model.SettCycleStart = null;
            model.SettCycleEnd = null;
            model.Shopid = null;
            var result = DataAccess.UpdateSettLog(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 删除结算记录中的一张图片
        /// </summary>
        /// <param name="innerid">记录id</param>
        /// <param name="pic"></param>
        /// <returns></returns>
        public JResult DeleteSettPicture(string innerid, string pic)
        {
            var settModel = DataAccess.GetSettLogById(innerid);
            if (settModel == null)
            {
                return JResult._jResult(401,"记录不存在");
            }

            var listP = settModel.Pictures.Split(',').ToList();
            listP.Remove(pic);
            settModel.Pictures = listP.JoinStrings(",");

            var result = DataAccess.UpdateSettLogPic(innerid, settModel.Pictures);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 删除结算记录
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DelSettLog(string innerid)
        {
            var result = DataAccess.DelSettLog(innerid);
            return JResult._jResult(result);
        }
        
        /// <summary>
        /// 根据id获取结算记录信息
        /// </summary>
        /// <returns></returns>
        public JResult GetSettLogById(string innerid)
        {
            var model = DataAccess.GetSettLogById(innerid);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 结算记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<SettlementLogViewModel> GetSettLogPageList(SettlementLogQueryModel query)
        {
            return DataAccess.GetSettLogPageList(query);
        }


        /// <summary>
        /// 根据settid获取已结算的code列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<SettedCodeViewListModel> GetSettedCodePageList(SettedCodeQueryModel query)
        {
            return DataAccess.GetSettedCodePageList(query);
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
            var list = DataAccess.GetShopAreaByCityid(cityid);
            return JResult._jResult(0, list);
        }

        /// <summary>
        /// 根据区获取商户列表
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public JResult GetShopByArea(string area)
        {
            var list = DataAccess.GetShopByArea(area);
            return JResult._jResult(0, list);
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
            if (string.IsNullOrWhiteSpace(model?.Custid) || string.IsNullOrWhiteSpace(model.Cardid))
            {
                return JResult._jResult(401, "参数不完整");
            }

            //生成随机数
            model.Code = RandomUtility.GetRandomCode();
            var result = DataAccess.PointToCoupon(model);

            switch (result)
            {
                case 402: return JResult._jResult(result, "礼券不存在");
                case 403: return JResult._jResult(result, "该礼券不可兑换");
                case 404: return JResult._jResult(result, "库存不够");
                case 405: return JResult._jResult(result, "礼券已过期");
                case 406: return JResult._jResult(result, "会员不存在");
                case 407: return JResult._jResult(result, "积分不够");
                case 0: return JResult._jResult(408, "兑换异常");
                default:
                    Task.Run(() =>
                    {
                        UpdateQrcode(model.Code);
                    });
                    return JResult._jResult(0, "");
            }
        }

        #endregion

        /// <summary>
        /// 获取礼券实例
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CouponCodeListModel> GetCouponCode(CodeQueryModel query)
        {
            return DataAccess.GetCouponCode(query);
        }
    }
}

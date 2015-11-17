using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CCN.Modules.Rewards.BusinessEntity;
using CCN.Modules.Rewards.DataAccess;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
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
            if (model == null)
            {
                return JResult._jResult(401, "参数不正确");
            }
            if (string.IsNullOrWhiteSpace(model.Custid))
            {
                return JResult._jResult(402, "会员不存在");
            }
            if (model.Point == 0)
            {
                return JResult._jResult(403, "积分不够");
            }
            if (string.IsNullOrWhiteSpace(model.Cardid))
            {
                return JResult._jResult(404, "礼券不存在");
            }

            //生成随机数
            model.Code = RandomUtility.GetRandomCode();
            //生成二维码位图
            var bitmap = BarCodeUtility.CreateBarcode(model.Code, 240, 240);

            //保存二维码图片到临时文件夹
            var filename = string.Concat("card_qrcode_", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ".jpg");
            var filepath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "TempFile\\", filename);
            bitmap.Save(filepath);

            //上传图片到七牛云
            var qinniu = new QiniuUtility();
            model.QrCode = qinniu.PutFile(filepath, "", filename);

            //删除本地临时文件
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            //开始兑换
            model.Createdtime = DateTime.Now;
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
        public BasePageList<CouponInfoModel> GetCouponPageList(CouponQueryModel query)
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

        #endregion

        #region 礼券对外接口

        /// <summary>
        /// 批量购买礼券
        /// </summary>
        /// <param name="model">购买信息</param>
        /// <returns></returns>
        public JResult WholesaleCoupon(CouponBuyModel model)
        {
            LoggerFactories.CreateLogger().Write("购买礼券參數：" + JsonConvert.SerializeObject(model), TraceEventType.Information);
            if (string.IsNullOrWhiteSpace(model?.Openid) || string.IsNullOrWhiteSpace(model.ProductId))
            {
                return JResult._jResult(401, "参数不正确");
            }

            var couponModel = DataAccess.GetCouponByProductId(model.ProductId);
            if (couponModel == null)
            {
                return JResult._jResult(402, "该商品ID没有绑定礼券");
            }

            var custid = DataAccess.GetCustidByOpenid(model.Openid);
            if (string.IsNullOrWhiteSpace(custid))
            {
                return JResult._jResult(403, "会员不存在");
            }
                        
            var codeList = new List<CouponCodeModel>();

            for (int i = 0; i < model.Number; i++)
            {
                //生成随机数
                var code = RandomUtility.GetRandomCode();
                //生成二维码位图
                var bitmap = BarCodeUtility.CreateBarcode(code, 240, 240);
                var filename = QiniuUtility.GetFileName(Picture.card_qrcode);
                var filepath = QiniuUtility.GetFilePath(filename);
                bitmap.Save(filepath);

                //上传图片到七牛云
                var qinniu = new QiniuUtility();
                var qrcode = qinniu.PutFile(filepath, "", filename);

                //删除本地临时文件
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                codeList.Add(new CouponCodeModel
                {
                    Code = code,
                    QrCode = qrcode
                });
            }
            
            var sendModel = new CouponSendModel
            {
                Cardid = couponModel.Innerid,
                Createdtime = DateTime.Now,
                Number = model.Number,
                Custid = custid,
                ListCode = codeList
            };

            var result = DataAccess.CouponToCustomer(sendModel);
            return JResult._jResult(
                result > 0 ? 0 : 400,
                result > 0 ? "购买成功" : "购买失败");
        }

        /// <summary>
        /// 礼券核销
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JResult CancelCoupon(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return JResult._jResult(401,"code空");
            }

            var codeModel = DataAccess.GetCode(code);
            if (codeModel == null)
            {
                return JResult._jResult(402, "code不存在");
            }

            if (codeModel.Gettime == null)
            {
                return JResult._jResult(500, "code数据异常");
            }

            if (codeModel.Usedtime != null)
            {
                return JResult._jResult(403, "该券已被使用");
            }

            var cardModel = DataAccess.GetCouponById(codeModel.Cardid);
            if (cardModel == null)
            {
                return JResult._jResult(404, "礼券不存在");
            }

            if (cardModel.IsEnabled != 1)
            {
                return JResult._jResult(405, "礼券被禁用");
            }

            var nowDate = DateTime.Now;

            if (cardModel.Vtype.HasValue && cardModel.Vtype.Value == 1 )
            {
                if (cardModel.Vstart > nowDate && cardModel.Vend < nowDate)
                {
                    return JResult._jResult(406, "不在有效期范围内");
                }
            }
            else if (cardModel.Vtype.HasValue && cardModel.Vtype.Value == 2)
            {
                if (cardModel.Value1 == null || cardModel.Value2 == null)
                {
                    return JResult._jResult(406, "不在有效期范围内");
                }

                var startTime = codeModel.Gettime.Value.AddDays(cardModel.Value1.Value);
                var endTime = codeModel.Gettime.Value.AddDays(cardModel.Value1.Value).AddDays(cardModel.Value2.Value);
                if (startTime > nowDate && endTime < nowDate)
                {
                    return JResult._jResult(406, "不在有效期范围内");
                }
            }

            var result = DataAccess.CancelCoupon(code);

            return JResult._jResult(
                result > 0 ? 0 : 400,
                result > 0 ? "核销成功" : "核销失败");
        }

        #endregion
    }
}

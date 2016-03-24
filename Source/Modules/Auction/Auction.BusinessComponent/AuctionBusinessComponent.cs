using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Auction.BusinessEntity;
using CCN.Modules.Auction.DataAccess;
using Cedar.Core.ApplicationContexts;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Foundation.WeChat.WxPay.Business.WxPay.Entity;
using Cedar.Foundation.WeChat.WxPay.Business;

namespace CCN.Modules.Auction.BusinessComponent
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionBusinessComponent : BusinessComponentBase<AuctionDataAccess>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="da"></param>
        public AuctionBusinessComponent(AuctionDataAccess da) : base(da)
        {

        }

        #region 拍卖车辆基本信息

        /// <summary>
        /// 获取正在拍卖的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetAuctioningList(AuctionCarInfoQueryModel query)
        {
            return DataAccess.GetAuctioningList(query);
        }

        /// <summary>
        /// 获取拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetAuctionList(AuctionCarInfoQueryModel query)
        {
            return DataAccess.GetAuctionList(query);
        }

        /// <summary>
        /// 获取拍卖车辆详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctionInfoById(string id)
        {
            var model = DataAccess.GetAuctionInfoById(id);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 获取拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctionViewById(string id)
        {
            var model = DataAccess.GetAuctionViewById(id);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 获取正在拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctioningViewById(string id)
        {
            var model = DataAccess.GetAuctionViewById(id);
            if (model == null)
            {
                return JResult._jResult(400, "暂无数据");
            }
            model.mobile = null;
            return JResult._jResult(model);
        }

        /// <summary>
        /// 添加拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddAuctionCar(AuctionCarInfoModel model)
        {
            //判断拍卖开始/结束时间是否在合理范围内
            if (model.publishedtime >= model.validtime || model.publishedtime <= DateTime.Now)
            {
                return JResult._jResult(401, "请输入合理的拍卖时间");
            }
            model.createrid = ApplicationContext.Current.UserId;
            model.createdtime = DateTime.Now;
            model.Innerid = Guid.NewGuid().ToString();
            model.no = "A" + GetOrderNumber();
            model.status = 1;
            var result = DataAccess.AddAuctionCar(model);
            return JResult._jResult
            (
                result > 0 ? 0 : 400,
                model.Innerid
            );
        }

        /// <summary>
        /// 修改拍卖车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult UpdateAuctionCar(AuctionCarInfoModel model)
        {
            //if (string.IsNullOrWhiteSpace(model?.Innerid) || model.model_id == null || model.colorid == null ||
            //    model.register_date == null || model.cityid == null || model.mileage == null)
            //{
            //    return JResult._jResult(401, "参数不完整");
            //}

            if (string.IsNullOrWhiteSpace(model?.Innerid))
            {
                return JResult._jResult(401, "参数不完整");
            }
            //判断拍卖开始/结束时间是否在合理范围内
            if (model.publishedtime >= model.validtime || model.publishedtime <= DateTime.Now)
            {
                return JResult._jResult(402, "请输入合理的拍卖时间");
            }

            //model.createrid = "";
            //model.createdtime = null;
            model.modifierid = ApplicationContext.Current.UserId;
            model.modifiedtime = DateTime.Now;
            //model.publisherid = "";
            //model.publishedtime = null;
            //model.dealerid = "";
            //model.deletedtime = null;
            var result = DataAccess.UpdateAuctionCar(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改拍卖车辆(状态)
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult UpdateAuctionCarStatus(AuctionCarInfoModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Innerid))
            {
                return JResult._jResult(401, "参数不完整");
            }
            var result = DataAccess.UpdateAuctionCarStatus(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 删除拍卖车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        public JResult DeleteAuctionCar(AuctionCarInfoModel model)
        {
            model.deleterid = ApplicationContext.Current.UserId;
            model.deletedtime = DateTime.Now;
            var result = DataAccess.DeleteAuctionCar(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 发布拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        public JResult PublishAuctionCar(AuctionCarInfoModel model)
        {
            model.publisherid = ApplicationContext.Current.UserId;
            model.publishedtime = DateTime.Now;
            var result = DataAccess.PublishAuctionCar(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 成交拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        public JResult DealAuctionCar(AuctionCarInfoModel model)
        {
            if (model?.dealedprice == null)
            {
                return JResult._jResult(401, "参数不完整");
            }
            model.dealerid = ApplicationContext.Current.UserId;
            model.dealedtime = DateTime.Now;
            var result = DataAccess.DealAuctionCar(model);
            return JResult._jResult(result);
        }

        #endregion

        #region 图片处理

        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        public JResult GetCarPictureByCarid(string carid)
        {
            var list = DataAccess.GetCarPictureByCarid(carid);
            return JResult._jResult(list);
        }

        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult SaveAuctionCarPicture(AuctionPictureListModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Carid) || (model.DelIds.Count == 0 && model.AddPaths.Count == 0))
            {
                return JResult._jResult(401, "参数不完整");
            }

            var result = 0;
            //获取即将删除的图片
            List<AuctionCarPictureModel> picedList = null;

            //only delete
            if (model.DelIds.Count > 0 && model.AddPaths.Count == 0)
            {
                picedList = DataAccess.GetCarPictureByIds(model.DelIds).ToList();
                result = DataAccess.DelCarPictureList(model.DelIds, model.Carid);
            }
            //only add
            else if (model.DelIds.Count == 0 && model.AddPaths.Count > 0)
            {
                result = DataAccess.AddAuctionPictureList(model.AddPaths, model.Carid);
            }
            else if (model.DelIds.Count > 0 && model.AddPaths.Count > 0)
            {
                picedList = DataAccess.GetCarPictureByIds(model.DelIds).ToList();
                result = DataAccess.SaveCarPicture(model);
            }

            switch (result)
            {
                case 402:
                    return JResult._jResult(402, "图片数量不对");
                case 0:
                    return JResult._jResult(400, "批量删除图片失败");
            }

            //异步删除七牛上的图片
            Task.Run(() =>
            {
                if (picedList == null || !picedList.Any())
                    return;

                var qiniu = new QiniuUtility();
                foreach (var item in picedList.Where(item => !string.IsNullOrWhiteSpace(item?.Path)))
                {
                    qiniu.DeleteFile(item.Path);
                }
            });

            return JResult._jResult(0, "批量操作图片成功");
        }

        #endregion

        #region 竞拍

        /// <summary>
        /// 获取竞拍人列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarParticipantViewModel> GetAuctionParticipantList(AuctionCarParticipantQueryModel query)
        {
            return DataAccess.GetAuctionParticipantList(query);
        }


        /// <summary>
        /// 添加拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddParticipant(AuctionCarParticipantModel model)
        {
            if (model?.Mobile == null || model.Amount == null || model.Auctionid == null)
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Createrid = ApplicationContext.Current.UserId;
            model.Createdtime = DateTime.Now;
            model.Modifierid = "";
            model.Modifiedtime = null;
            model.Innerid = Guid.NewGuid().ToString();
            model.orderno = "P" + GetOrderNumber();
            model.status = 1;
            var result = DataAccess.AddParticipant(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 更新竞价信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateParticipant(AuctionCarParticipantModel model)
        {
            var result = DataAccess.UpdateParticipant(model);
            //更新成功并且状态为成交（5）时，更新其他付款状态，并且更新车辆拍卖状态,更新成“成交”
            if (result == 1 && model.status == 5)
            {
                var auctioncar = new AuctionCarInfoModel();
                auctioncar.status = 7;
                auctioncar.Innerid = model.Auctionid;
                DataAccess.UpdateAuctionCar(auctioncar);

                //更新其他竞价信息 
                model.status = 7;
                DataAccess.UpdateOtherParticipant(model);

            }
            return JResult._jResult(result);
        }

        /// <summary>
        /// 若已成交更改其他竞价信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateOtherParticipant(AuctionCarParticipantModel model)
        {
            var result = DataAccess.UpdateOtherParticipant(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 根据ID获取出价详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetAuctionParticipantByID(string innerid)
        {
            var result = DataAccess.GetAuctionParticipantByID(innerid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 根据订单号获取出价详情
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public JResult GetAuctionParticipantByOrderNo(string orderno)
        {
            var result = DataAccess.GetAuctionParticipantByOrderNo(orderno);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 中标拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult BidParticipant(AuctionCarParticipantModel model)
        {
            //if (model.Isbid == 1)
            //{
            //    var chkRes = DataAccess.CheckBidParticipant(model.Auctionid);
            //    if (chkRes > 0)
            //    {
            //        return JResult._jResult(401, "只能一人中标");
            //    }
            //}

            var bidModel = new AuctionCarParticipantModel
            {
                Innerid = model.Innerid,
                Modifierid = ApplicationContext.Current.UserId,
                Modifiedtime = DateTime.Now
                //,
                //Isbid = model.Isbid
            };
            var result = DataAccess.BidParticipant(bidModel);
            return JResult._jResult(result);
        }

        /// <summary>
        ///  获取所有竞拍记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JResult GetAllAuctionParticipantList(AuctionCarParticipantQueryModel query)
        {
            var result = DataAccess.GetAllAuctionParticipantList(query);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 根据拍卖ID 获取竞拍记录
        /// </summary>
        /// <param name="auctionid"></param>
        /// <returns></returns>
        public JResult GetPriceCount(string auctionid)
        {
            var result = DataAccess.GetPriceCount(auctionid);
            var jResult = new JResult();

            jResult.errcode = 0;
            jResult.errmsg = result;

            return jResult;
        }

        #endregion

        #region 押金

        /// <summary>
        /// 获取押金列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionDepositViewModel> GetAuctionDepositList(AuctionDepositQueryModel query)
        {
            return DataAccess.GetAuctionDepositList(query);
        }

        /// <summary>
        /// 添加押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddDeposit(AuctionDepositModel model)
        {
            if (model?.Mobile == null || model.Dpsamount == null || model.Auctionid == null)
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Createrid = ApplicationContext.Current.UserId;
            model.Createdtime = DateTime.Now;
            model.Modifierid = "";
            model.Modifiedtime = null;
            model.Innerid = Guid.NewGuid().ToString();
            var result = DataAccess.AddDeposit(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateDeposit(AuctionDepositModel model)
        {
            if (model?.Innerid == null)
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Modifierid = ApplicationContext.Current.UserId;
            model.Modifiedtime = DateTime.Now;
            model.Createrid = "";
            model.Createdtime = null;
            var result = DataAccess.UpdateDeposit(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 获取押金详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetDepositInfoById(string innerid)
        {
            var result = DataAccess.GetDepositInfoById(innerid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 删除押金
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteDeposit(string innerid)
        {
            var result = DataAccess.DeleteDeposit(innerid);
            return JResult._jResult(result);
        }

        #endregion

        #region 拍卖时间区间

        /// <summary>
        /// 获取拍卖时间列表
        /// </summary>
        /// <returns></returns>
        public JResult GetAuctionTimeList()
        {
            var result = DataAccess.GetAuctionTimeList();
            return JResult._jResult(result);
        }

        #endregion

        #region 认证报告

        /// <summary>
        /// 获取认证项
        /// </summary>
        /// <returns></returns>
        public JResult AuctionCarInspectionItem()
        {
            var result = DataAccess.AuctionCarInspectionItem();
            return JResult._jResult(result);
        }

        /// <summary>
        /// 根据拍卖ID获取拍卖报告内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetAuctionCarInspectionResult(string id)
        {
            var result = DataAccess.GetAuctionCarInspectionResult(id);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetInspectionResultForHtml(string id)
        {
            //获取所有
            IEnumerable<AuctionAllCarInspection> list = DataAccess.AuctionCarInspectionItem();
            var listHtML = list.ToList();
            if (listHtML != null && listHtML.Count() > 0)
            {
                AuctionAllCarInspection model = new AuctionAllCarInspection();
                model.name_en = "总结报告";
                List<AuctionCarInspectionDetailShowModel> auctioncarinspectiondetail = new List<AuctionCarInspectionDetailShowModel>();
                foreach (var item in listHtML)
                {
                    AuctionCarInspectionDetailShowModel detailModel = new AuctionCarInspectionDetailShowModel();
                    detailModel.name_en = item.name_en;
                    detailModel.name_zh = item.name_zh;
                    detailModel.innerid = item.innerid;
                    detailModel.inspectioncount = 0;
                    detailModel.defaultvalue = "全部完好";
                    detailModel.sort = -1;
                    auctioncarinspectiondetail.Add(detailModel);
                }
                model.auctioncarinspectiondetail = auctioncarinspectiondetail;
                listHtML.Add(model);
                foreach (var item in listHtML)
                {
                    if (item.auctioncarinspectiondetail != null)
                    {
                        foreach (var detail in item.auctioncarinspectiondetail)
                        {
                            var listFindings = DataAccess.GetAuctionInspectionResult(id, detail.innerid);
                            if (listFindings != null)
                            {
                                //认证项内容
                                detail.defaultvalue = listFindings.result;
                                //异常项数目
                                detail.intactcount = listFindings.intactcount;
                            }
                        }
                    }
                }
            }

            var result = listHtML.Where(p => p.auctioncarinspectiondetail != null).OrderBy(s => s.sort).ToList();
            return JResult._jResult(result);
        }

        /// <summary>
        /// 添加认证报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarInspection(List<AuctionSaveCarInspectionModel> model)
        {
            var result = DataAccess.AddCarInspection(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改认证报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult EditCarInspection(List<AuctionSaveCarInspectionModel> model)
        {
            var result = DataAccess.EditCarInspection(model);
            return JResult._jResult(result);
        }

        #endregion

        #region 关注

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult Follow(AuctionFollowModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Auctionid) || string.IsNullOrWhiteSpace(model.Userid))
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            model.Isdelete = 0;
            model.Deletedtime = null;
            var result = DataAccess.AddFollow(model);

            return result == -1 ? JResult._jResult(402, "重复关注") : JResult._jResult(result);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JResult Unfollow(string auctionid, string userid)
        {
            if (string.IsNullOrWhiteSpace(auctionid) || string.IsNullOrWhiteSpace(userid))
            {
                return JResult._jResult(401, "参数不完整");
            }
            var result = DataAccess.DelFollow(auctionid, userid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 判断用户是否关注了该拍卖车辆
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JResult IsFollow(string auctionid, string userid)
        {
            if (string.IsNullOrWhiteSpace(auctionid) || string.IsNullOrWhiteSpace(userid))
            {
                return JResult._jResult(401, "参数不完整");
            }
            var result = DataAccess.IsFollow(auctionid, userid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 获取关注的拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetFollowPageList(AuctionFollowQueryModel query)
        {
            if (string.IsNullOrWhiteSpace(query?.Userid))
            {
                return new BasePageList<AuctionCarInfoViewModel>();
            }

            return DataAccess.GetFollowPageList(query);
        }



        #endregion

        #region 公共方法

        /// <summary>
        /// 获取订单号
        /// </summary>
        /// <returns></returns>
        public string GetOrderNumber()
        {
            string Number = DateTime.Now.ToString("yyMMddHHmm");//yyyyMMddHHmmssms
            return Number + Next(1000, 1).ToString();
        }

        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="numSeeds"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int Next(int numSeeds, int length)
        {
            // Create a byte array to hold the random value.  
            byte[] buffer = new byte[length];
            // Create a new instance of the RNGCryptoServiceProvider.  
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();
            // Fill the array with a random value.  
            Gen.GetBytes(buffer);
            // Convert the byte to an uint value to make the modulus operation easier.  
            uint randomResult = 0x0;//这里用uint作为生成的随机数  
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            // Return the random number mod the number  
            // of sides. The possible values are zero-based  
            return (int)(randomResult % numSeeds);
        }

        #endregion

        #region 支付相关

        /// <summary>
        /// 添加定金拍卖定金支付记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddPaymentRecord(AuctionPaymentRecordModel model)
        {
            //更新竞价人的定金支付结果
            var result = DataAccess.AddPaymentRecord(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 微信定金支付
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public JResult WeChatPayForAuction(string innerid, string orderno)
        {
            var ran = new Random();
            var modelname = string.Empty;
            var qrcode = string.Empty;
            //获取定金金额
            var deposit = Convert.ToInt32(ConfigHelper.GetAppSettings("depositauction"));

            var data = new NativePayData
            {
                Body = "快拍立信拍车定金",//商品描述
                Attach = "【kply】",//附加数据
                TotalFee = deposit,//总金额
                ProductId = orderno,//商品ID
                OutTradeNo = orderno,//订单编号
                GoodsTag = ""
            };
            //获取竞拍详情
            var auctionParticipant = DataAccess.GetAuctionParticipantByID(innerid);
            if (auctionParticipant != null)
            {
                var auctionParticipantModel = (AuctionCarParticipantViewModel)auctionParticipant;
                modelname = auctionParticipantModel.model_name;
            }
            var qrcodeResult = WxPayAPIs.GetNativePayQrCode(data);
            if (qrcodeResult.errcode == 0)
            {
                qrcode = qrcodeResult.errmsg.ToString();
                AuctionCarParticipantModel model = new AuctionCarParticipantModel();
                model.Innerid = innerid;
                model.qrcode = qrcode;
                DataAccess.UpdateParticipant(model);
            }
            else//从数据库中获取二维码
            {
                var participantResult = DataAccess.GetAuctionParticipantByID(innerid);
                if (participantResult != null)
                {
                    qrcode = participantResult.qrcode;
                }
            }
            var result = "{\"qrcode\": \"" + qrcode + "\",\"modelname\": \"" + modelname + "\",\"deposit\": " + deposit + "}";
            return JResult._jResult(0, result);
        }

        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CCN.Modules.Car.BusinessEntity;
using CCN.Modules.Car.DataAccess;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.EntLib.Logging;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framework.AuditTrail.Interception;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace CCN.Modules.Car.BusinessComponent
{
    /// <summary>
    /// </summary>
    public class CarBC : BusinessComponentBase<CarDataAccess>
    {
        /// <summary>
        /// </summary>
        /// <param name="da"></param>
        public CarBC(CarDataAccess da)
            : base(da)
        {

        }

        #region 车辆

        /// <summary>
        /// 全城搜车(官网页面)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageListEx(CarGlobalExQueryModel query)
        {
            if (query == null)
            {
                return new BasePageList<CarInfoListViewModel>();
            }

            Task.Run(() =>
            {
                //保存搜车条件
                DataAccess.SaveSearchRecord(new CarSearchRecordModel
                {
                    Createdtime = DateTime.Now,
                    Custid = ApplicationContext.Current.UserId,
                    Innerid = Guid.NewGuid().ToString(),
                    Jsonobj = "web:" + JsonConvert.SerializeObject(query)
                });
            });

            switch (query.Order)
            {
                case "1":
                    query.Order = "a.price asc";
                    break;
                case "-1":
                    query.Order = "a.price desc";
                    break;
                case "2":
                    query.Order = "a.register_date desc";
                    break;
                case "-2":
                    query.Order = "a.register_date asc";
                    break;
                case "3":
                    query.Order = "a.mileage asc";
                    break;
                case "-3":
                    query.Order = "a.mileage desc";
                    break;
                default:
                    query.Order = "a.createdtime desc";
                    break;
            }

            query.keyword = query.keyword.ToDbValue();

            var list = DataAccess.SearchCarPageListEx(query);
            return list;
        }

        /// <summary>
        /// 全城搜车列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageList(CarGlobalQueryModel query)
        {
            var list = DataAccess.SearchCarPageList(query);

            if (list.aaData == null)
                return list;

            foreach (var item in list.aaData)
            {
                if (item.custid.Equals(query.custid))
                {
                    item.isfriend = -1;
                }
            }

            Task.Run(() =>
            {
                //保存搜车条件
                DataAccess.SaveSearchRecord(new CarSearchRecordModel
                {
                    Createdtime = DateTime.Now,
                    Custid = ApplicationContext.Current.UserId,
                    Innerid = Guid.NewGuid().ToString(),
                    Jsonobj = "mobile:" + JsonConvert.SerializeObject(query)
                });
            });

            query.keyword = query.keyword.ToDbValue();

            return list;
        }

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> GetCarPageList(CarQueryModel query)
        {
            var list = DataAccess.GetCarPageList(query);
            if (list.aaData == null || !list.aaData.Any())
                return list;
            var carids = list.aaData.Aggregate("", (current, model) => current + $"'{model.Innerid}',").TrimEnd(',');
            var listShare = DataAccess.GetShareList(carids).ToList();
            if (!listShare.Any())
            {
                return list;
            }

            foreach (var model in list.aaData)
            {
                var firstOrDefault = listShare.Where(x => x.Carid == model.Innerid).ToList().FirstOrDefault();
                if (firstOrDefault != null)
                    model.ShareModel = firstOrDefault;
            }
            query.SearchField = query.SearchField.ToDbValue();
            return list;
        }

        /// <summary>
        /// 获取车辆详情
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        public JResult GetCarInfoById(string id)
        {
            var jResult = new JResult();
            var carInfo = DataAccess.GetCarInfoById(id);
            if (carInfo == null)
            {
                jResult.errcode = 400;
                jResult.errmsg = "";
                return jResult;
            }

            jResult.errcode = 0;
            jResult.errmsg = carInfo;
            return jResult;
        }

        /// <summary>
        /// 获取车辆详情
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        public JResult GetCarViewById(string id)
        {
            var jResult = new JResult();
            var carInfo = DataAccess.GetCarViewById(id);
            if (carInfo == null)
            {
                jResult.errcode = 400;
                jResult.errmsg = "";
                return jResult;
            }

            jResult.errcode = 0;
            jResult.errmsg = carInfo;
            return jResult;
        }

        #region 感兴趣

        /// <summary>
        /// 获取感兴趣的车列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> GetInterestList(CarInterestQueryModel query)
        {
            if (string.IsNullOrWhiteSpace(query?.carid) || query.series_id == null || query.regdate == null || query.price == null)
            {
                LoggerFactories.CreateLogger().Write("获取感兴趣的车列表，参数不完整", TraceEventType.Warning);
                return new BasePageList<CarInfoListViewModel>();
            }
            return DataAccess.GetInterestList(query);
        }

        #endregion

        /// <summary>
        /// 车辆估值（根据城市，车型，时间）
        /// </summary>
        /// <param name="carInfo"></param>
        /// <returns></returns>
        public JResult GetCarEvaluateByCar(CarEvaluateModel carInfo)
        {
            var jResult = new JResult();
            decimal price5 = 0;
            decimal estimateprice5 = 0;
            decimal price10 = 0;
            decimal estimateprice10 = 0;

            var paramList = new Dictionary<string, string>
            {
                {"modelId", carInfo.model_id.ToString()},
                {"regDate", Convert.ToDateTime(carInfo.register_date).ToString("yyyy-MM")},
                {"mile", "5"},
                {"zone", carInfo.cityid.ToString()}
            };

            var juhe = new JuheUtility();
            var result5 = juhe.GetUsedCarPrice(paramList);
            paramList["mile"] = "10";
            var result10 = juhe.GetUsedCarPrice(paramList);

            if (string.IsNullOrWhiteSpace(result5) && string.IsNullOrWhiteSpace(result10))
            {
                jResult.errcode = 400;
                jResult.errmsg = "没有车辆评估信息";
                return jResult;
            }

            try
            {
                JObject jobj5 = JObject.Parse(result5);
                estimateprice5 = Math.Round(Convert.ToDecimal(jobj5["eval_price"].ToString()), 2);
                price5 = Math.Round(Convert.ToDecimal(jobj5["price"].ToString()), 2);

                JObject jobj10 = JObject.Parse(result5);
                estimateprice10 = Math.Round(Convert.ToDecimal(jobj10["eval_price"].ToString()), 2);
                price10 = Math.Round(Convert.ToDecimal(jobj10["price"].ToString()), 2);
            }
            catch (Exception)
            {
                jResult.errcode = 400;
                jResult.errmsg = "没有车辆评估信息";
                return jResult;
            }

            //估价
            carInfo.estimateprice = (estimateprice5 + estimateprice10) / 2;
            //指导价
            carInfo.price = price5 > 0 ? price5 : price10;
            //保存评估信息
            DataAccess.SaveCarEvaluateInfo(carInfo);

            //最低价，最高价，指导价
            string[] result = new string[3] { "0", "0", "0" };
            result[0] = estimateprice5.ToString();
            result[1] = estimateprice10.ToString();
            result[2] = carInfo.price.ToString();

            jResult.errcode = 0;
            jResult.errmsg = result;

            return jResult;
        }
        
        /// <summary>
        /// 车辆估值
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        public JResult GetCarEvaluateById(string id)
        {
            var jResult = new JResult();
            var carInfo = DataAccess.GetCarInfoById(id);

            if (carInfo == null)
            {
                jResult.errcode = 400;
                jResult.errmsg = "车辆信息不存在";
                return jResult;
            }

            //车辆评估信息已经存在
            if (!string.IsNullOrWhiteSpace(carInfo.estimateprice))
            {
                jResult.errcode = 0;
                jResult.errmsg = carInfo.estimateprice;
                return jResult;
            }

            var paramList = new Dictionary<string, string>
            {
                {"modelId", carInfo.model_id.ToString()},
                {"regDate", Convert.ToDateTime(carInfo.register_date).ToString("yyyy-MM")},
                {"mile", carInfo.mileage.ToString()},
                {"zone", carInfo.cityid.ToString()}
            };

            var juhe = new JuheUtility();
            var result = juhe.GetUsedCarPrice(paramList);

            if (string.IsNullOrWhiteSpace(result))
            {
                jResult.errcode = 400;
                jResult.errmsg = "没有车辆评估信息";
                return jResult;
            }

            try
            {
                JObject jobj = JObject.Parse(result);
                result = Math.Round(Convert.ToDouble(jobj["eval_price"].ToString()), 2).ToString();
            }
            catch (Exception)
            {
                jResult.errcode = 400;
                jResult.errmsg = "没有车辆评估信息";
                return jResult;
            }


            //保存评估信息
            Task.Run(() =>
            {
                DataAccess.SaveCarEvaluateInfo(id, result);
            });

            jResult.errcode = 0;
            jResult.errmsg = result;
            return jResult;
        }

        /// <summary>
        /// 获取本月本车型成交数量
        /// </summary>
        /// <param name="modelid">车型id</param>
        /// <returns></returns>
        public JResult GetCarSales(string modelid)
        {
            var num = DataAccess.GetCarSales(modelid);
            return new JResult
            {
                errcode = 0,
                errmsg = num
            };
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult AddCar(CarInfoModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.custid) || model.model_id == null || model.colorid == null || model.price == null || model.register_date == null || model.cityid == null || model.mileage == null)
            {
                return JResult._jResult(401,"参数不完整");
            }
            LoggerFactories.CreateLogger().Write("添加车辆",TraceEventType.Information);
            model.Innerid = Guid.NewGuid().ToString();
            model.status = 1;
            model.createdtime = DateTime.Now;
            model.modifiedtime = null;
            var result = DataAccess.AddCar(model);
            if (result > 0)
            {
                DataAccess.AddShareInfo(model.Innerid);
            }
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = model.Innerid
            };
        }

        /// <summary>
        /// 修改车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult UpdateCar(CarInfoModel model)
        {
            model.createdtime = null;
            model.status = null;
            model.custid = null;

            model.modifiedtime = DateTime.Now;
            var result = DataAccess.UpdateCar(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }

        /// <summary>
        /// 删除车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        public JResult DeleteCar(CarInfoModel model)
        {
            var result = DataAccess.DeleteCar(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "操作成功" : "操作失败"
            };
        }

        /// <summary>
        /// 车辆成交
        /// </summary>
        /// <param name="model">车辆成交model</param>
        /// <returns>1.操作成功</returns>
        public JResult DealCar(CarInfoModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Innerid))
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "参数不正确"
                };
            }
            if (model.sold_time == null)
            {
                model.sold_time = DateTime.Now; //成交时间    
            }
            if (model.closecasetime == null)
            {
                model.closecasetime = DateTime.Now; //结案时间
            }
            var result = DataAccess.DealCar(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "操作成功" : "操作失败"
            };
        }

        /// <summary>
        /// 删除车辆(物理删除，暂不用)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.删除成功</returns>
        public JResult DeleteCar(string id)
        {
            var result = DataAccess.DeleteCar(id);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }

        /// <summary>
        /// 车辆状态更新
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="status"></param>
        /// <returns>1.操作成功</returns>
        public JResult UpdateCarStatus(string carid, int status)
        {
            var result = DataAccess.UpdateCarStatus(carid, status);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }

        /// <summary>
        /// 车辆分享
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public JResult ShareCar(string id)
        {
            var result = DataAccess.ShareCar(id);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }

        /// <summary>
        /// 累计车辆查看次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="count">新增次数</param>
        /// <returns>1.累计成功</returns>
        public JResult UpSeeCount(string id, int count)
        {
            var result = DataAccess.UpSeeCount(id, count);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.累计成功</returns>
        public JResult UpPraiseCount(string id)
        {
            var result = DataAccess.UpPraiseCount(id);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="content">评论内容</param>
        /// <returns>1.累计成功</returns>
        public JResult CommentCar(string id, string content)
        {
            var result = DataAccess.CommentCar(id, content);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }

        /// <summary>
        /// 审核车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="status">审核状态</param>
        /// <returns>1.操作成功</returns>
        public int AuditCar(string id, int status)
        {
            return DataAccess.AuditCar(id, status);
        }

        /// <summary>
        /// 核销车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public int CancelCar(string id)
        {
            return DataAccess.CancelCar(id);
        }

        /// <summary>
        /// 获取车辆 分享/查看次数
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        public JResult GetCarShareInfo(string carid)
        {
            var carInfo = DataAccess.GetCarShareInfo(carid);
            return carInfo == null
                ? JResult._jResult(400, "")
                : JResult._jResult(0, carInfo);
        }
        #endregion

        #region 车辆图片

        /// <summary>
        /// 添加车辆图片
        /// </summary>
        /// <param name="model">车辆图片信息</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarBC.AddCarPicture")]
        public JResult AddCarPicture(CarPictureModel model)
        {
            if (model == null)
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "参数不完整"
                };
            }
            model.Innerid = Guid.NewGuid().ToString();

            //初始化排序
            var max = DataAccess.GetCarPictureMaxSort(model.Carid);

            model.Sort = max + 1;

            var result = DataAccess.AddCarPicture(model);

            //设置第一张图片为封面
            if (result > 0 && model.Sort == 1)
            {
                DataAccess.SetCarCover(model.Carid, model.Path);
            }

            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }
        
        /// <summary>
        /// 删除车辆图片
        /// </summary>
        /// <param name="innerid">车辆图片id</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarBC.DeleteCarPicture")]
        public JResult DeleteCarPicture(string innerid)
        {
            var model = DataAccess.GetCarPictureByid(innerid);

            var result = DataAccess.DeleteCarPicture(innerid);
            switch (result)
            {
                case 400: return JResult._jResult(result, "删除失败");
                case 401: return JResult._jResult(result, "图片不存在");
                case 402: return JResult._jResult(result, "图片不能少于三张");
            }

            if (!string.IsNullOrWhiteSpace(model?.Path))
            {
                var qiniu = new QiniuUtility();
                qiniu.DeleteFile(model.Path);
            }

            return JResult._jResult(0, "删除成功");
        }
        
        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarBC.GetCarPictureByCarid")]
        public JResult GetCarPictureByCarid(string carid)
        {
            var model = DataAccess.GetCarPictureByCarid(carid);
            if (model == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = model
            };
        }

        /// <summary>
        /// 图片调换位置
        /// </summary>
        /// <param name="listPicture">车辆图片列表</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarDataAccess.ExchangePictureSort")]
        public JResult ExchangePictureSort(List<CarPictureModel> listPicture)
        {
            //如果图片数量小与2个，或要交换的排序为0，或者两个要交换的排序相同 的话就不能调整排序
            if (listPicture.Count() < 2 || listPicture[0].Sort == 0 || listPicture[1].Sort == 0 || listPicture[0].Sort == listPicture[1].Sort)
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "参数不完整"
                };
            }

            var result = DataAccess.ExchangePictureSort(listPicture);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }

        /// <summary>
        /// 批量保存图片(删除)
        /// </summary>
        /// <param name="picModel"></param>
        /// <returns></returns>
        public JResult DelCarPictureList(PictureDelListModel picModel)
        {
            if (picModel == null || picModel.IdList.Count == 0)
            {
                return JResult._jResult(401, "参数不完整");
            }

            //获取即将删除的图片
            var picedList = DataAccess.GetCarPictureByIds(picModel.IdList).ToList();
            
            var result = DataAccess.DelCarPictureList(picModel.IdList, picModel.Carid);
            
            switch (result)
            {
                case 400:
                    return JResult._jResult(400, "图片数量不对");
                case 0:
                    return JResult._jResult(401, "批量删除图片失败");
            }

            //异步删除七牛上的图片
            Task.Run(() =>
            {
                if (!picedList.Any())
                    return;

                var qiniu = new QiniuUtility();
                foreach (var item in picedList.Where(item => !string.IsNullOrWhiteSpace(item?.Path)))
                {
                    qiniu.DeleteFile(item.Path);
                }
            });

            return JResult._jResult(0, "批量删除图片成功");
        }

        /// <summary>
        /// 批量添加车辆图片(添加)(后台)
        /// </summary>
        /// <param name="picModel">车辆图片信息</param>
        /// <returns></returns>
        public JResult AddCarPictureList(PictureListModel picModel)
        {
            if (picModel == null)
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "参数不完整"
                };
            }

            var result = DataAccess.AddCarPictureList(picModel.KeyList, picModel.Carid);

            if (result == 400)
            {
                return JResult._jResult(400, "图片数量不对");
            }
            return result == 0
                ? JResult._jResult(401, "批量添加图片失败")
                : JResult._jResult(0, "批量添加图片成功");
        }
        
        /// <summary>
        /// 批量添加车辆图片(添加)(微信端使用)
        /// </summary>
        /// <param name="picModel">车辆图片信息</param>
        /// <returns></returns>
        public JResult AddCarPictureList(WechatPictureModel picModel)
        {
            if (string.IsNullOrWhiteSpace(picModel?.AccessToken) || picModel.MediaIdList.Count == 0)
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "参数不完整"
                };
            }

            var pathList = new List<string>();

            //上传图片到七牛云
            var qinniu = new QiniuUtility();

            foreach (var item in picModel.MediaIdList)
            {
                var filename = QiniuUtility.GetFileName(Picture.car_picture);

                //下载图片写入文件流
                var filebyte = MediaApi.Get(picModel.AccessToken, item);
                Stream stream = new MemoryStream(filebyte);

                //上传到七牛
                var qnKey = qinniu.Put(stream, "", filename);
                stream.Dispose();

                //上传图片成功
                if (string.IsNullOrWhiteSpace(qnKey))
                {
                    continue;
                }

                pathList.Add(qnKey);
            }

            var result = DataAccess.AddCarPictureList(pathList, picModel.Carid);

            if (result == 400)
            {
                return JResult._jResult(400, "图片数量不对");
            }
            return result == 0
                ? JResult._jResult(401, "批量添加图片失败")
                : JResult._jResult(0, "批量添加图片成功");
        }
        
        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult SaveCarPicture(BatchPictureListWeichatModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Carid) || (model.IdList.Count == 0 || model.WechatPicture == null))
            {
                return JResult._jResult(401, "参数不完整");
            }

            var pathList = new List<string>();

            //上传图片到七牛云
            var qinniu = new QiniuUtility();

            foreach (var item in model.WechatPicture.MediaIdList)
            {
                var filename = QiniuUtility.GetFileName(Picture.car_picture);

                //下载图片写入文件流
                var filebyte = MediaApi.Get(model.WechatPicture.AccessToken, item);
                Stream stream = new MemoryStream(filebyte);

                //上传到七牛
                var qnKey = qinniu.Put(stream, "", filename);
                stream.Dispose();

                //上传图片成功
                if (string.IsNullOrWhiteSpace(qnKey))
                {
                    continue;
                }

                pathList.Add(qnKey);
            }

            var saveList = new BatchPictureListModel
            {
                Carid = model.Carid,
                AddPaths = pathList,
                DelIds = model.IdList
            };

            //获取即将删除的图片
            var picedList = DataAccess.GetCarPictureByIds(model.IdList).ToList();

            var result = DataAccess.SaveCarPicture(saveList);

            switch (result)
            {
                case 400:
                    return JResult._jResult(400,"图片数量不对");
                case 0:
                    return JResult._jResult(401, "批量操作图片失败");
            }

            //异步删除七牛上的图片
            Task.Run(() =>
            {
                if (!picedList.Any())
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

        #region 车辆收藏

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCollection(CarCollectionModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Custid) || string.IsNullOrWhiteSpace(model.Carid))
            {
                return JResult._jResult(401,"参数不完整！");
            }

            if (DataAccess.CheckCollection(model) != null)
            {
                return JResult._jResult(402, "重复收藏！");
            }
            
            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            var result = DataAccess.AddCollection(model);

            return JResult._jResult(result > 0 ? 0 : 400, model.Innerid);
        }

        /// <summary>
        /// 删除收藏 by innerid
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCollection(string innerid)
        {
            var result = DataAccess.DeleteCollection(innerid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 删除收藏 by carid
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        public JResult DeleteCollectionByCarid(string carid)
        {
            var result = DataAccess.DeleteCollectionByCarid(carid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 获取收藏的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarCollectionViewListModel> GetCollectionList(CarCollectionQueryModel query)
        {
            if (string.IsNullOrWhiteSpace(query?.Custid))
            {
                return new BasePageList<CarCollectionViewListModel>();
            }

            var list = DataAccess.GetCollectionList(query);
            if (list.aaData == null)
                return list;

            foreach (var item in list.aaData.Where(item => item.custid.Equals(query.Custid)))
            {
                item.isfriend = -1;
            }
            
            return list;
        }

        #endregion
    }
}

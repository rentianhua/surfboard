using System;
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
        /// 全城搜车(官网页面)（查询到置顶车辆）
        /// </summary>
        /// <param name="query">查询条件
        /// query.Echo 用于第几次进入最后一页，补齐的时候就代表第几页
        /// </param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageListTop(CarGlobalExQueryModel query)
        {
            if (query == null)
            {
                return new BasePageList<CarInfoListViewModel>();
            }

            string strwhere;
            var list = DataAccess.SearchCarPageListTop(query, out strwhere);

            //判断是否设置条件，如果没有条件表示所有的查询所有的置顶数据，到了最后一页也不需要补齐数据
            if (string.IsNullOrWhiteSpace(strwhere))
                return list;

            var fill = 0;       //标识是否需要补数据
            var total = list.iTotalRecords ?? 0;

            if (total <= 0)  //没有数据
            {
                fill = query.PageSize;
            }
            else
            {
                var ys = total % query.PageSize; //余数
                if (ys > 0)
                {
                    var maxindex = total / query.PageSize + 1;   //最大页数
                    if (maxindex == query.PageIndex)  //最后一页
                    {
                        fill = query.PageSize - ys;
                    }
                }
            }

            //需要补数据
            if (fill <= 0)
                return list;

            //查询补全数据
            var filllist = DataAccess.SearchCarPageListTopFill(new CarTopFillQueryModel
            {
                @where = strwhere,
                PageIndex = query.Echo ?? 1, //第几次进入最后一页，补齐的时候就代表第几页(重要字段)
                PageSize = fill
            });

            if (!filllist.aaData.Any())
                return list;

            //将补全数据填充到置顶数据中
            var aaData = list.aaData.ToList();
            aaData.AddRange(filllist.aaData);
            list.aaData = aaData;
            list.iTotalDisplayRecords += filllist.aaData.Count();   //补齐数据后的总记录数

            return list;
        }

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

            var custid = ApplicationContext.Current.UserId;
            Task.Run(() =>
            {
                //保存搜车条件
                DataAccess.SaveSearchRecord(new CarSearchRecordModel
                {
                    Createdtime = DateTime.Now,
                    Custid = custid,
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
            if (query == null)
            {
                return new BasePageList<CarInfoListViewModel>();
            }
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

            var custid = ApplicationContext.Current.UserId;
            Task.Run(() =>
            {
                //保存搜车条件
                DataAccess.SaveSearchRecord(new CarSearchRecordModel
                {
                    Createdtime = DateTime.Now,
                    Custid = custid,
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
            if (query == null)
            {
                return new BasePageList<CarInfoListViewModel>();
            }
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
            var model = DataAccess.GetCarViewById(id);

            if (model == null)
            {
                return JResult._jResult(null);
            }

            var custid = ApplicationContext.Current.UserId;
            if (string.IsNullOrWhiteSpace(custid))
                return JResult._jResult(model);

            var col = DataAccess.CheckCollection(new CarCollectionModel
            {
                Custid = custid,
                Carid = model.Innerid
            });

            if (col != null)
            {
                model.IsCollection = 1;
            }

            return JResult._jResult(model);
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

            var juhe = new Che300Utility();
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

            var juhe = new Che300Utility();
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
                return JResult._jResult(401, "参数不完整");
            }
            LoggerFactories.CreateLogger().Write("添加车辆", TraceEventType.Information);
            model.Innerid = Guid.NewGuid().ToString();
            model.status = 1;
            model.createdtime = DateTime.Now;

            var ts = model.createdtime.Value - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            model.refreshtime = (long)ts.TotalSeconds;

            model.istop = 0;            ////

            model.modifiedtime = null;
            var result = DataAccess.AddCar(model);
            if (result == -1)
            {
                return JResult._jResult(402, "会员不存在");
            }
            if (result > 0)
            {
                DataAccess.AddShareInfo(model.Innerid);
            }
            return JResult._jResult
            (
                result > 0 ? 0 : 400,
                model.Innerid
            );
        }

        /// <summary>
        /// 修改车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public JResult UpdateCar(CarInfoModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Innerid))
            {
                return JResult._jResult(401, "参数不完整");
            }
            LoggerFactories.CreateLogger().Write("车辆修改：" + JsonConvert.SerializeObject(model), TraceEventType.Information);

            model.createdtime = null;
            model.status = null;
            model.custid = null;

            model.brand_name = null;
            model.series_name = null;
            model.cityname = null;
            model.provname = null;
            model.model_name = null;
            model.color = null;

            model.geartype = null;
            model.liter = null;
            model.dischargeName = null;

            model.refreshtime = null;
            model.istop = null;

            model.modifiedtime = DateTime.Now;
            var result = DataAccess.UpdateCar(model);
            return JResult._jResult(result);
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

        /// <summary>
        /// 刷新车辆
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>1.操作成功</returns>
        public JResult RefreshCar(string carid)
        {
            var result = DataAccess.RefreshCar(carid);
            return result == 401 
                ? JResult._jResult(401, "刷新次数已经用完") 
                : JResult._jResult(result);
        }

        /// <summary>
        /// 置顶车辆
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>1.操作成功</returns>
        public JResult PushUpCar(string carid)
        {
            var result = DataAccess.PushUpCar(carid);
            return result == 401
                ? JResult._jResult(401, "置顶次数已经用完")
                : JResult._jResult(result);
        }

        /// <summary>
        /// 置顶或取消置顶
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="istop">1.置顶 0取消置顶</param>
        /// <returns>1.操作成功</returns>
        public JResult DoTopCar(string carid, int istop)
        {
            if (1 != istop)
            {
                istop = 0;
            }
            var result = DataAccess.DoTopCar(carid, istop);
            return JResult._jResult(result);
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
        /// 添加车辆图片（单个）
        /// </summary>
        /// <param name="model">车辆图片信息</param>
        /// <returns></returns>
        public JResult AddCarPictureEx(CarPictureModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Carid) || string.IsNullOrWhiteSpace(model.Path))
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Innerid = Guid.NewGuid().ToString();
            var result = DataAccess.AddCarPictureEx(model);

            switch (result)
            {
                case 402:
                    return JResult._jResult(402, "图片数量超过");
                case 0:
                    return JResult._jResult(400, "批量删除图片失败");
            }

            return JResult._jResult(0, "保存图片成功");
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
                case 402:
                    return JResult._jResult(402, "图片数量不对");
                case 0:
                    return JResult._jResult(400, "批量删除图片失败");
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

            if (result == 402)
            {
                return JResult._jResult(402, "图片数量不对");
            }
            return result == 0
                ? JResult._jResult(400, "批量添加图片失败")
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

            if (result == 402)
            {
                return JResult._jResult(402, "图片数量不对");
            }
            return result == 0
                ? JResult._jResult(400, "批量添加图片失败")
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
                case 402:
                    return JResult._jResult(402, "图片数量不对");
                case 0:
                    return JResult._jResult(400, "批量删除图片失败");
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

        #region new

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<string> UploadPicture(WechatPictureExModel model)
        {
            var pathList = new List<string>();

            //上传图片到七牛云
            var qinniu = new QiniuUtility();

            foreach (var item in model.MediaIdList)
            {
                var filename = QiniuUtility.GetFileName(Picture.car_picture);

                //下载图片写入文件流
                var filebyte = MediaApi.Get(model.AccessToken, item);
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

            return pathList;
        }

        /// <summary>
        /// 批量保存图片(wechat webapp)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult BatchSaveCarPictureWechat(WechatPictureExModel model)
        {

            if (string.IsNullOrWhiteSpace(model?.Carid) || (model.DelIds.Count == 0 && model.MediaIdList.Count == 0))
            {
                return JResult._jResult(401, "参数不完整");
            }

            var result = 0;
            //获取即将删除的图片
            List<CarPictureModel> picedList = null;

            //only delete
            if (model.DelIds.Count > 0 && model.MediaIdList.Count == 0)
            {
                //获取需要删除的图片
                picedList = DataAccess.GetCarPictureByIds(model.DelIds).ToList();
                result = DataAccess.DelPictureList(model.DelIds, model.Carid);
            }
            //only add
            else if (model.DelIds.Count == 0 && model.MediaIdList.Count > 0)
            {
                var addlist = UploadPicture(model);
                result = DataAccess.AddPictureList(addlist, model.Carid);
            }
            //add and delete
            else if (model.DelIds.Count > 0 && model.MediaIdList.Count > 0)
            {
                var addlist = UploadPicture(model);
                picedList = DataAccess.GetCarPictureByIds(model.DelIds).ToList();
                result = DataAccess.AddAndDelPicture(new BatchPictureListModel
                {
                    AddPaths = addlist,
                    DelIds = model.DelIds,
                    Carid = model.Carid
                });
            }

            switch (result)
            {
                case 402:
                    return JResult._jResult(402, "图片数量不对");
                case 0:
                    return JResult._jResult(400, "批量删除图片失败");
            }

            //异步删除七牛上的图片
            if (picedList != null && picedList.Any())
            {
                Task.Run(() =>
                {
                    var qiniu = new QiniuUtility();
                    foreach (var item in picedList.Where(item => !string.IsNullOrWhiteSpace(item?.Path)))
                    {
                        qiniu.DeleteFile(item.Path);
                    }
                });
            }

            //调用nodejs接口即时更新车辆列表图片
            if (model.MediaIdList.Count > 0)
            {
                Task.Run(() =>
                {
                    var param = new Dictionary<string, string>
                    {
                        {"mobile", model.Mobile},
                        {"carid", model.Carid},
                        {"picurl", DataAccess.GetCarPicByCarid(model.Carid)}
                    };

                    var nodejs = ConfigHelper.GetAppSettings("nodejssiteurl") + "api/carImgSaveMsg";
                    var nodeRes = DynamicWebService.SendPost(nodejs, param, "post");
                    LoggerFactories.CreateLogger().Write("nodejs返回结果：" + nodeRes, TraceEventType.Information);
                });
            }

            return JResult._jResult(0, "批量操作图片成功");
        }

        /// <summary>
        /// 批量保存图片(通用，除微信端)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult BatchSaveCarPicture(BatchPictureListModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Carid))
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.DelIds = model.DelIds ?? new List<string>();
            model.AddPaths = model.AddPaths ?? new List<string>();

            if (model.DelIds.Count == 0 && model.AddPaths.Count == 0)
            {
                return JResult._jResult(401, "参数不完整");
            }

            var result = 0;
            //获取即将删除的图片
            List<CarPictureModel> picedList = null;

            //only delete
            if (model.DelIds.Count > 0 && model.AddPaths.Count == 0)
            {
                picedList = DataAccess.GetCarPictureByIds(model.DelIds).ToList();
                result = DataAccess.DelPictureList(model.DelIds, model.Carid);
            }
            //only add
            else if (model.DelIds.Count == 0 && model.AddPaths.Count > 0)
            {
                result = DataAccess.AddPictureList(model.AddPaths, model.Carid);
            }
            //add and delete
            else if (model.DelIds.Count > 0 && model.AddPaths.Count > 0)
            {
                picedList = DataAccess.GetCarPictureByIds(model.DelIds).ToList();
                result = DataAccess.AddAndDelPicture(model);
            }

            switch (result)
            {
                case 402:
                    return JResult._jResult(402, "图片数量不对");
                case 0:
                    return JResult._jResult(400, "批量删除图片失败");
            }

            //异步删除七牛上的图片
            if (picedList != null && picedList.Any())
            {
                Task.Run(() =>
                {
                    var qiniu = new QiniuUtility();
                    foreach (var item in picedList.Where(item => !string.IsNullOrWhiteSpace(item?.Path)))
                    {
                        qiniu.DeleteFile(item.Path);
                    }
                });
            }

            return JResult._jResult(0, "批量操作图片成功");
        }
        
        #endregion
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
                return JResult._jResult(401, "参数不完整！");
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

        #region 车辆悬赏

        /// <summary>
        /// 车辆悬赏列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarRewardViewModel> CarRewardPageList(CarRewardQueryModel query)
        {
            var list = DataAccess.CarRewardPageList(query);
            return list;
        }

        /// <summary>
        /// 添加车辆悬赏信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarReward(CarReward model)
        {
            var result = DataAccess.AddCarReward(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="innerid">主键</param>
        /// <returns></returns>
        public JResult UpdateCarRewardStatus(int status, string innerid)
        {
            var result = DataAccess.UpdateCarRewardStatus(status, innerid);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 车辆悬赏推荐
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> GetCarRewardPageList(CarRewardQueryModel query)
        {
            if (query == null)
            {
                return new BasePageList<CarInfoListViewModel>();
            }
            CarQueryModel model = new CarQueryModel();
            model.brand_id = query.brand_id;
            model.series_id = query.series_id;
            model.provid = query.provid;
            model.cityid = query.cityid;
            //车龄区间
            string[] coty = new string[2];
            if (!string.IsNullOrWhiteSpace(query.mileage))
            {
                coty = query.mileage.Split('-');
                model.minprice = Convert.ToInt32(coty[0]);
                model.maxprice = Convert.ToInt32(coty[1]);
            }
            //里程区间
            string[] mileage = new string[2];
            if (!string.IsNullOrWhiteSpace(query.mileage))
            {
                model.mincoty = Convert.ToInt32(coty[0]);
                model.maxcoty = Convert.ToInt32(coty[1]);
            }
            //价格区间
            string[] price = new string[2];
            if (!string.IsNullOrWhiteSpace(query.mileage))
            {
                price = query.mileage.Split('-');
                model.minbuyprice = Convert.ToDecimal(coty[0]);
                model.maxbuyprice = Convert.ToDecimal(coty[1]);
            }

            var list = DataAccess.GetCarRewardPageList(model);
            return list;
        }

        #endregion
    }
}

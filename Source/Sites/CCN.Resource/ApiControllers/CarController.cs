using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CCN.Modules.Car.BusinessEntity;
using CCN.Modules.Car.Interface;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using CCN.Modules.Rewards.BusinessEntity;
using CCN.Modules.Rewards.Interface;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.IoC;
using Cedar.Core.Logging;
using Cedar.Foundation.SMS.Common;
using Cedar.Framework.Common.BaseClasses;
using Newtonsoft.Json;

namespace CCN.Resource.ApiControllers
{
    /// <summary>
    /// 车辆管理
    /// </summary>
    [RoutePrefix("api/Car")]
    public class CarController : ApiController
    {
        private readonly ICarManagementService _carervice;

        public CarController()
        {
            _carervice = ServiceLocatorFactory.GetServiceLocator().GetService<ICarManagementService>();
        }

        #region 车辆基本信息

        /// <summary>
        /// 分页查询车辆
        /// </summary>
        /// <param name="query">车辆信息</param>
        [Route("GetCarPageList")]
        [HttpPost]
        public BasePageList<CarInfoListViewModel> GetCarPageList([FromBody] CarQueryModel query)
        {
            return _carervice.GetCarPageList(query);
        }

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [Route("GetAllCarPageList")]
        [HttpPost]
        public BasePageList<CarInfoListViewModel> GetAllCarPageList(CarQueryModel query)
        {
            return _carervice.GetAllCarPageList(query);
        }

        /// <summary>
        /// 获取车辆详细信息(info)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        [Route("GetCarInfoById")]
        [HttpGet]
        public JResult GetCarInfoById(string id)
        {
            return _carervice.GetCarInfoById(id);
        }

        /// <summary>
        /// 获取车辆详情(view)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        [Route("GetCarViewById")]
        [HttpGet]
        public JResult GetCarViewById(string id)
        {
            return _carervice.GetCarViewById(id);
        }

        /// <summary>
        /// 车辆估值 （根据城市，车型，时间）
        /// </summary>
        /// <param name="carinfo">车辆信息</param>
        /// <returns></returns>
        [Route("GetCarEvaluateByCar")]
        [HttpPost]
        public JResult GetCarEvaluateByCar([FromBody] CarEvaluateModel carinfo)
        {
            return _carervice.GetCarEvaluateByCar(carinfo); ;
        }

        /// <summary>
        /// 车辆估值
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        [Route("GetCarEvaluateById")]
        [HttpGet]
        public JResult GetCarEvaluateById(string id)
        {
            return _carervice.GetCarEvaluateById(id);
        }

        /// <summary>
        /// 获取本月本车型成交数量
        /// </summary>
        /// <param name="modelid">车型id</param>
        /// <returns></returns>
        [Route("GetCarSales")]
        [HttpGet]
        public JResult GetCarSales(string modelid)
        {
            return _carervice.GetCarSales(modelid);
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        [Route("AddCar")]
        [HttpPost]
        public JResult AddCar([FromBody] CarInfoModel model)
        {
            return _carervice.AddCar(model);
        }

        /// <summary>
        /// 后台添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        [Route("AddCarBack")]
        [HttpPost]
        public JResult AddCarBack([FromBody] CarInfoModel model)
        {
            var jresult = _carervice.AddCar(model);

            //if (jresult.errcode == 0)
            //{
            //    Task.Run(() =>
            //    {
            //        ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>().UpdateCustType(model.custid);
            //    });
            //}

            return jresult;
        }

        /// <summary>
        /// 快速录车
        /// </summary>
        /// <param name="model">车辆信息</param>
        [Route("AddCarFast")]
        [HttpPost]
        public JResult AddCarFast([FromBody] CarInfoFastModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.mobile))
            {
                return JResult._jResult(401, "参数不完整");
            }

            //判断是否登录
            if (string.IsNullOrWhiteSpace(model.custid)) //没有登录
            {
                var custservice = ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();

                //用手机号获取会员信息
                CustModel custinfo = null;
                var cust = custservice.GetCustByMobile(model.mobile);
                if (!string.IsNullOrWhiteSpace(cust.errmsg?.ToString()))
                {
                    custinfo = (CustModel)custservice.GetCustByMobile(model.mobile).errmsg;
                }

                if (custinfo == null) //会员不存在
                {
                    var password = RandomUtility.GetRandom(6);
                    //自动注册
                    var regResult = custservice.CustRegister(new CustModel
                    {
                        Mobile = model.mobile,
                        Password = password,
                        Custname = model.contacts,
                        Type = 2 //快速录车时自动注册也默认个人
                    });

                    if (string.IsNullOrWhiteSpace(regResult.errmsg?.ToString()))
                    {
                        return JResult._jResult(500, "自动注册失败");
                    }
                    model.custid = regResult.errmsg.ToString();

                    Task.Run(() =>
                    {
                        var sms = new SMSMSG();
                        sms.PostSms(model.mobile, $"亲爱的用户：感谢您使用玖伍淘车发布车辆！如您是车商，强烈推荐您关注并使用专为车商朋友服务的【玖伍淘车】公众号！已为您自动注册【用户名：{model.mobile}】【初始随机密码：{password}】若需使用建议您尽快修改密码。如无需要，请忽略。玖伍淘车承诺不会透露用户信息。");
                    });

                    return _carervice.AddCar(model);
                }

                model.custid = custinfo.Innerid;
            }

            //添加车辆
            var jresult = _carervice.AddCar(model);

            //if (jresult.errcode == 0)
            //{
            //    Task.Run(() =>
            //    {
            //        ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>().UpdateCustType(model.custid);
            //    });
            //}

            return jresult;
        }

        /// <summary>
        /// 修改车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        [Route("UpdateCar")]
        [HttpPost]
        public JResult UpdateCar([FromBody] CarInfoModel model)
        {
            return _carervice.UpdateCar(model);
        }

        /// <summary>
        /// 删除车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        [Route("DeleteCar")]
        [HttpPost]
        public JResult DeleteCar([FromBody] CarInfoModel model)
        {
            return _carervice.DeleteCar(model);
        }

        /// <summary>
        /// 回复车辆
        /// </summary>
        /// <param name="model">回复成交model</param>
        /// <returns>1.操作成功</returns>
        [Route("RecoveryCar")]
        [HttpPost]
        public JResult RecoveryCar([FromBody]CarInfoModel model)
        {
            return _carervice.RecoveryCar(model);
        }

        /// <summary>
        /// 车辆成交
        /// </summary>
        /// <param name="model">车辆成交model</param>
        /// <returns>1.操作成功</returns>
        [Route("DealCar")]
        [HttpPost]
        public JResult DealCar([FromBody] CarInfoModel model)
        {
            var result = _carervice.DealCar(model);

            #region 车辆结案送积分

            if (result.errcode == 0)
            {
                //获取会员id
                var custid = ApplicationContext.Current.UserId;
                Task.Run(() =>
                {
                    if (string.IsNullOrWhiteSpace(custid))
                    {
                        LoggerFactories.CreateLogger().Write("车辆结案送积分:会员id空", TraceEventType.Warning);
                        return;
                    }

                    var rewardsservice = ServiceLocatorFactory.GetServiceLocator().GetService<IRewardsManagementService>();

                    rewardsservice.ChangePoint(new CustPointModel
                    {
                        Custid = custid,
                        Type = 1,
                        Point = 1000,
                        Remark = "车辆结案送积分",
                        Sourceid = 6
                    });

                    //结案送礼券
                    //rewardsservice.SendCoupon(new SendCouponModel()
                    //{
                    //    Custid = custid,
                    //    ActionType = 2,
                    //    Sourceid = 4
                    //});
                });
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 删除车辆(物理删除，暂不用)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.删除成功</returns>
        [Route("DeleteCar")]
        [HttpDelete]
        [NonAction]
        public JResult DeleteCar(string id)
        {
            return _carervice.DeleteCar(id);
        }

        /// <summary>
        /// 车辆状态更新(废弃)
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="status"></param>
        /// <returns>1.操作成功</returns>
        [Route("UpdateCarStatus")]
        [HttpGet]
        [NonAction]
        public JResult UpdateCarStatus(string carid, int status)
        {
            return _carervice.UpdateCarStatus(carid, status);
        }

        /// <summary>
        /// 车辆分享
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        [Route("ShareCar")]
        [HttpGet]
        public JResult ShareCar(string id)
        {
            var result = _carervice.ShareCar(id);

            #region 注册送积分

            if (result.errcode == 0)
            {
                Task.Run(() =>
                {
                    var rewardsservice = ServiceLocatorFactory.GetServiceLocator().GetService<IRewardsManagementService>();

                    //获取会员id
                    var custid = ApplicationContext.Current.UserId;

                    //会员每分享一次有效车辆信息至朋友圈奖励10积分，每个会员每天最多由此获得150积分
                    //////////////////////////
                    rewardsservice.ChangePoint(new CustPointModel
                    {
                        Custid = custid,
                        Createdtime = DateTime.Now,
                        Type = 1,
                        Innerid = Guid.NewGuid().ToString(),
                        Point = 10,
                        Remark = "",
                        Sourceid = 5,
                        Validtime = null
                    });
                });
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 累计车辆查看次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="count">新增次数</param>
        /// <returns>1.累计成功</returns>
        [Route("UpSeeCount")]
        [HttpGet]
        public JResult UpSeeCount(string id, int count = 1)
        {
            return _carervice.UpSeeCount(id, count);
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.累计成功</returns>
        [Route("UpPraiseCount")]
        [HttpGet]
        public JResult UpPraiseCount(string id)
        {
            return _carervice.UpPraiseCount(id);
        }

        /// <summary>
        /// 累计评论次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="content">评论内容</param>
        /// <returns>1.累计成功</returns>
        [Route("CommentCar")]
        [HttpGet]
        public JResult CommentCar(string id, string content = "")
        {
            return _carervice.CommentCar(id, content);
        }

        /// <summary>
        /// 获取车辆 分享/查看次数
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        [Route("GetCarShareInfo")]
        [HttpGet]
        public JResult GetCarShareInfo(string carid)
        {
            return _carervice.GetCarShareInfo(carid);
        }

        /// <summary>
        /// 刷新车辆
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>1.操作成功</returns>
        [Route("RefreshCar")]
        [HttpGet]
        public JResult RefreshCar(string carid)
        {
            return _carervice.RefreshCar(carid);
        }
        
        /// <summary>
        /// 置顶车辆
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>1.操作成功</returns>
        [Route("PushUpCar")]
        [HttpGet]
        public JResult PushUpCar(string carid)
        {
            return _carervice.PushUpCar(carid);
        }

        /// <summary>
        /// 置顶或取消置顶
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="istop">1.置顶 0取消置顶</param>
        /// <returns>1.操作成功</returns>
        [Route("DoTopCar")]
        [HttpGet]
        public JResult DoTopCar(string carid, int istop)
        {
            return _carervice.DoTopCar(carid, istop);
        }
        #endregion

        #region 车辆图片

        /// <summary>
        /// 添加车辆图片
        /// </summary>
        /// <param name="model">车辆图片信息</param>
        /// <returns></returns>
        [Route("AddCarPicture")]
        [HttpPost]
        public JResult AddCarPicture([FromBody] CarPictureModel model)
        {
            return _carervice.AddCarPicture(model);
        }

        /// <summary>
        /// 添加车辆图片
        /// </summary>
        /// <param name="innerid">车辆图片id</param>
        /// <returns></returns>
        [Route("DeleteCarPicture")]
        [HttpDelete]
        public JResult DeleteCarPicture(string innerid)
        {
            return _carervice.DeleteCarPicture(innerid);
        }

        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        [Route("GetCarPictureByCarid")]
        [HttpGet]
        public JResult GetCarPictureByCarid(string carid)
        {
            return _carervice.GetCarPictureByCarid(carid);
        }

        /// <summary>
        /// 图片调换位置
        /// </summary>
        /// <param name="listPicture">车辆图片列表</param>
        /// <returns></returns>
        [Route("ExchangePictureSort")]
        [HttpPost]
        public JResult ExchangePictureSort([FromBody] List<CarPictureModel> listPicture)
        {
            return _carervice.ExchangePictureSort(listPicture);
        }

        /// <summary>
        /// 批量保存图片(删除)
        /// </summary>
        /// <param name="picModel"></param>
        /// <returns></returns>
        [Route("DelCarPictureList")]
        [HttpPost]
        public JResult DelCarPictureList([FromBody]PictureDelListModel picModel)
        {
            LoggerFactories.CreateLogger().Write("批量删除图片参数：" + JsonConvert.SerializeObject(picModel), TraceEventType.Information);
            return _carervice.DelCarPictureList(picModel);
        }

        /// <summary>
        /// 批量添加车辆图片(添加)(后台)
        /// </summary>
        /// <param name="picModel">车辆图片信息</param>
        /// <returns></returns>
        [Route("AddCarPictureKeyList")]
        [HttpPost]
        public JResult AddCarPictureList([FromBody]PictureListModel picModel)
        {
            return _carervice.AddCarPictureList(picModel);
        }
        
        /// <summary>
        /// 批量保存图片(通用，除微信端)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("BatchSaveCarPicture")]
        [HttpPost]
        public JResult BatchSaveCarPicture([FromBody] BatchPictureListModel model)
        {
            LoggerFactories.CreateLogger().Write("批量保存图片参数：" + JsonConvert.SerializeObject(model), TraceEventType.Information);
            return _carervice.BatchSaveCarPicture(model);
        }

        #endregion

        #region 车辆举报

        /// <summary>
        /// 获取举报信息
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        [Route("GetTipOffListByCarId")]
        [HttpGet]
        public JResult GetTipOffListByCarId(string carid)
        {
            return _carervice.GetTipOffListByCarId(carid);
        }

        /// <summary>
        /// 添加举报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddTipOff")]
        [HttpPost]
        public JResult AddTipOff([FromBody]CarTipOffModel model)
        {
            return _carervice.AddTipOff(model);
        }

        /// <summary>
        /// 获取举报
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetTipOffPageList")]
        [HttpPost]
        public BasePageList<CarTipOffModel> GetTipOffPageList([FromBody]CarTipQueryModel query)
        {
            return _carervice.GetTipOffPageList(query);
        }

        /// <summary>
        /// 举报处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("HandleTipOff")]
        [HttpPost]
        public JResult HandleTipOff([FromBody]CarTipHandleModel model)
        {
            return _carervice.HandleTipOff(model);
        }

        #endregion

        #region 会员车辆

        /// <summary>
        /// 根据手机号获取会员拥有的车辆
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [Route("GetCarInfoByMobile")]
        [HttpGet]
        public JResult GetCarInfoByMobile(string mobile)
        {
            return _carervice.GetCarInfoByMobile(mobile);
        }

        #endregion

        #region 车贷相关

        /// <summary>
        /// 获取贷款列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetCarLoanList")]
        [HttpPost]
        public BasePageList<CarLoanViewModel> GetCarLoanList(CarLoanQueryModel query)
        {
            return _carervice.GetCarLoanList(query);
        }

        /// <summary>
        /// 车贷申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddCarLoan")]
        [HttpPost]
        public JResult AddCarLoan(CarLoanModel model)
        {
            return _carervice.AddCarLoan(model);
        }

        /// <summary>
        /// 车贷修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateCarLoan")]
        [HttpPost]
        public JResult UpdateCarLoan(CarLoanModel model)
        {
            return _carervice.UpdateCarLoan(model);
        }

        /// <summary>
        /// 根据ID获取贷款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("CarLoanInfo")]
        [HttpGet]
        public JResult CarLoanInfo(string id)
        {
            return _carervice.CarLoanInfo(id);
        }

        /// <summary>
        /// 添加贷款图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddLoanPicture")]
        [HttpPost]
        public JResult AddLoanPicture(CarLoanPicture model)
        {
            return _carervice.AddLoanPicture(model);
        }

        /// <summary>
        /// 根据贷款ID获取对应的图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetLoanPictureByloanid")]
        [HttpGet]
        public JResult GetLoanPictureByloanid(string id)
        {
            return _carervice.GetLoanPictureByloanid(id);
        }

        /// <summary>
        /// 删除贷款图片
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("DeleteLoanPicture")]
        [HttpDelete]
        public JResult DeleteLoanPicture(string innerid)
        {
            return _carervice.DeleteLoanPicture(innerid);
        }

        #endregion

        #region 金融方案

        /// <summary>
        /// 获取金融方案列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("GetFinanceProgrammeList")]
        [HttpPost]
        public BasePageList<FinanceProgrammeViewModel> GetFinanceProgrammeList(FinanceProgrammeQueryModel query)
        {
            return _carervice.GetFinanceProgrammeList(query);
        }

        /// <summary>
        /// 金融方案新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddFinanceProgramme")]
        [HttpPost]
        public JResult AddFinanceProgramme(FinanceProgrammeModel model)
        {
            return _carervice.AddFinanceProgramme(model);
        }

        /// <summary>
        /// 金融方案新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateFinanceProgramme")]
        [HttpPost]
        public JResult UpdateFinanceProgramme(FinanceProgrammeModel model)
        {
            return _carervice.UpdateFinanceProgramme(model);
        }

        /// <summary>
        /// 根据id获取金融方案详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("GetFinanceProgrammeById")]
        [HttpGet]
        public JResult GetFinanceProgrammeById(string innerid)
        {
            return _carervice.GetFinanceProgrammeById(innerid);
        }

        /// <summary>
        /// 经融方案明细新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddFinanceProgrammeDetail")]
        [HttpPost]
        public JResult AddFinanceProgrammeDetail(FinanceProgrammeDetailModel model)
        {
            return _carervice.AddFinanceProgrammeDetail(model);
        }

        /// <summary>
        /// 金融方案明细修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateFinanceProgrammeDetail")]
        [HttpPost]
        public JResult UpdateFinanceProgrammeDetail(FinanceProgrammeDetailModel model)
        {
            return _carervice.UpdateFinanceProgrammeDetail(model);
        }

        /// <summary>
        /// 根据id获取金融方案明细详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [Route("GetFinanceProgrammeDetailById")]
        [HttpGet]
        public JResult GetFinanceProgrammeDetailById(string innerid)
        {
            return _carervice.GetFinanceProgrammeDetailById(innerid);
        }

        #endregion

        #region 供应商管理

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="model">供应商信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddSupplier")]
        public JResult AddSupplier([FromBody]CarSupplierModel model)
        {
            return _carervice.AddSupplier(model);
        }

        /// <summary>
        /// 修改供应商
        /// </summary>
        /// <param name="model">供应商信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSupplier")]
        public JResult UpdateSupplier([FromBody]CarSupplierModel model)
        {
            return _carervice.UpdateSupplier(model);
        }

        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="innerid">供应商model</param>
        /// <returns>1.操作成功</returns>
        [HttpDelete]
        [Route("DeleteSupplier")]
        public JResult DeleteSupplier(string innerid)
        {
            return _carervice.DeleteSupplier(innerid);
        }
        /// <summary>
        /// 供应商列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSupplierCarPageList")]
        public BasePageList<CarSupplierModel> GetSupplierCarPageList([FromBody]CarSupplierQueryModel query)
        {
            return _carervice.GetSupplierCarPageList(query);
        }
        /// <summary>
        /// 获取会员所有供应商列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSupplierAll")]
        public JResult GetSupplierAll()
        {
            return _carervice.GetSupplierAll();
        }

        /// <summary>
        /// 根据id获取供应商的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSupplierInfoById")]
        public JResult GetSupplierInfoById(string innerid)
        {
            return _carervice.GetSupplierInfoById(innerid);
        }

        #endregion

        #region 神秘车源
        
        /// <summary>
        /// 后台查询神秘车源列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMysteriousBackCarPageList")]
        public BasePageList<CarMysteriousListModel> GetMysteriousBackCarPageList([FromBody]CarGlobalQueryModel query)
        {
            return _carervice.GetMysteriousBackCarPageList(query);
        }

        /// <summary>
        /// 顶神秘车源
        /// </summary>
        /// <param name="innerid">车辆id</param>
        /// <returns>1.操作成功</returns>
        [HttpGet]
        [Route("PushUpMysteriousCar")]
        public JResult PushUpMysteriousCar(string innerid)
        {
            return _carervice.PushUpMysteriousCar(innerid);
        }
        #endregion

        #region 劲爆车源

        /// <summary>
        /// 添加劲爆车源
        /// </summary>
        /// <param name="model">车源信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddMaddenCar")]
        public JResult AddMaddenCar([FromBody]CarMaddenModel model)
        {
            return _carervice.AddMaddenCar(model);
        }

        /// <summary>
        /// 修改劲爆车源
        /// </summary>
        /// <param name="model">车源信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateMaddenCar")]
        public JResult UpdateMaddenCar([FromBody]CarMaddenModel model)
        {
            return _carervice.UpdateMaddenCar(model);
        }

        /// <summary>
        /// 删除劲爆车源
        /// </summary>
        /// <param name="model">删除model</param>
        /// <returns>1.操作成功</returns>
        [HttpDelete]
        [Route("DeleteMaddenCar")]
        public JResult DeleteMaddenCar([FromBody]CarMaddenModel model)
        {
            return _carervice.DeleteMaddenCar(model);
        }

        /// <summary>
        /// 回复劲爆车辆
        /// </summary>
        /// <param name="model">回复model</param>
        /// <returns>1.操作成功</returns>
        [HttpPost]
        [Route("RecoveryMaddenCar")]
        public JResult RecoveryMaddenCar([FromBody]CarMaddenModel model)
        {
            return _carervice.RecoveryMaddenCar(model);
        }

        /// <summary>
        /// 车辆劲爆成交
        /// </summary>
        /// <param name="model">车辆model</param>
        /// <returns>1.操作成功</returns>
        [HttpPost]
        [Route("DealMaddenCar")]
        public JResult DealMaddenCar([FromBody]CarMaddenModel model)
        {
            return _carervice.DealMaddenCar(model);
        }
        /// <summary>
        /// 顶劲爆车源
        /// </summary>
        /// <param name="innerid">车辆id</param>
        /// <returns>1.操作成功</returns>
        [HttpGet]
        [Route("PushUpMaddenCar")]
        public JResult PushUpMaddenCar(string innerid)
        {
            return _carervice.PushUpMaddenCar(innerid);
        }
        /// <summary>
        /// 根据id获取劲爆车源的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMaddenCarInfoById")]
        public JResult GetMaddenCarInfoById(string innerid)
        {
            return _carervice.GetMaddenCarInfoById(innerid);
        }

        /// <summary>
        /// 根据id获取劲爆车源的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMaddenCarViewById")]
        public JResult GetMaddenCarViewById(string innerid)
        {
            return _carervice.GetMaddenCarViewById(innerid);
        }

        /// <summary>
        /// 后台查询劲爆车源列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMaddenCarBackPageList")]
        public BasePageList<CarMaddenListModel> GetMaddenCarBackPageList([FromBody]CarMaddenQueryModel query)
        {
            return _carervice.GetMaddenCarBackPageList(query);
        }

        #endregion
    }
}

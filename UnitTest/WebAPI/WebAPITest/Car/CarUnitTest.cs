using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cedar.Core.IoC;
using CCN.Modules.Car.Interface;
using CCN.Modules.Car.BusinessService;
using CCN.Modules.Car.DataAccess;
using CCN.Modules.Car.BusinessEntity;

namespace WebAPITest.Car
{
    [TestClass]
    public class CarUnitTest
    {
        private readonly ICarManagementService cms;

        public CarUnitTest()
        {
            cms = ServiceLocatorFactory.GetServiceLocator().GetService<ICarManagementService>();
        }

        /// <summary>
        /// 获取车辆详细信息
        /// </summary>
        [TestMethod]
        public void GetCarInfoById()
        {
            string id = "";//车辆ID
            var value = cms.GetCarInfoById(id);
            Assert.IsTrue(value.errcode == 0);
        }

        /// <summary>
        /// 获取车辆详情(view)
        /// </summary>
        [TestMethod]
        public void GetCarViewById()
        {
            string id = "";//车辆ID
            var value = cms.GetCarViewById(id);
            Assert.IsTrue(value.errcode == 0);
        }

        /// <summary>
        /// 车辆赋值
        /// </summary>
        [TestMethod]
        public void GetCarEvaluateById()
        {
            string id = "";//车辆ID
            var value = cms.GetCarEvaluateById(id);
            Assert.IsTrue(value.errcode == 0);
        }

        /// <summary>
        /// 获取本月本车型成交数量
        /// </summary>
        [TestMethod]
        public void GetCarSales()
        {
            string modelId = "";//车型ID
            var value = cms.GetCarSales(modelId);
            Assert.IsTrue(value.errcode == 0);
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        [TestMethod]
        public void AddCar()
        {
            var model = new CarInfoModel
            {
                Innerid = Guid.NewGuid().ToString(),
                Carid = 0,
                title = "",
                pic_url = "",
                provid = 0,
                cityid = 0,
                brand_id = 0,
                series_id = 0,
                model_id = 0,
                colorid = 0,
                buytime = DateTime.Now,
                buyprice = 0,
                dealprice = 0,
                avgprice = 0,
                dealnumber = 0,
                isproblem = 0,
                sellreason = "转让原因",
                masterdesc = "原车主信息",
                dealdesc = "成交备注",
                deletedesc = "删除备注",
                ckyear_date = DateTime.Now,
                tlci_date = DateTime.Now,
                istain = 0,
                price = 0,
                mileage = 0,
                register_date = DateTime.Now,
                seller_type = 2,
                status = 1,
                remark = "车况备注/优势",
                createdtime = DateTime.Now,
                modifiedtime = DateTime.Now,
                post_time = DateTime.Now,
                audit_time = DateTime.Now,
                sold_time = DateTime.Now,
                estimateprice = "车源估值情况",
                eval_price = 0,
                next_year_eval_price = 0,
                audit_date = DateTime.Now,
                custid = "会员ID",
                provname = "省份",
                cityname = "城市",
                brand_name = "品牌",
                series_name = "车系",
                model_name = "车型",
                geartype = "变速箱类型",
                color = "颜色",
                liter = "排量",
                dischargeName = "排放标准"

            };
            var value = cms.AddCar(model);
            Assert.IsTrue(value.errcode == 0);
        }
    }
}

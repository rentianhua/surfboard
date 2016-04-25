using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cedar.Core.IoC;
using CCN.Modules.Base.Interface;
using CCN.Modules.Base.BusinessService;
using CCN.Modules.Base.DataAccess;
using CCN.Modules.Base.BusinessEntity;

namespace WebAPITest.Base
{
    [TestClass]
    public class BaseUnitTest
    {
        private readonly IBaseManagementService bms;

        public BaseUnitTest()
        {
            bms = ServiceLocatorFactory.GetServiceLocator().GetService<IBaseManagementService>();
        }

        /// <summary>
        /// 获取代码值列表
        /// </summary>
        [TestMethod]
        public void GetCodeByTypeKey()
        {
            var value = bms.GetCodeByTypeKey("1");
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 手机获取验证码
        /// </summary>
        [TestMethod]
        public void SendVerification()
        {
            var model = new BaseVerification
            {
                Innerid = Guid.NewGuid().ToString(),
                Target = "18662240324"
            };
            var sss = nameof(model);
            var value = bms.SendVerification(model);
            Assert.IsTrue(value.errcode == 0);
        }

        /// <summary>
        /// 测试新特性
        /// </summary>
        [TestMethod]
        public void Test()
        {
            var model = new BaseVerification
            {
                Innerid = Guid.NewGuid().ToString(),
                Target = "18662240324"
            };
            var sss = nameof(model);
            var value = bms.SendVerification(model);
            Assert.IsTrue(value.errcode == 0);
        }
    }
}

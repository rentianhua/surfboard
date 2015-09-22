using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cedar.Core.IoC;
using Cedar.Framwork.AuditTrail.BusinessEntity;
using Cedar.Framwork.AuditTrail.Interface;
using FrameworkTest.TestService;

namespace FrameworkTest.AuditTrail
{
    [TestClass]
    public class AuditTrailUnitTest
    {
        private static IAuditTrailManagementService iAuditTrailManagementService;
        private static ITestService iTestService;

        public AuditTrailUnitTest()
        {
            iAuditTrailManagementService = ServiceLocatorFactory.GetServiceLocator().GetService<IAuditTrailManagementService>();
            iTestService = ServiceLocatorFactory.GetServiceLocator().GetService<ITestService>();
        }


        [TestMethod]
        public void InsertAuditLogTestMethod()
        {
            var data = new AuditLogModel()
            {
                ID = Guid.NewGuid().ToString(),
                AuditName = "AuditName",
                AuditType = "AuditType",
                Arguments = "Arguments",
                LogDateTime = DateTime.Now,
                Result = "Result",
                Target = "Target",
                TransactionID = "TransactionID"
            };
            var result = iAuditTrailManagementService.InsertAuditLog(data);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AuditTrailCallHandlerTestMethod()
        {
            var result = iTestService.SayHello(new { id = "1", name = "name" });
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartac.SR.Core.EntLib.IoC;
using Smartac.SR.Core.IoC;

namespace FrameworkTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var a = new UnityContainerServiceLocator();
            var service = ServiceLocatorFactory.GetServiceLocator("DemoInstanceService");
            Assert.IsNotNull(service);
        }
    }
}

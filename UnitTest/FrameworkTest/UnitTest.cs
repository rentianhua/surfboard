using System;
using System.Linq;
using Cedar.Core.EntLib.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrameworkTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var factoy = new DatabaseWrapperFactory().GetDatabase("mysqldb");
            var d = factoy.Query("select * from base_carbrand").ToList();
            Assert.IsNotNull(factoy);
        }
    }
}

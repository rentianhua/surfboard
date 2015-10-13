using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Weixin.MP.CommonAPIs;

namespace FrameworkTest.Wechat
{
    [TestClass]
    public class WechatUnitTest
    {
        private static readonly string APPID = ConfigurationManager.AppSettings["APPID"];
        private static readonly string AppSecret = ConfigurationManager.AppSettings["AppSecret"];
        private const string wechatkey = "wechatkey";

        public WechatUnitTest()
        {
            AccessTokenRedisContainer.Register(APPID, AppSecret);
        }

        [TestMethod]
        public void CheckRegisteredTestMethod()
        {
            var result = AccessTokenRedisContainer.CheckRegistered(APPID);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetAccessTokenTestMethod()
        {
            var result = AccessTokenRedisContainer.GetAccessToken(APPID);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetJsApiTicketResultTestMethod()
        {
            var result = AccessTokenRedisContainer.GetJsApiTicketResult(APPID);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TryGetAccessTokenTestMethod()
        {
            var originalresult = AccessTokenRedisContainer.GetAccessToken(APPID);
            var result = AccessTokenRedisContainer.TryGetAccessToken(APPID, AppSecret, true);
            Assert.AreNotEqual(originalresult, result);
        }

        [TestMethod]
        public void TryGetJsApiTicketTestMethod()
        {
            var originalresult = AccessTokenRedisContainer.GetJsApiTicket(APPID);
            var result = AccessTokenRedisContainer.TryGetJsApiTicket(APPID, AppSecret, true);
            Assert.AreEqual(originalresult, result);
        }
    }
}

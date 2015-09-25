using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.AuditTrail.Interception;
using Cedar.Framwork.Caching.Interception;

namespace FrameworkTest.TestService
{
    public class TestService : ITestService
    {
        [AuditTrailCallHandler("SayHello")]
        public string SayHello(dynamic words)
        {
            return "SayHelloResults";
        }

        [CachingCallHandler()]
        public string SayHelloCaching(dynamic words)
        {
            return "SayHelloResults";
        }
    }
}

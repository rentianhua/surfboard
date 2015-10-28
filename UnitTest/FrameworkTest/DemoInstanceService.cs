using Cedar.AuditTrail.Interception;
using Cedar.Framwork.AuditTrail.Interception;

namespace FrameworkTest
{
    public class DemoInstanceService
    {
        [AuditTrailCallHandler("test")]
        public string TestMethod(string input)
        {
            return input;
        }
    }
}
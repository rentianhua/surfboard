using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smartac.SR.AuditTrail.Interception;

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

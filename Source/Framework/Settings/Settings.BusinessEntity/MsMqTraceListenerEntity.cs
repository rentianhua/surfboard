using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiiP.Framework.Settings.BusinessEntity
{
    public class MsMqTraceListenerEntity
    {
        public string QueuePath { get; set; }
        public string TimeToBeReceived { get; set; }
        public string TimeToReachQueue { get; set; }
    }
}

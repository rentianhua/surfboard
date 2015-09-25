using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity.Utility;

namespace Cedar.Framwork.AuditTrail
{
    internal class CompositeAuditLogListener : AuditLogListenerBase
    {
        public IEnumerable<AuditLogListenerBase> Listeners
        {
            get;
            private set;
        }

        public CompositeAuditLogListener(IEnumerable<AuditLogListenerBase> listeners) : base(Guid.NewGuid().ToString(), null)
        {
            Guard.ArgumentNotNull(listeners, "listeners");
            this.Listeners = listeners;
        }

        protected override void WriteCore(AuditLogEntry logEntry)
        {
            Guard.ArgumentNotNull(logEntry, "logEntry");
            foreach (AuditLogListenerBase current in this.Listeners)
            {
                current.Write(logEntry);
            }
        }
    }
}

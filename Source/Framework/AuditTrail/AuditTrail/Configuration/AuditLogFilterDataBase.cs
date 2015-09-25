using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Core.Configuration;

namespace Cedar.Framwork.AuditTrail.Configuration
{
    /// <summary>
    /// This class is base class of all concrete audit log filter based configuration element classes.
    /// </summary>
    public class AuditLogFilterDataBase : ProviderDataBase<IAuditLogFilter>
    {
    }
}

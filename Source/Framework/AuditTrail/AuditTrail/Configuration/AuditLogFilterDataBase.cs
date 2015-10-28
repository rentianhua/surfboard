using Cedar.Core.Configuration;
using Cedar.Framework.AuditTrail.Base;

namespace Cedar.Framework.AuditTrail.Configuration
{
    /// <summary>
    ///     This class is base class of all concrete audit log filter based configuration element classes.
    /// </summary>
    public class AuditLogFilterDataBase : ProviderDataBase<IAuditLogFilter>
    {
    }
}
using System;
using Cedar.Core.Configuration;
using Cedar.Framework.AuditTrail.Base;

namespace Cedar.Framework.AuditTrail.Configuration
{
    /// <summary>
    ///     The <see cref="T:Cedar.Framework.AuditTrail.MatchAllAuditLogFilter" /> based configuration element.
    /// </summary>
    public class MatchAllAuditLogFilterData : AuditLogFilterDataBase
    {
        /// <summary>
        ///     Gets the provider creation expression.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The delegate to create <see cref="T:Cedar.Framework.AuditTrail.MatchAllAuditLogFilter" />.</returns>
        public override Func<IAuditLogFilter> GetProviderCreator(ServiceLocatableSettings settings)
        {
            return () => new MatchAllAuditLogFilter(null);
        }
    }
}
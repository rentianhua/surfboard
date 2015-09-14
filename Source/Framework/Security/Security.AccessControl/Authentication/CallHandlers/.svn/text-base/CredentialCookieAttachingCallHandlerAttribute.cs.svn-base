using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

using NCS.IConnect.PolicyInjection.CallHandlers;
using System;

namespace HiiP.Framework.Security.AccessControl.CallHandlers
{
    /// <summary>
    /// CredentialCookieAttachingCallHandler attribute used in the attribute-driven interception policy
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public sealed class CredentialCookieAttachingCallHandlerAttribute : HandlerAttributeBase
    {
        /// <summary>
        /// Create the attribute matched call handler: CredentialCookieAttachingCallHandler
        /// </summary>
        /// <returns>The created CredentialCookieAttachingCallHandler instance.</returns>
        public override ICallHandler CreateHandler()
        {
            return new CredentialCookieAttachingCallHandler {Ordinal = Ordinal};
        }
    }
}
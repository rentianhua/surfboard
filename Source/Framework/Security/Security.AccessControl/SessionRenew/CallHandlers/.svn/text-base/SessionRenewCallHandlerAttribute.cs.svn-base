using NCS.IConnect.PolicyInjection.CallHandlers;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System;

namespace HiiP.Framework.Security.AccessControl.SessionRenew.CallHandlers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public sealed class SessionRenewCallHandlerAttribute : HandlerAttributeBase
    {
        public override ICallHandler CreateHandler()
        {
            return new SessionRenewCallHandler { Ordinal = this.Ordinal };
        }
    }
}

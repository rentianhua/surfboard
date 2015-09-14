

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

namespace HiiP.Framework.Security.AccessControl.SessionRenew.CallHandlers
{
    /// <summary>
    /// A call handler used to perform session refresh.
    /// </summary>
    [ConfigurationElementType(typeof(SessionRenewCallHandlerData))]
    public class SessionRenewCallHandler : CallHandlerBase
    {
        /// <summary>
        /// Performs the operation of the handler.
        /// </summary>
        /// <param name="input">Input to the method call.</param>
        /// <param name="getNext">Delegate used to get the next delegate in the call handler pipeline.</param>
        /// <returns> Return value from the target.</returns>
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            SessionRenewManager.RefreshSession(false);

            return getNext()(input, getNext);
        }
    }
}

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

using Cedar.PolicyInjection.Configuration;

namespace HiiP.Framework.Security.AccessControl.CallHandlers
{
    /// <summary>
    /// A CallHandlerData class for configuration information stored about a CredentialCookieAttachingCallHandler.
    /// </summary>
    [Assembler(typeof (CredentialCookieAttachingCallHandlerAssembler))]
    public class CredentialCookieAttachingCallHandlerData : CallHandlerDataBase
    {
    }
}
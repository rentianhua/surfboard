using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ObjectBuilder;

namespace HiiP.Framework.Security.AccessControl.CallHandlers
{
    /// <summary>
    /// A call handler assembler used to create CredentialCookieAttachingCallHandler based on the configured CredentialCookieAttachingCallHandlerData.
    /// </summary>
    public class CredentialCookieAttachingCallHandlerAssembler : IAssembler<ICallHandler, CallHandlerData>
    {
        #region IAssembler<ICallHandler,CallHandlerData> Members

        /// <summary>
        /// Builds an instance of the CredentialCookieAttachingCallHandler based on an configured CredentialCookieAttachingCallHandlerData object.
        /// </summary>
        /// <param name="context">The Microsoft.Practices.ObjectBuilder.IBuilderContext that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the CredentialCookieAttachingCallHandler object.</returns>
        public ICallHandler Assemble(IBuilderContext context, CallHandlerData objectConfiguration,
                                     IConfigurationSource configurationSource,
                                     ConfigurationReflectionCache reflectionCache)
        {
            var handlerData = objectConfiguration as CredentialCookieAttachingCallHandlerData;
            return new CredentialCookieAttachingCallHandler { Ordinal = (handlerData==null) ? 0 : handlerData.Ordinal };
        }

        #endregion
    }
}
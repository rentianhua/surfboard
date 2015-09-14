using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace HiiP.Framework.Security.AccessControl.SessionRenew.CallHandlers
{
    public class SessionRenewCallHandlerAssembler : IAssembler<ICallHandler, CallHandlerData>
    {
        #region IAssembler<ICallHandler,CallHandlerData> Members

        public ICallHandler Assemble(IBuilderContext context, CallHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            SessionRenewCallHandlerData handlerData = objectConfiguration as SessionRenewCallHandlerData;
            return new SessionRenewCallHandler { Ordinal = (handlerData==null) ? 0 : handlerData.Ordinal };
        }

        #endregion
    }
}

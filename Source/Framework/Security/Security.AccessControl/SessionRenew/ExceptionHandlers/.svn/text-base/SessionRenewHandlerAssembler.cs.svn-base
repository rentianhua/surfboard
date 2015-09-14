using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace HiiP.Framework.Security.AccessControl.SessionRenew.ExceptionHandlers
{
    public class SessionRenewHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
    {
        #region IAssembler<IExceptionHandler,ExceptionHandlerData> Members

        public IExceptionHandler Assemble(IBuilderContext context, ExceptionHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            SessionRenewHandlerData data = (SessionRenewHandlerData)objectConfiguration;
            return new SessionRenewHandler(data.Caption, data.FormatterType, data.Template, data.IncludeInnerException);

        }

        #endregion
    }
}

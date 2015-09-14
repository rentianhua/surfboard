using System;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Security.AccessControl.Interface;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using HiiP.Framework.Logging.Library;
using HiiP.Infrastructure.Interface.Constants;

namespace HiiP.Framework.Security.AccessControl.Authorization
{
    public class ViewAuthorizationProxy : ServiceProxyBase<IViewAuthorizationService>, IViewAuthorizationService
    {
        public const string EndpointName = "ViewAuthorizationService";

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAuthorizationProxy"/> class.
        /// </summary>
        /// <param name="endpointName">Name of the endpoint.</param>
        public ViewAuthorizationProxy(string endpointName)
            : base(endpointName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAuthorizationProxy"/> class.
        /// </summary>
        public ViewAuthorizationProxy()
        {
            this.WrapObject(new ViewAuthorizationProxy(EndpointName));
        }

        #region IViewAuthorizationService Members

        /// <summary>
        /// Tries the get action code.
        /// </summary>
        /// <param name="viewType">The full name of view type.</param>
        /// <param name="viewStatus">The view status.</param>
        /// <param name="actionCode">The action code matching the combination of view type and view status.</param>
        /// <returns>
        /// A <see cref="Boolean"/> value indicating if the view exists in the mapping table.
        /// </returns>
        [CachingCallHandler]
        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.AdminModuleID,
   FunctionID = HiiP.Framework.Security.AccessControl.Interface.Constants.FunctionNames.GetActionCodeFunctionID)]
        public string GetActionCode(string viewType, string viewStatus)
        {
            return this.Proxy.GetActionCode(viewType, viewStatus);
        }

        #endregion
    }
}

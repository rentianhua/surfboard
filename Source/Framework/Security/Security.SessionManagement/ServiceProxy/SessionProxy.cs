using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using HiiP.Framework.Common;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.ApplicationContexts.CallHandlers;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.SessionManagement.Interface.Constants;
using HiiP.Framework.Security.SessionManagement.Interface.Services;
using HiiP.Framework.Security.AccessControl.Interface;
using HiiP.Framework.Security.AccessControl.BusinessEntity;

namespace HiiP.Framework.Security.SessionManagement.ServiceProxy
{
    /// <summary>
    /// Session Proxy used to invocate session service.
    /// </summary>   
    public class SessionProxy : ServiceProxyBase<ISessionService>, ISessionService
    {
        public SessionProxy(string endpointName)
            : base(endpointName)
        { }

        public SessionProxy()
        {
            base.WrapObject(new SessionProxy(EndpointNames.SessionService));
        }


        #region ISessionService Members

         [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.AdminModuleID,
         FunctionID = FunctionNames.KillSessionsFunctionID)]
        public void KillSessions(Guid[] ids)
        {
            this.Proxy.KillSessions(ids);
        }

         [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.AdminModuleID,
          FunctionID = FunctionNames.GetActiveSessionsFunctionID)]
         public SessionInfo[] GetActiveSessions()
        {
            return this.Proxy.GetActiveSessions();
        }

         [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.AdminModuleID,
          FunctionID = FunctionNames.GetActiveSessionsFunctionID)]
         public SessionInfo[] GetActiveSessions(string userName)
        {
            return this.Proxy.GetActiveSessions(userName);
        }

         [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.AdminModuleID,
          FunctionID = FunctionNames.GetActiveSessionsFunctionID)]
         public SessionInfo[] GetActiveSessions(string userName, string ipAddress, string hostName)
        {
            return this.Proxy.GetActiveSessions(userName, ipAddress, hostName);
        }

         [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.AdminModuleID,
          FunctionID = FunctionNames.GetActiveSessionsFunctionID)]
         public SessionInfo[] GetActiveSessions(string userName, DateTime startTimeFrom, DateTime startTimeTill)
        {
            return this.Proxy.GetActiveSessions(userName, startTimeFrom, startTimeTill);
        }

         [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.AdminModuleID,
          FunctionID = FunctionNames.GetActiveSessionsFunctionID)]
         public SessionInfo[] GetActiveSessions(string userName, string ipAddress, string hostName, DateTime startTimeFrom, DateTime startTimeTill, DateTime lastActivityTimeFrom, DateTime lastActivityTimeTill)
        {
            return this.Proxy.GetActiveSessions(userName, ipAddress, hostName, startTimeFrom, startTimeTill, lastActivityTimeFrom, lastActivityTimeTill);
        }

        #endregion
    }
}

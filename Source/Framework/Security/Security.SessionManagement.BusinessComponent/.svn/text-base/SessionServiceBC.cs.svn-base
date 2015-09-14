using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.SessionManagement.Interface.Constants;
using HiiP.Framework.Security.SessionManagement.DataAccess;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Security.AccessControl.Interface.Configuration;
using HiiP.Framework.Security.AccessControl.BusinessEntity;
using HiiP.Framework.Security.AccessControl.Interface;
using System.Diagnostics.CodeAnalysis;
using HiiP.Framework.Common.ApplicationContexts;
using System.Data;

namespace HiiP.Framework.Security.SessionManagement.BusinessComponent
{
    public class SessionServiceBC : HiiPBusinessComponentBase
    {
        #region Private Static Fields
        private static DateTime MinDateTime = HiiP.Framework.Common.Constants.MinMaxValues.MinDate;
        private static DateTime MaxDateTime = HiiP.Framework.Common.Constants.MinMaxValues.MinDate;
        #endregion

        //Unable to use instance builder here, because it will make IIS crash.

        [SuppressMessage("HiiP.Usages", "HP5002:UseInstanceBuilderToCreateObjectOnServerSide", MessageId = "UseInstanceBuilderToCreateObjectOnServerSide")]
        private SessionDA _sessionDA = new SessionDA();

        #region ISessionService Members

        /// <summary>
        /// Kill a given existing active session list.
        /// </summary>
        /// <param name="ids">A Guid array representing the session id list to kill.</param>
        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AdminModuleID,
         FunctionID = FunctionNames.KillSessionsFunctionID, Ordinal = 1)]
        public void KillSessions(Guid[] ids)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(",");
            foreach (Guid sessionID in ids)
            {
                stringBuilder.Append(sessionID.ToString() + ",");
            }

            this._sessionDA.KillSessions(stringBuilder.ToString());
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AdminModuleID,
        FunctionID = FunctionNames.GetActiveSessionsFunctionID, Ordinal = 1)]
        public SessionInfo[] GetActiveSessions()
        {
            int sessionTimeoutMinutes = SessionSettings.GetServiceSetting().SessionTimeout;
            return this._sessionDA.GetAllSessions(sessionTimeoutMinutes);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AdminModuleID,
        FunctionID = FunctionNames.GetActiveSessionsFunctionID, Ordinal = 1)]
        public SessionInfo[] GetActiveSessions(string userName)
        {
            return this.GetActiveSessions(userName, string.Empty, string.Empty, MinDateTime, MaxDateTime, MinDateTime, MaxDateTime);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AdminModuleID,
       FunctionID = FunctionNames.GetActiveSessionsFunctionID, Ordinal = 1)]
        public SessionInfo[] GetActiveSessions(string userName, string ipAddress, string hostName)
        {
            return this.GetActiveSessions(userName, ipAddress, hostName, MinDateTime, MaxDateTime, MinDateTime, MaxDateTime);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AdminModuleID,
       FunctionID = FunctionNames.GetActiveSessionsFunctionID, Ordinal = 1)]
        public SessionInfo[] GetActiveSessions(string userName, DateTime startTimeFrom, DateTime startTimeTill)
        {
            return this.GetActiveSessions(userName, string.Empty, string.Empty, startTimeFrom, startTimeTill, MinDateTime, MaxDateTime);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AdminModuleID,
       FunctionID = FunctionNames.GetActiveSessionsFunctionID, Ordinal = 1)]
        public SessionInfo[] GetActiveSessions(string userName, string ipAddress, string hostName, DateTime startTimeFrom, DateTime startTimeTill, DateTime lastActivityTimeFrom, DateTime lastActivityTimeTill)
        {
            int sessionTimeoutMinutes = SessionSettings.GetServiceSetting().SessionTimeout;
            return this._sessionDA.GetSessions(userName, ipAddress, hostName, startTimeFrom, startTimeTill, lastActivityTimeFrom, lastActivityTimeTill, sessionTimeoutMinutes);
        }

        #endregion

        #region ISessionService Members
        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AdminModuleID, FunctionID = FunctionNames.CreateSessionFunctionID)]
        public SessionData CreateOrRenewSession(string userName, string fullName, string ipAddress, string hostName)
        {
            string sessionID = AppContext.Current.SessionID;
            if (string.IsNullOrEmpty(sessionID))
            {
                sessionID = Guid.NewGuid().ToString();
            }
            return this._sessionDA.CreateOrRenewSession(sessionID,userName, fullName, ipAddress, hostName);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AdminModuleID,FunctionID = FunctionNames.CheckSessionFunctionID)]
        public SessionData CheckSessionID(string sessionID, string userName)
        {
            return this._sessionDA.CheckSessionID(sessionID, userName);
        }
 
        public DataRow GetLogInfo(string userName, string ipAddress, string hostName, out SessionData sessionData)
        {
            return this._sessionDA.GetLogInfo(userName, ipAddress, hostName, out sessionData);
        }
        #endregion
    }
}

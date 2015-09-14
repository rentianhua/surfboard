#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :    HiiP.Framework.Security.SessionManagement.DataAccess/SessionDA
// COMPONENT DESC:  
//
// CREATED DATE/BY:   22/09/2008/Jiang Nan
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;

using HiiP.Framework.Common;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.SessionManagement.Interface.Constants;
using HiiP.Framework.Security.AccessControl.BusinessEntity;
using HiiP.Framework.Security.AccessControl.Interface;
using HiiP.Framework.Security.AccessControl.Interface.Configuration;
using NCS.IConnect.Helpers.Data;
using HiiP.Framework.Common.ApplicationContexts;

namespace HiiP.Framework.Security.SessionManagement.DataAccess
{
    /// <summary>
    /// This class is used to perform data access related operation for session management.
    /// </summary>
    public class SessionDA : HiiPDataAccessBase
    {

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.AdminModuleID,
    FunctionID = FunctionNames.CheckSessionFunctionID)]
        public SessionData CheckSessionID(string sessionID, string userName)
        {
            int timeoutMinutes = SessionSettings.GetServiceSetting().SessionTimeout;
            DbHelper dbHelper = new DbHelper();
            DbCommand comannd = dbHelper.BuildDbCommand("P_IC_SESSIONS_CHECK_USER_NAME");
            dbHelper.AssignParameterValues(comannd, sessionID, userName, timeoutMinutes,DateTime.Now);
            dbHelper.ExecuteNonQuery(comannd);
            SessionData sessionData = new SessionData();
            sessionData.IsKilled = (bool)dbHelper.GetParameterValue(comannd, "p_is_killed");
            sessionData.IsSessionMatched = (bool)dbHelper.GetParameterValue(comannd, "p_user_session_matched");
            sessionData.IsTimeoutOrInvalid = (bool)dbHelper.GetParameterValue(comannd, "p_is_timeout");
            return sessionData;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.AdminModuleID,
          FunctionID = FunctionNames.GetActiveSessionsFunctionID)]
        public SessionInfo[] GetSessions(string userName, string ipAddress, string hostName, DateTime startTimeFrom, DateTime startTimeTill, DateTime lastActivityTimeFrom, DateTime lastActivityTimeTill, int timeoutMinute)
        {
            DbCommand command = this.Helper.BuildDbCommand("P_IC_SESSIONS_GET");
            this.Helper.AssignParameterValues(command, userName, ipAddress, hostName, startTimeFrom, startTimeTill, lastActivityTimeFrom, lastActivityTimeTill, DateTime.Now, timeoutMinute);

            List<SessionInfo> sessionList = new List<SessionInfo>();
            DataTable sessionTable = new DataTable();
            this.Helper.Fill(sessionTable, command);

            foreach (DataRow row in sessionTable.Rows)
            {
                var sessionID = row["SESSION_ID"] as string;
                SessionInfo sessionInfo = new SessionInfo
                                              {
                    SessionID = string.IsNullOrEmpty(sessionID)?Guid.NewGuid():new Guid(sessionID),
                    UserName = row["USER_NAME"] as string,
                    FullName = row["FULL_NAME"] as string,
                    IPAddress = row["IP_ADDRESS"] as string,
                    HostName = row["HOST_NAME"] as string,
                    StartTime = (DateTime)row["START_TIME"],
                    LastActivityTime = (DateTime)row["LAST_ACTIVITY_TIME"]
                };

                sessionList.Add(sessionInfo);
            }

            return sessionList.ToArray();
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.AdminModuleID,FunctionID = FunctionNames.GetActiveSessionsFunctionID)]
        public SessionInfo[] GetAllSessions(int timeoutMinute)
        {
            DbCommand command = this.Helper.BuildDbCommand("P_IC_SESSIONS_GET_ALL");
            this.Helper.AssignParameterValues(command,DateTime.Now, timeoutMinute);

            List<SessionInfo> sessionList = new List<SessionInfo>();
            DataTable sessionTable = new DataTable();
            this.Helper.Fill(sessionTable, command);

            foreach (DataRow row in sessionTable.Rows)
            {
                var sessionID = row["SESSION_ID"] as string;
                SessionInfo sessionInfo = new SessionInfo
                {
                    SessionID = string.IsNullOrEmpty(sessionID) ? Guid.NewGuid() : new Guid(sessionID),
                    UserName = row["USER_NAME"] as string,
                    FullName = row["FULL_NAME"] as string,
                    IPAddress = row["IP_ADDRESS"] as string,
                    HostName = row["HOST_NAME"] as string,
                    StartTime = (DateTime)row["START_TIME"],
                    LastActivityTime = (DateTime)row["LAST_ACTIVITY_TIME"]
                };

                sessionList.Add(sessionInfo);
            }

            return sessionList.ToArray();
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.AdminModuleID,
       FunctionID = FunctionNames.KillSessionsFunctionID)]
        public void KillSessions(string sessionIds)
        {
            DbCommand command = this.Helper.BuildDbCommand("P_IC_SESSIONS_KILL");
            this.Helper.AssignParameterValues(command, sessionIds);
            this.Helper.ExecuteNonQuery(command);
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.AdminModuleID,
    FunctionID = FunctionNames.CreateSessionFunctionID)]
        public SessionData CreateOrRenewSession(string sessionId,string userName, string fullName, string ipAddress, string hostName)
        {
            int timeoutMinutes = SessionSettings.GetServiceSetting().SessionTimeout;
            DbHelper dbHelper = new DbHelper();
            DbCommand command = dbHelper.BuildDbCommand("P_IC_SESSIONS_CREATE_OR_RENEW");
            dbHelper.AssignParameterValues(command, sessionId, userName, fullName, DateTime.Now,
                DateTime.Now, ipAddress, hostName, timeoutMinutes, DateTime.Now);
            dbHelper.ExecuteNonQuery(command);
            TimeSpan refreshInterval = new TimeSpan(0, 0, (int)(SessionSettings.GetServiceSetting().RefreshInterval * 60));
            TimeSpan timeoutInterval = new TimeSpan(0, 0, (SessionSettings.GetServiceSetting().SessionTimeout * 60));
            return new SessionData { SessionID = sessionId, RefreshInterval = refreshInterval, SessionTimeoutInterval = timeoutInterval };

        }

        public DataRow GetLogInfo(string userName, string ipAddress, string hostName, out SessionData sessionData)
        {
            //Not use this.Helper to avoid to insert log, because the method will be called by UserNameExtractionCallHanlders
            DbHelper dbHelper = new DbHelper();
            DataTable table = new DataTable();

            string sessionID = AppContext.Current.SessionID;
            if (string.IsNullOrEmpty(sessionID))
            {
                sessionID = Guid.NewGuid().ToString();
            }


            int timeoutMinutes = SessionSettings.GetServiceSetting().SessionTimeout;
            dbHelper.Fill(table, "P_IC_USER_INFO_FOR_LOGIN", userName, sessionID, DateTime.Now,
               DateTime.Now, ipAddress, hostName, timeoutMinutes, DateTime.Now);

            TimeSpan refreshInterval = new TimeSpan(0, 0, (int)(SessionSettings.GetServiceSetting().RefreshInterval * 60));
            TimeSpan timeoutInterval = new TimeSpan(0, 0, (SessionSettings.GetServiceSetting().SessionTimeout * 60));
            sessionData = new SessionData { SessionID = sessionID, RefreshInterval = refreshInterval, SessionTimeoutInterval = timeoutInterval };

            return (table.Rows.Count == 0) ? null : table.Rows[0];
        }

    }
}

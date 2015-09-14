#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Library
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Yang Jian Hua
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

using HiiP.Framework.Common.ApplicationContexts;

using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using HiiP.Framework.Common.ApplicationContexts.Configuration;

namespace HiiP.Framework.Logging.Library
{
    public static class Utility
    {
        #region Logging Utility

        public static Guid SetContextValues()
        {
            Guid id = Guid.NewGuid();
            AppContext.Current.ActivityID = id.ToString();
            if (ApplicationContextSettings.GetContextDomainScope() == ContextDomainScope.Client)
            {
                AppContext.SetToCallContext(AppContext.Current.ToDictionary());
            }
            return id;
        }

        private static string _connectionStringName;
        public static string GetLoggingConnectionStringName()
        {
            if (!string.IsNullOrEmpty(_connectionStringName))
            {
                return _connectionStringName;
            }
            object settings = ConfigurationManager.GetSection("loggingConfiguration");
            if (settings != null)
            {
                TraceListenerData data = (((LoggingSettings)settings).TraceListeners.Get("SqlServerDatabase"));
                if (data != null)
                {
                    _connectionStringName = ((MonitoringDatabaseTraceListenerData)data).ConnectionStringName;
                }
            }
            return _connectionStringName;
        }

        public static void TraceToEventLog(string message)
        {
            Logger.Write(message, "EventLog");
        }

        #endregion
    }
}
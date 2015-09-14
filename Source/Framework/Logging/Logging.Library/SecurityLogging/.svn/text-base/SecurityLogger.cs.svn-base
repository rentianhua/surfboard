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
using System.Text;
using HiiP.Framework.Common.ApplicationContexts;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace HiiP.Framework.Logging.Library
{
    public class SecurityLogger
    {
        public static void WriteLog(SecurityExceptionType type, string message)
        {
            MonitoringLogEntry logEntry = new MonitoringLogEntry();
            logEntry.Categories.Add(type.ToString());
            logEntry.ExtendedActivityId = AppContext.Current.ActivityID;
            logEntry.ModuleId = AppContext.Current.ModuleID;
            logEntry.FunctionId = AppContext.Current.FunctionID;
            logEntry.UserName = AppContext.Current.UserName;
            logEntry.UserID = AppContext.Current.UserID;
            logEntry.IpAddress = AppContext.Current.IPAddress;
            logEntry.UserRoles = AppContext.Current.UserRoles;
            logEntry.UserGraphicArea = AppContext.Current.GraphicArea;
            //logEntry.Organization = AppContext.Current.Organization;
            logEntry.Office = AppContext.Current.Office;
            logEntry.Message = message;
            logEntry.EventId = 0;
            logEntry.Priority = -1;
            logEntry.Severity = TraceEventType.Warning;
            logEntry.Title = type.ToString() + " Error";
            Logger.Write(logEntry);
        }

        public static void WriteLogWhenLoginSuccessful(long? startTrick, long? endTrick, long elapsedMilliseconds)
        {
            MonitoringPopulateLogEntry utility = new MonitoringPopulateLogEntry();

            Guid id = Guid.NewGuid();

            MonitoringLogEntry entry = new MonitoringLogEntry();
            entry.TracingStartTicks = startTrick;
            entry.TracingEndTicks = endTrick;
            decimal secondsElapsed = utility.GetSecondsElapsed(elapsedMilliseconds);
            entry.SecondsElapsed = secondsElapsed;

            entry.MethodName = "HiiP.Framework.Security.AccessControl.Authentication.Authenticate";
            entry.ParameterValues = "void";

            entry.ExtendedActivityId = id.ToString();
            entry.UserID = AppContext.Current.UserID;
            entry.UserName = AppContext.Current.UserName;
            entry.UserRoles = AppContext.Current.UserRoles;
            entry.UserGraphicArea = AppContext.Current.GraphicArea;
            //entry.Organization = AppContext.Current.Organization;
            entry.Office = AppContext.Current.Office;
            entry.IpAddress =AppContext.Current.IPAddress;
            entry.Categories.Add("Trace");
            entry.Title = "Login Successful";
            entry.Message = "Login Successful";
            entry.ModuleId = "Login";
            entry.FunctionId = "Login";
            entry.Severity = TraceEventType.Information;
            entry.Component = ComponentType.BusinessService;

            if (Logger.ShouldLog(entry))
            {
                Logger.Write(entry);
            }
        }
    }
}

#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Interface
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

namespace HiiP.Framework.Logging.Interface.Constants
{
    public static class FunctionNames //: HiiP.Infrastructure.Interface.Constants.FunctionNames
    {
        /// <summary>
        /// Key for System Administration menu
        /// </summary>
        private const string AdminModuleID = "SystemAdmin";

        public const string LoggingModuleID = AdminModuleID + ".LoggingView";
        public const string LoggingModuleName = "Logging View";
        public const string LoggingModuleScreenID = "LV000";

        public const string InstrumentationFunctionID = LoggingModuleID + ".Instrumentation";
        public const string InstrumentationFunctionName = "View instrumentation logs";
        public const string InstrumentationFunctionScreenID = "FW-LI-001";

        public const string InstrumentationDetailViewFunctionID = LoggingModuleID + ".InstrumentationDetailView";
        public const string InstrumentationDetailViewFunctionName = "View instrumentation log details";
        public const string InstrumentationDetailViewFunctionScreenID = "FW-LI-002";

        public const string MonitoringFunctionID = LoggingModuleID + ".Monitoring";
        public const string MonitoringFunctionName = "View performance monitoring logs";
        public const string MonitoringFunctionScreenID = "FW-LP-001";

        public const string UsageFunctionID = LoggingModuleID + ".Usage";
        public const string UsageFunctionName = "View usage logs";
        public const string UsageFunctionScreenID = "FW-LU-001";



        public const string ExceptionLogModuleID = AdminModuleID + ".ExceptionHandling";
        public const string ExceptionLogModuleName = "ExceptionHandlingView";
        public const string ExceptionLogModuleScreenID = "";

        public const string ExceptionLogViewFunctionID = ExceptionLogModuleID + ".LogView";
        public const string ExceptionLogViewFunctionName = "View exception logs";
        public const string ExceptionLogViewScreenID = "FW-EXM-001";

        public const string ExceptionLogMessageIDViewFunctionID = ExceptionLogModuleID + ".LogMessgeIDView";

        public const string ExceptionDetailViewFunctionID = ExceptionLogModuleID + ".LogDetailView";
        public const string ExceptionDetailViewFunctionName = "View exception log details";
        public const string ExceptionDetailViewScreenID = "FW-EXM-002";

        // Audit Log View
        public const string AuditLogModuleID = AdminModuleID + ".AuditTrail";
        public const string AuditLogModuleName = "AuditLogView";
        public const string AuditLogModuleScreenID = "";

        public const string AuditLogViewFunctionID = AuditLogModuleID + ".LogView";
        public const string AuditLogViewFunctionName = "View audit logs";
        public const string AuditLogViewScreenID = "FW-ALM-001";

        public const string AuditLogDetailViewFunctionID = AuditLogModuleID + ".LogDetailView";
        public const string AuditLogDetailViewFunctionName = "View audit log";
        public const string AuditLogDetailViewScreenID = "FW-ALM-002";

    }
}

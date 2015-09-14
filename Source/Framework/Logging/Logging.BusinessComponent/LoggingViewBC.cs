#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Business Component
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

using HiiP.Framework.Common.ApplicationContexts.CallHandlers;
using HiiP.Framework.Common;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.DataAccess;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using HiiP.Framework.Logging.Library;

namespace HiiP.Framework.Logging.BusinessComponent
{
    public class LoggingViewBC : HiiPBusinessComponentBase
    {
        private readonly LoggingViewDA _loggingViewDA = InstanceBuilder.CreateInstance<LoggingViewDA>();

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.InstrumentationFunctionID)]
        public LogIDPairEntity GetInstrumentationLogIDRangeByLogTime(DateTimeCompare timeEntity)
        {
            return _loggingViewDA.GetInstrumentationLogIDRangeByLogTime(timeEntity);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.InstrumentationFunctionID)]
        public LoggingViewDataSet RetrieveInstrumentation(LogIDPairEntity logIDPair, DateTimeCompare timeEntity,string userName, string ipAddress, string moduleId, string functionId, string componentName, string category, string pcName)
        {
            return _loggingViewDA.RetrieveInstrumentation(logIDPair,timeEntity, userName, ipAddress, moduleId, functionId, componentName, category, pcName);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.ExceptionLogModuleID, FunctionID = FunctionNames.ExceptionLogViewFunctionID)]
        public LogIDPairEntity GetLogIDRangeByLogTime(DateTimeCompare timeEntity, string userName, string machineName)
        {
            return _loggingViewDA.GetLogIDRangeByLogTime(timeEntity, userName, machineName);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.ExceptionLogModuleID, FunctionID = FunctionNames.ExceptionLogViewFunctionID)]
        public LoggingViewDataSet RetrieveExceptionLog(LogIDPairEntity logIDPair, DateTimeCompare timeEntity, string userName, string category, string severity, string machineName, string logContent, string instanceID)
        {
            return _loggingViewDA.RetrieveExceptionLog(logIDPair, timeEntity, userName, category, severity, machineName, logContent, instanceID);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.ExceptionLogModuleID, FunctionID = FunctionNames.ExceptionLogViewFunctionID)]
        public LoggingViewDataSet GetLogsByID(string logId)
        {
            return _loggingViewDA.GetLogsByID(logId);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.MonitoringFunctionID)]
        public LoggingViewDataSet RetrievePerformanceInformation(DateTimeCompare timeEntity, string functionId, string componentName, string userName)
        {
            return _loggingViewDA.RetrievePerformanceInformation(timeEntity, functionId, componentName, userName);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForUser(DateTimeCompare timeEntity, string userName)
        {
            return _loggingViewDA.RetrieveUsagesForUser(timeEntity, userName);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForRole(DateTimeCompare timeEntity, string roleId)
        {
            return _loggingViewDA.RetrieveUsagesForRole(timeEntity, roleId);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForOrganization(DateTimeCompare timeEntity, string organization)
        {
            return _loggingViewDA.RetrieveUsagesForOrganization(timeEntity, organization);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForOffice(DateTimeCompare timeEntity, string office)
        {
            return _loggingViewDA.RetrieveUsagesForOffice(timeEntity, office);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForGraphicArea(DateTimeCompare timeEntity, string geographicArea)
        {
            return _loggingViewDA.RetrieveUsagesForGraphicArea(timeEntity, geographicArea);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForModule(DateTimeCompare timeEntity, string moduleId)
        {
            return _loggingViewDA.RetrieveUsagesForModule(timeEntity, moduleId);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForFunction(DateTimeCompare timeEntity, string functionId)
        {
            return _loggingViewDA.RetrieveUsagesForFunction(timeEntity, functionId);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForUsersCount(DateTimeCompare timeEntity)
        {
            return _loggingViewDA.RetrieveUsagesForUsersCount(timeEntity);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForUsersCountByModule(DateTimeCompare timeEntity, string moduleId)
        {
            return _loggingViewDA.RetrieveUsagesForUsersCountByModule(timeEntity, moduleId);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.ExceptionLogMessageIDViewFunctionID)]
        public string GetExceptionMessageID(string userName, string ipAddress)
        {
            return _loggingViewDA.GetExceptionMessageID(userName, ipAddress);
        }
    }
}

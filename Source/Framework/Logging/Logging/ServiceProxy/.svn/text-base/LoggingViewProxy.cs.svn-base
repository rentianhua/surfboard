#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging
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
using HiiP.Framework.Logging.Interface.Services;
using HiiP.Framework.Common;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Common.ApplicationContexts.CallHandlers;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Logging.ServiceProxy
{
    public class LoggingViewProxy : ServiceProxyBase<ILoggingViewService>, ILoggingViewService
    {
        protected LoggingViewProxy(string endPointName)
            : base(endPointName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public LoggingViewProxy()
        {
            base.WrapObject(new LoggingViewProxy(EndpointNames.LoggingViewEndpoint));
        }

        #region ILoggingViewService Members

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.InstrumentationFunctionID)]
        public LogIDPairEntity GetInstrumentationLogIDRangeByLogTime(DateTimeCompare timeEntity)
        {
            return Proxy.GetInstrumentationLogIDRangeByLogTime(timeEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logIDPair"></param>
        /// <param name="timeEntity"></param>
        /// <param name="userName"></param>
        /// <param name="ipAddress"></param>
        /// <param name="moduleId"></param>
        /// <param name="functionId"></param>
        /// <param name="componentName"></param>
        /// <param name="category"></param>
        /// <param name="pcName"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.InstrumentationFunctionID)]
        public LoggingViewDataSet RetrieveInstrumentation(LogIDPairEntity logIDPair, DateTimeCompare timeEntity,string userName, string ipAddress, string moduleId, string functionId, string componentName, string category, string pcName)
        {
            return Proxy.RetrieveInstrumentation(logIDPair, timeEntity,userName,ipAddress, moduleId, functionId, componentName, category, pcName);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.ExceptionLogModuleID, FunctionID = FunctionNames.ExceptionLogViewFunctionID)]
        public LogIDPairEntity GetLogIDRangeByLogTime(DateTimeCompare timeEntity, string userName, string machineName)
        {
            return Proxy.GetLogIDRangeByLogTime(timeEntity, userName, machineName);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.ExceptionLogModuleID, FunctionID = FunctionNames.ExceptionLogViewFunctionID)]
        public LoggingViewDataSet RetrieveExceptionLog(LogIDPairEntity logIDPair, DateTimeCompare timeEntity, string userName, string category, string severity, string machineName, string logContent, string instanceID)
        {
            return Proxy.RetrieveExceptionLog(logIDPair, timeEntity, userName, category, severity, machineName, logContent, instanceID);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.ExceptionLogModuleID, FunctionID = FunctionNames.ExceptionLogViewFunctionID)]
        public LoggingViewDataSet GetLogsByID(string logId)
        {
            return Proxy.GetLogsByID(logId);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.MonitoringFunctionID)]
        public LoggingViewDataSet RetrievePerformanceInformation(DateTimeCompare timeEntity, string functionId, string componentName, string userName)
        {
            return Proxy.RetrievePerformanceInformation(timeEntity, functionId, componentName, userName);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForUser(DateTimeCompare timeEntity, string userName)
        {
            return Proxy.RetrieveUsagesForUser(timeEntity, userName);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForRole(DateTimeCompare timeEntity, string userRoles)
        {
            return Proxy.RetrieveUsagesForRole(timeEntity, userRoles);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForOrganization(DateTimeCompare timeEntity, string organization)
        {
            return Proxy.RetrieveUsagesForOrganization(timeEntity, organization);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForOffice(DateTimeCompare timeEntity, string office)
        {
            return Proxy.RetrieveUsagesForOffice(timeEntity, office);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForGraphicArea(DateTimeCompare timeEntity, string geographicArea)
        {
            return Proxy.RetrieveUsagesForGraphicArea(timeEntity, geographicArea);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForModule(DateTimeCompare timeEntity, string moduleId)
        {
            return Proxy.RetrieveUsagesForModule(timeEntity, moduleId);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForFunction(DateTimeCompare timeEntity, string functionId)
        {
            return Proxy.RetrieveUsagesForFunction(timeEntity, functionId);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForUsersCount(DateTimeCompare timeEntity)
        {
            return Proxy.RetrieveUsagesForUsersCount(timeEntity);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForUsersCountByModule(DateTimeCompare timeEntity, string moduleId)
        {
            return Proxy.RetrieveUsagesForUsersCountByModule(timeEntity, moduleId);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.ExceptionLogMessageIDViewFunctionID)]
        public string GetExceptionMessageID(string userName, string ipAddress)
        {
            return Proxy.GetExceptionMessageID(userName, ipAddress);
        }
        #endregion

    }
}

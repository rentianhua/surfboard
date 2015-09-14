#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Data Access
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Yang Jian Hua
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using HiiP.Framework.Common;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using HiiP.Framework.Logging.Library;

namespace HiiP.Framework.Logging.DataAccess
{
    public class LoggingViewDA : HiiPDataAccessBase
    {
        // Definition of constant variables
        private const int MaxRecords = 100;
        private const int MaxNullFounds = 10;

        public LoggingViewDA()
            : base(ConnectionStringNames.HiiPLogging)
        {

        }

        private LogIDPairEntity GetIDRangeByLogTime(DateTimeCompare timeEntity, string spName)
        {
            DataTable dt = new DataTable();
            DbCommand command = Helper.BuildDbCommand(spName);
            Helper.AssignParameterValues(
                command,
                timeEntity.StartTime,
                timeEntity.EndTime
                );
            Helper.Fill(dt, command);

            long minLogID = (long)(Helper.GetParameterValue(command, "p_min_log_id"));
            long maxLogID = (long)(Helper.GetParameterValue(command, "p_max_log_id"));

            //LogIDPairEntity logIDPair = new LogIDPairEntity();
            //long logID;
            //if (dt.Rows.Count > 0 && dt.Rows.Count <= 1)
            //{
            //    logID = long.Parse(dt.Rows[0][0].ToString());
            //    logIDPair = new LogIDPairEntity(logID, logID);
            //}
            //if (dt.Rows.Count > 1)
            //{
            //    List<long> logIDCollection = new List<long>();
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        logID = long.Parse(row[0].ToString());
            //        logIDCollection.Add(logID);
            //    }
            //    logIDPair = new LogIDPairEntity(logIDCollection.Min<long>(), logIDCollection.Max<long>());
            //}

            //return logIDPair;

            return new LogIDPairEntity(minLogID, maxLogID);
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.InstrumentationFunctionID)]
        public LogIDPairEntity GetInstrumentationLogIDRangeByLogTime(DateTimeCompare timeEntity)
        {
            return GetIDRangeByLogTime(timeEntity, "P_IC_LOGGING_INSTRUMENTATION_LOG_ID_RANGE");
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.InstrumentationFunctionID)]
        public LoggingViewDataSet RetrieveInstrumentation(LogIDPairEntity logIDPair, DateTimeCompare timeEntity,string userName, string ipAddress, string moduleId, string functionId, string componentName, string category, string pcName)
        {
            long minLogID = logIDPair.MinLogID;
            long maxLogID = logIDPair.MaxLogID;
            long currentMinLogID = maxLogID - MaxRecords;

            LoggingViewDataSet logDs;
            LoggingViewDataSet logDsByFilter = new LoggingViewDataSet();
            DbCommand command;
            int recordCount = 0;
            int computeCount = 0;

            while (recordCount == 0 && minLogID <= maxLogID && computeCount < MaxNullFounds)
            {
                command = Helper.BuildDbCommand("dbo.P_IC_LOGGING_INSTRUMENTATION_S");
                command.CommandTimeout = 0;
                if (currentMinLogID < minLogID)
                {
                    currentMinLogID = minLogID;
                }

                logDs = new LoggingViewDataSet();

                Helper.AssignParameterValues(command, currentMinLogID, maxLogID);
                Helper.Fill(logDs.T_IC_LOGGING_LOG, command);

                var list = (from n in logDs.T_IC_LOGGING_LOG
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                            where (string.IsNullOrEmpty(category) || n.CATEGORY_NAME.Equals(category))
                            && (string.IsNullOrEmpty(componentName) || n.COMPONENT.Equals(ConvertToString(componentName)))
                            && (!string.IsNullOrEmpty(userName) ? SearchHelper.IsRegexMatch(ConvertToString(n.USER_NAME), userName, @"[\w|\W]*") : true)
                            && (!string.IsNullOrEmpty(ipAddress) ? SearchHelper.IsRegexMatch(ConvertToString(n.IP_ADDRESS), ipAddress, @"[\w|\W]*") : true)
                            && (!string.IsNullOrEmpty(moduleId) ? SearchHelper.IsRegexMatch(ConvertToString(n.MODULE_ID), moduleId, @"[\w|\W]*") : true)
                            && (!string.IsNullOrEmpty(functionId) ? SearchHelper.IsRegexMatch(ConvertToString(n.FUNCTION_ID), functionId, @"[\w|\W]*") : true)
                            && (!string.IsNullOrEmpty(pcName) ? SearchHelper.IsRegexMatch(ConvertToString(n.MACHINE_NAME), pcName, @"[\w|\W]*") : true)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                            && (timeEntity == null ? true : n.LOG_TIME <= timeEntity.EndTime)
                            && (timeEntity == null ? true : n.LOG_TIME >= timeEntity.StartTime)
                            select n);

                maxLogID = currentMinLogID - 1;
                currentMinLogID = maxLogID - MaxRecords;
                recordCount += list.Count();
                computeCount += 1;

                foreach (var row in list)
                {
                    logDsByFilter.T_IC_LOGGING_LOG.ImportRow(row);
                }
                logDs.Dispose();
            }

            logDsByFilter.ExtendedProperties.Add("LogIDPair", minLogID.ToString() + "," + maxLogID.ToString());

            logDsByFilter.AcceptChanges();

            return logDsByFilter;
        }

        private static string ConvertToString(object value)
        {
            return (DBNull.Value == value || null == value) ? "" : value.ToString();
        }
        #region

        private static string TranslateWildcardForExceptionLog(string criteria)
        {
            //Changed for MQC 9338
            /*
             * 1) Tune the search logic to better handles huge number of records in the database table.
Currently, MachineName, UserID, etc. are not used in the T-SQL "where" clause so as to avoid timeout issue encountered in the wildcard search especially when the search text starts with asterisk *.
We will now include these 2 fields in the "where" clause if they do NOT start with * and hence reducing the number of records which the app server processes.  Additional indices will be created for these 2 fields.

            */
            string temporaryInput = (criteria ?? "").Trim();
            if (temporaryInput.StartsWith("*"))
            {
                temporaryInput = null;
            }

            temporaryInput = SearchHelper.TranslateWildcard(temporaryInput);
            return string.IsNullOrEmpty(temporaryInput) ? null : temporaryInput;
        }

        // #######################  Get LogID range  #######################
        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.ExceptionLogModuleID, FunctionID = FunctionNames.ExceptionLogViewFunctionID)]
        public LogIDPairEntity GetLogIDRangeByLogTime(DateTimeCompare timeEntity, string userName, string machineName)
        {
            string temporaryUserNameInput = TranslateWildcardForExceptionLog(userName);

            string temporaryMachineameInput = TranslateWildcardForExceptionLog(machineName);

            DbCommand command = Helper.BuildDbCommand("P_IC_LOGGING_EXCEPTION_LOG_ID_RANGE");
            Helper.AssignParameterValues(
                command,
                timeEntity.StartTime,
                timeEntity.EndTime,
                temporaryUserNameInput,
                temporaryMachineameInput
                );
            Helper.ExecuteScalar(command);

            long minLogID = (long)(Helper.GetParameterValue(command, "p_min_log_id"));
            long maxLogID = (long)(Helper.GetParameterValue(command, "p_max_log_id"));


            return new LogIDPairEntity(minLogID, maxLogID);
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.ExceptionLogModuleID, FunctionID = FunctionNames.ExceptionLogViewFunctionID)]
        public LoggingViewDataSet RetrieveExceptionLog(LogIDPairEntity logIDPair, DateTimeCompare timeEntity, string userName, string category, string severity, string machineName, string logContent, string instanceID)
        {
            const int MaxErrorLogNumber = 1000;
            if (!string.IsNullOrEmpty(instanceID))
            {
                LoggingViewDataSet logDs = new LoggingViewDataSet();
                DbCommand command;
                command = Helper.BuildDbCommand("dbo.P_IC_LOGGING_EXCEPTION_LOG_S_BY_ID");
                Helper.AssignParameterValues(command, instanceID);
                Helper.Fill(logDs.T_IC_LOGGING_LOG, command);
                return logDs;
            }
            else
            {
                long minLogID = logIDPair.MinLogID;
                long maxLogID = logIDPair.MaxLogID;
                long currentMinLogID = maxLogID - MaxErrorLogNumber;

                LoggingViewDataSet logDs;
                LoggingViewDataSet logDsByFilter = new LoggingViewDataSet();
                DbCommand command;
                int recordCount = 0;
                int computeCount = 0;

                string temporaryUserNameInput = TranslateWildcardForExceptionLog(userName);

                string temporaryMachineameInput = TranslateWildcardForExceptionLog(machineName);


                while (recordCount == 0 && minLogID <= maxLogID && computeCount < MaxNullFounds)
                {
                    command = Helper.BuildDbCommand("dbo.P_IC_LOGGING_EXCEPTION_LOG_S");
                    command.CommandTimeout = 0;
                    if (currentMinLogID < minLogID)
                    {
                        currentMinLogID = minLogID;
                    }

                    logDs = new LoggingViewDataSet();
                    Helper.AssignParameterValues(command, currentMinLogID, maxLogID, temporaryUserNameInput, temporaryMachineameInput);
                    Helper.Fill(logDs.T_IC_LOGGING_LOG, command);

                    var list = (from n in logDs.T_IC_LOGGING_LOG
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                                where (string.IsNullOrEmpty(severity) || ConvertToString(n.SEVERITY).Equals(severity))
                                && (!string.IsNullOrEmpty(userName) ? SearchHelper.IsRegexMatch(ConvertToString(n.USER_NAME), userName, @"[\w|\W]*") : true)
                                && (string.IsNullOrEmpty(category) || ConvertToString(n.CATEGORY_NAME).Equals(category))
                                && (!string.IsNullOrEmpty(machineName) ? SearchHelper.IsRegexMatch(ConvertToString(n.MACHINE_NAME), machineName, @"[\w|\W]*") : true)
                                && (!string.IsNullOrEmpty(logContent) ? SearchHelper.IsRegexMatch(ConvertToString(n.FORMATTED_MESSAGE), logContent, @"[\w|\W]*") : true)
                                && (string.IsNullOrEmpty(instanceID) || ConvertToString(n.INSTANCE_ID).Equals(instanceID))
                                && (timeEntity == null ? true : n.LOG_TIME <= timeEntity.EndTime)
                                && (timeEntity == null ? true : n.LOG_TIME >= timeEntity.StartTime)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                                select n);

                    maxLogID = currentMinLogID - 1;
                    currentMinLogID = maxLogID - MaxErrorLogNumber;
                    recordCount += list.Count();
                    computeCount += 1;

                    foreach (var row in list)
                    {
                        logDsByFilter.T_IC_LOGGING_LOG.ImportRow(row);
                    }

                    logDs.Dispose();
                }

                logDsByFilter.ExtendedProperties.Add("LogIDPair", minLogID.ToString() + "," + maxLogID.ToString());

                logDsByFilter.AcceptChanges();

                return logDsByFilter;
            }
        }

        #endregion

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.ExceptionLogModuleID, FunctionID = FunctionNames.ExceptionLogViewFunctionID)]
        public LoggingViewDataSet GetLogsByID(string logId)
        {
            LoggingViewDataSet logDs = new LoggingViewDataSet();
            Helper.Fill(logDs.T_IC_LOGGING_LOG, "dbo.P_IC_LOGGING_SEARCH_BY_ID", logId);
            return logDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.MonitoringFunctionID)]
        public LoggingViewDataSet RetrievePerformanceInformation(DateTimeCompare timeEntity, string functionId, string componentName, string userName)
        {
            LoggingViewDataSet logDs = new LoggingViewDataSet();
            Helper.Fill(logDs.T_IC_PERFORMANCE_LOG, "dbo.P_IC_LOGGING_PERFORMANCE_S", timeEntity.StartTime, timeEntity.EndTime, functionId, componentName, userName);
            return logDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForUser(DateTimeCompare timeEntity, string userName)
        {
            LoggingUsageDataSet usageDs = new LoggingUsageDataSet();
            Helper.Fill(usageDs.T_IC_LOGGING_USAGE, "dbo.P_IC_LOGGING_USAGE_BY_USER", timeEntity.StartTime, timeEntity.EndTime, userName);
            return usageDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForRole(DateTimeCompare timeEntity, string roleId)
        {
            LoggingUsageDataSet usageDs = new LoggingUsageDataSet();
            Helper.Fill(usageDs.T_IC_LOGGING_USAGE, "dbo.P_IC_LOGGING_USAGE_BY_ROLE", timeEntity.StartTime, timeEntity.EndTime, roleId);
            return usageDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForOrganization(DateTimeCompare timeEntity, string organization)
        {
            LoggingUsageDataSet usageDs = new LoggingUsageDataSet();
            Helper.Fill(usageDs.T_IC_LOGGING_USAGE, "dbo.P_IC_LOGGING_USAGE_BY_ORGANIZATION", timeEntity.StartTime, timeEntity.EndTime, organization);
            return usageDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForOffice(DateTimeCompare timeEntity, string office)
        {
            LoggingUsageDataSet usageDs = new LoggingUsageDataSet();
            Helper.Fill(usageDs.T_IC_LOGGING_USAGE, "dbo.P_IC_LOGGING_USAGE_BY_OFFICE", timeEntity.StartTime, timeEntity.EndTime, office);
            return usageDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForGraphicArea(DateTimeCompare timeEntity, string geographicArea)
        {
            LoggingUsageDataSet usageDs = new LoggingUsageDataSet();
            Helper.Fill(usageDs.T_IC_LOGGING_USAGE, "dbo.P_IC_LOGGING_USAGE_BY_AREA", timeEntity.StartTime, timeEntity.EndTime, geographicArea);
            return usageDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForModule(DateTimeCompare timeEntity, string moduleId)
        {
            LoggingUsageDataSet usageDs = new LoggingUsageDataSet();
            Helper.Fill(usageDs.T_IC_LOGGING_USAGE, "dbo.P_IC_LOGGING_USAGE_BY_MODULE", timeEntity.StartTime, timeEntity.EndTime, moduleId);
            return usageDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForFunction(DateTimeCompare timeEntity, string functionId)
        {
            LoggingUsageDataSet usageDs = new LoggingUsageDataSet();
            Helper.Fill(usageDs.T_IC_LOGGING_USAGE, "dbo.P_IC_LOGGING_USAGE_BY_FUNCTION", timeEntity.StartTime, timeEntity.EndTime, functionId);
            return usageDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForUsersCount(DateTimeCompare timeEntity)
        {
            LoggingUsageDataSet usageDs = new LoggingUsageDataSet();
            Helper.Fill(usageDs.T_IC_LOGGING_USAGE, "dbo.P_IC_LOGGING_COUNT_BY_TIME", timeEntity.StartTime, timeEntity.EndTime);
            return usageDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.UsageFunctionID)]
        public LoggingUsageDataSet RetrieveUsagesForUsersCountByModule(DateTimeCompare timeEntity, string moduleId)
        {
            LoggingUsageDataSet usageDs = new LoggingUsageDataSet();
            Helper.Fill(usageDs.T_IC_LOGGING_USAGE, "dbo.P_IC_LOGGING_COUNT_BY_MODULE", timeEntity.StartTime, timeEntity.EndTime, moduleId);
            return usageDs;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.LoggingModuleID, FunctionID = FunctionNames.ExceptionLogMessageIDViewFunctionID)]
        public string GetExceptionMessageID(string userName, string ipAddress)
        {
            DbCommand command = Helper.BuildDbCommand("dbo.P_IC_EXCEPTION_LOG_SEARCH_INSTANCE_ID");
            Helper.AssignParameterValues(
                command,
                userName,
                ipAddress
                );
            object result = Helper.ExecuteScalar(command);
            if (result != null && result != DBNull.Value)
            {
                return (string)result;
            }
            
            return "";

        }

    }
}

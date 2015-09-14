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
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NCS.IConnect.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System.Diagnostics;
using System.Transactions;
using System.Data;

namespace HiiP.Framework.Logging.Library
{
    [ConfigurationElementType(typeof(MonitoringDatabaseTraceListenerData))]
    public class MonitoringDatabaseTraceListener : FormattedTraceListenerBase
    {
        #region Variable
        private const string AddCategoryStoredProcName = "P_IC_LOGGING_CATEGORY_I";

        private const string WriteLogStoredProcName = "P_IC_LOGGING_LOG_I";

        private string _applicationName;

        private string _connectionStringName;

        private Database _database;
        #endregion

        #region Construction
        /// <summary>
        /// Initializes a new instance of <see cref="MonitoringDatabaseTraceListener"/>.
        /// </summary>
        /// <param name="database">The database for writing the log.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public MonitoringDatabaseTraceListener(Database database, ILogFormatter formatter, string applicationName,
                                              string connectionStringName)
            : base(formatter)
        {
            _database = database;
            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException("applicationName");
            }
            _applicationName = applicationName;
            _connectionStringName = connectionStringName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the name of the connection string.
        /// </summary>
        /// <value>The name of the connection string.</value>
        public string ConnectionStringName
        {
            get
            {
                return _connectionStringName;
            }
        }
        #endregion

        #region Public method
        /// <summary>
        /// Writes a predefined messgae.
        /// </summary>
        /// <param name="message">The log message.</param>
        public override void Write(string message)
        {
            MonitoringLogEntry logEntry = new MonitoringLogEntry();
            logEntry.EventId = 0;
            logEntry.Priority = 5;
            logEntry.Severity = TraceEventType.Information;
            logEntry.Title = string.Empty;
            logEntry.TimeStamp = DateTime.UtcNow;
            logEntry.Message = message;

            ExecuteWriteLogStoredProcedure(logEntry, _database);
        }

        /// <summary>
        /// Writes a message with a line terminator.
        /// </summary>
        /// <param name="message">The log message.</param>
        public override void WriteLine(string message)
        {
            Write(message);
        }

        /// <summary>
        /// Delivers the trace data to the underlying database.
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
                                       object data)
        {
            LogEntry entry = data as LogEntry;
            if (entry == null)
            {
                string da = data as string;
                if (da != null)
                {
                    Write(da);
                    return;
                }
                base.TraceData(eventCache, source, eventType, id, data);
                return;
            }
            if (ValidateParameters(entry))
            {
                ExecuteStoredProcedure(entry);
            }
            InstrumentationProvider.FireTraceListenerEntryWrittenEvent();
        }

        #endregion

        #region Protected method
        /// <summary>
        /// Declare the supported attributes for <see cref="MonitoringDatabaseTraceListener"/>
        /// </summary>
        protected override string[] GetSupportedAttributes()
        {
            return new [] { "formatter", "connectionStringName", "applicationName" };
        }

        #endregion

        #region Private method

        /// <summary>
        /// Validates that enough information exists to attempt executing the stored procedures
        /// </summary>
        /// <param name="entry">The LogEntry to validate.</param>
        /// <returns>A boolean indicating whether the parameters for the LogEntry configuration are valid.</returns>
        private static bool ValidateParameters(LogEntry entry)
        {
            bool valid = true;
            if (entry.Categories == null)
            {
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Executes the stored procedures
        /// </summary>
        /// <param name="logEntry">The LogEntry to store in the database</param>
        private void ExecuteStoredProcedure(LogEntry logEntry)
        {
            try
            {
                if (System.Transactions.Transaction.Current == null)
                {
                    Execute(logEntry);
                }
                else
                {

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        Execute(logEntry);
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleCommonDbException(ex);
                throw;
            }

        }


        private void Execute(LogEntry logEntry)
        {
            int logID ;
            MonitoringLogEntry entry = logEntry as MonitoringLogEntry;

            if (entry == null)
            {
                logID = ExecuteWriteLogStoredProcedureForLogEntry(logEntry, _database);
            }
            else
            {
                logID = ExecuteWriteLogStoredProcedure(entry, _database);
            }
            ExecuteAddCategoryStoredProcedure(logEntry, logID, _database);
        }


        /// <summary>
        /// Executes the WriteLog stored procedure
        /// </summary>
        /// <param name="logEntry">The MyLogEntry to store in the database.</param>
        /// <param name="database">The database.</param>
        /// <returns>An integer for the MyLogEntry Id</returns>
        private int ExecuteWriteLogStoredProcedure(MonitoringLogEntry logEntry, Database database)
        {
            try
            {
                DbCommand commmand = database.GetStoredProcCommand(WriteLogStoredProcName);

                database.AddInParameter(commmand, "p_app_name", DbType.String, _applicationName);
                database.AddInParameter(commmand, "p_event_id", DbType.Int32, logEntry.EventId);
                database.AddInParameter(commmand, "p_priority", DbType.Int32, logEntry.Priority);
                database.AddInParameter(commmand, "p_severity", DbType.String, logEntry.Severity.ToString());
                database.AddInParameter(commmand, "p_title", DbType.String, logEntry.Title);
                database.AddInParameter(commmand, "p_log_time", DbType.DateTime, logEntry.TimeStamp.ToLocalTime());
                database.AddInParameter(commmand, "p_machine_name", DbType.String, logEntry.MachineName);
                database.AddInParameter(commmand, "p_app_domain_name", DbType.String, logEntry.AppDomainName);
                database.AddInParameter(commmand, "p_process_id", DbType.String, logEntry.ProcessId);
                database.AddInParameter(commmand, "p_process_name", DbType.String, logEntry.ProcessName);
                database.AddInParameter(commmand, "p_thread_name", DbType.String, logEntry.ManagedThreadName);
                database.AddInParameter(commmand, "p_win32_thread_id", DbType.String, logEntry.Win32ThreadId);
                database.AddInParameter(commmand, "p_message", DbType.String, logEntry.Message);
                database.AddInParameter(commmand, "p_method_name", DbType.String, logEntry.MethodName);
                database.AddInParameter(commmand, "p_activity_id", DbType.String, logEntry.ExtendedActivityId);
                database.AddInParameter(commmand, "p_tracing_start_ticks", DbType.String, logEntry.TracingStartTicks);
                database.AddInParameter(commmand, "p_tracing_end_ticks", DbType.String, logEntry.TracingEndTicks);
                database.AddInParameter(commmand, "p_seconds_elapsed", DbType.Decimal, logEntry.SecondsElapsed);
                database.AddInParameter(commmand, "p_user_name", DbType.String, logEntry.UserName);
                database.AddInParameter(commmand, "p_ip_address", DbType.String, logEntry.IpAddress);
                database.AddInParameter(commmand, "p_user_roles", DbType.String, logEntry.UserRoles);
                database.AddInParameter(commmand, "p_user_graphic_area", DbType.String, logEntry.UserGraphicArea);
                database.AddInParameter(commmand, "p_organization", DbType.String, null);
                database.AddInParameter(commmand, "p_office", DbType.String, logEntry.Office);
                database.AddInParameter(commmand, "p_module_id", DbType.String, logEntry.ModuleId);
                database.AddInParameter(commmand, "p_function_id", DbType.String, logEntry.FunctionId);
                database.AddInParameter(commmand, "p_component", DbType.String, logEntry.Component.ToString());
                database.AddInParameter(commmand, "p_parameter_values", DbType.String, logEntry.ParameterValues);
                database.AddInParameter(commmand, "p_flag", DbType.Int32, logEntry.Flag);
                database.AddInParameter(commmand, "p_instance_id", DbType.String, logEntry.InstanceID);
                database.AddInParameter(commmand, "p_return_value", DbType.String, logEntry.ReturnValue);


                if (Formatter != null)
                {
                    database.AddInParameter(commmand, "p_formatted_message", DbType.String, Formatter.Format(logEntry));
                }
                else
                {
                    database.AddInParameter(commmand, "p_formatted_message", DbType.String, logEntry.Message);
                }

                database.AddOutParameter(commmand, "p_log_id", DbType.Int32, 9);
                database.ExecuteNonQuery(commmand);

                int logId = (int)database.GetParameterValue(commmand, "p_log_id");
                return logId;
            }
            catch (DbException ex)
            {
                ExceptionHelper.HandleCommonDbException(ex);
                throw;
            }
        }

        /// <summary>
        /// Executes the WriteLog stored procedure
        /// </summary>
        /// <param name="logEntry">The LogEntry to store in the database.</param>
        /// <param name="database">The database.</param>
        /// <returns>An integer for the LogEntry Id</returns>
        private int ExecuteWriteLogStoredProcedureForLogEntry(LogEntry logEntry, Database database)
        {
            try
            {
                DbCommand commmand = database.GetStoredProcCommand(WriteLogStoredProcName);

                database.AddInParameter(commmand, "p_app_name", DbType.String, _applicationName);
                database.AddInParameter(commmand, "p_event_id", DbType.Int32, logEntry.EventId);
                database.AddInParameter(commmand, "p_priority", DbType.Int32, logEntry.Priority);
                database.AddInParameter(commmand, "p_severity", DbType.String, logEntry.Severity.ToString());
                database.AddInParameter(commmand, "p_title", DbType.String, logEntry.Title);
                database.AddInParameter(commmand, "p_log_time", DbType.DateTime, logEntry.TimeStamp.ToLocalTime());
                database.AddInParameter(commmand, "p_machine_name", DbType.String, logEntry.MachineName);
                database.AddInParameter(commmand, "p_app_domain_name", DbType.String, logEntry.AppDomainName);
                database.AddInParameter(commmand, "p_process_id", DbType.String, logEntry.ProcessId);
                database.AddInParameter(commmand, "p_process_name", DbType.String, logEntry.ProcessName);
                database.AddInParameter(commmand, "p_thread_name", DbType.String, logEntry.ManagedThreadName);
                database.AddInParameter(commmand, "p_win32_thread_id", DbType.String, logEntry.Win32ThreadId);
                database.AddInParameter(commmand, "p_message", DbType.String, logEntry.Message);
                database.AddInParameter(commmand, "p_method_name", DbType.String, null);
                database.AddInParameter(commmand, "p_activity_id", DbType.String, logEntry.ActivityId.ToString());
                database.AddInParameter(commmand, "p_tracing_start_ticks", DbType.String, null);
                database.AddInParameter(commmand, "p_tracing_end_ticks", DbType.String, null);
                database.AddInParameter(commmand, "p_seconds_elapsed", DbType.Decimal, null);
                database.AddInParameter(commmand, "p_user_name", DbType.String, null);
                database.AddInParameter(commmand, "p_ip_address", DbType.String, null);
                database.AddInParameter(commmand, "p_user_roles", DbType.String, null);
                database.AddInParameter(commmand, "p_user_graphic_area", DbType.String, null);
                database.AddInParameter(commmand, "p_organization", DbType.String, null);
                database.AddInParameter(commmand, "p_office", DbType.String, null);
                database.AddInParameter(commmand, "p_module_id", DbType.String, null);
                database.AddInParameter(commmand, "p_function_id", DbType.String, null);
                database.AddInParameter(commmand, "p_component", DbType.String, null);
                database.AddInParameter(commmand, "p_parameter_values", DbType.String, null);
                database.AddInParameter(commmand, "p_flag", DbType.Int32, 0);
                database.AddInParameter(commmand, "p_instance_id", DbType.String, null);
                database.AddInParameter(commmand, "p_return_value", DbType.String, null);

                if (Formatter != null)
                {
                    database.AddInParameter(commmand, "p_formatted_message", DbType.String, Formatter.Format(logEntry));
                }
                else
                {
                    database.AddInParameter(commmand, "p_formatted_message", DbType.String, logEntry.Message);
                }

                database.AddOutParameter(commmand, "p_log_id", DbType.Int32, 9);
                database.ExecuteNonQuery(commmand);

                int logId = (int)database.GetParameterValue(commmand, "p_log_id");
                return logId;
            }
            catch (DbException ex)
            {
                ExceptionHelper.HandleCommonDbException(ex);
                throw;
            }
        }

        /// <summary>
        /// Executes the AddCategory stored procedure
        /// </summary>
        /// <param name="logEntry">The MyLogEntry to store in the database.</param>
        /// <param name="logID">The unique identifer for the MyLogEntry as obtained from the WriteLog Stored procedure.</param>
        /// <param name="database">The database.</param>
        private void ExecuteAddCategoryStoredProcedure(LogEntry logEntry, int logID, Database database)
        {
            try
            {
                foreach (string category in logEntry.Categories)
                {
                    DbCommand command = database.GetStoredProcCommand(AddCategoryStoredProcName);
                    database.AddInParameter(command, "p_app_name", DbType.String, _applicationName);
                    database.AddInParameter(command, "p_category_name", DbType.String, category);
                    database.AddInParameter(command, "p_log_id", DbType.Int32, logID);
                    database.ExecuteNonQuery(command);
                }
            }
            catch (DbException ex)
            {
                ExceptionHelper.HandleCommonDbException(ex);
                throw;
            }
        }

        #endregion
    }
}

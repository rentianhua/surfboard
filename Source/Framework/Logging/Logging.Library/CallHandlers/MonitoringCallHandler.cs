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
using System.Diagnostics;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Logging.Library.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;

namespace HiiP.Framework.Logging.Library
{
    [ConfigurationElementType(typeof(MonitoringCallHandlerData))]
    public class MonitoringCallHandler : CallHandlerBase
    {
        #region Variable
        //private LogWriter logWriter;
        //private bool logBeforeCall = true;
        //private bool logAfterCall = true;
        //private string beforeMessage = string.Empty;
        //private string afterMessage = string.Empty;
        //private List<string> categories = new List<string>();
        //private bool includeParameters = true;
        //private bool includeCallStack ;
        //private bool includeCallTime = true;
        private string moduleId;
        private string functionId;
        private ComponentType component;

        //private Stopwatch stopwatch;
        //private Nullable<long> tracingStartTicks;

        private static LogWriter lastUsedLogWriter;
        private static IConfigurationSource lastUsedConfigSource;
        private static object logWriterCacheLock = new object();

        #endregion

        #region Construction
        /// <summary>
        /// Creates a <see cref="MonitoringCallHandler"/> with default settings that writes
        /// to the default log writer.
        /// </summary>
        /// <remarks>See the <see cref="LogCallHandlerDefaults"/> class for the default values.</remarks>
        public MonitoringCallHandler()
        {

        }

        ///// <summary>
        ///// Creates a <see cref="LogCallHandler"/> with default settings that writes
        ///// to the given <see cref="LogWriter"/>.
        ///// </summary>
        ///// <remarks>See the <see cref="LogCallHandlerDefaults"/> class for the default values.</remarks>
        ///// <param name="logWriter"><see cref="LogWriter"/> to write logs to.</param>
        //public MonitoringCallHandler(LogWriter logWriter)
        //{
        //    this.logWriter = logWriter;
        //}

        /// <summary>
        /// Creates a <see cref="LogCallHandler"/> with default settings that writes
        /// to the logging block as defined in <paramref name="configurationSource"/>.
        /// </summary>
        /// <remarks>See the <see cref="LogCallHandlerDefaults"/> class for the default values.</remarks>
        /// <param name="configurationSource"><see cref="IConfigurationSource"/> containing logging configuration.</param>
        public MonitoringCallHandler(IConfigurationSource configurationSource)
        {
            GetWriterFromConfiguration(configurationSource);
        }

        #endregion

        #region Properties
        ///// <summary>
        ///// Should there be a log entry before calling the target?
        ///// </summary>
        ///// <value>true = yes, false = no</value>
        //public bool LogBeforeCall
        //{
        //    get { return logBeforeCall; }
        //    set { logBeforeCall = value; }
        //}

        ///// <summary>
        ///// Should there be a log entry after calling the target?
        ///// </summary>
        ///// <value>true = yes, false = no</value>
        //public bool LogAfterCall
        //{
        //    get { return logAfterCall; }
        //    set { logAfterCall = value; }
        //}

        ///// <summary>
        ///// Message to include in a pre-call log entry.
        ///// </summary>
        ///// <value>The message</value>
        //public string BeforeMessage
        //{
        //    get { return beforeMessage; }
        //    set { beforeMessage = value; }
        //}

        ///// <summary>
        ///// Message to include in a post-call log entry.
        ///// </summary>
        ///// <value>the message.</value>
        //public string AfterMessage
        //{
        //    get { return afterMessage; }
        //    set { afterMessage = value; }
        //}

        ///// <summary>
        ///// Gets the collection of categories to place the log entries into.
        ///// </summary>
        ///// <remarks>The category strings can include replacement tokens. See
        ///// the <see cref="CategoryFormatter"/> class for the list of tokens.</remarks>
        ///// <value>The list of category strings.</value>
        //public List<string> Categories
        //{
        //    get { return categories; }
        //}

        ///// <summary>
        ///// Should the log entry include the parameters to the call?
        ///// </summary>
        ///// <value>true = yes, false = no</value>
        //public bool IncludeParameters
        //{
        //    get { return includeParameters; }
        //    set { includeParameters = value; }
        //}

        ///// <summary>
        ///// Should the log entry include the call stack?
        ///// </summary>
        ///// <remarks>Logging the call stack requires full trust code access security permissions.</remarks>
        ///// <value>true = yes, false = no</value>
        //public bool IncludeCallStack
        //{
        //    get { return includeCallStack; }
        //    set { includeCallStack = value; }
        //}

        ///// <summary>
        ///// Should the log entry include the time to execute the target?
        ///// </summary>
        ///// <value>true = yes, false = no</value>
        //public bool IncludeCallTime
        //{
        //    get { return includeCallTime; }
        //    set { includeCallTime = value; }
        //}

        public string ModuleId
        {
            get
            {
                return moduleId;
            }
            set
            {
                moduleId = value;
            }
        }

        public string FunctionId
        {
            get
            {
                return functionId;
            }
            set
            {
                functionId = value;
            }
        }

        public ComponentType Component
        {
            get { return component; }
            set { component = value; }
        }
        #endregion

        #region ICallHandler Members

        /// <summary>
        /// Executes the call handler.
        /// </summary>
        /// <param name="input"><see cref="IMethodInvocation"/> containing the information about the current call.</param>
        /// <param name="getNext">delegate to get the next handler in the pipeline.</param>
        /// <returns>Return value from the target method.</returns>
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            Stopwatch stopwatch=null;
            Nullable<long> tracingStartTicks=null;
            if (Component != ComponentType.Suppress)
            {
                var logWriter = GetLogWriter();
                if (logWriter!=null && logWriter.IsTracingEnabled())
                {
                    stopwatch = Stopwatch.StartNew();
                    tracingStartTicks = Stopwatch.GetTimestamp();

                    //wipe off start trace
                    //LogPreCall(input);
                }
                if (Component != ComponentType.StoredProcedure)
                {
                    AppContext.Current.ModuleID = ModuleId;
                    AppContext.Current.FunctionID = FunctionId;
                }
            }
            
            IMethodReturn result = getNext()(input, getNext);

            if (Component != ComponentType.Suppress)
            {
                var logWriter = GetLogWriter();
                if (logWriter != null && logWriter.IsTracingEnabled())
                {
                    LogPostCall(input, result, stopwatch, tracingStartTicks);
                }
            }
            
            return result;
        }

        private void LogPostCall(IMethodInvocation input, IMethodReturn result, Stopwatch stopwatch, Nullable<long> tracingStartTicks)
        {
            //if (logAfterCall)
            {
                MonitoringPopulateLogEntry utility = new MonitoringPopulateLogEntry();

                if (FunctionId != null)
                {
                    AppContext.Current.FunctionID = FunctionId;
                }

                if (ModuleId != null)
                {
                    AppContext.Current.ModuleID = ModuleId;
                }


                utility.PopulateLogEntryForCallHandler(input,
                    //categories, 
                    //includeParameters, includeCallStack,
                    result,
                    tracingStartTicks,
                    Stopwatch.GetTimestamp(),
                    (stopwatch==null)?0:stopwatch.ElapsedMilliseconds,
                    Resources.EndTrace,
                    AppContext.Current.ModuleID,
                    AppContext.Current.FunctionID,
                    Component);

            }
        }

        #endregion

        #region Private method
        private static LogWriter GetLogWriter()
        {
            if (lastUsedLogWriter!=null)
            {
                return lastUsedLogWriter;
            }
            var logWriter = Logger.Writer;
            if (logWriter != null)
            {
                return logWriter;
            }

            return GetWriterFromConfiguration(ConfigurationSourceFactory.Create());
        }
        private static LogWriter GetWriterFromConfiguration(IConfigurationSource configSource)
        {
            
            if (configSource != lastUsedConfigSource)
            {
                lock (logWriterCacheLock)
                {
                    if (configSource != lastUsedConfigSource)
                    {
                        lastUsedConfigSource = configSource;
                        LogWriterFactory factory = new LogWriterFactory(configSource);
                        lastUsedLogWriter = factory.Create();
                    }
                }
            }
            return lastUsedLogWriter;
        }
        #endregion

    }
}

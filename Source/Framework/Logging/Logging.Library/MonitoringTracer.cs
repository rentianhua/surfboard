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
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Logging.Library.Constants;
using HiiP.Framework.Logging.Library.Properties;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace HiiP.Framework.Logging.Library
{
    public class MonitoringTracer : IDisposable
    {
        #region Variable
        private TracerInstrumentationListener instrumentationListener;

        private Stopwatch stopwatch;
        private long tracingStartTicks;
        private bool tracerDisposed;
        private bool tracingAvailable;

        //Unused field
        //private static Guid id;

        private string moduleId;
        private string functionId;
        private ComponentType component;
        #endregion

        #region Construction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="moduleId"></param>
        /// <param name="functionId"></param>
        /// <param name="component"></param>
        public MonitoringTracer(Guid activityId, string moduleId, string functionId, ComponentType component):
            this(activityId,LoggingCategories.Monitoring, moduleId,functionId,component)
        {
        }

        /// <summary>
        /// Tracer for specified category, like exception handling
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="category"></param>
        /// <param name="moduleId"></param>
        /// <param name="functionId"></param>
        /// <param name="component"></param>
        public MonitoringTracer(Guid activityId, string category,string moduleId, string functionId, ComponentType component)
        {
            if (CheckTracingAvailable())
            {
                SetActivityId(activityId);

                ModuleId = moduleId;
                FunctionId = functionId;
                Component = component;
                //The default category is Trace, the user can use other constructor to overwrite it.
                Initialize(category, ConfigurationSourceFactory.Create());
            }
        }
        #endregion

        #region Properties
        public string ModuleId
        {
            get
            {
                return moduleId;
            }
            set { moduleId = value; }
        }

        public string FunctionId
        {
            get
            {
                return functionId;
            }
            set { functionId = value; }
        }

        public ComponentType Component
        {
            get { return component; }
            set { component = value; }
        }
        #endregion

        #region DeConstruction

        /// <summary>
        /// <para>Releases unmanaged resources and performs other cleanup operations before the <see cref="Tracer"/> is 
        /// reclaimed by garbage collection</para>
        /// </summary>
        ~MonitoringTracer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Causes the <see cref="Tracer"/> to output its closing message.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="Tracer"/> and optionally releases 
        /// the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> 
        /// to release only unmanaged resources.</para>
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !tracerDisposed)
            {
                if (tracingAvailable)
                {
                    try
                    {
                        if (IsTracingEnabled()) WriteTraceEndMessage();
                    }
                    finally
                    {
                        try
                        {
                            StopLogicalOperation();
                        }
                        catch (SecurityException)
                        {
                        }
                    }
                }

                this.tracerDisposed = true;
            }
        }
        #endregion

        #region Public method
        /// <summary>
        /// Answers whether tracing is enabled
        /// </summary>
        /// <returns>true if tracing is enabled</returns>
        public bool IsTracingEnabled()
        {
            LogWriter writer = GetWriter();
            return writer.IsTracingEnabled();
        }


        internal static bool IsTracingAvailable()
        {
            bool tracingAvailable = false;

            try
            {
                tracingAvailable = SecurityManager.IsGranted(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
            }
            catch (SecurityException)
            { }

            return tracingAvailable;
        }
        #endregion

        #region Private method
        private bool CheckTracingAvailable()
        {
            tracingAvailable = IsTracingAvailable();

            return tracingAvailable;
        }

        private void Initialize(string operation, IConfigurationSource configurationSource)
        {
            if (configurationSource != null)
            {
                instrumentationListener = EnterpriseLibraryFactory.BuildUp<TracerInstrumentationListener>(configurationSource);
            }
            else
            {
                instrumentationListener = new TracerInstrumentationListener(false);
            }

            StartLogicalOperation(operation);
            if (IsTracingEnabled())
            {
                instrumentationListener.TracerOperationStarted(PeekLogicalOperationStack() as string);

                stopwatch = Stopwatch.StartNew();
                tracingStartTicks = Stopwatch.GetTimestamp();

                //wipe off start trace
                //WriteTraceStartMessage();
            }
        }

        //Uncalled method
        //private void WriteTraceStartMessage()
        //{
        //    MonitoringPopulateLogEntry utility = new MonitoringPopulateLogEntry();
        //    MonitoringLogEntry logEntry = utility.PopulateLogEntryForTrace(true);
        //    logEntry.TracingStartTicks = tracingStartTicks;
        //    logEntry.MethodName = GetExecutingMethodName();
        //    string message = string.Format(Resources.Culture, Resources.StartTrace, logEntry.ActivityId, logEntry.MethodName, logEntry.TracingStartTicks);
        //    logEntry.Message = message;
        //    logEntry.Component = Component;
        //    logEntry.ModuleId = ModuleId;
        //    logEntry.FunctionId = FunctionId;

        //    LogWriter writer = GetWriter();
        //    writer.Write(logEntry);
        //}

        private void WriteTraceEndMessage()
        {
            MonitoringPopulateLogEntry utility = new MonitoringPopulateLogEntry();
            MonitoringLogEntry logEntry = utility.PopulateLogEntryForTrace(false);
            long tracingEndTicks = Stopwatch.GetTimestamp();
            decimal secondsElapsed = utility.GetSecondsElapsed(stopwatch.ElapsedMilliseconds);
            logEntry.TracingStartTicks = tracingStartTicks;
            logEntry.TracingEndTicks = tracingEndTicks;
            logEntry.SecondsElapsed = secondsElapsed;
            logEntry.MethodName = GetExecutingMethodName();
            string message = string.Format(Resources.Culture, Resources.EndTrace, logEntry.ActivityId, logEntry.MethodName, logEntry.TracingEndTicks, logEntry.SecondsElapsed);
            logEntry.Message = message;
            logEntry.Component = Component;
            logEntry.ModuleId = ModuleId;
            logEntry.FunctionId = FunctionId;
            LogWriter logWriter = GetWriter();

            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += delegate
                {
                    AppContext.SetToCallContext(AppContext.Current.ToDictionary());
                    logWriter.Write(logEntry);
                };

                worker.RunWorkerAsync();
            }
            instrumentationListener.TracerOperationEnded(PeekLogicalOperationStack() as string, stopwatch.ElapsedMilliseconds);
        }

        private LogWriter GetWriter()
        {
            return Logger.Writer;
        }

        //Uncalled method
        //private static Guid GetActivityId()
        //{
        //    return AppContext.Current.ActivityID != null ? new Guid(AppContext.Current.ActivityID) : Trace.CorrelationManager.ActivityId;
        //}

// ReSharper disable UnusedMethodReturnValue.Local
        private static Guid SetActivityId(Guid activityId)
// ReSharper restore UnusedMethodReturnValue.Local
        {
            return Trace.CorrelationManager.ActivityId = activityId;
        }

        private static void StartLogicalOperation(string operation)
        {
            Trace.CorrelationManager.StartLogicalOperation(operation);
        }

        private static void StopLogicalOperation()
        {
            Trace.CorrelationManager.StopLogicalOperation();
        }

        private static object PeekLogicalOperationStack()
        {
            return Trace.CorrelationManager.LogicalOperationStack.Peek();
        }

        private string GetExecutingMethodName()
        {
            string result = "Unknown";
            StackTrace trace = new StackTrace(false);

            for (int index = 0; index < trace.FrameCount; ++index)
            {
                StackFrame frame = trace.GetFrame(index);
                MethodBase method = frame.GetMethod();
                //Unused field
                //ParameterInfo[] info = method.GetParameters();

                if (method.DeclaringType != GetType())
                {
                    result = string.Concat(method.DeclaringType.FullName, ".", method.Name);
                    break;
                }
            }
            return result;
        }
        #endregion
    }
}

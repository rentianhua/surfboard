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
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Messaging;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using NCS.IConnect.ExceptionHandling.Database;

namespace HiiP.Framework.Logging.Library
{
    [ConfigurationElementType(typeof(HiiPLoggingExceptionHandlerData))]
    public class HiiPLoggingExceptionHandler : IExceptionHandler
    {
        private readonly string logCategory;
        private readonly int eventId;
        private TraceEventType severity;
        private readonly string defaultTitle;
        private readonly Type formatterType;
        private readonly int minimumPriority;
        private readonly ComponentType component;
        private readonly LogWriter logWriter;

        public HiiPLoggingExceptionHandler(
            string logCategory,
            int eventId, 
            TraceEventType severity,
            string title,
            int priority,
            Type formatterType,
            ComponentType component,
            LogWriter writer)
        {
            this.logCategory = logCategory;
            this.eventId = eventId;
            this.severity = severity;
            this.defaultTitle = title;
            this.minimumPriority = priority;
            this.formatterType = formatterType;
            this.component = component;
            this.logWriter = writer;
        }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            //#3493, if it is sql exceptio and error number is 50000, then severity is warning.
            DbBusinessException sqlError = exception as DbBusinessException;
            if (null!=sqlError)
            {
                this.severity = TraceEventType.Warning;
            }

            WriteToLog(CreateMessage(exception, handlingInstanceId), exception.Data, handlingInstanceId.ToString());
            return exception;
        }

        protected virtual void WriteToLog(string logMessage, IDictionary exceptionData, string InstanceId)
        {
            MonitoringLogEntry logEntry = new MonitoringLogEntry();
            logEntry.InstanceID = InstanceId;
            logEntry.ExtendedActivityId= AppContext.Current.ActivityID;
            logEntry.ModuleId = AppContext.Current.ModuleID;
            logEntry.FunctionId = AppContext.Current.FunctionID;
            logEntry.UserName = AppContext.Current.UserName;
            logEntry.IpAddress = AppContext.Current.IPAddress;
            logEntry.UserRoles = AppContext.Current.UserRoles;
            logEntry.UserGraphicArea = AppContext.Current.GraphicArea;
            //logEntry.Organization = AppContext.Current.Organization;
            logEntry.Office = AppContext.Current.Office;
            logEntry.Categories.Add(logCategory);
            logEntry.EventId = eventId;
            logEntry.Title = defaultTitle;
            logEntry.Priority = minimumPriority;
            logEntry.Severity = severity;
            logEntry.Component = component;
            logEntry.Message = logMessage;
           
            foreach (DictionaryEntry dataEntry in exceptionData)
            {
                var key = dataEntry.Key as string;
                if (key==null)
                {
                    continue;
                }
                logEntry.ExtendedProperties.Add(key, dataEntry.Value);
            }

            this.logWriter.Write(logEntry);
        }
        


        protected virtual StringWriter CreateStringWriter()
        {
            return new StringWriter(CultureInfo.InvariantCulture);
        }

        protected virtual Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionFormatter CreateFormatter(StringWriter writer, Exception exception)
        {
            ConstructorInfo constructor = GetFormatterConstructor();
            return (Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionFormatter)constructor.Invoke(
                new object[] { writer, exception }
                );
        }

        private ConstructorInfo GetFormatterConstructor()
        {
            Type[] types = new [] { typeof(TextWriter), typeof(Exception) };
            ConstructorInfo constructor = formatterType.GetConstructor(types);
            if (constructor == null)
            {
                throw new ExceptionHandlingException(Messages.Framework.FWE101.Format(formatterType.AssemblyQualifiedName));
            }
            return constructor;
        }

        private string CreateMessage(Exception exception, Guid handlingInstanceID)
        {
            StringWriter writer = null;
            StringBuilder stringBuilder ;
            try
            {
                writer = CreateStringWriter();
                writer.WriteLine("HandlingInstanceID: {0}", handlingInstanceID.ToString());
                Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionFormatter formatter = CreateFormatter(writer, exception);
                formatter.Format();
                stringBuilder = writer.GetStringBuilder();
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return stringBuilder.ToString();
        }
    }
}

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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace HiiP.Framework.Logging.Library
{
    [Assembler(typeof(HiiPLoggingExceptionHandlerAssembler))]
    public class HiiPLoggingExceptionHandlerData : ExceptionHandlerData
    {

        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        private const string logCategory = "logCategory";
        private const string eventId = "eventId";
        private const string severity = "severity";
        private const string title = "title";
        private const string formatterType = "formatterType";
        private const string priority = "priority";
        private const string component = "component";

        public HiiPLoggingExceptionHandlerData()
        { }


        public HiiPLoggingExceptionHandlerData(string name, string logCategory, int eventId, TraceEventType severity, Type formatterType, int priority, ComponentType component)
            : this(name, logCategory, eventId, severity, title, typeConverter.ConvertToString(formatterType), priority,component)
        {
        }


        public HiiPLoggingExceptionHandlerData(string name, string logCategory, int eventId, TraceEventType severity, string title, string formatTypeName, int priority, ComponentType component)
            : base(name, typeof(HiiPLoggingExceptionHandler))
        {
            LogCategory = logCategory;
            EventId = eventId;
            Severity = severity;
            Title = title;
            FormatterTypeName = formatTypeName;
            Priority = priority;
            Component = component;
        }

        /// <summary>
        /// Gets or sets the default log category.
        /// </summary>
        [ConfigurationProperty(logCategory, IsRequired = true)]
        public string LogCategory
        {
            get { return (string)this[logCategory]; }
            set { this[logCategory] = value; }
        }

        /// <summary>
        /// Gets or sets the default event ID.
        /// </summary>
        [ConfigurationProperty(eventId, IsRequired = true)]
        public int EventId
        {
            get { return (int)this[eventId]; }
            set { this[eventId] = value; }
        }

        /// <summary>
        /// Gets or sets the default severity.
        /// </summary>
        [ConfigurationProperty(severity, IsRequired = true)]
        public TraceEventType Severity
        {
            get { return (TraceEventType)this[severity]; }
            set { this[severity] = value; }
        }

        /// <summary>
        ///  Gets or sets the default title.
        /// </summary>
        [ConfigurationProperty(title, IsRequired = true)]
        public string Title
        {
            get { return (string)this[title]; }
            set { this[title] = value; }
        }

        /// <summary>
        /// Gets or sets the formatter type.
        /// </summary>
        public Type FormatterType
        {
            get { return (Type)typeConverter.ConvertFrom(FormatterTypeName); }
            set { FormatterTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the formatter fully qualified assembly type name.
        /// </summary>
        /// <value>
        /// The formatter fully qualified assembly type name
        /// </value>
        [ConfigurationProperty(formatterType, IsRequired = true)]
        public string FormatterTypeName
        {
            get { return (string)this[formatterType]; }
            set { this[formatterType] = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value for messages to be processed.  Messages with a priority
        /// below the minimum are dropped immediately on the client.
        /// </summary>
        [ConfigurationProperty(priority, IsRequired = true)]
        public int Priority
        {
            get { return (int)this[priority]; }
            set { this[priority] = value; }
        }

        /// <summary>
        /// Gets or sets the default component.
        /// </summary>
        [ConfigurationProperty(component, IsRequired = true)]
        public ComponentType Component
        {
            get { return (ComponentType)this[component]; }
            set { this[component] = value; }
        }
    }

    public class HiiPLoggingExceptionHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
    {
        public IExceptionHandler Assemble(IBuilderContext context, ExceptionHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            HiiPLoggingExceptionHandlerData castedObjectConfiguration = (HiiPLoggingExceptionHandlerData)objectConfiguration;

            LogWriter writer
                = Logger.Writer;// (LogWriter)context.HeadOfChain.BuildUp(context, typeof(LogWriter), null, null);

            HiiPLoggingExceptionHandler createdObject
                = new HiiPLoggingExceptionHandler(
                    castedObjectConfiguration.LogCategory,
                    castedObjectConfiguration.EventId,
                    castedObjectConfiguration.Severity,
                    castedObjectConfiguration.Title,
                    castedObjectConfiguration.Priority,
                    castedObjectConfiguration.FormatterType,
                    castedObjectConfiguration.Component,
                    writer);

            return createdObject;
        }
    }
}

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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace HiiP.Framework.Logging.Library
{
    /// <summary>
    /// Configuration data defining FormattedMultiDatabaseTraceListenerData. This configuration section adds the name
    /// of the logging.	
    /// </summary>
    [Assembler(typeof(MonitoringDatabaseTraceListenerAssembler))]
    public class MonitoringDatabaseTraceListenerData : TraceListenerData
    {
        #region Variable
        private const string applicationNameProperty = "applicationName";

        private const string connectionStringNameProperty = "connectionStringName";

        private const string formatterNameProperty = "formatter";

        #endregion

        #region Construction
        /// <summary>
        /// Initializes a <see cref="MonitoringDatabaseTraceListenerData"/>.
        /// </summary>
        public MonitoringDatabaseTraceListenerData()
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="MonitoringDatabaseTraceListenerData"/> with
        /// name, stored procedure name, databse instance name, and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="connectionStringName">The database instance name.</param>
        /// <param name="appName">Name of the application.</param>
        /// <param name="formatterName">The formatter name.</param>
        public MonitoringDatabaseTraceListenerData(string name, string connectionStringName, string appName, string formatterName)
            : this(name, connectionStringName, appName,formatterName, TraceOptions.None)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="MonitoringDatabaseTraceListenerData"/> with
        /// name, stored procedure name, databse instance name, and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <param name="appName">Name of the application.</param>
        /// <param name="formatterName">The formatter name.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        public MonitoringDatabaseTraceListenerData(string name, string connectionStringName, string appName, string formatterName,
                                                  TraceOptions traceOutputOptions)
            : base(name, typeof(MonitoringDatabaseTraceListener), traceOutputOptions)
        {
            ConnectionStringName = connectionStringName;
            ApplicationName = appName;
            Formatter = formatterName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets the database instance name.
        /// </summary>
        [ConfigurationProperty(connectionStringNameProperty, IsRequired = true)]
        public string ConnectionStringName
        {
            get
            {
                return (string)base[connectionStringNameProperty];
            }
            set
            {
                base[connectionStringNameProperty] = value;
            }
        }

        /// <summary>
        /// Gets and sets the formatter name.
        /// </summary>
        [ConfigurationProperty(formatterNameProperty, IsRequired = true)]
        public string Formatter
        {
            get
            {
                return (string)base[formatterNameProperty];
            }
            set
            {
                base[formatterNameProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        [ConfigurationProperty(applicationNameProperty, IsRequired = true)]
        public string ApplicationName
        {
            get
            {
                return (string)base[applicationNameProperty];
            }
            set
            {
                base[applicationNameProperty] = value;
            }
        }
        #endregion
    }


    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to 
    /// be used directly from your code.	
    /// </summary>
    public class MonitoringDatabaseTraceListenerAssembler : TraceListenerAsssembler
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="MonitoringDatabaseTraceListener"/> based on an instance of <see cref="MonitoringDatabaseTraceListenerData"/>.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="MonitoringDatabaseTraceListenerData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="MonitoringDatabaseTraceListener"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration,
                                               IConfigurationSource configurationSource,
                                               ConfigurationReflectionCache reflectionCache)
        {
            MonitoringDatabaseTraceListenerData castedObjectConfiguration
                = (MonitoringDatabaseTraceListenerData)objectConfiguration;

            Database database =
                (Database)
                context.HeadOfChain.BuildUp(context, typeof(Database), null,
                                            castedObjectConfiguration.ConnectionStringName);
            ILogFormatter formatter
                =
                LogFormatterCustomFactory.Instance.Create(context, castedObjectConfiguration.Formatter,
                                                          configurationSource, reflectionCache);

            TraceListener createdObject
                = new MonitoringDatabaseTraceListener(
                    database,
                    formatter,
                    castedObjectConfiguration.ApplicationName,
                    castedObjectConfiguration.ConnectionStringName);

            return createdObject;
        }
    }
}

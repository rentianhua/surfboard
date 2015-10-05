using System;
using System.Collections.Generic;
using Cedar.Core;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.IoC;
using Cedar.Framwork.AuditTrail.Configuration;
using Cedar.Framwork.AuditTrail.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Cedar.Framwork.AuditTrail
{
    /// <summary>
	/// This class is used to write audit log entry.
	/// </summary>
	public class AuditLogger : IDisposable
    {
        private static readonly object syncHelper = new object();
        private static AuditLogProviderBase provider;
        private bool disposed;
        internal AuditLogEntry logEntry;
        private AuditLogListenerBase listener;

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>
        /// The providers.
        /// </value>
        public static ServiceLocatableDictionary<AuditLogProviderBase> Providers
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public static AuditLogProviderBase Provider
        {
            get
            {
                if (AuditLogger.provider != null)
                {
                    return AuditLogger.provider;
                }
                AuditLogProviderBase result;
                lock (AuditLogger.syncHelper)
                {
                    if (AuditLogger.provider != null)
                    {
                        result = AuditLogger.provider;
                    }
                    else
                    {
                        result = (AuditLogger.provider = ServiceLocatorFactory.GetServiceLocator(null).GetService<AuditLogProviderBase>(null));
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the audit log listener.
        /// </summary>
        /// <value>
        /// The audit log listener.
        /// </value>
        public AuditLogListenerBase AuditLogListener
        {
            get
            {
                if (this.listener != null)
                {
                    return this.listener;
                }
                this.listener = new CompositeAuditLogListener(ServiceLocatorFactory.GetServiceLocator(null).GetAllServices<AuditLogListenerBase>());
                return this.listener;
            }
        }

        private AuditLogger(AuditLogEntry logEntry)
        {
            AuditLogger.Providers = new ServiceLocatableDictionary<AuditLogProviderBase>(null);
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Create <see cref="T:Cedar.Framwork.AuditTrail.AuditLogger" /> based on the specified function name.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>The creatd <see cref="T:Cedar.Framwork.AuditTrail.AuditLogger" />.</returns>
        public static AuditLogger CreateAuditLogger(string functionName)
        {
            Guard.ArgumentNotNullOrEmpty(functionName, "functionName");
            if (string.IsNullOrEmpty(ApplicationContext.Current.UserName))
            {
                //throw new InvalidOperationException(Resources.ExceptionCurrentUserNameNotExists);
            }
            if (string.IsNullOrEmpty(ApplicationContext.Current.TransactionId))
            {
                //throw new InvalidOperationException(Resources.ExceptionCurrentTransactionIdNotExists);
            }
            AuditLogEntry auditLogEntry = new AuditLogEntry(functionName, null, null, null, null);

            ConfigManager.GetConfigurationSection<AuditTrailSettings>().Configure(ServiceLocatorFactory.GetServiceLocator());
            return new AuditLogger(auditLogEntry);
        }

        /// <summary>
        /// Writes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="actionDescription">The action description.</param>
        /// <param name="actionRemarks">The action remarks.</param>
        public void Write(string action, string actionDescription, string inputsParams, dynamic actionRemarks)
        {
            this.EnsureNotDisposed();
            Guard.ArgumentNotNullOrEmpty(action, "action");
            this.logEntry.Action = action;
            this.logEntry.ActionDescription = actionDescription;
            this.logEntry.ActionDescription = actionRemarks;
            this.logEntry.ActionRemarks = actionRemarks;
            this.logEntry.InputsParams = inputsParams;
        }

        /// <summary>
        /// Writes the specified log data.
        /// </summary>
        /// <param name="logData">The log data.</param>
        /// <param name="operationType">Type of the operation.</param>
        /// <param name="customOperationName">Name of the custom operation.</param>
        public void Write(object logData, OperationType operationType, string customOperationName = null)
        {
            this.EnsureNotDisposed();
            if (operationType == OperationType.Custom)
            {
                Guard.ArgumentNotNullOrEmpty(customOperationName, "customOperationName");
            }
            string operation = customOperationName;
            if (operationType != OperationType.Custom)
            {
                operation = operationType.ToString();
            }
            AuditLogEntryItem item = new AuditLogEntryItem(operation, Guid.NewGuid().ToString(), 1)
            {
                LogData = logData
            };
            this.logEntry.Items.Add(item);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.EnsureNotDisposed();
            this.Flush();
        }

        /// <summary>
        /// Drive the audit log listeners to write the stored audit log entry.
        /// </summary>
        public void Flush()
        {
            this.disposed = true;
            this.AuditLogListener.Write(this.logEntry);
        }

        /// <summary>
        /// Gets the log detail.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="logId">The log ID.</param>
        /// <returns>The <see cref="T:System.Data.DataSet" /> contains the given table specific data change.</returns>
        public static List<dynamic> GetLogDetail(string tableName, string logId)
        {
            Guard.ArgumentNotNullOrEmpty(tableName, "tableName");
            Guard.ArgumentNotNullOrEmpty(logId, "logId");
            return AuditLogger.Provider.GetLogDetail(tableName, logId);
        }

        private void EnsureNotDisposed()
        {
            if (this.disposed)
            {
                throw new InvalidOperationException(Resources.ExceptionLoggerIsDisposed);
            }
        }
    }
}

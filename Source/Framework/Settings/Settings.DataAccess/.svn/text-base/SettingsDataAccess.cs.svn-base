#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :   Settings/Data Access
// COMPONENT DESC    :  
//
// CREATED DATE/BY   : 06/11/2008/Yang Jian Hua
//
// REVISION HISTORY  : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Settings.Interface.Constants;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using NCS.IConnect.Messaging;

namespace HiiP.Framework.Settings.DataAccess
{
    public class SettingsDataAccess : HiiPDataAccessBase
    {
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
   FunctionID = FunctionNames.RetrieveLoggingFilterID)]
        public LoggingFilterDS RetrieveLoggingFilter(string category)
        {
            LoggingFilterDS ds = new LoggingFilterDS();
            Helper.Fill(ds.T_IC_LOGGING_FILTER, "P_IC_LOGGING_FILTER_S", category);
            return ds;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
   FunctionID = FunctionNames.UpdateLoggingFilterID)]
        public void UpdateLoggingFilter(LoggingFilterDS ds)
        {
            DbCommand insertCommand = null;
            DbCommand updateCommand = null;
            DbCommand deleteCommand = null;

            string databaseName = ((DatabaseSettings)ConfigurationManager.
                GetSection("dataConfiguration")).DefaultDatabase;
            Database db = DatabaseFactory.CreateDatabase(databaseName);

            if (ds.T_IC_LOGGING_FILTER.Select(string.Empty, string.Empty, DataViewRowState.Added).Length > 0)
            {
                insertCommand = db.GetStoredProcCommand("P_IC_LOGGING_FILTER_I");
                db.AddInParameter(insertCommand, "Category", DbType.String, "CATEGORY", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "User_ID", DbType.String, "USER_ID", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "Flag", DbType.Int32, "FLAG", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "Version_No", DbType.Int32, "VERSION_NO", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "Transaction_ID", DbType.String, AppContext.Current.TransactionID);
                db.AddInParameter(insertCommand, "Created_By", DbType.String, AppContext.Current.UserName);
                db.AddInParameter(insertCommand, "Created_Time", DbType.DateTime, DateTime.Now);
            }
            if (ds.T_IC_LOGGING_FILTER.Select(string.Empty, string.Empty, DataViewRowState.ModifiedCurrent).Length > 0)
            {
                updateCommand = db.GetStoredProcCommand("P_IC_LOGGING_FILTER_U");
                db.AddInParameter(updateCommand, "Category", DbType.String, "CATEGORY", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "User_ID", DbType.String, "USER_ID", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "Flag", DbType.Int32, "FLAG", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "Originalversion_no", DbType.Int32, "VERSION_NO", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "Transaction_ID", DbType.String, AppContext.Current.TransactionID);
                db.AddInParameter(updateCommand, "Last_Updated_By", DbType.String, AppContext.Current.UserName);
                db.AddInParameter(updateCommand, "Last_Updated_Time", DbType.DateTime, DateTime.Now);
            }

            if (ds.T_IC_LOGGING_FILTER.Select(string.Empty, string.Empty, DataViewRowState.Deleted).Length > 0)
            {
                deleteCommand = db.GetStoredProcCommand("P_IC_LOGGING_FILTER_D");
                db.AddInParameter(deleteCommand, "Category", DbType.String, "CATEGORY", DataRowVersion.Original);
                db.AddInParameter(deleteCommand, "User_ID", DbType.String, "USER_ID", DataRowVersion.Original);
                db.AddInParameter(deleteCommand, "Version_No", DbType.Int32, "VERSION_NO", DataRowVersion.Original);
                db.AddInParameter(deleteCommand, "Transaction_ID", DbType.String, AppContext.Current.TransactionID);
                db.AddInParameter(deleteCommand, "Last_Updated_By", DbType.String, AppContext.Current.UserName);
                db.AddInParameter(deleteCommand, "Last_Updated_Time", DbType.DateTime, DateTime.Now);
            }

            db.UpdateDataSet(ds, "T_IC_LOGGING_FILTER",
                               insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard);
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
          FunctionID = FunctionNames.UpdateLoggingFilterID)]
        public bool IsUserExists(string category, string userId)
        {
            DbCommand command = Helper.BuildDbCommand("P_IC_LOGGING_FILTER_EXISTS");
            Helper.AssignParameterValues(command, category, userId);
            Helper.ExecuteNonQuery(command);

            if (!command.Parameters["@p_count"].Value.ToString().Equals("0"))
            {
                return true;
            }

            return false;

        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
          FunctionID = FunctionNames.MessageMaintenanceID)]
        public DataTable RetrieveMessages(string category, string severity, string messageValue)
        { 
            DataTable dt = new DataTable();
            var manger = MessageManagerFactory.Create();
            var provider = manger.MessageProviders.Where(x => typeof(DbMessageProvider).IsAssignableFrom(x.GetType())).FirstOrDefault<MessageProvider>() as DbMessageProvider;
            if (provider == null)
            {
                throw new ConfigurationErrorsException("Unable to find out DbMessageProvider");
            }
            Helper.Fill(dt, "P_IC_MESSAGES_S", provider.ApplicationName,
                string.IsNullOrEmpty(category)?null:category,
                string.IsNullOrEmpty(severity)?null:severity,
                string.IsNullOrEmpty(messageValue)?null:messageValue);

            foreach (DataRow item in dt.Rows)
            {
                item["SEVERITY"] = MessageSeverityHelper.ParseSeverity(item["SEVERITY"].ToString());
            }
            return dt;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
          FunctionID = FunctionNames.MessageDetailID)]
        public DataTable RetrieveMessage(string category, string id)
        { 
            DataTable dt = new DataTable();
            var manger = MessageManagerFactory.Create();
            var provider = manger.MessageProviders.Where(x => typeof(DbMessageProvider).IsAssignableFrom(x.GetType())).FirstOrDefault<MessageProvider>() as DbMessageProvider;
            if (provider == null)
            {
                throw new ConfigurationErrorsException("Unable to find out DbMessageProvider");
            }

            Helper.Fill(dt, "P_IC_MESSAGES_GET_SINGLE_MSG",provider.ApplicationName,null, id, category);
            return dt;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
          FunctionID = FunctionNames.MessageDetailID)]
        public void UpdateMessage(DataTable messages)
        {
            var manger = MessageManagerFactory.Create();
            var provider = manger.MessageProviders.Where(x => typeof(DbMessageProvider).IsAssignableFrom(x.GetType())).FirstOrDefault<MessageProvider>() as DbMessageProvider;
            if (provider == null)
            {
                throw new ConfigurationErrorsException("Unable to find out DbMessageProvider");
            }
            DbCommand updateCommand = null;

            string databaseName = ((DatabaseSettings)ConfigurationManager.
                GetSection("dataConfiguration")).DefaultDatabase;
            Database db = DatabaseFactory.CreateDatabase(databaseName);
            var rows = messages.Select(string.Empty, string.Empty, DataViewRowState.ModifiedCurrent);

            if (rows.Length > 0)
            {
                updateCommand = db.GetStoredProcCommand("P_IC_MESSAGES_U");
                db.AddInParameter(updateCommand, "p_id", DbType.String, "ID", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "o_id", DbType.String, "ID", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "p_category", DbType.String, "CATEGORY", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "o_category", DbType.String, "CATEGORY", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "p_value", DbType.String, "VALUE", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "p_severity", DbType.String, "SEVERITY", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "p_app_name", DbType.String, provider.ApplicationName);
                db.AddInParameter(updateCommand, "p_culture_name", DbType.String, "CULTURE_NAME", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "o_culture_name", DbType.String, "CULTURE_NAME", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "p_remark", DbType.String, "REMARK", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "o_version_no", DbType.Int32, "VERSION_NO", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "p_transaction_id", DbType.String, AppContext.Current.TransactionID);
                db.AddInParameter(updateCommand, "p_last_updated_by", DbType.String, AppContext.Current.UserName);
                db.AddInParameter(updateCommand, "p_last_updated_time", DbType.DateTime, DateTime.Now);
            }


            db.UpdateDataSet(rows, null, updateCommand, null, UpdateBehavior.Standard);
        }
    }
}

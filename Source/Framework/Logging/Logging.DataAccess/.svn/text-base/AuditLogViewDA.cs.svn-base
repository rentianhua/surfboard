#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :   Framework/Audit Log View
// COMPONENT DESC    :  
//
// CREATED DATE/BY   : 12/9/2008/He Jiang Yan
//
// REVISION HISTORY  : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion
using System;
using System.Collections.Generic;
using System.Text;
using HiiP.Framework.Common;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.BusinessEntity;
using System.Data.Common;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using System.Linq;
using System.Text.RegularExpressions;
using HiiP.Framework.Common.Server;
using System.Data;
using NCS.IConnect.AuditTrail;
using NCS.IConnect.AuditTrail.Configuration;

namespace HiiP.Framework.Logging.DataAccess
{
    public class AuditLogViewDA : HiiPDataAccessBase
    {
        HiiPDbHelper _loggingDBHelper;
        private const string GetAppIdSP = "P_IC_APP_GET_ID";
        private const string GetIndexRangeSP = "P_IC_AUDIT_LOG_ID_RANGE";

        public AuditLogViewDA()
        {
            string connectionName = Utility.GetLoggingConnectionStringName();
            foreach (var storage in AuditTrailConfigurationManager.StorageProviders)
            {
                DbStorageProviderData dbStorage = storage as DbStorageProviderData;
                if (null!=dbStorage)
                {
                    connectionName = dbStorage.DatabaseName;
                    break;
                }
            }
            _loggingDBHelper = InstanceBuilder.PolicyInjector.Create<HiiPDbHelper>(connectionName);
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.AuditLogModuleID, FunctionID = FunctionNames.AuditLogViewFunctionID)]
        public string GetAppID(string appName)
        {
            DbCommand cmd = _loggingDBHelper.BuildDbCommand(GetAppIdSP);
            _loggingDBHelper.AssignParameterValues(cmd, appName);
            _loggingDBHelper.ExecuteNonQuery(cmd);
            object result = _loggingDBHelper.GetParameterValue(cmd, "p_app_id");

            if (null == result
                || DBNull.Value == result)
            {
                return string.Empty;
            }

            return result.ToString();

        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.AuditLogModuleID, FunctionID = FunctionNames.AuditLogViewFunctionID)]
        public Int64[] GetIndexRange(DateTimeCompare timeEntity)
        {
            DbCommand cmd = _loggingDBHelper.BuildDbCommand(GetIndexRangeSP);
            _loggingDBHelper.AssignParameterValues(cmd,timeEntity.StartTime,timeEntity.EndTime);
            _loggingDBHelper.ExecuteNonQuery(cmd);

            object temp = _loggingDBHelper.GetParameterValue(cmd, "min");
            Int64 min = (temp==DBNull.Value)?0:(Int64)temp;

            temp = _loggingDBHelper.GetParameterValue(cmd, "max");
            Int64 max = (temp == DBNull.Value) ? 0 : (Int64)temp;

            return new [] { min,max};
            
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.AuditLogModuleID, FunctionID = FunctionNames.AuditLogViewFunctionID)]
        public AuditLogViewDataSet GetSummary(AuditTrailSearchCriteria criteria,string appName )
        { 
            AuditLogViewDataSet ds = new AuditLogViewDataSet();
            _loggingDBHelper.Fill(ds.T_IC_AUDIT_LOG_QUERY, "P_IC_AUDIT_LOG_SUMMARY_GET",
                appName,
                criteria.TransactionId,
                criteria.FunctionName,
                criteria.UserName,
                criteria.StartDate,
                criteria.EndDate,
                criteria.AppVersion,
                criteria.ExtendedProperties["@p_app_id"],
                criteria.ExtendedProperties["@startIndex"],
                criteria.ExtendedProperties["@endIndex"]);

            return ds;
        }
        
    }
}

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
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using NCS.IConnect.AuditTrail;
using NCS.IConnect.AuditTrail.Configuration;

using HiiP.Framework.Common.ApplicationContexts.CallHandlers;
using HiiP.Framework.Common;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.DataAccess;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using HiiP.Framework.Logging.Library;
using Const = HiiP.Framework.Logging.Interface.Constants;

namespace HiiP.Framework.Logging.BusinessComponent
{
    public class AuditLogViewBC : HiiPBusinessComponentBase
    {
        private const Int64 Offset = 100;
        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AuditLogModuleID, FunctionID = FunctionNames.AuditLogViewFunctionID)]
        public AuditLogViewDataSet GetAuditDataSummary(AuditTrailSearchCriteria criteria)
        {
            var da = InstanceBuilder.CreateInstance<AuditLogViewDA>();

            //Must there is a storage provider of audit trail
            string appName = AuditTrailConfigurationManager.StorageProviders[0].ApplicationName;
            string appId = da.GetAppID(appName);

            //Store the value of criteria first
            string funcName = ReplaceWildcard(criteria.FunctionName);
            string userName = ReplaceWildcard(criteria.UserName);

            string tableName = ReplaceWildcard(criteria.ExtendedProperties["@tableName"].ToString());
            string operationType = ReplaceWildcard(criteria.ExtendedProperties["@operationType"].ToString());

            string hostName = ReplaceWildcard(criteria.ExtendedProperties["@hostName"].ToString());
            string ipAddress = ReplaceWildcard(criteria.ExtendedProperties["@ipAddress"].ToString());
            string device = ReplaceWildcard(criteria.ExtendedProperties["@device"].ToString());

            Int64 startIndex = (Int64)criteria.ExtendedProperties["@startIndex"];
            Int64 endIndex = (Int64)criteria.ExtendedProperties["@endIndex"];
            Int64 min = (Int64)criteria.ExtendedProperties["min"];
            Int64 max = (Int64)criteria.ExtendedProperties["max"];

            //Do not want to pass the above criteria to SP, so re-create the extended properties
            criteria.ExtendedProperties = new Dictionary<string, object>();
            criteria.ExtendedProperties.Add("@p_app_id", string.IsNullOrEmpty(appId) ? null : appId);
            criteria.ExtendedProperties.Add("@startIndex", startIndex);
            criteria.ExtendedProperties.Add("@endIndex",startIndex+ Offset);

            AuditLogViewDataSet dsResult = null;
            Int64 lastEndIndex = startIndex;
            int count ;

            do
            {
                lastEndIndex = lastEndIndex + Offset;
                //Retrieve from database
                AuditLogViewDataSet ds = da.GetSummary(criteria,appName);

                //Using linq to filter data so that the performance can be improved
                var tempData = (from data in ds.T_IC_AUDIT_LOG_QUERY
                                where CompareData(data["LOG_FUNCTION"].ToString(), funcName)
                       && CompareData(data["USER_NAME"].ToString(), userName)
                       && CompareData(data["TABLE_NAME"].ToString(), tableName)
                       && CompareData(data["OPERATION"].ToString(), operationType)
                       && CompareData(data["HOST_NAME"].ToString(), hostName)
                       && CompareData(data["IP_ADDRESS"].ToString(), ipAddress)
                       && CompareData(data["DEVICE"].ToString(), device)
                       && data.INDEX>=min
                       && data.INDEX<=max
                                select data);

                if (null == dsResult)
                {
                    dsResult = ds.Clone() as AuditLogViewDataSet;
                }
                //Move to next batch
                criteria.ExtendedProperties["@startIndex"] = lastEndIndex;
                criteria.ExtendedProperties["@endIndex"] = lastEndIndex + Offset;
                count = tempData.Count();

                if (dsResult == null)
                {
                    dsResult=new AuditLogViewDataSet();
                }
                DataTable dt = dsResult.T_IC_AUDIT_LOG_QUERY;
                dt.BeginLoadData();
                foreach (var data in tempData)
                {
                    dt.LoadDataRow(data.ItemArray, true);
                }
                dt.EndLoadData();

                ds.Dispose();

                //Found anything, will return
            } while (count == 0 && lastEndIndex < max && lastEndIndex < endIndex);

            dsResult.ExtendedProperties.Add("endIndex", lastEndIndex);
            return dsResult;
        }


        private static string ReplaceWildcard(string criteria)
        {
            //obsolete
            return criteria;
        }

        private static bool CompareData(string data, string criteria)
        {
            return string.IsNullOrEmpty(criteria) || SearchHelper.IsRegexMatch(data,criteria,".*");
        }

        //Unused method
        //private static bool Reg(string criteria, StringComparison stringComparison)
        //{
        //    throw new NotImplementedException();
        //}

        //public DataSet GetAuditActionSummary(AuditTrailSearchCriteria criteria)
        //{
        //    AuditLog.RetrievingData onRetrievingData = delegate(HandleProviderEventArgs eg)
        //    {
        //        eg.Cancel = !(eg.Provider is ActionStorageProvider);
        //    };

        //    return AuditLog.GetLogSummary(criteria, onRetrievingData);
        //}

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.AuditLogModuleID, FunctionID = FunctionNames.AuditLogViewFunctionID)]
        public Int64[] GetIndexRange(DateTimeCompare timeEntity)
        {
            var da = InstanceBuilder.CreateInstance<AuditLogViewDA>();
            return da.GetIndexRange(timeEntity);
        }


    }
}

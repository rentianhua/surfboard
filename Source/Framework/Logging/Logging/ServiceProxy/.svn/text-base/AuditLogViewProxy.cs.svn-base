#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/He Jiang Yan
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using HiiP.Framework.Logging.Interface.Services;
using HiiP.Framework.Common;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Common.ApplicationContexts.CallHandlers;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Logging.ServiceProxy
{
    public class AuditLogViewProxy : ServiceProxyBase<IAuditLogViewService>, IAuditLogViewService
    {
        protected AuditLogViewProxy(string endPointName)
            : base(endPointName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public AuditLogViewProxy()
        {
            base.WrapObject(new AuditLogViewProxy(EndpointNames.AuditLogViewEndpoint));
        }



        #region IAuditLogViewService Members

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.AuditLogModuleID, FunctionID = FunctionNames.AuditLogViewFunctionID)]
        public AuditLogViewDataSet GetAuditDataSummary(NCS.IConnect.AuditTrail.AuditTrailSearchCriteria criteria)
        {
            return Proxy.GetAuditDataSummary(criteria);
        }

        //public System.Data.DataSet GetAuditActionSummary(NCS.IConnect.AuditTrail.AuditTrailSearchCriteria criteria)
        //{
        //    return Proxy.GetAuditActionSummary(criteria);
        //}

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.AuditLogModuleID, FunctionID = FunctionNames.AuditLogDetailViewFunctionID)]
        public System.Data.DataSet GetLogDetail(string tableName, string logID)
        {
            return Proxy.GetLogDetail(tableName,logID);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.AuditLogModuleID, FunctionID = FunctionNames.AuditLogViewFunctionID)]
        public Int64[] GetIndexRange(DateTimeCompare timeEntity)
        {
            return Proxy.GetIndexRange(timeEntity);
        }

        #endregion
    }
}

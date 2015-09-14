#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Interface
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/He Jiang Yan
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.ServiceModel;
using System.Data;

using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Interface.ValidationEntity;

using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

using NCS.IConnect.AuditTrail;
using NCS.IConnect.Common;

namespace HiiP.Framework.Logging.Interface.Services
{
    /// <summary>
    /// Logging View Service Interface
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface IAuditLogViewService
    {
        /// <summary>
        /// Gets the changed data that were audited
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        AuditLogViewDataSet GetAuditDataSummary(AuditTrailSearchCriteria criteria);

        
        /// <summary>
        /// Gets the detail of audit data
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="logID"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        DataSet GetLogDetail(string tableName, string logID);

        /// <summary>
        /// Gets the min/max value of index of audit data
        /// </summary>
        /// <param name="timeEntity"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        Int64[] GetIndexRange([ObjectValidator("Date Time Compare Set")]DateTimeCompare timeEntity);

    }
}

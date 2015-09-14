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

using System.Data;
using NCS.IConnect.AuditTrail;
using HiiP.Framework.Logging.BusinessEntity;
using System;

namespace HiiP.Framework.Logging
{
    public interface IAuditLogView
    {
        void BindLogsSummary(DataSet data, string dataMember, int percent);
        AuditTrailSearchCriteria CreateCriteria();
        void ProcessRetrieve(AuditTrailSearchCriteria criteria);
        void SetRetrieveState(bool isAsyncWorking);
        void Clear();

        event EventHandler<BatchProcessEventArgs> BatchProcessCompleted;
    }

    public class BatchProcessEventArgs:EventArgs
    {
        public AuditLogViewDataSet BatchData { get; set; }

        public AuditTrailSearchCriteria Criteria { get; set; }
        public BatchProcessEventArgs(AuditLogViewDataSet batchData, AuditTrailSearchCriteria criteria)
        {
            BatchData = batchData;
            Criteria = criteria;
        }
    }
}


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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using HiiP.Infrastructure.Interface;
using System.Data;
using NCS.IConnect.AuditTrail;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Common.Constants;

namespace HiiP.Framework.Logging
{
    public partial class AuditLogDetailViewPresenter : Presenter<IAuditLogDetailView>
    {
        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            base.OnCloseView();
        }

        public override AppTitleData GetAppTitle()
        {
            return new AppTitleData(
                HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogDetailViewFunctionName,
                HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogDetailViewScreenID);
        }
        
        protected override void InitData()
        {

            base.InitData();

            //Load detail data.
            const string TableName = "Detail";

            DataSet data ;
            using (var auditLog = AuditLogViewPresenter.GetAuditLogProxy())
            {
                Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
                using (new MonitoringTracer(id, HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogModuleID, HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogDetailViewFunctionID, ComponentType.Screen))
                {
                    data = auditLog.GetLogDetail(Data.ToString(), Key);
                }
            }
            DataSet detailDs = new DataSet();

            //Convert the rows to columns
            DataTable detailDt = new DataTable(TableName);
            detailDt.Columns.Add("FieldName");
            detailDt.Columns.Add("ChangedBefore");
            detailDt.Columns.Add("ChangedAfter");

            detailDs.Tables.Add(detailDt);

            if (null == data
                || data.Tables.Count <= 0)
            {
                View.LoadDetail(null, string.Empty);
                return;
            }

            var beforeRows = data.Tables[0].Select("IS_NEW_VALUE=0");
            var afterRows = data.Tables[0].Select("IS_NEW_VALUE=1");

            foreach (DataColumn column in data.Tables[0].Columns)
            {
                if (column.ColumnName.Equals("LOG_ID", StringComparison.InvariantCultureIgnoreCase)
                    || column.ColumnName.Equals("IS_NEW_VALUE", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                object objBefore = (beforeRows.Length == 0) ? string.Empty : beforeRows[0][column.ColumnName];
                object objAfter = (afterRows.Length == 0) ? string.Empty : afterRows[0][column.ColumnName];

                if (objBefore is DateTime
                    && (((DateTime)objBefore) <= MinMaxValues.MinDate
                      || ((DateTime)objBefore) >= MinMaxValues.MaxDate))
                {
                    objBefore = string.Empty;
                }
                if (objAfter is DateTime
                    && (((DateTime)objAfter) <= MinMaxValues.MinDate
                      || ((DateTime)objAfter) >= MinMaxValues.MaxDate))
                {
                    objAfter = string.Empty;
                }
                detailDt.Rows.Add(new [] { column.ColumnName,
                objBefore,
                objAfter});
            }

            View.LoadDetail(detailDs, TableName);
        }
    }
}


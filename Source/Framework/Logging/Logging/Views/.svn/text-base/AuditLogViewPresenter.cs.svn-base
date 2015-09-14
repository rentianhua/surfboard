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

using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;
using NCS.IConnect.AuditTrail;

using HiiP.Framework.Logging.ServiceProxy;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Logging.Interface.ValidationEntity;

namespace HiiP.Framework.Logging
{
    public partial class AuditLogViewPresenter : Presenter<IAuditLogView>
    {
        const int Offset = 100;
        private bool _isSyncWorking;
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
                HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogViewFunctionName,
                HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogViewScreenID);
        }

        public void GetSummary(bool start)
        {
            bool tryToStop = !start;
            if (tryToStop)
            {
                SetState(false);
                return;
            }

            View.Clear();

            AuditTrailSearchCriteria criteria = View.CreateCriteria();

            //Get the index range first so that we can know how to end the process.
            Int64[] indexes ;
            Guid id = Utility.SetContextValues();
            DateTimeCompare timeEntity = new DateTimeCompare(criteria.StartDate,
                criteria.EndDate);

            using (new MonitoringTracer(id, HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogModuleID, HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogViewFunctionID, ComponentType.Screen))
            {
                using (var auditLog = AuditLogViewPresenter.GetAuditLogProxy())
                {
                    indexes = auditLog.GetIndexRange(timeEntity);
                }
            }

            criteria.ExtendedProperties["@startIndex"] = indexes[0];
            criteria.ExtendedProperties["@endIndex"] = indexes[0] + Offset * 10;
            criteria.ExtendedProperties["min"] = indexes[0];
            criteria.ExtendedProperties["max"] = indexes[1];

            if (indexes[1] == indexes[0] && indexes[0]==0)
            {
                //No records
                SetState(false);
                return;
            }

            SetState(true);
            _lastEndIndex = indexes[0];

            this.View.BatchProcessCompleted += View_BatchProcessCompleted;
            this.View.ProcessRetrieve(criteria);
        }

        private void SetState(bool start)
        {
            View.SetRetrieveState(start);
            _isSyncWorking = start;
        }

        Int64 _lastEndIndex ;
        void View_BatchProcessCompleted(object sender, BatchProcessEventArgs e)
        {
            bool needStop = !_isSyncWorking;
            if (needStop)
            {
                //Stopped, maybe user clicked the button to stop
                SetState(false);
                return;
            }

            if (null == e.Criteria)
            {
                throw new ArgumentException("The value of e.Criteria is null.");
            }
            if (null == e.BatchData)
            {
                throw new ArgumentException("The value of e.BatchData is null.");
            }

            Int64 startIndex ;
            Int64 endIndex ;
            Int64 min ;
            Int64 max ;
            //Int64 lastIndex = 0;
            Int64 lastEndIndex ;

            //They always will be modified in one thread. That means, not concurrently
            startIndex = (Int64)e.Criteria.ExtendedProperties["@startIndex"];
            endIndex = (Int64)e.Criteria.ExtendedProperties["@endIndex"];
            min = (Int64)e.Criteria.ExtendedProperties["min"];
            max = (Int64)e.Criteria.ExtendedProperties["max"];
            //lastIndex = Int64.Parse(e.BatchData.ExtendedProperties["lastIndex"]);
            //Because it will immediately return when server found anything, we have to check the last end index
            lastEndIndex = Int64.Parse(e.BatchData.ExtendedProperties["endIndex"].ToString());

            if (_lastEndIndex>=lastEndIndex)
            {
                //Why?
                return;
            }

            _lastEndIndex = lastEndIndex;

            e.Criteria.ExtendedProperties["@startIndex"] = lastEndIndex ;
            e.Criteria.ExtendedProperties["@endIndex"] = lastEndIndex + Offset * 10;

            int percent ;

            if (min == max)
            {
                //Only one record
                percent = 97;
            }
            else
            {
                percent = (int)(((lastEndIndex - min)  * 97)/(max - min)) + 3;
            }

            if (startIndex == endIndex)
            {
                //Not found
            }
            else
            {
                this.View.BindLogsSummary(e.BatchData, e.BatchData.T_IC_AUDIT_LOG_QUERY.TableName, (percent>100)?100:percent);
            }

            if (lastEndIndex == 0
                || lastEndIndex >= max
                || percent >= 100)
            {
                //Finished
                SetState(false);
                return;
            }

            //Check again
            needStop = !_isSyncWorking;
            if (needStop)
            {
                //Stopped, maybe user clicked the button to stop
                SetState(false);
                return;
            }

            this.View.ProcessRetrieve(e.Criteria);
        }

        public AuditLogViewDataSet BatchGetSummary(AuditTrailSearchCriteria criteria)
        {
            AuditLogViewDataSet data ;
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogModuleID, HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogViewFunctionID, ComponentType.Screen))
            {
                using (var auditLog = AuditLogViewPresenter.GetAuditLogProxy())
                {
                    data = auditLog.GetAuditDataSummary(criteria);
                    return data;
                }
            }
        }

        internal static AuditLogViewProxy GetAuditLogProxy()
        {
            return new AuditLogViewProxy();

        }

        public void DisplayLogDetail(string logId, string tableName)
        {
            ViewParameter parameter = new ViewParameter();
            parameter.CurrentViewStatus = ViewStatus.View;
            parameter.Key = logId;
            parameter.Data = tableName;
            parameter.AppTitleData = new AppTitleData(
                 string.Format("{0} - {1}", HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogDetailViewFunctionName, logId),
                HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogDetailViewScreenID);
            parameter.ViewId = string.Format("{0} - {1}", HiiP.Framework.Logging.Interface.Constants.FunctionNames.AuditLogDetailViewFunctionName, logId);
            ShowViewInWorkspace<AuditLogDetailView>(parameter);
        }




    }


}


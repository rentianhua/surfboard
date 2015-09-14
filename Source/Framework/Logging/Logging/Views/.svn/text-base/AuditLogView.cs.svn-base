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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.ComponentModel;

using Infragistics.Win.UltraWinGrid;

using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;

using NCS.IConnect.AuditTrail;

using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Common.Client.Async;
using HiiP.Framework.Logging.BusinessEntity;

namespace HiiP.Framework.Logging
{
    public partial class AuditLogView : BaseView, IAuditLogView
    {
        public AuditLogView()
        {
            InitializeComponent();
        }

        const string TotalCountFormat = "Total record(s) : {0}";
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                _presenter.OnViewReady();

                //Set Default Value
                DateTimeStartTime.Value = DateTime.Today;
                DateTimeEndTime.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

                string[] operationTypes = Enum.GetNames(typeof(OperationType));
                List<string> tempOperationTypes = new List<string>();
                string emptyItemDisplayText = CodeTableAdapter.GetEmptyItemDisplayText();
                tempOperationTypes.Add(emptyItemDisplayText);
                tempOperationTypes.AddRange(operationTypes);

                comboOperationName.DataSource = tempOperationTypes;

                comboOperationName.SelectedIndex = 0;
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                this.Enabled = false;
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        public override void ProcessParameter(ViewParameter parameter)
        {
            _presenter.Key = parameter.Key;
            _presenter.CurrentViewStatus = ViewStatus.View;
            AppTitle = _presenter.GetAppTitle();
            base.ProcessParameter(parameter);
        }

        #region Event
        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!IsAsyncWorking && !this.ValidateChildren())
                {
                    return;
                }

                _presenter.GetSummary(!IsAsyncWorking);
                this.GridViewAuditLog.Focus();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void GridViewAuditLog_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (e.Row.Cells == null)
                {
                    return;
                }

                if (e.Row.Cells["Operation"].Value.ToString() != "ActionLog")
                {
                    string logId = e.Row.Cells["LOG_ID"].Value.ToString();
                    string tableName = e.Row.Cells["TABLE_NAME"].Value.ToString();

                    _presenter.DisplayLogDetail(logId, tableName);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.OnCloseView();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.TextboxDevice.Text = string.Empty;
                this.TextboxFunctionName.Text = string.Empty;
                this.TextBoxHostName.Text = string.Empty;
                this.TextboxIPAddress.Text = string.Empty;
                this.TextboxTableName.Text = string.Empty;
                this.TextBoxUseName.Text = string.Empty;

                DateTimeStartTime.Value = DateTime.Today;
                DateTimeEndTime.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

                this.ultraButtonPrint.Enabled = false;

                comboOperationName.SelectedIndex = 0;

                this.GridViewAuditLog.DataSource = new AuditLogViewDataSet();
                this.totalCount.Text = string.Format(TotalCountFormat, GridViewAuditLog.Rows.Count.ToString());
                this.errorProvider1.Clear();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        #endregion

        #region IAuditLogView Members

        public event EventHandler<BatchProcessEventArgs> BatchProcessCompleted;
        public void BindLogsSummary(DataSet data, string dataMember, int percent)
        {
            DataSet ds = this.GridViewAuditLog.DataSource as DataSet;
            if (null == ds)
            {
                ds = data;
            }
            else
            {
                ds.Tables[dataMember].Merge(data.Tables[dataMember]);
            }
            this.GridViewAuditLog.DataSource = ds;
            this.GridViewAuditLog.DataMember = dataMember;
            _ultraProgressBarForSearch.Value = percent;
            this.totalCount.Text = string.Format(TotalCountFormat,(null != ds)?ds.Tables[dataMember].Rows.Count.ToString():"0");

        }

        public AuditTrailSearchCriteria CreateCriteria()
        {
            string transId = string.Empty;
            string appVersion = string.Empty;
            AuditTrailSearchCriteria criteria = new AuditTrailSearchCriteria(
                TextboxFunctionName.Text.Trim(),
                transId,
                TextBoxUseName.Text.Trim(),
                appVersion,
                DateTimeStartTime.DateTime,
                DateTimeEndTime.DateTime);

            string emptyItemDisplayText = CodeTableAdapter.GetEmptyItemDisplayText();
            //Add the extesion fields
            criteria.ExtendedProperties = new System.Collections.Generic.Dictionary<string, object>();
            criteria.ExtendedProperties.Add("@tableName", TextboxTableName.Text.Trim());
            criteria.ExtendedProperties.Add("@operationType", (comboOperationName.Text.Equals(emptyItemDisplayText,StringComparison.InvariantCultureIgnoreCase)?string.Empty:comboOperationName.Text));
            criteria.ExtendedProperties.Add("@hostName", TextBoxHostName.Text.Trim());
            criteria.ExtendedProperties.Add("@ipAddress", TextboxIPAddress.Text.Trim());
            criteria.ExtendedProperties.Add("@device",  TextboxDevice.Text.Trim());
            criteria.ExtendedProperties.Add("@startIndex", 0);
            criteria.ExtendedProperties.Add("@endIndex", 0);
            criteria.ExtendedProperties.Add("minId", 0);
            criteria.ExtendedProperties.Add("maxId", 0);
            return criteria;
        }

        public void ProcessRetrieve(AuditTrailSearchCriteria criteria)
        {
            using (AsyncWorkerByTrunk<IAuditLogView> worker = new AsyncWorkerByTrunk<IAuditLogView>(_presenter, this.GridViewAuditLog, new Control[] { ButtonClear }, false))
            {
                worker.BackgroundWorker.WorkerSupportsCancellation = true;
                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    eDoWork.Result = _presenter.BatchGetSummary(criteria);
                    Thread.Sleep(100);
                };
                worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                {
                    AuditLogViewDataSet ds = eCompleted.Result as AuditLogViewDataSet;

                    if (null != BatchProcessCompleted && null!=ds)
                    {
                        BatchProcessCompleted(this, new BatchProcessEventArgs(ds, criteria));
                        worker.BackgroundWorker.CancelAsync();
                    }
                    else
                    {
                        //try to stop
                        _presenter.GetSummary(false);
                    }
                };
                worker.Run();
            }
        }

        public void SetRetrieveState(bool isAsyncWorking)
        {
            IsAsyncWorking = isAsyncWorking;

            //As request, it cannot be visible, so comment the following code
            //this._ultraProgressBarForSearch.Visible = changedToAsyncWork;

            if (!isAsyncWorking)
            {
                _presenter.OnUpdateProgressBar(ProgressBarStatus.OnEnd);
                ProgressCounter--;
                this.ButtonSearch.Text = "&Search";
                this.Cursor = Cursors.Default;

                this.ultraButtonPrint.Enabled = this.GridViewAuditLog.Rows.Count > 0;
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.OnUpdateProgressBar(ProgressBarStatus.OnProcess);
                ProgressCounter++;
                this.ButtonSearch.Text = "&Stop";

                this._ultraProgressBarForSearch.Value = 3;
                this.ultraButtonPrint.Enabled = false;

            }
        }

        public void Clear()
        {
            var ds = new AuditLogViewDataSet();
            this.GridViewAuditLog.DataSource = ds;
            this.GridViewAuditLog.DataMember = ds.T_IC_AUDIT_LOG_QUERY.TableName;
            this.totalCount.Text = string.Format(TotalCountFormat,"0");
        }


        #endregion

        private void GridViewAuditLog_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (e.KeyData == Keys.Enter || e.KeyData == Keys.Space)
                {
                    UltraGridRow row = this.GridViewAuditLog.ActiveRow;

                    if (row != null)
                    {
                        if (row.Cells == null)
                        {
                            return;
                        }

                        if (row.Cells["Operation"].Value.ToString() != "ActionLog")
                        {
                            string logId = row.Cells["LOG_ID"].Value.ToString();
                            string tableName = row.Cells["TABLE_NAME"].Value.ToString();
                            _presenter.DisplayLogDetail(logId, tableName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ultraButtonPrint_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.GridViewAuditLog.Rows.Count <= 0)
                {
                    //will never go here
                    return;
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
                    ultraGridPrintDocument.DefaultPageSettings.Margins.Top = 12;
                    ultraGridPrintDocument.DefaultPageSettings.Margins.Left = 12;
                    ultraGridPrintDocument.DefaultPageSettings.Margins.Right = 25;
                    ultraGridPrintDocument.DefaultPageSettings.Margins.Bottom = 25;
                    ultraGridPrintDocument.OriginAtMargins = true;
                    ultraGridPrintDocument.RowProperties = RowPropertyCategories.Hidden;
                    this.GridViewAuditLog.PrintPreview(this.GridViewAuditLog.DisplayLayout, this.ultraGridPrintDocument);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

    }
}


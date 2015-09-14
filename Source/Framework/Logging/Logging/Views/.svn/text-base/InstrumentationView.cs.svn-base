#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Yang Jian Hua
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using NCS.IConnect.ExceptionHandling;
using NCS.IConnect.Common;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Common;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Messaging;
using HiiP.Infrastructure.Interface.BusinessEntities;
using System.ComponentModel;
using HiiP.Framework.Common.Client.Async;
using HiiP.Framework.Common.Client;
using System.Threading;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using System.Data;
using Infragistics.Win.UltraWinGrid;

namespace HiiP.Framework.Logging
{
    public partial class InstrumentationView : BaseView, IInstrumentationView
    {
        public InstrumentationView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.OnViewReady();
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


        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidateChildren())
                {
                    this.Cursor = Cursors.WaitCursor;

                    switch (this.ButtonSearch.Text)
                    {
                        case "&Search":
                            // Step 1. Return Min LOG_ID and Max LOG_ID
                            DateTimeCompare timepair = new DateTimeCompare(DateTimeStartDate.DateTime, DateTimeEndDate.DateTime);
                            PrepareShowInstrumentationList(timepair);
                            this.GridViewInstrumentation.Focus();
                            break;
                        case "&Stop":
                            IsAsyncWorking = false;
                            this.ButtonSearch.Text = "&Search";
                            _presenter.OnUpdateProgressBar(ProgressBarStatus.OnEnd);
                            ProgressCounter--;
                            break;
                    }



                }
            }
            catch (Exception ex)
            {
                _presenter.OnUpdateProgressBar(ProgressBarStatus.OnEnd);
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #region

        private void PrepareShowInstrumentationList(DateTimeCompare timepair)
        {
            using (AsyncWorkerByTrunk<IInstrumentationView> worker = new AsyncWorkerByTrunk<IInstrumentationView>(_presenter, this.GridViewInstrumentation, new Control[] { ButtonSearch, ButtonClear }, true))
            {
                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    eDoWork.Result = _presenter.GetLogIDRangeByLogTime(timepair);
                };
                worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                {
                    LogIDPairEntity logIDPairEntity = eCompleted.Result as LogIDPairEntity;

                    GridViewInstrumentation.DataSource = new LoggingViewDataSet();
                    this.totalCount.Text = GridViewInstrumentation.Rows.Count.ToString();

                    if (logIDPairEntity != null && logIDPairEntity.MinLogID > 0)
                    {
                        // Step 1. Return Min LOG_ID and Max LOG_ID

                        _presenter.OnUpdateProgressBar(ProgressBarStatus.OnProcess);
                        ProgressCounter++;
                        this.ButtonSearch.Text = "&Stop";
                        IsAsyncWorking = true;

                        // Step 2. Recursive loading data according to LOG_ID and LOG_ID
                        InstrumentationSearchCondition condition = new InstrumentationSearchCondition();
                        condition.UserName = TextBoxUserName.Text;
                        condition.IpAddress = TextBoxIpAddress.Text;
                        condition.ModuleId = TextBoxModuleId.Text;
                        condition.FunctionId = TextBoxFunctionId.Text;
                        condition.ComponentName = ComboxComponentName.Value.ToString();
                        condition.PCName = PCName.Text;
                        ShowInstrumentationLogList(
                            new LoggingViewDataSet(),
                            logIDPairEntity,timepair,
                            condition);
                    }
                };
                worker.Run();
            }
        }

        private void ShowInstrumentationLogList(
            LoggingViewDataSet loggingViewData,
            LogIDPairEntity logIDPair,DateTimeCompare timeEntity,
            InstrumentationSearchCondition condition)
        {
            using (AsyncWorkerByTrunk<IInstrumentationView> worker = new AsyncWorkerByTrunk<IInstrumentationView>(_presenter, this.GridViewInstrumentation, new Control[] { ButtonClear }, true))
            {
                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    eDoWork.Result = _presenter.GetInstrumentationData(
                                logIDPair,timeEntity,
                                condition);
                    Thread.Sleep(300); // default 300
                };
                worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                {
                    if (WorkItem.SmartParts[ViewId] == null || !IsAsyncWorking)
                    {
                        this.ButtonSearch.Text = "&Search";
                        return;
                    }

                    // ExceptionViewData exceptionViewData = eCompleted.Result as ExceptionViewData;
                    LoggingViewDataSet tempLoggingViewData = eCompleted.Result as LoggingViewDataSet;
                    if (tempLoggingViewData == null) tempLoggingViewData = new LoggingViewDataSet();
                    loggingViewData.Merge(tempLoggingViewData);
                    GridViewInstrumentation.DataSource = loggingViewData;
                    this.totalCount.Text = GridViewInstrumentation.Rows.Count.ToString();

                    string[] logIDPairArray = tempLoggingViewData.ExtendedProperties["LogIDPair"].ToString().Split(',');
                    Int64 minLogID = Convert.ToInt64(logIDPairArray[0]);
                    Int64 maxLogID = Convert.ToInt64(logIDPairArray[1]);

                    if (minLogID <= maxLogID)
                    {
                        ShowInstrumentationLogList(
                            loggingViewData,
                            new LogIDPairEntity(minLogID, maxLogID),timeEntity,
                            condition);
                    }
                    else
                    {
                        IsAsyncWorking = false;
                        this.ButtonSearch.Text = "&Search";
                        _presenter.OnUpdateProgressBar(ProgressBarStatus.OnEnd);
                        ProgressCounter--;

                        AsyncWaiting(500);
                    }
                };
                worker.Run();
            }
        }

        private void AsyncWaiting(int waitTime)
        {
            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += delegate
                {
                    System.Threading.Thread.Sleep(waitTime);
                };
                worker.RunWorkerCompleted += delegate
                {
                };
                worker.RunWorkerAsync();
            }
        }

        #endregion

        private void InstrumentationView_Load(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                BindCombox();
                SetDefaultValue();
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
                this.DateTimeStartDate.Value = DateTime.Now.AddDays(-1).AddSeconds(1);
                this.DateTimeEndDate.Value = DateTime.Now;
                this.TextBoxUserName.Text = string.Empty;
                this.TextBoxIpAddress.Text = string.Empty;
                this.TextBoxModuleId.Text = string.Empty;
                this.TextBoxFunctionId.Text = string.Empty;
                this.PCName.Text = string.Empty;
                this.ComboxComponentName.SelectedIndex = 0;
                GridViewInstrumentation.DataSource = new LoggingViewDataSet();
                this.totalCount.Text = GridViewInstrumentation.Rows.Count.ToString();
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

        private void BindCombox()
        {
            string emptyItemDisplayText = CodeTableAdapter.GetEmptyItemDisplayText();
            ComboxComponentName.Items.Add("", emptyItemDisplayText);
            ComboxComponentName.Items.Add(ComponentType.Screen, ComponentType.Screen.ToString());
            ComboxComponentName.Items.Add(ComponentType.ServiceProxy, ComponentType.ServiceProxy.ToString());
            ComboxComponentName.Items.Add(ComponentType.BusinessService, ComponentType.BusinessService.ToString());
            ComboxComponentName.Items.Add(ComponentType.BusinessComponent, ComponentType.BusinessComponent.ToString());
            ComboxComponentName.Items.Add(ComponentType.DataAccess, ComponentType.DataAccess.ToString());
            ComboxComponentName.Items.Add(ComponentType.StoredProcedure, ComponentType.StoredProcedure.ToString());
            ComboxComponentName.Items.Add(ComponentType.BatchJob, ComponentType.BatchJob.ToString());
            ComboxComponentName.Items.Add(ComponentType.ExternalSystem, ComponentType.ExternalSystem.ToString());
            ComboxComponentName.Items.Add(ComponentType.Report, ComponentType.Report.ToString());
            ComboxComponentName.SelectedIndex = 0;
        }

        private void SetDefaultValue()
        {
            this.DateTimeStartDate.Value = DateTime.Now.AddDays(-1).AddSeconds(1);
            this.DateTimeEndDate.Value = DateTime.Now;
        }

        public override void ProcessParameter(ViewParameter parameter)
        {
            AppTitle = new AppTitleData(FunctionNames.InstrumentationFunctionName,
                FunctionNames.InstrumentationFunctionName, FunctionNames.InstrumentationFunctionScreenID);
            base.ProcessParameter(parameter);
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
        private void GridViewInstrumentation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (e.KeyData == Keys.Enter || e.KeyData == Keys.Space)
                {
                    UltraGridRow row = this.GridViewInstrumentation.ActiveRow;

                    if (row != null)
                    {

                        if (row.Cells == null)
                        {
                            return;
                        }
                        DataRowView rowView = (DataRowView)row.ListObject;
                        _presenter.ShowInstrumentationDetailView((LoggingViewDataSet.T_IC_LOGGING_LOGRow)rowView.Row);
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

        private void GridViewInstrumentation_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (e.Row.Cells == null)
                {
                    return;
                }
                DataRowView rowView = (DataRowView)e.Row.ListObject;
                _presenter.ShowInstrumentationDetailView((LoggingViewDataSet.T_IC_LOGGING_LOGRow)rowView.Row);
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


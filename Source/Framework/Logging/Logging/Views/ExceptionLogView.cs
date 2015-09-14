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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using HiiP.Framework.Common;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Infrastructure.Interface.BusinessEntities;
using System.ComponentModel;
using Infragistics.Win.UltraWinGrid;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using HiiP.Framework.Common.Client.Async;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Interface.ValidationEntity;

namespace HiiP.Framework.ExceptionHandling
{
    public partial class ExceptionLogView : BaseView, IExceptionLogView
    {
        public ExceptionLogView()
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
                            this._ultraProgressBarForSearch.Value = 0;
                            this._ultraProgressBarForSearch.Visible = false;  // Only open here.
                            DateTimeCompare timepair = new DateTimeCompare(DateTimeStartTime.DateTime, DateTimeEndTime.DateTime);
                            PrepareShowExceptionList(timepair,
                                TextBoxUseName.Text,
                            TextBoxMachineName.Text);
                            this.GridViewExceptionLog.Focus();
                            break;
                        case "&Stop":
                            IsAsyncWorking = false;
                            this.ButtonSearch.Text = "&Search";
                            _presenter.OnUpdateProgressBar(ProgressBarStatus.OnEnd);
                            ProgressCounter--;
                            this._ultraProgressBarForSearch.Visible = false;
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

        private void PrepareShowExceptionList(DateTimeCompare timepair, string userName, string machineName)
        {
            string instanceID = TextBoxInstanceID.Text.Trim();

            if (!string.IsNullOrEmpty(instanceID))
            {
                using (AsyncWorkerByTrunk<IExceptionLogView> worker = new AsyncWorkerByTrunk<IExceptionLogView>(_presenter, this.GridViewExceptionLog, new Control[] { ButtonSearch, ButtonClear }, true))
                {
                    worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                    {
                        eDoWork.Result = _presenter.GetExceptionLog(
                                new LogIDPairEntity(),
                                null,
                                null,
                                null,
                                null,
                                null,
                                instanceID);
                    };
                    worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                    {
                        LoggingViewDataSet loggingViewData = eCompleted.Result as LoggingViewDataSet;
                        GridViewExceptionLog.DataSource = loggingViewData;
                        this.totalCount.Text = GridViewExceptionLog.Rows.Count.ToString();
                    };
                    worker.Run();
                }
            }
            else
            {
                using (AsyncWorkerByTrunk<IExceptionLogView> worker = new AsyncWorkerByTrunk<IExceptionLogView>(_presenter, this.GridViewExceptionLog, new Control[] { ButtonSearch, ButtonClear }, true))
                {
                    worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                    {
                        eDoWork.Result = _presenter.GetLogIDRangeByLogTime(timepair, userName, machineName);
                    };
                    worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                    {
                        LogIDPairEntity logIDPairEntity = eCompleted.Result as LogIDPairEntity;

                        GridViewExceptionLog.DataSource = new LoggingViewDataSet();
                        this.totalCount.Text = GridViewExceptionLog.Rows.Count.ToString();

                        if (logIDPairEntity != null && logIDPairEntity.MinLogID > 0)
                        {
                            // Step 1. Return Min LOG_ID and Max LOG_ID

                            totalMaxLogID = logIDPairEntity.MaxLogID;
                            this._ultraProgressBarForSearch.Value = 3;  // only prepare for loading data, initial progress is 3%

                            _presenter.OnUpdateProgressBar(ProgressBarStatus.OnProcess);
                            ProgressCounter++;
                            this.ButtonSearch.Text = "&Stop";
                            IsAsyncWorking = true;

                            // Step 2. Recursive loading data according to LOG_ID and LOG_ID
                            ShowExceptionLogList(
                                new LoggingViewDataSet(),
                                logIDPairEntity,
                                timepair,
                                TextBoxUseName.Text,
                                ComboxSeverity.Value.ToString(),
                                TextBoxMachineName.Text,
                                TextBoxLogContent.Text,
                                TextBoxInstanceID.Text.Trim());
                        }
                        else
                        {
                            this._ultraProgressBarForSearch.Visible = false;
                        }
                    };
                    worker.Run();
                }
            }
        }

        private void ShowExceptionLogList(
            LoggingViewDataSet loggingViewData,
            LogIDPairEntity logIDPair,
            DateTimeCompare timeEntity,
            string userName,
            string severity,
            string machineName,
            string logContent,
            string instanceID)
        {
            using (AsyncWorkerByTrunk<IExceptionLogView> worker = new AsyncWorkerByTrunk<IExceptionLogView>(_presenter, this.GridViewExceptionLog, new Control[] { ButtonClear }, true))
            {
                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    eDoWork.Result = _presenter.GetExceptionLog(
                                logIDPair,
                                timeEntity,
                                userName,
                                severity,
                                machineName,
                                logContent,
                                instanceID);
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
                    LoggingViewDataSet LoggingViewData = eCompleted.Result as LoggingViewDataSet;
                    if (LoggingViewData == null) LoggingViewData = new LoggingViewDataSet();
                    loggingViewData.Merge(LoggingViewData);
                    GridViewExceptionLog.DataSource = loggingViewData;
                    this.totalCount.Text = GridViewExceptionLog.Rows.Count.ToString();

                    string[] logIDPairArray = LoggingViewData.ExtendedProperties["LogIDPair"].ToString().Split(',');
                    Int64 minLogID = Convert.ToInt64(logIDPairArray[0]);
                    Int64 maxLogID = Convert.ToInt64(logIDPairArray[1]);

                    double remain = maxLogID - minLogID;
                    double total = totalMaxLogID - minLogID;
                    double remains = (1 - remain / total) * 97 + 3;

                    if (remains > 0 && remains <= 100)
                    {
                        this._ultraProgressBarForSearch.Value = Convert.ToInt32(remains);
                    }

                    if (minLogID <= maxLogID)
                    {
                        ShowExceptionLogList(
                            loggingViewData,
                            new LogIDPairEntity(minLogID, maxLogID),
                            timeEntity, 
                            TextBoxUseName.Text,
                            ComboxSeverity.Value.ToString(),
                            TextBoxMachineName.Text,
                            TextBoxLogContent.Text,
                            TextBoxInstanceID.Text.Trim());
                    }
                    else
                    {
                        IsAsyncWorking = false;
                        this.ButtonSearch.Text = "&Search";
                        _presenter.OnUpdateProgressBar(ProgressBarStatus.OnEnd);
                        ProgressCounter--;

                        this._ultraProgressBarForSearch.Value = 100;

                        AsyncWaiting(500);
                    }
                };
                worker.Run();
            }
        }

        private Int64 totalMaxLogID ;

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
                    this._ultraProgressBarForSearch.Visible = false;
                };
                worker.RunWorkerAsync();
            }
        }

        //private class ExceptionViewData
        //{
        //    public LoggingViewDataSet Data
        //    {
        //        get;
        //        set;
        //    }

        //    public Int64 MinID
        //    {
        //        get;
        //        set;
        //    }

        //    public Int64 MaxID
        //    {
        //        get;
        //        set;
        //    }
        //}

        //private ExceptionLogSearchCondition GetSearchCondition()
        //{
        //    Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
        //    ExceptionLogSearchCondition condition = new ExceptionLogSearchCondition();
        //    condition.MinLogIDFromClient = 0;
        //    condition.MaxLogIDFromClient = 0;
        //    condition.UserName = TextBoxUseName.Text;
        //    condition.Severity = ComboxSeverity.Value.ToString();
        //    condition.MachineName = TextBoxMachineName.Text;
        //    condition.LogContent = TextBoxLogContent.Text;

        //    return condition;
        //}

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.DateTimeStartTime.Value = DateTime.Now.AddDays(-1);
                this.DateTimeEndTime.Value = DateTime.Now;
                this.TextBoxUseName.Text = string.Empty;
                this.ComboxSeverity.SelectedIndex = 0;
                this.TextBoxMachineName.Text = string.Empty;
                this.TextBoxLogContent.Text = string.Empty;
                this.TextBoxInstanceID.Text = string.Empty;
                GridViewExceptionLog.DataSource = new LoggingViewDataSet();
                this.totalCount.Text = GridViewExceptionLog.Rows.Count.ToString();
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

        private void ExceptionLogView_Load(object sender, EventArgs e)
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

        private void BindCombox()
        {
            string emptyItemDisplayText = CodeTableAdapter.GetEmptyItemDisplayText();
            ComboxSeverity.Items.Add("", emptyItemDisplayText);
            ComboxSeverity.Items.Add("Information", "Information");
            ComboxSeverity.Items.Add("Warning", "Warning");
            ComboxSeverity.Items.Add("Error", "Error");
            ComboxSeverity.Items.Add("Critical", "Critical");
            ComboxSeverity.SelectedIndex = 0;
        }

        private void SetDefaultValue()
        {
            this.DateTimeStartTime.Value = DateTime.Now.AddDays(-1);
            this.DateTimeEndTime.Value = DateTime.Now;
        }

        public override void ProcessParameter(ViewParameter parameter)
        {
            AppTitle = new AppTitleData(FunctionNames.ExceptionLogViewFunctionName,
                FunctionNames.ExceptionLogViewFunctionName, FunctionNames.ExceptionLogViewScreenID);
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

        private void GridViewExceptionLog_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (e.Row.Cells == null)
                {
                    return;
                }

                string logID = e.Row.Cells["LOG_ID"].Value.ToString();
                _presenter.ShowExceptionDetailView(logID);
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

        private void GridViewExceptionLog_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (e.KeyData == Keys.Enter || e.KeyData == Keys.Space)
                {
                    UltraGridRow row = this.GridViewExceptionLog.ActiveRow;

                    if (row != null)
                    {

                        if (row.Cells == null)
                        {
                            return;
                        }

                        string logID = row.Cells["LOG_ID"].Value.ToString();
                        _presenter.ShowExceptionDetailView(logID);
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
    }
}


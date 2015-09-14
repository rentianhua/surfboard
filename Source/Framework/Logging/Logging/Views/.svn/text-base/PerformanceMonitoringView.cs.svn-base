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
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Common.Client.Async;
using System.ComponentModel;
using HiiP.Framework.Logging.Interface.ValidationEntity;

namespace HiiP.Framework.Logging
{
    public partial class PerformanceMonitoringView : BaseView, IPerformanceMonitoringView
    {
        public PerformanceMonitoringView()
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

        private void PerformanceMonitoringView_Load(object sender, EventArgs e)
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

        #region IPerformanceMonitroingView Members

        public LoggingViewDataSet LoggingViewData
        {
            get;
            set;
        }

        #endregion

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.ValidateChildren())
                {
                    // Initialize data grid 
                    LoggingViewData = new LoggingViewDataSet();
                    GridViewPerformance.DataSource = LoggingViewData.T_IC_PERFORMANCE_LOG;
                    this.totalCount.Text = GridViewPerformance.Rows.Count.ToString();
                    this.GridViewPerformance.Focus();

                    DateTimeCompare timeEntity = new DateTimeCompare(DateTimeStartDate.DateTime,
                        DateTimeEndDate.DateTime);
                    var argus = new object[] { timeEntity,
                        TextBoxFunctionId.Text,
                        ComboxComponentName.Value.ToString(),
                        this.TextBoxUserName.Text };

                    using (AsyncWorkerByTrunk<IPerformanceMonitoringView> worker = new AsyncWorkerByTrunk<IPerformanceMonitoringView>(_presenter, this.GridViewPerformance, new Control[] { ButtonSearch, ButtonClear }))
                    {
                        #region worker definition
                        worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                        {
                            var tempArgus = eDoWork.Argument as object[];
                            if (tempArgus==null || tempArgus.Length<=3)
                            {
                                return;
                            }
                            eDoWork.Result = _presenter.GetPerformanceData(
                                tempArgus[0] as DateTimeCompare, 
                                tempArgus[1] as string, 
                                tempArgus[2] as string, tempArgus[3] as string);
                        };
                        worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                        {
                            LoggingViewData = eCompleted.Result as LoggingViewDataSet;
                            if (null==LoggingViewData)
                            {
                                LoggingViewData = new LoggingViewDataSet();
                            }
                            GridViewPerformance.DataSource = LoggingViewData.T_IC_PERFORMANCE_LOG;
                            this.totalCount.Text = GridViewPerformance.Rows.Count.ToString();
                        };
                        #endregion
                        worker.Run(argus);
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

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.DateTimeStartDate.Value = DateTime.Now.AddDays(-1);
                this.DateTimeEndDate.Value = DateTime.Now;
                this.TextBoxFunctionId.Text = string.Empty;
                this.TextBoxUserName.Text = string.Empty;
                this.ComboxComponentName.SelectedIndex = 0;
                GridViewPerformance.DataSource = (new LoggingViewDataSet()).T_IC_PERFORMANCE_LOG;
                this.totalCount.Text = GridViewPerformance.Rows.Count.ToString();
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
            this.DateTimeStartDate.Value = DateTime.Now.AddDays(-1);
            this.DateTimeEndDate.Value = DateTime.Now;
        }

        public override void ProcessParameter(ViewParameter parameter)
        {
            AppTitle = new AppTitleData(FunctionNames.MonitoringFunctionName,
                FunctionNames.MonitoringFunctionName, FunctionNames.MonitoringFunctionScreenID);
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
    }
}


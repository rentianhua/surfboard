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
// 10/02/2009/YJH                                              Defect #1359                          Set the interval value to 1 when the max data value < 6 to avoid the decimal fraction       
// ======================================================================================================================

#endregion

using System;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Common.Client.Async;
using System.ComponentModel;
using HiiP.Framework.Logging.BusinessEntity;
using System.Linq;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using Infragistics.Win.UltraWinChart;
using System.Collections.Generic;
using System.Data;
using Infragistics.UltraChart.Shared.Styles;

namespace HiiP.Framework.Logging
{
    public partial class UsageLogView : BaseView, IUsageLogView
    {
        private const int CHART_BASE_HEIGHT = 90;
        private const int EXPANDABLE_GROUP_BOX_BASE_HEIGHT = 185;
        private const int INCREMENT_FEED = 60;

        #region Initialize usage log view

        public UsageLogView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                _presenter.OnViewReady();
                base.OnLoad(e);
                InitializeUsageLogView();
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

        public override void ProcessParameter(HiiP.Infrastructure.Interface.BusinessEntities.ViewParameter parameter)
        {
            AppTitle = new AppTitleData(
                FunctionNames.UsageFunctionName,
                FunctionNames.UsageFunctionName,
                FunctionNames.UsageFunctionScreenID);

            base.ProcessParameter(parameter);
        }

        private void InitializeUsageLogView()
        {
            this.InitializeUsageByUser();
            this.InitializeUsageByRole();
            this.InitializeUsageByOffice();
            this.InitializeUsageByModule();
            this.InitializeUsageByFunction();
            this.InitializeUsageByUserCount();
            this.InitializeUsageByUserCountPerModule();
        }

        private void InitializeUsageByUser()
        {
            this.ultraChart1.Visible = false;
            this.usageByUserExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;

            this.TextBoxUserName.Text = string.Empty;
            this.DateTimeStartDateForUser.Value = DateTime.Now.Date.AddDays(-2);
            this.DateTimeEndDateForUser.Value = DateTime.Now.Date.AddDays(-1);
            this.errorProvider1.SetError(this.DateTimeStartDateForUser, null);
            this.errorProvider1.SetError(this.DateTimeEndDateForUser, null);
        }

        private void InitializeUsageByRole()
        {
            this.ultraChart2.Visible = false;
            this.usageByRoleExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;

            this.TextBoxRoleId.Text = string.Empty;
            this.DateTimeStartDateForRole.Value = DateTime.Now.Date.AddDays(-2);
            this.DateTimeEndDateForRole.Value = DateTime.Now.Date.AddDays(-1);
            this.errorProvider1.SetError(this.DateTimeStartDateForRole, null);
            this.errorProvider1.SetError(this.DateTimeEndDateForRole, null);
        }

        private void InitializeUsageByOffice()
        {
            this.ultraChart3.Visible = false;
            this.usageByOfficeExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;

            this.TextBoxOfficeName.Text = string.Empty;
            this.DateTimeStartDateForOffice.Value = DateTime.Now.Date.AddDays(-2);
            this.DateTimeEndDateForOffice.Value = DateTime.Now.Date.AddDays(-1);
            this.errorProvider1.SetError(this.DateTimeStartDateForOffice, null);
            this.errorProvider1.SetError(this.DateTimeEndDateForOffice, null);
        }

        private void InitializeUsageByModule()
        {
            this.ultraChart5.Visible = false;
            this.usageByModuleExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;

            this.TextBoxModuleId.Text = string.Empty;
            this.DateTimeStartDateForModule.Value = DateTime.Now.Date.AddDays(-2);
            this.DateTimeEndDateForModule.Value = DateTime.Now.Date.AddDays(-1);
            this.errorProvider1.SetError(this.DateTimeStartDateForModule, null);
            this.errorProvider1.SetError(this.DateTimeEndDateForModule, null);
        }

        private void InitializeUsageByFunction()
        {
            this.ultraChart6.Visible = false;
            this.usageByFunctionExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;

            this.TextBoxFunctionId.Text = string.Empty;
            this.DateTimeStartDateForFunction.Value = DateTime.Now.Date.AddDays(-2);
            this.DateTimeEndDateForFunction.Value = DateTime.Now.Date.AddDays(-1);
            this.errorProvider1.SetError(this.DateTimeStartDateForFunction, null);
            this.errorProvider1.SetError(this.DateTimeEndDateForFunction, null);
        }

        private void InitializeUsageByUserCount()
        {
            this.ultraChart7.Visible = false;
            this.usageByUserCountExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;

            this.DateTimeStartDateForCount.Value = DateTime.Now.Date.AddDays(-2);
            this.DateTimeEndDateForCount.Value = DateTime.Now.Date.AddDays(-1);
            this.errorProvider1.SetError(this.DateTimeStartDateForCount, null);
            this.errorProvider1.SetError(this.DateTimeEndDateForCount, null);
        }

        private void InitializeUsageByUserCountPerModule()
        {
            this.ultraChart8.Visible = false;
            this.usageByUserCountPerModuleExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;

            this.TextBoxModuleIdForCount.Text = string.Empty;
            this.DateTimeStartDateForCountPerModule.Value = DateTime.Now.Date.AddDays(-2);
            this.DateTimeEndDateForCountPerModule.Value = DateTime.Now.Date.AddDays(-1);
            this.errorProvider1.SetError(this.DateTimeStartDateForCountPerModule, null);
            this.errorProvider1.SetError(this.DateTimeEndDateForCountPerModule, null);
        }

        #endregion

        #region View report

        private void ButtonSearchForUser_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DisableValidationProvider();
                this.validationProvider1.Enabled = true;
                if (this.ValidateChildren())
                {
                    DateTimeCompare timeEntity = new DateTimeCompare(DateTimeStartDateForUser.DateTime.Date,
                        DateTimeEndDateForUser.DateTime.Date);
                    var argus = new object[] {timeEntity, TextBoxUserName.Text};

                    using (AsyncWorker<IUsageLogView> worker = new AsyncWorker<IUsageLogView>(_presenter, this.ultraChart1, new Control[] { ButtonSearchForUser }))
                    {
                        #region worker definition
                        worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                        {
                            var tempArgus = eDoWork.Argument as object[];

                            if (tempArgus==null || tempArgus.Length<=1)
                            {
                                return;
                            }
                            eDoWork.Result = _presenter.GetUsageForUserData(
                                tempArgus[0] as DateTimeCompare,tempArgus[1] as string );
                        };
                        worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                        {
                            LoggingUsageDataSet dataset = eCompleted.Result as LoggingUsageDataSet;
                            if (dataset != null && dataset.T_IC_LOGGING_USAGE.Rows.Count > 0)
                            {
                                int rowCount = dataset.T_IC_LOGGING_USAGE.Rows.Count - 1;
                                this.ultraChart1.Height = CHART_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                this.usageByUserExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                SetChartStype(this.ultraChart1, "FREQUENCY", dataset);
                                this.ultraChart1.DataSource = dataset.T_IC_LOGGING_USAGE.Select(
                                    data => new
                                    {
                                        username = data["USER_NAME"].ToString(),
                                        logintimes = Convert.ToInt32(data["FREQUENCY"])
                                    }
                                    ).ToList();
                                this.ultraChart1.Visible = true;
                            }
                            else
                            {
                                this.ultraChart1.Visible = false;
                                this.usageByUserExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;
                            }
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

        private void ButtonSearchForRole_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DisableValidationProvider();
                this.validationProvider2.Enabled = true;
                if (this.ValidateChildren())
                {
                    DateTimeCompare timeEntity = new DateTimeCompare(
                        DateTimeStartDateForRole.DateTime.Date, DateTimeEndDateForRole.DateTime.Date);
                    var argus = new object[] { timeEntity, TextBoxRoleId.Text };
                    using (AsyncWorker<IUsageLogView> worker = new AsyncWorker<IUsageLogView>(_presenter, this.ultraChart2, new Control[] { ButtonSearchForRole }))
                    {
                        #region worker definition
                        worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                        {
                            var tempArgus = eDoWork.Argument as object[];

                            if (tempArgus==null || tempArgus.Length <= 1)
                            {
                                return;
                            }
                            eDoWork.Result = _presenter.GetUsageForRoleData(
                                tempArgus[0] as DateTimeCompare, tempArgus[1] as string);
                        };
                        worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                        {
                            LoggingUsageDataSet dataset = eCompleted.Result as LoggingUsageDataSet;
                            if (dataset != null && dataset.T_IC_LOGGING_USAGE.Rows.Count > 0)
                            {
                                int rowCount = dataset.T_IC_LOGGING_USAGE.Rows.Count - 1;
                                this.ultraChart2.Height = CHART_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                this.usageByRoleExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                SetChartStype(this.ultraChart2, "FREQUENCY", dataset);
                                this.ultraChart2.DataSource = dataset.T_IC_LOGGING_USAGE.Select(
                                    data => new
                                    {
                                        roleid = data["USER_ROLES"].ToString(),
                                        logintimes = Convert.ToInt32(data["FREQUENCY"])
                                    }
                                    ).ToList();
                                this.ultraChart2.Visible = true;
                            }
                            else
                            {
                                this.ultraChart2.Visible = false;
                                this.usageByRoleExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;
                            }
                        };
                        #endregion 
                        worker.Run(argus);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ButtonSearchForOffice_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DisableValidationProvider();
                this.validationProvider3.Enabled = true;
                if (this.ValidateChildren())
                {
                    DateTimeCompare timeEntity = new DateTimeCompare(
                        DateTimeStartDateForOffice.DateTime.Date, DateTimeEndDateForOffice.DateTime.Date);
                    var argus = new object[] { timeEntity, TextBoxOfficeName.Text };
                    using (AsyncWorker<IUsageLogView> worker = new AsyncWorker<IUsageLogView>(_presenter, this.ultraChart3, new Control[] { ButtonSearchForOffice }))
                    {
                        #region worker definition
                        worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                        {
                            var tempArgus = eDoWork.Argument as object[];

                            if (tempArgus == null || tempArgus.Length <= 1)
                            {
                                return;
                            }
                            eDoWork.Result = _presenter.GetUsageForOffice(
                               tempArgus[0] as DateTimeCompare , tempArgus[1] as string);
                        };
                        worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                        {
                            LoggingUsageDataSet dataset = eCompleted.Result as LoggingUsageDataSet;
                            if (dataset != null && dataset.T_IC_LOGGING_USAGE.Rows.Count > 0)
                            {
                                int rowCount = dataset.T_IC_LOGGING_USAGE.Rows.Count - 1;
                                this.ultraChart3.Height = CHART_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                this.usageByOfficeExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                SetChartStype(this.ultraChart3, "FREQUENCY", dataset);
                                this.ultraChart3.DataSource = dataset.T_IC_LOGGING_USAGE.Select(
                                    data => new
                                    {
                                        officenname = data["OFFICE_NAME"].ToString(),
                                        logintimes = Convert.ToInt32(data["FREQUENCY"])
                                    }
                                    ).ToList();
                                this.ultraChart3.Visible = true;
                            }
                            else
                            {
                                this.ultraChart3.Visible = false;
                                this.usageByOfficeExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;
                            }
                        };
                        #endregion
                        worker.Run(argus);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ButtonSearchForModule_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DisableValidationProvider();
                this.validationProvider5.Enabled = true;
                if (this.ValidateChildren())
                {
                    DateTimeCompare timeEntity = new DateTimeCompare(
                        DateTimeStartDateForModule.DateTime.Date, DateTimeEndDateForModule.DateTime.Date);

                    var argus = new object[] { timeEntity, TextBoxModuleId.Text };

                    using (AsyncWorker<IUsageLogView> worker = new AsyncWorker<IUsageLogView>(_presenter, this.ultraChart5, new Control[] { ButtonSearchForModule }))
                    {
                        #region workder definition
                        worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                        {
                            var tempArgus = eDoWork.Argument as object[];
                            if (tempArgus == null || tempArgus.Length <= 1)
                            {
                                return;
                            }
                            eDoWork.Result = _presenter.GetUsageForModuleData(
                                tempArgus[0] as DateTimeCompare, tempArgus[1] as string);
                        };
                        worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                        {
                            LoggingUsageDataSet dataset = eCompleted.Result as LoggingUsageDataSet;
                            if (dataset != null && dataset.T_IC_LOGGING_USAGE.Rows.Count > 0)
                            {
                                int rowCount = dataset.T_IC_LOGGING_USAGE.Rows.Count - 1;
                                this.ultraChart5.Height = CHART_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                this.usageByModuleExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                SetChartStype(this.ultraChart5, "FREQUENCY", dataset);
                                this.ultraChart5.DataSource = dataset.T_IC_LOGGING_USAGE.Select(
                                    data => new
                                    {
                                        moduleid = data["MODULE_ID"].ToString(),
                                        usingtimes = Convert.ToInt32(data["FREQUENCY"])
                                    }
                                    ).ToList();
                                this.ultraChart5.Visible = true;
                            }
                            else
                            {
                                this.ultraChart5.Visible = false;
                                this.usageByModuleExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;
                            }
                        };
                        #endregion
                        worker.Run(argus);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ButtonSearchForFunction_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DisableValidationProvider();
                this.validationProvider6.Enabled = true;
                if (this.ValidateChildren())
                {
                    DateTimeCompare timeEntity = new DateTimeCompare(
                        DateTimeStartDateForFunction.DateTime.Date, DateTimeEndDateForFunction.DateTime.Date);

                    var argus = new object[] { timeEntity, TextBoxFunctionId.Text };
                    using (AsyncWorker<IUsageLogView> worker = new AsyncWorker<IUsageLogView>(_presenter, this.ultraChart6, new Control[] { ButtonSearchForFunction }))
                    {
                        #region worker definition
                        worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                        {
                            var tempArgus = eDoWork.Argument as object[];
                            if (tempArgus==null || tempArgus.Length<=1)
                            {
                                return;
                            }
                            eDoWork.Result = _presenter.GetUsageForFunctionData(
                                tempArgus[0] as DateTimeCompare, tempArgus[1] as string);
                        };
                        worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                        {
                            LoggingUsageDataSet dataset = eCompleted.Result as LoggingUsageDataSet;
                            if (dataset != null && dataset.T_IC_LOGGING_USAGE.Rows.Count > 0)
                            {
                                int rowCount = dataset.T_IC_LOGGING_USAGE.Rows.Count - 1;
                                this.ultraChart6.Height = CHART_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                this.usageByFunctionExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                SetChartStype(this.ultraChart6, "FREQUENCY", dataset);
                                this.ultraChart6.DataSource = dataset.T_IC_LOGGING_USAGE.Select(
                                    data => new
                                    {
                                        functionid = data["FUNCTION_ID"].ToString(),
                                        usingtimes = Convert.ToInt32(data["FREQUENCY"])
                                    }
                                    ).ToList();
                                this.ultraChart6.Visible = true;
                            }
                            else
                            {
                                this.ultraChart6.Visible = false;
                                this.usageByFunctionExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;
                            }
                        };
                        #endregion 
                        worker.Run(argus);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ButtonSearchForCount_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DisableValidationProvider();
                this.validationProvider7.Enabled = true;
                if (this.ValidateChildren())
                {
                    DateTimeCompare timeEntity = new DateTimeCompare(
                        DateTimeStartDateForCount.DateTime.Date, DateTimeEndDateForCount.DateTime.Date);

                    using (AsyncWorker<IUsageLogView> worker = new AsyncWorker<IUsageLogView>(_presenter, this.ultraChart7, new Control[] { ButtonSearchForCount }))
                    {
                        #region worker defini
                        worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                        {
                            var tempEntity = eDoWork.Argument as DateTimeCompare;
                            if (tempEntity == null)
                            {
                                return;
                            }
                            eDoWork.Result = _presenter.GetAllCountOfUsers(tempEntity);
                        };
                        worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                        {
                            LoggingUsageDataSet dataset = eCompleted.Result as LoggingUsageDataSet;
                            if (dataset != null && dataset.T_IC_LOGGING_USAGE.Rows.Count > 0)
                            {
                                int rowCount = dataset.T_IC_LOGGING_USAGE.Rows.Count - 1;
                                this.ultraChart7.Height = CHART_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                this.usageByUserCountExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                SetChartStype(this.ultraChart7, "USER_COUNT", dataset);
                                this.ultraChart7.DataSource = dataset.T_IC_LOGGING_USAGE.Select(
                                    data => new
                                    {
                                        emptyname = String.Empty,
                                        usercount = Convert.ToInt32(data["USER_COUNT"])
                                    }
                                    ).ToList();
                                this.ultraChart7.Visible = true;
                            }
                            else
                            {
                                this.ultraChart7.Visible = false;
                                this.usageByUserCountExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;
                            }
                        };
                        #endregion
                        worker.Run(timeEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ButtonSearchForCountPerModule_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DisableValidationProvider();
                this.validationProvider8.Enabled = true;
                if (this.ValidateChildren())
                {
                    DateTimeCompare timeEntity = new DateTimeCompare(
                        DateTimeStartDateForCountPerModule.DateTime.Date, DateTimeEndDateForCountPerModule.DateTime.Date);
                    var argus = new object[] { timeEntity, TextBoxModuleIdForCount.Text };

                    using (AsyncWorker<IUsageLogView> worker = new AsyncWorker<IUsageLogView>(_presenter, this.ultraChart8, new Control[] { ButtonSearchForCountPerModule }))
                    {
                        #region worker definition
                        worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                        {
                            var tempArgus = eDoWork.Argument as object[];
                            if (tempArgus==null || tempArgus.Length<=1)
                            {
                                return;
                            }
                            eDoWork.Result = _presenter.GetCountOfUsersByModule(
                                tempArgus[0] as DateTimeCompare, tempArgus[1] as string);
                        };
                        worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                        {
                            LoggingUsageDataSet dataset = eCompleted.Result as LoggingUsageDataSet;
                            if (dataset != null && dataset.T_IC_LOGGING_USAGE.Rows.Count > 0)
                            {
                                int rowCount = dataset.T_IC_LOGGING_USAGE.Rows.Count - 1;
                                this.ultraChart8.Height = CHART_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                this.usageByUserCountPerModuleExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT + rowCount * INCREMENT_FEED;
                                SetChartStype(this.ultraChart8, "USER_COUNT", dataset);
                                this.ultraChart8.DataSource = dataset.T_IC_LOGGING_USAGE.Select(
                                    data => new
                                    {
                                        moduleid = data["MODULE_ID"].ToString(),
                                        usercount = Convert.ToInt32(data["USER_COUNT"])
                                    }
                                    ).ToList();
                                this.ultraChart8.Visible = true;
                            }
                            else
                            {
                                this.ultraChart8.Visible = false;
                                this.usageByUserCountPerModuleExpandableGroupBox.Height = EXPANDABLE_GROUP_BOX_BASE_HEIGHT - CHART_BASE_HEIGHT;
                            }
                        };
                        #endregion
                        worker.Run(argus);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Reset

        private void ButtonClearForUser_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
            this.InitializeUsageByUser();
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

        private void ButtonClearForRole_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
            this.InitializeUsageByRole();
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

        private void ButtonClearForOffice_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
            this.InitializeUsageByOffice();
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

        private void ButtonClearForModule_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
            this.InitializeUsageByModule();
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

        private void ButtonClearForFunction_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
            this.InitializeUsageByFunction();
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

        private void ButtonClearForCount_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
            this.InitializeUsageByUserCount();
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

        private void ButtonClearForCountPerModule_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            this.InitializeUsageByUserCountPerModule();

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

        #region Private method

        private void DisableValidationProvider()
        {
            this.validationProvider1.Enabled = false;
            this.validationProvider2.Enabled = false;
            this.validationProvider3.Enabled = false;
            this.validationProvider4.Enabled = false;
            this.validationProvider5.Enabled = false;
            this.validationProvider6.Enabled = false;
            this.validationProvider7.Enabled = false;
            this.validationProvider8.Enabled = false;
        }

        //Revised for defect #1359
        private void SetChartStype(UltraChart chart, string columnName, LoggingUsageDataSet dataset)
        {
            List<int> countsList = new List<int>();
            foreach (DataRow row in dataset.T_IC_LOGGING_USAGE.Rows)
            {
                countsList.Add(Convert.ToInt32(row[columnName]));
            }
            int maxValue = countsList.Max();
            chart.Axis.X.TickmarkStyle = AxisTickStyle.Smart;

            if (maxValue < 6)
            {
                chart.Axis.X.TickmarkStyle = AxisTickStyle.DataInterval;
                chart.Axis.X.TickmarkInterval = 1;
            }
        }

        #endregion

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


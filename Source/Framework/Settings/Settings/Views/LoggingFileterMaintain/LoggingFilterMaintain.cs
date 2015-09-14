#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :   Settings
// COMPONENT DESC    :  
//
// CREATED DATE/BY   : 06/11/2008/Yang Jian Hua
//
// REVISION HISTORY  : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Data;
using System.Windows.Forms;
using HiiP.Framework.Common;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Messaging;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Infrastructure.Interface;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace HiiP.Framework.Settings
{
    public partial class LoggingFilterMaintain : BaseView, ILoggingFilterMaintain
    {
        public LoggingFilterMaintain()
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
                this.SetDirtyStatus(false);
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

        #region ILoggingFilterMaintain Members

        public void BindGrid(LoggingFilterDS ds)
        {
            this.GridViewLoggingFilter.DataSource = ds;
            this.totalCount.Text = this.GridViewLoggingFilter.Rows.Count.ToString();
        }

       public void SetUsage(bool turnOn)
       {
           ultraOptionSetUsage.Value = turnOn;
       }
        #endregion

        private void GridViewLoggingFilter_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string monitoring = e.Row.Cells["CATEGORY"].Value.ToString();
                if (monitoring.Equals(FilterCategory.Monitoring.ToString()))
                {
                    e.Row.Cells["CATEGORY"].Value = "Performance";
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

        private void btnUsageSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int flag ;
                flag = (bool)ultraOptionSetUsage.Value ? 1 : 0;

                LoggingFilterDS ds = _presenter.GetLoggingFilterByCategory(FilterCategory.Usage.ToString());
                if (ds.T_IC_LOGGING_FILTER.Count != 0)
                {
                    ds.T_IC_LOGGING_FILTER[0].FLAG = flag;
                    _presenter.UpdateLoggingFilter(ds, true);
                    this.SetDirtyStatus(false);
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

        private void btnInstrumentationAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.ShowInstrumentationFilterView(null, null);
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

        [EventSubscription(EventTopicNames.AddInstrumentationFilter, ThreadOption.UserInterface)]
        public void OnAddInstrumentationFilter(object sender, EventArgs eventArgs)
        {
            try
            {
                _presenter.RefreshDataGrid();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                LoggingFilterDS ds = _presenter.GetLoggingFilterByCategory(string.Empty);
                foreach (UltraGridRow row in GridViewLoggingFilter.Rows)
                {
                    if (row.Cells["Select"].Text == "True")
                    {
                        string userId = row.Cells["USER_ID"].Value.ToString();
                        string category = row.Cells["Category"].Text.Equals("Performance")?FilterCategory.Monitoring.ToString()
                        :row.Cells["Category"].Text;

                        LoggingFilterDS.T_IC_LOGGING_FILTERRow[] rows
                            = ds.T_IC_LOGGING_FILTER.Select("USER_ID='" + userId.Replace("'", "''") + "'And CATEGORY='" + category.Replace("'", "''") + "'")
                            as LoggingFilterDS.T_IC_LOGGING_FILTERRow[];

                        if (rows!=null && rows.Length != 0)
                        {
                            rows[0].Delete();
                        }
                    }
                }

                if (ds.T_IC_LOGGING_FILTER.Select(string.Empty,
                    string.Empty, DataViewRowState.Deleted).Length > 0)
                {
                    if(HiiP.Framework.Common.Client.Utility.ShowMessageBox(Messages.Framework.FWC601)
                        .Equals(DialogResult.Yes))
                    {
                        _presenter.UpdateLoggingFilter(ds, false);
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


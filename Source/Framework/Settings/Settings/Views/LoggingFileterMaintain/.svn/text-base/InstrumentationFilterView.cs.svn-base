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
using System.Windows.Forms;
using System.Collections.Generic;

using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;

using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Framework.Messaging;

namespace HiiP.Framework.Settings
{
    public partial class InstrumentationFilterView : BaseView, IInstrumentationFilterView
    {
        public InstrumentationFilterView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                InitForm();
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

        private void InitForm()
        {
            ultraOptionSet.Value = FilterCategory.Instrumentation.ToString();
            this.ultraComboEditorUserName.Items.Clear();
            this.errorProvider1.SetError(this.ultraComboEditorUserName, null);
            Dictionary<string, string> users = _presenter.GetAllUsers();
            foreach (KeyValuePair<string,string> user in users  )
            {
                this.ultraComboEditorUserName.Items.Add(user.Key,
                    user.Value);
            }
            this.ultraComboEditorUserName.SelectedIndex = 0;

            if (_presenter.Key != null)
            {
                this.ultraComboEditorUserName.ReadOnly = true;
                this.ultraComboEditorUserName.Value = _presenter.Key;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
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

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
               

                if (_presenter.CurrentViewStatus.Equals(ViewStatus.Add))
                {
                    if (!this.ValidateChildren())
                    {
                        return;
                    }

                    LoggingFilterDS ds = new LoggingFilterDS();
                    LoggingFilterDS.T_IC_LOGGING_FILTERRow newRow = ds.T_IC_LOGGING_FILTER
                            .NewT_IC_LOGGING_FILTERRow();

                    if (ultraOptionSet.Value.Equals(FilterCategory.Instrumentation.ToString()))
                    {
                        newRow.CATEGORY = FilterCategory.Instrumentation.ToString();
                    }
                    else if(ultraOptionSet.Value.Equals(FilterCategory.Monitoring.ToString()))
                    {
                        newRow.CATEGORY = FilterCategory.Monitoring.ToString();
                    }
                    newRow.USER_ID = ultraComboEditorUserName.Value.ToString();
                    newRow.FLAG = 1;
                    HiiP.Framework.Common.Client.Utility.UpdateIConnectCommonFields(newRow);
                    ds.T_IC_LOGGING_FILTER.AddT_IC_LOGGING_FILTERRow(newRow);
                    _presenter.UpdateLoggingFilter(ds);
                }
                this.SetDirtyStatus(false);
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

        public override void ProcessParameter(ViewParameter parameter)
        {
            _presenter.Key = parameter.Key;
            _presenter.Data = parameter.Data;
            _presenter.CurrentViewStatus = parameter.CurrentViewStatus;
            base.ProcessParameter(parameter);
        }

        private void validationProvider1_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.SourcePropertyName.Equals("UserName"))
                {
                    if (ultraOptionSet.Value.Equals(FilterCategory.Instrumentation.ToString()))
                    {
                        if (_presenter.IsUserExists(FilterCategory.Instrumentation.ToString(),
                            this.ultraComboEditorUserName.Value.ToString()))
                        {
                            e.ConversionErrorMessage = Messages.Framework.FWE605.Format(this.ultraComboEditorUserName.Text);
                        }
                    }
                    else if (ultraOptionSet.Value.Equals(FilterCategory.Monitoring.ToString()))
                    {
                        if (_presenter.IsUserExists(FilterCategory.Monitoring.ToString(),
                            this.ultraComboEditorUserName.Value.ToString()))
                        {
                            e.ConversionErrorMessage = Messages.Framework.FWE605.Format(this.ultraComboEditorUserName.Text);
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
    }
}


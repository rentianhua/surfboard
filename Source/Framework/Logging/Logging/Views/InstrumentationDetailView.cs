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
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Common;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Logging
{
    public partial class InstrumentationDetailView : BaseView, IInstrumentationDetailView
    {
        public InstrumentationDetailView()
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

        public override void ProcessParameter(ViewParameter parameter)
        {
            _presenter.Key = parameter.Key;
            _presenter.Data = parameter.Data;
            _presenter.CurrentViewStatus = ViewStatus.View;
            AppTitle = _presenter.GetAppTitle();
            base.ProcessParameter(parameter);
        }

        #region IExceptionLogDetailView Members

        public void LoadDetailView(LoggingViewDataSet.T_IC_LOGGING_LOGRow row)
        {
            if (row == null)
            {
                return;
            }
            this.TextBoxOffice.Text = ConvertToString(row.OFFICE);
            this.TextBoxComponent.Text = ConvertToString(row.COMPONENT);
            this.TextBoxParameterValues.Text = ConvertToString(row.PARAMETER_VALUES);
            this.TextBoxIPAddress.Text = ConvertToString(row.IP_ADDRESS);
            this.TextBoxPCName.Text = ConvertToString(row.MACHINE_NAME);
            this.TextBoxModuleID.Text = ConvertToString(row.MODULE_ID);
            this.TextBoxFunctionID.Text = ConvertToString(row.FUNCTION_ID);
            this.TextBoxTime.Text = row.LOG_TIME.ToString();
            this.TextBoxUserName.Text = ConvertToString(row.USER_NAME);
            this.TextBoxUserRoles.Text = ConvertToString(row.USER_ROLES);
            this.TextBoxMethodName.Text = ConvertToString(row.METHOD_NAME);
            this.TextBoxStartTicks.Text = ConvertToString(row.TRACING_START_TICKS);
            this.TextBoxEndTicks.Text = ConvertToString(row.TRACING_END_TICKS);
            this.TextBoxDuration.Text = ConvertToString(row.SECONDS_ELAPSED);
        }

        private static string ConvertToString(object value)
        {
            return (DBNull.Value == value || null == value) ? "" : value.ToString();
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


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
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using NCS.IConnect.Hierarchy.Parameter;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Framework.Common.Client;


namespace HiiP.Framework.Settings
{
    public partial class HomePageRefreshConfiguration : BaseView, IHomePageRefreshConfiguration
    {
        public HomePageRefreshConfiguration()
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
            catch (Exception exception)
            {
                this.Enabled = false;
                if (ExceptionManager.Handle(exception))
                {
                    throw;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ultraButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!this.ValidateChildren() || !this.HasDirtyData())
                {
                    return;
                }

                _presenter.Save();
                this.SetDirtyStatus(false);
            }
            catch (Exception exception)
            {
                if (ExceptionManager.Handle(exception))
                {
                    throw;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
           
        }

        #region IHomePageRefreshConfiguration Members

        public string AlertRefreshSeconds
        {
            get { return this.ultraTextEditorAlert.Text; }
            set { this.ultraTextEditorAlert.Text = value; }
        }

        public string MessageRefreshSeconds
        {
            get { return this.ultraTextEditorMessage.Text; }
            set { this.ultraTextEditorMessage.Text = value; }
        }

        public string ToDoListRefreshSeconds
        {
            get { return this.ultraTextEditorToDoList.Text; }
            set { this.ultraTextEditorToDoList.Text = value; }
        }

        public string MyAppointmentsRefreshSeconds
        {
            get { return this.ultraTextEditorAppointments.Text; }
            set { this.ultraTextEditorAppointments.Text = value; }
        }

        public string MyReportRefreshSeconds
        {
            get { return this.ultraTextEditorReport.Text; }
            set { this.ultraTextEditorReport.Text = value; }
        }

        public string MyDashboardRefreshSeconds
        {
            get { return this.ultraTextEditorDashboards.Text; }
            set { this.ultraTextEditorDashboards.Text = value; }
        }

        #endregion

        public override void ProcessParameter(ViewParameter parameter)
        {
            AppTitle = new AppTitleData(FunctionNames.HomePageRefreshSettingsMaintenanceName,
                FunctionNames.HomePageRefreshSettingsMaintenanceScreenID);
            base.ProcessParameter(parameter);
        }

        private void ultraButtonClose_Click(object sender, EventArgs e)
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


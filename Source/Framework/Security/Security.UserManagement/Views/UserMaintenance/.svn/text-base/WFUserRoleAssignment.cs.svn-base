#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/User maintenance
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;


namespace HiiP.Framework.Security.UserManagement
{
    public partial class WFUserRoleAssignment : BaseView, IWFUserRoleAssignment
    {
        public bool IsLoaded { get; set; }
        private IntegratedETHelper _ETHelper;
        public string[] OldParticipations { get; set; }
        private DataSetETRoles _WFAssignedRoles, _WFAssignRoles ;

        public WFUserRoleAssignment()
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

                this.LoadWFGroups();
                Serach();
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().Contains("SAL"))
                {
                    this.LoadWFGroups();
                }
                else
                {
                    this.Enabled = false;
                    if (ExceptionManager.Handle(ex)) throw;
                }

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Process Show View Event Parameters
        /// </summary>
        /// <param name="parameter">The parameters.</param>
        /// <remarks>
        /// This method mostly used to show a add view or deal with event call.
        /// </remarks>
        public override void ProcessParameter(ViewParameter parameter)
        {
            _presenter.Key = parameter.Key;
            _presenter.Data = parameter.Data;
            _presenter.CurrentViewStatus = parameter.CurrentViewStatus;

            AppTitle = _presenter.GetAppTitle();

            base.ProcessParameter(parameter);
        }

        private void LoadWFGroups()
        {
            if (_ETHelper == null) _ETHelper = new IntegratedETHelper(_presenter.Key, WorkItem);
            _WFAssignRoles = new DataSetETRoles();
            _WFAssignedRoles = new DataSetETRoles();

            switch (_presenter.CurrentViewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add:
                    try
                    {
                        // ultraOptionQueueType.Visible = this._ETHelper.IsCurrentUserSupervisoryMember(_presenter.Key.ToString());
                        this._WFAssignRoles = this._ETHelper.GetAllWFGroups();

                    }
                    catch (Exception ex) { ExceptionManager.HandleWithLogOnly(ex); }
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update:
                    this._WFAssignRoles = this._ETHelper.GetAllWFGroups();

                    try
                    {
                        // ultraOptionQueueType.Visible = this._ETHelper.IsCurrentUserSupervisoryMember(_presenter.Key.ToString());
                        this._WFAssignedRoles = this._ETHelper.GetWFRolesForUser(_presenter.Key);
                        this._WFAssignedRoles.Merge(this._ETHelper.GetParticipationForUser(_presenter.Key).ETTable);
                        this.ultraOptionUserType.CheckedIndex = _ETHelper.GetWFUser(_presenter.Key).RoleName.Equals(HiiP.Foundation.Workflow.Interface.Constants.IprocessServer.WorkflowRole.ADMIN) ? 1 : 0;
                    }
                    catch (Exception ex) { ExceptionManager.HandleWithLogOnly(ex); }
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:
                    this._WFAssignRoles = this._ETHelper.GetAllWFGroups();
                    try
                    {
                        // ultraOptionQueueType.Visible = this._ETHelper.IsCurrentUserSupervisoryMember(_presenter.Data.ToString());
                        this._WFAssignedRoles = this._ETHelper.GetWFRolesForUser(_presenter.Data.ToString());
                        this._WFAssignedRoles.Merge(this._ETHelper.GetParticipationForUser(_presenter.Data.ToString()).ETTable);
                        this.ultraOptionUserType.CheckedIndex = _ETHelper.GetWFUser(_presenter.Data.ToString()).RoleName.Equals(HiiP.Foundation.Workflow.Interface.Constants.IprocessServer.WorkflowRole.ADMIN) ? 1 : 0;
                    }
                    catch (Exception ex) { ExceptionManager.HandleWithLogOnly(ex); }
                    break;
            }

            this._ETHelper.BindGird(this.ultraGridWFAssigned, this._WFAssignedRoles);
            this._ETHelper.BindGird(this.ultraGridWFAllGroups, _WFAssignRoles);
            OldParticipations = this.GetParticipation();
        }

        private void Serach()
        {
            if (this._ETHelper == null) return;
            this._ETHelper.SetDataToGrid(IntegratedETHelper.ETType.Workflow,
                                             this.ultraGridWFAllGroups,
                                             this._WFAssignedRoles,
                                             this.ultraTextEditorWFGroupName.Text,
                                             this.ultraTextEditorWFDesc.Text);
        }

        private void ultraButtonWFSearch_Click(object sender, EventArgs e)
        {
            this._WFAssignRoles = ultraOptionQueueType.CheckedIndex == 0 ? this._ETHelper.GetAllWFGroups() : this._ETHelper.GetWFUserQueues(); 
            Serach();
        }

        private void ultraButtonWFReset_Click(object sender, EventArgs e)
        {
            // Reset();
            this._WFAssignRoles = ultraOptionQueueType.CheckedIndex == 0 ? this._ETHelper.GetAllWFGroups() : this._ETHelper.GetWFUserQueues(); 
            this.ultraTextEditorWFGroupName.Text = this.ultraTextEditorWFDesc.Text = string.Empty;
            Serach();
        }

        private void ultraButtonWFAssign_Click(object sender, EventArgs e)
        {
            this._ETHelper.AssignETRoles(this.ultraGridWFAllGroups, this._WFAssignedRoles);
        }

        private void ultraButtonWFUnassign_Click(object sender, EventArgs e)
        {
            this._ETHelper.UnAssignETRoles(this.ultraGridWFAllGroups, this.ultraGridWFAssigned);
        }


        public string[] GetAssignedGroups()
        {

            ultraGridWFAssigned.UpdateData();
            List<string> WFRoles = new List<string>();
            foreach (UltraGridRow row in this.ultraGridWFAssigned.Rows)
            {
                if (row.Cells[_WFAssignedRoles.ETTable.RoleTypeColumn.ColumnName].Value.ToString().Equals("G"))
                    WFRoles.Add(row.Cells[_WFAssignedRoles.ETTable.RoleNameColumn.ColumnName].Value.ToString());
            }
            return WFRoles.ToArray();
        }

        public string[] GetParticipation()
        {
            ultraGridWFAssigned.UpdateData();
            List<string> Participents = new List<string>();

            foreach (UltraGridRow row in this.ultraGridWFAssigned.Rows)
            {
                if (row.Cells[_WFAssignedRoles.ETTable.RoleTypeColumn.ColumnName].Value.ToString().Equals("U"))
                    Participents.Add(row.Cells[_WFAssignedRoles.ETTable.RoleNameColumn.ColumnName].Value.ToString());
            }

            return Participents.ToArray();
        }

        public string[] GetSupervisedUsers()
        {
            ultraGridWFAssigned.UpdateData();
            List<string> SupervisedUsers = new List<string>();

            foreach (UltraGridRow row in this.ultraGridWFAssigned.Rows)
            {
                if (row.Cells[_WFAssignedRoles.ETTable.RoleTypeColumn.ColumnName].Value.ToString().Equals("U"))
                    if ((bool)(row.Cells[_WFAssignedRoles.ETTable.IsSupervisorColumn.ColumnName].Value))
                        SupervisedUsers.Add(row.Cells[_WFAssignedRoles.ETTable.RoleNameColumn.ColumnName].Value.ToString());
            }

            return SupervisedUsers.ToArray();
        }


        public string[] OldParticipation()
        {
            ultraGridWFAssigned.UpdateData();
            List<string> Participents = new List<string>();

            foreach (UltraGridRow row in this.ultraGridWFAssigned.Rows)
            {
                if (row.Cells[_WFAssignedRoles.ETTable.RoleTypeColumn.ColumnName].Value.ToString().Equals("U"))
                    Participents.Add(row.Cells[_WFAssignedRoles.ETTable.RoleNameColumn.ColumnName].Value.ToString());
            }

            return Participents.ToArray();
        }


        public string[] GetSupervisedGroups()
        {
            ultraGridWFAssigned.UpdateData();
            List<string> WFRoles = new List<string>();
            foreach (UltraGridRow row in this.ultraGridWFAssigned.Rows)
            {
                if (row.Cells[_WFAssignedRoles.ETTable.RoleTypeColumn.ColumnName].Value.ToString().Equals("G"))
                    if ((bool)(row.Cells[_WFAssignedRoles.ETTable.IsSupervisorColumn.ColumnName].Value))
                        WFRoles.Add(row.Cells[_WFAssignedRoles.ETTable.RoleNameColumn.ColumnName].Value.ToString());

            }
            return WFRoles.ToArray();
        }

        public bool IsWorkflowAdministrator()
        {
            return ultraOptionUserType.CheckedIndex.Equals(0) ? false : true;
        }

        private void ultraOptionQueueType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ultraOptionQueueType.CheckedIndex == 0)
                {
                    this._WFAssignRoles = this._ETHelper.GetAllWFGroups();
                }
                else if (ultraOptionQueueType.CheckedIndex == 1)
                {
                    this._WFAssignRoles = this._ETHelper.GetWFUserQueues();
                }

                this._ETHelper.BindGird(this.ultraGridWFAssigned, this._WFAssignedRoles);
                this._ETHelper.BindGird(this.ultraGridWFAllGroups, _WFAssignRoles);
                Serach();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        private void ultraGridWFAllGroups_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            e.Row.Cells["QueueType"].Value = e.Row.Cells[_WFAssignedRoles.ETTable.RoleTypeColumn.ColumnName].Value.ToString().Equals("G") ? global::HiiP.Framework.Security.UserManagement.Properties.Resources.group : global::HiiP.Framework.Security.UserManagement.Properties.Resources.single;
        }

        private void ultraGridWFAssigned_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells[_WFAssignedRoles.ETTable.RoleTypeColumn.ColumnName].Value.ToString().Equals("G"))
            {
                e.Row.Cells["QueueType"].Value = global::HiiP.Framework.Security.UserManagement.Properties.Resources.group;
                //e.Row.Cells["IsSupervisor"].Hidden = false; //Because user shouls aslo be supervised
            }
            else
            {
                e.Row.Cells["QueueType"].Value = global::HiiP.Framework.Security.UserManagement.Properties.Resources.single;
                e.Row.Cells["IsSupervisor"].Activation = Activation.NoEdit; //Because user shouls aslo be supervised
                e.Row.Cells["IsSupervisor"].Value = true;
            }

        }
    }
}



namespace HiiP.Framework.Security.UserManagement
{
    [SmartPart]
    public partial class WFUserRoleAssignment
    {
        /// <summary>
        /// Sets the presenter. The dependency injection system will automatically
        /// create a new presenter for you.
        /// </summary>
        [CreateNew]
        public WFUserRoleAssignmentPresenter Presenter
        {
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

    }
}

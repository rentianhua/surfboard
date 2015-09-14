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
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class DMSUserRoleAssignment : BaseView, IDMSUserRoleAssignment
    {
        public bool IsLoaded { get; set; }

        private IntegratedETHelper _ETHelper ;

        private DataSetETRoles _DMSAssignedRoles, _DMSAssignRoles ;

        public DMSUserRoleAssignment()
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
                this.LoadUserDMSRoles();
                IsLoaded = true;
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

        private void LoadUserDMSRoles()
        {
            if (_ETHelper == null) _ETHelper = new IntegratedETHelper(_presenter.Key, WorkItem);
            _DMSAssignedRoles = new DataSetETRoles();
            _DMSAssignRoles = new DataSetETRoles();

            switch (_presenter.CurrentViewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add:
                    this._DMSAssignRoles = _ETHelper.DMSRoles();
                    cb_dmsprofiles.DataSource = _DMSAssignRoles.ETTable;
                    cb_dmsprofiles.DataBind();
                    //this._DMSAssignRoles = this._ETHelper.RetrieveDMSRolesByUserName();
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update:
                    this._DMSAssignedRoles = this.LoadDMSRoles(_presenter.Key);
                    this._DMSAssignRoles = _ETHelper.DMSRoles();
                    cb_dmsprofiles.DataSource = _DMSAssignRoles.ETTable;
                    cb_dmsprofiles.DataBind();
                    cb_dmsprofiles.Value = _DMSAssignedRoles.ETTable.Rows.Count > 0 ? _DMSAssignedRoles.ETTable.Rows[0][_DMSAssignedRoles.ETTable.RoleNameColumn.ColumnName].ToString() : "";
                    this.SetDirtyStatus(false);
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:
                    this._DMSAssignRoles = _ETHelper.DMSRoles();
                    this._DMSAssignedRoles = this.LoadDMSRoles(_presenter.Data.ToString());
                    cb_dmsprofiles.DataSource = _DMSAssignRoles.ETTable;
                    cb_dmsprofiles.DataBind();
                    cb_dmsprofiles.Value = _DMSAssignedRoles.ETTable.Rows.Count >0? _DMSAssignedRoles.ETTable.Rows[0][_DMSAssignedRoles.ETTable.RoleNameColumn.ColumnName].ToString():"";
                    break;
            }

            //this._ETHelper.BindGird(this.ultraGridDMSAssigned, this._DMSAssignedRoles);
            //this._ETHelper.BindGird(this.ultraGridDMSAllRoles, _DMSAssignRoles);

        }

        public DataSetETRoles LoadDMSRoles(string userName)
        {
            string ETRolesForuser ;

            DataSetETRoles ETRoles = new DataSetETRoles();
            DataSetETRoles.ETTableRow newRow ;


            //DataTable ETRolesForuser = new DataTable();
            //ETRolesForuser = _presenter.LoadDMSRoles(username, et_type, true);
            if (_ETHelper == null) _ETHelper = new IntegratedETHelper(userName, WorkItem);

            ETRolesForuser = _ETHelper.FindTrimUser(userName);
            if (!string.IsNullOrEmpty(ETRolesForuser))
            {
                newRow = ETRoles.ETTable.NewETTableRow();
                newRow.BeginEdit();
                newRow.RoleName = ETRolesForuser;
                newRow.Description = ETRolesForuser;
                newRow.UserName = userName;
                newRow.RoleType = ETRolesForuser;
                newRow.EndEdit();
                ETRoles.ETTable.AddETTableRow(newRow);
            }
            return ETRoles;
        }

        
        private void ultraButtonDMSSearch_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this._ETHelper == null) return;
                this._ETHelper.SetDataToGrid(IntegratedETHelper.ETType.DMS,
                                                 this.ultraGridDMSAllRoles,
                                                 this._DMSAssignedRoles,
                                                 this.ultraTextEditorDMSRole.Text,
                                                 this.ultraTextEditorDMSDesc.Text);
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

        private void ultraButtonDMSReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this._ETHelper == null) return;
                this._ETHelper.ResetGrid(this.ultraGridDMSAllRoles,
                                         this.ultraTextEditorDMSRole,
                                         this.ultraTextEditorDMSDesc);

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

        private void ultraButtonDMSAssign_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this._ETHelper.AssignETRoles(this.ultraGridDMSAllRoles, this._DMSAssignedRoles);
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

        private void ultraButtonDMSUnassign_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this._ETHelper.UnAssignETRoles(this.ultraGridDMSAllRoles, this.ultraGridDMSAssigned);
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

        
        public string[] GetAssignedRoles()
        {
            List<string> DMSRoles = new List<string>();
            if (cb_dmsprofiles.Value  == null) return null;
            //foreach (UltraGridRow row in this.ultraGridDMSAssigned.Rows)
            //{
            //    DMSRoles.Add(row.Cells[_DMSAssignedRoles.ETTable.RoleNameColumn.ColumnName].Value.ToString());
            //}
            DMSRoles.Add(cb_dmsprofiles.Value.ToString()); 
            return DMSRoles.ToArray();
        }
    }
}



namespace HiiP.Framework.Security.UserManagement
{
    [SmartPart]
    public partial class DMSUserRoleAssignment
    {
        /// <summary>
        /// Sets the presenter. The dependency injection system will automatically
        /// create a new presenter for you.
        /// </summary>
        [CreateNew]
        public DMSUserRoleAssignmentPresenter Presenter
        {
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

    }
}

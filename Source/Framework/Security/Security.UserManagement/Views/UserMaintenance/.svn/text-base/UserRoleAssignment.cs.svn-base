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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolTip;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class UserRoleAssignment : BaseView, IUserRoleAssignment
    {
        private bool isUserSelectDF;

        public UserRoleAssignment()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                isUserSelectDF = false;
                _presenter.OnViewReady();
                base.OnLoad(e);
                IsLoaded = true;
                EnableDisableViewDF();
                SetDirtyStatus(false);
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

        #region Business

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FindRoleList();
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

        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ResetRoleList();
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

        private void btn_assign_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.AssignRolesForUser(this.ug_availableRoles, this.ug_assignedRoles);
                EnableDisableViewDF();
                SetDirtyStatus(true);

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

        private void EnableDisableViewDF()
        {
            if (ug_assignedRoles.Rows.Count > 0 && isUserSelectDF && ug_assignedRoles.ActiveRow!=null)
                _presenter.EnableDisableViewDF(true);
            else
                _presenter.EnableDisableViewDF(false);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.DeleteRolesFromUser(
                    this.ug_availableRoles, 
                    this.ug_assignedRoles, 
                    this.UltraExpandableGroupBoxPanelMain);
                EnableDisableViewDF();
                SetDirtyStatus(true);

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

        #region Interface Implementation

        public bool IsLoaded { get; set;}

        public UltraToolTipManager ViewUltraToolTipManager 
        {
            get { return this.ultraToolTipManager; }
        }

        public void LoadCopyRoles(string userName)
        {
            RoleEntity[] roles = _presenter.GetRoleEntitysByUserName(userName);
            if (roles != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Role Name");
                dt.Columns.Add("Description");
                foreach (RoleEntity assignedRole in roles)
                {
                    dt.Rows.Add(assignedRole.RoleName, assignedRole.Description);
                }
                this.ug_assignedRoles.DataSource = dt;
            } 
        }

        public void FindRoleList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Role Name");
            dt.Columns.Add("Description");
            RoleEntity[] roleEntitys = _presenter.FindRoleListByConditions(this.txt_rolename.Text, this.txt_description.Text);

            List<RoleEntity> assignedRoles = new List<RoleEntity>();
            foreach (UltraGridRow row in this.ug_assignedRoles.Rows)
            {
                assignedRoles.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
            }

            if (roleEntitys.Length > 0)
            {
                foreach (RoleEntity entity in roleEntitys)
                {
                    if (!_presenter.RoleExists(entity, assignedRoles.ToArray()))
                    {
                        dt.Rows.Add(entity.RoleName, entity.Description);
                    }
                }
            }
            this.ug_availableRoles.DataSource = dt;
        }

        public void ResetRoleList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Role Name");
            dt.Columns.Add("Description");
            this.ug_availableRoles.DataSource = dt;

            this.txt_rolename.Text = String.Empty;
            this.txt_description.Text = String.Empty;
        }

        public string[] GetAssignedRoles()
        {
            ArrayList remainRoleList = new ArrayList();
            foreach (UltraGridRow row in this.ug_assignedRoles.Rows)
            {
                remainRoleList.Add(row.Cells["Role Name"].Value.ToString());
            }
            string[] remainRoles = (string[])remainRoleList.ToArray(typeof(string));

            return remainRoles;
        }

        public void InitAssignedRoles(DataTable dt)
        {
            this.ug_assignedRoles.DataSource = dt;
        }

        public List<DataFilterEntity> DataFilterEntities
        {
            get;
            set;
        }

        public string CurrentRoleNameForDataFilter
        {
            get;
            set;
        }

        #endregion

        private void ug_availableRoles_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                UltraGridRow row = this.ug_availableRoles.ActiveRow;
                if (row != null && e.KeyData == Keys.Space)
                {
                    row.Cells["Select"].Value = !Convert.ToBoolean(row.Cells["Select"].Text);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        private void ug_assignedRoles_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                UltraGridRow row = this.ug_assignedRoles.ActiveRow;
                if (row != null)
                {
                    switch (e.KeyData)
                    {
                        case Keys.Enter :
                            // Load data
                            string roleName = row.Cells["Role Name"].Value.ToString();
                            CurrentRoleNameForDataFilter = roleName;
                            // Load Data Filter Values On a Role For User 
                            _presenter.LoadDataFilterValuesOnRoleForUser(this.UltraExpandableGroupBoxPanelMain, roleName);
                            break;
                        case Keys.Space :
                            row.Cells["Select"].Value = !Convert.ToBoolean(row.Cells["Select"].Text);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        public void SetDirty()
        {
            base.SetDirtyStatus(true);
        }


        // Re-locate internal container, keep it at absolute centeral place.
        private void buttonPanel_Layout(object sender, LayoutEventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.internalButtonPanel.Left = this.buttonPanel.Width / 2 - this.internalButtonPanel.Width / 2;
                this.internalButtonPanel.Top = this.buttonPanel.Height / 2 - this.internalButtonPanel.Height / 2;
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
        public string GetCurrentRole()
        {
            return CurrentRoleNameForDataFilter;
        }


        private void ug_assignedRoles_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                UltraGridRow selectedRow = this.ug_assignedRoles.ActiveRow;
                if (selectedRow == null
                    || selectedRow.Cells==null)
                {
                    return;
                }

                UltraGridCell roleNameCell = selectedRow.Cells["Role Name"];

                if (roleNameCell==null 
                    || roleNameCell.Value==null)
                {
                    return;
                }
                // Load data
                string roleName = roleNameCell.Value.ToString();
                CurrentRoleNameForDataFilter = roleName;
                isUserSelectDF = string.IsNullOrEmpty(roleName)?false:true;

                bool hasDirty = this.HasDirtyData();
                //Load Data Filter Values On a Role For User 
                 _presenter.LoadDataFilterValuesOnRoleForUser(this.UltraExpandableGroupBoxPanelMain, roleName);
                EnableDisableViewDF();

                if (!hasDirty)
                {
                    this.SetDirtyStatus(false);
                }

            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }
     
    }
}


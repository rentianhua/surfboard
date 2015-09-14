#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Organisation maintenance
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using System.Data;
using HiiP.Framework.Security.UserManagement.Interface;
using System.Collections;
using Infragistics.Win.UltraWinGrid;
using System.Collections.Generic;
using HiiP.Framework.Common;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using System.Text.RegularExpressions;
using HiiP.Framework.Messaging;
using HiiP.Infrastructure.Interface.BusinessEntities;
using System.Linq;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class OrganisationDetail : BaseView, IOrganisationUpdate
    {
        public OrganisationDetail()
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

        public override void ProcessParameter(HiiP.Infrastructure.Interface.BusinessEntities.ViewParameter parameter)
        {
            _presenter.Key = parameter.Key;
            _presenter.CurrentViewStatus = parameter.CurrentViewStatus;

            AppTitle = _presenter.GetAppTitle();

            base.ProcessParameter(parameter);
        }

        #region Event

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

                ArrayList availableRoleList = new ArrayList();
                foreach (UltraGridRow row in this.ug_availableRoles.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[2].Value))
                    {
                        availableRoleList.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
                    }
                }
                RoleEntity[] availableRoles = (RoleEntity[])availableRoleList.ToArray(typeof(RoleEntity));

                if (availableRoles.Length == 0) return;

                ArrayList assignedRoleList = new ArrayList();
                foreach (UltraGridRow row in this.ug_assignedRoles.Rows)
                {
                    assignedRoleList.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
                }
                RoleEntity[] assignedRoles = (RoleEntity[])assignedRoleList.ToArray(typeof(RoleEntity));

                // Load assigned role list
                DataTable dt = new DataTable();
                dt.Columns.Add("Role Name");
                dt.Columns.Add("Description");
                foreach (RoleEntity assignedRole in assignedRoles)
                {
                    dt.Rows.Add(assignedRole.RoleName, assignedRole.Description);
                }
                foreach (RoleEntity availableRole in availableRoles)
                {
                    if (!RoleExists(availableRole, assignedRoles))
                    {
                        dt.Rows.Add(availableRole.RoleName, availableRole.Description);
                    }
                }

                foreach (UltraGridRow ultraGridRow in this.ug_availableRoles.Rows)
                {
                    if (Convert.ToBoolean(ultraGridRow.Cells[2].Value))
                    {
                        ultraGridRow.Selected = true;
                    }
                }
                this.ug_availableRoles.DeleteSelectedRows(false);
                this.ug_assignedRoles.DataSource = dt;
                base.SetDirtyStatus(true);
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ArrayList remainRoleList = new ArrayList();
                List<RoleEntity> deletedRoleList = new List<RoleEntity>();
                foreach (UltraGridRow row in this.ug_assignedRoles.Rows)
                {
                    if (!Convert.ToBoolean(row.Cells[2].Value))
                    {
                        remainRoleList.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
                    }
                    else
                    {
                        deletedRoleList.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
                    }
                }

                if (deletedRoleList.Count == 0) return;

                RoleEntity[] remainRoles = (RoleEntity[])remainRoleList.ToArray(typeof(RoleEntity));

                // Load assigned role list
                DataTable dt = new DataTable();
                dt.Columns.Add("Role Name");
                dt.Columns.Add("Description");
                foreach (RoleEntity remainRole in remainRoles)
                {
                    dt.Rows.Add(remainRole.RoleName, remainRole.Description);
                }

                //int assignedSearchResultCount = this.ug_assignedRoles.Rows.Count;

                this.ug_assignedRoles.DataSource = dt;

                //int searchResultCount = _presenter.FindRoleListByConditions(this.txt_rolename.Text, this.txt_description.Text).Length;
                //int availableSearchResultCount = this.ug_availableRoles.Rows.Count;
                //if (availableSearchResultCount + assignedSearchResultCount == searchResultCount)

                //if (this.ug_availableRoles.Rows.Count > 0)
                //{
                //    FindRoleList();
                //}
                //else
                //{
                //    DataTable dtAvailable = new DataTable();
                //    dtAvailable.Columns.Add("Role Name");
                //    dtAvailable.Columns.Add("Description");
                //    foreach (RoleEntity deletedRole in deletedRoleList)
                //    {
                //        dtAvailable.Rows.Add(deletedRole.RoleName, deletedRole.Description);
                //    }
                //    this.ug_availableRoles.DataSource = dtAvailable;
                //}

                List<RoleEntity> availableRoles = new List<RoleEntity>();
                foreach (UltraGridRow row in this.ug_availableRoles.Rows)
                {
                    availableRoles.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
                }
                foreach (RoleEntity deletedRole in deletedRoleList)
                {
                    availableRoles.Add(deletedRole);
                }
                List<RoleEntity> aRoles = availableRoles.OrderBy(r => r.RoleName).ToList();
                DataTable dtAvailable = new DataTable();
                dtAvailable.Columns.Add("Role Name");
                dtAvailable.Columns.Add("Description");
                foreach (RoleEntity deletedRole in aRoles)
                {
                    dtAvailable.Rows.Add(deletedRole.RoleName, deletedRole.Description);
                }
                this.ug_availableRoles.DataSource = dtAvailable;

                base.SetDirtyStatus(true);
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
                if (this.ValidateChildren())
                {
                    _presenter.SaveOrganisation();
                    this.SetDirtyStatus(false);
                    _presenter.OnCloseView();
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

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.OnCloseView();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        private void validationProvider_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        {
            try
            {
                if (_presenter.CurrentViewStatus == ViewStatus.Add && e.SourcePropertyName == "OrganisationName")
                {
                    if (string.IsNullOrEmpty(this.txt_organisationname.Text.Trim()))
                    {
                        e.ConversionErrorMessage = Messages.Framework.FWC225.Format();
                    }
                    else if (!Regex.IsMatch(this.txt_organisationname.Text.Trim(), @"^[A-Za-z0-9_.&@()!' -]+$"))
                    {
                        e.ConversionErrorMessage = Messages.Framework.FWC226.Format();
                    }
                    else if (_presenter.OrganisationExists(this.txt_organisationname.Text.Trim()))
                    {
                        e.ConversionErrorMessage = Messages.Framework.FWC227.Format();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        #endregion

        #region IOrganisationUpdate Members

        public void LoadOrgUpdateData(OrganisationEntity orgEntity)
        {
            // Load organisation 
            this.txt_organisationname.Text = orgEntity.OrganisationName;
            this.txt_organisationDescription.Text = orgEntity.OrganisationDescription;
            _versionNo = orgEntity.VersionNo;

            // Load roles for organisation
            List<RoleEntity> roles = _presenter.GetRolesByOrgName(orgEntity.OrganisationName);
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

        public OrganisationEntity GetOrganisationEntity()
        {
            OrganisationEntity orgEntity = new OrganisationEntity(
                this.txt_organisationname.Text,
                this.txt_organisationDescription.Text
                );

            return orgEntity;
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
                    if (!RoleExists(entity, assignedRoles.ToArray()))
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

        private int _versionNo;
        public int VersionNo
        {
            get { return _versionNo; }
        }

        public void SetOrgNameReadOnly(bool readOnly)
        {
            this.txt_organisationname.ReadOnly = readOnly;
        }

        public void AccessControl(bool isDo)
        {
            this.btn_ok.Visible = isDo;
        }

        #endregion

        #region Private Method

        private bool RoleExists(RoleEntity entity, RoleEntity[] entitys)
        {
            foreach (RoleEntity e in entitys)
            {
                if (e.RoleName == entity.RoleName)
                {
                    return true;
                }
            }

            return false;
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
    }
}


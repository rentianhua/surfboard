#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Role maintenance
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Infrastructure.Interface;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class RoleDetail : BaseView, IRoleDetail
    {
        public RoleDetail()
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
                tv_systemadmin.CollapseAll(); // to fix Defect # 2985
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.DeleteRole();
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

                this.errorProvider.Clear();

                if (!this.ActionsInRole.Contains(RoleDetailPresenter.MinAuthorisationRoleFunctionID))
                {
                    this.errorProvider.SetError(this.tv_systemadmin, Messages.General.GEE007.Format(RoleDetailPresenter.MinAuthorisationRoleFunctionID));
                    return;
                }
                if (this.ValidateChildren())
                {
                    _presenter.SaveRole(this.txt_rolename.Text, this.txt_description.Text);
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

        private void CopyButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.ShowDuplicateRoleView();
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

        private void btn_viewusers_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.ViewUsers();
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

        private void tv_systemadmin_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                //if (e.Action == TreeViewAction.ByMouse)
                //{
                //    CheckOrUnCheckParentNodes(e.Node);
                //}
                // work to update "ActionsInRoles"
                //if (e.Node.Nodes.Count <= 0)
                //{
                    _presenter.UpdateActionsInRole(e.Node.Text, e.Node.Checked);
                //}

                    if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
                    {
                        SelectChildNodes(e.Node, e.Node.Checked);
                        if (e.Node.Checked) SelectParenetNodes(e.Node);
                    }
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
                if (_presenter.CurrentViewStatus == ViewStatus.Add && e.SourcePropertyName == "RoleName")
                {
                    if (string.IsNullOrEmpty(this.txt_rolename.Text.Trim()))
                    {
                        e.ConversionErrorMessage = Messages.Framework.FWE104.Format();
                    }
                    else if (_presenter.RoleExists(this.txt_rolename.Text.Trim()))
                    {
                        e.ConversionErrorMessage = Messages.Framework.FWE105.Format();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        #endregion

        #region IRoleUpdate Members

        /// <summary>
        /// LoadRoleData
        /// </summary>
        /// <param name="roleEntity"></param>
        public void LoadRoleData(RoleEntity roleEntity)
        {
            this.txt_rolename.Text = roleEntity.RoleName;
            this.txt_description.Text = roleEntity.Description;
            _versionNo = roleEntity.VersionNo;
        }

        /// <summary>
        /// Load treeview
        /// </summary>
        public void LoadRoleFunctions()
        {
            // Initialize function tree view
            this.tv_systemadmin.Nodes.Clear();

            List<TreeNode> moduleNodeCollection = _presenter.GetModuleNodeCollection();
            foreach (TreeNode node in moduleNodeCollection)
            {
                // deny any updates to MinAuthorisationRoleFunctionID for add/copy
                if (string.Compare(node.Text, RoleDetailPresenter.MinAuthorisationRoleFunctionID, false) == 0)
                {
                    if (_presenter.CurrentViewStatus != ViewStatus.Update
                        || (_presenter.CurrentViewStatus == ViewStatus.Update && node.Checked))
                    {
                        //If the view is in update mode, and the action code not exits in the selected role.But for better experience, it will not add here. 
                        //So that user will misunderstand it already exists in the role. We will use validation to make sure it.
                        node.ForeColor = Color.Gray;
                        if (!node.Checked)
                        {
                            _presenter.UpdateActionsInRole(node.Text, true);
                            node.Checked = true;
                        }
                    }

                }
                this.tv_systemadmin.Nodes.Add(node);
            }

            this.tv_systemadmin.ExpandAll();
        }

        private List<string> _actionsInRole = new List<string>();
        /// <summary>
        /// local action list in role
        /// every node click action, action list will response correspondingly
        /// </summary>
        public List<string> ActionsInRole
        {
            get { return _actionsInRole; }
            set { _actionsInRole = value; }
        }

        public void DeleteRoleEnabled(bool isEnabled)
        {
            this.btn_delete.Visible = isEnabled;
        }

        public void ViewUsersEnabled(bool isEnabled)
        {
            this.btn_viewusers.Visible = isEnabled;
        }

        #endregion

        #region IRoleDetail Members

        private int _versionNo;
        public int VersionNo
        {
            get { return _versionNo; }
        }

        public void SetRoleNameReadOnly(bool readOnly)
        {
            this.txt_rolename.ReadOnly = readOnly;
        }

        public void AccessControl(bool isDelete, bool isDo, bool isDuplicate)
        {
            this.btn_ok.Visible = isDo;
            this.btn_delete.Visible = isDelete;
            this.CopyButton.Visible = isDuplicate;
        }

        #endregion

      
        private void SelectChildNodes(TreeNode node, bool isChecked)
        {
            if (node == null) return;
            node.Checked = isChecked;
            foreach (TreeNode childNode in node.Nodes)
            {
                node.Checked = isChecked;
                SelectChildNodes(childNode, isChecked);
            }

        }

        private void SelectParenetNodes(TreeNode node)
        {
            if (node == null) return;
            if (node.Parent != null) node.Parent.Checked = true;
            SelectParenetNodes(node.Parent);
        }

        private void tv_systemadmin_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // deny any updates to MinAuthorisationRoleFunctionID
                if (string.Compare(e.Node.Text, RoleDetailPresenter.MinAuthorisationRoleFunctionID, false) == 0)
                {
                    //If the view is in update mode, and the action code not exits in the selected role.But for better experience, it will not add here. 
                    //So that user will misunderstand it already exists in the role. We will use validation to make sure it.

                    e.Cancel = (_presenter.CurrentViewStatus != ViewStatus.Update
                        || (_presenter.CurrentViewStatus == ViewStatus.Update && e.Node.ForeColor == Color.Gray));
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


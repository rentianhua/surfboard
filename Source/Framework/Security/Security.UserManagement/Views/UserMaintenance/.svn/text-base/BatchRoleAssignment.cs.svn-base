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
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Framework.Security.UserManagement.Interface;
using HiiP.Infrastructure.Interface;
using System.Collections;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NCS.IConnect.Common;
using HiiP.Framework.Security.UserManagement.Constants;
using System.Collections.Generic;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.Misc;
using HiiP.Framework.Common;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Infrastructure.Interface.BusinessEntities;
using Microsoft.Practices.CompositeUI.WinForms;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class BatchRoleAssignment : BaseView, IBatchRoleAssignment
    {
        public BatchRoleAssignment()
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
            _presenter.Data = parameter.Data;
            _presenter.CurrentViewStatus = ViewStatus.AssignRolesToUsers;

            _assignRolesToUsersWorkspaceName = Guid.NewGuid().ToString();

            AppTitle = _presenter.GetAppTitle();

            base.ProcessParameter(parameter);
        }

         private void ultraButton4_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.AssignRolesToUsers();
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

        private void ultraButton5_Click(object sender, EventArgs e)
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

        #region IRoleAssignment Members

        private DeckWorkspace _assignRolesToUsersWorkspace;

        public IWorkspace AssignRolesToUsersWorkspace
        {
            get { return _assignRolesToUsersWorkspace; } 
        }

        private string _assignRolesToUsersWorkspaceName;

        public string AssignRolesToUsersWorkspaceName
        {
            get { return _assignRolesToUsersWorkspaceName; }
        }

        public void LoadViewWorkspace()
        {
            _assignRolesToUsersWorkspace = new DeckWorkspace();
            this.functionalRolesUltraGroupBox.Controls.Add(_assignRolesToUsersWorkspace);
            _assignRolesToUsersWorkspace.Name = _assignRolesToUsersWorkspaceName;
            _assignRolesToUsersWorkspace.Dock = DockStyle.Fill;
        }

        public void LoadSelectedUsers()
        {
            UserInfoEntity[] userInfoEntitys = _presenter.GetUserEntitys(); 

            DataTable dt = new DataTable();
            dt.Columns.Add("User ID", Type.GetType("System.String"));
            dt.Columns.Add("Status", Type.GetType("System.String"));
            dt.Columns.Add("Created On", Type.GetType("System.DateTime"));
            dt.Columns.Add("User Name", Type.GetType("System.String"));
            dt.Columns.Add("Gender", Type.GetType("System.String"));
            dt.Columns.Add("Title", Type.GetType("System.String"));
            dt.Columns.Add("Email", Type.GetType("System.String"));
            dt.Columns.Add("Telephone No", Type.GetType("System.String"));
            //dt.Columns.Add("Organisation", Type.GetType("System.String"));
            dt.Columns.Add("Master Office", Type.GetType("System.String"));
            dt.Columns.Add("Office", Type.GetType("System.String"));
            if (userInfoEntitys != null && userInfoEntitys.Length > 0)
            {
                foreach (UserInfoEntity entity in userInfoEntitys)
                {
                    dt.Rows.Add(
                        entity.UserName,
                        entity.UserStatus,
                        entity.CreatedOn,
                        entity.Display,
                        entity.Gender,
                        entity.Title,
                        entity.Email,
                        entity.TelephoneNo,
                        //entity.Organisation,
                        entity.IsMaster,
                        entity.Office);
                }
            }

            this.ug_userlist.DataSource = dt;
        }

        #endregion
    }
}


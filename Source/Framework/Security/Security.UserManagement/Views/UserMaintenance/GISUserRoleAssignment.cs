#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME    :  Housing Integrated Information Program
// COMPONENT ID   :  User management/User maintenance
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 09/06/2009 Anton Fernando
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using HiiP.Foundation.Workflow.Interface.BusinessEntities;
using HiiP.Framework.Common;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Miscellaneous;
using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolTip;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class GISUserRoleAssignment : BaseView, IGISUserRoleAssignment
    {
        public bool IsLoaded { get; set; }
        private IntegratedETHelper _ETHelper;
        private DataSetETRoles _GISAssignedRoles, _GISAssignRoles;
        string[] GISRolesAssigned;
        public static string SelectedUser = string.Empty; 

        public GISUserRoleAssignment()
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
                this.LoadGISRoles();

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

        private void LoadGISRoles()
        {
            if (_ETHelper == null) _ETHelper = new IntegratedETHelper(_presenter.Key, WorkItem);
            _GISAssignRoles = new DataSetETRoles();
            _GISAssignedRoles = new DataSetETRoles();
       

            switch (_presenter.CurrentViewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add:
                    this._ETHelper.BindGird(this.ultraGridGISAssignedRoles, this._GISAssignedRoles);
                    this._ETHelper.BindGird(this.ultraGridGISAllRoles, _GISAssignRoles);
                    Search();
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update:
                    SelectedUser = _presenter.Key;
                    _GISAssignedRoles = _ETHelper.GetUsersGISRoles(_presenter.Key);
                    GISRolesAssigned = AssignedGISRoles();
                    this._ETHelper.BindGird(this.ultraGridGISAssignedRoles, this._GISAssignedRoles);
                    this._ETHelper.BindGird(this.ultraGridGISAllRoles, _GISAssignRoles);
                    Search();
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:

                    _GISAssignedRoles = _ETHelper.GetUsersGISRoles(_presenter.Data.ToString());
                    GISRolesAssigned = AssignedGISRoles();
                    this._ETHelper.BindGird(this.ultraGridGISAssignedRoles, this._GISAssignedRoles);
                    this._ETHelper.BindGird(this.ultraGridGISAllRoles, _GISAssignRoles);
                    this.Search();
                    break;
            }
              
        }

        private string[] AssignedGISRoles()
        {
            List<string> AssignedGIS = new List<string>();
            foreach (DataSetETRoles.ETTableRow ETRow in _GISAssignedRoles.ETTable.Rows)
            {
                AssignedGIS.Add(ETRow.RoleName.ToString());
            }
            return AssignedGIS.ToArray();
        }

        private void ultraButtonGISReset_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this._ETHelper == null) return;
               
                this._ETHelper.ResetGrid(this.ultraGridGISAllRoles,
                                         this.ultraTextEditorGISRole,
                                         this.ultraTextEditorGISDesc);
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

        private void ultraButtonGISSearch_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Search();
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

        private void Search()
        {
            if (this._ETHelper == null) return;
            _GISAssignRoles = this._ETHelper.GetAllGISRoles();
            this._ETHelper.BindGird(this.ultraGridGISAllRoles, _GISAssignRoles);
            this._ETHelper.SetDataToGrid(IntegratedETHelper.ETType.GIS,
                                             this.ultraGridGISAllRoles,
                                             this._GISAssignedRoles,
                                             this.ultraTextEditorGISRole.Text,
                                             this.ultraTextEditorGISDesc.Text);
            
        }

        private void ultraButtonGISAssign_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this._ETHelper.AssignETRoles(this.ultraGridGISAllRoles, this._GISAssignedRoles);
                this.SetDirtyStatus(this.GetAssignedGISRoles().Count>0);
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

        private void ultraButtonGISUnassign_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this._ETHelper.UnAssignETRoles(this.ultraGridGISAllRoles, this.ultraGridGISAssignedRoles);
                this.SetDirtyStatus(this.GetUnassignedGISRoles().Count>0);
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

        public List<ETRoleEntity> GetAssignedGISRoles()
        {

            List<ETRoleEntity> AssignedGISRoles = new List<ETRoleEntity>();
            ETRoleEntity AssignedGISRole;
            if( _GISAssignedRoles != null)
            foreach (DataSetETRoles.ETTableRow ETROW in _GISAssignedRoles.ETTable.Rows)
            {
                if (ETROW.RowState == DataRowState.Added)
                {
                    AssignedGISRole = new ETRoleEntity();
                    AssignedGISRole.RoleName = ETROW[_GISAssignedRoles.ETTable.RoleNameColumn.ColumnName].ToString();
                    AssignedGISRole.UserId = SelectedUser;
                    AssignedGISRole.Description = ETROW[_GISAssignedRoles.ETTable.DescriptionColumn.ColumnName].ToString();
                    AssignedGISRole.EtType = ETROW[_GISAssignedRoles.ETTable.RoleTypeColumn.ColumnName].ToString();
                    AssignedGISRoles.Add(AssignedGISRole);
                }

            }

            return AssignedGISRoles;
        }


        public List<ETRoleEntity> GetUnassignedGISRoles()
        {
            ultraGridGISAllRoles.UpdateData();
            List<ETRoleEntity> UnAssignedGISRoles = new List<ETRoleEntity>();
            ETRoleEntity UnAssignedGISRole;
            if(GISRolesAssigned!= null)
            foreach (string GISRow in GISRolesAssigned)
            {
                UnAssignedGISRole = new ETRoleEntity();
                UnAssignedGISRole.RoleName = GISRow;
                UnAssignedGISRole.UserId = SelectedUser;
                UnAssignedGISRoles.Add(UnAssignedGISRole);
            }
            return UnAssignedGISRoles;
        }
    }
}


namespace HiiP.Framework.Security.UserManagement
{
    [SmartPart]
    public partial class GISUserRoleAssignment
    {
        /// <summary>
        /// Sets the presenter. The dependency injection system will automatically
        /// create a new presenter for you.
        /// </summary>
        [CreateNew]
        public GISUserRoleAssignmentPresenter Presenter
        {
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

    }
}

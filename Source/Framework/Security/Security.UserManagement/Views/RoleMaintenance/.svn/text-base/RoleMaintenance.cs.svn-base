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
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Common.Client.Async;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Infrastructure.Interface;
using Infragistics.Win.UltraWinGrid;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class RoleMaintenance : BaseView, IRoleMaintenance
    {
        public RoleMaintenance()
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
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        public override void ProcessParameter(HiiP.Infrastructure.Interface.BusinessEntities.ViewParameter parameter)
        {
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

        private void ug_rolelist_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.ShowUpdateRoleView(e.Cell.Row.Cells["Role Name"].Value.ToString());
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

        private void ug_rolelist_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter || e.KeyData == Keys.Space)
                {
                    UltraGridRow row = this.ug_rolelist.ActiveRow;

                    if (row != null)
                    {
                        if (row.Cells == null)
                        {
                            return;
                        }

                        _presenter.ShowUpdateRoleView(row.Cells["Role Name"].Value.ToString());
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

        private void closeButton_Click(object sender, EventArgs e)
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

        #endregion

        #region IRoleMaintenance Members

        public void FindRoleList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Role Name");
            dt.Columns.Add("Description");
            dt.Columns.Add("Status");
            this.ug_rolelist.DataSource = dt;
            this.lbl_rolecount.Text = this.ug_rolelist.Rows.Count.ToString();
            this.ug_rolelist.Focus();

            var argus = new object[] { this.txt_rolename.Text, this.txt_description.Text };
            using (AsyncWorker<IRoleMaintenance> worker = new AsyncWorker<IRoleMaintenance>(_presenter, this.ug_rolelist, new Control[] { btn_search, btn_reset }))
            {
                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    var tempArgus = eDoWork.Argument as object[];
                    if (tempArgus == null || tempArgus.Length <= 1)
                    {
                        return;
                    }
                    eDoWork.Result = _presenter.FindRoleListByConditions(tempArgus[0] as string,tempArgus[1] as string);
                };
                worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                {
                    RoleEntity[] roleEntitys = eCompleted.Result as RoleEntity[];
                    if (roleEntitys != null)
                    {
                        if (roleEntitys.Length > 0)
                        {
                            foreach (RoleEntity entity in roleEntitys)
                            {
                                dt.Rows.Add(entity.RoleName, entity.Description, entity.Status);
                            }
                        }
                        this.ug_rolelist.DataSource = dt;
                        this.lbl_rolecount.Text = this.ug_rolelist.Rows.Count.ToString();
                    }
                };
                worker.Run(argus);
            }
        }

        public void ResetRoleList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Role Name");
            dt.Columns.Add("Description");
            dt.Columns.Add("Status");
            this.ug_rolelist.DataSource = dt;
            this.lbl_rolecount.Text = "0";

            this.txt_rolename.Text = String.Empty;
            this.txt_description.Text = String.Empty;
        }

        #endregion
    }
}


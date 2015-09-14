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
using System.Collections.Generic;
using HiiP.Framework.Common;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Infrastructure.Interface.BusinessEntities;
using Infragistics.Win.UltraWinGrid;
using System.ComponentModel;
using HiiP.Framework.Common.Client.Async;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class OrganisationMaintenance : BaseView, IOrganisationMaintenance
    {
        public OrganisationMaintenance()
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
                FindOrganisationList();
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
                ResetOrganisationList();
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

        private void ug_organisationlist_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.UpdateOrganisation(e.Cell.Row.Cells["Organisation"].Value.ToString());
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

        private void ug_organisationlist_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter || e.KeyData == Keys.Space)
                {
                    UltraGridRow row = this.ug_organisationlist.ActiveRow;

                    if (row != null)
                    {
                        if (row.Cells == null)
                        {
                            return;
                        }

                        _presenter.UpdateOrganisation(row.Cells["Organisation"].Value.ToString());
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

        private void CloseButton_Click(object sender, EventArgs e)
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

        #region IOrganisationMaintenance Members

        public void FindOrganisationList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Organisation");
            dt.Columns.Add("Description");
            this.ug_organisationlist.DataSource = dt;
            this.lbl_recordCount.Text = this.ug_organisationlist.Rows.Count.ToString();

            using (AsyncWorker<IOrganisationMaintenance> worker = new AsyncWorker<IOrganisationMaintenance>(_presenter, this.ug_organisationlist, new Control[] { btn_search, btn_reset }))
            {
                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    eDoWork.Result = _presenter.FindeOrganisationsByConditions(this.txt_organisationName.Text, this.txt_organisationDescription.Text);
                };
                worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                {
                    List<OrganisationEntity> orgEntities = eCompleted.Result as List<OrganisationEntity>;
                    if (orgEntities != null)
                    {
                        foreach (OrganisationEntity entity in orgEntities)
                        {
                            dt.Rows.Add(entity.OrganisationName, entity.OrganisationDescription);
                        }
                    }
                    this.ug_organisationlist.DataSource = dt;
                    this.lbl_recordCount.Text = this.ug_organisationlist.Rows.Count.ToString();
                };
                worker.Run();
            }
        }

        public void ResetOrganisationList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Organisation");
            dt.Columns.Add("Description");
            this.ug_organisationlist.DataSource = dt;
            this.lbl_recordCount.Text = "0";

            this.txt_organisationName.Text = String.Empty;
            this.txt_organisationDescription.Text = String.Empty;
        }

        #endregion
    }
}


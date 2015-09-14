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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using System.Collections.Generic;
using HiiP.Framework.Security.UserManagement.Interface;
using Infragistics.Win.UltraWinEditors;
using HiiP.Infrastructure.Interface.Constants;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Common;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class StaffMovement : BaseView, IStaffMovement
    {
        public StaffMovement()
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
            _presenter.Data = parameter.Data;

            AppTitle = _presenter.GetAppTitle();

            base.ProcessParameter(parameter);
        }

        #region Event

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                OfficeEntity officeEntity = GetOfficeEntity();
                if (officeEntity.OfficeRecordStatus == OfficeRecordStatus.Original)
                {
                    officeEntity.OfficeRecordStatus = OfficeRecordStatus.Update;
                }
                _presenter.PostOfficeAction(officeEntity);
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

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                // TODO: Message to confirm whether to delete current office
                if (MessageBox.Show("Are you sure to delete the selected office?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    CurrentOfficeEntity.OfficeRecordStatus = OfficeRecordStatus.Delete;
                    _presenter.PostOfficeAction(CurrentOfficeEntity);
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

        #endregion

        #region IStaffMovement Members

        public void LoadViewData(OfficeEntity officeEntity)
        {
            CurrentOfficeEntity = officeEntity;

            if ((officeEntity.OfficeRecordStatus == OfficeRecordStatus.New && !String.IsNullOrEmpty(officeEntity.IsMaster))
                || officeEntity.OfficeRecordStatus == OfficeRecordStatus.Original || officeEntity.OfficeRecordStatus == OfficeRecordStatus.Update)
            {
                this.cb_masterOffice.Value = officeEntity.IsMaster;
                this.cb_masterOffice.Text = officeEntity.IsMaster;

                this.cb_office.Value = officeEntity.Office;
                this.cb_office.Text = officeEntity.Office;

                this.dt_effectiveDate.Value = officeEntity.EffectiveDate;

                this.deleteButton.Visible = true;
            }
        }

        public void SetCodeTableControls(string ultraComboEditorName, List<CodeTableEntity> codeTableEntityList)
        {
            UltraComboEditor ultraComboEditor = new UltraComboEditor();
            switch (ultraComboEditorName)
            {
                case CodeTableCategoryNames.IsMasterOffice:
                    ultraComboEditor = this.cb_masterOffice;
                    break;
                case CodeTableCategoryNames.Office:
                    ultraComboEditor = this.cb_office;
                    break;
                default:
                    break;
            }

            ultraComboEditor.Items.Clear();
            if (codeTableEntityList.Count > 0)
            {
                foreach (CodeTableEntity codeTableEntity in codeTableEntityList)
                {
                    ultraComboEditor.Items.Add(
                        codeTableEntity.Code,
                        codeTableEntity.Code
                        );
                }
            }
            // set default item value
            ultraComboEditor.SelectedIndex = 0;
        }

        public OfficeEntity CurrentOfficeEntity
        {
            get;
            set;
        }

        #endregion

        #region Private Method

        private OfficeEntity GetOfficeEntity()
        {
            OfficeEntity officeEntity = CurrentOfficeEntity;
            officeEntity.IsMaster = this.cb_masterOffice.Value.ToString();
            officeEntity.Office = this.cb_office.Value.ToString();
            officeEntity.EffectiveDate = this.dt_effectiveDate.DateTime;

            return officeEntity;
        }

        #endregion
    }
}


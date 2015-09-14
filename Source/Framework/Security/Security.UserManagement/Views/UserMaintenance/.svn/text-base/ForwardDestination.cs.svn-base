// ==============================================================================
// Copyright(C) 2005 NCS Pte Ltd
//
// SYSTEM NAME			: NCS HiiP System
// COMPONENT ID			: HiiP.Foundation.Workflow.ForwardDestination
// COMPONENT DESC		: forward the work item to another user
//
// CREATED DATE/BY	    :  04 Jun 2008 / Wang Xunnian
//
// REVISION HISTORY:
// DATE/BY                                  ISSUE#/SR#/CS/PM#/OTHERS	DESCRIPTION OF CHANGE
// ==============================================================================

using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;

using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Common.Client;
using HiiP.Foundation.Workflow.Interface.Constants;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using NCS.IConnect.CodeTable;
using Infragistics.Win.UltraWinEditors;
using HiiP.Framework.Logging.Library;
using System.Data;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class ForwardDestination : BaseView, IForwardDestination
    {
        private CodeBindingOptions _bindingOptions = new CodeBindingOptions();

        public ForwardDestination()
        {
            InitializeComponent();
        }

        #region Properties

        public string ReportsTo
        {
            get;
            private set;
        }

        public bool IsRegionOptionSet
        {
            get{return ultraOptionSetType.Value.ToString().Equals("region");}
        }

        #endregion

        #region Base methods
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
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
                Cursor = Cursors.Default;
            }
        }

        public override void ProcessParameter(ViewParameter parameter)
        {
            _presenter.Key = parameter.Key;
            this.ReportsTo = parameter.Key;
            _presenter.Data = parameter.Data;
            _presenter.CurrentViewStatus = parameter.CurrentViewStatus;

            AppTitle = _presenter.GetAppTitle();
            base.ProcessParameter(parameter);
        }
        #endregion

        private void BindRootOrganizationalUnit(UltraComboEditor ultraComboEditor, bool isHeadOffice)
        {
            string valueField;
            string textField;
            var source = _presenter.GetRootOrganizationalUnits(isHeadOffice, out valueField, out textField);
            ultraComboEditor.Items.Clear();
            ultraComboEditor.Items.Add(null, _bindingOptions.EmptyItemDisplayText);
            Array.ForEach<DataRow>(source, row => ultraComboEditor.Items.Add(row[valueField], row[textField].ToString()));
        }

        private void BindSubOrganizationalUnit(UltraComboEditor ultraComboEditor, string parentId)
        {
            if (string.IsNullOrEmpty(parentId))
            {
                ultraComboEditor.Items.Clear();
                ultraComboEditor.Items.Add(null, _bindingOptions.EmptyItemDisplayText);
                return;
            }

            string valueField;
            string textField;
            var source = _presenter.GetSubOrganizationalUnits(parentId, out valueField, out textField);
            ultraComboEditor.Items.Clear();
            ultraComboEditor.Items.Add(null, _bindingOptions.EmptyItemDisplayText);
            Array.ForEach<DataRow>(source, row => ultraComboEditor.Items.Add(row[valueField], row[textField].ToString()));
        }


        #region event regions
        private void ultraOptionSetType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                this.panelRegionalOffice.Visible = this.IsRegionOptionSet;
                this.panelHeadOffice.Visible = !this.panelRegionalOffice.Visible;
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ultraButtonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (this._presenter.CurrentModule.Equals(FunctionNames.ManualAssignmentID) || this._presenter.CurrentModule.Equals(FunctionNames.StartCaseID))
                {
                    this._presenter.CancelAssignment();
                }
                this._presenter.OnCloseView();                
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ultraComboEditorRegion_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string selectedRegion = null == this.ultraComboEditorRegion.Value ? string.Empty : this.ultraComboEditorRegion.Value.ToString();
                this.BindSubOrganizationalUnit(this.ultraComboEditorOffice, selectedRegion);
                PopulateUsers(selectedRegion.ToString());               
                
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ultraComboEditorUnit_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string selectedUnit = null == this.ultraComboEditorUnit.Value ? string.Empty : this.ultraComboEditorUnit.Value.ToString();
                this.BindSubOrganizationalUnit(this.ultraComboEditorSubUnit, selectedUnit);
                PopulateUsers(selectedUnit);    
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ultraComboEditorOffice_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string selectedOffice = null == this.ultraComboEditorOffice.Value ? string.Empty : this.ultraComboEditorOffice.Value.ToString();
                PopulateUsers(selectedOffice);    
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ultraComboEditorSubUnit_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string selectedSubUnit = null == this.ultraComboEditorSubUnit.Value ? string.Empty : this.ultraComboEditorSubUnit.Value.ToString();
                PopulateUsers(selectedSubUnit);    
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ultraButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.ValidateChildren();
                UltraComboEditor comboBo = IsRegionOptionSet ? ultraComboEditorUser4RegionalOffice : ultraComboEditorUser4HeadOffice;
                if (null == comboBo.Value)
                {
                    return;
                }
// ReSharper disable ConstantNullCoalescingCondition
                ReportsTo = (comboBo.Value ?? "").ToString();  
// ReSharper restore ConstantNullCoalescingCondition
                this._presenter.OnCloseView();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region private methods

        private void PopulateUsers(string selectedOffice)
        {
            int id ;
            if (string.IsNullOrEmpty(selectedOffice)|| !int.TryParse(selectedOffice,out id))
            {
                PopulateUsers(new UserBasicInfoEntity[0]);
                return;
            }
            UserBasicInfoEntity[] users = _presenter.GetUsersWithSpecifiedOffice(id);

            PopulateUsers(users);
        }

        private void PopulateUsers(UserBasicInfoEntity[] users)
        {
            UltraComboEditor comboBo = IsRegionOptionSet ? ultraComboEditorUser4RegionalOffice : ultraComboEditorUser4HeadOffice;
            CodeBindingOptions options = new CodeBindingOptions();

            comboBo.BeginUpdate();

            //Used to display items in dropdownlist completely
            comboBo.Items.Clear();
            comboBo.Items.ValueList.Reset();

            //If the dropdownlist which embedded in the gridview, it cannot use binding source.
            if (options.AppendEmptyItem)
            {
                comboBo.Items.Add(options.EmptyItemValue, options.EmptyItemDisplayText);
            }

            var orderedUsers = users.OrderBy(u=>u.Display);

            foreach (UserBasicInfoEntity user in orderedUsers)
            {
                if (!string.IsNullOrEmpty(user.Title.ToString()))
                {
                    comboBo.Items.Add(user.UserName, user.Display + "(" + user.Title + ")" );
                }
                else
                {
                    comboBo.Items.Add(user.UserName, user.Display);
                }
            }

            comboBo.EndUpdate();
            comboBo.SelectedIndex = 0;
        }
        #endregion

        #region IForwardDestination Members

        public void SetDefaultOption(int selectedOption)
        {
            ultraOptionSetType.CheckedIndex = selectedOption;
        }

        public void BindUsers(UserBasicInfoEntity[] users)
        {
            PopulateUsers(users);
        }

        public void SetDefaultRegion(int defaultOfficeTypeID)
        {
            CodeTableAdapter.SetUltraComboEditor(this.ultraComboEditorRegion, defaultOfficeTypeID.ToString());
        }

        public void SetDefaultOffice(int defaultOfficeTypeID)
        {
            CodeTableAdapter.SetUltraComboEditor(this.ultraComboEditorOffice, defaultOfficeTypeID.ToString());
        }

        public void SetDefaultBranch(int defaultOfficeTypeID)
        {
            CodeTableAdapter.SetUltraComboEditor(this.ultraComboEditorBranch, defaultOfficeTypeID.ToString());
        }

        public void SetDefaultUnit(int defaultOfficeTypeID)
        {
            CodeTableAdapter.SetUltraComboEditor(this.ultraComboEditorUnit, defaultOfficeTypeID.ToString());
        }

        public void SetDefaultSubUnit(int defaultOfficeTypeID)
        {
            CodeTableAdapter.SetUltraComboEditor(this.ultraComboEditorSubUnit, defaultOfficeTypeID.ToString());
        }

        public void SetDefaultRegionUser(string userName)
        {
            CodeTableAdapter.SetUltraComboEditor(this.ultraComboEditorUser4RegionalOffice, userName);
        }

        public void SetDefaultHeadOfficeUser(string userName)
        {
            CodeTableAdapter.SetUltraComboEditor(this.ultraComboEditorUser4HeadOffice, userName);
        }

        #endregion

        private void ultraComboEditorBranch_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string selectedBranch = null == this.ultraComboEditorBranch.Value ? string.Empty : this.ultraComboEditorBranch.Value.ToString();
                this.BindSubOrganizationalUnit(this.ultraComboEditorUnit, selectedBranch);
                PopulateUsers(selectedBranch);   
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        #region IForwardDestination Members


        public void BindRegions()
        {
            this.BindRootOrganizationalUnit(this.ultraComboEditorRegion, false);
        }

        public void BindHeadOffices()
        {
            this.BindRootOrganizationalUnit(this.ultraComboEditorBranch, true);

        }

        #endregion
    }
}


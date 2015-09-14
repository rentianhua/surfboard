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
using HiiP.Foundation.DMS.Interface;
using HiiP.Foundation.Workflow.Interface.ExceptionHandlers;

using HiiP.Framework.Common.Client;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;
using Infragistics.Practices.CompositeUI.WinForms;

using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinListView;
using Infragistics.Win.UltraWinTree;

using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI.WinForms;
using NCS.IConnect.CodeTable;



namespace HiiP.Framework.Security.UserManagement
{
    public partial class UserDetail : BaseView, IUserDetail
    {

        private string _changedDefaultOfficeKey = string.Empty;
        private string ReportsTo = string.Empty;

        public UserDetail()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.OnViewReady();
                _presenter.SetHelpUrlForSubTab(this.ultraTabPageControlGeneral.Tab.Key, this.ultraTabPageControlGeneral.TabControl);
                base.OnLoad(e);
                // Fix for  Defect # 2762 
                this.CheckDirtyRequired = HiiP.Framework.Common.ApplicationContexts.AppContext.Current.UserName.ToLower().Equals(txt_username.Text.ToLower()) ? false : true;
                if (this.CheckDirtyRequired)
                {
                    this._presenter.RegisterEventForDataControl(this.ultraTabPageControlGeneral);
                    this._presenter.RegisterEventForDataControl(this._dmsuserRoleAssignmentDeckWorkspace);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Equals(Messaging.Messages.Workflow.WFI001.Value.ToString()))
                {
                    ExceptionManager.HandleWithLogOnly(ex);
                    Utility.ShowMessageBox(Messages.Workflow.WFI001);
                }
                else if (ex.Message.ToLower().Contains("workflow") 
                    || ex.Message.ToLower().Contains("iprocess"))
                {
                    ExceptionManager.Handle(ex);
                    ultraTabControl1.Tabs[3].Enabled = false;
                }
                else if (ex.Message.ToLower().Contains("trim") 
                    || ex.Message.ToLower().Contains("dms"))
                {
                    ExceptionManager.Handle(ex);
                    ultraTabControl1.Tabs[4].Enabled = false;
                }
                else
                {
                    this.Enabled = false;
                    if (ExceptionManager.Handle(ex)) throw;
                }
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
        private void btn_viewDF_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.ViewFunctionDF();

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
                Dictionary<string, string> ErrorMessages ;
                this.Cursor = Cursors.WaitCursor;
                if (this.ValidateChildren())
                {
                    GISUserRoleAssignment.SelectedUser = txt_username.Text.ToString();


                    ErrorMessages = _presenter.SaveUser();

                    this.SetDirtyStatus(false);
                    _presenter.OnCloseView();

                    foreach (KeyValuePair<string, string> kvp in ErrorMessages)
                    {
                        if (kvp.Key.Equals(HiiP.Framework.Security.UserManagement.Interface.Constants.Exceptions.HiiPUserCreationException))
                        {
                            ExceptionManager.HandleWithLogOnly(new Exception(kvp.Value.ToString()));
                        }
                        else if (kvp.Key.Equals(HiiP.Framework.Security.UserManagement.Interface.Constants.Exceptions.DMSUserCreationException))
                        {
                            DMSExceptionManager.Handle(new Exception(kvp.Value.ToString()));
                        }
                        else if (kvp.Key.Equals(HiiP.Framework.Security.UserManagement.Interface.Constants.Exceptions.WorkflowUserCreationException))
                        {
                            ExceptionManager.HandleWithLogOnly(new WorkflowException(kvp.Value.ToString()));
                        }

                    }
                }
                else
                {
                    //Because just the first tab page has validation, 
                    //it will always navigate to the page when validation failed
                    this.ultraTabControl1.SelectedTab = this.ultraTabPageControlGeneral.Tab;
                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Equals(Messages.Workflow.WFE005.Value))
                {
                    Utility.ShowMessageBox(Messages.Workflow.WFE005);
                }

                else if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public void SetWorkflowTab(bool enabled)
        {
            ultraTabControl1.Tabs[3].Enabled = enabled;
        }

        public void SetDMSTab(bool enabled)
        {
            ultraTabControl1.Tabs[4].Enabled = enabled;
        }


        private void btn_disable_Click(object sender, EventArgs e)
        {
            try
            {

                string value = string.Empty;
                _presenter.OnUpdateStatusBarMessage(value);

                if (_presenter.DeletionCriteriaCheck(this.txt_username.Text, out value))
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.StatusTextEditor.Text)
                    {
                        case UserStatus.Active:
                            if (Utility.ShowMessageBox(Messages.Framework.FWC201,_presenter.Key)
                                == DialogResult.Yes)
                            {
                                _presenter.DisableOrActivateUser();
                                _presenter.DisableDMSUser(txt_username.Text.Trim());

                                this.StatusTextEditor.Text = UserStatus.Inactive;
                                this.btn_disable.Text = "Activate";
                            }
                            break;
                        case UserStatus.Inactive:
                            if (Utility.ShowMessageBox(Messages.Framework.FWC202,_presenter.Key) == DialogResult.Yes)
                            {
                                _presenter.DisableOrActivateUser();
                                _presenter.EnableDMSUser(txt_username.Text.Trim());
                                this.StatusTextEditor.Text = UserStatus.Active;
                                this.btn_disable.Text = "Disable";
                            }
                            break;
                    }
                    this.SetDirtyStatus(false);
                    _presenter.OnCloseView();
                }
                else
                {
                    _presenter.OnUpdateStatusBarMessage(value);
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

        private void btn_copy_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.CopyUser();


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

        #region IUserAdd Members

        public UserInfoEntity GetUserInfo()
        {
            UserInfoEntity userInfoEntity = new UserInfoEntity(
                this.txt_username.Text,
                this.txt_firstname.Text,
                this.txt_initials.Text,
                this.txt_lastname.Text,
                this.txt_display.Text,
                this.txt_alias.Text,
                this.cb_gender.Value.ToString(),
                this.cb_title.Value.ToString(),
                this.dt_dateofbirth.DateTime,
                this.txt_email.Text,
                this.txt_telephoneno.Text,
                this.txt_faxno.Text,
                this.txt_mobileno.Text,
                this.txt_pagerno.Text,
                this.txt_remarks.Text,
                this.StatusTextEditor.Text,
                DateTime.Now,
                String.Empty,
                _changedDefaultOfficeKey,
                "",
                this.Txt_JobTitle.Text,
                cb_branch.Value == null || cb_branch.Value.ToString() == "0" || string.IsNullOrEmpty(cb_branch.Value.ToString()) ? null : cb_branch.Value.ToString(),
                cb_unit.Value == null || cb_unit.Value.ToString() == "0" || string.IsNullOrEmpty(cb_unit.Value.ToString()) ? null : cb_unit.Value.ToString(),
                cb_subunit.Value == null || cb_subunit.Value.ToString() == "0" || string.IsNullOrEmpty(cb_subunit.Value.ToString()) ? null : cb_subunit.Value.ToString(),
                cb_grade.Value == null || cb_grade.Value.ToString() == "0" || string.IsNullOrEmpty(cb_grade.Value.ToString()) ? null : cb_grade.Value.ToString(),
                // Extend "job title" property
                ReportsTo   // Extend "Reports To" property, TO-DO: set "Reports To" value here.
                );
            //userInfoEntity.ConfigurationInfo = UserHelper.CreateUserBasicConfigurationSection();
            int epRIN ;
            userInfoEntity.IsInternal = !this.chb_ExternalUser.Checked;

            if (!userInfoEntity.IsInternal && int.TryParse(txt_EPRIN.Text.Split('-')[0], out epRIN))
            {
                //It should be validated ok in validation
                userInfoEntity.EPRIN = epRIN;
            }
            else
            {
                userInfoEntity.EPRIN = null;
            }

            return userInfoEntity;
        }

        public UserInfoEntity GetUpdatedUserInfo(UserInfoEntity userInfo)
        {
            if (userInfo == null) return GetUserInfo();

            userInfo.FirstName = this.txt_firstname.Text;
            userInfo.Initials = this.txt_initials.Text;
            userInfo.LastName = this.txt_lastname.Text;
            userInfo.Display = this.txt_display.Text;
            userInfo.Alias = this.txt_alias.Text;
            userInfo.Gender = this.cb_gender.Value.ToString();
            userInfo.Title = this.cb_title.Value.ToString();
            userInfo.DateOfBirth = this.dt_dateofbirth.DateTime;
            userInfo.Email = this.txt_email.Text;
            userInfo.TelephoneNo = this.txt_telephoneno.Text;
            userInfo.FaxNo = this.txt_faxno.Text;
            userInfo.MobileNo = this.txt_mobileno.Text;
            userInfo.PageNo = this.txt_pagerno.Text;
            //userInfo.Organisation = String.Empty;
            userInfo.Remarks = this.txt_remarks.Text;

            //User office
            userInfo.Office = this._changedDefaultOfficeKey;

            // Extend "job title" property
            userInfo.JobTitle = this.Txt_JobTitle.Text;

            //Delegations
            userInfo.Branch = cb_branch.Value == null || cb_branch.Value.ToString() == "0" || string.IsNullOrEmpty(cb_branch.Value.ToString()) ? null : cb_branch.Value.ToString();
            userInfo.Unit = cb_unit.Value == null || cb_unit.Value.ToString() == "0" || string.IsNullOrEmpty(cb_unit.Value.ToString()) ? null : cb_unit.Value.ToString();
            userInfo.SubUnit = cb_subunit.Value == null || cb_subunit.Value.ToString() == "0" || string.IsNullOrEmpty(cb_subunit.Value.ToString()) ? null : cb_subunit.Value.ToString();
            userInfo.Grade = cb_grade.Value == null || cb_grade.Value.ToString() == "0" || string.IsNullOrEmpty(cb_grade.Value.ToString()) ? null : cb_grade.Value.ToString();


            // Extend "Reports To" property, TO-DO: set "Reports To" value here.
            userInfo.ReportsTo = ReportsTo == null ? string.Empty : ReportsTo; // 

            int epRIN ;
            userInfo.IsInternal = !this.chb_ExternalUser.Checked;

            if (!userInfo.IsInternal && int.TryParse(txt_EPRIN.Text.Split('-')[0], out epRIN))
            {
                //It should be validated ok in validation
                userInfo.EPRIN = epRIN;
            }
            else
            {
                userInfo.EPRIN = null;
            }

            return userInfo;
        }

        private DeckWorkspace _userRoleAssignmentDeckWorkspace;

        private DeckWorkspace _dmsuserRoleAssignmentDeckWorkspace;

        private DeckWorkspace _wfuserRoleAssignmentDeckWorkspace;

        private DeckWorkspace _gisuserRoleAssignmentDeckWorkspace;

        public void LoadViewWorkspace()
        {
            _userRoleAssignmentDeckWorkspace = new DeckWorkspace();
            this.ultraTabPageControl2.Controls.Add(_userRoleAssignmentDeckWorkspace);
            _userRoleAssignmentDeckWorkspace.Name = String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.UserRoleAssignmentDeckWorkspace, _presenter.Key);
            _userRoleAssignmentDeckWorkspace.Dock = DockStyle.Fill;
        }

        public void LoadViewDMSWorkspace()
        {
            _dmsuserRoleAssignmentDeckWorkspace = new DeckWorkspace();
            _dmsuserRoleAssignmentDeckWorkspace.Name = String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.DMSUserRoleAssignmentDeckWorkspace, _presenter.Key);
            _dmsuserRoleAssignmentDeckWorkspace.Dock = DockStyle.Fill;
            this.ultraTabPageControl9.Controls.Add(_dmsuserRoleAssignmentDeckWorkspace);
        }

        public void LoadViewWFWorkspace()
        {
            _wfuserRoleAssignmentDeckWorkspace = new DeckWorkspace();
            _wfuserRoleAssignmentDeckWorkspace.Name = String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.WFUserRoleAssignmentDeckWorkspace, _presenter.Key);
            _wfuserRoleAssignmentDeckWorkspace.Dock = DockStyle.Fill;
            this.ultraTabPageControl3.Controls.Add(_wfuserRoleAssignmentDeckWorkspace);
        }


        public void LoadViewGISWorkspace()
        {
            _gisuserRoleAssignmentDeckWorkspace = new DeckWorkspace();
            _gisuserRoleAssignmentDeckWorkspace.Name = String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.GISUserRoleAssignmentDeckWorkspace, _presenter.Key);
            _gisuserRoleAssignmentDeckWorkspace.Dock = DockStyle.Fill;
            this.ultraTabPageControl10.Controls.Add(_gisuserRoleAssignmentDeckWorkspace);
        }

        public void LoadViewDropDownList()
        {
            CodeTableAdapter.BindComboByCodeTable(
                this.cb_gender,
                CodeTableCategoryNames.Gender);

            CodeTableAdapter.BindComboByCodeTable(
                this.cb_title,
                CodeTableCategoryNames.Title);

            LoadBranch();
            LoadGrade();

            //CodeTableAdapter.BindComboByCodeTable(
            //    this.cb_unit,
            //    CodeTableCategoryNames.Unit);

            //CodeTableAdapter.BindComboByCodeTable(
            //    this.cb_subunit,
            //    CodeTableCategoryNames.Subunit);
        }

        private void LoadBranch()
        {
            cb_branch.DataSource = _presenter.GetAllBranch();
            cb_branch.DisplayMember = "OrganisationalUnitName";
            cb_branch.ValueMember = "OrganisationalUnitID";
            cb_branch.Value = 0;
        }

        private void LoadGrade()
        {
            CodeBindingOptions option = new CodeBindingOptions();
            option.DisplayTextFormatString="{CODE}";
            option.ValueFormatString = "{ID}";
            CodeTableAdapter.BindComboByCodeTable(
                this.cb_grade,
                CodeManager.GetCodes(CodeTableCategoryNames.Grade,CodeStatus.Effective),option);

        }

        private void LoadUnit(string OrganisationUnitID)
        {
            CodeTableAdapter.BindComboByGenericDataTable(this.cb_unit,_presenter.GetAllUnit(OrganisationUnitID),"OrganisationalUnitID","OrganisationalUnitName");
        }

        private void LoadSubUnit(string OrganisationUnitID)
        {
            CodeTableAdapter.BindComboByGenericDataTable(this.cb_subunit, _presenter.GetAllSubUnit(OrganisationUnitID), "OrganisationalUnitID", "OrganisationalUnitName");
        }
      

        public void LoadUserData(string userName)
        {
            UserInfoEntity entity = _presenter.GetUserInfoByUserName(userName);
            this.txt_username.Text = entity.UserName;
            switch (entity.UserStatus)
            {
                case UserStatus.Active:
                    this.StatusTextEditor.Text = UserStatus.Active;
                    this.btn_disable.Text = "Disable";
                    break;
                case UserStatus.Inactive:
                    this.StatusTextEditor.Text = UserStatus.Inactive;
                    this.btn_disable.Text = "Activate";
                    break;
                default:
                    this.StatusTextEditor.Text = "-";
                    this.btn_disable.Text = "Activate";
                    break;
            }
            this.txt_firstname.Text = entity.FirstName;
            this.txt_initials.Text = entity.Initials;
            this.txt_lastname.Text = entity.LastName;
            this.txt_display.Text = entity.Display;
            this.txt_alias.Text = entity.Alias;

            CodeTableAdapter.SetUltraComboEditor(this.cb_gender, entity.Gender);

            CodeTableAdapter.SetUltraComboEditor(this.cb_title, entity.Title);

            if (entity.DateOfBirth > this.dt_dateofbirth.MinDate &&
                entity.DateOfBirth < this.dt_dateofbirth.MaxDate)
            {
                this.dt_dateofbirth.Value = entity.DateOfBirth;
            }
            this.txt_email.Text = entity.Email;
            this.txt_telephoneno.Text = entity.TelephoneNo;
            this.txt_faxno.Text = entity.FaxNo;
            this.txt_mobileno.Text = entity.MobileNo;
            this.txt_pagerno.Text = entity.PageNo;

            // Extend "job title" property
            this.Txt_JobTitle.Text = entity.JobTitle;

            this.txt_remarks.Text = entity.Remarks;

            if (!string.IsNullOrEmpty(entity.Branch))
                CodeTableAdapter.SetUltraComboEditor(this.cb_branch, entity.Branch);

            if (!string.IsNullOrEmpty(entity.Unit))
                CodeTableAdapter.SetUltraComboEditor(this.cb_unit, entity.Unit);

            if (!string.IsNullOrEmpty(entity.SubUnit))
                CodeTableAdapter.SetUltraComboEditor(this.cb_subunit, entity.SubUnit);

            if (!string.IsNullOrEmpty(entity.Grade))
                CodeTableAdapter.SetUltraComboEditor(this.cb_grade, entity.Grade);

            ReportsTo = entity.ReportsTo;
            if (!string.IsNullOrEmpty(ReportsTo))
            {
                UserBasicInfoEntity UBE = UserHelper.GetUsersBasicInformation(new [] { ReportsTo })[0];
                txt_ReportsTo.Text = string.IsNullOrEmpty(UBE.Title.Trim()) ? UBE.Display : UBE.Display + "(" + UBE.Title + ")";
                txt_ReportsTo.Tag = UBE.UserName;
            }
            _versionNo = entity.VersionNo;

            //User default office
            _changedDefaultOfficeKey = entity.Office;

            this.chb_ExternalUser.Checked = !entity.IsInternal;
            this.txt_EPRIN.Text = (entity.EPRIN == null) ? "" : entity.EPRIN.ToString();
        }

        public void ControlViewStatus()
        {
            switch (_presenter.CurrentViewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add:
                    this.txt_username.ReadOnly = false;
                    this.statusLabel.Visible = false;
                    this.StatusTextEditor.Visible = false;
                    this.StatusTextEditor.Text = UserStatus.Active;

                    this.btn_ok.Visible = _presenter.HasRight(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.NewUserFunctionID);
                    this.btn_disable.Visible = false;
                    this.btn_copy.Visible = false;
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update:
                    this.txt_username.ReadOnly = true;
                    this.statusLabel.Visible = true;
                    this.StatusTextEditor.Visible = true;

                    this.btn_ok.Visible = _presenter.IsUserEditable() & _presenter.HasRight(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionID);
                    this.btn_disable.Visible = _presenter.IsUserEditable() & _presenter.HasRight(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ActivateOrDisableUsersFunctionID);
                    this.btn_copy.Visible = _presenter.HasRight(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.NewUserFunctionID);
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:
                    this.txt_username.ReadOnly = false;
                    this.statusLabel.Visible = false;
                    this.StatusTextEditor.Visible = false;
                    this.StatusTextEditor.Text = UserStatus.Active;

                    this.btn_ok.Visible = _presenter.HasRight(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.NewUserFunctionID);
                    this.btn_disable.Visible = false;
                    this.btn_copy.Visible = false;
                    break;
                default:
                    Utility.SetToViewState(this);
                    break;
            }
        }


        public IWorkspace UserRoleAssignmentWorkspace
        {
            get { return _userRoleAssignmentDeckWorkspace; }
        }

        public IWorkspace DMSUserRoleAssignmentWorkspace
        {
            get { return _dmsuserRoleAssignmentDeckWorkspace; }
        }
        public IWorkspace WFUserRoleAssignmentWorkspace
        {
            get { return _wfuserRoleAssignmentDeckWorkspace; }
        }

        public IWorkspace GISUserRoleAssignmentWorkspace
        {
            get { return _gisuserRoleAssignmentDeckWorkspace; }
        }
        private int _versionNo;
        public int VersionNo
        {
            get { return _versionNo; }
        }

        public string GetUserStatus()
        {
            string userstatus = String.Empty;
            switch (this.StatusTextEditor.Text)
            {
                case UserStatus.Active:
                    userstatus = UserStatus.Inactive;
                    break;
                case UserStatus.Inactive:
                    userstatus = UserStatus.Active;
                    break;
            }

            return userstatus;
        }

        #endregion

        private void validationProvider_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        {
            try
            {
                switch (e.SourcePropertyName)
                {
                    case "UserName":
                        if (_presenter.CurrentViewStatus == HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add ||
                            _presenter.CurrentViewStatus == HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser)
                        {
                            e.ConvertedValue = this.txt_username.Text.Trim();
                            if (string.IsNullOrEmpty(this.txt_username.Text.Trim()))
                            {
                                //e.ConversionErrorMessage = Messages.Framework.FWC221.Format();
                            }
                            else if (_presenter.UserExists(this.txt_username.Text.Trim()))
                            {
                                e.ConversionErrorMessage = Messages.Framework.FWE103.Format();
                            }
                        }
                        break;

                    case "EPRIN":
                        int epRIN ;
                        if (!string.IsNullOrEmpty(this.txt_EPRIN.Text.Trim())
                            && !int.TryParse(this.txt_EPRIN.Text.Trim(), out epRIN))
                        {
                            e.ConversionErrorMessage = "EPRIN is not valid number.";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        #region IUserDetail Members


        public void SetDirty()
        {
            base.SetDirtyStatus(true);
        }

        public void ActivateFunctionalRoleTab()
        {
            this.ultraTabControl1.Tabs[2].Selected = true;
            this.ultraTabControl1.Tabs[4].Selected = true;
            this.ultraTabControl1.Tabs[3].Selected = true;
            this.ultraTabControl1.Tabs[5].Selected = true;
            this.ultraTabControl1.Tabs[0].Selected = true;
        }

        #endregion

        #region IUserDetail for maintain organisation Members
        private MaintainOrganisationForUser _maintainOrganisationForUser ;
        public void LoadOrganisationData()
        {
            _maintainOrganisationForUser = new MaintainOrganisationForUser();
            string userName ;
            switch (_presenter.CurrentViewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:
                    userName = _presenter.Data as string;
                    break;
                default:
                    userName = this.txt_username.Text;
                    break;
            }

            //Set the status of the following buttons before loading data to tree.
            //They will be turned on when some one node is activated.
            this.UnselectButton.Enabled = false;
            this.DefaultButton.Enabled = false;

            OfficesHierarchyDataSet hierarchy = _presenter.GetOfficesAllHierarchy(userName);
            this._maintainOrganisationForUser.InitOrganisationTree(this.OfficeTree, hierarchy.LookupOrganisationalUnitHierarchy);
            //if previous method has set it to false, will remain here
            this.SelectButton.Enabled = (this.SelectButton.Enabled && this.OfficeTree.Nodes.Count > 0);

            if (string.IsNullOrEmpty(userName)) return;

            UserInfoEntity entity = _presenter.GetUserInfoByUserName(userName);

            this._changedDefaultOfficeKey = entity.Office;

            //_changedDefaultOfficeKey was set with the value of profile, refer to LoadUserData(string)
            this._maintainOrganisationForUser.InitUserOrganisationTree(this.SelectOfficeListView,
                this._changedDefaultOfficeKey, _presenter.CurrentViewStatus,
                hierarchy.OrganisationUser);

        }

        public void CollectOrganisationData(out OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, out OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            this._maintainOrganisationForUser.SaveSelectedOffices(this.SelectOfficeListView, this.txt_username.Text.Trim(), out removedOfficeList, out newOfficeList);
        }

        #endregion  IUserDetail for maintain organisation Members

        #region Maintain organisation
        private void SelectButton_Click(object sender, EventArgs e)
        {
            try
            {
                DevTreeNode node = this.OfficeTree.ActiveNode as DevTreeNode;
                if (node == null) return;
                UltraListViewItem item = CreateOfficeOfUser(node);
                if (item == null || IsAppendItemToList(item.Key))
                {
                    this.SelectOfficeListView.Items.Add(item);
                    this.SetDirty();
                }

                this.OfficeTree.ExpandAll();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        private bool IsAppendItemToList(string key)
        {
            return (!(this.SelectOfficeListView.Items.IndexOf(key) > -1));

        }

        private UltraListViewItem CreateOfficeOfUser(DevTreeNode node)
        {
            OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow hierarchy = node.Tag as OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow;
            if (hierarchy == null)
            {
                return null;
            }
            var tag = hierarchy.OrganisationalUnitTypeID.ToString();
            return MaintainOrganisationForUser.CreateOfficeOfUser(hierarchy.OrganisationalUnitID, node.Text, tag);
        }

        private void UnselectButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.SelectOfficeListView.SelectedItems.Count>0)
                {
                    this.SetDirty();
                }
                while (this.SelectOfficeListView.SelectedItems.Count > 0)
                {
                    UltraListViewItem item = this.SelectOfficeListView.SelectedItems[0];
                    if (item == null) break;
                    
                    this.SelectOfficeListView.Items.Remove(item);

                    //if remove the default office item ,remove the default office
                    var defaultOfficeId = item.Key;
                    if (defaultOfficeId.Equals(_changedDefaultOfficeKey))
                        _changedDefaultOfficeKey = string.Empty;
                }

                if (this.SelectOfficeListView.Items.Count > 0)
                {
                    this.SelectOfficeListView.SelectedItems.Add(this.SelectOfficeListView.Items[0]);
                    this.SelectOfficeListView.ActiveItem = this.SelectOfficeListView.Items[0];
                }

                //When no item was selected, disable the following buttons
                this.DefaultButton.Enabled = this.SelectOfficeListView.Items.Count != 0;
                this.UnselectButton.Enabled = this.SelectOfficeListView.Items.Count != 0;

                this.OfficeTree.ExpandAll();

            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        private void OpenTreeNodeVisiable(UltraTreeNode currentNode)
        {
            UltraTreeNode parent = currentNode.Parent;
            if (parent != null)
            {
                parent.Visible = true;
                this.OpenTreeNodeVisiable(parent);
            }
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SelectOfficeListView.ActiveItem == null) return;

                _changedDefaultOfficeKey = this.SelectOfficeListView.ActiveItem.Key;

                this._maintainOrganisationForUser.SetDefaultOffice(this.SelectOfficeListView);
                this.SetDirty();
            }
            catch (Exception ex)
            {

                if (ExceptionManager.Handle(ex)) throw;

            }
        }

        #endregion


        private void ultraTextEditorReportsTo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                UltraTextEditor editor = e.Context as UltraTextEditor;
                ReportsTo = _presenter.ShowReportsTo((editor == null || editor.Tag==null) ? "" : editor.Tag.ToString());
                if (!string.IsNullOrEmpty(ReportsTo))
                {
                    UserBasicInfoEntity UBE = UserHelper.GetUsersBasicInformation(new [] { ReportsTo })[0];
                    txt_ReportsTo.Text = string.IsNullOrEmpty(UBE.Title.Trim()) ? UBE.Display : UBE.Display + "(" + UBE.Title + ")";
                    txt_ReportsTo.Tag = ReportsTo;
                }
                else
                {
                    txt_ReportsTo.Text = "";
                    txt_ReportsTo.Tag = "";
                }


            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {


            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.SetHelpUrlForSubTab(e.Tab.Key, e.Tab.TabControl);
                if (ultraTabControl1.Tabs[2].Active)
                {
                    btn_viewDF.Enabled = this._presenter.IsViewFunctionDFEnable();
                }
                else
                {
                    btn_viewDF.Enabled = false;
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

        private void SelectOfficeListView_ItemActivated(object sender, ItemActivatedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.UnselectButton.Enabled = true;
                this.DefaultButton.Enabled = true;

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

        private void chb_ExternalUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {


                if (!chb_ExternalUser.Checked)
                {
                    txt_EPRIN.ReadOnly = true;
                    txt_EPRIN.Text = string.Empty;
                }
                else
                {
                    txt_EPRIN.ReadOnly = false;
                }

                btn_ViewEP.Enabled = chb_ExternalUser.Checked;
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        private void btn_ViewEP_Click(object sender, EventArgs e)
        {

            try
            {
                ViewParameter parameter = new ViewParameter();
                parameter.Key = Guid.NewGuid().ToString();
                parameter.CurrentViewStatus = ViewStatus.Update;
                _presenter.OnShowSearchExternalParty(this.ViewId);

            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        public void FillExternalParty(string EPRIN)
        {
            txt_EPRIN.Text =  EPRIN;
        }

        private void cb_branch_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadUnit(cb_branch.Value == null ? string.Empty : cb_branch.Value.ToString());
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        private void cb_unit_ValueChanged(object sender, EventArgs e)
        {

            try
            {
                LoadSubUnit(cb_unit.Value == null ? string.Empty : cb_unit.Value.ToString());
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        [EventSubscription(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.EnableDisableViewDF, ThreadOption.UserInterface)]
        public void EnableDisableViewDF(object sender, EventArgs<bool> eventArgs)
        {
            btn_viewDF.Enabled = eventArgs.Data;
        }     

    }
}


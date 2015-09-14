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
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

using HiiP.Framework.Common.Client;
using HiiP.Framework.Common.Client.Async;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.BusinessEntity;

using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;

using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;

using Microsoft.Practices.CompositeUI.EventBroker;
using NCS.IConnect.CodeTable;
using System.Security.Permissions;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class UserMaintenance :BaseView, IUserMaintenance
    {
        [EventPublication(HiiP.Framework.Security.UserManagement.Interface.Constants.EventTopicNames.CommonSelectHiiPUser, PublicationScope.Global)]
        public event EventHandler<EventArgs<ViewParameter>> SelectHiiPUser;

        [EventPublication(HiiP.Framework.Security.UserManagement.Interface.Constants.EventTopicNames.CommonSearchHiiPUser, PublicationScope.Global)]
#pragma warning disable 67
        public event EventHandler<EventArgs<ViewParameter>> SearchHiiPUser;
#pragma warning restore 67

        public UserMaintenance()
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

        public override void ProcessParameter(ViewParameter parameter)
        {
            AppTitle = _presenter.GetAppTitle();
            _presenter.ViewStatus = parameter.CurrentViewStatus;

            base.ProcessParameter(parameter);
        }

        #region Event

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ShowUserList();
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
                ResetUserList();
                this.btn_disable.Enabled = false;
                this.btn_assignroles.Enabled = false;
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

        private void ug_userlist_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.ShowUpdateUserView(e.Cell.Row.Cells["User ID"].Value.ToString());
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

        private void ug_userlist_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                UltraGridRow row = this.ug_userlist.ActiveRow;

                if (row != null)
                {
                    if (row.Cells == null)
                    {
                        return;
                    }

                    switch (e.KeyData)
                    {
                        case Keys.Enter:
                            _presenter.ShowUpdateUserView(row.Cells["User ID"].Value.ToString());
                            break;
                        case Keys.Space:
                            row.Cells["Select"].Value = !Convert.ToBoolean(row.Cells["Select"].Text);
                            ug_userlist_CellChange(sender, new CellEventArgs(row.Cells["Select"]));
                            break;
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

        private void ug_userlist_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.Cell.Column.Key == "Select")  // cell change -- it is "Select" column
                {
                    if (Convert.ToBoolean(e.Cell.Text))  // current cell is selected
                    {
                        #region current cell is selected
                        switch (e.Cell.Row.Cells["Status"].Value.ToString())
                        {
                            case UserStatus.Active:
                                this.btn_disable.Text = "Disable";
                                this.btn_disable.Enabled = true;
                                this.btn_assignroles.Enabled = true;
                                break;
                            case UserStatus.Inactive:
                                this.btn_disable.Text = "Activate";
                                this.btn_disable.Enabled = true;
                                this.btn_assignroles.Enabled = true;
                                break;
                            default:
                                this.btn_disable.Enabled = false;
                                this.btn_assignroles.Enabled = false;
                                break;
                        }
                        #endregion
                    }
                    else // current cell is unselected
                    {
                        #region current cell is unselected
                        bool isSelected = false;
                        foreach (UltraGridRow row in this.ug_userlist.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells["Select"].Text))
                            {
                                isSelected = true;
                                switch (row.Cells["Status"].Value.ToString())
                                {
                                    case UserStatus.Active:
                                        this.btn_disable.Text = "Disable";
                                        this.btn_disable.Enabled = true;
                                        this.btn_assignroles.Enabled = true;
                                        break;
                                    case UserStatus.Inactive:
                                        this.btn_disable.Text = "Activate";
                                        this.btn_disable.Enabled = true;
                                        this.btn_assignroles.Enabled = true;
                                        break;
                                    default:
                                        this.btn_disable.Enabled = false;
                                        this.btn_assignroles.Enabled = false;
                                        break;
                                }
                                break;
                            }
                        }
                        if (!isSelected)
                        {
                            this.btn_disable.Enabled = false;
                            this.btn_assignroles.Enabled = false;
                        }
                        #endregion
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

        private List<string> FilterUsersForDisableing(List<string> usernames, out List<string> UnableUsers)
        {
            
                List<string> DisableUsers = new List<string>();
                UnableUsers = new List<string>();
                List<string> outvalue = new List<string>();
                string value ;
                foreach (string User in usernames)
                {
                    if (_presenter.DeletionCriteriaCheck(User, out value))
                    {
                        DisableUsers.Add(User);
                    }
                    else
                    {
                        UnableUsers.Add(User);
                        outvalue.Add(value);
                    }
                    

                }

                if (UnableUsers.Count > 0)
                {
                    UserStatusException UserStatusException = new UserStatusException();
                    UserStatusException.CustomMessage = Buildexceptiontolog(UnableUsers, outvalue);
                    ExceptionManager.HandleWithLogOnly(UserStatusException);
                }

                return DisableUsers;
            
        }

        private string Buildexceptiontolog(List<string> Users, List<string> Messages)
        {
            StringBuilder exeptionstring = new StringBuilder();

            for (int i = 0; i < Users.Count; i++)
            {
                exeptionstring.Append(Users[i] + " can't be disabled - " + Messages[i] + Environment.NewLine);
            }

            return exeptionstring.ToString();
        }

        private string FormatUsers(List<string> usernames)
        {
            string returnFormat = string.Empty;
            foreach (string User in usernames)
            {
                returnFormat += User + " ";
            }
            return returnFormat;
        }


        private void btn_disable_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                string userstatus = UserStatus.Active;
                List<string> usernames = new List<string>();

                foreach (UltraGridRow row in this.ug_userlist.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Select"].Text))
                    {
                        usernames.Add(row.Cells["User ID"].Value.ToString());
                        userstatus = row.Cells["Status"].Value.ToString();
                    }
                }

                switch (userstatus)
                {
                    case UserStatus.Active:
                        if (Utility.ShowMessageBox(Messages.Framework.FWC203) == DialogResult.Yes)
                        {
                            List<string> Unableusernames ;
                            List<string> Ableusernames;

                            Ableusernames = FilterUsersForDisableing(usernames, out Unableusernames);
                            _presenter.DisableOrActivateUser(Ableusernames, UserStatus.Inactive);
                            if (Unableusernames.Count > 0)
                            {
                                if (Ableusernames.Count > 0)
                                {
                                    _presenter.OnUpdateStatusBarMessage("User(s) " + FormatUsers(Ableusernames) + " were sucessfully disabled. But " + FormatUsers(Unableusernames) + " were not disabled because there are workflow items/queues assigned");
                                     
                                }
                                else
                                {
                                    _presenter.OnUpdateStatusBarMessage("User(s) " + FormatUsers(Unableusernames) + " were not disabled because there are workflow items/queues assigned");
                                }
                            }
                        }
                        break;
                    case UserStatus.Inactive:
                        if (Utility.ShowMessageBox(Messages.Framework.FWC204) == DialogResult.Yes)
                        {
                            _presenter.DisableOrActivateUser(usernames, UserStatus.Active);
                        }
                        break;
                }
                // Initialize user list
                ShowUserList();
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

        private void ultraButton3_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<string> usernames = new List<string>();
                foreach (UltraGridRow row in this.ug_userlist.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Select"].Text))
                    {
                        usernames.Add(row.Cells["User ID"].Value.ToString());
                    }
                }
                _presenter.RoleAssignment(usernames.ToArray());
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

        #region IUserMaintenance Members

        public void ShowUserList()
        {
            this.btn_disable.Enabled = false;
            this.btn_assignroles.Enabled = false;

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
            dt.Columns.Add("AllOffices", Type.GetType("System.String"));
            dt.Columns.Add("First Name", Type.GetType("System.String"));
            dt.Columns.Add("Last Name", Type.GetType("System.String"));
            dt.Columns.Add("Mobile", Type.GetType("System.String"));
            this.ug_userlist.DataSource = dt;
            this.ug_userlist.Focus();
            this.lbl_recordCount.Text = "0";

            var criteria = GetSearchCriteria();
            using (AsyncWorker<IUserMaintenance> worker = new AsyncWorker<IUserMaintenance>(_presenter, this.ug_userlist, new Control[] { btn_search }))
            {
                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    var tempCriteria = eDoWork.Argument as UserInfoSearchCriteria;
                    if (tempCriteria == null)
                    {
                        return;
                    }
                    eDoWork.Result = _presenter.GetUserInfoArrayEntity(tempCriteria);
                };
                worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                {
                    UserInfoEntity[] userInfoEntitys = eCompleted.Result as UserInfoEntity[];
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
                                entity.Office,
                                entity.AllOffices,
                                entity.FirstName,
                                entity.LastName,
                                entity.MobileNo
                                );
                        }
                    }
                    this.ug_userlist.DataSource = dt;
                    this.lbl_recordCount.Text = this.ug_userlist.Rows.Count.ToString();
                };
                worker.Run(criteria);
            }
        }

        public void ResetUserList()
        {
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
            dt.Columns.Add("AllOffices", Type.GetType("System.String"));
            dt.Columns.Add("First Name", Type.GetType("System.String"));
            dt.Columns.Add("Last Name", Type.GetType("System.String"));
            dt.Columns.Add("Mobile", Type.GetType("System.String"));
            this.ug_userlist.DataSource = dt;
            this.lbl_recordCount.Text = "0";

            // clear seach criteria
            this.txt_username.Text = String.Empty;
            this.cb_status.SelectedIndex = 0;
            this.dt_createdfrom.Value = null;
            this.dt_createdto.Value = null;
            this.txt_display.Text = String.Empty;
            this.txt_email.Text = String.Empty;
            this.cb_userType.SelectedIndex = 0;
            this.txt_office.Text = String.Empty;
        }

        private UserInfoSearchCriteria GetSearchCriteria()
        {
            UserInfoSearchCriteria userInfoSearchCriteria = new UserInfoSearchCriteria();
            userInfoSearchCriteria.UserName = this.txt_username.Text;
            if (this.cb_status.Value != null)
            {
                userInfoSearchCriteria.UserStatus = this.cb_status.Value.ToString();
            }
            else
            {
                // For exception handler
                userInfoSearchCriteria.UserStatus = String.Empty;
            }
            if (this.dt_createdfrom.Value != null)
            {
                userInfoSearchCriteria.CreatedFrom = this.dt_createdfrom.DateTime;
            }
            else
            {
                userInfoSearchCriteria.CreatedFrom = this.dt_createdfrom.MinDate;
            }
            if (this.dt_createdto.Value != null)
            {
                userInfoSearchCriteria.CreatedTo = this.dt_createdto.DateTime;
            }
            else
            {
                userInfoSearchCriteria.CreatedTo = this.dt_createdto.MaxDate;
            }
            userInfoSearchCriteria.Display = this.txt_display.Text;
            userInfoSearchCriteria.Email = this.txt_email.Text;

            userInfoSearchCriteria.UserType = (this.cb_userType.Value == DBNull.Value) ? null : (this.cb_userType.Value as bool?);

            userInfoSearchCriteria.Office = this.txt_office.Text;

            return userInfoSearchCriteria;
        }

        /// <summary>
        /// Initialize organisation list
        /// </summary>
        public void SetUserTypeList()
        {
            //DataTable userTypes = new DataTable();
            //userTypes.Columns.Add("Value",typeof(bool));
            //userTypes.Columns.Add("Display");

            //userTypes.Rows.Add(DBNull.Value, "All");
            //userTypes.Rows.Add(true, "Internal");
            //userTypes.Rows.Add(false, "External");

            //CodeTableAdapter.BindComboByGenericDataTable(this.cb_userType,userTypes,"Value","Display",new CodeBindingOptions("","",false));
            cb_userType.Items.Add(null, "All");
            cb_userType.Items.Add(true, "Internal");
            cb_userType.Items.Add(false, "External");

            // set default item value
            this.cb_userType.SelectedIndex = 0;
        }

        /// <summary>
        /// Initialize control by category name
        /// </summary>
        /// <param name="ultraComboEditorName"></param>
        /// <param name="defaultValue"></param>
        public void SetCodeTableControls(string ultraComboEditorName, bool defaultValue)
        {
            UltraComboEditor ultraComboEditor = null;
            string category = "";
            switch (ultraComboEditorName)
            {
                case CodeTableCategoryNames.UserStatus:
                    ultraComboEditor = this.cb_status;
                    category = HiiP.Infrastructure.Interface.Constants.CodeTableCategoryNames.UserStatus;
                    break;
                default:
                    break;
            }

            var options = new NCS.IConnect.CodeTable.CodeBindingOptions();
            options.AppendEmptyItem = defaultValue;

            CodeTableAdapter.BindComboByCodeTable(ultraComboEditor, category, options);

            // set default item value
            if (ultraComboEditor!=null) ultraComboEditor.SelectedIndex = 0;

            // Dirty solution to resolve default selected item "Active"
            if (ultraComboEditor!=null && ultraComboEditorName.Equals(CodeTableCategoryNames.UserStatus) 
                && ultraComboEditor.Text.Equals(UserStatus.Inactive))
            {
                ultraComboEditor.SelectedIndex = 1;
            }
        }

        public void AccessControl(bool activate, bool assignRoles)
        {
            this.btn_disable.Visible = activate;
            this.btn_assignroles.Visible = assignRoles;
        }

        public void Show4CommonSearch(bool isCommonSearch)
        {
            //Remove "Assign roles" and "Disable" buttons
            AccessControl(!isCommonSearch, !isCommonSearch);

            //Add "Select" button to return to calling program
            this.ultraButtonSelect.Visible = isCommonSearch;

            //Disable multi-select user. Only single user select
            if (this.ug_userlist.DisplayLayout == null
                || this.ug_userlist.DisplayLayout.Bands.Count == 0
                || this.ug_userlist.DisplayLayout.Bands[0].Columns.IndexOf("Select") == -1)
            {
                return;
            }
            UltraGridColumn columnSelected = this.ug_userlist.DisplayLayout.Bands[0].Columns["Select"];

            if (columnSelected == null)
            {
                return;
            }
            columnSelected.Hidden = isCommonSearch;
            this.ug_userlist.DisplayLayout.Override.MaxSelectedRows = isCommonSearch ? 1 : this.ug_userlist.DisplayLayout.Override.MaxSelectedCells;

        }
        #endregion

        private void ultraButtonSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                /*
                 * Return parameters to calling program:
                    i. User ID
                    ii. Display Name
                    iii. Status
                    iv. Title
                    v. Email
                    vi. Default Office
                 *  vii. First Name
                 *  viii. Last Name
                 *  ix.mobile
                 */
                if (this.ug_userlist.ActiveRow == null)
                {
                    return;
                }
                UltraGridRow gridRow = this.ug_userlist.ActiveRow;

                DataRowView rowView = gridRow.ListObject as DataRowView;
                if (rowView == null)
                {
                    return;
                }

                ViewParameter data = new ViewParameter();
                data.ViewId = this.ViewId;
                data.Data = rowView.Row;
                EventArgs<ViewParameter> eArgs = new EventArgs<ViewParameter>(data);
                OnSelectHiiPUser(eArgs);
                this._presenter.OnCloseView();
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

        protected virtual void OnSelectHiiPUser(EventArgs<ViewParameter> eventArgs)
        {
            if (SelectHiiPUser != null)
            {
                SelectHiiPUser(this, eventArgs);
            }
        }
    }

    [Serializable]
    public class UserStatusException : System.Exception
    {


        // Gets or Sets the custom error message detailing the error 
        public string CustomMessage
        {
            get; set;
        }

        public UserStatusException() { }

        public UserStatusException(string message)
            : base(message)
        { }

        protected UserStatusException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public UserStatusException(string message, Exception inner)
            : base(message, inner)
        { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            base.GetObjectData(info, context);

            if (info.GetValue("CustomMessage", typeof(string)) == null)
            {
                info.AddValue("CustomMessage", this.CustomMessage);
            }
        }

    }

}


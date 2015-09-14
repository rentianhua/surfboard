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
using System.Data;
using System.Windows.Forms;
using HiiP.Foundation.DMS.Interface.Services;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Modules.ExternalParty.BusinessEntity.BusinessEntities;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using UserManagementConstants = HiiP.Framework.Security.UserManagement.Interface.Constants;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class UserDetailPresenter : Presenter<IUserDetail>
    {
        private UserRoleAssignment _userRoleAssignment;
        private DMSUserRoleAssignment _dmsuserRoleAssignment;
        private WFUserRoleAssignment _wfuserRoleAssignment;
        private GISUserRoleAssignment _gisuserRoleAssignment;
        private string _reportsTo = string.Empty;


        public override AppTitleData GetAppTitle()
        {
            AppTitleData appTitleData = base.GetAppTitle();
            switch (CurrentViewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add:
                    // New User
                    appTitleData = new AppTitleData(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.NewUserFunctionName, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.NewUserFunctionScreenID);
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update:
                    // Update User
                    appTitleData = new AppTitleData(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionName, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionScreenID).Format(Key);
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:
                    // Copy User
                    appTitleData = new AppTitleData(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.CopyUserFunctionName, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.CopyUserFunctionScreenID);
                    break;
            } 


            return appTitleData;
        }

        /// <summary>
        /// Initialize view data
        /// </summary>
        protected override void InitData()
        {
            base.InitData();

            // Load a new workspace
            View.LoadViewWorkspace();
            View.LoadViewDMSWorkspace();
            View.LoadViewWFWorkspace();
            View.LoadViewGISWorkspace();
            // Dropdownlist control data
            View.LoadViewDropDownList();

            // Load general user info or not
            switch (CurrentViewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add:
                    // New User
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update:
                    // Update User
                    // View.LoadETRoles(Key);
                    View.LoadUserData(Key);
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:
                    // View.LoadUserData(Key);
                    break;
            }

            // Share view in the current view Functional Role Assignment 
            if (WorkItem.Workspaces.Contains(String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.UserRoleAssignmentDeckWorkspace, Key)))
            {
                WorkItem.Workspaces.Get(
                    String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.UserRoleAssignmentDeckWorkspace, Key)
                    );
            }
            else
            {
                WorkItem.Workspaces.Add(
                    View.UserRoleAssignmentWorkspace,
                    String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.UserRoleAssignmentDeckWorkspace, Key)
                    );
            }

            // Share view in the current view DMS Role Assignment 
            if (WorkItem.Workspaces.Contains(String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.DMSUserRoleAssignmentDeckWorkspace, Key)))
            {
                WorkItem.Workspaces.Get(
                    String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.DMSUserRoleAssignmentDeckWorkspace, Key)
                    );
            }
            else
            {
                WorkItem.Workspaces.Add(
                    View.DMSUserRoleAssignmentWorkspace,
                    String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.DMSUserRoleAssignmentDeckWorkspace, Key)
                    );
            }

            // Share view in the current view Workflow Role Assignment 
            if (WorkItem.Workspaces.Contains(String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.WFUserRoleAssignmentDeckWorkspace, Key)))
            {
                WorkItem.Workspaces.Get(
                    String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.WFUserRoleAssignmentDeckWorkspace, Key)
                    );
            }
            else
            {
                WorkItem.Workspaces.Add(
                    View.WFUserRoleAssignmentWorkspace,
                    String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.WFUserRoleAssignmentDeckWorkspace, Key)
                    );
            }

            // Share view in the current view GIS Role Assignment 
            if (WorkItem.Workspaces.Contains(String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.GISUserRoleAssignmentDeckWorkspace, Key)))
            {
                WorkItem.Workspaces.Get(
                    String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.GISUserRoleAssignmentDeckWorkspace, Key)
                    );
            }
            else
            {
                WorkItem.Workspaces.Add(
                    View.GISUserRoleAssignmentWorkspace,
                    String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.GISUserRoleAssignmentDeckWorkspace, Key)
                    );
            }

            // Load Workspace Functional Role Assignment 
            ViewParameter parameter = new ViewParameter();
            parameter.ViewId = String.Format("User Role Assignment - {0}", Key);
            parameter.Key = Key;
            parameter.Data = Data;
            parameter.CurrentViewStatus = CurrentViewStatus;
            parameter.WorkItem = WorkItem;
            parameter.WorkspaceName = String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.UserRoleAssignmentDeckWorkspace, Key);
            _userRoleAssignment = ShowViewInWorkspace<UserRoleAssignment>(parameter);

            // Load Workspace DMS Role Assignment 
            parameter.ViewId = String.Format("DMS Role Assignment - {0}", Key);
            parameter.Key = Key;
            parameter.Data = Data;
            parameter.CurrentViewStatus = CurrentViewStatus;
            parameter.WorkItem = WorkItem;
            parameter.WorkspaceName = String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.DMSUserRoleAssignmentDeckWorkspace, Key);
            _dmsuserRoleAssignment = ShowViewInWorkspace<DMSUserRoleAssignment>(parameter);

           

            // Load Workspace GIS Role Assignment 
            parameter.ViewId = String.Format("GIS Role Assignment - {0}", Key);
            parameter.Key = Key;
            parameter.Data = Data;
            parameter.CurrentViewStatus = CurrentViewStatus;
            parameter.WorkItem = WorkItem;
            parameter.WorkspaceName = String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.GISUserRoleAssignmentDeckWorkspace, Key);
            _gisuserRoleAssignment = ShowViewInWorkspace<GISUserRoleAssignment>(parameter);

            // Control buttons on the view
            View.ControlViewStatus();



            if (CurrentViewStatus.Equals(HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser))
            {
                View.ActivateFunctionalRoleTab();
            }

            //Load organisation
            View.LoadOrganisationData();

            // Load Workspace Workflow Role Assignment 
            parameter.ViewId = String.Format("Workflow Role Assignment - {0}", Key);
            parameter.Key = Key;
            parameter.Data = Data;
            parameter.CurrentViewStatus = CurrentViewStatus;
            parameter.WorkItem = WorkItem;
            parameter.WorkspaceName = String.Format("{0} - {1}", HiiP.Framework.Security.UserManagement.Constants.WorkspaceNames.WFUserRoleAssignmentDeckWorkspace, Key);
            _wfuserRoleAssignment = ShowViewInWorkspace<WFUserRoleAssignment>(parameter);

            //Load ET roles data
            // View.LoadETRoles();
        }

        #region Event Publication

        [EventPublication(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.UserToAdd, PublicationScope.Global)]
        public event EventHandler<EventArgs<string>> UserToAdd;

        protected virtual void OnUserToAdd(string user)
        {
            if (UserToAdd != null)
                UserToAdd(this, new EventArgs<string>(user));
        }

        #endregion

        #region Business Logic


        internal void RegisterEventForDataControl(Control control)
        {
            RegisterEventForDataControl(this.View as BaseView,control);
        }

        /// <summary>
        /// Get user info by username
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>UserInfoEntity</returns>
        internal UserInfoEntity GetUserInfoByUserName(string username)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                return proxy.GetUser(username);
            }
        }
        
        /// <summary>
        /// Save user(New, Update, CopyNew)
        /// </summary>
        internal Dictionary<string, string> SaveUser()
        {

            Dictionary<string, string> MessageString = new Dictionary<string, string>();
            OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, newOfficeList;

            this.View.CollectOrganisationData(out removedOfficeList, out newOfficeList);

            switch (CurrentViewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add:
                    MessageString = AddUser(removedOfficeList, newOfficeList);
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update:
                    MessageString = UpdateUser(removedOfficeList, newOfficeList);
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:
                  
                    MessageString = AddUser(removedOfficeList, newOfficeList);
                    break;
            }
            try
            {
                if (_wfuserRoleAssignment.IsLoaded)
                    this.UpdateParticipation();
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleWithLogOnly(ex);
            }
            return MessageString;
        }

        #region Add, Update and Copy user action

        /// <summary>
        /// Add users
        /// </summary>
        private Dictionary<string, string> AddUser(OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            Dictionary<string, string> MessageString ;
            UserInfoEntity userInfoEntity = View.GetUserInfo();
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                if (userInfoEntity.ReportsTo == null) userInfoEntity.ReportsTo = string.Empty;
                MessageString = proxy.CreateUserInfo(userInfoEntity, _userRoleAssignment.DataFilterEntities, _userRoleAssignment.GetAssignedRoles(), _wfuserRoleAssignment.IsWorkflowAdministrator(), _wfuserRoleAssignment.GetSupervisedGroups(), _wfuserRoleAssignment.GetAssignedGroups(), _dmsuserRoleAssignment.GetAssignedRoles(), _gisuserRoleAssignment.GetAssignedGISRoles(), removedOfficeList, newOfficeList);
                OnUpdateStatusBarMessage(Messages.Framework.FWI201.Format());
                foreach (KeyValuePair<string, string> kvp in MessageString)
                {
                    if (kvp.Key.Equals(Constants.EventTopicNames.DMSUserCreationException))
                        OnUpdateStatusBarMessage(Constants.EventTopicNames.DMSUserCreationException);
                    if (kvp.Key.Equals(Constants.EventTopicNames.WorkflowUserCreationException))
                        OnUpdateStatusBarMessage(Constants.EventTopicNames.WorkflowUserCreationException);
                }

                OnUserToAdd(userInfoEntity.UserName);
            }
            return MessageString;
        }

        /// <summary>
        /// Update user
        /// </summary>
        private Dictionary<string, string> UpdateUser(OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            Dictionary<string, string> MessaegeSting = new Dictionary<string, string>();
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                // Check concurrent operation
                UserInfoEntity currentUser = proxy.GetUser(Key);

                int currentVersionNo = currentUser.VersionNo;

                if (View.VersionNo < currentVersionNo)
                {
                    Utility.ShowMessageBox(Messages.Framework.FWW001, Key);
                }
                else
                {
                    string[] roles;
                    if (_userRoleAssignment.IsLoaded)
                    {
                        roles = _userRoleAssignment.GetAssignedRoles();

                    }
                    else
                    {
                        roles = null;
                    }
                    MessaegeSting = proxy.UpdateUserInfo(View.GetUpdatedUserInfo(currentUser), _userRoleAssignment.DataFilterEntities, roles, _wfuserRoleAssignment.IsWorkflowAdministrator(), _wfuserRoleAssignment.GetSupervisedGroups(), _wfuserRoleAssignment.GetAssignedGroups(), _wfuserRoleAssignment.OldParticipations, _wfuserRoleAssignment.GetParticipation(), _dmsuserRoleAssignment.GetAssignedRoles(), _wfuserRoleAssignment.IsLoaded, _dmsuserRoleAssignment.IsLoaded, _gisuserRoleAssignment.GetAssignedGISRoles(), _gisuserRoleAssignment.GetUnassignedGISRoles(), removedOfficeList, newOfficeList);

                    OnUpdateStatusBarMessage(Messages.Framework.FWI202.Format(Key));
                    foreach (KeyValuePair<string, string> kvp in MessaegeSting)
                    {
                        if (kvp.Key.Equals(Constants.EventTopicNames.DMSUserUpdateException))
                            OnUpdateStatusBarMessage(Constants.EventTopicNames.DMSUserUpdateException);
                        if (kvp.Key.Equals(Constants.EventTopicNames.WorkflowUserUpdateException))
                            OnUpdateStatusBarMessage(Constants.EventTopicNames.WorkflowUserUpdateException);
                    }

                }
            }

            OnUserToAdd(Key);

            return MessaegeSting;
        }

        #endregion

        internal void DisableOrActivateUser()
        {
            string userstatus = View.GetUserStatus();
            List<string> usernames = new List<string>();
            usernames.Add(Key);
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                proxy.UpdateUserStatus(usernames, userstatus);
            }

            switch (userstatus)
            {
                case UserStatus.Inactive:
                    OnUpdateStatusBarMessage(Messages.Framework.FWI203.Format(Key));
                    break;
                case UserStatus.Active:
                    OnUpdateStatusBarMessage(Messages.Framework.FWI204.Format(Key));
                    break;
            }

            OnUserToAdd(Key);
        }

        internal bool DeletionCriteriaCheck(string userName, out string message)
        {
            if (View.GetUserStatus() == UserStatus.Inactive)
            {
                using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                {
                    return proxy.DeletionCriteriaCheck(userName, out message);
                }
            }
            message = "";
            return true;

        }

        /// <summary>
        /// Show user to update view
        /// </summary>
        public void CopyUser()
        {
            ViewParameter parameter = new ViewParameter();
            parameter.Key = Guid.NewGuid().ToString();
            parameter.Data = Key;
            parameter.CurrentViewStatus = HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser;
            ShowViewInWorkspace<UserDetail>(parameter);
        }

        #endregion

        internal bool UserExists(string username)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                return proxy.UserExists(username);
            }
        }

        internal bool HasRight(string ActionCode)
        {
            return UIAccessControl.IsAuthorised(ActionCode);  // to verify true / false ?
        }

        internal bool IsUserEditable()
        {
            return Key.ToLower() != AppContext.Current.UserName.ToLower();
        }

        public string ShowReportsTo(string reportsTo)
        {

            ViewParameter para = new ViewParameter("ReportToForm");
            para.WorkspaceName = HiiP.Infrastructure.Interface.Constants.WorkspaceNames.ModalWindows;
            para.Key = reportsTo;

            para.CurrentViewStatus = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ETRolesFunctionID;
            ForwardDestination _ForwardDestination = ShowViewInWorkspace<ForwardDestination>(para);
            _reportsTo = _ForwardDestination.ReportsTo;
            return _reportsTo;

        }

        public void SetHelpUrlForSubTab(string key, Control tabControl)
        {
            string screenID = "";
            switch (key)
            {
                case "General":
                    screenID = UserManagementConstants.FunctionNames.GeneralUserInfoScreenID;
                    break;
                case "Offices":
                    screenID = UserManagementConstants.FunctionNames.OfficesScreenID;
                    break;
                case "FuncRoles":
                    screenID = UserManagementConstants.FunctionNames.AssignRolesToUsersFunctionScreenID;
                    break;
                case "WfGroups":
                    screenID = UserManagementConstants.FunctionNames.WfGroupScreenID;
                    break;
                case "DMSRoles":
                    screenID = UserManagementConstants.FunctionNames.DMSRolesScreenID;
                    break;
                case "GISRoles":
                    screenID = UserManagementConstants.FunctionNames.GISRolesScreenID;
                    break;
                default:
                    break;
            }

            SetHelpUrl(tabControl, screenID);
        }

        public override void OnCloseView()
        {
            base.OnCloseView();
        }


        public void UpdateParticipation()
        {
            

            UserInfoEntity userInfoEntity = View.GetUserInfo();
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                if (_wfuserRoleAssignment != null)
                {
                    if (_wfuserRoleAssignment.OldParticipations != null)
                    {
                        proxy.UpdateParticipation(userInfoEntity, _wfuserRoleAssignment.OldParticipations, _wfuserRoleAssignment.GetParticipation(), _wfuserRoleAssignment.GetSupervisedUsers());
                    }
                }
            }
        }

        internal bool IsViewFunctionDFEnable()
        {
            string role = _userRoleAssignment.GetCurrentRole();

            if (string.IsNullOrEmpty(role))
            {
                string[] roles = _userRoleAssignment.GetAssignedRoles();
                return (roles.Length >= 1);
            }

            return true;

        }

        internal void ViewFunctionDF()
        {
                   
            string role = _userRoleAssignment.GetCurrentRole();
            if (string.IsNullOrEmpty(role))
            {
                string[] roles = _userRoleAssignment.GetAssignedRoles();
                if (roles.Length == 1)
                    role = roles[0].ToString();
                else
                {
                    MessageBox.Show(
                        "Please double click on the role you want to view!",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }
            ViewParameter parameter = new ViewParameter(String.Format("{0}.{1}", HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ListOfFunctionAndDataFilterFunctionID, role));
            parameter.Key = role;
            ShowViewInWorkspace<ListOfFunctionAndDataFilter>(parameter);
        }

        #region office
        public OfficesHierarchyDataSet GetOfficesAllHierarchy(string userName)
        {
            OfficesHierarchyDataSet officesHierarchyDataSet ;
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                officesHierarchyDataSet = proxy.GetOfficesAllHierarchy(userName);
            }
            return officesHierarchyDataSet;
        }

        public void DisableDMSUser(string username)
        {
            var cacheService = WorkItem.Services.Get<IDMSService>();
            cacheService.DisableTrimUser(username);
        }

        public void EnableDMSUser(string username)
        {

           var cacheService = WorkItem.Services.Get<IDMSService>();
            cacheService.EnableTrimUser(username);
 
        }

        


        #endregion



        public void OnShowSearchExternalParty(string ViewId)
        {
            ExternalPartyParameter para = new ExternalPartyParameter();
            para.CallerFunction = ExternalPartyParameter.ShowExternalPartySearchFromUserManagement;
            para.TargetViewId = ViewId;
            para.WorkspaceName = this.WorkspaceName;
            para.WorkItem = this.WorkItem;
            para.ViewId = "ExternalParty" + ViewId;
            WorkItem.EventTopics[HiiP.Modules.Common.Interface.Constants.EventTopicNames.ShowSearchExternalPartyEvent].Fire(this, new EventArgs<ViewParameter>(para), this.WorkItem, PublicationScope.Global);
        }

        [EventSubscription(HiiP.Modules.Common.Interface.Constants.EventTopicNames.ReturnExternalPartyEvent, ThreadOption.UserInterface)]
        public void ReturnExternalPartyEventEventHandler(object sender, EventArgs<ViewParameter> eventArgs)
        {
            try
            {

                var para = eventArgs.Data as ExternalPartyParameter;
                if (para==null)
                {
                    return;
                }

                if (para.TargetViewId != ((BaseView) this.View).ViewId)

                    return;

                ExternalPartyInfoDataSet ds = para.Data as ExternalPartyInfoDataSet;

                if ((ds != null) && (ds.EP_EPCore != null) && (ds.EP_EPCore.Count == 1))
                {
                    View.FillExternalParty(string.Concat(ds.EP_EPCore[0].EPRIN.ToString(), "-", ds.EP_EPCore[0].EPName));
                    //GetEPName(ds.EP_EPCore[0].EPRIN);
                }
            }
            catch (Exception ex)
            {

                if (ExceptionManager.Handle(ex)) throw;
            }


        }

        public string GetEPName(int? EPRIN)
        {            
            int _epRin = Convert.ToInt32(EPRIN);

           using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
           {
               return proxy.GetEPName(_epRin);
           }

        }

        #region Branch, Unit, Subunit and Grade Events

        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllBranch()
        {
            using (DelegationServiceProxy Proxy = new DelegationServiceProxy())
            {
                LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable tblLOUD = Proxy.GetAllBranch();
                return (LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable)AddSelectStr(tblLOUD).Table;
            }
        }

        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllUnit(string OrganisationalUnitID)
        {

            using (DelegationServiceProxy Proxy = new DelegationServiceProxy())
            {
                LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable tblLOUD = Proxy.GetAllUnit(OrganisationalUnitID);
                //return (LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable)AddSelectStr(tblLOUD).Table;
                return tblLOUD;
            }
        }

        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllSubUnit(string OrganisationalUnitID)
        {
            using (DelegationServiceProxy Proxy = new DelegationServiceProxy())
            {
                LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable tblLOUD = Proxy.GetAllSubUnit(OrganisationalUnitID);
                //return (LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable)AddSelectStr(tblLOUD).Table;
                return tblLOUD;
            }
        }

        private DataView AddSelectStr(LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                LookupOrganisationalUnitDataSet.LookupOrganisationalUnitRow rw = dt.NewLookupOrganisationalUnitRow();
                rw.OrganisationalUnitName = "Select a value";
                rw.OrganisationalUnitID = "0";
                dt.Rows.Add(rw);

                //Sort order by OrganisationalUnitID
                dt.DefaultView.Sort = "OrganisationalUnitID";
            }

            return dt.DefaultView;
        }

        #endregion


    }
}

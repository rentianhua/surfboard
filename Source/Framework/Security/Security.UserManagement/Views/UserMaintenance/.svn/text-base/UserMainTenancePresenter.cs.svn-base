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

using HiiP.Foundation.DMS.Interface.Services;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.ServiceProxies;

using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;

using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Constant = HiiP.Framework.Security.UserManagement.Interface.Constants;



namespace HiiP.Framework.Security.UserManagement
{
    public partial class UserMaintenancePresenter : Presenter<IUserMaintenance>
    {
        public override AppTitleData GetAppTitle()
        {
            return new AppTitleData(
                HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SearchUserFunctionName,
                HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SearchUserFunctionScreenID);
        }

        internal string ViewStatus { get; set; }

        protected override void InitData()
        {
            View.AccessControl(
                UIAccessControl.IsAuthorised(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ActivateOrDisableUsersFunctionID),
                UIAccessControl.IsAuthorised(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionID));

            View.SetUserTypeList();

            View.SetCodeTableControls(HiiP.Infrastructure.Interface.Constants.CodeTableCategoryNames.UserStatus, false);

            View.Show4CommonSearch(this.ViewStatus == Constant.ViewStatus.Show4CommonSearch);
        }

        #region Event Subscription

        [EventSubscription(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.UserToAdd, ThreadOption.UserInterface)]
        public void OnUserToAdd(object sender, EventArgs<string> eventArgs)
        {
            try
            {
                View.ShowUserList();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
            }
            
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Update users' status
        /// </summary>
        internal void DisableOrActivateUser(List<string> usernames, string userstatus)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                proxy.UpdateUserStatus(usernames, userstatus);
            }
            switch (userstatus)
            {
                case UserStatus.Inactive:
                    ChangeDMSUserStauts(usernames, true);
                    OnUpdateStatusBarMessage(Messages.Framework.FWI205.Format());
                    break;
                case UserStatus.Active:
                    ChangeDMSUserStauts(usernames, false);
                    OnUpdateStatusBarMessage(Messages.Framework.FWI206.Format());
                    break;
            }
        }

        internal void ChangeDMSUserStauts(List<string> usernames,bool isDisable)
        {
            var cacheService = WorkItem.Services.Get<IDMSService>();

            foreach (string user in usernames)
            {

                if (isDisable)
                {
                    cacheService.DisableTrimUser(user);
                }
                else
                {
                    cacheService.EnableTrimUser(user);
                }
            }
        }
        

        public UserInfoEntity[] GetUserInfoArrayEntity(UserInfoSearchCriteria userInfoSearchCriteria)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                UserInfoEntity[] userInfoEntitys = proxy.FindUsers(
                    userInfoSearchCriteria);
                return userInfoEntitys;
            }
        }

        /// <summary>
        /// Show user to update view
        /// </summary>
        public void RoleAssignment(string[] userNames)
        {
            ViewParameter parameter = new ViewParameter();
            parameter.Data = userNames;
            parameter.CurrentViewStatus = HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.AssignRolesToUsers;
            ShowViewInWorkspace<BatchRoleAssignment>(parameter);
        }

        internal void ShowUpdateUserView(string username)
        {
            if (this.ViewStatus == Constant.ViewStatus.Show4CommonSearch)
            {
                //Disable double-click event to "maintain user" screen
                return;
            }
            ViewParameter parameter = new ViewParameter(String.Format("{0}.{1}", HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionID, username));
            parameter.Key = username;
            parameter.CurrentViewStatus = HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update;
            ShowViewInWorkspace<UserDetail>(parameter);
        }

        internal bool DeletionCriteriaCheck(string userName, out string message)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                return proxy.DeletionCriteriaCheck(userName, out message);
            }
        }

        #endregion
    }
}


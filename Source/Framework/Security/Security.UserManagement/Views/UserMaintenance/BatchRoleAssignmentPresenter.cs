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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Security.UserManagement.Interface;
using System.Collections.Generic;
using HiiP.Framework.Common.ApplicationContexts;
using System.Collections;
using Microsoft.Practices.CompositeUI.EventBroker;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Infrastructure.Interface.Constants;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Framework.Messaging;
using System.Linq;
using HiiP.Framework.Logging.Library;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class BatchRoleAssignmentPresenter : Presenter<IBatchRoleAssignment>
    {
        private UserRoleAssignment _userRoleAssignment;

        public override AppTitleData GetAppTitle()
        {
            return new AppTitleData(
                HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionName,
                HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionScreenID
                );
        }

        protected override void InitData()
        {
            base.InitData();

            View.LoadViewWorkspace();

            View.LoadSelectedUsers();

            // Share view in the current view
            WorkItem.Workspaces.Add(View.AssignRolesToUsersWorkspace, View.AssignRolesToUsersWorkspaceName);
            ViewParameter parameter = new ViewParameter();
            parameter.CurrentViewStatus = CurrentViewStatus;
            parameter.WorkspaceName = View.AssignRolesToUsersWorkspaceName;
            _userRoleAssignment = ShowViewInWorkspace<UserRoleAssignment>(parameter);
        }

        #region Business Logic

        internal UserInfoEntity[] GetUserEntitys()
        {
            string[] users = Data as string[];
            //List<UserInfoEntity> userEntities = new List<UserInfoEntity>();
            if (users != null)
            {
                UserInfoEntity[] allUserInfoEntity ;

                Guid id = Utility.SetContextValues();
                using (new MonitoringTracer(id, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SearchUserFunctionID, ComponentType.Screen))
                {
                    using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                    {
                        allUserInfoEntity = proxy.FindUsers(new UserInfoSearchCriteria());
                        return proxy.GetUsers(allUserInfoEntity, users);
                    }
                }
            }

            return new UserInfoEntity[]{};
        }

        internal void AssignRolesToUsers()
        {
            string[] selectedUsers = Data as string[];
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SecurityModuleID, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionID, ComponentType.Screen))
            {
                using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                {
                    proxy.UpdateRolesForUsers(selectedUsers, _userRoleAssignment.GetAssignedRoles());
                    proxy.AssignUsersDataFilterValues(selectedUsers, _userRoleAssignment.DataFilterEntities);
                }
            }
            
            OnUpdateStatusBarMessage(Messages.Framework.FWI207.Format());
        }

        #endregion
    }
}


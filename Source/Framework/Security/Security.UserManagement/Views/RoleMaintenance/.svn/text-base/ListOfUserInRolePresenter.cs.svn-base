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

using HiiP.Infrastructure.Interface;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Common.Client;

using Microsoft.Practices.CompositeUI.EventBroker;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class ListOfUserInRolePresenter : Presenter<IListOfUserInRole>
    {
        public override AppTitleData GetAppTitle()
        {
            return new AppTitleData(Key,
                FunctionNames.ListOfUserInRoleFunctionScreenID
                );
        }

        #region Event Subscription

        [EventSubscription(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.RoleUpdateOrDelete, ThreadOption.UserInterface)]
        public void RoleUpdateOrDeleteHandler(object sender, EventArgs<string> e)
        {
            try
            {
                View.ViewUserList();
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
        /// search user list by rolename
        /// </summary>
        /// <param name="roleName">roleNameToMatch</param>
        /// <returns>UserEntity[]</returns>
        internal UserInfoEntity[] GetUserInRoleByRoleName(string roleName)
        {
            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.RoleModuleID, FunctionNames.ListOfUserInRoleFunctionID, ComponentType.Screen))
            {
                using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                {
                    return proxy.GetUserInRoleByRoleName(roleName);
                }
            }
        }

        internal void ShowUpdateUserView(string username)
        {
            ViewParameter parameter = new ViewParameter(String.Format("{0}.{1}", HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionID, username));
            parameter.Key = username;
            parameter.CurrentViewStatus = HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update;
            ShowViewInWorkspace<UserDetail>(parameter);
        }
        #endregion
    }
}


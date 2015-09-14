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
using System.Windows.Forms;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using Microsoft.Practices.CompositeUI.EventBroker;
using NCS.IConnect.Security;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class RoleMaintenancePresenter : Presenter<IRoleMaintenance>
    {
        public override AppTitleData GetAppTitle()
        {
            return new AppTitleData(
                FunctionNames.SearchRoleFunctionName,
                FunctionNames.SearchRoleFunctionScreenID
                );
        }

        #region Event Subscription

        [EventSubscription(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.RoleUpdateOrDelete, ThreadOption.UserInterface)]
        public void RoleUpdateOrDeleteHandler(object sender, EventArgs<string> e)
        {
            try
            {
                View.FindRoleList();
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
        /// search role list by rolename and description
        /// </summary>
        /// <param name="roleNameToMatch">roleNameToMatch</param>
        /// <param name="descriptionToMatch">descriptionToMatch</param>
        /// <returns>RoleEntity[]</returns>
        internal RoleEntity[] FindRoleListByConditions(string roleNameToMatch, string descriptionToMatch)
        {
            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.RoleModuleID, FunctionNames.SearchRoleFunctionID, ComponentType.Screen))
            {
                using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                {
                    return proxy.FindRolesByConditions(roleNameToMatch, descriptionToMatch);
                }
            }
        }

        ///// <summary>
        ///// search role list by rolename and description
        ///// </summary>
        ///// <param name="roleNameToMatch">roleNameToMatch</param>
        ///// <returns>RoleEntity[]</returns>
        //internal RoleEntity[] FindRoleListByConditions(RoleEntity searchConditionEntity)
        //{
        //    Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
        //    using (new MonitoringTracer(id, FunctionNames.RoleModuleID, FunctionNames.SearchRoleFunctionID, ComponentType.Screen))
        //    {
        //        using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
        //        {
        //            return proxy.FindRolesByConditions(searchConditionEntity.RoleName, searchConditionEntity.Description, String.Empty);
        //        }
        //    }
        //}

        /// <summary>
        /// Show role to update view
        /// </summary>
        public void ShowUpdateRoleView(string roleName)
        {
            if (!ExtendedRoles.RoleExists(roleName))
            {
               HiiP.Framework.Common.Client.Utility.ShowMessageBox(Messages.Framework.FWW004,roleName);
               View.FindRoleList();
            }
            else
            {
                ViewParameter parameter = new ViewParameter(
                    String.Format("{0}.{1}", FunctionNames.UpdateRoleFunctionID, roleName));
                parameter.Key = roleName;
                parameter.CurrentViewStatus = ViewStatus.Update;
                ShowViewInWorkspace<RoleDetail>(parameter);
            }
        }

        #endregion
    }
}


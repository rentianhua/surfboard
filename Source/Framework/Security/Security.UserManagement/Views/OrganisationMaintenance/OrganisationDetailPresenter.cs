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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Security.UserManagement.Interface;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Practices.CompositeUI.EventBroker;
using HiiP.Framework.Security.UserManagement.Constants;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using System.Windows.Forms;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Logging.Library;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class OrganisationDetailPresenter : Presenter<IOrganisationUpdate>
    {
        public override AppTitleData GetAppTitle()
        {
            AppTitleData appTitleData = base.GetAppTitle();

            switch (CurrentViewStatus)
            {
                case ViewStatus.Add:
                    View.SetOrgNameReadOnly(false);
                    appTitleData = new HiiP.Infrastructure.Interface.BusinessEntities.AppTitleData(
                        FunctionNames.NewOrganisationFunctionName,
                        FunctionNames.NewOrganisationFunctionScreenID
                        );
                    break;
                case ViewStatus.Update:
                    View.SetOrgNameReadOnly(true);
                    appTitleData = new HiiP.Infrastructure.Interface.BusinessEntities.AppTitleData(
                        FunctionNames.UpdateOrganisationFunctionName,
                        FunctionNames.UpdateOrganisationFunctionScreenID
                        ).Format(Key);
                    break;
            }

            return appTitleData;
        }

        protected override void InitData()
        {
            base.InitData();

            switch (CurrentViewStatus)
            {
                case ViewStatus.Add :
                    View.AccessControl(
                        UIAccessControl.IsAuthorised(FunctionNames.NewOrganisationFunctionID));
                    break;
                case ViewStatus.Update:
                    View.AccessControl(
                        UIAccessControl.IsAuthorised(FunctionNames.UpdateOrganisationFunctionID));
                    OrganisationEntity entity = new OrganisationEntity();
                    Guid id = Utility.SetContextValues();
                    using (new MonitoringTracer(id, FunctionNames.OrganisationModuleID, FunctionNames.SearchOrganisationFunctionID, ComponentType.Screen))
                    using(var proxy = new OrganisationMaintenanceServiceProxy())
                    {
                        entity = proxy.GetOrganisationByOrgName(Key);
                    }

                    View.LoadOrgUpdateData(entity);
                    break;
            }
        }

        #region Event Publication

        /// <summary>
        /// After op, refresh search, update UI etc.
        /// </summary>
        [EventPublication(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.OrgAddOrUpdateOrDelete, PublicationScope.Global)]
        public event EventHandler<EventArgs<OrganisationEntity>> OrgAddOrUpdateOrDelete;

        protected virtual void OnOrgAddOrUpdateOrDelete(OrganisationEntity org)
        {
            if (OrgAddOrUpdateOrDelete != null)
                OrgAddOrUpdateOrDelete(this, new EventArgs<OrganisationEntity>(org));
        }

        #endregion

        #region Business Logic

        internal void SaveOrganisation()
        {
            switch (CurrentViewStatus)
            {
                case ViewStatus.Add:
                    Guid id = Utility.SetContextValues();
                    using (new MonitoringTracer(id, FunctionNames.OrganisationModuleID, FunctionNames.NewOrganisationFunctionID, ComponentType.Screen))
                    using(var proxy = new OrganisationMaintenanceServiceProxy())
                    {
                        proxy.CreateOrganisationAndRoles(View.GetOrganisationEntity(), View.GetAssignedRoles());
                    }
                    
                    OnUpdateStatusBarMessage(Messages.Framework.FWI221.Format(View.GetOrganisationEntity().OrganisationName));
                    OnOrgAddOrUpdateOrDelete(null);
                    break;
                case ViewStatus.Update:
                    int currentVersionNo;
                    Guid activityId1 = Utility.SetContextValues();
                    using (new MonitoringTracer(activityId1, FunctionNames.OrganisationModuleID, FunctionNames.SearchOrganisationFunctionID, ComponentType.Screen))
                    using (var proxy = new OrganisationMaintenanceServiceProxy())
                    {
                         currentVersionNo = proxy.GetOrganisationByOrgName(Key).VersionNo;
                    }
                    
                    if (View.VersionNo < currentVersionNo)
                    {
                        MessageBox.Show(
                            Messages.Framework.FWC215.Format(Key),
                            "Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        OnOrgAddOrUpdateOrDelete(View.GetOrganisationEntity());
                    }
                    else
                    {
                        Guid activityId2 = Utility.SetContextValues();
                        using (new MonitoringTracer(activityId2, FunctionNames.OrganisationModuleID, FunctionNames.UpdateOrganisationFunctionID, ComponentType.Screen))
                        using(var proxy = new OrganisationMaintenanceServiceProxy())
                        {
                            proxy.UpdateOrganisationAndRoles(View.GetOrganisationEntity(), View.GetAssignedRoles());
                        }
                        
                        OnUpdateStatusBarMessage((Messages.Framework.FWI222.Format(View.GetOrganisationEntity().OrganisationName)));
                        OnOrgAddOrUpdateOrDelete(View.GetOrganisationEntity());
                    }
                    break;
            }
        }

        /// <summary>
        /// search role list by rolename and description
        /// </summary>
        /// <param name="roleNameToMatch">roleNameToMatch</param>
        /// <param name="descriptionToMatch">descriptionToMatch</param>
        /// <returns>RoleEntity[]</returns>
        internal RoleEntity[] FindRoleListByConditions(string roleNameToMatch, string descriptionToMatch)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.RoleModuleID, FunctionNames.SearchRoleFunctionID, ComponentType.Screen))
            using(var proxy = new UserMaintenanceServiceProxy())
            {
                return proxy.FindRolesByConditions(roleNameToMatch, descriptionToMatch, RoleStatus.Active);
            }
        }

        /// <summary>
        /// Get organisation's roles by organisation name
        /// </summary>
        /// <param name="orgName">OrganisationName</param>
        /// <returns>Roles</returns>
        internal List<RoleEntity> GetRolesByOrgName(string orgName)
        {
            using (var proxy = new OrganisationMaintenanceServiceProxy())
            {
                List<RoleEntity> roles = proxy.GetRolesByOrgName(orgName);

                return roles;
            }
        }

        #endregion

        internal bool OrganisationExists(string orgName)
        {
            using (var proxy = new OrganisationMaintenanceServiceProxy())
            {
                return proxy.OrganisationExists(orgName);
            }
        }
    }
}


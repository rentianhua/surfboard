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
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI.EventBroker;
using HiiP.Infrastructure.Interface.Services;
using HiiP.Framework.Security.UserManagement.Interface;
using HiiP.Framework.Security.UserManagement.Constants;
using System.Collections.Generic;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Common.Client;


namespace HiiP.Framework.Security.UserManagement
{
    public partial class OrganisationMaintenancePresenter : Presenter<IOrganisationMaintenance>
    {
        public override AppTitleData GetAppTitle()
        {
            return new HiiP.Infrastructure.Interface.BusinessEntities.AppTitleData(
                FunctionNames.SearchOrganisationFunctionName,
                FunctionNames.SearchOrganisationFunctionScreenID);
        }

        #region Event Subscription

        [EventSubscription(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.OrgAddOrUpdateOrDelete, ThreadOption.UserInterface)]
        public void OrgAddOrUpdateOrDeleteHandler(object sender, EventArgs<OrganisationEntity> eventArgs)
        {
            try
            {
                View.FindOrganisationList();
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
        /// Find organisations by org name and description
        /// </summary>
        /// <param name="orgName">orgName</param>
        /// <param name="orgDescription">orgDescription</param>
        /// <returns></returns>
        internal List<OrganisationEntity> FindeOrganisationsByConditions(string orgName, string orgDescription)
        {
            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.OrganisationModuleID, FunctionNames.SearchOrganisationFunctionID, ComponentType.Screen))
            using(var proxy = new OrganisationMaintenanceServiceProxy())
            {
                return proxy.FindOrganisationsByConditions(orgName, orgDescription);
            }
        }

        /// <summary>
        /// navigate to Organisation Update view.
        /// </summary>
        internal void UpdateOrganisation(string orgName)
        {
            ViewParameter parameter = new ViewParameter(
                String.Format("{0}.{1}", FunctionNames.UpdateOrganisationFunctionID, orgName));
            parameter.Key = orgName;
            parameter.CurrentViewStatus = ViewStatus.Update;
            ShowViewInWorkspace<OrganisationDetail>(parameter);
        }

        #endregion
    }
}


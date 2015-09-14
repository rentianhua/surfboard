#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Service proxy
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System.Collections.Generic;

using HiiP.Framework.Common;
using HiiP.Framework.Security.UserManagement.Interface;
using HiiP.Framework.Security.UserManagement.Interface.Constants;

using Microsoft.Practices.CompositeUI;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;

namespace HiiP.Framework.Security.UserManagement.ServiceProxies
{
    public class OrganisationMaintenanceServiceProxy : ServiceProxyBase<IOrganisationMaintenanceService>,
                                                  IOrganisationMaintenanceService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationMaintenanceServiceProxy"/> class.
        /// </summary>
        /// <param name="endpointName">Name of the endpoint.</param>
        protected OrganisationMaintenanceServiceProxy(string endpointName)
            : base(endpointName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationMaintenanceServiceProxy"/> class.
        /// </summary>
        public OrganisationMaintenanceServiceProxy()
        {
            WrapObject(new OrganisationMaintenanceServiceProxy(EndpointNames.OrganisationServiceEndpoint));
        }

        #region IOrganisationMaintenanceService Members

       
         /// <summary>
        /// check org name is unique?
        /// </summary>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.OrganisationModuleID,
            FunctionID = FunctionNames.ViewOrganisationFunctionID )]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable  GetOrganisationLookup()
        {
            return Proxy.GetOrganisationLookup();
        }


        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.OrganisationModuleID,
          FunctionID = FunctionNames.ViewOrganisationFunctionID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetOrganisationLookupByOrgID(string OrganisationalUnitID)
        {
            return Proxy.GetOrganisationLookupByOrgID(OrganisationalUnitID);
        }


        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.OrganisationModuleID,
           FunctionID = FunctionNames.UpdateOrganisationFunctionID)]
        public void SaveOrganisationLookup(LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtLookupOrganisationalUnitDataTable)
        {
            Proxy.SaveOrganisationLookup(dtLookupOrganisationalUnitDataTable);
        }
        #endregion
    }
}
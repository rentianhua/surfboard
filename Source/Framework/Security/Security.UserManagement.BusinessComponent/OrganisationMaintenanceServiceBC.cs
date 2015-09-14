#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Business component
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Text;

using HiiP.Framework.Common;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.DataAccess;

using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.Interface.Constants;

namespace HiiP.Framework.Security.UserManagement.BusinessComponent
{
    public class OrganisationMaintenanceServiceBC : HiiPBusinessComponentBase
    {
        private OrganisationMaintenanceServiceDA _orgMaintenanceServiceDA = 
            InstanceBuilder.Wrap<OrganisationMaintenanceServiceDA>(new OrganisationMaintenanceServiceDA());


        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.OrganisationModuleID,
FunctionID = FunctionNames.ViewOrganisationFunctionID)]
        public OfficeDetailEntity GetOfficeDetails(string officeCode)
        {
            return this._orgMaintenanceServiceDA.GetOfficeDetails(officeCode);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.OrganisationModuleID,
FunctionID = FunctionNames.MaintainOrganisationFunctionID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetOrganisationLookup()
        {
            return this._orgMaintenanceServiceDA.GetOrganisationLookup();
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.OrganisationModuleID,
FunctionID = FunctionNames.ViewOrganisationFunctionID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetOrganisationLookupByOrgID(string OrganisationalUnitID)
        {
            return this._orgMaintenanceServiceDA.GetOrganisationLookupByOrgID(OrganisationalUnitID);
        }


		 [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.OrganisationModuleID,
FunctionID = FunctionNames.UpdateOrganisationFunctionID)]
        public void SaveOrganisationLookup(LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtLookupOrganisationalUnitDataTable)
         {
             this._orgMaintenanceServiceDA.SaveOrganisationLookup(dtLookupOrganisationalUnitDataTable);
         }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.OrganisationModuleID,
FunctionID = FunctionNames.UpdateOrganisationFunctionID)]
         public void SaveOrganisationLookup(string OrganisationalUnitID, string OrganisationName, int VersionNumber)
         {
             this._orgMaintenanceServiceDA.SaveOrganisationLookup(OrganisationalUnitID, OrganisationName, VersionNumber);
         }

    }
}

#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Interface
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System.Collections.Generic;
using System.ServiceModel;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using NCS.IConnect.Common;
using NCS.IConnect.Validation.Validators;

namespace HiiP.Framework.Security.UserManagement.Interface
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface IOrganisationMaintenanceService
    {
 
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetOrganisationLookup();

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetOrganisationLookupByOrgID(string OrganisationalUnitID);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void SaveOrganisationLookup([DataTableValidator("Organisation Detail Rule Set")]
            [DataTableSchemaValidator()]
// ReSharper disable BitwiseOperatorOnEnumWihtoutFlags
            [DataTableOperationValidator(AllowedOperation.Add | AllowedOperation.Update)]
// ReSharper restore BitwiseOperatorOnEnumWihtoutFlags
            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtLookupOrganisationalUnitDataTable);
    }
}
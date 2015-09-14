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

using HiiP.Framework.Security.UserManagement.BusinessEntity;
using Infragistics.Win.UltraWinEditors;

namespace HiiP.Framework.Security.UserManagement
{
    public interface IOrganisationUpdate
    {
        void LoadOrgUpdateData(OrganisationEntity orgEntity);
        OrganisationEntity GetOrganisationEntity();
        string[] GetAssignedRoles();
        void FindRoleList();
        void ResetRoleList();

        int VersionNo { get; }
        void SetOrgNameReadOnly(bool readOnly);

        void AccessControl(bool isDo);
    }
}


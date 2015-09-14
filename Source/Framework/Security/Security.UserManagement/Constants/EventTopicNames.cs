#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

namespace HiiP.Framework.Security.UserManagement.Constants
{
    /// <summary>
    /// Constants for event topic names.
    /// </summary>
    public static class EventTopicNames 
    {
       
        public const string UserToAdd = "UserToAdd";
        public const string RoleUpdateOrDelete = "RoleUpdateOrDelete";
        public const string OrgAddOrUpdateOrDelete = "OrgAddOrUpdateOrDelete";
        public const string OfficeAddOrUpdateOrDelete = "OfficeAddOrUpdateOrDelete";
        public const string UpdateOrganisationName = "UpdateOrganisationName";
        public const string EnableDisableViewDF = "EnableDisableViewDF";


        public const string WorkflowUserCreationSuccess = "Workflow user creation successful";
        public const string WorkflowUserCreationException = "Workflow user creation failed";

        public const string DMSUserCreationSuccess = "DMS user creation successful";
        public const string DMSUserCreationException = "DMS user creation failed";

        public const string HiiPUserCreationSuccess = "HiiP user creation successful";
        public const string HiiPUserCreationException = "HiiP user creation failed";

        public const string WorkflowUserUpdateSuccess = "Workflow user creation successful";
        public const string WorkflowUserUpdateException = "Workflow user creation failed";

        public const string DMSUserUpdateSuccess = "DMS user creation successful";
        public const string DMSUserUpdateException = "DMS user creation failed";

        public const string HiiPUserUpdateSuccess = "HiiP user creation successful";
        public const string HiiPUserUpdateException = "HiiP user creation failed";
    }
}

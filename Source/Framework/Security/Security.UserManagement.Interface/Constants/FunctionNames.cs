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

using System;
using System.Collections.Generic;
using System.Text;

namespace HiiP.Framework.Security.UserManagement.Interface.Constants
{
    public sealed class FunctionNames : HiiP.Infrastructure.Interface.Constants.FunctionNames
    {
        private FunctionNames()
        {}
        public const string OrganisationModuleID = AdminModuleID + ".Organisation";
        public const string OrganisationModuleName = "User";
        public const string OrganisationModuleScreenID = "ORG000";

        public const string ViewOrganisationFunctionID = OrganisationModuleID + ".ViewOrganisation";

        public const string MaintainOrganisationFunctionID = OrganisationModuleID + ".MaintainOrganisationName";
        public const string MaintainOrganisationFunctionName = "Maintain organisation name";
        public const string MaintainOrganisationFunctionScreenID = "FW-ORM-005";

        public const string UpdateOrganisationFunctionID = OrganisationModuleID + ".UpdateOrganisation";
        public const string UpdateOrganisationFunctionName = "Update organisation";
        public const string UpdateOrganisationFunctionScreenID = "FW-ORM-003";

        public const string SecurityModuleID = AdminModuleID + ".Security";
        public const string SecurityModuleName = "Security";
        public const string SecurityModuleScreenID = "SEC000";

        public const string UserModuleID = SecurityModuleID + ".User";
        public const string UserModuleName = "User";
        public const string UserModuleScreenID = "USR000";

        public const string NewUserFunctionID = UserModuleID + ".NewUser";
        public const string NewUserFunctionName = "Create user";
        public const string NewUserFunctionScreenID = "FW-SEC-USRM-004";

        public const string SearchUserFunctionID = UserModuleID + ".SearchUser";
        public const string SearchUserFunctionName = "Maintain users";
        public const string SearchUserFunctionScreenID = "FW-SEC-USRM-001";

        public const string ViewUserFunctionID = UserModuleID + ".ViewUser";
        public const string ViewOfficeFunctionID = UserModuleID + ".ViewOffice";
        public const string ViewGroupCalendarFunctionID = UserModuleID + ".ViewGroupCalendar";
        public const string SaveGroupCalendarFunctionID = UserModuleID + ".SaveGroupCalendar";

        public const string UpdateUserFunctionID = UserModuleID + ".UpdateUser";
        public const string UpdateUserFunctionName = "Update user";
        public const string UpdateUserFunctionScreenID = "FW-SEC-USRM-003";

        public const string AssignRolesToUsersFunctionID = UserModuleID + ".AssignRolesToUsers";
        public const string AssignRolesToUsersFunctionName = "Assign roles To users";
        public const string AssignRolesToUsersFunctionScreenID = "FW-SEC-USRM-007";

        public const string ActivateOrDisableUsersFunctionID = UserModuleID + ".ActivateOrDisableUsers";
        public const string ActivateOrDisableUsersFunctionName = "Activate or disable users";
        public const string ActivateOrDisableUsersFunctionScreenID = "";

        public const string CopyUserFunctionID = UserModuleID + ".CopyUser";
        public const string CopyUserFunctionName = "Copy user";
        public const string CopyUserFunctionScreenID = "FW-SEC-USRM-006";

        public const string GeneralUserInfoScreenID = "FW-SEC-USRM-008";
        public const string OfficesScreenID = "FW-SEC-USRM-009";
        public const string WfGroupScreenID = "FW-SEC-USRM-010";
        public const string DMSRolesScreenID = "FW-SEC-USRM-011";
        public const string GISRolesScreenID = "FW-SEC-USRM-012";

        public const string GetUserInfoFunctionID = UserModuleID + ".GetUserInfo";
        public const string UpdateCurrentUserProfileFunctionID = UserModuleID + ".UpdateCurrentUserProfile";

        public const string GetAllUserInfoFunctionID = UserModuleID + ".GetAllUserInfo";
        public const string GetAllUserInfoFunctionName = "Get All User Information";

        public const string GetUserBasicInfoFunctionID = UserModuleID + ".GetBasicUserInfo";
        public const string GetUserBasicInfoFunctionName = "Get Basic User Information";

        public const string RoleModuleID = SecurityModuleID + ".Role";
        public const string RoleModuleName = "Role";
        public const string RoleModuleScreenID = "ROLE000";

        public const string ViewRoleFunctionID = RoleModuleID + ".ViewRole";

        public const string NewRoleFunctionID = RoleModuleID + ".NewRole";
        public const string NewRoleFunctionName = "Create role";
        public const string NewRoleFunctionScreenID = "FW-SEC-ROLM-004";

        public const string DuplicateRoleFunctionID = RoleModuleID + ".DuplicateRole";
        public const string DuplicateRoleFunctionName = "Copy role";
        public const string DuplicateRoleFunctionScreenID = "FW-SEC-ROLM-005";

        public const string SearchRoleFunctionID = RoleModuleID + ".SearchRole";
        public const string SearchRoleFunctionName = "Maintain roles";
        public const string SearchRoleFunctionScreenID = "FW-SEC-ROLM-001";

        public const string UpdateRoleFunctionID = RoleModuleID + ".UpdateRole";
        public const string UpdateRoleFunctionName = "Update role";
        public const string UpdateRoleFunctionScreenID = "FW-SEC-ROLM-003";

        public const string ListOfUserInRoleFunctionID = RoleModuleID + ".ListOfUserInRole";
        public const string ListOfUserInRoleFunctionName = "List User in Role";
        public const string ListOfUserInRoleFunctionScreenID = "FW-SEC-ROLM-004";

        public const string ListOfFunctionAndDataFilterFunctionID = RoleModuleID + ".ListOfFunctionAndDatafilter";
        public const string ListOfFunctionAndDataFilterFunctionName = "List Function and Data Filters";
        public const string ListOfFunctionAndDataFilterFunctionScreenID = "FW-SEC-USRM-013";

        public const string ETRolesFunctionID = RoleModuleID + "GetETRoles";
        public const string ETRolesFunctionName = "GetETRoles";
        public const string ETRolesFunctionScreenID = "FW-SEC-USRM-004";

        public const string GetCustomizationFunctionID = SecurityModuleID + "GetCustomization";
        public const string GetCustomizationFunctionName = "GetCustomization";

        public const string UpdateCustomizationFunctionID = SecurityModuleID + ".UpdateCustomization";
        public const string UpdateCustomizationFunctionName = "UpdateCustomization";

        public const string DelegationsModuleID = AdminModuleID + ".Delegations";
        public const string DelegationsModuleName = "Delegations";
        public const string DelegationsModuleScreenID = "FW-SEC-DELM-000";

        public const string MaintainDelegationFunctionID = DelegationsModuleID + ".MaintainDelegation";
        public const string MaintainDelegationFunctionName = "Maintain delegation";
        public const string MaintainDelegationFunctionScreenID = "FW-SEC-DELM-001";

        public const string CreateDelegationFunctionID = DelegationsModuleID + ".CreateDelegation";
        public const string CreateDelegationFunctionName = "Create delegation";
        public const string CreateDelegationFunctionScreenID = "FW-SEC-DELM-002";

        public const string UpdateDelegationFunctionID = DelegationsModuleID + ".UpdateDelegation";
        public const string UpdateDelegationFunctionName = "Update delegation";
        public const string UpdateDelegationFunctionScreenID = "FW-SEC-DELM-003";

    }
}

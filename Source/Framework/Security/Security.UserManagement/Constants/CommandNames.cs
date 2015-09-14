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
    /// Constants for command names.
    /// </summary>
    public static class CommandNames// : HiiP.Infrastructure.Interface.Constants.CommandNames
    {
        //private CommandNames()
        //{ }

        public const string NewUser = "AddUser";
        public const string SearchUser = "SearchUser";
        public const string NewRole = "AddRole";
        public const string SearchRole = "SearchRole";
        public const string MaintainOrganisation = "MaintainOrganisation";

        public const string MaintainDelegation = "MaintainDelegation";
        public const string CreateDelegation = "NewFinancialDelegation";

    }
}

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

using System.Collections;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using System.Collections.Generic;
using Infragistics.Win.UltraWinEditors;

namespace HiiP.Framework.Security.UserManagement
{
    public interface IRoleDetail
    {
        void LoadRoleData(RoleEntity roleEntity);

        void LoadRoleFunctions();

        List<string> ActionsInRole { get; set; }

        void DeleteRoleEnabled(bool isEnabled);

        int VersionNo { get; }

        void SetRoleNameReadOnly(bool readOnly);

        void AccessControl(bool isDelete, bool isDo, bool isDuplicate);

        void ViewUsersEnabled(bool isEnabled);
    }
}


#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/User maintenance
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System.Collections.Generic;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using System.Data;
using HiiP.Infrastructure.Interface.Miscellaneous;
using Infragistics.Win.UltraWinToolTip;

namespace HiiP.Framework.Security.UserManagement
{
    public interface IUserRoleAssignment
    {
        UltraToolTipManager ViewUltraToolTipManager { get; }

        void LoadCopyRoles(string userName);

        void FindRoleList();

        void ResetRoleList();

        string[] GetAssignedRoles();

        void InitAssignedRoles(DataTable dt);

        List<DataFilterEntity> DataFilterEntities { get; set; }

        string CurrentRoleNameForDataFilter { get; set; }
        void SetDirty();
    }
}


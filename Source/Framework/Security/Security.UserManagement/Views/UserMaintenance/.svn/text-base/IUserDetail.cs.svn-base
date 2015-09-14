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
using Microsoft.Practices.CompositeUI.SmartParts;

namespace HiiP.Framework.Security.UserManagement
{
    public interface IUserDetail
    {
        UserInfoEntity GetUserInfo();

        UserInfoEntity GetUpdatedUserInfo(UserInfoEntity userInfo);

        string GetUserStatus();

        void LoadViewWorkspace();
            
        void LoadViewDMSWorkspace();

        void LoadViewWFWorkspace();

        void LoadViewGISWorkspace();

        void LoadViewDropDownList();

        void LoadUserData(string userName);

        void ControlViewStatus();

        IWorkspace UserRoleAssignmentWorkspace { get; }

        IWorkspace DMSUserRoleAssignmentWorkspace { get; }

        IWorkspace WFUserRoleAssignmentWorkspace { get; }

        IWorkspace GISUserRoleAssignmentWorkspace { get; }

        int VersionNo { get; }

        void SetDirty();

        void ActivateFunctionalRoleTab();

        //Begin to maintain organisation
        void LoadOrganisationData();

        void CollectOrganisationData(out OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, out OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList);
        //End to maintain organisation



        void FillExternalParty(string EPRIN);
    }
}


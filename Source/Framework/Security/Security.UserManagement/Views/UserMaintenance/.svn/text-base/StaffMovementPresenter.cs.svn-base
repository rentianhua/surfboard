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

using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Security.UserManagement.Interface;
using System.Collections.Generic;
using Microsoft.Practices.CompositeUI.EventBroker;
using HiiP.Framework.Security.UserManagement.Constants;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Infrastructure.Interface.Constants;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Infrastructure.Interface.BusinessEntities;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class StaffMovementPresenter : Presenter<IStaffMovement>
    {
        public override AppTitleData GetAppTitle()
        {
            return new HiiP.Infrastructure.Interface.BusinessEntities.AppTitleData(
                "Office",
                "Office"
                );
        }

        protected override void InitData()
        {
            base.InitData();

            List<CodeTableEntity> codeTableEntity = GetCodeTableEntity(CodeTableCategoryNames.IsMasterOffice);
            View.SetCodeTableControls(CodeTableCategoryNames.IsMasterOffice, codeTableEntity);
            codeTableEntity = GetCodeTableEntity(CodeTableCategoryNames.Office);
            View.SetCodeTableControls(CodeTableCategoryNames.Office, codeTableEntity);

            OfficeEntity officeEntity = Data as OfficeEntity;
            View.LoadViewData(officeEntity);
        }

        #region Event Publication

        [EventPublication(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.OfficeAddOrUpdateOrDelete, PublicationScope.Global)]
        public event EventHandler<EventArgs<string, OfficeEntity>> OfficeAddOrUpdateOrDelete;

        protected virtual void OnOfficeAddOrUpdateOrDelete(string key, OfficeEntity officeEntity)
        {
            if (OfficeAddOrUpdateOrDelete != null)
                OfficeAddOrUpdateOrDelete(this, new EventArgs<string, OfficeEntity>(key, officeEntity));
        }

        #endregion

        #region Business Logic

        internal List<CodeTableEntity> GetCodeTableEntity(string category)
        {
            List<CodeTableEntity> codeTableEntity = new OrganisationMaintenanceServiceProxy().FindCodesByConditions(
                category,
                String.Empty,
                String.Empty);
            return codeTableEntity;
        }

        internal void PostOfficeAction(OfficeEntity officeEntity)
        {
            OnOfficeAddOrUpdateOrDelete(Key, officeEntity);
        }

        #endregion
    }
}


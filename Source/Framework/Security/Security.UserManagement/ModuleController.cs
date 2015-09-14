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

using System;
using HiiP.Framework.Security.UserManagement.Constants;
using HiiP.Infrastructure.Interface;
using Infragistics.Practices.CompositeUI.WinForms;
using Microsoft.Practices.CompositeUI.Commands;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Miscellaneous;
using System.Windows.Forms;
using HiiP.Framework.Common.Client;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace HiiP.Framework.Security.UserManagement
{
    public class ModuleController : WorkItemController
    {
        public override void Run() 
        {
            ExecuteActions(); 
        }

        public override void ParseParameter(System.Collections.Generic.Dictionary<string, string> param)
        {
            base.ParseParameter(param);

            if (ParameterUtil.IsAvailableParam(param, "Security"))
            {
                switch (param["Security"].ToString())
                {
                    case "1":
                        ShowViewInWorkspace<UserMaintenance>(FunctionNames.SearchUserFunctionID);
                        break;
                }
            }

            if (ParameterUtil.IsAvailableParam(param, "username"))
            {
                string username = param["username"].ToString();
                ViewParameter parameter = new ViewParameter(String.Format("{0}.{1}", HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionID, username));
                parameter.Key = username;
                parameter.CurrentViewStatus = HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update;
                ShowViewInWorkspace<UserDetail>(parameter);
            }
        }

        private void ExecuteActions()
        {
            this.LoadAdministrationMenu();

            // add a root menu item: Administration
            ActionCatalogService.Execute(FunctionNames.OrganisationModuleID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.MaintainOrganisationFunctionID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.SecurityModuleID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.UserModuleID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.NewUserFunctionID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.SearchUserFunctionID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.RoleModuleID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.NewRoleFunctionID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.SearchRoleFunctionID, WorkItem, this, null);

            ActionCatalogService.Execute(FunctionNames.DelegationsModuleID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.MaintainDelegationFunctionID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.CreateDelegationFunctionID, WorkItem, this, null);

        }

        #region Actions

        [Action(FunctionNames.DelegationsModuleID)]
        public void ShowDelegationMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "FinancialDelegation");
        }

        [Action(FunctionNames.MaintainDelegationFunctionID)]
        public void ShowMaintainDelegationMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "MaintainDelegation");
        }


        [Action(FunctionNames.CreateDelegationFunctionID)]
        public void ShowCreateDelegationMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "NewFinancialDelegation");
        }

               
        [Action(FunctionNames.OrganisationModuleID)]
        public void ShowOrganisationMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Organisation");
        }

        [Action(FunctionNames.MaintainOrganisationFunctionID)]
        public void ShowMaintainOrganisationMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "MaintainOrganisation");
        }

        [Action(FunctionNames.SecurityModuleID)]
        public void ShowSecurityMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Security");
        }

        [Action(FunctionNames.UserModuleID)]
        public void ShowUserMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "User");
        }

        [Action(FunctionNames.NewUserFunctionID)]
        public void ShowNewUserMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "AddUser");
        }

        [Action(FunctionNames.SearchUserFunctionID)]
        public void ShowSearchUserMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "SearchUser");
        }

        [Action(FunctionNames.RoleModuleID)]
        public void ShowRoleMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Role");
        }

        [Action(FunctionNames.NewRoleFunctionID)]
        public void ShowNewRoleMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "AddRole");
        }

        [Action(FunctionNames.SearchRoleFunctionID)]
        public void ShowSearchRoleMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "SearchRole");
        }

        #endregion

        [CommandHandler(HiiP.Framework.Security.UserManagement.Constants.CommandNames.MaintainOrganisation)]
        public void ShowSearchOrganisationView(object sender, EventArgs e)
        {
            ShowViewInWorkspace<MaintainOrganisationName>(FunctionNames.MaintainOrganisationFunctionID);
        }

        [CommandHandler(HiiP.Framework.Security.UserManagement.Constants.CommandNames.NewUser)]
        public void ShowNewUserView(object sender, EventArgs e)
        {
            ViewParameter parameter = new ViewParameter();
            parameter.Key = Guid.NewGuid().ToString();
            parameter.CurrentViewStatus = ViewStatus.Add;
            ShowViewInWorkspace<UserDetail>(parameter);
        }

        [CommandHandler(HiiP.Framework.Security.UserManagement.Constants.CommandNames.SearchUser)]
        public void ShowSearchUserView(object sender, EventArgs e)
        {
            ShowViewInWorkspace<UserMaintenance>(FunctionNames.SearchUserFunctionID);
        }


        [Microsoft.Practices.CompositeUI.EventBroker.EventSubscription(HiiP.Framework.Security.UserManagement.Interface.Constants.EventTopicNames.CommonSearchHiiPUser, ThreadOption.UserInterface)]
        public void OnSearchHiiPUser(object sender, EventArgs<ViewParameter> eventArgs)
        {
            ShowViewInWorkspace<UserMaintenance>(eventArgs.Data );
        }
        [CommandHandler(HiiP.Framework.Security.UserManagement.Constants.CommandNames.NewRole)]
        public void ShowNewRoleView(object sender, EventArgs e)
        {
            ViewParameter parameter = new ViewParameter();
            parameter.CurrentViewStatus = ViewStatus.Add;
            ShowViewInWorkspace<RoleDetail>(parameter);
        }

        [CommandHandler(HiiP.Framework.Security.UserManagement.Constants.CommandNames.SearchRole)]
        public void ShowSearchRoleView(object sender, EventArgs e)
        {
            ShowViewInWorkspace<RoleMaintenance>(FunctionNames.SearchRoleFunctionID);
        }

        [CommandHandler(HiiP.Framework.Security.UserManagement.Constants.CommandNames.MaintainDelegation)]
        public void MaintainDelegationView(object sender, EventArgs e)
        {
              ShowViewInWorkspace<MaintainDelegation>(FunctionNames.MaintainDelegationFunctionID );
        }


        [CommandHandler(HiiP.Framework.Security.UserManagement.Constants.CommandNames.CreateDelegation)]
        public void CreateDelegationView(object sender, EventArgs e)
        {
            ViewParameter para = new ViewParameter();
            para.EventFunction = HiiP.Infrastructure.Interface.Constants.EventTopicNames.EventFunctions.CreateDelegation;
            para.ViewId = FunctionNames.CreateDelegationFunctionID;
          
            ShowViewInWorkspace<DelegationDetail>(para);
        }
    }
}
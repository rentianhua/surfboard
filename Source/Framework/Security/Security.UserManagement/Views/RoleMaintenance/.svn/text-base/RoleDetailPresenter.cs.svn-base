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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using Microsoft.Practices.CompositeUI.EventBroker;
using NCS.IConnect.Security;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class RoleDetailPresenter : Presenter<IRoleDetail>
    {
        public override AppTitleData GetAppTitle()
        {
            AppTitleData appTitleData = base.GetAppTitle();

            switch (CurrentViewStatus)
            {
                case ViewStatus.Add:
                    View.SetRoleNameReadOnly(false);
                    appTitleData = new HiiP.Infrastructure.Interface.BusinessEntities.AppTitleData(
                        FunctionNames.NewRoleFunctionName,
                        FunctionNames.NewRoleFunctionScreenID
                        );
                    break;
                case ViewStatus.Update:
                    View.SetRoleNameReadOnly(true);
                    appTitleData = new HiiP.Infrastructure.Interface.BusinessEntities.AppTitleData(
                        FunctionNames.UpdateRoleFunctionName,
                        FunctionNames.UpdateRoleFunctionScreenID
                        ).Format(Key);
                    break;
                case ViewStatus.DuplicateRole:
                    View.SetRoleNameReadOnly(false);
                    appTitleData = new HiiP.Infrastructure.Interface.BusinessEntities.AppTitleData(
                        FunctionNames.DuplicateRoleFunctionName,
                        FunctionNames.DuplicateRoleFunctionScreenID
                        );
                    break;
            }

            return appTitleData;
        }

        private static string _minAuthorisationRoleFunctionID;
        public static string MinAuthorisationRoleFunctionID
        {
            get
            {
                if (string.IsNullOrEmpty(_minAuthorisationRoleFunctionID))
                {
                    using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                    {
                        _minAuthorisationRoleFunctionID = proxy.GetMinAuthorisationRoleFunctionID();
                    }
                }
                return _minAuthorisationRoleFunctionID;
            }
        }

        protected override void InitData()
        {
            base.InitData();

            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                switch (CurrentViewStatus)
                {
                    case ViewStatus.Add:
                        View.DeleteRoleEnabled(false);
                        View.ViewUsersEnabled(false);
                        View.AccessControl(
                            false,
                            UIAccessControl.IsAuthorised(FunctionNames.NewRoleFunctionID),
                            false);
                        break;
                    case ViewStatus.Update:
                        View.ActionsInRole = (from n in proxy.GetActionsInRole(Key)
                                              select n).ToList();
                        View.AccessControl(
                            UIAccessControl.IsAuthorised(FunctionNames.UpdateRoleFunctionID),
                            UIAccessControl.IsAuthorised(FunctionNames.UpdateRoleFunctionID),
                            UIAccessControl.IsAuthorised(FunctionNames.DuplicateRoleFunctionID));

                        RoleEntity roleEntity ;
                        Guid id = Utility.SetContextValues();
                        using (new MonitoringTracer(id, FunctionNames.RoleModuleID, FunctionNames.SearchRoleFunctionID, ComponentType.Screen))
                        {
                            roleEntity = proxy.GetRoleByRoleName(Key);
                        }

                        View.LoadRoleData(roleEntity);
                        break;
                    case ViewStatus.DuplicateRole:
                        View.ActionsInRole = (from n in proxy.GetActionsInRole(Key)
                                              select n).ToList();
                        View.AccessControl(
                            false,
                            UIAccessControl.IsAuthorised(FunctionNames.NewRoleFunctionID),
                            false);
                        break;
                }
            }

            // Initialize role functions
            View.LoadRoleFunctions();
        }

        #region Event Publication

        /// <summary>
        /// After updating or deleting a role
        /// notify other form to do something
        /// </summary>
        [EventPublication(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.RoleUpdateOrDelete, PublicationScope.Global)]
        public event EventHandler<EventArgs<string>> RoleUpdatedOrDeleted;

        public void OnRoleUpdatedOrDeleted()
        {
            if (null != RoleUpdatedOrDeleted)
                RoleUpdatedOrDeleted(this, new EventArgs<string>(string.Empty));
        }

        #endregion

        #region Business Logic

        internal void SaveRole(string roleName, string description)
        {
            switch (CurrentViewStatus)
            {
                case ViewStatus.Add:
                    AddRole(roleName, description);
                    break;
                case ViewStatus.Update:
                    UpdateRole(roleName, description, RoleStatus.Active);
                    break;
                case ViewStatus.DuplicateRole:
                    AddRole(roleName, description);
                    break;
            }
        }

        /// <summary>
        /// Add a new role
        /// </summary>
        /// <param name="roleName">role name</param>
        /// <param name="description">role description</param>
        private void AddRole(string roleName, string description)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                if (View.ActionsInRole.Count > 0)
                {
                    // new UserMaintenanceServiceProxy().AddActionsInRole(roleName, View.ActionsInRole.ToArray());
                    Guid id = Utility.SetContextValues();
                    using (new MonitoringTracer(id, FunctionNames.RoleModuleID, FunctionNames.NewRoleFunctionID, ComponentType.Screen))
                    {
                        proxy.ExtendedCreateRole(roleName, description, View.ActionsInRole.ToArray());
                    }
                }
                else
                {
                    // new UserMaintenanceServiceProxy().AddActionsInRole(roleName, View.ActionsInRole.ToArray());
                    Guid id = Utility.SetContextValues();
                    using (new MonitoringTracer(id, FunctionNames.RoleModuleID, FunctionNames.NewRoleFunctionID, ComponentType.Screen))
                    {
                        proxy.ExtendedCreateRole(roleName, description, new string[] { });
                    }
                }
            }
            OnUpdateStatusBarMessage(Messages.Framework.FWI211.Format());
            OnRoleUpdatedOrDeleted();
        }

        /// <summary>
        /// Update an existed role
        /// </summary>
        /// <param name="roleName">role name</param>
        /// <param name="description">role description</param>
        /// <param name="status">role status</param>
        private void UpdateRole(string roleName, string description, string status)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                // Check concurrent operation
                if (!ExtendedRoles.RoleExists(Key))
                {
                    HiiP.Framework.Common.Client.Utility.ShowMessageBox(Messages.Framework.FWW002, Key);
                }
                else
                {
                    RoleEntity currentRole ;
                    Guid activityId1 = Utility.SetContextValues();
                    using (new MonitoringTracer(activityId1, FunctionNames.RoleModuleID, FunctionNames.SearchRoleFunctionID, ComponentType.Screen))
                    {
                        currentRole = proxy.GetRoleByRoleName(Key);
                    }

                    int currentVersionNo = currentRole.VersionNo;
                    if (View.VersionNo < currentVersionNo)
                    {
                        HiiP.Framework.Common.Client.Utility.ShowMessageBox(Messages.Framework.FWW003, Key);
                    }
                    else
                    {
                        // new UserMaintenanceServiceProxy().UpdateRole(Key, roleName, description, status);
                        if (View.ActionsInRole.Count > 0)
                        {
                            // new UserMaintenanceServiceProxy().UpdateActionsInRole(roleName, View.ActionsInRole.ToArray());
                            Guid activityId2 = Utility.SetContextValues();
                            using (new MonitoringTracer(activityId2, FunctionNames.RoleModuleID, FunctionNames.UpdateRoleFunctionID, ComponentType.Screen))
                            {
                                proxy.ExtendedUpdateRole(Key, description, status, View.ActionsInRole.ToArray());
                            }
                        }
                        else
                        {
                            Guid id = Utility.SetContextValues();
                            using (new MonitoringTracer(id, FunctionNames.RoleModuleID, FunctionNames.UpdateRoleFunctionID, ComponentType.Screen))
                            {
                                proxy.ExtendedUpdateRole(Key, description, status, new string[] { });
                            }
                        }
                        OnUpdateStatusBarMessage(Messages.Framework.FWI212.Format(roleName));
                    }
                }
            }

            OnRoleUpdatedOrDeleted();
        }

        /// <summary>
        /// Delete or Activate role
        /// </summary>
        /// <returns></returns>
        internal void DeleteRole()
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                // Check concurrent operation
                if (!ExtendedRoles.RoleExists(Key))
                {
                    HiiP.Framework.Common.Client.Utility.ShowMessageBox(Messages.Framework.FWW002, Key);
                    OnRoleUpdatedOrDeleted();
                    OnCloseView();
                }
                else
                {
                    string[] usernames;
                    Guid id = Utility.SetContextValues();
                    using (new MonitoringTracer(id, FunctionNames.UserModuleID, FunctionNames.SearchUserFunctionID, ComponentType.Screen))
                    {
                        usernames = proxy.GetUsersByRoleName(Key, UserStatus.Active);
                    }

                    if (usernames.Length > 0)
                    {
                        if (MessageBox.Show(
                            String.Format("There are {0} user(s) assigned to this role.  Do you wish to proceed? ", usernames.Length),
                            "Confirmation",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question)
                                            == DialogResult.OK)
                        {
                            Guid activityId1 = Utility.SetContextValues();
                            using (new MonitoringTracer(activityId1, FunctionNames.RoleModuleID, FunctionNames.UpdateRoleFunctionID, ComponentType.Screen))
                            {
                                proxy.DeleteRole(Key);
                            }
                            OnUpdateStatusBarMessage(Messages.Framework.FWI213.Format(Key));
                            OnRoleUpdatedOrDeleted();
                            OnCloseView();
                        }
                    }
                    else
                    {
                        if (HiiP.Framework.Common.Client.Utility.ShowMessageBox(Messages.Framework.FWC211,Key) == DialogResult.Yes)
                        {
                            Guid activityId2 = Utility.SetContextValues();
                            using (new MonitoringTracer(activityId2, FunctionNames.RoleModuleID, FunctionNames.UpdateRoleFunctionID, ComponentType.Screen))
                            {
                                proxy.DeleteRole(Key);
                            }

                            OnUpdateStatusBarMessage(Messages.Framework.FWI213.Format(Key));
                            OnRoleUpdatedOrDeleted();
                            OnCloseView();
                        }
                    }
                }
            }
        }

        internal void ViewUsers()
        {
            ViewParameter parameter = new ViewParameter(String.Format("{0}.{1}", HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ListOfUserInRoleFunctionID, Key));
            parameter.Key = Key;
            ShowViewInWorkspace<ListOfUserInRole>(parameter);
        }

        #endregion

        #region Private method

        /// <summary>
        /// update actions in role
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="ticked">true=>checked ? false=>unchecked</param>
        internal void UpdateActionsInRole(string action, bool ticked)
        {
            if(((from n in View.ActionsInRole
                       where action == n
                 select n).Count() == 1) && !ticked)
            {
                View.ActionsInRole.Remove(action);  // existed and unchecked
            }
            else if (!((from n in View.ActionsInRole
                        where action == n
                        select n).Count() == 1) && ticked)
            {
                View.ActionsInRole.Add(action);     //not existed and checked
            }
        }

        /// <summary>
        /// Show duplication role screen
        /// </summary>
        internal void ShowDuplicateRoleView()
        {
            ViewParameter param = new ViewParameter();
            param.Key = Key;
            param.CurrentViewStatus = ViewStatus.DuplicateRole;
            ShowViewInWorkspace<RoleDetail>(param);
        }

        internal List<TreeNode> GetModuleNodeCollection()
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                List<TreeNode> moduleNodeCollection = new List<TreeNode>();

                string[] actions = proxy.GetAllActions();
                if (actions == null) return moduleNodeCollection;
                string[] allFunctionModules = GetAllFunctionModules(actions);
                // Algorithm for actions translation
                // array -> xml (format) 
                // Ready ...
                string FuncionsRoot = "Functions";

                XmlDocument xDoc = new XmlDocument();
                XmlDeclaration xdeclaration = xDoc.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
                xDoc.AppendChild(xdeclaration);
                XmlElement xelement = xDoc.CreateElement(FuncionsRoot);
                xDoc.AppendChild(xelement);
                foreach (string FunctionModule in allFunctionModules)
                {
                    XmlElement xFunctionElement = xDoc.CreateElement(FunctionModule);
                    xelement.AppendChild(xFunctionElement);
                    LoadXmlFromActions(FunctionModule, actions, ref xFunctionElement, xDoc);
                }

                // root element
                XmlElement root = xDoc.DocumentElement;

                // Load systemadmin tree
                if (root!=null && root.HasChildNodes)
                {
                    int childNodeCount = root.ChildNodes.Count;
                    for (int i = 0; i <= childNodeCount - 1; i++)
                    {
                        moduleNodeCollection.Add(new TreeNode(root.ChildNodes[i].Name));
                        foreach (string action in View.ActionsInRole)
                        {
                            if (action == root.ChildNodes[i].Name)
                            {
                                moduleNodeCollection[i].Checked = true;
                                break;
                            }
                        }
                        InitTreeNode(root.ChildNodes[i], moduleNodeCollection[i], View.ActionsInRole.ToArray());
                    }
                }

                return moduleNodeCollection;
            }
        }

        #region Load Action Tree

        // Sync treeview by xmldocument  --- tree is fixed
        private void InitTreeNode(XmlNode xmlnode, TreeNode treenode, string[] actions)
        {
            XmlNode xnode;
            TreeNode tnode;
            XmlNodeList xnodelist;
            if (xmlnode.HasChildNodes)
            {
                xnodelist = xmlnode.ChildNodes;
                for (int i = 0; i <= xnodelist.Count - 1; i++)
                {
                    xnode = xmlnode.ChildNodes[i];
                    treenode.Nodes.Add(new TreeNode(xnode.Name));
                    foreach (string action in actions)
                    {
                        if (action == xnode.Name)
                        {
                            treenode.Nodes[i].Checked = true;
                            break;
                        }
                    }
                    tnode = treenode.Nodes[i];
                    InitTreeNode(xnode, tnode, actions);
                }
            }
        }

        #endregion

        internal bool RoleExists(string roleName)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                return proxy.RoleExists(roleName);
            }
        }

   
        #region check presibling and nextsibling nodes


        //private void checkPreSiblingNodes(TreeNode treeNode, ref bool result)
        //{
        //    if (treeNode != null)
        //    {
        //        if (treeNode.Checked)
        //        {
        //            result &= true;
        //            checkPreSiblingNodes(treeNode.PrevNode, ref result);
        //        }
        //        else
        //        {
        //            result &= false;
        //        }
        //    }
        //}

        //private void checkNextSiblingNodes(TreeNode treeNode, ref bool result)
        //{
        //    if (treeNode != null)
        //    {
        //        if (treeNode.Checked)
        //        {
        //            result &= true;
        //            checkNextSiblingNodes(treeNode.NextNode, ref result);
        //        }
        //        else
        //        {
        //            result &= false;
        //        }
        //    }
        //}

        #endregion

        #region Load Xml From Actions

        private string[] GetAllFunctionModules(string[] actions)
        {
            ArrayList allFunctionModules = new ArrayList();
            foreach (string action in actions)
            {
                if (Regex.IsMatch(action, @"^\w+$"))
                {
                    allFunctionModules.Add(action);
                }
            }

            return (string[])allFunctionModules.ToArray(typeof(string));
        }

        private void LoadXmlFromActions(string functionAction, string[] functionActions, ref XmlElement xelement, XmlDocument xdoc)
        {
            foreach (string action in functionActions)
            {
                if (Regex.IsMatch(action, "^" + functionAction + @"\.\w+$"))
                {
                    XmlElement childElement = xdoc.CreateElement(action);
                    xelement.AppendChild(childElement);
                    LoadXmlFromActions(action, functionActions, ref childElement, xdoc);
                }
            }
        }

        #endregion

        #endregion
    }
}


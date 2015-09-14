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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;

using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolTip;

using Microsoft.Practices.CompositeUI.EventBroker;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class UserRoleAssignmentPresenter : Presenter<IUserRoleAssignment>
    {
        public override AppTitleData GetAppTitle()
        {
            return new AppTitleData(
                HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionName,
                HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionScreenID
                );
        }

        /// <summary>
        /// Initialize view data
        /// </summary>
        protected override void InitData()
        {
            base.InitData();

            // Load general user info or not
            switch (CurrentViewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add:
                    // New User
                    View.DataFilterEntities = new List<DataFilterEntity>();
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update:
                    // Update User
                    LoadAssignedRolesForUser();
                    // Get data filter entites by current logon user
                    View.DataFilterEntities = GetUserDataFiltersByUserName(Key);
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:
                    // Copy User
                    string username = Data as string;
                    if (null != username)
                    {
                        View.LoadCopyRoles(username);
                        View.DataFilterEntities = GetCopiedUserDataFiltersByUserName(username);
                    }

                    // must swtich the original status to new status
                    foreach (DataFilterEntity entity in View.DataFilterEntities)
                    {
                        if (entity.RecordStatus == DataFilterRecordStatus.Original)
                            entity.RecordStatus = DataFilterRecordStatus.New;
                    }

                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.AssignRolesToUsers:
                    // Assign Roles To User
                    View.DataFilterEntities = new List<DataFilterEntity>();
                    break;
            }
        }

        #region Business Logic

        #region Common logic

        /// <summary>
        /// Assign roles for user
        /// </summary>
        private void LoadAssignedRolesForUser()
        {
            // Binding roles to assigned role grid
            DataTable dt = new DataTable();
            dt.Columns.Add("Role Name");
            dt.Columns.Add("Description");
            RoleEntity[] roles = GetRoleEntitysByUserName(Key);
            if (roles != null)
            {
                foreach (RoleEntity assignedRole in roles)
                {
                    dt.Rows.Add(assignedRole.RoleName, assignedRole.Description);
                }
            }
            View.InitAssignedRoles(dt);
        }

        /// <summary>
        /// Role Exists ?
        /// </summary>
        /// <param name="entity">RoleEntity</param>
        /// <param name="entitys">RoleEntity[]</param>
        /// <returns>true ? false</returns>
        internal bool RoleExists(RoleEntity entity, RoleEntity[] entitys)
        {
            foreach (RoleEntity e in entitys)
            {
                if (e.RoleName == entity.RoleName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Assign Roles For User
        /// </summary>
        /// <param name="availableRolesGrid"></param>
        /// <param name="assignedRolesGrid"></param>
        internal void AssignRolesForUser(UltraGrid availableRolesGrid, UltraGrid assignedRolesGrid)
        {
            ArrayList availableRoleList = new ArrayList();
            foreach (UltraGridRow row in availableRolesGrid.Rows)
            {
                if (Convert.ToBoolean(row.Cells[2].Value))
                {
                    availableRoleList.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
                }
            }
            RoleEntity[] availableRoles = (RoleEntity[])availableRoleList.ToArray(typeof(RoleEntity));

            if (availableRoles.Length == 0) return;

            ArrayList assignedRoleList = new ArrayList();
            foreach (UltraGridRow row in assignedRolesGrid.Rows)
            {
                assignedRoleList.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
            }
            RoleEntity[] assignedRoles = (RoleEntity[])assignedRoleList.ToArray(typeof(RoleEntity));

            // Load assigned role list
            DataTable dt = new DataTable();
            dt.Columns.Add("Role Name");
            dt.Columns.Add("Description");
            foreach (RoleEntity assignedRole in assignedRoles)
            {
                dt.Rows.Add(assignedRole.RoleName, assignedRole.Description);
            }
            foreach (RoleEntity availableRole in availableRoles)
            {
                if (!RoleExists(availableRole, assignedRoles))
                {
                    dt.Rows.Add(availableRole.RoleName, availableRole.Description);
                }
            }

            foreach (UltraGridRow ultraGridRow in availableRolesGrid.Rows)
            {
                if (Convert.ToBoolean(ultraGridRow.Cells[2].Value))
                {
                    ultraGridRow.Selected = true;
                }
            }
            availableRolesGrid.DeleteSelectedRows(false);

            assignedRolesGrid.DataSource = dt;

            View.SetDirty();
        }

        /// <summary>
        /// Delete Roles From User
        /// </summary>
        /// <param name="availableRolesGrid"></param>
        /// <param name="assignedRolesGrid"></param>
        /// <param name="UltraExpandableGroupBoxPanelMain"></param>
        internal void DeleteRolesFromUser(UltraGrid availableRolesGrid, UltraGrid assignedRolesGrid, UltraExpandableGroupBoxPanel UltraExpandableGroupBoxPanelMain)
        {
            ArrayList remainRoleList = new ArrayList();
            List<RoleEntity> deletedRoleList = new List<RoleEntity>();
            foreach (UltraGridRow row in assignedRolesGrid.Rows)
            {
                if (!Convert.ToBoolean(row.Cells[2].Value))
                {
                    remainRoleList.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
                }
                else
                {
                    deletedRoleList.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));

                    string roleName = row.Cells["Role Name"].Value.ToString();
                    if (View.DataFilterEntities != null && View.DataFilterEntities.Count > 0)
                    {
                        int DataFilterEntitiesCount = View.DataFilterEntities.Count;
                        for (int i = DataFilterEntitiesCount - 1; i > -1; i--)
                        {
                            if (View.DataFilterEntities[i].RoleName == roleName)
                            {
                                switch (View.DataFilterEntities[i].RecordStatus)
                                {
                                    case DataFilterRecordStatus.Original:
                                        View.DataFilterEntities[i].RecordStatus = DataFilterRecordStatus.Delete;
                                        break;
                                    case DataFilterRecordStatus.New:
                                        View.DataFilterEntities.RemoveAt(i);
                                        break;
                                }
                            }
                        }
                    }
                    if (View.CurrentRoleNameForDataFilter == roleName)
                    {
                        UltraExpandableGroupBoxPanelMain.Controls.Clear();
                        View.CurrentRoleNameForDataFilter = String.Empty;
                    }
                }
            }

            if (deletedRoleList.Count == 0) return;

            RoleEntity[] remainRoles = (RoleEntity[])remainRoleList.ToArray(typeof(RoleEntity));

            // Load assigned role list
            DataTable dt = new DataTable();
            dt.Columns.Add("Role Name");
            dt.Columns.Add("Description");
            foreach (RoleEntity remainRole in remainRoles)
            {
                dt.Rows.Add(remainRole.RoleName, remainRole.Description);
            }

            assignedRolesGrid.DataSource = dt;

            List<RoleEntity> availableRoles = new List<RoleEntity>();
            foreach (UltraGridRow row in availableRolesGrid.Rows)
            {
                availableRoles.Add(new RoleEntity(row.Cells["Role Name"].Value.ToString(), row.Cells["Description"].Value.ToString(), RoleStatus.Active));
            }
            foreach (RoleEntity deletedRole in deletedRoleList)
            {
                availableRoles.Add(deletedRole);
            }
            List<RoleEntity> aRoles = availableRoles.OrderBy(r => r.RoleName).ToList();
            DataTable dtAvailable = new DataTable();
            dtAvailable.Columns.Add("Role Name");
            dtAvailable.Columns.Add("Description");
            foreach (RoleEntity deletedRole in aRoles)
            {
                dtAvailable.Rows.Add(deletedRole.RoleName, deletedRole.Description);
            }
            availableRolesGrid.DataSource = dtAvailable;

            View.SetDirty();
        }

        /// <summary>
        /// search role list by rolename and description
        /// </summary>
        /// <param name="roleNameToMatch">roleNameToMatch</param>
        /// <param name="descriptionToMatch">descriptionToMatch</param>
        /// <returns>RoleEntity[]</returns>
        internal RoleEntity[] FindRoleListByConditions(string roleNameToMatch, string descriptionToMatch)
        {
            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            using (new MonitoringTracer(id, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SearchRoleFunctionID, ComponentType.Screen))
            {
                using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                {
                    return proxy.FindRolesByConditions(roleNameToMatch, descriptionToMatch);
                }
            }
        }

        internal RoleEntity[] GetRoleEntitysByUserName(string username)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                Guid id = Utility.SetContextValues();
                using (new MonitoringTracer(id, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID, HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SearchRoleFunctionID, ComponentType.Screen))
                {
                    return proxy.GetRolesByUserName(username);
                }
            }
        }


        #endregion

        #region Data Filter & Values Actions

        #region Load Data Filter Values Action

        private bool isInitializing = true;
        private List<DataFilterEntity> _templateDataFilterEntities = new List<DataFilterEntity>();
        private List<DataFilterEntity> _dataFiltersForLogonUser;

        /// <summary>
        /// Loads the data filter values on role for user.
        /// </summary>
        /// <param name="ultraExpandableGroupBoxPanelMain">The ultra expandable group box panel main.</param>
        /// <param name="rolename">The rolename.</param>
        internal void LoadDataFilterValuesOnRoleForUser(UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanelMain, string rolename)
        {
            this.isInitializing = true;

            // Load controls
            // 1. Create UltraExpandableGroupBoxes by data filters 
            // 2. Create CheckBoxes by data filter values
            ultraExpandableGroupBoxPanelMain.Controls.Clear();

            List<string> fullDataFilters = new List<string>();
            List<string> authorisedDataFilters = new List<string>();
            _dataFiltersForLogonUser = new List<DataFilterEntity>();

            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                _templateDataFilterEntities = proxy.GetDataFilterTemplateData(rolename, ref fullDataFilters, ref authorisedDataFilters, ref _dataFiltersForLogonUser);
            }

            Dictionary<string, string> fullDataFilterPairs = GetDataFiltersByIDs(fullDataFilters, _templateDataFilterEntities)
                .OrderByDescending(d => d.Value)
                .ToDictionary(d => d.Key, d => d.Value, StringComparer.OrdinalIgnoreCase);

            int i = 0;
            foreach (var dataFilterType in fullDataFilterPairs)
            {
                var key = dataFilterType.Key;
                if (authorisedDataFilters.Contains(key)
                    && this.IsAuthorisedDataFilter(key, _dataFiltersForLogonUser))
                {
                    ultraExpandableGroupBoxPanelMain.Controls.Add(
                        LoadUltraExpandableGroupBoxByDataFilter(ultraExpandableGroupBoxPanelMain,
                        dataFilterType,
                        _templateDataFilterEntities.Where(d => d.DataFilterID.Equals(key)).ToList(),
                        _dataFiltersForLogonUser.Where(d => d.DataFilterID.Equals(key)).ToList()));
                    i++;
                }
            }

            for (int groupIndex = 0; groupIndex < i; groupIndex++)
            {
                ultraExpandableGroupBoxPanelMain.Controls[groupIndex].TabIndex = i - 1 - groupIndex;
            }

            // Single expandable mode
            foreach (Control control in ultraExpandableGroupBoxPanelMain.Controls)
            {
                UltraExpandableGroupBox expandableControl = control as UltraExpandableGroupBox;
                if (null != expandableControl)
                {
                    expandableControl.ExpandedStateChanged += delegate(object sender, EventArgs e)
                    {
                        UltraExpandableGroupBox currentBox = sender as UltraExpandableGroupBox;
                        if(currentBox==null) 
                        {
                            return;
                        }
                        if (currentBox.Expanded)
                        {
                            foreach (Control internalControl in ultraExpandableGroupBoxPanelMain.Controls)
                            {
                                UltraExpandableGroupBox internalExpandableControl = internalControl as UltraExpandableGroupBox;
                                if (null != internalExpandableControl
                                    && !currentBox.Name.Equals(internalExpandableControl.Name)
                                    && internalExpandableControl.Expanded)
                                {
                                    internalExpandableControl.Expanded = false;
                                    break;
                                }
                            }
                        }
                    };
                }
            }

            this.isInitializing = false;

        }

        #region Helper for loading and applying data filters

        /// <summary>
        /// Gets the data filters by Ids.
        /// </summary>
        /// <param name="targetDataFilterIDs">The target data filter I ds.</param>
        /// <param name="templateDataFilterEntities">The template data filter entities.</param>
        /// <returns></returns>
        private Dictionary<string, string> GetDataFiltersByIDs(List<string> targetDataFilterIDs, List<DataFilterEntity> templateDataFilterEntities)
        {
            Dictionary<string, string> dataFilters = new Dictionary<string, string>();

            foreach (string targetDataFilterID in targetDataFilterIDs)
            {
                foreach (DataFilterEntity df in templateDataFilterEntities)
                {
                    if (df.DataFilterID.Equals(targetDataFilterID))
                    {
                        dataFilters.Add(df.DataFilterID, df.DataFilter);
                        break;
                    }
                }
            }

            return dataFilters;
        }

        /// <summary>
        /// Determines whether [is authorised data filter] [the specified target data filter ID].
        /// </summary>
        /// <param name="targetDataFilterID">The target data filter ID.</param>
        /// <param name="dataFiltersForLogonUser">The data filters for logon user.</param>
        /// <returns>
        /// 	<c>true</c> if [is authorised data filter] [the specified target data filter ID]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAuthorisedDataFilter(string targetDataFilterID, List<DataFilterEntity> dataFiltersForLogonUser)
        {
            foreach (DataFilterEntity df in dataFiltersForLogonUser)
            {
                if (df.DataFilterID.Equals(targetDataFilterID))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is authorised data filter value] [the specified target data filter value].
        /// </summary>
        /// <param name="targetDataFilterValueID">The target data filter value.</param>
        /// <param name="dataFilterValuesForLogonUser">The data filter values for logon user.</param>
        /// <returns>
        /// 	<c>true</c> if [is authorised data filter value] [the specified target data filter value]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAuthorisedDataFilterValue(string targetDataFilterValueID, List<DataFilterEntity> dataFilterValuesForLogonUser)
        {
            foreach (DataFilterEntity dfv in dataFilterValuesForLogonUser)
            {
                if (dfv.DataFilterValueID.Equals(targetDataFilterValueID))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is full authorised data filter] [the specified rolename].
        /// </summary>
        /// <param name="rolename">The rolename.</param>
        /// <param name="dataFilterID">The data filter ID.</param>
        /// <returns>
        /// 	<c>true</c> if [is full authorised data filter] [the specified rolename]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsFullAuthorisedDataFilterOnSelectedUser(string rolename, string dataFilterID)
        {
            foreach (DataFilterEntity udfv in View.DataFilterEntities)
            {
                if (udfv.RoleName.Equals(rolename)
                    && udfv.DataFilterID.Equals(dataFilterID)
                    && udfv.DataFilterValueID.Equals("*")
                    && (udfv.RecordStatus.Equals(DataFilterRecordStatus.Original) || udfv.RecordStatus.Equals(DataFilterRecordStatus.New)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the full DFV on data filter.
        /// </summary>
        /// <param name="dataFilterID">The data filter ID.</param>
        /// <returns></returns>
        private List<DataFilterEntity> GetFullDFVOnDataFilter(string dataFilterID)
        {
            var fullDFVOnDataFilter = _dataFiltersForLogonUser.Where(d => d.DataFilterID.Equals(dataFilterID));

            //if logon user have full authority, need to expand the items.(* means full authority)
            //now, if target user is *, when logon user update it, it will delete the * and create the items which logon user's selected items.[On security grounds]
            //and if target user is not *, when logon user update it, it will delete the unselected items only.
            return fullDFVOnDataFilter.Any(item => item.DataFilterValueID.Equals("*"))
                       ? _templateDataFilterEntities.Where(d => d.DataFilterID.Equals(dataFilterID)).ToList()
                       : fullDFVOnDataFilter.ToList();
        }

        #endregion

        #endregion

        #region Load Data Filters and Values

        /// <summary>
        /// const variables help to draw panel and others
        /// </summary>
        private const int ItemWidth = 136;
        private const int ItemHeight = 20;
        private const int BaseX = 9;
        private const int BaseXSpan = ItemWidth+5;
        private const int BaseY = 8;
        private const int BaseYSpan = ItemHeight+6;
        private const int BaseDivisor = 3;
        private const int BaseTopBlank = 3;
        private const int FontPoint = 9;

       /// <summary>
        /// Load UltraExpandableGroupBox By DataFilters
        /// </summary>
       /// <param name="ultraExpandableGroupBoxPanelMain"></param>
       /// <param name="dataFilterType"></param>
       /// <param name="templateDataFilterValues"></param>
       /// <param name="userDataFilterValues"></param>
       /// <returns></returns>
        private UltraExpandableGroupBox LoadUltraExpandableGroupBoxByDataFilter(UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanelMain,
            KeyValuePair<string, string> dataFilterType, List<DataFilterEntity> templateDataFilterValues, List<DataFilterEntity> userDataFilterValues)
        {
            string selectedRole = View.CurrentRoleNameForDataFilter;
            bool isFullAuthorisedDataFilterOnLogonUser = this.IsAuthorisedDataFilterValue("*", userDataFilterValues);
            bool isFullAuthorisedDataFilterOnSelectedUser = this.IsFullAuthorisedDataFilterOnSelectedUser(selectedRole, dataFilterType.Key);

            // Container
            UltraExpandableGroupBox ultraExpandableGroupBox = new HiiP.Framework.Common.Client.ExtendedUltraExpandableGroupBox();
            UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel = GetUltraExpandableGroupBoxPanel(dataFilterType.Value + "Panel");

            // All
            UltraCheckEditor allCheckEditor = GetUltraCheckEditor(
                selectedRole + "|" + dataFilterType.Key + "|*", "All", BaseX, BaseY);
            ultraExpandableGroupBoxPanel.Controls.Add(allCheckEditor);
            allCheckEditor.Enabled = false;

            // Childen
            int rowCounter = 1;
            int colCounter ;
            int i = 0;
            templateDataFilterValues = templateDataFilterValues.OrderBy(d => d.DataFilterValue).ToList();
            foreach (DataFilterEntity tdf in templateDataFilterValues)
            {
                if (isFullAuthorisedDataFilterOnLogonUser
                    || this.IsAuthorisedDataFilterValue(tdf.DataFilterValueID, userDataFilterValues))
                {
                    rowCounter = i / BaseDivisor + 1;
                    colCounter = i % BaseDivisor;
                    UltraCheckEditor ultraCheckEditor = GetUltraCheckEditor(
                        selectedRole + "|" + tdf.DataFilterID + "|" + tdf.DataFilterValueID,
                        tdf.DataFilterValue,
                        BaseX + BaseXSpan * colCounter,
                        BaseY + BaseYSpan * rowCounter);
                    ultraExpandableGroupBoxPanel.Controls.Add(ultraCheckEditor);
                    i++;

                    if (!isFullAuthorisedDataFilterOnSelectedUser)
                    {
                        foreach (DataFilterEntity selectedUserDFV in View.DataFilterEntities)
                        {
                            if (selectedUserDFV.RoleName.Equals(selectedRole)
                                && selectedUserDFV.DataFilterID.Equals(tdf.DataFilterID)
                                && selectedUserDFV.DataFilterValueID.Equals(tdf.DataFilterValueID)
                                && (selectedUserDFV.RecordStatus.Equals(DataFilterRecordStatus.Original) || selectedUserDFV.RecordStatus.Equals(DataFilterRecordStatus.New)))
                            {
                                ultraCheckEditor.Checked = true;
                            }
                        }
                    }
                }
            }

            // Check state of data filter value
            if (isFullAuthorisedDataFilterOnSelectedUser) allCheckEditor.Checked = true;

            // Should "All" be enabled?
            // The logon user has full right to current data filter type, OR
            // The selected user has full right to current data filter type.
            if (isFullAuthorisedDataFilterOnLogonUser) allCheckEditor.Enabled = true; // || isFullAuthorisedDataFilterOnSelectedUser

            int containHeight = (rowCounter + 3) * BaseYSpan + BaseTopBlank;

            ultraExpandableGroupBox.Controls.Add(ultraExpandableGroupBoxPanel);

            ultraExpandableGroupBox.Size = new System.Drawing.Size(ultraExpandableGroupBoxPanelMain.Size.Width, BaseYSpan);
            ultraExpandableGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            ultraExpandableGroupBox.Expanded = false;
            ultraExpandableGroupBox.ExpandedSize = new System.Drawing.Size(ultraExpandableGroupBox.Size.Width, containHeight);
            ultraExpandableGroupBox.Location = new System.Drawing.Point(0, 0);
            ultraExpandableGroupBox.Name = dataFilterType.Value;
            ultraExpandableGroupBox.TabIndex = 0; // index;
            ultraExpandableGroupBox.Text = dataFilterType.Value;
            ultraExpandableGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;

            return ultraExpandableGroupBox;
        }

        /// <summary>
        /// Get UltraExpandableGroupBoxPanel
        /// </summary>
        /// <param name="dataFilterPanelName"></param>
        /// <returns></returns>
        private UltraExpandableGroupBoxPanel GetUltraExpandableGroupBoxPanel(string dataFilterPanelName)
        {
            UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel = new UltraExpandableGroupBoxPanel();

            ultraExpandableGroupBoxPanel.Location = new System.Drawing.Point(-10000, -10000);
            ultraExpandableGroupBoxPanel.Name = dataFilterPanelName;
            ultraExpandableGroupBoxPanel.Size = new System.Drawing.Size(336, 134);
            ultraExpandableGroupBoxPanel.TabIndex = 0;
            ultraExpandableGroupBoxPanel.Visible = false;
            ultraExpandableGroupBoxPanel.AutoScroll = true;
            ultraExpandableGroupBoxPanel.AutoSize = true;

            return ultraExpandableGroupBoxPanel;
        }

        /// <summary>
        /// Get Control Appearance(for Infragistics)
        /// </summary>
        /// <returns></returns>
        private Infragistics.Win.Appearance GetAppearance()
        {
            Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
            appearance.BackColorAlpha = Infragistics.Win.Alpha.Transparent;

            return appearance;
        }

        /// <summary>
        /// Get UltraCheckEditor
        /// </summary>
        /// <param name="filterID">filterID</param>
        /// <param name="filterValue">filterValue</param>
        /// <param name="x">default x: 9</param>
        /// <param name="y">default y: 8</param>
        /// <returns></returns>
        private UltraCheckEditor GetUltraCheckEditor(string filterID, string filterValue, int x, int y)
        {
            UltraCheckEditor ultraCheckEditor = new UltraCheckEditor();

            ultraCheckEditor.Appearance = GetAppearance();
            ultraCheckEditor.Location = new System.Drawing.Point(x, y);
            ultraCheckEditor.Name = filterID;
            ultraCheckEditor.Size = new System.Drawing.Size(ItemWidth, ItemHeight);
            ultraCheckEditor.TabIndex = 0;

            int maxCharactorsCount = ItemWidth/(FontPoint + 1) + 1;// (((ItemWidth % (FontPoint + 1)) == 0) ? 0 : 1);//->12

            ultraCheckEditor.Text = filterValue.Length > maxCharactorsCount ? filterValue.Substring(0, maxCharactorsCount - 3) + "..." : filterValue;
            UltraToolTipInfo ultraToolTipInfo = new UltraToolTipInfo(filterValue, ToolTipImage.Default, String.Empty, DefaultableBoolean.Default);
            View.ViewUltraToolTipManager.SetUltraToolTip(ultraCheckEditor, ultraToolTipInfo);
            ultraCheckEditor.AfterCheckStateChanged += ultraCheckEditor_AfterCheckStateChanged;

            return ultraCheckEditor;
        }

        /// <summary>
        ///  Check data filter value to change Data
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ultraCheckEditor_AfterCheckStateChanged(object sender, EventArgs e)
        {
            UltraCheckEditor ultraCheckEditor = (UltraCheckEditor)sender;
            // All check box event
            if (ultraCheckEditor.Text == "All")
            {
                if (ultraCheckEditor.Checked)
                {
                    ChangeDataFilterEntities(ultraCheckEditor.Name, true); // Check all
                    foreach (Control control in ultraCheckEditor.Parent.Controls)
                    {
                        UltraCheckEditor editor = control as UltraCheckEditor;
                        if (editor == null)
                        {
                            continue;
                        }
                        if (!editor.Text.Equals("All") && !editor.Checked)
                        {
                            this.TickDataFilterValueChecker(editor, true);
                        }
                    }
                }
                else
                {
                    ChangeDataFilterEntities(ultraCheckEditor.Name, false); // Uncheck all
                    foreach (Control control in ultraCheckEditor.Parent.Controls)
                    {
                        UltraCheckEditor editor = control as UltraCheckEditor;
                        if (editor == null)
                        {
                            continue;
                        }
                        if (!editor.Text.Equals("All") && editor.Checked)
                        {
                            this.TickDataFilterValueChecker(editor, false);
                        }
                    }
                }
            }
            // Child check box event
            else
            {
                if (ultraCheckEditor.Checked)
                {
                    ChangeDataFilterEntities(ultraCheckEditor.Name, true);  // Execute change data filter values change event
                }
                else
                {
                    ChangeDataFilterEntities(ultraCheckEditor.Name, false);  // Execute change data filter values change event
                    foreach (Control control in ultraCheckEditor.Parent.Controls)
                    {
                        UltraCheckEditor editor = control as UltraCheckEditor;
                        if (editor == null)
                        {
                            continue;
                        }
                        if (editor.Text == "All" && editor.Checked)
                        {
                            this.TickDataFilterValueChecker(editor, false);
                            break;
                        }
                    }
                }
            }

            View.SetDirty();
        }

        /// <summary>
        /// Ticks the data filter value checker.
        /// </summary>
        /// <param name="checker">The checker.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        private void TickDataFilterValueChecker(UltraCheckEditor checker, bool isChecked)
        {
            checker.AfterCheckStateChanged -= ultraCheckEditor_AfterCheckStateChanged;
            checker.Checked = isChecked;
            checker.AfterCheckStateChanged += ultraCheckEditor_AfterCheckStateChanged;
        }

        #endregion

        #region Change User Data Filter Values Action

        /// <summary>
        /// Changes the data filter entities.
        /// </summary>
        /// <param name="RoleNameAndDataFilterIDAndDataFilterValueID">The role name and data filter ID and data filter value ID.</param>
        /// <param name="Checked">if set to <c>true</c> [checked].</param>
        private void ChangeDataFilterEntities(string RoleNameAndDataFilterIDAndDataFilterValueID, bool Checked)
        {
            if (this.isInitializing) return;

            if (View.DataFilterEntities == null) View.DataFilterEntities = new List<DataFilterEntity>();
            string[] RoleNameAndDataFilterIDAndDataFilterValueIDArray = RoleNameAndDataFilterIDAndDataFilterValueID.Split('|');
            string RoleName = RoleNameAndDataFilterIDAndDataFilterValueIDArray[0];
            string DataFilterID = RoleNameAndDataFilterIDAndDataFilterValueIDArray[1];
            string DataFilterValueID = RoleNameAndDataFilterIDAndDataFilterValueIDArray[2];

            if (Checked)
            {
                if (DataFilterValueID.Equals("*")) CleanAllDataFilterValues(RoleName, DataFilterID);
                bool flag = false;
                foreach (DataFilterEntity entity in View.DataFilterEntities)
                {
                    if (entity.RoleName == RoleName &&
                        entity.DataFilterID == DataFilterID &&
                        entity.DataFilterValueID == DataFilterValueID)
                    {
                        flag = true;
                        if (entity.RecordStatus == DataFilterRecordStatus.Delete)
                        {
                            entity.RecordStatus = DataFilterRecordStatus.Original;
                        }
                        break;
                    }
                }
                if (!flag)
                {
                    AddNewUserDataFilterValue(RoleName, DataFilterID, DataFilterValueID);
                }
            }
            else
            {
                if (DataFilterValueID.Equals("*"))
                {
                    CleanAllDataFilterValues(RoleName, DataFilterID);
                }
                else
                {
                    bool isFullAuthorisedDataFilter = this.IsFullAuthorisedDataFilterOnSelectedUser(RoleName, DataFilterID);
                    if (isFullAuthorisedDataFilter)
                    {
                        List<DataFilterEntity> fullDFVOnDataFilter = this.GetFullDFVOnDataFilter(DataFilterID);
                        foreach (DataFilterEntity dfv in fullDFVOnDataFilter)
                        {
                            bool isExisted = false;
                            foreach (DataFilterEntity uEntity in View.DataFilterEntities)
                            {
                                if (uEntity.RoleName.Equals(RoleName)
                                    && uEntity.DataFilterID.Equals(dfv.DataFilterID)
                                    && uEntity.DataFilterValueID.Equals(dfv.DataFilterValueID))
                                {
                                    uEntity.RecordStatus = DataFilterRecordStatus.Original;
                                    isExisted = true;
                                    break;
                                }
                            }
                            if (!isExisted)
                            {
                                AddNewUserDataFilterValue(RoleName, dfv.DataFilterID, dfv.DataFilterValueID);
                            }
                        }
                    }

                    int i = 0;
                    int iMAX = 1;
                    if (isFullAuthorisedDataFilter) iMAX = 2;
                    for (int index = View.DataFilterEntities.Count - 1; index > -1; index--)
                    {
                        if (View.DataFilterEntities[index].RoleName.Equals(RoleName)
                            && View.DataFilterEntities[index].DataFilterID.Equals(DataFilterID)
                            && (View.DataFilterEntities[index].DataFilterValueID.Equals(DataFilterValueID) || View.DataFilterEntities[index].DataFilterValueID.Equals("*")))
                        {
                            switch (View.DataFilterEntities[index].RecordStatus)
                            {
                                case DataFilterRecordStatus.New:
                                    View.DataFilterEntities.RemoveAt(index);
                                    break;
                                case DataFilterRecordStatus.Original:
                                    View.DataFilterEntities[index].RecordStatus = DataFilterRecordStatus.Delete;
                                    break;
                            }
                            i++;
                            if (i == iMAX) break;
                        }
                    }
                }
            }
        }

        private void AddNewUserDataFilterValue(string rolename, string dataFilterID, string dataFilterValuesID)
        {
            DataFilterEntity addedDataFilterEntity = new DataFilterEntity();
            addedDataFilterEntity.UserDataFilterValueID = Guid.NewGuid().ToString();
            addedDataFilterEntity.UserName = Key;
            addedDataFilterEntity.RoleName = rolename;
            addedDataFilterEntity.DataFilterID = dataFilterID;
            addedDataFilterEntity.DataFilterValueID = dataFilterValuesID;
            addedDataFilterEntity.RecordStatus = DataFilterRecordStatus.New;
            View.DataFilterEntities.Add(addedDataFilterEntity);
        }

        private void CleanAllDataFilterValues(string rolename, string dataFilterID)
        {
            for (int i = View.DataFilterEntities.Count - 1; i > -1; i--)
            {
                if (View.DataFilterEntities[i].RoleName.Equals(rolename)
                    && View.DataFilterEntities[i].DataFilterID.Equals(dataFilterID))
                {
                    switch (View.DataFilterEntities[i].RecordStatus)
                    {
                        case DataFilterRecordStatus.New:
                            View.DataFilterEntities.RemoveAt(i);
                            break;
                        case DataFilterRecordStatus.Original:
                            View.DataFilterEntities[i].RecordStatus = DataFilterRecordStatus.Delete;
                            break;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Data Filter

         /// <summary>
        /// Get user data filters by current user's name and rolename
        /// </summary>
        /// <param name="userName">rolename</param>
        /// <returns>dataFilterEntities</returns>
        private List<DataFilterEntity> GetUserDataFiltersByUserName(string userName)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                return proxy.GetDataFiltersForUser(userName, string.Empty, false);

            }
        }
        private List<DataFilterEntity> GetCopiedUserDataFiltersByUserName(string username)
        {

            List<DataFilterEntity> copiedUserDataFilterEntity ;
            List<DataFilterEntity> currentUserDataFilterEntity ;

            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                copiedUserDataFilterEntity = proxy.GetDataFiltersForUser(username, string.Empty, false);
                currentUserDataFilterEntity = proxy.GetDataFiltersForUser(AppContext.Current.UserName, string.Empty, false);
            }
            
            List<DataFilterEntity> canCopyDataFilters = new List<DataFilterEntity>();


            foreach (DataFilterEntity currentUserEntity in currentUserDataFilterEntity)
            {
                var id = currentUserEntity.DataFilterID;
                var list = copiedUserDataFilterEntity
                            .Where(d => d.DataFilterID.Equals(id));

                if (currentUserEntity.DataFilterValueID == "*")
                {
                    foreach (var item in list)
                    {
                        var entityItem = item;
                        if (canCopyDataFilters.Where(x => x.DataFilterID == entityItem.DataFilterID
                            && x.DataFilterValueID == entityItem.DataFilterValueID
                            && x.RoleName == entityItem.RoleName)
                                    .Count()==0)
                        {
                            var tempEntity = new DataFilterEntity(Guid.NewGuid().ToString(),
                                "",//userName, here still not know what the user name is
                                entityItem.RoleID, entityItem.RoleName,
                                entityItem.DataFilterID, entityItem.DataFilter,
                                entityItem.DataFilterValueID, entityItem.DataFilterValue,
                                OfficeRecordStatus.New);
                            canCopyDataFilters.Add(tempEntity);
                        }
                    }
                }
                else
                {
                    foreach (var item in list)
                    {
                        var entityItem = item;
                        if ((entityItem.DataFilterValueID == currentUserEntity.DataFilterValueID || entityItem.DataFilterValueID == "*")
                            && canCopyDataFilters.Where(x => x.DataFilterID == entityItem.DataFilterID
                                && x.DataFilterValueID == entityItem.DataFilterValueID
                                && x.RoleName == entityItem.RoleName).Count() == 0)
                        {
                            //when current user has limited data filters, just can add its data filters which is in the data filters of source user
                            var tempEntity = new DataFilterEntity(Guid.NewGuid().ToString(),
                                "",//userName, here still not know what the user name is
                                entityItem.RoleID, entityItem.RoleName,
                                currentUserEntity.DataFilterID, currentUserEntity.DataFilter,
                                currentUserEntity.DataFilterValueID, currentUserEntity.DataFilterValue,
                                OfficeRecordStatus.New);
                            canCopyDataFilters.Add(tempEntity);
                            break;
                        }
                    }
                }
            }
            return canCopyDataFilters;
        }

        [EventPublication(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.EnableDisableViewDF, PublicationScope.Global)]
        public event EventHandler<EventArgs<bool>> EnableDisableViewDFButton;



        internal virtual void EnableDisableViewDF(bool Enable)
        {
            if (EnableDisableViewDFButton != null)
                EnableDisableViewDFButton(this, new EventArgs<bool>(Enable));
        }

        #endregion

        #endregion
    }
}


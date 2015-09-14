using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HiiP.Foundation.DMS.BusinessEntity;
using HiiP.Foundation.DMS.Interface;
using HiiP.Foundation.DMS.Interface.Services;
using HiiP.Foundation.Workflow.Interface.BusinessEntities;
using HiiP.Framework.Common;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.CompositeUI;


namespace HiiP.Framework.Security.UserManagement
{
    internal class IntegratedETHelper:IDisposable
    {
        private string _userName;
        public DataSetETRoles ETRoles;
        private DataSetETRoles _ETRolesData;
        private DataSetETRoles _ETRemovedData;

        private IDMSService _DMSService;
      

        public IntegratedETHelper(string userName, WorkItem workItem)
        {
            _ETRemovedData = new DataSetETRoles();
            _DMSService = workItem.RootWorkItem.Services.Get<IDMSService>();
            this._userName = userName;
            // this.MockETDatas();
            _ETRolesData = new DataSetETRoles();
           // _ETRolesData = this.ETData(_userName);
           //this._ETRolesData.ReadXml("ETDataTest.xml");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(ETRoles!=null) ETRoles.Dispose();
                if(_ETRolesData!=null) _ETRolesData.Dispose();
                if(_ETRemovedData!=null)_ETRemovedData.Dispose();
            }
        }

        public string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
            }
        }


        public DataSetETRoles DMSRoles()
        {
            ETRoles = new DataSetETRoles();
            DataSetETRoles.ETTableRow newRow = ETRoles.ETTable.NewETTableRow();
                     
               
            string ProfileName = string.Empty;
            char[] splitter = { '\n' };

            try
            {
                if (_DMSService == null)
                {
                    //when no DMS authorization, means DMS service not loaded.
                    return ETRoles;
                }
                string[] DMSRoles = _DMSService.GetAllProfiles().Split(splitter);
                if (DMSRoles==null || DMSRoles.Length==0)
                {
                    return ETRoles;
                }

                for (int i = 0; i < DMSRoles.Length; i++)
                {
                    if (!string.IsNullOrEmpty(DMSRoles[i].ToString()))
                    {
                        ProfileName = DMSRoles[i].ToString().Replace("\r", "").Trim();
                        //ProfileName = ProfileName.Substring(2, ProfileName.Length - 2).Trim();

                        newRow = ETRoles.ETTable.NewETTableRow();
                        newRow.BeginEdit();
                        newRow.RoleName = ProfileName;
                        newRow.RoleType = ETType.DMS.ToString();
                        newRow.Description = ProfileName;
                        newRow.UserName = UserName;
                        newRow.EndEdit();
                        ETRoles.ETTable.AddETTableRow(newRow);
                    }
                }
            }
            catch (Exception ex)
            {
                if (DMSExceptionManager.Handle(ex) == ConnectTrimResult.NoTrimUser)
                {
                    Utility.ShowMessageBox(Messages.DMS.DME002);
                    //MessageBox.Show("You are not DMS user, please contact your System Administrator", "Error");
                }
                else if (DMSExceptionManager.Handle(ex) == ConnectTrimResult.ConfigurationIncorrect)
                {
                    Utility.ShowMessageBox(Messages.DMS.DME003);
                    //MessageBox.Show("DMS trim configuration is incorrect, please contact your System Administrator", "Error");
                }
                else
                {
                    if (ExceptionManager.Handle(ex)) throw ;
                }
            }

            return ETRoles;
        }

        /// <summary>
        /// Retrieve Workflow Group.
        /// </summary>
        /// <returns></returns>
        public DataSetETRoles RetrieveWorkflowGroup(string groupName, string description)
        {
            string expression = this.GetExpressionByRoleAndDesc("G");
            return RetrieveETRolesByWildParameter(this.GetETRolesByExpression(expression), groupName, description);
        }

        /// <summary>
        /// Retrieve DMS Roles
        /// </summary>
        /// <returns></returns>
        public DataSetETRoles RetrieveDMSRoles(string roleName, string description)
        {
            string expression = this.GetExpressionByRoleAndDesc("DMS");
            return RetrieveETRolesByWildParameter(this.GetETRolesByExpression(expression), roleName, description);
        }

        /// <summary>
        /// Retrieve GISRoles
        /// </summary>
        /// <returns></returns>
        public DataSetETRoles RetrieveGISRoles(string roleName, string description)
        {
            string expression = this.GetExpressionByRoleAndDesc("GIS");
            return RetrieveETRolesByWildParameter(this.GetETRolesByExpression(expression), roleName, description);
        }
       
        private DataSetETRoles RetrieveETRolesByWildParameter(DataSetETRoles ETRoles, string roleName, string desc)
        {
            if (ETRoles.ETTable.Count == 0) return ETRoles;

            foreach (DataSetETRoles.ETTableRow row in ETRoles.ETTable.Select(string.Empty, "RoleName"))
            {
                if (!this.IsMatchRoleName(roleName, row.RoleName))
                {
                    row.Delete();
                }
                else if (!this.IsMatchDescription(desc, row.Description))
                    row.Delete();
            }
            if (ETRoles.HasChanges()) ETRoles.ETTable.AcceptChanges();

            return ETRoles;
        }

        private bool IsMatchRoleName(string roleNameToMatch, string roleName)
        {
            return (string.IsNullOrEmpty(roleNameToMatch)
                   || SearchHelper.IsRegexMatch(roleName.ToLower(), roleNameToMatch.ToLower(), @"[\w|\W]*"));
        }

        private bool IsMatchDescription(string descriptionToMatch, string description)
        {
            return (string.IsNullOrEmpty(descriptionToMatch))
                    || (SearchHelper.IsRegexMatch(description.ToLower(), descriptionToMatch.ToLower(), @"[\w|\W]*"));
        }

        private DataSetETRoles GetETRolesByExpression(string expression)
        {
            
            try
            {
                ETRoles.BeginInit();
                ETRoles.Merge(this._ETRolesData.Tables[0].Select(expression));
                ETRoles.EndInit();

            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            return ETRoles;

        }

        private string GetExpressionByRoleAndDesc(string roleType)
        {
            ////bool isUsedWildExpression = false;
            string expression = string.Format("(RoleType='{0}')", roleType);
            //string changed = string.Empty;
            //if (!string.IsNullOrEmpty(roleName)) 
            //{
            //    changed = ChangeCharacter(roleName, out isUsedWildExpression);
            //    if (isUsedWildExpression)
            //    {
            //        expression = string.Format("{0} and (RoleName like '{1}')", expression, changed);
            //    }
            //}

            //if (!string.IsNullOrEmpty(description))
            //{
            //    changed = ChangeCharacter(description, out isUsedWildExpression);
            //    if (isUsedWildExpression)
            //    {
            //        expression = string.Format("{0} and (Description like '{1}')", expression, changed);
            //    }
            //}
            return expression;

        }
            
         
        #region "UI Logic"
        public void SetDataToGrid(ETType type, UltraGrid leftGrid, DataSetETRoles rightETRoles, string name, string desc)
        {
            try
            {
                DataSetETRoles allETRoles = new DataSetETRoles();
                if (type == ETType.Workflow)
                {
                    allETRoles = this.RetrieveWorkflowGroup(name, desc);
                }
                else if (type == ETType.DMS)
                {
                    allETRoles = this.RetrieveDMSRoles(name, desc);
                }
                else if (type == ETType.GIS)
                {
                    allETRoles = this.RetrieveGISRoles(name, desc);
                }

                if (rightETRoles.ETTable.Count > 0)
                {
                    foreach (DataSetETRoles.ETTableRow row in rightETRoles.ETTable)
                    {
                        string filter = string.Format("RoleName = '{0}'", (row.IsRoleNameNull())?"":row.RoleName.Replace("'","''"));
                        DataRow[] rows = allETRoles.ETTable.Select(filter, "RoleName");
                        if (rows.Length > 0) rows[0].Delete();
                    }
                    if (allETRoles.HasChanges()) allETRoles.ETTable.AcceptChanges();
                }

                this.BindGird(leftGrid, GetResortETRoles(allETRoles));

            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        public void BindGird(UltraGrid grid, DataSetETRoles allETRoles)
        {
            grid.DataSource = allETRoles;
            grid.DataMember = allETRoles.Tables[0].TableName;

        }

        public void ResetGrid(UltraGrid leftGrid, UltraTextEditor nameEdit, UltraTextEditor desEdit)
        {
            DataSetETRoles ETRoles = new DataSetETRoles();
            leftGrid.DataSource = ETRoles;
            leftGrid.DataMember = ETRoles.Tables[0].TableName;

            nameEdit.Text = string.Empty;
            desEdit.Text = string.Empty;


        }


        public void UnAssignETRoles(UltraGrid leftGrid, UltraGrid rightGrid)
        {
            DataSetETRoles currentRoles = rightGrid.DataSource as DataSetETRoles;
            _ETRemovedData = new DataSetETRoles();
            if (currentRoles == null) return;
            DataSetETRoles.ETTableRow newRow ;
            DataSetETRoles leftETRoles = leftGrid.DataSource as DataSetETRoles;
            DataSetETRoles.ETTableRow[] dataRows = GetTickedDataRows(rightGrid, currentRoles);
            if (leftETRoles == null) leftETRoles = new DataSetETRoles();
            {
               if (dataRows == null)return;
               for (int i = 0; i < dataRows.Length; i++)
               {
                   //_ETRemovedData.ETTable.Rows.Add(dataRows);

                   newRow = _ETRemovedData.ETTable.NewETTableRow();
                   newRow.BeginEdit();
                   newRow.RoleName = dataRows[i][_ETRemovedData.ETTable.RoleNameColumn.ColumnName].ToString();
                   newRow.RoleType = dataRows[i][_ETRemovedData.ETTable.RoleTypeColumn.ColumnName].ToString();
                   newRow.Description = dataRows[i][_ETRemovedData.ETTable.DescriptionColumn.ColumnName].ToString();
                   newRow.UserName = dataRows[i][_ETRemovedData.ETTable.UserNameColumn.ColumnName].ToString();
                   newRow.EndEdit();
                   _ETRemovedData.ETTable.AddETTableRow(newRow);

               }
            }

            this.MoveRowAndCreateNew(dataRows, leftETRoles);
            leftETRoles.AcceptChanges();

            this.BindGird(leftGrid, GetResortETRoles(leftETRoles));


        }

        private DataSetETRoles GetResortETRoles(DataSetETRoles currentETRoles)
        {
            DataSetETRoles.ETTableRow[] sortRows = currentETRoles.ETTable.Select(string.Empty, "RoleName") as DataSetETRoles.ETTableRow[];
            DataSetETRoles returnData = new DataSetETRoles();
            if (sortRows == null) return returnData;
            if (sortRows.Length == 0) return returnData;
            returnData.ETTable.BeginLoadData();
            returnData.Merge(sortRows);
            returnData.ETTable.EndLoadData();
            return returnData;
        }
       

        private const string TickFlagColumnName = "TickFlag";
        public void AssignETRoles(UltraGrid leftGrid,
                                  DataSetETRoles currentETRoles)
        {
            DataSetETRoles leftRoles = leftGrid.DataSource as DataSetETRoles;
            if (leftRoles == null) return;
            DataSetETRoles.ETTableRow[] dataRows = GetTickedDataRows(leftGrid, leftRoles);
            if (dataRows == null) return;
            this.MoveRowAndCreateNew(dataRows, currentETRoles);
            leftRoles.AcceptChanges();
        }

        private void MoveRowAndCreateNew(DataSetETRoles.ETTableRow[] dataRows, DataSetETRoles currentETRoles)
        {
            foreach (DataSetETRoles.ETTableRow row in dataRows)
            {
                DataSetETRoles.ETTableRow newRow = currentETRoles.ETTable.NewETTableRow();
                newRow.ItemArray = row.ItemArray;
                currentETRoles.ETTable.AddETTableRow(newRow);
                row.Delete();
            }
        }

        private DataSetETRoles.ETTableRow[] GetTickedDataRows(UltraGrid currentGrid, DataSetETRoles currentRoles)
        {
            if (currentRoles == null) return null;
            string expression = string.Empty;
            this.GetTickedRoleNames(currentGrid, out expression);
            if (string.IsNullOrEmpty(expression)) return null;
            return (currentRoles.ETTable.Select(expression) as DataSetETRoles.ETTableRow[]);
        }

        private List<string> GetTickedRoleNames(UltraGrid currentGrid, out string expression)
        {
            expression = string.Empty;
            List<string> tickedList = new List<string>();
            foreach (UltraGridRow gridRow in currentGrid.Rows)
            {
                if (!gridRow.IsDataRow) continue;
                bool flag = false;
                bool.TryParse(gridRow.Cells[TickFlagColumnName].Value.ToString(), out flag);
                if (flag)
                {
                    string data = gridRow.Cells["RoleName"].Value.ToString().Replace("'", "''");
                    tickedList.Add(gridRow.Cells["RoleName"].Value.ToString());
                    if (string.IsNullOrEmpty(expression))
                    {
                        expression = string.Format("(RoleName = '{0}')", data);
                    }
                    else
                    {
                        expression = string.Format("{0} or (RoleName = '{1}')", expression, data);
                    }
                }
            }
            return tickedList;
        }

        public static class ETTypeValue
        {
            public const string Workflow = "WF";
            public const string DMS = "DMS";
            public const string GIS = "GIS";
        }

        public enum ETType
        {
            Workflow,
            DMS,
            GIS
        }

        public string FindTrimUser(string userName)
        {
            if (_DMSService==null)
            {
                //when no DMS authorization, means DMS service not loaded.
                return string.Empty;
            }
            return _DMSService.GetUsersProfile(userName);
        }


        #endregion

        public DataSetETRoles GetWFRolesForUser(string userName)
        {
            DataSetETRoles WFGroups = new DataSetETRoles();
            DataSetETRoles.ETTableRow WFRow = WFGroups.ETTable.NewETTableRow();
            this.UserName = userName;
            try
            {
                try
                {

                    using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                    {
                        GroupQueueDataSet.WF_GroupQueueDataTable GQDS = new GroupQueueDataSet.WF_GroupQueueDataTable();
                        GQDS = proxy.RetrieveGroupsByUser(userName).WF_GroupQueue;
                        for (int i = 0; i < GQDS.Rows.Count; i++)
                        {
                            WFRow = WFGroups.ETTable.NewETTableRow();
                            WFRow.BeginEdit();
                            WFRow[WFGroups.ETTable.RoleNameColumn.ColumnName] = GQDS.Rows[i][GQDS.GroupNameColumn.ColumnName].ToString();
                            WFRow[WFGroups.ETTable.DescriptionColumn.ColumnName] = GQDS.Rows[i][GQDS.GroupDescColumn.ColumnName].ToString();
                            WFRow[WFGroups.ETTable.UserNameColumn.ColumnName] = userName;
                            WFRow[WFGroups.ETTable.RoleTypeColumn.ColumnName] = "G";//--- To Dertermine that this is a user
                            WFRow[WFGroups.ETTable.IsSupervisorColumn.ColumnName] = IsGroupSupervisor(GQDS.Rows[i][GQDS.QueueSupervisorNameColumn.ColumnName].ToString());
                            WFRow.EndEdit();
                            WFGroups.ETTable.AddETTableRow(WFRow);
                        }

                    }

                }
                catch (Exception ex)
                {
                    ExceptionManager.HandleWithLogOnly(ex);
                }
               
                return WFGroups;
            }
            finally
            {
                WFGroups = null;
            }
        }

        public DataSetETRoles GetParticipationForUser(string userName)
        {
            DataSetETRoles WFGroups = new DataSetETRoles();
            DataSetETRoles.ETTableRow WFRow = WFGroups.ETTable.NewETTableRow();
            try
            {
                List<UserEntity> Participents = new List<UserEntity>();
                using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                {
                    Participents = proxy.GetParticipation(userName);

                    for (int i = 0; i < Participents.Count; i++)
                    {
                        WFRow = WFGroups.ETTable.NewETTableRow();
                        WFRow.BeginEdit();
                        WFRow[WFGroups.ETTable.RoleNameColumn.ColumnName] = Participents[i].UserName.ToString();
                        WFRow[WFGroups.ETTable.DescriptionColumn.ColumnName] = Participents[i].Description.ToString();
                        WFRow[WFGroups.ETTable.UserNameColumn.ColumnName] = userName;
                        WFRow[WFGroups.ETTable.RoleTypeColumn.ColumnName] = "U";//--- To Dertermine that this is a user
                        WFRow[WFGroups.ETTable.IsSupervisorColumn.ColumnName] = IsGroupSupervisor(Participents[i].SupervisorID);
                        WFRow.EndEdit();
                        WFGroups.ETTable.AddETTableRow(WFRow);
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.HandleWithLogOnly(ex);
            }

            return WFGroups;
        }

    
        public DataSetETRoles GetWFUserQueues()
        {

            DataRow drRemove;

            ETRoles = new DataSetETRoles();
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                ETRoles = proxy.RetrieveAllWFUsers();
            }
            ETRoles.ETTable.PrimaryKey = new DataColumn[] { ETRoles.ETTable.RoleNameColumn };
            drRemove = ETRoles.ETTable.Rows.Find(this.UserName);
            if (drRemove != null)
                ETRoles.ETTable.Rows.Remove(drRemove);

            return ETRoles;
        }
        
        public DataSetETRoles GetAllWFGroups()
        {

            ETRoles = new DataSetETRoles();
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                ETRoles =  proxy.RetrieveAllWFGroups();
            }
            return ETRoles;
        }

        private bool IsGroupSupervisor(string usernames)
        {
            bool isSupervisor = false;
            char[] splitter = { ',' };
            isSupervisor = usernames.ToLower().Split(splitter).Contains(this.UserName.ToLower());
            return isSupervisor;
        }


        internal UserEntity GetWFUser(string username)
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                return proxy.GetWFUser(username);
            }
        }

        public DataSetETRoles GetAllGISRoles()
        {
            ETRoles = new DataSetETRoles();
            DataSetETRoles.ETTableRow GISRow = ETRoles.ETTable.NewETTableRow();
            string[]GISRoles; 
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                GISRoles = proxy.GetAllGISRoles();

                for (int i = 0; i < GISRoles.Length; i++)
                {
                    GISRow = ETRoles.ETTable.NewETTableRow();
                    GISRow.BeginEdit();
                   // GISRow[ETRoles.ETTable.RoleIDColumn.ColumnName] = GISRoles[i].ToString();
                    GISRow[ETRoles.ETTable.RoleNameColumn.ColumnName] = GISRoles[i].ToString();
                    GISRow[ETRoles.ETTable.DescriptionColumn.ColumnName] = GISRoles[i].ToString();
                    GISRow[ETRoles.ETTable.UserNameColumn.ColumnName] = _userName;
                    GISRow[ETRoles.ETTable.RoleTypeColumn.ColumnName] = Framework.Security.UserManagement.Interface.Constants.EnablingTechnologies.GIS.ToString();
                    GISRow.EndEdit();
                    ETRoles.ETTable.AddETTableRow(GISRow);
                }
            }

            return ETRoles;
        }

        public DataSetETRoles GetUsersGISRoles(string UserName)
        {
            DataSetETRoles _ETRoles = new DataSetETRoles();
            DataSetETRoles.ETTableRow GISRow = _ETRoles.ETTable.NewETTableRow();
            List<ETRoleEntity> GISRoles;
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                GISRoles = new List<ETRoleEntity>();
                GISRoles = proxy.GetGISRolesForUser(UserName);
                for (int i = 0; i < GISRoles.Count; i++)
                {
                    GISRow = _ETRoles.ETTable.NewETTableRow();
                    GISRow.BeginEdit();
                   // GISRow[_ETRoles.ETTable.RoleIDColumn.ColumnName] = GISRoles[i].RoleName;
                    GISRow[_ETRoles.ETTable.RoleNameColumn.ColumnName] = GISRoles[i].RoleName;
                    GISRow[_ETRoles.ETTable.DescriptionColumn.ColumnName] = GISRoles[i].RoleName;
                    GISRow[_ETRoles.ETTable.UserNameColumn.ColumnName] = GISRoles[i].UserId;
                    GISRow[_ETRoles.ETTable.RoleTypeColumn.ColumnName] = Framework.Security.UserManagement.Interface.Constants.EnablingTechnologies.GIS.ToString();
                    GISRow.EndEdit();
                    _ETRoles.ETTable.AddETTableRow(GISRow);
                }
            }
            return _ETRoles;
        }
    }
}

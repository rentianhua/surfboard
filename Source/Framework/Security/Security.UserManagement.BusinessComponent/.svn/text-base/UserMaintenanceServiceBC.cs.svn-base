#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Business component
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using HiiP.Foundation.DMS.BusinessEntity;
using HiiP.Foundation.DMS.Interface.Services;
using HiiP.Foundation.Workflow.Interface.BusinessEntities;
using HiiP.Foundation.Workflow.Interface.Constants;
using HiiP.Foundation.Workflow.Interface.Services;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Common.Server.CallHandlers;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.DataAccess;
using HiiP.Modules.ExternalParty.BusinessEntity.BusinessEntities;
using HiiP.Modules.ExternalParty.Interface.Services;
using Constants = HiiP.Framework.Security.UserManagement.Interface.Constants;

namespace HiiP.Framework.Security.UserManagement.BusinessComponent
{
    public class UserMaintenanceServiceBC : HiiPBusinessComponentBase
    {
        #region "Properties"
        private UserMaintenanceServiceDA _userMaintenanceServiceDA ;

        private UserMaintenanceServiceDA DAInstance
        {
            get
            {
                if (_userMaintenanceServiceDA == null)
                {
                    _userMaintenanceServiceDA =  InstanceBuilder.Wrap<UserMaintenanceServiceDA>(new UserMaintenanceServiceDA());
                }

                return _userMaintenanceServiceDA;
            }
        }

        private IWorkflowService _workflowService;

        private IWorkflowService WorkflowService
        {
            get
            {
                if (_workflowService == null)
                {
                    _workflowService = InstanceBuilder.CreateInstance<IWorkflowService>();
                }

                return _workflowService;
            }
        }

        private IDMSService _DMSService ;

        private IDMSService DMSService
        {
            get
            {
                if (_DMSService == null)
                {
                    _DMSService = InstanceBuilder.CreateInstance<IDMSService>();
                }

                return _DMSService;
            }
        }
        #endregion

        #region "User detail"
        /// <summary>
        /// Create user information(user account, user basic information)
        /// </summary>
        /// <param name="userInfoEntity"></param>
        /// <param name="dataFilterEntities"></param>
        /// <param name="roles"></param>
        /// <param name="isAdministrator"></param>
        /// <param name="superviseGroup"></param>
        /// <param name="assignedGroups"></param>
        /// <param name="dmsProfile"></param>
        /// <param name="etRoleEntity"></param>
        /// <param name="removedOfficeList"></param>
        /// <param name="newOfficeList"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.UserModuleID,
            FunctionID = Constants.FunctionNames.NewUserFunctionID)]
        public Dictionary<string, string> CreateUserInfo(UserInfoEntity userInfoEntity, List<DataFilterEntity> dataFilterEntities, string[] roles, bool isAdministrator, string[] superviseGroup, string[] assignedGroups, String[] dmsProfile, List<ETRoleEntity> etRoleEntity,
            OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            Dictionary<string, string> StatusString = new Dictionary<string, string>();
            try
            {

                InstanceBuilder.Wrap<UserMaintenanceServiceBC>(this).CreateHiiPUser(userInfoEntity, dataFilterEntities, roles, removedOfficeList, newOfficeList);
                StatusString.Add(Constants.Exceptions.HiiPUserCreationSuccess, "");
            }
            catch (Exception ex)
            {
                StatusString.Add(Constants.Exceptions.HiiPUserCreationException, ex.ToString());
                throw;
            }


            try
            {
                if (etRoleEntity.Count > 0)
                {
                    InstanceBuilder.Wrap<UserMaintenanceServiceBC>(this).CreateUsersETRole(etRoleEntity);
                }
                StatusString.Add(Constants.Exceptions.GISUserCreationSuccess, "");
            }
            catch (Exception ex)
            {
                StatusString.Add(Constants.Exceptions.GISPUserCreationException, ex.ToString());
            }

            try
            {
                CreateWFUser(userInfoEntity, isAdministrator, superviseGroup, assignedGroups);
                StatusString.Add(Constants.Exceptions.WorkflowUserCreationSuccess, "");
            }
            catch (Exception ex)
            {
                StatusString.Add(Constants.Exceptions.WorkflowUserCreationException, ex.ToString());
            }

            try
            {
                CreateTRIMUser(userInfoEntity, dmsProfile);
                StatusString.Add(Constants.Exceptions.DMSUserCreationSuccess, "");
            }
            catch (Exception ex)
            {
                StatusString.Add(Constants.Exceptions.DMSUserCreationException, ex.ToString());
            }
            //try
            //{
            //    if (Participation.Length > 0)
            //        InstanceBuilder.Wrap<UserMaintenanceServiceBC>(this).UpdateUsersSupervisors(SuperviseUsers, Participation, userInfoEntity.UserName);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            return StatusString;
        }


        [TransactionEnlistCallHandler(Ordinal = 1)]
        [AuditLogCallHandler(Constants.FunctionNames.NewUserFunctionID, Ordinal = 2)]
        public void CreateHiiPUser(UserInfoEntity userInfoEntity,
            List<DataFilterEntity> dataFilterEntities,
            string[] roles,
            OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            this.DAInstance.CreateUserInfo(userInfoEntity, dataFilterEntities, roles);
            SaveOrgasations( removedOfficeList, newOfficeList);

        }


        private void CreateTRIMUser(UserInfoEntity userInfoEntity, String[] dmsProfile)
        {
            this.CreateDMSUser(userInfoEntity, dmsProfile);
        }


        /// <summary>
        /// Accroding to username, updating user account and information
        /// </summary>
        /// <param name="userInfoEntity"></param>
        /// <param name="dataFilterEntities"></param>
        /// <param name="roles"></param>
        /// <param name="isAdministrator"></param>
        /// <param name="superviseGroup"></param>
        /// <param name="assignGroups"></param>
        /// <param name="oldParticipation"></param>
        /// <param name="newParticipation"></param>
        /// <param name="dmsProfile"></param>
        /// <param name="wfHasChanged"></param>
        /// <param name="dmsHasChanged"></param>
        /// <param name="gisRoles"></param>
        /// <param name="deletedGISRoles"></param>
        /// <param name="removedOfficeList"></param>
        /// <param name="newOfficeList"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.UserModuleID,
            FunctionID = Constants.FunctionNames.UpdateUserFunctionID)]
        public Dictionary<string, string> UpdateUserInfo(UserInfoEntity userInfoEntity, List<DataFilterEntity> dataFilterEntities, string[] roles, bool isAdministrator, string[] superviseGroup, string[] assignGroups, string[] oldParticipation, string[] newParticipation, string[] dmsProfile, bool wfHasChanged, bool dmsHasChanged, List<ETRoleEntity> gisRoles, List<ETRoleEntity> deletedGISRoles,
            OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            Dictionary<string, string> StatusString = new Dictionary<string, string>();

            // -- Enfoce the validation rule to not allow the User to update their own profile.
            if (userInfoEntity.UserName == AppContext.Current.UserName)
            {
                throw new HiiP.Framework.Security.UserManagement.Interface.ExceptionHandling.UserManagementException(Constants.Exceptions.HiiPModifyOwnProfileError);
            }
            //-------------------------------------------------------------------------------------

            try
            {
                InstanceBuilder.Wrap<UserMaintenanceServiceBC>(this).UpdateHiiPUser(userInfoEntity, dataFilterEntities, roles,removedOfficeList,newOfficeList);
                StatusString.Add(Constants.Exceptions.HiiPUserUpdateSuccess, "");
            }
            catch (Exception ex)
            {
                StatusString.Add(Constants.Exceptions.HiiPUserUpdateException, ex.ToString());
                throw;
            }

            try
            {
                if (deletedGISRoles.Count > 0)
                {
                    InstanceBuilder.Wrap<UserMaintenanceServiceBC>(this).DeleteUsersETRole(deletedGISRoles);
                }
                if (gisRoles.Count > 0)
                {
                    InstanceBuilder.Wrap<UserMaintenanceServiceBC>(this).CreateUsersETRole(gisRoles);
                }
               
                StatusString.Add(Constants.Exceptions.GISUserUpdateSuccess, "");
            }
            catch (Exception ex)
            {
                StatusString.Add(Constants.Exceptions.GISPUserUpdateException, ex.ToString());
            }


            if (wfHasChanged)
            {
                try
                {
                    this.UpdateWFUser(userInfoEntity, isAdministrator, superviseGroup, assignGroups);

                    StatusString.Add(Constants.Exceptions.WorkflowUserUpdateSuccess, "");
                }
                catch (Exception ex)
                {

                    StatusString.Add(Constants.Exceptions.WorkflowUserUpdateException, ex.ToString());
                }
            }

            if (dmsHasChanged)
            {
                try
                {
                    this.CreateTRIMUser(userInfoEntity, dmsProfile);
                    StatusString.Add(Constants.Exceptions.DMSUserUpdateSuccess, "");
                }
                catch (Exception ex)
                {
                    StatusString.Add(Constants.Exceptions.DMSUserUpdateException, ex.ToString());
                }
            }  
              


            return StatusString;
        }

        private void SaveOrgasations(OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            if ((removedOfficeList == null || removedOfficeList.Rows.Count == 0)
                && (newOfficeList == null || newOfficeList.Rows.Count == 0))
            {
                return;
            }

            this.DAInstance.DeleteOfficesOfUser(removedOfficeList);
            this.DAInstance.InsertOfficesOfUser(newOfficeList);
        }

       
        [TransactionEnlistCallHandler(Ordinal = 1)]
        [AuditLogCallHandler(Constants.FunctionNames.UpdateUserFunctionID, Ordinal = 2)]
        public void UpdateHiiPUser(UserInfoEntity userInfoEntity, List<DataFilterEntity> dataFilterEntities, string[] roles, OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            this.DAInstance.UpdateUserInfo(userInfoEntity, dataFilterEntities, roles);
            SaveOrgasations(removedOfficeList, newOfficeList);
        }


        internal void UpdateWFUser(UserInfoEntity userInfoEntity, bool isAdministrator, string[] superviseGroup, string[] assignGroups)
        {

            bool isWorkflowUser ;
            UserEntity UE= new UserEntity();
            try
            {

                UE = GetWFUser(userInfoEntity.UserName);
                isWorkflowUser = UE.Equals(null) ? false : true;
            }
            catch (Exception)
            {
                isWorkflowUser = false;
            }

            if (isWorkflowUser)
            {
                /* this part is required to alliviate the issue caused by saveing varied character casing in Workflow which has caused a lot of issues */
                userInfoEntity.UserName = UE.UserName; 
                this.UpdateUserAttributes(userInfoEntity, isAdministrator, superviseGroup, assignGroups);
                if (isAdministrator)
                    this.SetWFUserRole(userInfoEntity.UserName, IprocessServer.WorkflowRole.ADMIN);
                else
                    this.SetWFUserRole(userInfoEntity.UserName, IprocessServer.WorkflowRole.USER);
            }
            else
            {
                this.CreateWFUser(userInfoEntity, isAdministrator, superviseGroup, assignGroups);
            }
        }



        //}

        /// <summary>
        /// Get user information by username
        /// </summary>
        /// <param name="userName">username</param>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.UserModuleID,
            FunctionID = Constants.FunctionNames.SearchUserFunctionID)]
        public UserInfoEntity GetUser(string userName)
        {
            return this.DAInstance.GetUser(userName);
        }

        
        /// <summary>
        /// Search user by coditions
        /// </summary>
        /// <param name="userInfoSearchCriteria">userInfoSearchCriteria</param>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.UserModuleID,
            FunctionID = Constants.FunctionNames.SearchUserFunctionID)]
        public UserInfoEntity[] FindUsers(UserInfoSearchCriteria userInfoSearchCriteria)
        {
            return this.DAInstance.FindUsers(userInfoSearchCriteria);
        }

        /// <summary>
        /// update users's status 
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.UserModuleID,
            FunctionID = Constants.FunctionNames.UpdateUserFunctionID)]
        public void UpdateUserStatus(List<string> userNames, string userStatus)
        {
            this.DAInstance.UpdateUserStatus(userNames, userStatus);
        }

        /// <summary>
        /// check user name is unique
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
          ModuleID = Constants.FunctionNames.UserModuleID,
          FunctionID = Constants.FunctionNames.UpdateUserFunctionID)]
        public bool UserExists(string userName)
        {
            return this.DAInstance.UserExists(userName);
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="allUsers">All users.</param>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
          ModuleID = Constants.FunctionNames.UserModuleID,
          FunctionID = Constants.FunctionNames.UpdateUserFunctionID)]
        public UserInfoEntity[] GetUsers(UserInfoEntity[] allUsers, string[] users)
        {
            List<UserInfoEntity> userEntities = new List<UserInfoEntity>();

            foreach (string username in users)
            {
                var tempUserName = username;
                if (this.UserExists(username))
                {
                    userEntities.Add(
                        (from userEntity in allUsers
                         where userEntity.UserName == tempUserName
                         select userEntity).Single());
                }
            }

            return userEntities.ToArray();
        }


        /// <summary>
        /// Assign users data filter values
        /// </summary>
        /// <param name="userNames">usernames</param>
        /// <param name="dataFilterEntities">dataFilterEntities</param>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
          ModuleID = Constants.FunctionNames.RoleModuleID,
          FunctionID = Constants.FunctionNames.AssignRolesToUsersFunctionID)]
        [TransactionEnlistCallHandler(Ordinal = 2)]
        [AuditLogCallHandler(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID, Ordinal = 3)]
        public void AssignUsersDataFilterValues(string[] userNames, List<DataFilterEntity> dataFilterEntities)
        {
            this.DAInstance.AssignUsersDataFilterValues(userNames, dataFilterEntities);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
        ModuleID = Constants.FunctionNames.RoleModuleID,
        FunctionID = Constants.FunctionNames.AssignRolesToUsersFunctionID)]
        public bool ExistAssignedUsersDataFilterValues(string[] userNames, List<DataFilterEntity> dataFilterEntities)
        {
            bool existedUserDataFilterValue = false;
            List<DataFilterEntity> userDataFilterValues;
            foreach (string username in userNames)
            {
                userDataFilterValues = this.DAInstance.GetUserDataFilterValuesByUserNameAndRoleName(username, String.Empty);
                if (userDataFilterValues.
                    Where < DataFilterEntity>(x=>this.DAInstance.CheckDataFilterEntity(x)).Count() != 0)
                {
                    existedUserDataFilterValue = true;
                    break;
                }
            }

            if (existedUserDataFilterValue)
            {
                return true;
            }


            return (dataFilterEntities != null && dataFilterEntities.Count > 0 && userNames.Length > 0);
        }
        
        #endregion

        #region Data Filter Service

        /// <summary>
        /// Gets the data filter template data.
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = Constants.FunctionNames.UserModuleID,
FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public List<DataFilterEntity> GetDataFilterTemplateData(string roleName, ref List<string> fullDataFilters, ref List<string> authorisedDataFilters, ref List<DataFilterEntity> dataFiltersForUser)
        {
            return this.DAInstance.GetDataFilterTemplateData(roleName, ref fullDataFilters, ref authorisedDataFilters, ref dataFiltersForUser);
        }

        /// <summary>
        /// Gets the data filters for user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <param name="ignoreSameFilterValue"></param>
        /// <returns>Available data filters for the specific user.</returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = Constants.FunctionNames.UserModuleID,
FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public List<DataFilterEntity> GetDataFiltersForUser(string userName, string roleName, bool ignoreSameFilterValue)
        {
            return this.DAInstance.GetDataFiltersForUser(userName, roleName, ignoreSameFilterValue);
        }

        #endregion

        #region Role Maintenance

        /// <summary>
        ///  Define a new role
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
           ModuleID = Constants.FunctionNames.RoleModuleID,
           FunctionID = Constants.FunctionNames.NewRoleFunctionID)]
        public void ExtendedCreateRole(string roleName, string roleDescription, string[] actions)
        {
            this.DAInstance.CreateRole(roleName, roleDescription);
            this.DAInstance.AddActionsInRole(roleName, actions);
        }

         /// <summary>
        /// Update a existed role
        /// </summary>
        /// <param name="oldRoleName">oldRoleName</param>
        /// <param name="roleDescription">roleDescription</param>
        /// <param name="status">status</param>
        /// <param name="actions">actions</param>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.RoleModuleID,
            FunctionID = Constants.FunctionNames.UpdateRoleFunctionID)]
        public void ExtendedUpdateRole(string oldRoleName, string roleDescription, string status, string[] actions)
        {
            this.DAInstance.UpdateRole(oldRoleName, roleDescription, status);
            this.DAInstance.UpdateActionsInRole(oldRoleName, actions);
        }

        /// <summary>
        ///  Delete a role and its functions 
        /// </summary>
        /// <param name="roleName">roleName</param>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.RoleModuleID,
            FunctionID = Constants.FunctionNames.UpdateRoleFunctionID)]
        public void DeleteRole(string roleName)
        {
            this.DAInstance.DeleteRole(roleName);
        }

        /// <summary>
        ///  Get role information by rolename
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>RoleEntity</returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.RoleModuleID,
            FunctionID = Constants.FunctionNames.ViewRoleFunctionID)]
        public RoleEntity GetRoleByRoleName(string roleName)
        {
            return this.DAInstance.GetRoleByRoleName(roleName);
        }

        /// <summary>
        ///  Search roles by rolename and description
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <param name="description">description</param>
        /// <returns>RoleEntity[]</returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
           ModuleID = Constants.FunctionNames.RoleModuleID,
           FunctionID = Constants.FunctionNames.SearchRoleFunctionID)]
        public RoleEntity[] FindRolesByConditions(string roleName, string description)
        {
            return this.DAInstance.FindRolesByConditions(roleName, description);
        }

        /// <summary>
        ///  Search user in role by rolename
        /// </summary>
        /// <param name="roleName">rolename</param>
        /// <returns>UserInfoEntity[]</returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
           ModuleID = Constants.FunctionNames.RoleModuleID,
           FunctionID = Constants.FunctionNames.ListOfUserInRoleFunctionID)]
        public UserInfoEntity[] GetUserInRoleByRoleName(string roleName)
        {
            return this.DAInstance.GetUserInRoleByRoleName(roleName);
        }

        /// <summary>
        ///  Search user in role by rolename
        /// </summary>
        /// <param name="roleName">rolename</param>
        /// <returns>UserInfoEntity[]</returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
           ModuleID = Constants.FunctionNames.RoleModuleID,
           FunctionID = Constants.FunctionNames.ListOfFunctionAndDataFilterFunctionID)]
        public FunctionAndDataFilterEntity[] GetActionCodeAndDFListByRoleName(string roleName)
        {
            return this.DAInstance.GetActionCodeAndDFListByRoleName(roleName);
        }

        /// <summary>
        /// Get all actions
        /// </summary>
        /// <returns>all actions</returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
           ModuleID = Constants.FunctionNames.RoleModuleID,
           FunctionID = Constants.FunctionNames.ViewRoleFunctionID)]
        public string[] GetAllActions()
        {
            return this.DAInstance.GetAllActions();
        }

        /// <summary>
        ///  Get actions in role
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>Actions in role</returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.RoleModuleID,
            FunctionID = Constants.FunctionNames.ViewRoleFunctionID)]
        public string[] GetActionsInRole(string roleName)
        {
            return this.DAInstance.GetActionsInRole(roleName);
        }

        /// <summary>
        ///  Role exists ?
        /// </summary>
        /// <param name="roleName">rolename</param>
        /// <returns>true ? false</returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
            ModuleID = Constants.FunctionNames.RoleModuleID,
            FunctionID = Constants.FunctionNames.UpdateRoleFunctionID)]
        public bool RoleExists(string roleName)
        {
            return this.DAInstance.RoleExists(roleName);
        }

        #endregion

        #region "DMS User Management"

        private void CreateDMSUser(UserInfoEntity userInfoEntity, string[] profileName)
        {

            if (profileName != null)
            {
                if (this.DMSService.IsUserExist(userInfoEntity.UserName))
                {
                    foreach (string name in profileName)
                    {
                        this.UpdateProfileOfUser(userInfoEntity.UserName, name);
                    }
                }
                else
                {
                    foreach (string name in profileName)
                    {

                        TrimUserEntity _trimUserEntity = new TrimUserEntity();
                        _trimUserEntity.FirstName = userInfoEntity.FirstName;
                        _trimUserEntity.LastName = userInfoEntity.LastName;
                        _trimUserEntity.DateOfBirth = userInfoEntity.DateOfBirth;
                        _trimUserEntity.EmailAddress = userInfoEntity.Email;
                        _trimUserEntity.FaxNo = userInfoEntity.FaxNo;
                        _trimUserEntity.IdNumber = userInfoEntity.UserName;
                        _trimUserEntity.Title = userInfoEntity.Title;
                        _trimUserEntity.Profile = name;
                        DMSService.CreateTrimUser(_trimUserEntity);
                    }
                }
            }

        }


        private void UpdateProfileOfUser(string userName, string profile)
        {
            DMSService.UpdateProfileOfuser(userName, profile);
        }


        #endregion

        #region "WF user managerment"

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = Constants.FunctionNames.UserModuleID,
        FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public DataSetETRoles RetrieveAllWFGroups()
        {
            DataSetETRoles ETRoles = new DataSetETRoles();
            GroupQueueDataSet groupQueueDS = WorkflowService.RetrieveAllGroups();
            DataSetETRoles.ETTableRow newRow;


            foreach (GroupQueueDataSet.WF_GroupQueueRow dr in groupQueueDS.WF_GroupQueue)
            {
                newRow = ETRoles.ETTable.NewETTableRow();
                newRow.BeginEdit();
                newRow.RoleName = dr.GroupName;
                newRow.Description = dr.GroupDesc;
                newRow.RoleType = "G";
                //newRow.UserName = groupNameList.Contains(dr.GroupName) ? username : string.Empty;
                newRow.EndEdit();
                ETRoles.ETTable.AddETTableRow(newRow);
            }
            return ETRoles;
        }

        /// <summary>
        /// Retrivets all the workflow users except internal users and the current user.
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = Constants.FunctionNames.UserModuleID,
        FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public DataSetETRoles RetrieveAllWFUsers()
        {
            DataSetETRoles ETRoles = new DataSetETRoles();
            List<UserEntity> UE = WorkflowService.GetAllUsers();
            DataSetETRoles.ETTableRow newRow;

            foreach (UserEntity _ue in UE)
            {
                newRow = ETRoles.ETTable.NewETTableRow();
                newRow.BeginEdit();
                newRow.RoleName = _ue.UserName;
                newRow.Description = _ue.Description;
                newRow.RoleType = "U";
                //newRow.UserName = groupNameList.Contains(dr.GroupName) ? username : string.Empty;
                newRow.EndEdit();
                ETRoles.ETTable.AddETTableRow(newRow);
            }
            return ETRoles;
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = Constants.FunctionNames.UserModuleID,
        FunctionID = Constants.FunctionNames.UpdateUserFunctionID)]
        public void UpdateParticipation(UserInfoEntity userEntity, string[] oldParticipation, string[] newParticipation, string[] supervisors)
        {
            this.WorkflowService.UpdateParticipation(userEntity.UserName, oldParticipation, newParticipation, supervisors);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = Constants.FunctionNames.UserModuleID,
        FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public List<UserEntity> GetParticipation(string userName)
        {
            return WorkflowService.GetParticipation(userName);
        }


        /// <summary>
        /// Creates the user in Iprocess
        /// </summary>
        /// <param name="userInfoEntity"></param>
        /// <param name="isAdministrator"></param>
        /// <param name="superviseGroupNames"></param> 
        /// <param name="groupName"></param> 
        private void CreateWFUser(UserInfoEntity userInfoEntity, bool isAdministrator, string[] superviseGroupNames, string[] groupName)
        {
            UserEntity userE = new UserEntity();
            userE.UserName = userInfoEntity.UserName;
            userE.Description = userInfoEntity.FirstName + " " + userInfoEntity.LastName;
            userE.UserFlags = IprocessServer.UserAttribute.ForwardAnyFlags;
            userE.OfficeID = userInfoEntity.Office;
            userE.GroupNames = groupName;
            userE.Email = userInfoEntity.Email;
            userE.RoleName = isAdministrator ? IprocessServer.WorkflowRole.ADMIN : IprocessServer.WorkflowRole.USER;
            userE.SupervisedGroupNames = superviseGroupNames;
            WorkflowService.CreateUser(userE);
        }

      
        private void SetWFUserRole(string userName, IprocessServer.WorkflowRole role)
        {
            WorkflowService.ChangeUserRole(userName, role);
        }

        private void UpdateUserAttributes(UserInfoEntity userInfoEntity, bool isAdministrator, string[] superviseGroupNames, string[] groupName)
        {

            UserEntity userE = WorkflowService.GetWFUser(userInfoEntity.UserName);
            if (userE != null)
            {
                userE.UserName = userInfoEntity.UserName;
                userE.Description = userInfoEntity.FirstName + " " + userInfoEntity.LastName;
                userE.UserFlags = IprocessServer.UserAttribute.ForwardAnyFlags;
                userE.OfficeID = userInfoEntity.Office;
                userE.GroupNames = groupName;
                userE.RoleName = isAdministrator ? IprocessServer.WorkflowRole.ADMIN : IprocessServer.WorkflowRole.USER;
                userE.SupervisedGroupNames = superviseGroupNames;
                WorkflowService.ChangeUserAttributes(userE);


            }
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = Constants.FunctionNames.UserModuleID,
       FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public UserEntity GetWFUser(string userName)
        {
            return WorkflowService.GetWFUser(userName);


        }


        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = Constants.FunctionNames.UserModuleID,
   FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public DataSetETRoles GetWFSupervisedUsersForGivenUser(string userName)
        {
            DataSetETRoles ETRoles = new DataSetETRoles();
            List<UserEntity> UE = WorkflowService.GetParticipation(userName);

            DataSetETRoles.ETTableRow newRow;

            foreach (UserEntity _ue in UE)
            {
                string[] SP = WorkflowService.GetWFSupervisedUsersForGivenUser(_ue.UserName);

                newRow = ETRoles.ETTable.NewETTableRow();
                newRow.BeginEdit();
                newRow.RoleName = _ue.UserName;
                newRow.Description = _ue.Description;
                newRow.RoleType = "U";
                newRow.IsSupervisor = SP.ToString().Contains(userName);
                newRow.EndEdit();
                ETRoles.ETTable.AddETTableRow(newRow);
            }
            return ETRoles;
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = Constants.FunctionNames.UserModuleID,
     FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public List<UserEntity> GetAllUsers()
        {
            return WorkflowService.GetAllUsers();
        }


        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = Constants.FunctionNames.UserModuleID,
   FunctionID = Constants.FunctionNames.ActivateOrDisableUsersFunctionID)]
        public bool DeletionCriteriaCheck(string userName, out string message)
        {
            return WorkflowService.DeletionCriteriaCheck(userName, out message);
        }


        ///// <summary>
        ///// add user to group
        ///// </summary>
        ///// <param name="username"></param>
        ///// <param name="groupNames"></param>
        //public void AddUserToGroups(string username, string[] groupNames)
        //{
        //    _workflowService.AddUserToGroups(username, groupNames);
        //}

        ///// <summary>
        ///// get wf user
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //public UserEntity GetWFUser(string username)
        //{
        //    return _workflowService.GetWFUser(username);
        //}
        #endregion

        #region Role Assignment

        //public void AddUserToRoles(string username, string[] roles)
        //{
        //    this.DAInstance.AddUserToRoles(username, roles);
        //}

        //     [MonitoringCallHandler(ComponentType.BusinessService, Ordinal = 3, 
        //         ModuleID = Constants.FunctionNames,
        //FunctionID = FunctionNames.AssignRolesToUsersFunctionID)]
        //     public void UpdateRolesForUser(string username, string[] roles)
        //     {
        //         this.DAInstance.UpdateRolesForUser(username, roles);
        //     }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 3,
            ModuleID = Constants.FunctionNames.RoleModuleID,
           FunctionID = Constants.FunctionNames.AssignRolesToUsersFunctionID)]
        [AuditLogCallHandler(HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionID, Ordinal = 4)]
        public void UpdateRolesForUsers(string[] userNames, string[] roles)
        {
            this.DAInstance.UpdateRolesForUsers(userNames, roles);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1,
           ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
       FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ViewUserFunctionID)]
        public RoleEntity[] GetRolesByUserName(string userName)
        {
            return this.DAInstance.GetRolesByUserName(userName);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 3,
            ModuleID = Constants.FunctionNames.RoleModuleID,
           FunctionID = Constants.FunctionNames.UpdateRoleFunctionID)]
        public string[] GetUsersByRoleName(string roleName, string userStatus)
        {
            return this.DAInstance.GetUsersByRoleName(roleName, userStatus);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 3,
            ModuleID = Constants.FunctionNames.UserModuleID,
           FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public GroupQueueDataSet RetrieveGroupsByUser(string userName)
        {
            return WorkflowService.RetrieveGroupsByUser(userName);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 3,
             ModuleID = Constants.FunctionNames.UserModuleID,
            FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public Dictionary<int, ParticipationEntity> GetAllParticipationsByQName(string queueName)
        {
            return WorkflowService.GetAllParticipationsByQName(queueName);
        }
        #endregion

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 3,
            ModuleID = Constants.FunctionNames.UserModuleID,
           FunctionID = Constants.FunctionNames.SearchUserFunctionID)]
        public string GetEPName(int EPRIN)
        {
            IExternalPartyManagementService EPService = InstanceBuilder.CreateInstance<IExternalPartyManagementService>();
            int[] eprins = { EPRIN };
            ExternalPartyContactInfoDataSet ds = EPService.GetEPInformation(true, eprins);
            return ds.EPCore[0].EPName;

        }

        #region GIS Role Assignment

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = Constants.FunctionNames.UserModuleID,
     FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public string[] GetAllGISRoles()
        {
            using (HiiP.Framework.Security.UserManagement.Interface.HiiPGISRoles.HiiPRoleServiceClient GISRole = new HiiP.Framework.Security.UserManagement.Interface.HiiPGISRoles.HiiPRoleServiceClient())
            {
                return GISRole.GetRoleNames();
            }
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = Constants.FunctionNames.UserModuleID,
    FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public List<ETRoleEntity> GetETRolesForUser(string userName)
        {
            return this.DAInstance.GetETRolesForUser(userName);
        }

        [TransactionEnlistCallHandler(Ordinal = 1)]
        [AuditLogCallHandler(Constants.FunctionNames.NewUserFunctionID, Ordinal = 2)]
        public void CreateUsersETRole(List<ETRoleEntity> etRoleEntity)
        {
            foreach (ETRoleEntity ET in etRoleEntity)
            {
                this.DAInstance.CreateUsersETRole(ET);
            }
        }

        [TransactionEnlistCallHandler(Ordinal = 1)]
        [AuditLogCallHandler(Constants.FunctionNames.UpdateUserFunctionID, Ordinal = 2)]
        public void DeleteUsersETRole(List<ETRoleEntity> etRoleEntity)
        {
            foreach (ETRoleEntity ET in etRoleEntity)
            {
                this.DAInstance.DeleteUsersETRole(ET);
            }
        }

        #endregion

        #region office
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = Constants.FunctionNames.UserModuleID,
      FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public OfficesHierarchyDataSet GetAllOfficesHierarchy(string userName)
        {
            return this.DAInstance.GetOfficesAllHierarchy(userName);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = Constants.FunctionNames.UserModuleID,
      FunctionID = Constants.FunctionNames.ViewUserFunctionID)]
        public OfficesHierarchyDataSet GetOfficesAllHierarchy(string userName)
        {
            return this.DAInstance.GetOfficesAllHierarchy(userName);
        }

        #endregion
                
  
    }

    public static class Extension
    {
        public static IEnumerable<T> Descendants<T>(this IEnumerable<T> source,
                                        Func<T, IEnumerable<T>> DescendBy)
        {
            foreach (T value in source)
            {
                yield return value;

                foreach (T child in DescendBy(value).Descendants<T>(DescendBy))
                {
                    yield return child;
                }
            }
        }


    }
}

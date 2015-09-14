#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Service proxy
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System.Collections.Generic;
using System.Data;
using HiiP.Foundation.Workflow.Interface.BusinessEntities;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface;
using HiiP.Framework.Security.UserManagement.Interface.Constants;

namespace HiiP.Framework.Security.UserManagement.ServiceProxies
{
    public class UserMaintenanceServiceProxy : ServiceProxyBase<IUserMaintenanceService>, IUserMaintenanceService
    {
        //private DMSService _dmsSvc = new DMSService();
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMaintenanceServiceProxy"/> class.
        /// </summary>
        /// <param name="endpointName">Name of the endpoint.</param>
        protected UserMaintenanceServiceProxy(string endpointName)
            : base(endpointName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMaintenanceServiceProxy"/> class.
        /// </summary>
        public UserMaintenanceServiceProxy()
        {
            WrapObject(new UserMaintenanceServiceProxy(EndpointNames.UserMaintenanceServiceEndpoint));
        }


        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.NewUserFunctionID)]
        public Dictionary<string, string> CreateUserInfo(UserInfoEntity userInfoEntity, List<DataFilterEntity> dataFilterEntities, string[] roles, bool isAdministrator, string[] superviseGroup, string[] assignedGroups, string[] profiles, List<ETRoleEntity> etRoleEntity, OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            return Proxy.CreateUserInfo(userInfoEntity, dataFilterEntities, roles, isAdministrator, superviseGroup, assignedGroups, profiles, etRoleEntity, removedOfficeList, newOfficeList);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.UpdateUserFunctionID)]
        public Dictionary<string, string> UpdateUserInfo(UserInfoEntity userInfoEntity, List<DataFilterEntity> dataFilterEntities, string[] roles, bool isAdministrator, string[] superviseGroup, string[] assignedGroups, string[] oldParticipation, string[] newParticipation, string[] profiles, bool wfHasChanged, bool dmsHasChanged, List<ETRoleEntity> gisRoles, List<ETRoleEntity> deletedGISRoles, OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            return Proxy.UpdateUserInfo(userInfoEntity, dataFilterEntities, roles, isAdministrator, superviseGroup, assignedGroups, oldParticipation, newParticipation, profiles, wfHasChanged, dmsHasChanged, gisRoles, deletedGISRoles, removedOfficeList, newOfficeList);
        }


        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID, FunctionID = FunctionNames.UpdateUserFunctionID)]
        public void UpdateParticipation(UserInfoEntity userEntity, string[] oldParticipation, string[] newParticipation, string[] superviseUsers)
        {
            Proxy.UpdateParticipation(userEntity, oldParticipation, newParticipation, superviseUsers);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.SearchUserFunctionID)]
        public UserInfoEntity GetUser(string userName)
        {
            return Proxy.GetUser(userName);
        }

        /// <summary>
        /// search users by conditions
        /// </summary>
        /// <param name="userInfoSearchCriteria">userInfoSearchCriteria</param>
        /// <returns>UserInfoEntity[]</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.SearchUserFunctionID)]
        public UserInfoEntity[] FindUsers(UserInfoSearchCriteria userInfoSearchCriteria)
        {
            UserInfoEntity[] userInfoEntitys = Proxy.FindUsers(userInfoSearchCriteria);
            return userInfoEntitys;
        }

        /// <summary>
        /// update users's status 
        /// </summary>
        /// <param name="userNames">usernames</param>
        /// <param name="userStatus">userstatus</param>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.UpdateUserFunctionID)]
        public void UpdateUserStatus(List<string> userNames, string userStatus)
        {
            Proxy.UpdateUserStatus(userNames, userStatus);
        }

        /// <summary>
        /// check user name is unique
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns>true ? false</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.UpdateUserFunctionID)]
        public bool UserExists(string userName)
        {
            return Proxy.UserExists(userName);
        }


        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="allUsers">All users.</param>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.UserModuleID, FunctionID = FunctionNames.ViewUserFunctionID)]
        public UserInfoEntity[] GetUsers(UserInfoEntity[] allUsers, string[] users)
        {
            return Proxy.GetUsers(allUsers, users);
        }

        /// <summary>
        /// Assign users data filter values
        /// </summary>
        /// <param name="userNames">usernames</param>
        /// <param name="dataFilterEntities">dataFilterEntities</param>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.UpdateUserFunctionID)]
        public void AssignUsersDataFilterValues(string[] userNames, List<DataFilterEntity> dataFilterEntities)
        {
            Proxy.AssignUsersDataFilterValues(userNames, dataFilterEntities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.ETRolesFunctionID,
          FunctionID = FunctionNames.ETRolesFunctionID)]
        public GroupQueueDataSet RetrieveGroupsByUser(string userName)
        {
            return Proxy.RetrieveGroupsByUser(userName);
        }

        #region Data Filter Service

        /// <summary>
        ///  Get user function and data filter by rolename
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>FunctionAndDataFilterEntity</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
           FunctionID = FunctionNames.ListOfFunctionAndDataFilterFunctionID)]
        public FunctionAndDataFilterEntity[] GetActionCodeAndDFListByRoleName(string roleName)
        {
            return Proxy.GetActionCodeAndDFListByRoleName(roleName);
        }
        /// <summary>
        /// Gets the data filter template data.
        /// </summary>
        /// <param name="roleName">The rolename.</param>
        /// <param name="fullDataFilters">All data filters.</param>
        /// <param name="authorisedDataFilters">The authorised data filters.</param>
        /// <param name="dataFiltersForUser">The data filters for user.</param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
FunctionID = FunctionNames.ViewUserFunctionID)]
        public List<DataFilterEntity> GetDataFilterTemplateData(string roleName, ref List<string> fullDataFilters, ref List<string> authorisedDataFilters, ref List<DataFilterEntity> dataFiltersForUser)
        {
            return Proxy.GetDataFilterTemplateData(roleName, ref fullDataFilters, ref authorisedDataFilters, ref dataFiltersForUser);
        }

        /// <summary>
        /// Gets the data filters for user.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="roleName">The rolename.</param>
        /// <param name="ignoreSameFilterValue">if set to <c>true</c> [ignore same filter value].</param>
        /// <returns>Available data filters for the specific user.</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
FunctionID = FunctionNames.ViewUserFunctionID)]
        public List<DataFilterEntity> GetDataFiltersForUser(string userName, string roleName, bool ignoreSameFilterValue)
        {
            return Proxy.GetDataFiltersForUser(userName, roleName, ignoreSameFilterValue);
        }

        #endregion

        #region Role Maintenance

        /// <summary>
        ///  Define a new role
        /// </summary>
        //[MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
        //    FunctionID = FunctionNames.NewRoleFunctionID)]
        //public void CreateRole(string roleName, string roleDescription)
        //{
        //    Proxy.CreateRole(roleName, roleDescription);
        //}

        /// <summary>
        ///  Define a new role
        /// </summary>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
           FunctionID = FunctionNames.NewRoleFunctionID)]
        public void ExtendedCreateRole(string roleName, string roleDescription, string[] actions)
        {
            Proxy.ExtendedCreateRole(roleName, roleDescription, actions);
        }

        /// <summary>
        ///  Update a existed role
        /// </summary>
        /// <param name="oldRoleName">oldRoleName</param>
        /// <param name="roleDescription">roleDescription</param>
        /// <param name="status">status</param>
        /// <param name="actions">actions</param>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
           FunctionID = FunctionNames.UpdateRoleFunctionID)]
        public void ExtendedUpdateRole(string oldRoleName, string roleDescription, string status, string[] actions)
        {
            Proxy.ExtendedUpdateRole(oldRoleName, roleDescription, status, actions);
        }

        /// <summary>
        ///  Delete a role and its functions 
        /// </summary>
        /// <param name="roleName">roleName</param>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
           FunctionID = FunctionNames.UpdateRoleFunctionID)]
        public void DeleteRole(string roleName)
        {
            Proxy.DeleteRole(roleName);
        }

        /// <summary>
        ///  Get role information by rolename
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>RoleEntity</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
           FunctionID = FunctionNames.ViewRoleFunctionID)]
        public RoleEntity GetRoleByRoleName(string roleName)
        {
            return Proxy.GetRoleByRoleName(roleName);
        }

        /// <summary>
        ///  Get user information by rolename
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>UserEntity</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
           FunctionID = FunctionNames.ViewRoleFunctionID)]
        public UserInfoEntity[] GetUserInRoleByRoleName(string roleName)
        {
            return Proxy.GetUserInRoleByRoleName(roleName);
        }

        /// <summary>
        ///  Search roles by rolename and description
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <param name="description">description</param>
        /// <returns>RoleEntity[]</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
           FunctionID = FunctionNames.SearchRoleFunctionID)]
        public RoleEntity[] FindRolesByConditions(string roleName, string description)
        {
            return Proxy.FindRolesByConditions(roleName, description);
        }

        /// <summary>
        /// Get all actions
        /// </summary>
        /// <returns>all actions</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
          FunctionID = FunctionNames.ViewRoleFunctionID)]
        public string[] GetAllActions()
        {
            return Proxy.GetAllActions();
        }

        /// <summary>
        ///  Get actions in role
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>Actions in role</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
          FunctionID = FunctionNames.ViewRoleFunctionID)]
        public string[] GetActionsInRole(string roleName)
        {
            return Proxy.GetActionsInRole(roleName);
        }

        /// <summary>
        ///  Role exists ?
        /// </summary>
        /// <param name="roleName">rolename</param>
        /// <returns>true ? false</returns>
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
          FunctionID = FunctionNames.ViewRoleFunctionID)]
        public bool RoleExists(string roleName)
        {
            return Proxy.RoleExists(roleName);
        }

        #endregion

        #region Role Assignment

        //[MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.SecurityModuleID,
        //   FunctionID = FunctionNames.AssignRolesToUsersFunctionID)]
        //public void AddUserToRoles(string username, string[] roles)
        //{
        //    Proxy.AddUserToRoles(username, roles);
        //}

        //[MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.SecurityModuleID,
        //   FunctionID = FunctionNames.AssignRolesToUsersFunctionID)]
        //public void UpdateRolesForUser(string username, string[] roles)
        //{
        //    Proxy.UpdateRolesForUser(username, roles);
        //}

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.SecurityModuleID,
           FunctionID = FunctionNames.AssignRolesToUsersFunctionID)]
        public void UpdateRolesForUsers(string[] userNames, string[] roles)
        {
            Proxy.UpdateRolesForUsers(userNames, roles);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.ViewUserFunctionID)]
        public RoleEntity[] GetRolesByUserName(string userName)
        {
            return Proxy.GetRolesByUserName(userName);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.UpdateRoleFunctionID)]
        public string[] GetUsersByRoleName(string roleName, string userStatus)
        {
            return Proxy.GetUsersByRoleName(roleName, userStatus);
        }

        #endregion


        #region "WF User managerment"

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
         FunctionID = FunctionNames.ViewUserFunctionID)]
        public UserEntity GetWFUser(string userName)
        {
            return Proxy.GetWFUser(userName);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
         FunctionID = FunctionNames.ViewUserFunctionID)]
        public DataSetETRoles RetrieveAllWFGroups()
        {
            return Proxy.RetrieveAllWFGroups();
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
        FunctionID = FunctionNames.ViewUserFunctionID)]
        public DataSetETRoles RetrieveAllWFUsers()
        {
            return Proxy.RetrieveAllWFUsers();
        }
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
       FunctionID = FunctionNames.ViewUserFunctionID)]
        public Dictionary<int, ParticipationEntity> GetAllParticipationsByQName(string queueName)
        {
            return Proxy.GetAllParticipationsByQName(queueName);
        }
        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 2, ModuleID = FunctionNames.UserModuleID,
        FunctionID = FunctionNames.ViewUserFunctionID)]
        public List<UserEntity> GetAllWFUsers()
        {
            return Proxy.GetAllWFUsers();
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 2, ModuleID = FunctionNames.UserModuleID,
   FunctionID = FunctionNames.ViewUserFunctionID)]
        public DataSetETRoles GetWFSupervisedUsersForGivenUser(string userName)
        {
            return Proxy.GetWFSupervisedUsersForGivenUser(userName);
        }


        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 2, ModuleID = FunctionNames.UserModuleID,
 FunctionID = FunctionNames.ViewUserFunctionID)]
        public List<UserEntity> GetParticipation(string userName)
        {
            return Proxy.GetParticipation(userName);
        }
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.ViewUserFunctionID,
             FunctionID = FunctionNames.ViewUserFunctionID)]
        public bool DeletionCriteriaCheck(string userName, out string message)
        {
            return Proxy.DeletionCriteriaCheck(userName, out message);
        }
        #endregion

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.ETRolesFunctionID,
               FunctionID = FunctionNames.ETRolesFunctionID)]
        public string[] GetAllGISRoles()
        {
            return Proxy.GetAllGISRoles();
        }
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.ETRolesFunctionID,
              FunctionID = FunctionNames.ETRolesFunctionID)]
        public List<ETRoleEntity> GetGISRolesForUser(string userName)
        {
            return Proxy.GetGISRolesForUser(userName);
        }

		[MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.SearchUserFunctionID,
            FunctionID = FunctionNames.SearchUserFunctionID)]
        public string GetEPName(int EPRIN)
        {
            return Proxy.GetEPName(EPRIN);
        }

       

        #region Office

        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.UserModuleID,
              FunctionID = FunctionNames.ViewUserFunctionID)]
        public OfficesHierarchyDataSet GetOfficesAllHierarchy(string userName)
        {
            return Proxy.GetOfficesAllHierarchy(userName);
        }
        #endregion


        #region IUserMaintenanceService Members
        [MonitoringCallHandler(ComponentType.ServiceProxy, ModuleID = FunctionNames.RoleModuleID,
      FunctionID = FunctionNames.ViewRoleFunctionID)]
        public string GetMinAuthorisationRoleFunctionID()
        {
            return Proxy.GetMinAuthorisationRoleFunctionID();
        }

        #endregion


    }
}
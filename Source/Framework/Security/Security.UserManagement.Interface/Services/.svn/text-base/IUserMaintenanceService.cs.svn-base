#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Interface
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System.Collections.Generic;
using System.ServiceModel;

using HiiP.Foundation.Workflow.Interface.BusinessEntities;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

using NCS.IConnect.Common;

namespace HiiP.Framework.Security.UserManagement.Interface
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface IUserMaintenanceService
    {
        /// <summary>
        /// Gets the min authorisation role function ID.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        string GetMinAuthorisationRoleFunctionID();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfoEntity"></param>
        /// <param name="dataFilterEntities"></param>
        /// <param name="roles"></param>
        /// <param name="isAdministrator"></param>
        /// <param name="superviseGroup"></param>
        /// <param name="assignedGroups"></param>
        /// <param name="profiles"></param>
        /// <param name="etRoleEntity"></param>
        /// <param name="removedOfficeList"></param>
        /// <param name="newOfficeList"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, string> CreateUserInfo([ObjectValidator("User Info Operation Rule Set")]UserInfoEntity userInfoEntity, List<DataFilterEntity> dataFilterEntities, string[] roles, bool isAdministrator, string[] superviseGroup, string[] assignedGroups, string[] profiles, List<ETRoleEntity> etRoleEntity, OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList);

        /// <summary>
        /// Accroding to username, updating user account and information
        /// </summary>
        /// <param name="userInfoEntity"></param>
        /// <param name="dataFilterEntities"></param>
        /// <param name="roles"></param>
        /// <param name="isAdministrator"></param>
        /// <param name="superviseGroup"></param>
        /// <param name="assignedGroups"></param>
        /// <param name="oldParticipation"></param>
        /// <param name="newParticipation"></param>
        /// <param name="profiles"></param>
        /// <param name="wfHasChanged"></param>
        /// <param name="dmsHasChanged"></param>
        /// <param name="gisRoles"></param>
        /// <param name="deletedGISRoles"></param>
        /// <param name="removedOfficeList"></param>
        /// <param name="newOfficeList"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, string> UpdateUserInfo(
            [ObjectValidator("User Info Operation Rule Set")] 
            UserInfoEntity userInfoEntity, List<DataFilterEntity> dataFilterEntities, string[] roles, bool isAdministrator, string[] superviseGroup, string[] assignedGroups, string[] oldParticipation, string[] newParticipation, string[] profiles, bool wfHasChanged, bool dmsHasChanged, List<ETRoleEntity> gisRoles, List<ETRoleEntity> deletedGISRoles, OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList);



        /// <summary>
        /// Get user information by username
        /// </summary>
        /// <param name="userName">userName</param>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        UserInfoEntity GetUser(string userName);


        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        UserInfoEntity[] FindUsers(UserInfoSearchCriteria userInfoSearchCriteria);

        /// <summary>
        /// update users's status 
        /// </summary>
        /// <param name="userNames">usernames</param>
        /// <param name="userStatus">userstatus</param>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void UpdateUserStatus(List<string> userNames, string userStatus);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        bool UserExists(string userName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        UserInfoEntity[] GetUsers(UserInfoEntity[] allUsers, string[] users);


        /// <summary>
        /// Assign users data filter values
        /// </summary>
        /// <param name="userNames">usernames</param>
        /// <param name="dataFilterEntities">dataFilterEntities</param>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void AssignUsersDataFilterValues(string[] userNames, List<DataFilterEntity> dataFilterEntities);

        #region Data Filter Service

        /// <summary>
        /// Gets the data filter template data.
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="fullDataFilters"></param>
        /// <param name="authorisedDataFilters"></param>
        /// <param name="dataFiltersForUser"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<DataFilterEntity> GetDataFilterTemplateData(string roleName, ref List<string> fullDataFilters, ref List<string> authorisedDataFilters, ref List<DataFilterEntity> dataFiltersForUser);

        /// <summary>
        /// Gets the data filters for user.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="roleName">The rolename.</param>
        /// <param name="ignoreSameFilterValue">if set to <c>true</c> [ignore same filter value].</param>
        /// <returns>Available data filters for the specific user.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<DataFilterEntity> GetDataFiltersForUser(string userName, string roleName, bool ignoreSameFilterValue);

        #endregion

        #region Role Maintenance

        /// <summary>
        /// Define a new role
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void ExtendedCreateRole(
            [StringLengthValidator(1, 128, MessageTemplate = "The Role Name is mandatory, and the length must be between 1 and 128.")] 
            [RegexValidator(@"^[A-Za-z0-9_. -/\(\)&]+$", MessageTemplate = @"The Role Name is invalid.(Only permitting 'A-Za-z0-9_.-/()&')")]
            string roleName, string roleDescription, string[] actions);

        /// <summary>
        /// Define a new role
        /// </summary>
        //[OperationContract]
        //[FaultContract(typeof(ValidationFault))]
        //[FaultContract(typeof(ServiceException))]
        //void CreateRole(
        //    [StringLengthValidator(1, 128, MessageTemplate = "The Role Name is mandatory, and the length must be between 1 and 128.")] 
        //    [RegexValidator("^[A-Za-z0-9_. -]+$", MessageTemplate = "The Role Name is invalid.(Only permitting 'A-Za-z0-9_.-')")]
        //    string roleName, string roleDescription);

        /// <summary>
        /// Update a existed role
        /// </summary>
        /// <param name="oldRoleName">oldRoleName</param>
        /// <param name="roleDescription">roleDescription</param>
        /// <param name="status">status</param>
        /// <param name="actions">actions</param>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void ExtendedUpdateRole(
            string oldRoleName,
            string roleDescription, string status, string[] actions);

        /// <summary>
        /// Delete a role and its functions 
        /// </summary>
        /// <param name="roleName">roleName</param>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void DeleteRole(string roleName);

        /// <summary>
        /// Get role information by rolename
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>RoleEntity</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        RoleEntity GetRoleByRoleName(string roleName);

        /// <summary>
        /// Search roles by rolename and description
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>RoleEntity[]</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        UserInfoEntity[] GetUserInRoleByRoleName(string roleName);

        /// <summary>
        /// Search function and data filter by rolename 
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>FunctionAndDataFilterEntity[]</returns>

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        FunctionAndDataFilterEntity[] GetActionCodeAndDFListByRoleName(string roleName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        RoleEntity[] FindRolesByConditions(string roleName, string description);

        /// <summary>
        /// Get all actions
        /// </summary>
        /// <returns>all actions</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        string[] GetAllActions();

        /// <summary>
        /// Get actions in role
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>Actions in role</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        string[] GetActionsInRole(string roleName);


        /// <summary>
        /// Role exists ?
        /// </summary>
        /// <param name="roleName">rolename</param>
        /// <returns>true ? false</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        bool RoleExists(string roleName);

        #endregion

        #region "WF user managerment"
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        DataSetETRoles RetrieveAllWFGroups();

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        DataSetETRoles RetrieveAllWFUsers();

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        Dictionary<int, ParticipationEntity> GetAllParticipationsByQName(string queueName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        bool DeletionCriteriaCheck(string userName, out string message);

        ///// <summary>
        ///// update user's groups(delete current groups then add new groups)
        ///// </summary>
        ///// <param name="username"></param>
        ///// <param name="groupNames"></param>
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //void UpdateUserGroups(string username, string[] groupNames);

        ///// <summary>
        ///// add user to group
        ///// </summary>
        ///// <param name="username"></param>
        ///// <param name="groupNames"></param>
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //void AddUserToGroups(string username, string[] groupNames);

        ///// <summary>
        ///// get wf user
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //UserEntity GetWFUser(string username);
        #endregion



        /// <summary>
        /// Retrieves a workflow user
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        UserEntity GetWFUser(string userName);

        /// <summary>
        /// Retrieves all workflow users
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<UserEntity> GetAllWFUsers();

        /// <summary>
        /// Get workflow supervised users for a given user
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        DataSetETRoles GetWFSupervisedUsersForGivenUser(string userName);

        /// <summary>
        /// Get workflow supervised users for a given user
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void UpdateParticipation(UserInfoEntity userEntity, string[] oldParticipation, string[] newParticipation, string[] superviseUsers);

        /// <summary>
        /// Retrieve Externalparty name
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        string GetEPName(int EPRIN);

        #region role

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //void AddUserToRoles(string username, string[] roles);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //void UpdateRolesForUser(string username, string[] roles);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void UpdateRolesForUsers(string[] userNames, string[] roles);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        RoleEntity[] GetRolesByUserName(string userName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        string[] GetUsersByRoleName(string roleName, string userStatus);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        GroupQueueDataSet RetrieveGroupsByUser(string userName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<UserEntity> GetParticipation(string userName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        string[] GetAllGISRoles();

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<ETRoleEntity> GetGISRolesForUser(string userName);



        #endregion

        #region Office

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        OfficesHierarchyDataSet GetOfficesAllHierarchy(string userName);
        #endregion


    }
}

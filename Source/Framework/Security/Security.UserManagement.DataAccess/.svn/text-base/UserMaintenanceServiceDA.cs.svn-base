#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Data access
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
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Web.Security;
using System.Xml.Linq;
using HiiP.Framework.Common;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Constants;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NCS.IConnect.ApplicationContexts;
using NCS.IConnect.Helpers.Data;
using NCS.IConnect.Security;

namespace HiiP.Framework.Security.UserManagement.DataAccess
{
    public class UserMaintenanceServiceDA : HiiPDataAccessBase
    {

        #region User Maintenance Service

        /// <summary>
        /// Create user information(user account, user basic information)
        /// </summary>
        /// <param name="userInfoEntity"></param>
        /// <param name="dataFilterEntities"></param>
        /// <param name="roles"></param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
   FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.NewUserFunctionID)]
        public void CreateUserInfo(UserInfoEntity userInfoEntity, List<DataFilterEntity> dataFilterEntities, string[] roles)
        {
            // ceate a new user account
            Membership.CreateUser(userInfoEntity.UserName, "Password0");

            // create user basic information by created username
            ProfileBase profileInfo = ProfileBase.Create(userInfoEntity.UserName);
            profileInfo["FirstName"] = userInfoEntity.FirstName;
            profileInfo["Initials"] = userInfoEntity.Initials;
            profileInfo["LastName"] = userInfoEntity.LastName;
            profileInfo["Display"] = userInfoEntity.Display;
            profileInfo["Alias"] = userInfoEntity.Alias;
            profileInfo["Gender"] = userInfoEntity.Gender;
            profileInfo["Title"] = userInfoEntity.Title;
            profileInfo["DateOfBirth"] = userInfoEntity.DateOfBirth;
            profileInfo["Email"] = userInfoEntity.Email;
            profileInfo["TelephoneNo"] = userInfoEntity.TelephoneNo;
            profileInfo["FaxNo"] = userInfoEntity.FaxNo;
            profileInfo["MobileNo"] = userInfoEntity.MobileNo;
            profileInfo["PageNo"] = userInfoEntity.PageNo;
            //profileInfo["Organisation"] = userInfoEntity.Organisation;
            profileInfo["Remarks"] = userInfoEntity.Remarks;

            profileInfo["ConfigurationInfo"] = new XElement(HiiP.Framework.Common.Constants.UserConfigInfoSectionNames.RootSectionName).ToString();

            profileInfo["Office"] = userInfoEntity.Office;

            // Extend "job title" property
            profileInfo["JobTitle"] = userInfoEntity.JobTitle;
            
            //profileInfo
            
            // Extend "Reports to" property
            profileInfo["ReportsTo"] = userInfoEntity.ReportsTo;
            profileInfo["EPRIN"] = userInfoEntity.EPRIN;
            profileInfo["IsInternal"] = userInfoEntity.IsInternal;


            //Delegations
            profileInfo["Branch"] = userInfoEntity.Branch;
            profileInfo["Unit"] = userInfoEntity.Unit;
            profileInfo["SubUnit"] = userInfoEntity.SubUnit;
            profileInfo["Grade"] = userInfoEntity.Grade;

            profileInfo["VersionNo"] = userInfoEntity.VersionNo + 1;

            profileInfo.Save();

            //// Mapping office to user
            //if (officeEntities != null && officeEntities.Count > 0)
            //{
            //    foreach (OfficeEntity officeEntity in officeEntities)
            //    {
            //        officeEntity.UserName = userInfoEntity.UserName;
            //    }
            //    MappingUserOffices(officeEntities);
            //}

            // Mapping data filters to user
            if (dataFilterEntities != null && dataFilterEntities.Count > 0)
            {
                foreach (DataFilterEntity dataFilterEntity in dataFilterEntities)
                {
                    dataFilterEntity.UserName = userInfoEntity.UserName;
                }
                MappingUserDataFilterValues(dataFilterEntities);
            }

            // Add roles to the user
            if (roles.Length > 0)
            {
                AddUserToRoles(userInfoEntity.UserName, roles);
            }
        }

        /// <summary>
        /// Accroding to username, updating user account and information
        /// </summary>
        /// <param name="userInfoEntity">userInfoEntity</param>
        /// <param name="dataFilterEntities"></param>
        /// <param name="roles"></param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
   FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionID)]
        public void UpdateUserInfo(UserInfoEntity userInfoEntity, List<DataFilterEntity> dataFilterEntities, string[] roles)
        {
            // update user basic information by username
            ProfileBase profileInfo = ProfileBase.Create(userInfoEntity.UserName);
            profileInfo["FirstName"] = userInfoEntity.FirstName;
            profileInfo["Initials"] = userInfoEntity.Initials;
            profileInfo["LastName"] = userInfoEntity.LastName;
            profileInfo["Display"] = userInfoEntity.Display;
            profileInfo["Alias"] = userInfoEntity.Alias;
            profileInfo["Gender"] = userInfoEntity.Gender;
            profileInfo["Title"] = userInfoEntity.Title;
            profileInfo["DateOfBirth"] = userInfoEntity.DateOfBirth;
            profileInfo["Email"] = userInfoEntity.Email;
            profileInfo["TelephoneNo"] = userInfoEntity.TelephoneNo;
            profileInfo["FaxNo"] = userInfoEntity.FaxNo;
            profileInfo["MobileNo"] = userInfoEntity.MobileNo;
            profileInfo["PageNo"] = userInfoEntity.PageNo;
            //profileInfo["Organisation"] = userInfoEntity.Organisation;
            profileInfo["Remarks"] = userInfoEntity.Remarks;
            //profileInfo["ConfigurationInfo"] = userInfoEntity.ConfigurationInfo.ToString();
            profileInfo["VersionNo"] = userInfoEntity.VersionNo + 1;

            profileInfo["Office"] = userInfoEntity.Office;

            // Extend "job title" property
            profileInfo["JobTitle"] = userInfoEntity.JobTitle;

            // Extend "Reports to" property
            profileInfo["ReportsTo"] = userInfoEntity.ReportsTo;
            profileInfo["EPRIN"] = userInfoEntity.EPRIN;
            profileInfo["IsInternal"] = userInfoEntity.IsInternal;

            //Delegations
            profileInfo["Branch"] = string.IsNullOrEmpty(userInfoEntity.Branch ) ? null : userInfoEntity.Branch;
            profileInfo["Unit"] = string.IsNullOrEmpty(userInfoEntity.Unit ) ? null : userInfoEntity.Unit;
            profileInfo["SubUnit"] = string.IsNullOrEmpty(userInfoEntity.SubUnit ) ? null : userInfoEntity.SubUnit;
            profileInfo["Grade"] = string.IsNullOrEmpty(userInfoEntity.Grade ) ? null : userInfoEntity.Grade;

            profileInfo.Save();

            //// Mapping office to user
            //if (officeEntities != null && officeEntities.Count > 0)
            //{
            //    foreach (OfficeEntity officeEntity in officeEntities)
            //    {
            //        officeEntity.UserName = userInfoEntity.UserName;
            //    }
            //    MappingUserOffices(officeEntities);
            //}

            // Mapping data filters to user
            if (dataFilterEntities != null && dataFilterEntities.Count > 0)
            {
                foreach (DataFilterEntity dataFilterEntity in dataFilterEntities)
                {
                    dataFilterEntity.UserName = userInfoEntity.UserName;
                }
                MappingUserDataFilterValues(dataFilterEntities);
            }

            // Update roles for the user
            if (roles != null)
            {
                UpdateRolesForUser(userInfoEntity.UserName, roles);
            }
        }

        /// <summary>
        /// Get user information by username
        /// </summary>
        /// <param name="userName">username</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SearchUserFunctionID)]
        public UserInfoEntity GetUser(string userName)
        {
            return InstanceBuilder.CreateInstance<UserCommonServiceDA>().GetUser(userName);
        }

        /// <summary>
        /// Search user by coditions
        /// </summary>
        /// <param name="userInfoSearchCriteria">userInfoSearchCriteria</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SearchUserFunctionID)]
        public UserInfoEntity[] FindUsers(UserInfoSearchCriteria userInfoSearchCriteria)
        {
            if (userInfoSearchCriteria == null) return new UserInfoEntity[] { };
            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();

            DbCommand command = helper.BuildDbCommand("P_IC_USER_INFO_SEARCH");

            var appName = NCS.IConnect.Security.ExtendedMembership.ApplicationName;
            helper.AssignParameterValues(
                command,
                SearchHelper.TranslateWildcard(userInfoSearchCriteria.UserName),
                SearchHelper.TranslateWildcard(userInfoSearchCriteria.UserStatus),
                userInfoSearchCriteria.CreatedFrom,
                userInfoSearchCriteria.CreatedTo,
                SearchHelper.TranslateWildcard(userInfoSearchCriteria.Display),
                SearchHelper.TranslateWildcard(userInfoSearchCriteria.Email),
                userInfoSearchCriteria.UserType,
                SearchHelper.TranslateWildcard(userInfoSearchCriteria.Office),
                appName
                );
            Helper.Fill(dt, command);

            Dictionary<string, UserInfoEntity> arrayList = new Dictionary<string, UserInfoEntity>();
            const string Yes = "Y";

            DataRow[] matchedRecords = dt.Select(string.Format("IS_MASTER='{0}'", Yes));


            //1. add the records that is the default office
            foreach (DataRow row in matchedRecords)
            {
                string userId = row["USER_ID"].ToString();
                //if (!(string.IsNullOrEmpty(userInfoSearchCriteria.Office)
                //    || SearchHelper.IsRegexMatch(row["DEFAULT_OFFICE"].ToString(), userInfoSearchCriteria.Office, @"\w|\W")))
                //{
                //    //Not match office criteria
                //    continue;
                //}
                if (arrayList.ContainsKey(userId))
                {
                    //Duplicated
                    continue;
                }
                arrayList.Add(userId, CreatEntity(row));
            }

            //2.Union the records that have no default office

            matchedRecords = dt.Select(string.Format("IS_MASTER<>'{0}'",Yes));

            foreach (DataRow row in matchedRecords)
            {
                string userId = row["USER_ID"].ToString();
                //if (!(string.IsNullOrEmpty(userInfoSearchCriteria.Office)
                //   || SearchHelper.IsRegexMatch(row[""].ToString(), userInfoSearchCriteria.Office, @"\w|\W")))
                //{
                //    //Not match office criteria
                //    continue;
                //}

                if (arrayList.ContainsKey(userId))
                {
                    //Duplicated or has default office records.
                    continue;
                }

                UserInfoEntity entity = CreatEntity(row);
entity.Office = string.Empty;
                arrayList.Add(userId, entity);
            }

            //Compine all offices for every user
            foreach (KeyValuePair<string,UserInfoEntity> user in arrayList)
            {
                string userId = user.Key;
                matchedRecords = dt.Select(string.Format("USER_ID='{0}'", userId));
                StringBuilder allOffices = new StringBuilder();

                foreach (var row in matchedRecords)
                {
                    allOffices.Append(row["DEFAULT_OFFICE"].ToString());
                    allOffices.Append(",");
                }
                user.Value.AllOffices = allOffices.ToString().TrimEnd(',');
            }
            dt.Dispose();
            return arrayList.Values.ToArray<UserInfoEntity>();
        }

        /// <summary>
        /// Search user in role by rolename
        /// </summary>
        /// <param name="roleName">roleName</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ListOfUserInRoleFunctionID)]
        public UserInfoEntity[] GetUserInRoleByRoleName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return new UserInfoEntity[] { };
            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_IC_USER_IN_ROLE_SEARCH");
            helper.AssignParameterValues(
                command,
                roleName
                );
            Helper.Fill(dt, command);

            Dictionary<string, UserInfoEntity> arrayList = new Dictionary<string, UserInfoEntity>();
            
            UserInfoEntity userInfo  ;

            foreach (DataRow row in dt.Rows)
            {
                userInfo = new UserInfoEntity();

                string userId = row["USER_ID"].ToString();
                userInfo.UserName = row["USER_NAME"].ToString();
                userInfo.FirstName = row["FIRST_NAME"].ToString();
                userInfo.Initials = row["INITIALS"].GetType() == typeof(System.DBNull) ? String.Empty : row["INITIALS"].ToString();
                userInfo.LastName = row["LAST_NAME"].ToString();
                userInfo.Display = row["DISPLAY"].ToString();
                userInfo.Alias = row["ALIAS"].GetType() == typeof(System.DBNull) ? String.Empty : row["ALIAS"].ToString();
                userInfo.Gender = row["GENDER"].ToString();
                userInfo.Title = row["TITLE"].ToString();
                userInfo.DateOfBirth = row["DATE_OF_BIRTH"].GetType() == typeof(System.DBNull) ? MinMaxValues.MinDate : Convert.ToDateTime(row["DATE_OF_BIRTH"]);
                userInfo.Email = row["EMAIL"].ToString();
                userInfo.TelephoneNo = row["TELEPHONE_NO"].ToString();
                userInfo.FaxNo = row["FAX_NO"].GetType() == typeof(System.DBNull) ? String.Empty : row["FAX_NO"].ToString();
                userInfo.MobileNo = row["MOBILE_NO"].GetType() == typeof(System.DBNull) ? String.Empty : row["MOBILE_NO"].ToString();
                userInfo.PageNo = row["PAGER_NO"].GetType() == typeof(System.DBNull) ? String.Empty : row["PAGER_NO"].ToString();
                //UIE.Organisation = row["ORGANISATION"].ToString();
                userInfo.Remarks = row["REMARKS"].GetType() == typeof(System.DBNull) ? String.Empty : row["REMARKS"].ToString();
                userInfo.UserStatus = row["STATUS"].ToString();
                userInfo.CreatedOn = (DateTime)row["CREATED_TIME"];
                userInfo.Office = row["OFFICE"].GetType() == typeof(DBNull) ? String.Empty : row["OFFICE"].ToString();
                arrayList.Add(userId, userInfo);
            }
            dt.Dispose();
            return arrayList.Values.ToArray<UserInfoEntity>();
        }
        private static UserInfoEntity CreatEntity(DataRow row)
        {
            return new UserInfoEntity(
                    row["USER_NAME"].ToString(),
                    row["FIRST_NAME"].ToString(),
                    row["INITIALS"]== System.DBNull.Value ? String.Empty : row["INITIALS"].ToString(),
                    row["LAST_NAME"].ToString(),
                    row["DISPLAY"].ToString(),
                    row["ALIAS"] == System.DBNull.Value ? String.Empty : row["ALIAS"].ToString(),
                    row["GENDER"].ToString(),
                    row["TITLE"].ToString(),
                    row["DATE_OF_BIRTH"] == System.DBNull.Value ? MinMaxValues.MinDate : Convert.ToDateTime(row["DATE_OF_BIRTH"]),
                    row["EMAIL"].ToString(),
                    row["TELEPHONE_NO"].ToString(),
                    row["FAX_NO"] == System.DBNull.Value ? String.Empty : row["FAX_NO"].ToString(),
                    row["MOBILE_NO"] == System.DBNull.Value ? String.Empty : row["MOBILE_NO"].ToString(),
                    row["PAGER_NO"] == System.DBNull.Value ? String.Empty : row["PAGER_NO"].ToString(),
                    //row["ORGANISATION"].ToString(),
                    row["REMARKS"] == System.DBNull.Value ? String.Empty : row["REMARKS"].ToString(),
                    row["STATUS"].ToString(),
                    Convert.ToDateTime(row["CREATED_TIME"]),
                    row["IS_MASTER"].ToString(),
                    row["DEFAULT_OFFICE"].ToString(),
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty
                    );
        }

        /// <summary>
        /// update users's status 
        /// </summary>
        /// <param name="userNames">usernames</param>
        /// <param name="userStatus">userstatus</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionID)]
        public void UpdateUserStatus(List<string> userNames, string userStatus)
        {
            foreach (string username in userNames)
            {
                DbCommand command = Helper.BuildDbCommand("P_IC_USER_INFO_SET_ACTIVEORINACTIVE");
                Helper.AssignParameterValues(
                    command,
                    Membership.ApplicationName,
                    userStatus,
                    username,
                    DateTime.Now,
                    NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId,
                    AppContext.Current.UserName
                   );
                Helper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// check user name is unique
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1,
            ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
   FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionID)]
        public bool UserExists(string userName)
        {
            // Recommend changing current implementation to check unique user name.
            // It's necessary beacuse of current issue
            MembershipUser user;
            return ExtendedMembership.TryGetUser(userName, false, out user);
        }

        /// <summary>
        /// Assign users data filter values
        /// </summary>
        /// <param name="userNames">usernames</param>
        /// <param name="dataFilterEntities">dataFilterEntities</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1,
           ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
  FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateUserFunctionID)]
        public void AssignUsersDataFilterValues(string[] userNames, List<DataFilterEntity> dataFilterEntities)
        {
            // Clear selected users data filter values
            this.ClearUsersDataFilterValues(userNames);

            // Assign users' data fiter values
            if (dataFilterEntities != null && dataFilterEntities.Count > 0)
            {
                List<DataFilterEntity> userDataFilterEntities = new List<DataFilterEntity>();
                foreach (string username in userNames)
                {
                    foreach (DataFilterEntity dataFilterEntity in dataFilterEntities)
                    {
                        DataFilterEntity dataFilter = new DataFilterEntity();
                        dataFilter.UserDataFilterValueID = Guid.NewGuid().ToString();
                        dataFilter.UserName = username;
                        dataFilter.RoleName = dataFilterEntity.RoleName;
                        dataFilter.DataFilterID = dataFilterEntity.DataFilterID;
                        dataFilter.DataFilterValueID = dataFilterEntity.DataFilterValueID;
                        dataFilter.RecordStatus = DataFilterRecordStatus.New;
                        userDataFilterEntities.Add(dataFilter);
                    }
                }
                MappingUserDataFilterValues(userDataFilterEntities);
            }
        }

        #endregion

        #region Data Filter Sample Service

        /// <summary>
        /// Search action code and data filter by role name
        /// </summary>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ListOfFunctionAndDataFilterFunctionID)]
        public FunctionAndDataFilterEntity[] GetActionCodeAndDFListByRoleName(string roleName)
        {
            ArrayList arrayList = new ArrayList();
            if (!String.IsNullOrEmpty(roleName))
            {
                DataTable dt = new DataTable();
                DbHelper helper = new DbHelper();
                DbCommand command = helper.BuildDbCommand("P_IC_ACTION_CODE_DF_BY_ROLE_NAME");
                helper.AssignParameterValues(
                    command,
                    roleName
                    );
                Helper.Fill(dt, command);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        arrayList.Add(new FunctionAndDataFilterEntity(
                            row["ACTION_CODE"].ToString(),
                            row["DATA_FILTER"].ToString()
                            )
                            );
                    }
                }
            }

            return (FunctionAndDataFilterEntity[])arrayList.ToArray(typeof(FunctionAndDataFilterEntity));
        }
        #endregion

        #region Data Filter Service

        /// <summary>
        /// Get user data filter values
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1,
           ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
  FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ViewUserFunctionID)]
        public List<DataFilterEntity> GetUserDataFilterValuesByUserNameAndRoleName(string userName, string roleName)
        {
            List<DataFilterEntity> dataFilterEntities = new List<DataFilterEntity>();

            if (!String.IsNullOrEmpty(userName)) // username is not  allowed setting "Null" or "Empty" 
            {
                DataTable dt = new DataTable();
                DbHelper helper = new DbHelper();
                DbCommand command = helper.BuildDbCommand("P_IC_USERS_DATA_FILTER_VALUES_GET_BY_USER_NAME_AND_ROLE_NAME");
                helper.AssignParameterValues(
                    command,
                    Membership.ApplicationName,
                    userName,
                    roleName
                    );
                Helper.Fill(dt, command);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        dataFilterEntities.Add(
                            new DataFilterEntity(
                                row["USER_DATA_FILTER_VALUE_ID"].ToString(),
                                row["USER_NAME"].ToString(),
                                row["ROLE_ID"].ToString(),
                                row["ROLE_NAME"].ToString(),
                                row["DATA_FILTER_ID"].ToString(),
                                row["DATA_FILTER"].ToString(),
                                row["DATA_FILTER_VALUE_ID"].ToString(),
                                row["DATA_FILTER_VALUE"].ToString(),
                                OfficeRecordStatus.Original
                             )
                         );
                    }
                }
            }

            return dataFilterEntities;
        }
        /// <summary>
        /// Gets the data filter template data.
        /// </summary>
        /// <param name="roleName">The rolename.</param>
        /// <param name="fullDataFilters">The full data filters(ID).</param>
        /// <param name="authorisedDataFilters">The authorised data filters(ID).</param>
        /// <param name="dataFiltersForUser">The data filters for user.</param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
FunctionID = FunctionNames.ViewUserFunctionID)]
        public List<DataFilterEntity> GetDataFilterTemplateData(string roleName,
            ref List<string> fullDataFilters,
            ref List<string> authorisedDataFilters,
            ref List<DataFilterEntity> dataFiltersForUser)
        {
            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();

            //Get all data filter types and values
            DbCommand command = helper.BuildDbCommand("P_IC_DATA_FILTER_VALUES_FULL");
            Helper.Fill(dt, command);


            List<DataFilterEntity> dataFilterEntities = new List<DataFilterEntity>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    dataFilterEntities.Add(
                        new DataFilterEntity(
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            roleName,
                            row["DATA_FILTER_ID"].ToString(),
                            row["DATA_FILTER"].ToString(),
                            row["DATA_FILTER_VALUE_ID"].ToString(),
                            row["DATA_FILTER_VALUE"].ToString(),
                            DataFilterRecordStatus.New
                         )
                     );
                }
            }

            //Based on roleName, get all available filter types
            authorisedDataFilters = GetAuthorisedDataFilters(roleName);

            //Based on userName, get all available filter values
            List<DataFilterEntity> rawDataFiltersForUser = GetDataFiltersForUser(AppContext.Current.UserName, string.Empty, true);

            //Distinct types. For example, there are  A, B, C in RoleA; B, C, D in RoleB; but in view, we just want to see A, B, C, D
            fullDataFilters = dataFilterEntities.Select(d => d.DataFilterID).Distinct().ToList();

            //After distinct types, filter all other values except '*' for each type
            foreach (string singleDataFilter in fullDataFilters)
            {
                string filter = singleDataFilter;

                if (rawDataFiltersForUser
                    .Where(d => d.DataFilterID.Equals(filter) && d.DataFilterValueID.Equals("*")).Count() > 0)
                {
                    for (int i = rawDataFiltersForUser.Count - 1; i > -1; i--)
                    {
                        if (rawDataFiltersForUser[i].DataFilterID.Equals(singleDataFilter) && !rawDataFiltersForUser[i].DataFilterValueID.Equals("*"))
                        {
                            rawDataFiltersForUser.RemoveAt(i);
                        }
                    }
                }
            }

            dataFiltersForUser = rawDataFiltersForUser.Select(d =>
                                            new DataFilterEntity(
                                                d.UserDataFilterValueID.ToString(),
                                                AppContext.Current.UserName,
                                                string.Empty,
                                                d.RoleName,
                                                d.DataFilterID,
                                                string.Empty,
                                                d.DataFilterValueID,
                                                string.Empty,
                                                DataFilterRecordStatus.Original))
                                                .ToList();

            return dataFilterEntities;
        }

        /// <summary>
        /// Gets the data filters for user.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="roleName">The rolename.</param>
        /// <param name="ignoreSameFilterValue">if set to <c>true</c> [ignore same filter value].</param>
        /// <returns>
        /// Available data filters for the specific user.
        /// </returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
FunctionID = FunctionNames.ViewUserFunctionID)]
        public List<DataFilterEntity> GetDataFiltersForUser(string userName, string roleName, bool ignoreSameFilterValue)
        {
            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_IC_DATA_FILTER_VALUES_GET_FOR_USER");
            helper.AssignParameterValues(
                command,
                Membership.ApplicationName,
                userName,
                roleName
                );
            Helper.Fill(dt, command);

            List<DataFilterEntity> dataFilterEntities = new List<DataFilterEntity>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var filterID = row["DATA_FILTER_ID"];
                    var valueID = row["DATA_FILTER_VALUE_ID"];

                    if (!ignoreSameFilterValue
                        || dataFilterEntities
                            .Where(d => d.DataFilterID.Equals(filterID.ToString())
                                        && d.DataFilterValueID.Equals(valueID.ToString())).Count() == 0)
                    {
                        dataFilterEntities.Add(
                            new DataFilterEntity(
                                row["USER_DATA_FILTER_VALUE_ID"].ToString(),
                                userName,
                                string.Empty,
                                row["ROLE_NAME"].ToString(),
                                row["DATA_FILTER_ID"].ToString(),
                                string.Empty,
                                row["DATA_FILTER_VALUE_ID"].ToString(),
                                string.Empty,
                                DataFilterRecordStatus.Original
                             )
                         );
                    }
                }
            }

            return dataFilterEntities;
        }

        /// <summary>
        /// Gets the authorised data filters.
        /// </summary>
        /// <param name="roleName">The rolename.</param>
        /// <returns>The authorised data filters.</returns>
        private List<string> GetAuthorisedDataFilters(string roleName)
        {
            List<string> authorisedDataFilter = new List<string>();

            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_IC_DATA_FILTER_AUTHORISATION");
            helper.AssignParameterValues(
                command,
                Membership.ApplicationName,
                roleName
                );
            Helper.Fill(dt, command);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    authorisedDataFilter.Add(row["DATA_FILTER_ID"].ToString());
                }
            }

            return authorisedDataFilter;
        }

        #endregion

        #region Private Method For Data Filter

        /// <summary>
        /// Add selected data filter value
        /// </summary>
        /// <param name="dataFilterEntity">dataFilterEntity</param>
        private void CreateDataFilterValue(DataFilterEntity dataFilterEntity)
        {
            if (CheckDataFilterEntity(dataFilterEntity))
            {
                string transactionid = NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId;
                DbHelper helper = new DbHelper();
                DbCommand command = helper.BuildDbCommand("P_IC_USERS_DATA_FILTER_VALUES_INSERT");
                helper.AssignParameterValues(
                    command,
                    Membership.ApplicationName,
                    dataFilterEntity.UserDataFilterValueID,
                    dataFilterEntity.UserName,
                    dataFilterEntity.RoleName,
                    dataFilterEntity.DataFilterID,
                    dataFilterEntity.DataFilterValueID,
                    DateTime.Now,
                    transactionid,
                    AppContext.Current.UserName
                    );
                Helper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Delete selected data filter value
        /// </summary>
        /// <param name="dataFilterEntity">dataFilterEntity</param>
        private void DeleteDataFilterValue(DataFilterEntity dataFilterEntity)
        {
            if (CheckDataFilterEntity(dataFilterEntity))
            {
                //NOTE:No transaction id
                DbHelper helper = new DbHelper();
                DbCommand command = helper.BuildDbCommand("P_IC_USERS_DATA_FILTER_VALUES_DELETE");
                helper.AssignParameterValues(
                    command,
                    Membership.ApplicationName,
                    dataFilterEntity.UserDataFilterValueID
                    );
                Helper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Check data filter validation
        /// </summary>
        /// <param name="dataFilterEntity">dataFilterEntity</param>
        /// <returns>true ? false</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1,
     ModuleID = FunctionNames.RoleModuleID,
     FunctionID = FunctionNames.AssignRolesToUsersFunctionID)]
        public bool CheckDataFilterEntity(DataFilterEntity dataFilterEntity)
        {
            return (dataFilterEntity != null) &&
                !String.IsNullOrEmpty(dataFilterEntity.UserDataFilterValueID) &&
                !String.IsNullOrEmpty(dataFilterEntity.UserName) &&
                !String.IsNullOrEmpty(dataFilterEntity.RoleName) &&
                !String.IsNullOrEmpty(dataFilterEntity.DataFilterID) &&
                !String.IsNullOrEmpty(dataFilterEntity.DataFilterValueID);
        }

        /// <summary>
        /// Mapping data filter entity
        /// Add, updat, delete by record status
        /// </summary>
        /// <param name="dataFilterEntities">dataFilterEntities</param>
        private void MappingUserDataFilterValues(List<DataFilterEntity> dataFilterEntities)
        {
            // Validate inputs where are valid for current logon user
            List<DataFilterEntity> inputs = dataFilterEntities.Where(d => d.RecordStatus.Equals(DataFilterRecordStatus.New)).ToList();
            List<DataFilterEntity> dataFiltersForCurrentUser = GetDataFiltersForCurrentUser();
            ValidateDataFilterInputs(inputs, dataFiltersForCurrentUser);

            foreach (DataFilterEntity dataFilterEntity in dataFilterEntities)
            {
                switch (dataFilterEntity.RecordStatus)
                {
                    case DataFilterRecordStatus.New:
                        CreateDataFilterValue(dataFilterEntity);
                        break;
                    case DataFilterRecordStatus.Delete:
                        DeleteDataFilterValue(dataFilterEntity);
                        break;
                }
            }
        }

        #region ** Validate inputs where are valid for current logon user **

        /// <summary>
        /// Validates the data filter inputs.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="dataFiltersForCurrentUser">The data filters for current user.</param>
        private void ValidateDataFilterInputs(List<DataFilterEntity> inputs, List<DataFilterEntity> dataFiltersForCurrentUser)
        {
            bool isValid = true;

            foreach (DataFilterEntity input in inputs)
            {
                if (!Contains(input, dataFiltersForCurrentUser))
                {
                    isValid = false;
                    break;
                }
            }

            if (!isValid)
            {
                throw new AuthorizationException("The assigned data filter values include exceptional data.");
            }
        }

        /// <summary>
        /// Determines whether [contains] [the specified input].
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="dataFiltersForCurrentUser">The data filters for current user.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        private bool Contains(DataFilterEntity input, List<DataFilterEntity> dataFiltersForCurrentUser)
        {
            bool isContained = false;

            foreach (DataFilterEntity udf in dataFiltersForCurrentUser)
            {
                if (udf.DataFilterID.Equals(input.DataFilterID)
                    && (udf.DataFilterValueID.Equals(input.DataFilterValueID) || udf.DataFilterValueID.Equals("*")))
                {
                    isContained = true;
                    break;
                }
            }

            return isContained;
        }

        /// <summary>
        /// Gets the data filters for current user.
        /// </summary>
        /// <returns></returns>
        private List<DataFilterEntity> GetDataFiltersForCurrentUser()
        {
            List<DataFilterEntity> rawDataFiltersForUser = GetDataFiltersForUser(AppContext.Current.UserName, string.Empty, true);
            List<string> userDataFilters = rawDataFiltersForUser.Select(d => d.DataFilterID).Distinct().ToList();
            foreach (string singleDataFilter in userDataFilters)
            {
                string filter = singleDataFilter;
                if (rawDataFiltersForUser
                    .Where(d => d.DataFilterID.Equals(filter) && d.DataFilterValueID.Equals("*")).Count() > 0)
                {
                    for (int i = rawDataFiltersForUser.Count - 1; i > -1; i--)
                    {
                        if (rawDataFiltersForUser[i].DataFilterID.Equals(singleDataFilter) && !rawDataFiltersForUser[i].DataFilterValueID.Equals("*"))
                        {
                            rawDataFiltersForUser.RemoveAt(i);
                        }
                    }
                }
            }

            List<DataFilterEntity> dataFiltersForUser
                = rawDataFiltersForUser.Select(d =>
                    new DataFilterEntity(
                        d.UserDataFilterValueID.ToString(),
                        AppContext.Current.UserName,
                        string.Empty,
                        d.RoleName,
                        d.DataFilterID,
                        string.Empty,
                        d.DataFilterValueID,
                        string.Empty,
                        DataFilterRecordStatus.Original))
                        .ToList();

            return dataFiltersForUser;
        }

        #endregion

        /// <summary>
        /// Batch clrear one user data filter values
        /// </summary>
        /// <param name="userNames">usernames</param>
        private void ClearUsersDataFilterValues(string[] userNames)
        {
            List<DataFilterEntity> userDataFilterValues;
            foreach (string username in userNames)
            {
                userDataFilterValues = GetUserDataFilterValuesByUserNameAndRoleName(username, String.Empty);
                if (userDataFilterValues.Count == 0) continue;
                foreach (DataFilterEntity userDataFilterValue in userDataFilterValues)
                {
                    DeleteDataFilterValue(userDataFilterValue);
                }
            }
        }

        #endregion

        #region Role Maintenance

        /// <summary>
        ///  Define a new role
        /// </summary>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.NewRoleFunctionID)]
        public void CreateRole(string roleName, string roleDescription)
        {
            ExtendedRole role = new ExtendedRole(roleName, roleDescription, RoleStatus.Active);
            ExtendedRoles.CreateRole(role);
        }

        /// <summary>
        ///  Update a existed role
        /// </summary>
        /// <param name="oldRoleName">oldRoleName</param>
        /// <param name="roleDescription">roleDescription</param>
        /// <param name="status">status</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateRoleFunctionID)]
        public void UpdateRole(string oldRoleName, string roleDescription, string status)
        {
            ExtendedRole role = ExtendedRoles.GetRole(oldRoleName);
            role.Description = roleDescription;
            role.Flag = status;
            ExtendedRoles.UpdateRole(role);
        }

        /// <summary>
        ///  Delete a role and its functions 
        /// </summary>
        /// <param name="roleName">roleName</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateRoleFunctionID)]
        public void DeleteRole(string roleName)
        {
            ExtendedRole role = ExtendedRoles.GetRole(roleName);
            ExtendedRoles.DeleteRole(role, false);
            // Delete related actions for the role
            string[] functions = BusinessActions.GetActionsInRole(roleName);
            if (functions != null && functions.Length > 0)
            {
                BusinessActions.RemoveActionsFromRoles(functions, new [] { roleName });
            }
        }

        /// <summary>
        ///  Get role information by rolename
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>RoleEntity</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ViewRoleFunctionID)]
        public RoleEntity GetRoleByRoleName(string roleName)
        {
            ExtendedRole role = ExtendedRoles.GetRole(roleName);
            RoleEntity entity = new RoleEntity();
            entity.RoleName = role.RoleName;
            entity.Description = role.Description;
            entity.VersionNo = role.AuditData.VersionNo;
            entity.Status = role.Flag;

            return entity;
        }

        /// <summary>
        ///  Search roles by rolename and description
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <param name="description">description</param>
        /// <returns>RoleEntity[]</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SearchRoleFunctionID)]
        public RoleEntity[] FindRolesByConditions(string roleName, string description)
        {
            ExtendedRoleCollection extendedRoleCollection = ExtendedRoles.GetAllRolesAsCollection();

            List<RoleEntity> arrayList = new List<RoleEntity>();
            if (extendedRoleCollection.Count > 0)
            {
                arrayList = (from role in extendedRoleCollection
                             where (string.IsNullOrEmpty(roleName) || SearchHelper.IsRegexMatch(role.RoleName, roleName, @"[A-Za-z0-9_. -/\(\)&]*"))
                             && (string.IsNullOrEmpty(description) || SearchHelper.IsRegexMatch(role.Description, description, @"[\w|\W]*"))
                             orderby role.RoleName ascending
                             select new RoleEntity
                                 (
                                 role.RoleName,
                                 role.Description,
                                 role.Flag
                                 )).ToList();
            }

            return arrayList.ToArray();
        }

        /// <summary>
        ///  Add actions in role
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <param name="actions">actions</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateRoleFunctionID)]
        public void AddActionsInRole(string roleName, string[] actions)
        {
            BusinessActions.AddActionsToRoles(actions, new [] { roleName });
        }

        /// <summary>
        ///  Update actions in role
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <param name="actions">actions</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateRoleFunctionID)]
        public void UpdateActionsInRole(string roleName, string[] actions)
        {
            // find actions in role
            string[] userActions = BusinessActions.GetActionsInRole(roleName);

            if (userActions != null && userActions.Length > 0)
            {
                // 1. confirm user action is not in actions, remove it
                ArrayList userArrayList = new ArrayList();
                foreach (string userAction in userActions)
                {
                    bool userFlag = false;
                    foreach (string action in actions)
                    {
                        if (action == userAction)
                        {
                            userFlag = true;
                            break;
                        }
                    }
                    if (!userFlag)
                    {
                        userArrayList.Add(userAction);
                    }
                }
                if ( userArrayList.Count > 0)
                {
                    string[] removedUserActions = (string[])userArrayList.ToArray(typeof(string));
                    BusinessActions.RemoveActionsFromRoles(
                        removedUserActions,
                        new [] { roleName }
                        );
                }

                // 2. confirm new acton is not in user actions, add it
                //    otherwise, donot care
                userArrayList = new ArrayList();
                foreach (string action in actions)
                {
                    bool flag = false;
                    foreach (string userAction in userActions)
                    {
                        if (userAction == action)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        userArrayList.Add(action);
                    }
                }
                if ( userArrayList.Count > 0)
                {
                    string[] addedUserActions = (string[])userArrayList.ToArray(typeof(string));
                    BusinessActions.AddActionsToRoles(
                        addedUserActions,
                        new [] { roleName }
                        );
                }
            }
            else
            {
                // if role has not actions, adding actions directly
                BusinessActions.AddActionsToRoles(actions, new [] { roleName });
            }
        }

        /// <summary>
        /// Get all actions
        /// </summary>
        /// <returns>all actions</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ViewRoleFunctionID)]
        public string[] GetAllActions()
        {
            string[] actions = BusinessActions.GetAllActions();
            return actions;
        }

        /// <summary>
        ///  Get actions in role
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>Actions in role</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ViewRoleFunctionID)]
        public string[] GetActionsInRole(string roleName)
        {
            string[] actions = BusinessActions.GetActionsInRole(roleName);

            return actions;
        }


        /// <summary>
        ///  Role exists ?
        /// </summary>
        /// <param name="roleName">rolename</param>
        /// <returns>true ? false</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateRoleFunctionID)]
        public bool RoleExists(string roleName)
        {
            return Roles.RoleExists(roleName);
        }

        #endregion

        #region Role Assignment

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SecurityModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionID)]
        public void AddUserToRoles(string userName, string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                Roles.AddUserToRoles(userName, roles);
            }
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1,
            ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionID)]
        public void UpdateRolesForUser(string userName, string[] roles)
        {
            string[] oldRoles = Roles.GetRolesForUser(userName);
            if (oldRoles != null && oldRoles.Length > 0)
            {
                Roles.RemoveUserFromRoles(userName, oldRoles);
            }
            if (roles != null && roles.Length > 0)
            {
                Roles.AddUserToRoles(userName, roles);
            }
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SecurityModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.AssignRolesToUsersFunctionID)]
        public void UpdateRolesForUsers(string[] userNames, string[] roles)
        {
            foreach (string userName in userNames)
            {
                UpdateRolesForUser(userName, roles);
            }
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1,
            ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
        FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ViewUserFunctionID)]
        public RoleEntity[] GetRolesByUserName(string userName)
        {
            ExtendedRoleCollection roles = ExtendedRoles.GetRolesForUserAsCollection(userName);
            if (roles != null && roles.Count > 0)
            {
                return roles.Select(
                    d => new RoleEntity(
                        d.RoleName,
                        d.Description,
                        d.Flag))
                        .ToArray();
            }

            return new RoleEntity[] { };
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.RoleModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateRoleFunctionID)]
        public string[] GetUsersByRoleName(string roleName, string userStatus)
        {
            string[] allUsers = Roles.GetUsersInRole(roleName);

            if (allUsers.Length == 0) return new string[] { };

            if (userStatus == UserStatus.Both) return allUsers;

            string[] users = (from u in allUsers
                              where GetUser(u).UserStatus == userStatus
                              select u).ToArray();

            return users;
        }

        #endregion

        #region GIS Role Management
        ///<summary>
        //Retrives Roles for a given user
        ///</summary>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ETRolesFunctionID,
        FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ETRolesFunctionID)]
        public List<ETRoleEntity> GetETRolesForUser(string UserName)
        {
            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_GISUserRole_S");
            helper.AssignParameterValues(command, UserName);
            Helper.Fill(dt, command);


            List<ETRoleEntity> dtet = new List<ETRoleEntity>();
            ETRoleEntity ET;
            foreach (DataRow dr in dt.Rows)
            {
                ET = new ETRoleEntity();
                ET.RoleName = dr["RoleName"].ToString();
                ET.UserId = dr["UserName"].ToString();
                dtet.Add(ET);
            }

            return dtet;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ETRolesFunctionID,
       FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ETRolesFunctionID)]
        public void DeleteUsersETRole(ETRoleEntity etRoleEntity)
        {
            string transactionid = NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId;
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_GISUserRole_D");

            helper.AssignParameterValues(command, AppContext.Current.UserName.ToString(), Membership.ApplicationName, transactionid, etRoleEntity.UserId, etRoleEntity.RoleName);
            Helper.ExecuteNonQuery(command);
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ETRolesFunctionID,
       FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ETRolesFunctionID)]
        public void CreateUsersETRole(ETRoleEntity etRoleEntity)
        {
            string transactionid = NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId;
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_GISUserRole_I");

            helper.AssignParameterValues(command, transactionid, etRoleEntity.RoleName, etRoleEntity.UserId, AppContext.Current.UserName.ToString(), DateTime.Now);
            Helper.ExecuteNonQuery(command);
        }

        #endregion

        #region office

        /// <summary>
        /// Get the whole organizational unit hierarchy for a user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>The <see cref="OfficesHierarchyDataSet"/> containing the retrieved whole organizational unit hierarchy</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
      FunctionID = FunctionNames.ViewUserFunctionID)]
        public OfficesHierarchyDataSet GetOfficesAllHierarchy(string userName)
        {
            OfficesHierarchyDataSet ds = new OfficesHierarchyDataSet();
            this.Helper.Fill(ds.LookupOrganisationalUnitHierarchy, "P_IC_OFFICES_GET_ALL");
            if (!string.IsNullOrEmpty(userName))
            {
                this.Helper.Fill(ds.OrganisationUser, "P_IC_OFFICES_USER_GET", userName);
            }
            return ds;
        }

        /// <summary>
        /// Get the whole organizational unit hierarchy (Only Regional Ones) for a user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="codeID"></param>
        /// <returns>The <see cref="OfficesHierarchyDataSet"/> containing the retrieved whole organizational unit hierarchy</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,FunctionID = FunctionNames.ViewUserFunctionID)]
        public OfficesHierarchyDataSet GetOfficesHierarchy(string userName,string codeID)
        {
            OfficesHierarchyDataSet ds = new OfficesHierarchyDataSet();

            Dictionary<string, string> tables = new Dictionary<string, string>();
            tables.Add(ds.LookupOrganisationalUnitHierarchy.TableName, "P_IC_OFFICES_GET");
            tables.Add(ds.OrganisationUser.TableName, "P_IC_OFFICES_USER_GET");
            CommandList commands = Helper.BuildFillCommandList(ds, tables);

            Helper.AssignParameterValues(commands[ds.LookupOrganisationalUnitHierarchy.TableName].SelectCommand, codeID);

            Helper.AssignParameterValues(commands[ds.OrganisationUser.TableName].SelectCommand, userName);
            Helper.Fill(ds, commands);

            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officesOfUser"></param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
      FunctionID = FunctionNames.UpdateUserFunctionID)]
        public void InsertOfficesOfUser(OfficesHierarchyDataSet.OrganisationUserDataTable officesOfUser)
        {
            if (officesOfUser == null)
            {
                return;
            }
            DataRow[] rows = officesOfUser.Select("", "", DataViewRowState.Added);
            if (rows.Length == 0)
            {
                return;
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand command = db.GetStoredProcCommand("P_IC_OFFICES_USER_I");
            db.AddInParameter(command, "UserID", DbType.String, "UserID", DataRowVersion.Current);
            db.AddInParameter(command, "NodeID", DbType.String, "NodeID", DataRowVersion.Current);
            db.AddInParameter(command, "UnitID", DbType.String, "UnitID", DataRowVersion.Current);
            db.AddInParameter(command, "VersionNO", DbType.Int32, 1);
            db.AddInParameter(command, "TransactionID", DbType.String, ApplicationContextFactory.GetApplicationContext().TransactionId);
            db.AddInParameter(command, "CreatedBY", DbType.String, AppContext.Current.UserName);
            db.AddInParameter(command, "CreatedTime", DbType.DateTime, DateTime.Now);

            db.UpdateDataSet(rows, command, null, null, UpdateBehavior.Standard);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officesOfUser"></param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
      FunctionID = FunctionNames.UpdateUserFunctionID)]
        public void DeleteOfficesOfUser(OfficesHierarchyDataSet.OrganisationUserDataTable officesOfUser)
        {
            if (officesOfUser==null)
            {
                return;
            }
            DataRow[] rows = officesOfUser.Select("", "", DataViewRowState.Deleted);
            if (rows.Length == 0)
            {
                return;
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand command = db.GetStoredProcCommand("P_IC_OFFICES_USER_D");
            db.AddInParameter(command, "UserID", DbType.String, "UserID", DataRowVersion.Current);
            db.AddInParameter(command, "NodeID", DbType.String, "NodeID", DataRowVersion.Current);
            db.AddInParameter(command, "VersionNO", DbType.Int32, 1);
            db.AddInParameter(command, "TransactionID", DbType.String, ApplicationContextFactory.GetApplicationContext().TransactionId);
            db.AddInParameter(command, "LastUpdatedBy", DbType.String, AppContext.Current.UserName);
            db.AddInParameter(command, "LastUpdatedTime", DbType.DateTime, DateTime.Now);
            db.UpdateDataSet(rows,  null, null,command, UpdateBehavior.Standard);

        }
        #endregion


    }
}

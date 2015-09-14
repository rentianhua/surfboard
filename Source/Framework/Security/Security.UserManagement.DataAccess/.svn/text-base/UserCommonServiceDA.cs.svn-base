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
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Profile;
using HiiP.Framework.Common.Constants;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;


using NCS.IConnect.Helpers.Data;
using NCS.IConnect.Messaging;
using HiiP.Framework.Common.ApplicationContexts;

namespace HiiP.Framework.Security.UserManagement.DataAccess
{
    public class UserCommonServiceDA : HiiPDataAccessBase
    {

        #region User Maintenance Service
       

        /// <summary>
        /// Get user information by username
        /// </summary>
        /// <param name="userName">username</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.SearchUserFunctionID)]
        public UserInfoEntity GetUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }
            ProfileBase userProfile = ProfileBase.Create(userName);
            UserInfoEntity userInfoEntity = new UserInfoEntity();

            userInfoEntity.UserName = userName;
            userInfoEntity.FirstName = userProfile["FirstName"].ToString();
            userInfoEntity.Initials = userProfile["Initials"] == null ? String.Empty : userProfile["Initials"].ToString();
            userInfoEntity.LastName = userProfile["LastName"].ToString();
            userInfoEntity.Display = userProfile["Display"].ToString();
            userInfoEntity.Alias = userProfile["Alias"] == null ? String.Empty : userProfile["Alias"].ToString();
            userInfoEntity.Gender = userProfile["Gender"].ToString();
            userInfoEntity.Title = userProfile["Title"].ToString();
            userInfoEntity.DateOfBirth = userProfile["DateOfBirth"] == null ? MinMaxValues.MinDate : Convert.ToDateTime(userProfile["DateOfBirth"]);
            userInfoEntity.Email = userProfile["Email"].ToString();
            userInfoEntity.TelephoneNo = userProfile["TelephoneNo"].ToString();
            userInfoEntity.FaxNo = userProfile["FaxNo"] == null ? String.Empty : userProfile["FaxNo"].ToString();
            userInfoEntity.MobileNo = userProfile["MobileNo"] == null ? String.Empty : userProfile["MobileNo"].ToString();
            userInfoEntity.PageNo = userProfile["PageNo"] == null ? String.Empty : userProfile["PageNo"].ToString();
            //userInfoEntity.Organisation = userProfile["Organisation"].ToString();
            userInfoEntity.Remarks = userProfile["Remarks"] == null ? String.Empty : userProfile["Remarks"].ToString();
            userInfoEntity.UserStatus = (this.IsActiveUser(userInfoEntity.UserName)) ? UserStatus.Active : UserStatus.Inactive;
            //XElement userConfigurationInfo = new XElement(HiiP.Framework.Common.Constants.UserConfigInfoSectionNames.RootSectionName);
            //if (userProfile["ConfigurationInfo"] != null && userProfile["ConfigurationInfo"].ToString() != String.Empty)
            //{
            //    userConfigurationInfo = XElement.Parse(userProfile["ConfigurationInfo"].ToString());
            //}
            //userInfoEntity.ConfigurationInfo = userConfigurationInfo;
            userInfoEntity.VersionNo = Convert.ToInt32(userProfile["VersionNo"]);

            if (userProfile["Office"] != null)
            {
                userInfoEntity.Office = userProfile["Office"].ToString();
            }


            // Extend "job title" property
            userInfoEntity.JobTitle = userProfile["JobTitle"] == null ? String.Empty : userProfile["JobTitle"].ToString();

            // Extend "ReportsTo" property
            userInfoEntity.ReportsTo = userProfile["ReportsTo"] == null ? String.Empty : userProfile["ReportsTo"].ToString();

            userInfoEntity.EPRIN = Convert.ToInt32(userProfile["EPRIN"]);
            if (userInfoEntity.EPRIN == -999)
            {
                userInfoEntity.EPRIN = null;
            }

            if (userProfile["IsInternal"] != null)
            {
                userInfoEntity.IsInternal = Convert.ToBoolean(userProfile["IsInternal"]);
            }
            else
            {
                userInfoEntity.IsInternal = true;
            }

            //Delegations
            userInfoEntity.Branch = userProfile["Branch"].ToString();
            userInfoEntity.Unit = userProfile["Unit"].ToString();
            userInfoEntity.SubUnit = userProfile["SubUnit"].ToString();
            userInfoEntity.Grade = userProfile["Grade"].ToString();

            return userInfoEntity;
        }
        
        /// <summary>
        /// Get user information by username
        /// </summary>
        /// <param name="userName">username</param>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UserModuleID,
        FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.GetUserBasicInfoFunctionID)]
        public UserBasicInfoEntity GetUserBasic(string userName)
        {
            ProfileBase userProfile = ProfileBase.Create(userName);
            UserBasicInfoEntity UserBasicInfoEntity = new UserBasicInfoEntity();
            UserBasicInfoEntity.UserName = userName;
            UserBasicInfoEntity.Display = userProfile["Display"].ToString();
            UserBasicInfoEntity.Title = userProfile["JobTitle"].ToString();

            if (userProfile["Office"] != null)
            {
                UserBasicInfoEntity.Office = userProfile["Office"].ToString();
            }

            UserBasicInfoEntity.FirstName = userProfile["FirstName"].ToString();
            UserBasicInfoEntity.LastName = userProfile["LastName"].ToString();
            UserBasicInfoEntity.NameTitle = userProfile["Title"].ToString();

            UserBasicInfoEntity.Status = (this.IsActiveUser(userName)) ? UserStatus.Active : UserStatus.Inactive;
            UserBasicInfoEntity.ReportsTo = userProfile["ReportsTo"] == null ? String.Empty : userProfile["ReportsTo"].ToString();
            UserBasicInfoEntity.Email = userProfile["Email"].ToString();

            //Delegations
            UserBasicInfoEntity.Branch = userProfile["Branch"].ToString();
            UserBasicInfoEntity.Unit = userProfile["Unit"].ToString();
            UserBasicInfoEntity.SubUnit = userProfile["SubUnit"].ToString();

            return UserBasicInfoEntity;

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
        #endregion

        #region new APIs

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
     FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetAllOffices()
        {
            OfficesDataSet ds = new OfficesDataSet();

            Helper.Fill(ds,
                new []{ds.CM_LookupOrganisationalUnit.TableName,
                    ds.CM_LookupOrganisationalUnitByType.TableName},
                   "P_IC_ALL_OFFICES_GET");

            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
 FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesHierarchyDataSet GetAllOfficesOfUser(string userName)
        {
            OfficesHierarchyDataSet officesOfUser = new OfficesHierarchyDataSet();

            Helper.Fill(officesOfUser.OrganisationUser, "P_IC_OFFICES_USER_GET", userName);

            return officesOfUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeUnitID"></param>
        /// <returns></returns>
         [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
 FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesHierarchyDataSet GetAllUsersInOffice(string officeUnitID)
        {
            OfficesHierarchyDataSet officesOfUser = new OfficesHierarchyDataSet();

            Helper.Fill(officesOfUser.OrganisationUser, "P_IC_USERS_IN_OFFICE_GET", officeUnitID);

            return officesOfUser;
        }

         [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
 FunctionID = FunctionNames.ViewGroupCalendarFunctionID)]
         public AppointmentGroupCalendar GetGroupCalendarsList(string userID)
         {
             AppointmentGroupCalendar ds = new AppointmentGroupCalendar();

             Helper.Fill(ds.SS_AppointmentGroupCalendar, "P_SS_AppointmentGroupCalendar_S_ByUserID", userID);

             return ds;
         }

         [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
FunctionID = FunctionNames.SaveGroupCalendarFunctionID)]
         public void SaveGroupCalendarsList(AppointmentGroupCalendar dsGroupCalendar)
         {
             Helper.Update(dsGroupCalendar.SS_AppointmentGroupCalendar);


         }
       #endregion

         /// <summary>
         /// Get all messages with culture name
         /// </summary>
         /// <param name="latestUpdatedTime"></param>
         /// <returns></returns>
         [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Infrastructure.Interface.Constants.FunctionNames.FrameworkModuleID, FunctionID = HiiP.Infrastructure.Interface.Constants.FunctionNames.GetAllMessagesFunctionName)]
         public DataTable GetMessages(DateTime latestUpdatedTime)
         {
             DataTable dt = new DataTable();
             var manger = MessageManagerFactory.Create();
             var provider = manger.MessageProviders.Where(x => typeof(DbMessageProvider).IsAssignableFrom(x.GetType())).FirstOrDefault<MessageProvider>() as DbMessageProvider;
             if (provider == null)
             {
                 throw new ConfigurationErrorsException("Unable to find out DbMessageProvider");
             }

             var datePoint = latestUpdatedTime.ToLocalTime();
             if (datePoint>MinMaxValues.MaxDate)
             {
                 datePoint = MinMaxValues.MaxDate;
             }
             if (datePoint<MinMaxValues.MinDate)
             {
                 datePoint = MinMaxValues.MinDate;
             }
             Helper.Fill(dt, "dbo.P_IC_MESSAGE_GET_DELTA", datePoint,provider.ApplicationName);

             //foreach (DataRow item in dt.Rows)
             //{
             //    item["MessageRoute"] = provider.ProviderName;
             //}
             
             return dt;
         }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="userName"></param>
         /// <returns></returns>
         public bool IsActiveUser(string userName)
         {
             //Not use this.Helper to avoid to insert log, because the method will be called by UserNameExtractionCallHanlders
             DbHelper dbHelper = new DbHelper();
             var userStatus = dbHelper.ExecuteScalar("P_IC_USER_INFO_IS_ACTIVE_GET", userName);
             if (userStatus == null || userStatus == DBNull.Value)
             {
                 return false;
             }

             var result = userStatus.ToString().ToLower();
             return result.Equals(UserStatus.Active.ToLower());

         }
    }
}

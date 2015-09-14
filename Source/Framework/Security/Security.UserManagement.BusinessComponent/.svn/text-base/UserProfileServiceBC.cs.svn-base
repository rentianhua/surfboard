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

using System.Collections.Generic;
using System.Xml.Linq;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.DataAccess;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Infrastructure.Interface.BusinessEntities;

namespace HiiP.Framework.Security.UserManagement.BusinessComponent
{
    public class UserProfileServiceBC : HiiPBusinessComponentBase
    {

        /// <summary>
        /// Get current user profile
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID, 
            FunctionID = FunctionNames.GetUserInfoFunctionID)]
        public HiiP.Infrastructure.Interface.BusinessEntities.ProfileInfo GetCurrentUserProfile()
        {
            var da = InstanceBuilder.CreateInstance<UserMaintenanceServiceDA>();
            UserInfoEntity entity = da.GetUser(AppContext.Current.UserName);
            ProfileInfo profile = new ProfileInfo(
                entity.Title,
                entity.Email,
                entity.TelephoneNo,
                entity.FaxNo,
                entity.MobileNo,
                entity.PageNo,
                entity.Office);

            //Delegations
            profile.Branch = entity.Branch;
            profile.Unit = entity.Unit;
            profile.SubUnit = entity.SubUnit;
            
            profile.VersionNo = entity.VersionNo;

            return profile;
        }

        /// <summary>
        /// Change current user profile
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID, 
            FunctionID = FunctionNames.UpdateCurrentUserProfileFunctionID)]
        public void ChangeCurrentUserProfile(ProfileInfo profile)
        {
            var da = InstanceBuilder.CreateInstance<UserMaintenanceServiceDA>();
            UserInfoEntity userInfoEntity = da.GetUser(AppContext.Current.UserName);
            userInfoEntity.Title = profile.Title;
            userInfoEntity.Email = profile.Email;
            userInfoEntity.TelephoneNo = profile.TelephoneNo;
            userInfoEntity.FaxNo = profile.FaxNo;
            userInfoEntity.MobileNo = profile.MobileNo;
            userInfoEntity.PageNo = profile.PageNo;
            userInfoEntity.Office = profile.Office;

            InstanceBuilder.CreateInstance<UserMaintenanceServiceDA>().UpdateUserInfo(userInfoEntity,
               new List<DataFilterEntity>(),
               null);
        }

        /// <summary>
        /// Get user configuration information
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
            FunctionID = FunctionNames.GetUserInfoFunctionID)]
        public XElement GetUserConfigurationInfo()
        {
            System.Web.Profile.ProfileBase userProfile = System.Web.Profile.ProfileBase.Create(AppContext.Current.UserName);
            string configuration = userProfile["ConfigurationInfo"] as string;
            return (string.IsNullOrEmpty(configuration)) ? new XElement(HiiP.Framework.Common.Constants.UserConfigInfoSectionNames.RootSectionName) : XElement.Parse(userProfile["ConfigurationInfo"].ToString());
        }

        /// <summary>
        /// Set user configuration information
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID, 
            FunctionID = FunctionNames.UpdateCurrentUserProfileFunctionID)]
        public void SetUserConfigurationInfo(XElement userConfigurationInfo)
        {
            System.Web.Profile.ProfileBase userProfile = System.Web.Profile.ProfileBase.Create(AppContext.Current.UserName);
            if (null == userConfigurationInfo)
            {
                userProfile["ConfigurationInfo"] = null;
            }
            else
            {
                userProfile["ConfigurationInfo"] = userConfigurationInfo.ToString();
            }

            if (userProfile["EPRIN"] != null && userProfile["EPRIN"].ToString() == "-999")
            {
                userProfile["EPRIN"] = null;
            }

            userProfile.Save();
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
FunctionID = FunctionNames.ViewGroupCalendarFunctionID)]
        public AppointmentGroupCalendar GetGroupCalendarsList()
        {
            var da = InstanceBuilder.CreateInstance<UserProfileServiceDA>();
            return da.GetGroupCalendarsList(AppContext.Current.UserID);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
FunctionID = FunctionNames.SaveGroupCalendarFunctionID)]
        public void SaveGroupCalendarsList(AppointmentGroupCalendar dsGroupCalendar)
        {
            var da = InstanceBuilder.CreateInstance<UserProfileServiceDA>();
            da.SaveGroupCalendarsList(dsGroupCalendar);
        }

    }
}

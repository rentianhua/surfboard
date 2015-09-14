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
using System.Data;
using System.Linq;
using System.Web.Security;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.DataAccess;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace HiiP.Framework.Security.UserManagement.BusinessComponent
{
    public class UserCommonServiceBC : HiiPBusinessComponentBase
    {
        private CacheManager _cacheManager;
        private const string AllOfficesCacheKey = "__AllOffices";

        private CacheManager OfficeCacheManager
        {
            get
            {
                if (_cacheManager == null)
                {
                    _cacheManager = CacheFactory.GetCacheManager();
                }

                return _cacheManager;
            }
        }

        private UserCommonServiceDA _userCommonServiceDA;

        private UserCommonServiceDA DAInstance
        {
            get
            {
                if (_userCommonServiceDA == null)
                {
                    _userCommonServiceDA = InstanceBuilder.Wrap<UserCommonServiceDA>(new UserCommonServiceDA());
                }

                return _userCommonServiceDA;
            }
        }
        /// <summary>
        /// Create user information(user account, user basic information)
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID, 
            FunctionID = FunctionNames.GetUserInfoFunctionID)]
        public UserInfoEntity GetUserInformation()
        {
            UserInfoEntity entity = DAInstance.GetUser(AppContext.Current.UserName);
            //entity.ConfigurationInfo = new XElement(UserConfigInfoSectionNames.RootSectionName);
            return entity;
        }

        /// <summary>
        /// Get display name and title of the user
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID, 
            FunctionID = FunctionNames.GetUserBasicInfoFunctionID)]
        public List<UserBasicInfoEntity> GetUsersBasicInformation(string[] userNames)
        {
            List<UserBasicInfoEntity> Entity = new List<UserBasicInfoEntity>();
            foreach (string user in userNames) Entity.Add(DAInstance.GetUserBasic(user));
            return Entity;
        }

       


        /// <summary>
        /// GetOfficeDetails
        /// </summary>
        /// <param name="OrganisationalUnitID">office ID</param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID, 
            FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficeDetailEntity GetOfficeDetails(string OrganisationalUnitID)
        {
            OrganisationMaintenanceServiceDA orgMaintenanceServiceDA =
            InstanceBuilder.Wrap<OrganisationMaintenanceServiceDA>(new OrganisationMaintenanceServiceDA());
            return orgMaintenanceServiceDA.GetOfficeDetails(OrganisationalUnitID);
        }

        #region new APIs

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
    FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetDefaultOfficeDetailForUser(string userName)
        {
            var unitId = this.GetDefaultOfficeIDForUser(userName);

            if (string.IsNullOrEmpty(unitId))
            {
                return new OfficesDataSet();
            }

            var allOffices = this.GetAllOfficesWithoutCopy();

            var lookupOrganisationalUnitRow = allOffices.CM_LookupOrganisationalUnit.FindByOrganisationalUnitID(unitId);
            if (lookupOrganisationalUnitRow==null)
            {
                return new OfficesDataSet();
            }

            var defaultOffices = from type in allOffices.CM_LookupOrganisationalUnitByType
                                 where type.OrganisationalUnitID == unitId
                                 select type;

            
            var result = new OfficesDataSet();
            result.BeginInit();
            result.CM_LookupOrganisationalUnit.ImportRow(lookupOrganisationalUnitRow);

            foreach (var lookupOrganisationalUnitByTypeRow in defaultOffices)
            {
                result.CM_LookupOrganisationalUnitByType.ImportRow(lookupOrganisationalUnitByTypeRow);
            }
            result.EndInit();

            return result;
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
    FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public string GetDefaultOfficeNameForUser(string userName)
        {
            var officeID = this.GetDefaultOfficeIDForUser(userName);

            if (string.IsNullOrEmpty(officeID))
            {
                return string.Empty;
            }

            var officeRow = this.GetAllOfficesWithoutCopy().CM_LookupOrganisationalUnit.FindByOrganisationalUnitID(officeID);

            return (officeRow == null) ? "" : officeRow.OrganisationalUnitName;
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
    FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public string GetDefaultOfficeIDForUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return string.Empty;
            }

            var userProfile = System.Web.Profile.ProfileBase.Create(userName);
            return userProfile["Office"].ToString();
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
    FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetOffice(string officeUnitID)
        {
            if (string.IsNullOrEmpty(officeUnitID))
            {
                return new OfficesDataSet();
            }

            List<int> ids = new List<int>();

            //typeID --> unitID
            var allOffices = GetAllOfficesWithoutCopy();
            var typeRows = allOffices.CM_LookupOrganisationalUnitByType.AsEnumerable().Where(row => row.OrganisationalUnitID == officeUnitID);
            if (typeRows.Count() == 0)
            {
                return new OfficesDataSet();
            }

            foreach (var item in typeRows)
            {
                ids.Add(item.OrganisationalUnitTypeID);
            }
            OfficesDataSet result = ImportOfficesData(ids, allOffices);
            allOffices.Dispose();
            return result;
        }


        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
    FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public string GetOfficeName(string officeUnitID)
        {
            if (string.IsNullOrEmpty(officeUnitID))
            {
                return string.Empty;
            }

            var allOffices = GetAllOfficesWithoutCopy();
            var officeRow = allOffices.CM_LookupOrganisationalUnit.FindByOrganisationalUnitID(officeUnitID);

            if (officeRow == null)
            {
                return string.Empty;
            }

            return officeRow.OrganisationalUnitName;
        }


        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
   FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetAllOffices()
        {
            var data = this.GetAllOfficesWithoutCopy();

            return data.Copy() as OfficesDataSet;
        }

        private OfficesDataSet GetAllOfficesWithoutCopy()
        {
            if (OfficeCacheManager == null)
            {
                throw new HiiP.Framework.Common.BusinessException("Uable to create cache storage.");
            }

            object obj = OfficeCacheManager.GetData(AllOfficesCacheKey);
            OfficesDataSet data = obj as OfficesDataSet;
            if (obj == null || data == null)
            {
                data = DAInstance.GetAllOffices();
                OfficeCacheManager.Add(AllOfficesCacheKey, data, CacheItemPriority.None, null,
                    new AbsoluteTime(new TimeSpan(0, 0, 30)));
            }

            return data;
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
   FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetAllOfficesByType(string codeID)
        {
            if (string.IsNullOrEmpty(codeID))
            {
                return new OfficesDataSet();
            }

            OfficesDataSet allOffices = this.GetAllOfficesWithoutCopy();

            var selected = from d in allOffices.CM_LookupOrganisationalUnitByType
                           where d.CodeID.Equals(codeID,StringComparison.InvariantCultureIgnoreCase)
                           orderby d.OrganisationalUnitName
                           select d.OrganisationalUnitTypeID;

            OfficesDataSet ds = ImportOfficesData(selected.ToList<int>(), allOffices);


            return ds;
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
   FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetAllOfficesOfUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return new OfficesDataSet();
            }

            OfficesDataSet allOffices = this.GetAllOfficesWithoutCopy();
            OfficesHierarchyDataSet officesOfUser = DAInstance.GetAllOfficesOfUser(userName);

            var selected = from c in officesOfUser.OrganisationUser
                           join d in allOffices.CM_LookupOrganisationalUnitByType on c.UnitID equals d.OrganisationalUnitID
                           where c.UserID.Equals(userName,StringComparison.InvariantCultureIgnoreCase)
                           orderby d.OrganisationalUnitName
                           select d.OrganisationalUnitTypeID;

            OfficesDataSet ds = ImportOfficesData(selected.ToList<int>(), allOffices);

            officesOfUser.Dispose();
            return ds;

        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public UserBasicInfoEntity[] GetAllUsersInOffice(string officeUnitID)
        {
            //codeID is useless
            if (string.IsNullOrEmpty(officeUnitID))
                return new UserBasicInfoEntity[0];

            var ds = DAInstance.GetAllUsersInOffice(officeUnitID);
            ds.Dispose();
            var userBasicInfoEntities = (from OfficesHierarchyDataSet.OrganisationUserRow user in ds.OrganisationUser.Rows
                                         select new UserBasicInfoEntity(user.UserID, user["DISPLAY"].ToString(), user["JOB_TITLE"].ToString()) { Status = user["STATUS"].ToString() }).ToArray();
            return userBasicInfoEntities;
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
   FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetSubOffices(string officeUnitID, string codeID)
        {
            if (string.IsNullOrEmpty(officeUnitID)
                || string.IsNullOrEmpty(codeID))
            {
                return new OfficesDataSet();
            }

            OfficesDataSet allOffices = this.GetAllOfficesWithoutCopy();
            OfficesHierarchyDataSet officesHierarchy = DAInstance.GetOfficesHierarchy(null,codeID);

            var selected = from parent in allOffices.CM_LookupOrganisationalUnitByType
                           join h in officesHierarchy.LookupOrganisationalUnitHierarchy on parent.OrganisationalUnitTypeID equals (h.IsParentIDNull() ? -1 : h.ParentID)
                           join child in allOffices.CM_LookupOrganisationalUnitByType on h.OrganisationalUnitTypeID equals child.OrganisationalUnitTypeID
                           where parent.CodeID.Equals(codeID,StringComparison.InvariantCultureIgnoreCase)
                            && parent.OrganisationalUnitID.Equals(officeUnitID, StringComparison.InvariantCultureIgnoreCase)
                            orderby child.OrganisationalUnitName
                           select child.OrganisationalUnitTypeID;

            OfficesDataSet ds = ImportOfficesData(selected.ToList<int>(), allOffices);

            officesHierarchy.Dispose();
            return ds;
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
   FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetParentOffice(string officeUnitID, string codeID)
        {
            if (string.IsNullOrEmpty(officeUnitID)
                || string.IsNullOrEmpty(codeID))
            {
                return new OfficesDataSet();
            }

            OfficesDataSet allOffices = this.GetAllOfficesWithoutCopy();
            OfficesHierarchyDataSet officesHierarchy = DAInstance.GetOfficesHierarchy(null,codeID);

            var selected = from child in allOffices.CM_LookupOrganisationalUnitByType
                           join h in officesHierarchy.LookupOrganisationalUnitHierarchy on child.OrganisationalUnitTypeID equals h.OrganisationalUnitTypeID
                           join parent in allOffices.CM_LookupOrganisationalUnitByType on (h.IsParentIDNull() ? -1 : h.ParentID) equals parent.OrganisationalUnitTypeID
                           where child.CodeID.Equals(codeID, StringComparison.InvariantCultureIgnoreCase)
                            && child.OrganisationalUnitID.Equals(officeUnitID, StringComparison.InvariantCultureIgnoreCase)
                            orderby parent.OrganisationalUnitName
                           select parent.OrganisationalUnitTypeID;


            OfficesDataSet ds = ImportOfficesData(selected.ToList<int>(), allOffices);

            officesHierarchy.Dispose();
            return ds;
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
   FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetSubOffices(int officeTypeID)
        {
            if (officeTypeID<=0)
            {
                return new OfficesDataSet();
            }

            OfficesDataSet allOffices = this.GetAllOfficesWithoutCopy();
            OfficesDataSet.CM_LookupOrganisationalUnitByTypeRow officeRow = allOffices.CM_LookupOrganisationalUnitByType.FindByOrganisationalUnitTypeID(officeTypeID);
            if (officeRow==null)
            {
                return new OfficesDataSet();
            }
            OfficesHierarchyDataSet officesHierarchy = DAInstance.GetOfficesHierarchy(null, officeRow.CodeID);

            var selected = from parent in allOffices.CM_LookupOrganisationalUnitByType
                           join h in officesHierarchy.LookupOrganisationalUnitHierarchy on parent.OrganisationalUnitTypeID equals (h.IsParentIDNull() ? -1 : h.ParentID)
                           join child in allOffices.CM_LookupOrganisationalUnitByType on h.OrganisationalUnitTypeID equals child.OrganisationalUnitTypeID
                           where parent.OrganisationalUnitTypeID==officeTypeID
                           orderby child.OrganisationalUnitName
                           select child.OrganisationalUnitTypeID;

            OfficesDataSet ds = ImportOfficesData(selected.ToList<int>(), allOffices);

            officesHierarchy.Dispose();
            return ds;

        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
   FunctionID = FunctionNames.ViewOfficeFunctionID)]
        public OfficesDataSet GetParentOffice(int officeTypeID)
        {
            if (officeTypeID <= 0)
            {
                return new OfficesDataSet();
            }

            OfficesDataSet allOffices = this.GetAllOfficesWithoutCopy();
            OfficesDataSet.CM_LookupOrganisationalUnitByTypeRow officeRow = allOffices.CM_LookupOrganisationalUnitByType.FindByOrganisationalUnitTypeID(officeTypeID);
            if (officeRow == null)
            {
                return new OfficesDataSet();
            }
            OfficesHierarchyDataSet officesHierarchy = DAInstance.GetOfficesHierarchy(null, officeRow.CodeID);

            var selected = from child in allOffices.CM_LookupOrganisationalUnitByType
                           join h in officesHierarchy.LookupOrganisationalUnitHierarchy on child.OrganisationalUnitTypeID equals h.OrganisationalUnitTypeID
                           join parent in allOffices.CM_LookupOrganisationalUnitByType on (h.IsParentIDNull() ? -1 : h.ParentID) equals parent.OrganisationalUnitTypeID
                           where child.OrganisationalUnitTypeID == officeTypeID
                           orderby parent.OrganisationalUnitName
                           select parent.OrganisationalUnitTypeID;

            OfficesDataSet ds = ImportOfficesData(selected.ToList<int>(), allOffices);

            officesHierarchy.Dispose();
            return ds;

        }        


        private static OfficesDataSet ImportOfficesData(List<int> typeIDs, OfficesDataSet allOffices)
        {
            OfficesDataSet ds = new OfficesDataSet();

            //var selected = from unit in allOffices.CM_LookupOrganisationalUnit
            //               join type in
            //                   (from t in allOffices.CM_LookupOrganisationalUnitByType
            //                    where Contains(typeIDs,t.OrganisationalUnitTypeID)
            //                    select t) on unit.OrganisationalUnitID equals type.OrganisationalUnitID
            //               //join detail in allOffices.CM_LookupOrganisationalUnitDetails on unit.OrganisationalUnitID equals detail.OrganisationalUnitID 
            //               into temp
            //               from y in temp.DefaultIfEmpty()
            //               select new DataRow[] { unit, type, y };

            var selected = from unit in allOffices.CM_LookupOrganisationalUnit
                           join type in allOffices.CM_LookupOrganisationalUnitByType on unit.OrganisationalUnitID equals type.OrganisationalUnitID
                           where typeIDs.Contains<int>(type.OrganisationalUnitTypeID)
                           select new DataRow[] { unit, type };

            ds.BeginInit();
            foreach (var rows in selected)
            {
                if (ds.CM_LookupOrganisationalUnit.FindByOrganisationalUnitID(rows[0]["OrganisationalUnitID"].ToString())==null)
                {
                    ds.CM_LookupOrganisationalUnit.ImportRow(rows[0]);
                  
                }
                ds.CM_LookupOrganisationalUnitByType.ImportRow(rows[1]);
                //if (rows[2]!=null) ds.CM_LookupOrganisationalUnitDetails.ImportRow(rows[2]);
            }
            ds.EndInit();

            return ds;
        }

        #endregion

        /// <summary>
        /// Get all messages with culture name
        /// </summary>
        /// <param name="latestUpdatedTime"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Infrastructure.Interface.Constants.FunctionNames.FrameworkModuleID, FunctionID = HiiP.Infrastructure.Interface.Constants.FunctionNames.GetAllMessagesFunctionName)]
        public DataTable GetMessages(DateTime latestUpdatedTime)
        {
            return DAInstance.GetMessages( latestUpdatedTime);
        }

        public bool IsActiveUser(string userName)
        {
            return this.DAInstance.IsActiveUser(userName);
        }


    }
}

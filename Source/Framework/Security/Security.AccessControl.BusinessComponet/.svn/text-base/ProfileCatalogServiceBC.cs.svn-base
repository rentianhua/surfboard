#region Copyright(C) 2008-2009 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008-2009 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :    Access Control/Business Component
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 17/07/2009/Li Hu Sheng
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Text;

using HiiP.Framework.Common;
using HiiP.Framework.Common.InstanceBuilders;

using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.AccessControl.DataAccess;
using NCS.IConnect.Security;
using HiiP.Framework.Common.ApplicationContexts;

namespace HiiP.Framework.Security.AccessControl.BusinessComponent
{
    public class ProfileCatalogServiceBC : HiiPBusinessComponentBase
    {
        private ProfileCatalogServiceDA _profileCatelogServiceDA =
            InstanceBuilder.Wrap<ProfileCatalogServiceDA>(new ProfileCatalogServiceDA());

        /// <summary>
        /// Get all necessary functions including function and dependencies
        /// </summary>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
         FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.ProfileCatalogGetAvailableFunctionsFunctionID)]
        public List<string> GetAvailableFunctions()
        {
            return _profileCatelogServiceDA.GetAvailableFunctions();
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
         FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.ProfileCatalogGetAvailableFunctionsFunctionID)]
        public List<string> GetUIDependencyFunctions()
        {
            // Get all UI dependency actions
            int dependencyType = 0; //dependencyType=UI 
            return _profileCatelogServiceDA.GetFilterActionsByDependencyType(new List<string>(BusinessActions.GetActionsForUser(AppContext.Current.UserName)), dependencyType);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
         FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.ProfileCatalogGetAvailableFunctionsFunctionID)]
        public List<string> GetActionsForUser(string userName)
        {
            return _profileCatelogServiceDA.GetActionsForUser(userName);
        }
    }
}

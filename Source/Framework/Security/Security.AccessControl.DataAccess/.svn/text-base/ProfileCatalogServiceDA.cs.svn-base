#region Copyright(C) 2008-2009 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008-2009 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :    Access Control/Data Acess
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 17/07/2009/Li Hu Sheng
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Common.Interface;

using NCS.IConnect.Helpers.Data;
using NCS.IConnect.Security;

namespace HiiP.Framework.Security.AccessControl.DataAccess
{
    public class ProfileCatalogServiceDA : HiiPDataAccessBase
    {
        /// <summary>
        /// Get all necessary functions including function and dependencies
        /// </summary>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
         FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.ProfileCatalogGetAvailableFunctionsFunctionID)]
        public List<string> GetAvailableFunctions()
        {
            // 1. Get all user actions
            string[] userActions = BusinessActions.GetActionsForUser(AppContext.Current.UserName);
            List<string> allUserActions = userActions.ToList();  // (*)

            // 2. Get all UI dependency actions
            // Filter actions to get available dependency user actions
            List<string> availableDependencyUserActions = GetFilterActionsByDependencyType(allUserActions, 0);

            // 3. Merge all user actions
            foreach (string aUAction in allUserActions)
            {
                availableDependencyUserActions.Add(aUAction);
            }

            return availableDependencyUserActions;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
      FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.ProfileCatalogGetAvailableFunctionsFunctionID)]
        public List<string> GetActionsForUser(string userName)
        {
            string AppName = NCS.IConnect.Security.BusinessActions.ApplicationName;
            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_IC_ACTIONS_GET_FOR_USER");
            helper.AssignParameterValues(
                command,
                AppName,
                userName             
                );
            helper.Fill(dt, command);

            var list = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(row["ACTION_CODE"].ToString());
            }
            return list;
        }

        /// <summary>
        /// Get specific type dependency actions
        /// </summary>
        /// <param name="allUserActions">all user actions</param>
        /// <param name="dependencyType">dependency type</param>
        /// <returns>dependency action list</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
     FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.ProfileCatalogGetAvailableFunctionsFunctionID)]
        public List<string> GetFilterActionsByDependencyType(List<string> allUserActions, int dependencyType)
        {
            var dt = new AuthorizationDataSet.ActionDependenciesDataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_IC_ACTIONS_GET_DEPENDENCIES_BY_TYPE");
            helper.AssignParameterValues(
                command,
                dependencyType             //dependency actions
                );
            helper.Fill(dt, command);
            List<string> availableDependencyUserActions = new List<string>();
            if (dt.Rows.Count > 0)
            {
                var actions = from a in allUserActions
                              join b in dt on a equals b.ACTION_CODE
                              select a;

                // foreach (DataRow row in dt.Rows)
                foreach (string explicitAction in actions)
                {
                    // FindDependencyActionInDepth(ref availableDependencyUserActions, allUserActions, row["ACTION_CODE"].ToString(), dt);
                    FindDependencyActionInDepth(ref availableDependencyUserActions, allUserActions, explicitAction, dt);
                }

            }

            return availableDependencyUserActions.Distinct().ToList();
        }

        /// <summary>
        /// Find dependency actions
        /// </summary>
        /// <param name="availableDependencyUserActions">availableDependencyUserActions</param>
        /// <param name="allUserActions">allUserActions</param>
        /// <param name="currentAction">currentAction</param>
        /// <param name="dt">dt</param>
        private void FindDependencyActionInDepth(ref List<string> availableDependencyUserActions, List<string> allUserActions, string currentAction, AuthorizationDataSet.ActionDependenciesDataTable dt)
        {
            foreach (var row in dt)
            {
                var actionCode = row.ACTION_CODE;
                var dependentActionCode = row.DEPENDENCY_ACTION_CODE;
                if (actionCode == currentAction &&
                    !availableDependencyUserActions.Contains(dependentActionCode) &&
                    !allUserActions.Contains(dependentActionCode))
                {
                    availableDependencyUserActions.Add(dependentActionCode);
                    FindDependencyActionInDepth(ref availableDependencyUserActions, allUserActions, dependentActionCode, dt);
                }
            }
        }

    }
}

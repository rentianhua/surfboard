#region Copyright(C) 2009 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008-2009 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :    Access Control/Data Acess
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 14/12/2009/Jiang Jin Nan
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiiP.Framework.Common.Server;
using System.Data.Common;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Common.Server.Constants;
using System.Diagnostics;

namespace HiiP.Framework.Security.AccessControl.DataAccess
{
    /// <summary>
    /// The Data Access for the authorization matching the combination of view type and view status..
    /// </summary>
    public class ViewAuthorizationDA : HiiPDataAccessBase
    {
        /// <summary>
        /// Tries the get action code.
        /// </summary>
        /// <param name="viewType">The full name of view type.</param>
        /// <param name="viewStatus">The view status.</param>
        /// <param name="actionCode">The action code matching the combination of view type and view status.</param>
        /// <returns>A <see cref="Boolean"/> value indicating if the view exists in the mapping table.</returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.AdminModuleID,
   FunctionID = FunctionNames.GetActionCodeFunctionID)]
        public bool TryGetActionCode(string viewType, string viewStatus, out string actionCode)
        {
            DbCommand command = this.Helper.BuildDbCommand("P_SS_GetActionCodeByViewType");
            this.Helper.AssignParameterValues(command, viewType, viewStatus);
            this.Helper.ExecuteNonQuery(command);
            actionCode = this.Helper.GetParameterValue(command, "actionCode") as string;
            actionCode = actionCode ?? string.Empty;           
            return (bool)this.Helper.GetParameterValue(command, "viewExists");
        }
    }
}

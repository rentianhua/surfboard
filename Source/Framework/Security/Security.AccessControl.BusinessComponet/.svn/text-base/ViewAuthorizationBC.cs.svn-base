#region Copyright(C) 2009 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008-2009 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :    Access Control/Business Component
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
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Common.Server.Constants;
using HiiP.Framework.Security.AccessControl.DataAccess;
using HiiP.Framework.Common.InstanceBuilders;

namespace HiiP.Framework.Security.AccessControl.BusinessComponent
{
    /// <summary>
    /// The Business Component of Authorization.
    /// </summary>
   public  class ViewAuthorizationBC:HiiPBusinessComponentBase
    {
       private readonly ViewAuthorizationDA _da = InstanceBuilder.CreateInstance<ViewAuthorizationDA>();

       /// <summary>
        /// Tries the get action code.
        /// </summary>
        /// <param name="viewType">The full name of view type.</param>
        /// <param name="viewStatus">The view status.</param>
        /// <param name="actionCode">The action code matching the combination of view type and view status.</param>
        /// <returns>A <see cref="Boolean"/> value indicating if the view exists in the mapping table.</returns>
       [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.AdminModuleID,
       FunctionID = FunctionNames.GetActionCodeFunctionID)]
       public bool TryGetActionCode(string viewType, string viewStatus, out string actionCode)
       {
           if (string.IsNullOrEmpty(viewType))
           {
               throw new ArgumentNullException("viewType");
           }

           if (string.IsNullOrEmpty(viewStatus))
           {
               throw new ArgumentNullException("viewStatus");
           }
           return this._da.TryGetActionCode(viewType, viewStatus, out actionCode);
       }
    }
}

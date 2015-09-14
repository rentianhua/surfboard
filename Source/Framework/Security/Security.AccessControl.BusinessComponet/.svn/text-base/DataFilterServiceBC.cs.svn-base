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
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Security.AccessControl.DataAccess;

namespace HiiP.Framework.Security.AccessControl.BusinessComponent
{
    public class DataFilterServiceBC : HiiPBusinessComponentBase
    {
        private DataFilterServiceDA _dataFilterServiceDA =
            InstanceBuilder.Wrap<DataFilterServiceDA>(new DataFilterServiceDA());

        /// <summary>
        /// Gets the data filter values.
        /// </summary>
        /// <param name="functionID">The function ID.</param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
   FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.GetAllDataFilterValueFunctionID)]
        public List<DataFilterValueEntity> GetDataFilterValues(string functionID)
        {
            return _dataFilterServiceDA.GetDataFilterValues(functionID);
        }
        /// <summary>
        /// Gets the data filter values by data filter.
        /// </summary>
        /// <param name="dataFilter">The data filter.</param>
        /// <param name="functionID">The function ID.</param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
   FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.GetValueOfDataFilterFunctionID)]
        public List<DataFilterValueEntity> GetDataFilterValuesByDataFilter(string dataFilter, string functionID)
        {
            return _dataFilterServiceDA.GetDataFilterValuesByDataFilter(dataFilter, functionID);
        }


        /// <summary>
        /// Lists the users for a specified data filter Value and a function ID.
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="dataFilterId"></param>
        /// <param name="dataFilterValueId"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.GetUsersWithFunctionAndDataFilter)]
        public string[] GetUsersWithFunctionAndDataFilter(string functionId, string dataFilterId, string dataFilterValueId)
        {
            return _dataFilterServiceDA.GetUsersWithFunctionAndDataFilter(functionId, dataFilterId,dataFilterValueId);
        }
     
    }
}

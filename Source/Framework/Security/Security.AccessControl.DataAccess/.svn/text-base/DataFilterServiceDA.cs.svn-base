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

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Infrastructure.Interface.BusinessEntities;

using NCS.IConnect.Helpers.Data;
using NCS.IConnect.Security;

namespace HiiP.Framework.Security.AccessControl.DataAccess
{
    public class DataFilterServiceDA : HiiPDataAccessBase
    {
        /// <summary>
        /// Get user's available data filter values by function id
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
  FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.GetAllDataFilterValueFunctionID)]
        public List<DataFilterValueEntity> GetDataFilterValues(string functionID)
        {
            return GetDataFilterValuesByDataFilter(string.Empty, functionID);
        }

        /// <summary>
        /// get logon user's available data filter values by function id and data filter
        /// </summary>
        /// <param name="dataFilter"></param>
        /// <param name="functionID"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
   FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.GetValueOfDataFilterFunctionID)]
        public List<DataFilterValueEntity> GetDataFilterValuesByDataFilter(string dataFilter, string functionID)
        {
            // 1. Get available user data filter values from database.
            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_IC_DATA_FILTER_VALUES_GET_BY_FUNCTION");
            helper.AssignParameterValues(
                command,
                ExtendedMembership.ApplicationName,
                AppContext.Current.UserName,
                dataFilter,
                functionID
                );
            helper.Fill(dt, command);

            // 2. Fill available data filter values into collcection (List<DataFilterValues>)
            List<DataFilterValueEntity> availableDataFilterValues = new List<DataFilterValueEntity>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    availableDataFilterValues.Add(
                        new DataFilterValueEntity(
                            functionID,
                            row["DATA_FILTER"].ToString(),
                            row["DATA_FILTER_VALUE"].ToString(),
                            row["DATA_FILTER_ID"].ToString(),
                            row["DATA_FILTER_VALUE_ID"].ToString()
                            ));
                }
            }

            return availableDataFilterValues;
        }


        /// <summary>
        /// Lists the users for a specified data filter Value and a function ID.
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="dataFilterId"></param>
        /// <param name="dataFilterValueId"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Common.Server.Constants.FunctionNames.FrameworkModuleID,
FunctionID = HiiP.Framework.Common.Server.Constants.FunctionNames.GetUsersWithFunctionAndDataFilter)]
        public string[] GetUsersWithFunctionAndDataFilter(string functionId, string dataFilterId,string dataFilterValueId)
        {
            // 1. Get available users from database.
            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_IC_DATA_FILTER_VALUES_GET_USERS_BY_FUNCTION");
            helper.AssignParameterValues(
                command,
                ExtendedMembership.ApplicationName,
                functionId,
                dataFilterId,
                dataFilterValueId
                );
            helper.Fill(dt, command);

            // 2. Fill available username values into collcection (List<string>)
            List<string> userNames = new List<string>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    userNames.Add(row["USER_NAME"].ToString()  );
                }
            }

            return userNames.ToArray();
        }
    }
}

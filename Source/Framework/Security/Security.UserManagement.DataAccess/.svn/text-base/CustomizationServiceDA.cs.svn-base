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
using System.Data;
using System.Data.Common;
using System.Xml.Linq;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.Interface.Constants;



namespace HiiP.Framework.Security.UserManagement.DataAccess
{
    public class CustomizationServiceDA : HiiPDataAccessBase
    {

        #region Customization Configuration
        /// <summary>
        /// Get the customization configuration info.
        /// </summary>
        /// <param name="userID">The specfied user ID.</param>
        /// <param name="menuOriginalVersion"></param>
        /// <param name="dockOriginalVersion"></param>
        /// <param name="homePageOriginalVersion"></param>
        /// <param name="favouritesOriginalVersion"></param>
        /// <param name="todoListOriginalVersion"></param>
        /// <param name="helpFlagOriginalVersion"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.SecurityModuleID, FunctionID = FunctionNames.GetCustomizationFunctionID)]
        public DataTable GetCustomizationConfigurationInfo(string userID,
            int menuOriginalVersion,
            int  dockOriginalVersion,
            int  homePageOriginalVersion, 
            int  favouritesOriginalVersion,
            int  todoListOriginalVersion,
            int  helpFlagOriginalVersion )
        {

            DataTable dt = new DataTable();
            Helper.Fill(dt, "dbo.P_SS_Customization_S", userID,
                menuOriginalVersion,
                dockOriginalVersion,
                homePageOriginalVersion,
                favouritesOriginalVersion,
                todoListOriginalVersion,
                helpFlagOriginalVersion);

            return dt;

        }

        /// <summary>
        /// Update the customization configuration infomation.
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="menuElement"></param>
        /// <param name="dockElement"></param>
        /// <param name="homePageElement"></param>
        /// <param name="favouritesMenuElement"></param>
        /// <param name="todoListElement"></param>
        /// <param name="helpElement"></param>
        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = FunctionNames.SecurityModuleID, FunctionID = FunctionNames.UpdateCustomizationFunctionID)]
        public void UpdateCustomizationConfigurationInfo(bool isNew, XElement menuElement,
            XElement dockElement, XElement homePageElement,
            XElement favouritesMenuElement, XElement todoListElement,
            XElement helpElement)
        {
            const string VersionNoKey = "version";
            DbCommand command;
            string transactionID = NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId;
            var helpFlagString = (helpElement == null) ? null : helpElement.Value;
            int helpVersion = -1;
            if (helpElement != null)
            {
                var attr = helpElement.Attribute(VersionNoKey);
                if (attr != null)
                {
                    if (!int.TryParse(attr.Value , out helpVersion))
                    {
                        helpVersion = -1;
                    }
                }
            }

            if (!isNew)
            {
                command = Helper.BuildDbCommand("dbo.P_SS_Customization_U");

            }
            else
            {
                command = Helper.BuildDbCommand("dbo.P_SS_Customization_I");
            }

            Helper.AssignParameterValues(
                command,
                AppContext.Current.UserID,
                (menuElement == null) ? null : menuElement.ToString(),
                (dockElement == null) ? null : dockElement.ToString(),
                (homePageElement == null) ? null : homePageElement.ToString(),
                (favouritesMenuElement == null) ? null : favouritesMenuElement.ToString(),
                (todoListElement == null) ? null : todoListElement.ToString(),
                (helpElement == null) ? null:((object)((helpFlagString == "0") ? 0 : 1)),
                transactionID,
                AppContext.Current.UserName,
                DateTime.Now,
                helpVersion
              );
            if (command != null)
            {
                Helper.ExecuteNonQuery(command);
            }

        }
        #endregion

    }
}

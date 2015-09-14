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

using System.Data;
using System.Xml.Linq;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.DataAccess;
using HiiP.Framework.Security.UserManagement.Interface.Constants;

namespace HiiP.Framework.Security.UserManagement.BusinessComponent
{
    public class CustomizationServiceBC : HiiPBusinessComponentBase
    {
        private CustomizationServiceDA _customizationServiceDA;

        private CustomizationServiceDA DAInstance
        {
            get
            {
                if (_customizationServiceDA == null)
                {
                    _customizationServiceDA = InstanceBuilder.Wrap<CustomizationServiceDA>(new CustomizationServiceDA());
                }

                return _customizationServiceDA;
            }
        }

        /// <summary>
        /// Get the customization configuration info.
        /// </summary>
        /// <param name="menuOriginalVersion"></param>
        /// <param name="dockOriginalVersion"></param>
        /// <param name="homePageOriginalVersion"></param>
        /// <param name="favouritesOriginalVersion"></param>
        /// <param name="todoListOriginalVersion"></param>
        /// <param name="helpFlagOriginalVersion"></param>
        /// <returns></returns>
        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.SecurityModuleID,
   FunctionID = FunctionNames.GetCustomizationFunctionID)]
        public DataTable GetCustomizationConfigurationInfo(int menuOriginalVersion,
            int dockOriginalVersion,
            int homePageOriginalVersion,
            int favouritesOriginalVersion,
            int todoListOriginalVersion,
            int helpFlagOriginalVersion)
        {
            return DAInstance.GetCustomizationConfigurationInfo(AppContext.Current.UserID,
                menuOriginalVersion,
                dockOriginalVersion,
                homePageOriginalVersion,
                favouritesOriginalVersion,
                todoListOriginalVersion,
                helpFlagOriginalVersion);
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
        [MonitoringCallHandler(ComponentType.BusinessComponent, ModuleID = FunctionNames.SecurityModuleID,
           FunctionID = FunctionNames.UpdateCustomizationFunctionID)]
        public void UpdateCustomizationConfigurationInfo(bool isNew, XElement menuElement,
            XElement dockElement, XElement homePageElement,
            XElement favouritesMenuElement, XElement todoListElement,
            XElement helpElement)
        {
            DAInstance.UpdateCustomizationConfigurationInfo(isNew, menuElement, dockElement, homePageElement, favouritesMenuElement, todoListElement, helpElement);
        }

    }
}

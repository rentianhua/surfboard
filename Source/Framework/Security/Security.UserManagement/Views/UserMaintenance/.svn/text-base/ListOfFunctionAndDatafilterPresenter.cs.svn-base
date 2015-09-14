using System;
using System.Windows.Forms;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using Microsoft.Practices.CompositeUI.EventBroker;
using HiiP.Framework.Security.UserManagement.BusinessEntity;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class ListOfFunctionAndDataFilterPresenter : Presenter<IListOfFunctionAndDataFilter>
    {
        public override AppTitleData GetAppTitle()
        {
            return new AppTitleData(
                Key,
                FunctionNames.ListOfFunctionAndDataFilterFunctionScreenID
                );
        }

        #region Event Subscription

        [EventSubscription(HiiP.Framework.Security.UserManagement.Constants.EventTopicNames.UserToAdd, ThreadOption.UserInterface)]
        public void UserToAddHandler(object sender, EventArgs<string> e)
        {
            try
            {
                View.ViewActionCodeAndDFList();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
            }
            
        }

        #endregion

        #region Business Logic
        
        /// <summary>
        /// search user list by rolename
        /// </summary>
        /// <param name="roleName">roleNameToMatch</param>
        /// <returns>UserEntity[]</returns>
        internal FunctionAndDataFilterEntity[] GetActionCodeAndDFListByRoleName(string roleName)
        {
            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.RoleModuleID, FunctionNames.ListOfFunctionAndDataFilterFunctionID, ComponentType.Screen))
            {
                using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
                {
                    return proxy.GetActionCodeAndDFListByRoleName(roleName);
                }
            }
        }
        #endregion
    }
}

        

 
using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI.EventBroker;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Security.SessionManagement.Constants;
using HiiP.Framework.Security.SessionManagement.Interface.Services;
using HiiP.Framework.Security.AccessControl.Interface;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.SessionManagement.Interface.Constants;
using HiiP.Framework.Security.AccessControl.BusinessEntity;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Security.SessionManagement
{
    public partial class SessionManagementPresenter : Presenter<ISessionManagementView>
    {
        // define the interface of service
        ISessionManagementService _sessionManagementService;

        // Injection Constructor
        [InjectionConstructor]
        public SessionManagementPresenter
            (
            [ServiceDependency] ISessionManagementService sessionManagementService
			)
		{
            _sessionManagementService = sessionManagementService;
		}

        public override AppTitleData GetAppTitle()
        {
            AppTitleData appTitleData = new AppTitleData(
                FunctionNames.SessionManagementFunctionName,
                FunctionNames.SessionManagementFunctionName,
                FunctionNames.SessionManagementScreenID);

            return appTitleData;
        }

        protected override void InitData()
        {
            View.AccessControls(UIAccessControl.IsAuthorised(FunctionNames.KillSessionsFunctionID));
            View.ShowSessionList(null);
        }

        #region Event Subscription

        [EventSubscription(EventTopicNames.FilterSession, ThreadOption.UserInterface)]
        public void OnFilterSession(object sender, EventArgs<SessionCriteria> eventArgs)
        {
            try
            {
                // Refresh Session List
                View.ShowSessionList(eventArgs.Data);
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
      

        public SessionInfo[] GetActiveSessions(SessionCriteria criteria)
        {
            Guid activityID = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            using (new MonitoringTracer(activityID, FunctionNames.AdminModuleID, FunctionNames.GetActiveSessionsFunctionID, ComponentType.Screen))
            {               
                return this._sessionManagementService.GetActiveSessions(criteria);
            }
        }        

        public void KillSessions(Guid[] sessionIds)
        {
            Guid activityID = HiiP.Framework.Logging.Library.Utility.SetContextValues();
              using (new MonitoringTracer(activityID, FunctionNames.AdminModuleID, FunctionNames.KillSessionsFunctionID, ComponentType.Screen))
              {

                  this._sessionManagementService.KillSessions(sessionIds);
              }
        }

        ///// <summary>
        ///// Filter session
        ///// </summary>
        //public void ShowFilterSessionView()
        //{
        //    if (WorkItem.Items.Contains("FilterSession"))
        //    {
        //        WorkItem.Items.Remove(WorkItem.Items.Get("FilterSession"));
        //    }
        //    WorkItem.Items.Add(new object(), "FilterSession");
        //    FilterSessionView filterSessionView = WorkItem.SmartParts.AddNew<FilterSessionView>();
        //    SmartPartInfo spi = new SmartPartInfo("Filter Setting", "Filter Setting");
        //    WorkItem.Workspaces[Infrastructure.Interface.Constants.WorkspaceNames.ModalWindows].Show(filterSessionView, spi);
        //}

        #endregion
    }
}


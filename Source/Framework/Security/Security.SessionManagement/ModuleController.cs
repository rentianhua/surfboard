using System;

using HiiP.Framework.Security.SessionManagement.Constants;
using HiiP.Infrastructure.Interface;

using Microsoft.Practices.CompositeUI.Commands;
using HiiP.Framework.Security.SessionManagement.Interface.Constants;
using HiiP.Infrastructure.Interface.BusinessEntities;
using System.Diagnostics.CodeAnalysis;

namespace HiiP.Framework.Security.SessionManagement
{
    public class ModuleController : WorkItemController
    {
        //private SessionManagement _sessionManagement;

        private void ExtendMenu()
        {
            this.LoadAdministrationMenu();


            // add a root menu item: Administration
            ActionCatalogService.Execute(FunctionNames.SessionManagementFunctionID, WorkItem, this, null);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "target")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "caller")]
        [Action(FunctionNames.SessionManagementFunctionID)]
        public void ShowMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "SessionMaintenance");
        }


        public override void Run()
        {
            ExtendMenu();
        }

       

        #region Session Maintenance

        [CommandHandler(CommandNames.SessionMaintenance)]
        public void ShowSessionMaintenanceView(object sender, EventArgs e)
        {
            ShowViewInWorkspace<SessionManagementView>(FunctionNames.SessionManagementFunctionID);
        }

        #endregion
    }
}
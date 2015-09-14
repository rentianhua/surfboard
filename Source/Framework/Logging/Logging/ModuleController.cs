#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Yang Jian Hua
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using HiiP.Framework.ExceptionHandling;
using HiiP.Framework.Logging.Constants;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI.Commands;

namespace HiiP.Framework.Logging
{
    public class ModuleController : WorkItemController
    {

        public override void Run()
        {
            AddServices();
            ExtendMenu();
        }

        private void AddServices()
        {
            //WorkItem.Services.AddNew<LoggingViewProxy, ILoggingViewService>();
        }

        private void ExtendMenu()
        {
            this.LoadAdministrationMenu();


            // add a menu item: Logging
            ActionCatalogService.Execute(FunctionNames.AuditLogModuleID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.AuditLogViewFunctionID, WorkItem, this, null);

            ActionCatalogService.Execute(FunctionNames.ExceptionLogModuleID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.ExceptionLogViewFunctionID, WorkItem, this, null);

            ActionCatalogService.Execute(FunctionNames.LoggingModuleID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.InstrumentationFunctionID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.MonitoringFunctionID, WorkItem, this, null);
            ActionCatalogService.Execute(FunctionNames.UsageFunctionID, WorkItem, this, null);

        }


        [Action(FunctionNames.LoggingModuleID)]
        public void ShowLoggingRootMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Logging");
        }

        [Action(FunctionNames.InstrumentationFunctionID)]
        public void ShowInstrumentationViewMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Instrumentation View");
        }

        [Action(FunctionNames.MonitoringFunctionID)]
        public void ShowMonitoringViewMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Performance Monitoring View");
        }

        [Action(FunctionNames.UsageFunctionID)]
        public void ShowUsageViewMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Usage View");
        }



        [Action(FunctionNames.ExceptionLogModuleID)]
        public void ShowExceptionLogRootMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "ExceptionHandling");
        }

        [Action(FunctionNames.ExceptionLogViewFunctionID)]
        public void ShowExceptionLogViewMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Log View");
        }

        [Action(FunctionNames.AuditLogModuleID)]
        public void ShowAuditLogRootMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Audit Trail");
        }

        [Action(FunctionNames.AuditLogViewFunctionID)]
        public void ShowAuditLogViewMenu(object caller, object target)
        {
            UIElementBuilder.BuildMenuItem(WorkItem, "Audit Log View");
        }

        [CommandHandler(CommandNames.InstrumentationView)]
        public void ShowInstrumentationView(object sender, EventArgs e)
        {
            ShowViewInWorkspace<InstrumentationView>(FunctionNames.InstrumentationFunctionID);
        }


        [CommandHandler(CommandNames.PerformanceMonitoringView)]
        public void ShowPerformanceMonitoringView(object sender, EventArgs e)
        {
            ShowViewInWorkspace<PerformanceMonitoringView>(FunctionNames.MonitoringFunctionID);
        }

        [CommandHandler(CommandNames.UsageView)]
        public void ShowUsageView(object sender, EventArgs e)
        {
            ShowViewInWorkspace<UsageLogView>(FunctionNames.UsageFunctionID);
        }


        [CommandHandler(CommandNames.ExceptionLogView)]
        public void ShowExceptionLogView(object sender, EventArgs e)
        {
            ShowViewInWorkspace<ExceptionLogView>(FunctionNames.ExceptionLogViewFunctionID);
        }

        [CommandHandler(CommandNames.AuditLogView)]
        public void ShowAuditLogView(object sender, EventArgs e)
        {
            ShowViewInWorkspace<AuditLogView>(FunctionNames.AuditLogViewFunctionID);
        }

    }
}

#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :   Settings
// COMPONENT DESC    :  
//
// CREATED DATE/BY   : 06/11/2008/Yang Jian Hua
//
// REVISION HISTORY  : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Messaging;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Settings.ServiceProxy;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;
using FunctionNames = HiiP.Framework.Settings.Interface.Constants.FunctionNames;
using ViewStatus = HiiP.Framework.Settings.Interface.Constants.ViewStatus;

namespace HiiP.Framework.Settings
{
    public partial class LoggingFilterMaintainPresenter : Presenter<ILoggingFilterMaintain>
    {
        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();

            bool usageTurnOn = RefreshDataGrid();
            View.SetUsage(usageTurnOn);

        }

        internal bool RefreshDataGrid()
        {
            bool usageTurnOn = false;
            LoggingFilterDS ds = GetLoggingFilterByCategory(string.Empty);

            //Bind the usage
            LoggingFilterDS.T_IC_LOGGING_FILTERRow[] rows =
                ds.T_IC_LOGGING_FILTER.Select("Category='Usage'") as LoggingFilterDS.T_IC_LOGGING_FILTERRow[];

            if (rows == null || rows.Length == 0)
            {
                View.BindGrid(ds);
                return false;
            }
            if (rows.Length != 0)
            {
                usageTurnOn = rows[0].FLAG == 1;
            }

            //Bind instrumentation and performance logging list
            ds.T_IC_LOGGING_FILTER.RemoveT_IC_LOGGING_FILTERRow(rows[0]);

            View.BindGrid(ds);
            return usageTurnOn;
        }

        internal LoggingFilterDS GetLoggingFilterByCategory(string category)
        {
            LoggingFilterDS ds ;
             Guid id = Utility.SetContextValues();
             using (new MonitoringTracer(id, FunctionNames.SettingsModuleID, FunctionNames.RetrieveLoggingFilterID, ComponentType.Screen))
             {
                 using (SettingsProxy proxy = new SettingsProxy())
                 {
                     ds = proxy.RetrieveLoggingFilter(category);
                 }
             }
            
            return ds;
        }

        internal void ShowInstrumentationFilterView(string key, object flag)
        {
            ViewParameter parameter = new ViewParameter("AddInstrumentationFilter");
            parameter.WorkspaceName = WorkspaceNames.ModalWindows;
            parameter.AppTitleData = new AppTitleData(FunctionNames.InstrumentationFilterAddName,
                FunctionNames.InstrumentationFilterAddScreenID);

            if (key == null)
            {
                parameter.CurrentViewStatus = ViewStatus.Add;
            }
            else
            {
                parameter.CurrentViewStatus = ViewStatus.Update;
            }
            
            parameter.Key = key;
            parameter.Data = flag;
            ShowViewInWorkspace<InstrumentationFilterView>(parameter);
        }

        internal void UpdateLoggingFilter(LoggingFilterDS ds, bool isFromUsage)
        {
            if (ds==null)
            {
                return;
            }
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.SettingsModuleID, FunctionNames.UpdateLoggingFilterID, ComponentType.Screen))
            {
                using (SettingsProxy proxy = new SettingsProxy())
                {
                    proxy.UpdateLoggingFilter(ds);
                }
            }
            OnUpdateStatusBarMessage(Messages.General.GEI002.Format());
            if (!isFromUsage)
            {
                RefreshDataGrid();
            }
        }
    }
}


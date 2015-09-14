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
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Settings.ServiceProxy;
using Microsoft.Practices.CompositeUI.EventBroker;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Messaging;

namespace HiiP.Framework.Settings
{
    public partial class InstrumentationFilterViewPresenter : Presenter<IInstrumentationFilterView>
    {
        [EventPublication(EventTopicNames.AddInstrumentationFilter, PublicationScope.Global)]
        public event EventHandler<EventArgs> AddInstrumentationFilter;

        public void OnAddInstrumentationFilter(EventArgs e)
        {
            if (null != AddInstrumentationFilter)
                AddInstrumentationFilter(this, e);
        }

        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        internal bool IsUserExists(string category, string userId)
        {
            using (SettingsProxy proxy = new SettingsProxy())
            {
                return proxy.IsUserExists(category, userId);
            }
        }

        internal Dictionary<string,string> GetAllUsers()
        {
            using (SettingsProxy proxy = new SettingsProxy())
            {
                return proxy.GetAllUsers();
            }
        }

        //internal LoggingFilterDS GetLoggingFilterByCategory(string category)
        //{
        //    LoggingFilterDS ds = new LoggingFilterDS();
        //    Guid id = Utility.SetContextValues();
        //    using (new MonitoringTracer(id, FunctionNames.SettingsModuleID, FunctionNames.RetrieveLoggingFilterID, ComponentType.Screen))
        //    {
        //        using (SettingsProxy proxy = new SettingsProxy())
        //        {
        //            ds = proxy.RetrieveLoggingFilter(category);
        //        }
        //    }

        //    return ds;
        //}

        internal void UpdateLoggingFilter(LoggingFilterDS ds)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.SettingsModuleID, FunctionNames.UpdateLoggingFilterID, ComponentType.Screen))
            {
                using (SettingsProxy proxy = new SettingsProxy())
                {
                    proxy.UpdateLoggingFilter(ds);
                }
            }
            OnUpdateStatusBarMessage(Messages.General.GEI002.Format());
            OnAddInstrumentationFilter(EventArgs.Empty);
        }
    }
}


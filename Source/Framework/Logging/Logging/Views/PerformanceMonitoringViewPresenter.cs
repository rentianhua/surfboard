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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Logging.Interface.Services;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Logging.ServiceProxy;

namespace HiiP.Framework.Logging
{
    public partial class PerformanceMonitoringViewPresenter : Presenter<IPerformanceMonitoringView>
    {
        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            base.OnCloseView();
        }

        public LoggingViewDataSet GetPerformanceData(DateTimeCompare timeEntity, string functionId, string componentName, string userName)
        {
            LoggingViewDataSet dataset;
            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();

            using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.MonitoringFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    dataset = proxy.RetrievePerformanceInformation(timeEntity, functionId, componentName, userName);
                }
            }

            return dataset;  
        }
    }
}


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
using System.Diagnostics;
using HiiP.Framework.Logging.Interface.Services;
using HiiP.Framework.Logging.Library.Constants;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Library;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using HiiP.Framework.Logging.ServiceProxy;

namespace HiiP.Framework.ExceptionHandling
{
    public partial class ExceptionLogViewPresenter : Presenter<IExceptionLogView>
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

        //public void ShowExceptionLog(DateTime startTime, DateTime endTime, string userName, string severity, string machineName, string logContent)
        //{
        //    _loggingViewService = WorkItem.Services.Get<ILoggingViewService>();
        //    View.LoggingViewData = _loggingViewService.RetrieveExceptionLog(startTime, endTime, userName, LoggingCategories.Exception, severity, machineName, logContent);
        //}

        internal LogIDPairEntity GetLogIDRangeByLogTime(DateTimeCompare timeEntity, string userName, string machineName)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, HiiP.Framework.Logging.Interface.Constants.FunctionNames.ExceptionLogModuleID, HiiP.Framework.Logging.Interface.Constants.FunctionNames.ExceptionLogViewFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    return proxy.GetLogIDRangeByLogTime(timeEntity, userName, machineName);
                }
            }
        }

        public LoggingViewDataSet GetExceptionLog(
            LogIDPairEntity logIDPair,
            DateTimeCompare timeEntity,
            string userName,
            string severity,
            string machineName,
            string logContent,
            string instanceID
            )
        {
            Guid id = Utility.SetContextValues();
            LoggingViewDataSet dataset;
            using (new MonitoringTracer(id, HiiP.Framework.Logging.Interface.Constants.FunctionNames.ExceptionLogModuleID, HiiP.Framework.Logging.Interface.Constants.FunctionNames.ExceptionLogViewFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    dataset = proxy.RetrieveExceptionLog(
                              logIDPair,
                              timeEntity,
                              userName,
                              LoggingCategories.Exception,
                              severity,
                              machineName,
                              logContent,
                              instanceID);
                }
            }

            return dataset;
        }

        //Unused method
        //internal LoggingViewDataSet GetLogById(string logId)
        //{
        //    using (LoggingViewProxy proxy = new LoggingViewProxy())
        //    {
        //        return proxy.GetLogsByID(logId);
        //    }
        //}

        internal void ShowExceptionDetailView(string logId)
        {
            ViewParameter parameter = new ViewParameter();
            parameter.CurrentViewStatus = ViewStatus.View;
            parameter.Key = logId;
            parameter.ViewId = string.Format("{0}.{1}", HiiP.Framework.Logging.Interface.Constants.FunctionNames.ExceptionLogViewFunctionID, logId);
            ShowViewInWorkspace<ExceptionLogDetailView>(parameter);
        }
    }

}


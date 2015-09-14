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
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;
using HiiP.Framework.Logging.Interface.Services;
using HiiP.Framework.Logging.Library.Constants;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Logging.ServiceProxy;

namespace HiiP.Framework.Logging
{
    public partial class InstrumentationViewPresenter : Presenter<IInstrumentationView>
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

        internal LogIDPairEntity GetLogIDRangeByLogTime(DateTimeCompare timeEntity)
        {
            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            using (new MonitoringTracer(id, HiiP.Framework.Logging.Interface.Constants.FunctionNames.LoggingModuleID, HiiP.Framework.Logging.Interface.Constants.FunctionNames.InstrumentationFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    return proxy.GetInstrumentationLogIDRangeByLogTime(timeEntity);
                }
            }
        }

        public LoggingViewDataSet GetInstrumentationData(LogIDPairEntity logIDPair,DateTimeCompare timeEntity, InstrumentationSearchCondition condition)
        {
            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            LoggingViewDataSet dataset;
            using (new MonitoringTracer(id, HiiP.Framework.Logging.Interface.Constants.FunctionNames.LoggingModuleID, HiiP.Framework.Logging.Interface.Constants.FunctionNames.InstrumentationFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    dataset = proxy.RetrieveInstrumentation(
                    logIDPair,timeEntity,
                    condition.UserName,
                    condition.IpAddress,
                    condition.ModuleId,
                    condition.FunctionId,
                    condition.ComponentName,
                    LoggingCategories.Monitoring,
                    condition.PCName);
                }
            }

            return dataset;
        }

        internal void ShowInstrumentationDetailView(LoggingViewDataSet.T_IC_LOGGING_LOGRow row)
        {
            ViewParameter parameter = new ViewParameter();
            parameter.CurrentViewStatus = ViewStatus.View;
            parameter.Key = row.LOG_ID.ToString();
            parameter.ViewId = string.Format("{0}.{1}", HiiP.Framework.Logging.Interface.Constants.FunctionNames.InstrumentationFunctionID, row.LOG_ID.ToString());
            parameter.Data = row;
            ShowViewInWorkspace<InstrumentationDetailView>(parameter);
        }
    }

    public class InstrumentationSearchCondition
    {
        public string UserName 
        {
            get;
            set;
        }

        public string IpAddress 
        {
            get;
            set;
        }

        public string ModuleId 
        {
            get;
            set;
        }

        public string FunctionId 
        {
            get;
            set;
        }

        public string ComponentName 
        {
            get;
            set;
        }

        public string PCName 
        {
            get;
            set;
        }
    }
}


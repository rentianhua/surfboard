#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==============================================================================
// Copyright(C) 2008 NCS Pte Ltd
//
// SYSTEM NAME			: HiiP
// COMPONENT ID			: HiiP.Framework.Settings
// COMPONENT DESC		: 
//
// CREATED DATE/BY	    : 15 Sep 2008 / Yang Jian Hua
//
// REVISION HISTORY     :
// DATE/BY  ISSUE#/SR#/CS/PM#/OTHERS    DESCRIPTION OF CHANGE
// 
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiiP.Framework.Common.ApplicationContexts.CallHandlers;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Settings.Interface.Services;
using HiiP.Framework.Settings.Interface;
using HiiP.Framework.Settings.BusinessEntity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Xml;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Framework.Logging.Library;
using System.Data;

namespace HiiP.Framework.Settings.ServiceProxy
{
    public class SettingsProxy : ServiceProxyBase<ISettingsService>, ISettingsService
    {
        protected SettingsProxy(string endpointName)
            : base(endpointName)
        { }

        public SettingsProxy()
        {
            base.WrapObject(new SettingsProxy(EndpointNames.SettingService));
        }

        #region ISettingsService Members

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
  FunctionID = FunctionNames.RetrieveLoggingFilterID)]
        public LoggingFilterDS RetrieveLoggingFilter(string category)
        {
            return Proxy.RetrieveLoggingFilter(category);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
          FunctionID = FunctionNames.UpdateLoggingFilterID)]
        public bool IsUserExists(string category, string userId)
        {
            return Proxy.IsUserExists(category, userId);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
  FunctionID = FunctionNames.UpdateLoggingFilterID)]
        public void UpdateLoggingFilter(LoggingFilterDS ds)
        {
            this.Proxy.UpdateLoggingFilter(ds);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
  FunctionID = FunctionNames.InstrumentationFilterAddID)]
        public Dictionary<string, string> GetAllUsers()
        {
            return this.Proxy.GetAllUsers();
        }


        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
FunctionID = FunctionNames.HomePageRefreshSettingsMaintenanceID)]
        public void SaveParameterValue(IList<ParameterEntity> parameters)
        {
            this.Proxy.SaveParameterValue(parameters);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
FunctionID = FunctionNames.MessageMaintenanceID)]
        public DataTable RetrieveMessages(string category, string severity, string messageValue)
        { 
            return this.Proxy.RetrieveMessages(category,severity,messageValue);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
FunctionID = FunctionNames.MessageDetailID)]
        public DataTable RetrieveMessage(string category, string id)
        { 
            return this.Proxy.RetrieveMessage(category,id);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
FunctionID = FunctionNames.MessageDetailID)]
        public void UpdateMessage(DataTable messages)
        {
            this.Proxy.UpdateMessage(messages);
        }
        #endregion

   }
}

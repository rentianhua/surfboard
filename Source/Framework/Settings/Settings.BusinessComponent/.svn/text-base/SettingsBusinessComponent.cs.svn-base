#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :   Settings/Business Component
// COMPONENT DESC    :  
//
// CREATED DATE/BY   : 06/11/2008/Yang Jian Hua
//
// REVISION HISTORY  : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using HiiP.Framework.Common.Server;
using HiiP.Framework.Settings.DataAccess;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Framework.Logging.Library;
using System.Data;

namespace HiiP.Framework.Settings.BusinessComponent
{
    public class SettingsBusinessComponent : HiiPBusinessComponentBase
    {
        private readonly SettingsDataAccess _settingsDA = InstanceBuilder.CreateInstance<SettingsDataAccess>();

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
   FunctionID = FunctionNames.RetrieveLoggingFilterID)]
        public LoggingFilterDS RetrieveLoggingFilter(string category)
        {
            return _settingsDA.RetrieveLoggingFilter(category);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
          FunctionID = FunctionNames.UpdateLoggingFilterID)]
        public bool IsUserExists(string category, string userId)
        {
            return _settingsDA.IsUserExists(category, userId);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
   FunctionID = FunctionNames.UpdateLoggingFilterID)]
        public void UpdateLoggingFilter(LoggingFilterDS ds)
        {
            _settingsDA.UpdateLoggingFilter(ds);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
   FunctionID = FunctionNames.MessageMaintenanceID)]
        public DataTable RetrieveMessages(string category, string severity, string messageValue)
        {
            return this._settingsDA.RetrieveMessages(category,severity,messageValue);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
   FunctionID = FunctionNames.MessageDetailID)]
        public DataTable RetrieveMessage(string category, string id)
        {
            return this._settingsDA.RetrieveMessage(category,id);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = FunctionNames.SettingsModuleID,
   FunctionID = FunctionNames.MessageDetailID)]
        public void UpdateMessage(DataTable messages)
        {
            _settingsDA.UpdateMessage(messages);
        }
    }
}

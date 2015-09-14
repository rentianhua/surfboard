#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// SYSTEM NAME      : Housing Integrated Information Program
// COMPONENT ID     : Settings/MessageMaintainPresenter
// COMPONENT DESC   :  
// CREATED DATE/BY  : 25/08/2008/Mei Bo
// REVISION HISTORY : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================
#endregion

namespace HiiP.Framework.Settings.Interface.Constants
{
    /// <summary>
    /// Constants for command names.
    /// </summary>
    public sealed class CommandNames : HiiP.Infrastructure.Interface.Constants.CommandNames
    {
        private CommandNames()
        {}
        public const string ShowCodeTable = "ShowCodeTableManager";
        public const string ShowMessage = "ShowMessageManager";
        public const string ShowMessageDetail = "ShowMessageDetail";

        public const string ShowLoggingFilterMaintain = "ShowLoggingFilterMaintain";
        public const string ShowLoggingSettingsMaintain = "ShowLoggingSettingsMaintain";
        public const string ShowExceptionSettingsMaintain = "ShowExceptionSettingsMaintain";
        public const string ShowBatchJobServiceSettingsMaintain = "ShowBatchJobServiceSettingsMaintain";
        public const string ShowHomePageRefreshSettingsMaintain = "ShowHomePageRefreshSettingsMaintain";
    }
}

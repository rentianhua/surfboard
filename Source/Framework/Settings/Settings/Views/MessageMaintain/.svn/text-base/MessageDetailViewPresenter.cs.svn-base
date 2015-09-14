#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// SYSTEM NAME      : Housing Integrated Information Program
// COMPONENT ID     : Settings/MessageMaintainPresenter
// COMPONENT DESC   :  
// CREATED DATE/BY  : 05/09/2008/Mei Bo
// REVISION HISTORY : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================
#endregion

using System;
using System.Collections.Generic;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI.EventBroker;
using NCS.IConnect.Messaging;
using HiiP.Framework.Messaging;
using System.Data;
using HiiP.Framework.Settings.ServiceProxy;
using HiiP.Framework.Logging.Library;

namespace HiiP.Framework.Settings
{
    public partial class MessageDetailViewPresenter : Presenter<IMessageDetailView>
    {
        private DataRow _message;
        #region EventPublication
        [EventPublication(EventTopicNames.MessageSaveEvent, PublicationScope.Global)]
        public event EventHandler<KeyValueEventArgs> MessageSaveEvent;

        protected virtual void OnMessageSaveEvent(KeyValueEventArgs eventArgs)
        {
            if (MessageSaveEvent != null)
            {
                MessageSaveEvent(this, eventArgs);
            }
        }
        #endregion

        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        protected override void InitData()
        {           
            base.InitData();
            View.BindSeverity(this.GetSeverity());
            //View.BindCategory(this.GetCategory());

            var key = this.Data as InternalMessageKey;
            if (key == null)
            {
                View.Authorize(false, false);
                return;
            }

            if (this.CurrentViewStatus == FunctionNames.ActionStatus.Update)
            {
                DataTable messageTable;
                Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
                using (new MonitoringTracer(id, FunctionNames.SettingsModuleID, FunctionNames.MessageDetailID, ComponentType.Screen))
                {
                    using (SettingsProxy proxy = new SettingsProxy())
                    {
                        messageTable = proxy.RetrieveMessage(key.Category,key.Id);
                    }
                }

                if (messageTable == null || messageTable.Rows.Count==0)
                {
                    View.Authorize(false, false);
                    return;
                }

                var message = messageTable.Rows[0];
                _message = message;
                View.Authorize(true, UIAccessControl.IsAuthorised(FunctionNames.MessageDetailID));
                View.BindUpdateData(message);
            }
        }

        /// <summary>
        /// Get severity
        /// </summary>
        /// <returns>string[]</returns>
        private string[] GetSeverity()
        {
            string[] severities = Enum.GetNames(typeof(MessageSeverity));
            List<string> severityList = new List<string>();
            severityList.AddRange(severities);

            severityList.RemoveAt(0);//Default
            return severityList.ToArray();
        }

        /// <summary>
        /// Get category
        /// </summary>
        /// <returns>DataTable</returns>
        //private List<string> GetCategory()
        //{
        //    InternalMessageEntryCollection collection = _manager.GetAllMessages();
        //    List<string> categories = new List<string>();
        //    foreach (var item in collection)
        //    {
        //        if (!categories.Contains(item.Category))
        //        {
        //            categories.Add(item.Category);
        //        }
        //    }

        //    return categories;
        //}

        /// <summary>
        /// Save message
        /// </summary>
        internal void SaveMessage(string severity, string value, string remark)
        {
            if (this.CurrentViewStatus != FunctionNames.ActionStatus.Update
                || _message==null)
            {
                return;
            }

            DataTable messageTable = _message.Table;
            if (messageTable==null)
            {
                return;
            }

            _message.BeginEdit();
            _message["SEVERITY"] = severity;
            _message["VALUE"] = value;
            _message["REMARK"] = remark;
            _message.EndEdit();

            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.SettingsModuleID, FunctionNames.MessageDetailID, ComponentType.Screen))
            {
                using (SettingsProxy proxy = new SettingsProxy())
                {
                    proxy.UpdateMessage(messageTable);
                }
            }

            messageTable.AcceptChanges();
            this.OnMessageSaveEvent(new KeyValueEventArgs(string.Empty,_message,this.Key));
        }

    }
}


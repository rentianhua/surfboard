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
using System.Data;
using System.Linq;
using HiiP.Framework.Common;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using Microsoft.Practices.CompositeUI.EventBroker;
using NCS.IConnect.Messaging;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Settings.ServiceProxy;
using System.Collections.Generic;

namespace HiiP.Framework.Settings
{
    public partial class MessageMaintainPresenter : Presenter<IMessageMaintain>
    {
        #region EventSubscription
        [EventSubscription(EventTopicNames.MessageSaveEvent, ThreadOption.UserInterface)]
        public void OnMessageSaveEvent(object sender, KeyValueEventArgs eventArgs)
        {
            try
            {
                DataRow message = eventArgs.Value as DataRow;

                if (message != null )
                {
                    DataTable messageTable = GenerateMessageTable();
                    var row = messageTable.NewRow();
                    row.BeginEdit();
                    row["ID"] = message["ID"];
                    row["CATEGORY"] = message["CATEGORY"];
                    row["VALUE"] = message["VALUE"];
                    row["SEVERITY"] = message["SEVERITY"].ToString();
                    row.EndEdit();
                    messageTable.Rows.Add(row);
                    messageTable.AcceptChanges();

                    View.BindUltraGrid(messageTable, true);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
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

            View.BindCategory(GetCategories());
            View.BindSeverity(this.GetSeverity());
        }

        /// <summary>
        /// Set shell title and tab title
        /// </summary>
        /// <returns>AppTitleData</returns>
        public override AppTitleData GetAppTitle()
        {
            return new AppTitleData(FunctionNames.MessageMaintenanceName, FunctionNames.MessageMaintenanceScreenID);
        }

        private static string[] GetCategories()
        {
            var categories = new List<string>();
            var messageType = typeof(HiiP.Framework.Messaging.Messages);

            var properties = messageType.GetProperties();

            foreach (var item in properties)
            {
                if (item.PropertyType.IsAssignableFrom(typeof(NCS.IConnect.Messaging.MessageManager)))
                {
                    continue;
                }

                categories.Add(item.Name);
            }

            return categories.ToArray();
        }
        /// <summary>
        /// Search message and Bind data on UI
        /// </summary>
        internal void SearchMessage(string category,string severity, string messageVal )
        {
            DataTable messageTable;
            Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.SettingsModuleID, FunctionNames.MessageMaintenanceID, ComponentType.Screen))
            {
                using (SettingsProxy proxy = new SettingsProxy())
                {
                    messageTable = proxy.RetrieveMessages(category,severity,messageVal);
                }
            }
            var data = GenerateMessageTable();
            data.Merge(messageTable);

            View.BindUltraGrid(data,false);
        }

        /// <summary>
        /// Generate message table
        /// </summary>
        /// <returns></returns>
        public DataTable GenerateMessageTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("CATEGORY");
            dt.Columns.Add("VALUE");
            dt.Columns.Add("SEVERITY");

            dt.PrimaryKey = new[] { dt.Columns[1], dt.Columns[0] };
            return dt;
        }

        /// <summary>
        /// Transfer to update page
        /// </summary>
        /// <param name="ultraGridRow"></param>
        internal void UpdateMessage(Infragistics.Win.UltraWinGrid.UltraGridRow ultraGridRow)
        {
            if (!UIAccessControl.IsAuthorised(FunctionNames.MessageDetailID))
            {
                return;
            }
            ViewParameter parameter = new ViewParameter();
            var category = ultraGridRow.Cells["CATEGORY"].Value.ToString();
            var id = ultraGridRow.Cells["ID"].Value.ToString();

            parameter.ViewId = string.Format("{0} ({1} - {2})",
                FunctionNames.MessageDetailName, string.IsNullOrEmpty(category) ? string.Empty : "- " + category, id);
            parameter.AppTitleData = new AppTitleData(parameter.ViewId, FunctionNames.MessageDetailScreenID);
            parameter.CurrentViewStatus = FunctionNames.ActionStatus.Update;

            var key = new InternalMessageKey(category,id,"");
            parameter.Data = key;
            parameter.Key = parameter.ViewId;
                 
            ShowViewInWorkspace<MessageDetailView>(parameter);
        }


        /// <summary>
        /// Get severity date)
        /// </summary>
        /// <returns></returns>
        private string[] GetSeverity()
        {
            var severity = Enum.GetNames(typeof(MessageSeverity));
            var result = severity.Where(x => !x.Equals("Default",
                                            StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.ToString());

            return result.ToArray();
        }


    }
}


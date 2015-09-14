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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using HiiP.Infrastructure.Interface;
using NCS.IConnect.Hierarchy.Parameter;
using HiiP.Framework.Settings.ServiceProxy;
using HiiP.Framework.Settings.Interface.Services;
using HiiP.Framework.Common;
using HiiP.Framework.Messaging;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using NCS.IConnect.Hierarchy;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace HiiP.Framework.Settings
{
    public partial class HomePageRefreshConfigurationPresenter : Presenter<IHomePageRefreshConfiguration>
    {

        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        protected override void InitData()
        {
            ParameterManager manager = new ParameterManager(ParameterUtil.HierarchyProviderName);

            //Alert
            View.AlertRefreshSeconds = GetData(manager, ParameterUtil.AlertPath);
            //MOTD
            View.MessageRefreshSeconds = GetData(manager, ParameterUtil.MessageOfTheDayPath);
            //TodoList
            View.ToDoListRefreshSeconds = GetData(manager, ParameterUtil.ToDoListPath);
            //MyAppointment
            View.MyAppointmentsRefreshSeconds = GetData(manager, ParameterUtil.MyAppointmentPath);
            //MyReport
            View.MyReportRefreshSeconds = GetData(manager, ParameterUtil.MyReportPath);
            //MyDashboard
            View.MyDashboardRefreshSeconds = GetData(manager, ParameterUtil.MyDashboardPath);

            base.InitData();
        }

        internal void Save()
        {
            IList<ParameterEntity> parameters = new List<ParameterEntity>();

            //Alert
            SaveParameterKey( ParameterEntity.AlertKey,View.AlertRefreshSeconds,parameters);
            //TodoList
            SaveParameterKey(ParameterEntity.ToDoListKey, View.ToDoListRefreshSeconds, parameters);
            //MyAppointments
            SaveParameterKey(ParameterEntity.MyAppointmentsKey, View.MyAppointmentsRefreshSeconds, parameters);
            //MOTD
            SaveParameterKey(ParameterEntity.MOTDKey, View.MessageRefreshSeconds, parameters);
            //MyReport
            SaveParameterKey(ParameterEntity.MyReportKey, View.MyReportRefreshSeconds, parameters);
            //MyDashboard
            SaveParameterKey(ParameterEntity.MyDashboardKey, View.MyDashboardRefreshSeconds, parameters);

            using (SettingsProxy proxy = new SettingsProxy())
            {
                proxy.SaveParameterValue(parameters);
            }

            RefreshCache(parameters);
            OnUpdateStatusBarMessage(Messages.General.GEI002.Format());
        }

        private string GetData(ParameterManager manager, string path)
        {
            ParameterKey key = manager.GetKeyByPath(path);
            return key.Value ?? string.Empty;
        }

        private void SaveParameterKey(string code, string viewValue, IList<ParameterEntity> parameters)
        {
            int value ;
            if (!int.TryParse(viewValue,out value))
            {
                throw new InvalidDataException(Messages.Framework.FWE604.Format(code));
            }
            parameters.Add(new ParameterEntity(code,value));
        }

        private void RefreshCache(IList<ParameterEntity> parameters)
        {
            ParameterManager manager = new ParameterManager(ParameterUtil.HierarchyProviderName);

            foreach (ParameterEntity entity in parameters)
            {
                ParameterKey key = manager.GetKeyByPath(entity.Key);
                if (key == null)
                {
                    //It is impossbile to come to here.
                    continue;
                }
                key.Value = entity.Value.ToString();
            }
        }
    }
}


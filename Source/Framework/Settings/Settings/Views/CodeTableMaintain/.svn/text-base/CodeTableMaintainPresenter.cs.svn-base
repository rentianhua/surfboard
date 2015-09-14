#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// SYSTEM NAME      : Housing Integrated Information Program
// COMPONENT ID     : Settings/CodeTableMaintainPresenter
// COMPONENT DESC   : 
// CREATED DATE/BY  : 25/08/2008/Mei Bo
// REVISION HISTORY : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================
#endregion

using System;
using System.Collections.Generic;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using Microsoft.Practices.CompositeUI.EventBroker;
using NCS.IConnect.CodeTable;

namespace HiiP.Framework.Settings
{
    public partial class CodeTableMaintainPresenter : Presenter<ICodeTableMaintain>
    {

        /// <summary>
        /// Initialize data
        /// </summary>
        protected override void InitData()
        {
            List<string> listCategory = GetBindingCategories();
            View.BindCategory(listCategory);
        }

        public static List<string> GetBindingCategories()
        {
            List<string> listCategory = CodeManager.GetAllCategories();
            listCategory.Sort();

            string emptyItemDisplayText = CodeTableAdapter.GetEmptyItemDisplayText();
            listCategory.Insert(0, emptyItemDisplayText);
            return listCategory;
        }

        /// <summary>
        /// Set shell title and tab title
        /// </summary>
        /// <returns>AppTitleData</returns>
        public override AppTitleData GetAppTitle()
        {
            return new AppTitleData(FunctionNames.CodeTableMaintenanceName,FunctionNames.CodeTableMaintenanceScreenID);
        }

        /// <summary>
        /// To be use when "Search" button is clicked
        /// </summary>
        public void SearchCodeTable(string category)
        {
            CodeTableDataSet.T_IC_CODEDataTable allData = CodeManager.Find(category, string.Empty, string.Empty);

            //DataColumn col = new DataColumn("EffStartDate");
            //col.Caption = "Effective start date";
            //col.Expression = "IIF(EFFECTIVE_START_DATE <= #1/1/1753# OR EFFECTIVE_START_DATE >= #12/31/9998#,NULL, EFFECTIVE_START_DATE)";

            //allData.Columns.Add(col);

            //col = new DataColumn("EffEndDate");
            //col.Caption = "Effective end date";
            //col.Expression = "IIF(EFFECTIVE_END_DATE <= #1/1/1753# OR EFFECTIVE_END_DATE >= #12/31/9998#,NULL, EFFECTIVE_END_DATE)";
       
            //allData.Columns.Add(col);

            View.BindCodeTable(allData,false);
        }

        public void DisplayDetail(string id,string category,string code)
        {
            if (!UIAccessControl.IsAuthorised(FunctionNames.CodeTableDetailID))
            {
                return;
            }
            ViewParameter parameter = new ViewParameter();
            parameter.Key = this.Key;
            parameter.Data = id;

            if (string.IsNullOrEmpty(id))
            {
                parameter.CurrentViewStatus = ViewStatus.Add;

                parameter.ViewId = string.Format("{0}", HiiP.Framework.Settings.Interface.Constants.FunctionNames.CodeTableCreateDetailName);
                parameter.AppTitleData = new AppTitleData(string.Format("{0}",
                    HiiP.Framework.Settings.Interface.Constants.FunctionNames.CodeTableCreateDetailName),
                    HiiP.Framework.Settings.Interface.Constants.FunctionNames.CodeTableCreateDetailScreenID);
            }
            else
            {
                parameter.CurrentViewStatus = ViewStatus.Update;
                parameter.ViewId = string.Format("{0} - {1}", HiiP.Framework.Settings.Interface.Constants.FunctionNames.CodeTableDetailName, id);
                parameter.AppTitleData = new AppTitleData(string.Format("{0} - {1} - {2}",
                    HiiP.Framework.Settings.Interface.Constants.FunctionNames.CodeTableDetailName,
                    category, code),
                    HiiP.Framework.Settings.Interface.Constants.FunctionNames.CodeTableDetailScreenID);
            }
            ShowViewInWorkspace<CodeTableDetailView>(parameter);
        }

        public void Update(CodeTableDataSet.T_IC_CODEDataTable codes)
        { 
            if (codes==null)
            {
                return;
            }
            CodeManager.Update(codes);

            List<string> tempCodes = new List<string>();
            foreach (CodeTableDataSet.T_IC_CODERow code in codes.Rows)
            {
                if (!tempCodes.Contains(code.CODE_CATEGORY))
                {
                    tempCodes.Add(code.CODE_CATEGORY);
                    CodeTableDataSet.T_IC_CODEDataTable dt = CodeManager.Find(code.CODE_CATEGORY, string.Empty, string.Empty);
                    if (null==dt)
                    {
                        continue; 
                    }
                    codes.Merge(dt);
                }
            }
        }

        [EventSubscription(EventTopicNames.ClosedDetail, ThreadOption.UserInterface)]
        public void OnClosedDetail(object sender, KeyValueEventArgs eventArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(eventArgs.Key))
                {
                    return;
                }

                List<string> listCategory = GetBindingCategories();
                View.BindCategory(listCategory);

                CodeTableDataSet.T_IC_CODEDataTable allData = CodeManager.Find(eventArgs.Key);
                View.BindCodeTable(allData, true);

                if (allData.Count <= 0)
                {
                    return;
                }
                View.SetSelectedCategory(allData[0].CODE_CATEGORY);
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }
  
    }
}


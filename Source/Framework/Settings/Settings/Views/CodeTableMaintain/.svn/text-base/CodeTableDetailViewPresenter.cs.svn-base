#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :   Framework/Code Table Maintenance
// COMPONENT DESC    :  
//
// CREATED DATE/BY   : 06/11/2008/He Jiang Yan
//
// REVISION HISTORY  : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion
using System;
using System.Collections.Generic;
using HiiP.Framework.Common.Client.ServiceProxies;
using NCS.IConnect.CodeTable;
using HiiP.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI.EventBroker;
using HiiP.Framework.Settings.Interface.Constants;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Common.Constants;

namespace HiiP.Framework.Settings
{
    public partial class CodeTableDetailViewPresenter : Presenter<ICodeTableDetailView>
    {
        [EventPublication(EventTopicNames.ClosedDetail, PublicationScope.Global)]
        public event EventHandler<HiiP.Framework.Settings.BusinessEntity.KeyValueEventArgs> ClosedDetail;
    
        public CodeTableDataSet.T_IC_CODEDataTable Code { get; set; }

        private string _codeId;
        public string CodeID
        {
            get { return _codeId; }
        }

        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();

        }

        protected override void Dispose(bool disposing)
        {
            Code = null;
            base.Dispose(disposing);
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            base.OnCloseView();
        }

        public void BeforeClose()
        {
            //EndEdit will change the rowstate, so will not use it to set the dirty status.
            View.EndEdit();

            //if (Code.Count <= 0)
            //{
            //    View.SetDirtyStatus(false);
            //}
            //else
            //{
            //    View.SetDirtyStatus((Code[0].RowState == DataRowState.Added && IsEmptyRow(Code[0]))
            //       || (Code != null && Code.GetChanges() != null));
            //}

        }

        protected override void InitData()
        {
            object s = this.Data ?? "";
            _codeId = s.ToString();

            base.InitData();

            List<string> listCategory = CodeManager.GetAllCategories() ;
            listCategory.Sort();

            string emptyItemDisplayText = CodeTableAdapter.GetEmptyItemDisplayText();
            listCategory.Insert(0, emptyItemDisplayText);
            View.BindCategory(listCategory);

            switch (this.CurrentViewStatus)
            {
                case ViewStatus.Add:
                    InitCreateView();
                    break;
                case ViewStatus.Update:
                    InitUpdateView();
                    break;
                case ViewStatus.PropertyMaintainField:
                    InitPropertyMaintainFieldView();
                    break;
                default:
                    break;
            }
        }

        private void InitCreateView()
        {
            View.Authorize(true, true);

            CodeTableDataSet.T_IC_CODERow row = CreateEmptyRow();

            View.Fill(row);

            Code.AddT_IC_CODERow(row);
        }

        private CodeTableDataSet.T_IC_CODERow CreateEmptyRow()
        {
            CodeTableDataSet ds = new CodeTableDataSet();
            Code = ds.T_IC_CODE;
            CodeTableDataSet.T_IC_CODERow row = Code.NewT_IC_CODERow();

            row.BeginEdit();
            row.CODE_ID = Guid.NewGuid().ToString();
            row.CODE = string.Empty;
            row.CODE_CATEGORY = string.Empty;
            row.CODE_SEQ = 0;
            row.CREATED_BY = string.Empty;
            row.CREATED_TIME = DateTime.Now;
            row.EFFECTIVE_START_DATE = MinMaxValues.MinDate;
            row.EFFECTIVE_END_DATE = MinMaxValues.MaxDate;
            row.IS_DELETED = "0";
            row.LAST_UPDATED_BY = string.Empty;
            row.LAST_UPDATED_TIME = DateTime.Now;
            row.LOWERED_CODE = row.CODE.ToLowerInvariant();
            row.LOWERED_CODE_CATEGORY = row.CODE_CATEGORY.ToLowerInvariant();
            row.TRANSACTION_ID = string.Empty;
            row.VERSION_NO = 1;
            row.EndEdit();
            return row;
        }

        //private bool IsEmptyRow(CodeTableDataSet.T_IC_CODERow row)
        //{
        //    CodeTableDataSet.T_IC_CODERow emptyRow = CreateEmptyRow();

        //    return (row.CODE == emptyRow.CODE)
        //        && (row.CODE_CATEGORY == emptyRow.CODE_CATEGORY)
        //        && (row.EFFECTIVE_START_DATE == emptyRow.EFFECTIVE_START_DATE)
        //        && (row.EFFECTIVE_END_DATE == emptyRow.EFFECTIVE_END_DATE)
        //        && (row.CODE_DESC == emptyRow.CODE_DESC)
        //        && (row.CODE_REMARKS == emptyRow.CODE_REMARKS)
        //        && (row.CODE_SEQ == emptyRow.CODE_SEQ);
        //}
        private void InitUpdateView()
        {
            Code = CodeManager.Find(CodeID);
            if (Code == null || Code.Count <= 0)
            {
                View.Authorize(false, false);
                return;
            }
            View.Authorize(true, true);
            View.Fill(Code[0]);
        }

        private void InitPropertyMaintainFieldView()
        {
            View.Authorize(true, true);

            CodeTableDataSet.T_IC_CODERow row = CreateEmptyRow();

            object s = this.Data ?? "";
            row.CODE_CATEGORY = s.ToString();
            row.LOWERED_CODE_CATEGORY = s.ToString().ToLowerInvariant();
            View.Fill(row);

            Code.AddT_IC_CODERow(row);
        }

        public void Save()
        {
            CodeTableDataSet.T_IC_CODEDataTable changes = Code.GetChanges() as CodeTableDataSet.T_IC_CODEDataTable;

            //View have updated the value to the row.
            if (changes==null
                || changes.Count<=0)
            {
                return;
            }

            CodeManager.Update(changes);
            CodeManager.ClearCache();

            using (var proxy = new CodeTableExtensionServiceProxy())
            {
                proxy.ClearCache();
            }

            Code.AcceptChanges();

            View.SetDirtyStatus(false);
            OnClosedDetail(new KeyValueEventArgs(changes[0].CODE_ID, null,this.Key));
            OnCloseView();

        }

        protected virtual void OnClosedDetail(HiiP.Framework.Settings.BusinessEntity.KeyValueEventArgs eventArgs)
        {
            if (ClosedDetail != null)
            {
                ClosedDetail(this, eventArgs);
            }
        }
      
    }
}


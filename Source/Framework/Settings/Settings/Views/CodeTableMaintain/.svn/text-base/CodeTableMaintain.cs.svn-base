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
using System.Data;
using System.Windows.Forms;
using NCS.IConnect.CodeTable;
using HiiP.Framework.Common.Client;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Settings.Interface.Constants;
using NCS.IConnect.Messaging;
using Infragistics.Win.UltraWinGrid;

namespace HiiP.Framework.Settings
{
    public partial class CodeTableMaintain : BaseView, ICodeTableMaintain
    {
        public CodeTableMaintain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.OnViewReady();
                this.ultraButtonCreate.Visible = (_presenter.UIAccessControl.IsAuthorised(FunctionNames.CodeTableDetailID));
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                this.Enabled = false;
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Set shell title and tab title
        /// </summary>
        /// <param name="parameter"></param>
        public override void ProcessParameter(ViewParameter parameter)
        {
            AppTitle = _presenter.GetAppTitle();
            base.ProcessParameter(parameter);
        }

        #region ICodeTableMaintain
        /// <summary>
        /// Bind data to Category ComboBox
        /// </summary>
        /// <param name="listCategory"></param>
        public void BindCategory(List<string> listCategory)
        {
            this.ultraComboCategory.Items.Clear();
            this.ultraComboCategory.Items.ValueList.Reset();
            this.ultraComboCategory.DataSource = listCategory;
            this.ultraComboCategory.SelectedIndex = 0;
        }

        public void SetSelectedCategory(string category)
        {
            ultraComboCategory.Value = category;
        }

        /// <summary>
        /// Bind data on UltraGrid
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="needMerge"></param>
        public void BindCodeTable(CodeTableDataSet.T_IC_CODEDataTable codes, bool needMerge)
        {
            CodeTableDataSet.T_IC_CODEDataTable temp = this.ultraGridCodeTable.DataSource as CodeTableDataSet.T_IC_CODEDataTable;
            if (needMerge && (temp != null))
            {
                temp.Merge(codes);
            }
            else
            {
                temp = codes;
            }
            this.ultraLabTotalNum.Text = HiiP.Framework.Messaging.Messages.General.GEI001.Format(temp.Count);

            this.ultraGridCodeTable.DataSource = temp;
        }

       #endregion

        #region Control Event

        private void ultraBtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int index = this.ultraComboCategory.SelectedIndex;
                string category = (index == 0) ? "" : this.ultraComboCategory.SelectedItem.DataValue.ToString();

                this.ultraGridCodeTable.Focus();
                _presenter.SearchCodeTable(category);
            }
            catch (Exception exception)
            {
                if (ExceptionManager.Handle(exception)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ultraBtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                this.ultraComboCategory.SelectedIndex = 0;

                CodeTableDataSet ds = new CodeTableDataSet();
                BindCodeTable(ds.T_IC_CODE,false);
            }
            catch (Exception exception)
            {
                if (ExceptionManager.Handle(exception)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ultraBtnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                _presenter.OnCloseView();
            }
            catch (Exception exception)
            {
                if (ExceptionManager.Handle(exception)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ultraGridCodeTable_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                OnShowDetail(e.Row);
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ultraGridCodeTable_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.KeyData != Keys.Enter && e.KeyData != Keys.Space)
                {
                    return;
                }

                UltraGridRow row = this.ultraGridCodeTable.ActiveRow;

                OnShowDetail(row);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void OnShowDetail(UltraGridRow row)
        {

            this.Cursor = Cursors.WaitCursor;

            if (row == null
              || row.Cells == null)
            {
                return;
            }

            _presenter.DisplayDetail(row.Cells["CODE_ID"].Value.ToString(),
            row.Cells["CODE_CATEGORY"].Value.ToString(),
            row.Cells["CODE"].Value.ToString());
        }


        #endregion

        private void ultraButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                _presenter.DisplayDetail(null,null,null );

            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}


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
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Settings.Interface.Constants;
using NCS.IConnect.Messaging;

namespace HiiP.Framework.Settings
{
    public partial class MessageMaintain : BaseView, IMessageMaintain
    {
        public MessageMaintain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.OnViewReady();
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

        public override void ProcessParameter(ViewParameter parameter)
        {
            AppTitle = _presenter.GetAppTitle();
            base.ProcessParameter(parameter);
        }

        #region Function in interface IMessageMaintain
        /// <summary>
        /// Bind data on severity combox
        /// </summary>
        public void BindSeverity(string[] severities)
        {
            var table = BuildBandTable(this.severityMultipleSelectedComboEditor);
            foreach (var item in severities)
            {
                table.Rows.Add(item, item);
            }

            this.severityMultipleSelectedComboEditor.DataSource = table;
        }

        /// <summary>
        /// Bind data on category combox
        /// </summary>
        public void BindCategory(string[] categories)
        {
            var table = BuildBandTable(this.categoryMultipleSelectedComboEditor);
            foreach (var item in categories)
            {
                table.Rows.Add(item, item);
            }

            this.categoryMultipleSelectedComboEditor.DataSource = table;
        }

        private static DataTable BuildBandTable(MultipleSelectedComboEditor editor)
        {
            var table = new DataTable();
            table.Columns.Add(editor.DisplayName);
            table.Columns.Add(editor.ValueName);

            return table;
        }
        /// <summary>
        /// Bind data on UltraGird
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="needMerge"></param>
        public void BindUltraGrid(DataTable messages, bool needMerge)
        {
            DataTable temp = this.ultraGridMessage.DataSource as DataTable;
            if (!needMerge || temp == null)
            {
                temp = messages;
            }
            else
            {
                temp.Merge(messages);
            }
            this.ultraLabTotalNum.Text = HiiP.Framework.Messaging.Messages.General.GEI001.Format(temp.Rows.Count);
            this.ultraGridMessage.DataSource = temp;
        }
        #endregion

        #region Event function
        private void ultraBtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.ultraGridMessage.Focus();

                var categories = this.categoryMultipleSelectedComboEditor.GetSelectedValues().ConvertAll<string>(x => x.ToString());
                List<string> severities = this.severityMultipleSelectedComboEditor.GetSelectedValues().ConvertAll<string>(x => x.ToString());

                _presenter.SearchMessage(string.Join(",",categories.ToArray()),
                    string.Join(",",severities.ToArray()),
                    this.ultraTextMessage.Text);
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


        private void ultraGridMessage_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (e.Row==null || e.Row.Cells==null)
                {
                    return;
                }
                _presenter.UpdateMessage(e.Row);
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

        private void ultraButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.categoryMultipleSelectedComboEditor.ClearSelectedValues();
                this.severityMultipleSelectedComboEditor.ClearSelectedValues();
                this.ultraTextMessage.Text = string.Empty;

                BindUltraGrid(_presenter.GenerateMessageTable(),false);
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

        private void ultraButtonClose_Click(object sender, EventArgs e)
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

        private void MessageMaintain_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.KeyData != Keys.Enter && e.KeyData != Keys.Space)
                {
                    return;
                }

                this.ultraBtnSearch_Click(this, EventArgs.Empty);
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

        private void ultraGridMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter && e.KeyData != Keys.Space)
            {
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                Infragistics.Win.UltraWinGrid.UltraGridRow row = this.ultraGridMessage.ActiveRow;

                if (row == null || row.Cells == null)
                {
                    return;
                }
                _presenter.UpdateMessage(row);
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
        #endregion

    }
}


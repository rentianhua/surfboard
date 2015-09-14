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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;
using NCS.IConnect.CodeTable;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common;
using HiiP.Framework.Messaging;

namespace HiiP.Framework.Settings
{
    public partial class CodeTableDetailView : BaseView, ICodeTableDetailView
    {
        public CodeTableDetailView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.OnViewReady();
                this.Closing += CodeTableDetailView_Closing;
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

        void CodeTableDetailView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            _presenter.BeforeClose();
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
            _presenter.Key = parameter.Key;
            _presenter.Data = parameter.Data;
            _presenter.CurrentViewStatus = parameter.CurrentViewStatus;
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

        public void Fill(CodeTableDataSet.T_IC_CODERow data)
        {
            this.tICCODERowBindingSource.DataSource = data;
            if (!string.IsNullOrEmpty(data.CODE_CATEGORY))
            {
                this.ultraComboCategory.Value = data.CODE_CATEGORY;
            }
            else
            {
                ultraComboCategory.SelectedIndex = 0;
            }
            //DateTime obj = data.EFFECTIVE_START_DATE;
            //DateTime Date1753=new DateTime(1753,1,1),Date9998=new DateTime(9998,12,31);
            //if (obj >= Date9998 || obj <= Date1753)
            //{
            //    ultraDateTimeStartTime.DateTime = Date1753;
            //    ultraDateTimeStartTime.Value = null;
            //}
            //else
            //{
            //    ultraDateTimeStartTime.DateTime = obj;
            //}
            //obj = data.EFFECTIVE_END_DATE;
            //if (obj >= Date9998 || obj <= Date1753)
            //{
            //    ultraDateTimeEndTime.DateTime = Date9998;
            //    ultraDateTimeEndTime.Value = null;
            //}
            //else
            //{
            //    ultraDateTimeEndTime.DateTime = obj;
            //}
        }

        //private static ValueListItem GetSelectedValue(UltraComboEditor cb, string currentValue)
        //{
        //    foreach (ValueListItem item in cb.Items)
        //    {
        //        if (item.DataValue.ToString().ToLower().Equals(currentValue,StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            return item;
        //        }
        //    }

        //    return null;
        //}

        public void Authorize(bool canRead, bool canWrite)
        {
            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                ctrl.Visible = canRead;

                UltraTextEditor textEditor = ctrl as UltraTextEditor;
                if (null != textEditor && ctrl!=this.ultraTextEditorID)
                {
                    textEditor.ReadOnly = !canWrite;
                }
                else
                {
                    UltraComboEditor comboEditor = ctrl as UltraComboEditor;
                    if (null != comboEditor )
                    {
                        comboEditor.ReadOnly = !canWrite;
                    }
                    else
                    {
                        ctrl.Enabled = canWrite;
                    }
                }
            }
        }

        void ICodeTableDetailView.SetDirtyStatus(bool isDirty)
        {
            CheckDirtyRequired = true;
            SetDirtyStatus(isDirty);
        }

        public void EndEdit()
        {
            this.tICCODERowBindingSource.EndEdit();
            //Remove the binding so that there was no error :
            //"Cannot bind to the property or column id on the DataSource.
            //Parameter name: dataMember"
            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (ctrl.DataBindings!=null && ctrl.DataBindings.Count>0)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                {
                    ctrl.DataBindings.Clear();
                }
            }
            this.tICCODERowBindingSource.Dispose();
        }
        #endregion

        private void ultraButtonCancel_Click(object sender, EventArgs e)
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

        private void ultraButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.errorProvider.Clear();
                if(!ValidateChildren())
                {
                    return;
                }

                this.tICCODERowBindingSource.EndEdit();
                CodeTableDataSet.T_IC_CODERow row = this.tICCODERowBindingSource.Current as CodeTableDataSet.T_IC_CODERow;
                if (row!=null)
                {
                    row.LOWERED_CODE = row.CODE.ToLowerInvariant();
                    if (string.IsNullOrEmpty(this.ultraComboCategory.Text.Trim()))
                    {
                        //Because of validation, it will not go here.
                        throw new UnableHandleException(Messages.Framework.FWE601.Format());
                    }
                    row.CODE_CATEGORY = this.ultraComboCategory.Text.Trim();
                    row.LOWERED_CODE_CATEGORY = row.CODE_CATEGORY.ToLowerInvariant();

                    //object obj = this.ultraDateTimeStartTime.Value;
                    //row.EFFECTIVE_START_DATE = (obj == null) ? new DateTime(1753, 1, 1) : this.ultraDateTimeStartTime.DateTime;
                    //obj = this.ultraDateTimeEndTime.Value;
                    //row.EFFECTIVE_END_DATE = (obj == null) ? new DateTime(9998, 12, 31) : this.ultraDateTimeEndTime.DateTime;

                }

                this._presenter.OnUpdateStatusBarMessage(Framework.Messaging.Messages.General.GEI002.Format());
                _presenter.Save();
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

        private void validationProvider_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.SourcePropertyName == "CODE_CATEGORY")
                {
                    string emptyItemDisplayText = CodeTableAdapter.GetEmptyItemDisplayText();
                    if (e.ValueToConvert.ToString().Equals(emptyItemDisplayText, StringComparison.InvariantCultureIgnoreCase))
                    {
                        e.ConvertedValue = "";
                    }
                }
                //if (e.SourcePropertyName == "EFFECTIVE_END_DATE")
                //{
                //    if (null == ultraDateTimeEndTime.Value)
                //    {
                //        e.ConvertedValue = this.ultraDateTimeEndTime.MaxDate;
                //    }
                //}
                if (e.SourcePropertyName == "EFFECTIVE_START_DATE")
                {
                    //e.ConvertedValue = this.ultraDateTimeStartTime.DateTime;
                    //if (null == e.ValueToConvert)
                    //{
                    //    e.ConvertedValue = this.ultraDateTimeStartTime.MinDate;
                    //}

                    if (string.IsNullOrEmpty(this.ultraComboCategory.Text.Trim())
                        || string.IsNullOrEmpty(this.ultraTextEditorCode.Text.Trim()))
                    {
                        return;
                    }

                    CodeTableDataSet.T_IC_CODEDataTable dt = CodeManager.Find(this.ultraComboCategory.Text.Trim(), this.ultraTextEditorCode.Text.Trim(),null);
                    if (dt.Count <= 0)
                    {
                        return;
                    }

                    switch (this._presenter.CurrentViewStatus)
                    {
                        case ViewStatus.Add:
                            if (dt.Select(string.Format("EFFECTIVE_END_DATE>='{0}' AND EFFECTIVE_START_DATE<='{1}'",
                                    this.ultraDateTimeStartTime.DateTime,this.ultraDateTimeEndTime.DateTime)).Length > 0)
                            {
                                e.ConversionErrorMessage = Messages.Framework.FWE602.Format();
                            }
                            break;
                        case ViewStatus.Update:
                            if (dt.Select(string.Format("CODE_ID<>'{0}' AND EFFECTIVE_END_DATE>='{1}' AND EFFECTIVE_START_DATE<='{2}'",
                                   this.ultraTextEditorID.Text,this.ultraDateTimeStartTime.DateTime, this.ultraDateTimeEndTime.DateTime)).Length > 0)
                            {
                                e.ConversionErrorMessage = Messages.Framework.FWE602.Format();
                            }

                            if (dt.Select(string.Format("CODE_ID='{0}' AND EFFECTIVE_START_DATE<='{1}' AND EFFECTIVE_END_DATE>='{1}' AND EFFECTIVE_START_DATE<>'{2}'",
                                   this.ultraTextEditorID.Text, DateTime.Today,this.ultraDateTimeStartTime.DateTime)).Length > 0)
                            {
                                if (!string.IsNullOrEmpty(e.ConversionErrorMessage))
                                {
                                    e.ConversionErrorMessage += Environment.NewLine + Messages.Framework.FWE603.Format();
                                }
                                else
                                {
                                    e.ConversionErrorMessage = Messages.Framework.FWE603.Format();
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
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

        private void OnLeave(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.tICCODERowBindingSource.EndEdit();
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

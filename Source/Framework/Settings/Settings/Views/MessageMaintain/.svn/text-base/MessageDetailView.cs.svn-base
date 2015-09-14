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
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Settings.BusinessEntity;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Settings.Interface.Constants;
using NCS.IConnect.Messaging;
using Infragistics.Win.UltraWinEditors;
using HiiP.Framework.Messaging;

namespace HiiP.Framework.Settings
{
    public partial class MessageDetailView : BaseView, IMessageDetailView
    {
        public MessageDetailView()
        {
            InitializeComponent();
        }

        public override void ProcessParameter(ViewParameter parameter)
        {
            base.ProcessParameter(parameter);
            _presenter.Key = parameter.Key;
            _presenter.CurrentViewStatus = parameter.CurrentViewStatus;
            _presenter.Data = parameter.Data;
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

        #region Override interface IMessageDetailView

        /// <summary>
        /// Bind data on severity combox
        /// </summary>
        /// <param name="severities"></param>
        public void BindSeverity(string[] severities)
        {
            this.Severity.DataSource = severities;
            this.Severity.DataBind();
            this.Severity.Value = MessageSeverityHelper.GetName(MessageSeverity.Information);
        }

        /// <summary>
        /// Bind data on category combox
        /// </summary>
        /// <param name="categories"></param>
        public void BindCategory(List<string> categories)
        {
            //this.Category.DataSource = categories;
            //this.Category.DataBind();
        }

        /// <summary>
        /// Bind data on UI when current status is "Update"
        /// </summary>
        /// <param name="message"></param>
        public void BindUpdateData(DataRow message)
        {
            if (message == null)
            {
                return;
            }
            this.Category.Text = message["CATEGORY"].ToString();
            this.Id.Text = message["ID"].ToString();
            this.Value.Text = message["VALUE"].ToString();
            this.Remark.Text = message["REMARK"].ToString();
            this.Severity.Value = MessageSeverityHelper.ParseSeverity(message["SEVERITY"].ToString()).ToString();

            this.SetDirtyStatus(false);
        }

        public void Authorize(bool canRead, bool canWrite)
        {
            foreach (Control ctrl in this.ultraGroupBox1.Controls)
            {
                ctrl.Visible = canRead;

                UltraTextEditor textEditor = ctrl as UltraTextEditor;
                if (null != textEditor 
                    && ctrl != this.Id
                    && ctrl!=this.Category)
                {
                    textEditor.ReadOnly = !canWrite;
                }
                else
                {
                    ctrl.Enabled = canWrite;
                }
            }
        }
        #endregion

        private void ultraBtnSave_Click(object sender, EventArgs e)
        {
            if (!this.HasDirtyData())
            {
                return;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!base.ValidateChildren()) return;

                if (_presenter.CurrentViewStatus == FunctionNames.ActionStatus.Update)
                {
                    _presenter.SaveMessage(Severity.Value as string,Value.Text,Remark.Text);

                    this.SetDirtyStatus(false);

                    Messages.Manager.ClearCache();

                    this._presenter.OnUpdateStatusBarMessage(Framework.Messaging.Messages.General.GEI002.Format());
                    _presenter.OnCloseView();
                }
            }
            catch (Exception exception)
            {
                if (ExceptionManager.Handle(exception)) throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ultraBtnCancel_Click(object sender, EventArgs e)
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

        
    }
}


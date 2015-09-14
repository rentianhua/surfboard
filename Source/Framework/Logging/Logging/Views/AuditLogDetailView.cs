#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :   Framework/Audit Log View
// COMPONENT DESC    :  
//
// CREATED DATE/BY   : 12/9/2008/He Jiang Yan
//
// REVISION HISTORY  : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Infrastructure.Interface.Constants;
using HiiP.Framework.Common.Client;
using Infragistics.Win.UltraWinGrid;

namespace HiiP.Framework.Logging
{
    public partial class AuditLogDetailView : BaseView, IAuditLogDetailView
    {
        public AuditLogDetailView()
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
            _presenter.Key = parameter.Key;
            _presenter.Data = parameter.Data;
            _presenter.CurrentViewStatus = ViewStatus.View;
            AppTitle = _presenter.GetAppTitle();
            base.ProcessParameter(parameter);
        }

        #region IAuditLogDetailView Members

        public void LoadDetail(System.Data.DataSet detailData,string dataMember)
        {
            ultraGridDetail.DataSource = detailData;
            ultraGridDetail.DataMember = dataMember;
        }

        #endregion

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.OnCloseView();
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

        private void ultraButtonPrint_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
                ultraGridPrintDocument.DefaultPageSettings.Margins.Top = 12;
                ultraGridPrintDocument.DefaultPageSettings.Margins.Left = 12;
                ultraGridPrintDocument.DefaultPageSettings.Margins.Right = 25;
                ultraGridPrintDocument.DefaultPageSettings.Margins.Bottom = 25;
                ultraGridPrintDocument.OriginAtMargins = true;
                ultraGridPrintDocument.RowProperties = RowPropertyCategories.Hidden;
                this.ultraGridDetail.PrintPreview(this.ultraGridDetail.DisplayLayout, this.ultraGridPrintDocument);
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


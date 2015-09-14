#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Role maintenance
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Data;
using System.Windows.Forms;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Infrastructure.Interface;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class ListOfFunctionAndDataFilter : BaseView, IListOfFunctionAndDataFilter
    {
        public ListOfFunctionAndDataFilter()
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

                this.ViewActionCodeAndDFList();
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

        public override void ProcessParameter(HiiP.Infrastructure.Interface.BusinessEntities.ViewParameter parameter)
        {
            _presenter.Key = parameter.Key;
            AppTitle = _presenter.GetAppTitle();

            base.ProcessParameter(parameter);
        }

        #region Event
               

        private void closeButton_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.OnCloseView();
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        #endregion

        #region IListOfFunctionAndDatafilter Members

        public void ViewActionCodeAndDFList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ACTION CODE", Type.GetType("System.String"));
            dt.Columns.Add("DATA FILTER", Type.GetType("System.String"));           
            this.ug_functionDFlist.DataSource = dt;
            this.lbl_recordCount.Text = this.ug_functionDFlist.Rows.Count.ToString();

            FunctionAndDataFilterEntity[] functionAndDataFilterEntitys = _presenter.GetActionCodeAndDFListByRoleName(_presenter.Key);

            if (functionAndDataFilterEntitys != null && functionAndDataFilterEntitys.Length > 0)
            {
                foreach (FunctionAndDataFilterEntity entity in functionAndDataFilterEntitys)
                {
                    dt.Rows.Add(
                        entity.ActionCode,
                        entity.DataFilter);
                }
            }
            this.ug_functionDFlist.DataSource = dt;
            this.lbl_recordCount.Text = this.ug_functionDFlist.Rows.Count.ToString();
        }

        #endregion       
    }
}


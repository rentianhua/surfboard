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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using System.Data;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using Infragistics.Win.UltraWinGrid;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Security.UserManagement
{
    public partial class ListOfUserInRole : BaseView, IListOfUserInRole
    {
        public ListOfUserInRole()
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

                this.ViewUserList();
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

        #region IRoleMaintenance Members

        public void ViewUserList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("User ID", Type.GetType("System.String"));
            dt.Columns.Add("Status", Type.GetType("System.String"));
            dt.Columns.Add("Created On", Type.GetType("System.DateTime"));
            dt.Columns.Add("User Name", Type.GetType("System.String"));
            dt.Columns.Add("Gender", Type.GetType("System.String"));
            dt.Columns.Add("Title", Type.GetType("System.String"));
            dt.Columns.Add("Email", Type.GetType("System.String"));
            dt.Columns.Add("Telephone No", Type.GetType("System.String"));
            //dt.Columns.Add("Organisation", Type.GetType("System.String"));
            dt.Columns.Add("Office", Type.GetType("System.String"));
            dt.Columns.Add("Mobile", Type.GetType("System.String"));
            this.ug_userlist.DataSource = dt;
            this.lbl_recordCount.Text = this.ug_userlist.Rows.Count.ToString();

            UserInfoEntity[] userInfoEntitys = _presenter.GetUserInRoleByRoleName(_presenter.Key);

            if (userInfoEntitys != null && userInfoEntitys.Length > 0)
            {
                foreach (UserInfoEntity entity in userInfoEntitys)
                {
                    dt.Rows.Add(
                        entity.UserName,
                        entity.UserStatus,
                        entity.CreatedOn,
                        entity.Display,
                        entity.Gender,
                        entity.Title,
                        entity.Email,
                        entity.TelephoneNo,
                        //entity.Organisation,
                        entity.Office,
                        entity.MobileNo);
                }
            }
            this.ug_userlist.DataSource = dt;
            this.lbl_recordCount.Text = this.ug_userlist.Rows.Count.ToString();
        }

        #endregion


        private void ug_userlist_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _presenter.ShowUpdateUserView(e.Cell.Row.Cells["User ID"].Value.ToString());
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

namespace HiiP.Framework.Security.UserManagement
{
    [SmartPart]
    public partial class ListOfUserInRole
    {
        /// <summary>
        /// Sets the presenter. The dependency injection system will automatically
        /// create a new presenter for you.
        /// </summary>
        [CreateNew]
        public ListOfUserInRolePresenter Presenter
        {
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

    }
}


using System;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Security.SessionManagement
{
    public partial class FilterSessionView : BaseView, IFilterSessionView
    {
        /// <summary>
        /// Define a state variable to save session criteria
        /// session criteria share between "SessonManagement" view and "FileterSession" view 
        /// </summary>
        //[State("FilterSessionCriteria")]
        //public SessionCriteria SessionCriteria
        //{
        //    set { sessionCriteria = value; }
        //}

        public FilterSessionView()
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

        #region Event

        private void btn_ok_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                // execute filter setting
                _presenter.FilterSessionByCriteria(
                    this.txt_userName.Text,
                    this.txt_ipAddress.Text,
                    this.txt_host.Text,
                    this.dt_logintime_start.DateTime,
                    this.dt_logintime_end.DateTime,
                    this.dt_lastactivetime_start.DateTime,
                    this.dt_lastactivetime_end.DateTime
                    );
                // save filter setting
                // ?
                // after setting filter condition, close current view
                _presenter.OnCloseView();
                Dispose();
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

        private void btn_cancel_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                // notice, only close current view, not dispose it
                _presenter.OnCloseView();
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

        #endregion

    }
}


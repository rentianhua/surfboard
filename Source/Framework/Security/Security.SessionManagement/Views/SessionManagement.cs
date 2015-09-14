using System;

using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;

using Microsoft.Practices.CompositeUI.SmartParts;
using System.Collections.Generic;
using System.Windows.Forms;
using HiiPResources = HiiP.Framework.Security.SessionManagement.Properties;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Security.AccessControl.Interface;
using HiiP.Framework.Common;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.SessionManagement.Interface.Constants;
using System.Threading;
using System.ComponentModel;
using HiiP.Framework.Common.Client.Async;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Security.AccessControl.BusinessEntity;

namespace HiiP.Framework.Security.SessionManagement
{
    public partial class SessionManagementView : BaseView, ISessionManagementView
    {
        public SessionManagementView()
        {
            InitializeComponent();
        }

        private IDictionary<Guid,SessionInfo> _sessionList = new Dictionary<Guid,SessionInfo>();

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                base.OnLoad(e);
                _presenter.OnViewReady();
            }
            catch (Exception ex)
            {
                this.Enabled = false;
                if (ExceptionManager.Handle(ex))
                {
                    throw;
                }
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

        #region Event

        private void refreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.ug_sessionlist.DataSource = Array.CreateInstance(typeof(SessionInfo), 0);
                this.ug_sessionlist.DataBind();
                ShowSessionList(null);
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

        private void killSessionButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                KillSession();
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

        private void closeButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
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

        #region ISessionManagement Members

        public void ShowSessionList(SessionCriteria sessionCriteria)
        {
            this.ug_sessionlist.Focus();

            using (AsyncWorker<ISessionManagementView> worker = new AsyncWorker<ISessionManagementView>(_presenter, this.ug_sessionlist, new Control[] { refreshButton }))
            {
                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    eDoWork.Result = _presenter.GetActiveSessions(sessionCriteria);
                };
                worker.RunWorkerCompleted += delegate(object oCompleted, RunWorkerCompletedEventArgs eCompleted)
                {
                    SessionInfo[] sessions = eCompleted.Result as SessionInfo[];
                    if (sessions == null)
                    {
                        return;
                    }
                    this._sessionList.Clear();
                    foreach (SessionInfo sessionInfo in sessions)
                    {
                        this._sessionList.Add(sessionInfo.SessionID, sessionInfo);
                    }

                    this.ug_sessionlist.DataSource = sessions;
                    this.ug_sessionlist.DataBind();
                    this.lbl_totalsessionnumber.Text = this.ug_sessionlist.Rows.Count.ToString();
                };
                worker.Run();
            }
        }

        public void KillSession()
        {
            List<Guid> sessionIDList = new List<Guid>();
            foreach (UltraGridRow row in ug_sessionlist.Rows)
            {
                if (row.Cells == null) continue;
                if (row.Cells["Select"].Text == "True")
                {
                    sessionIDList.Add(new Guid(row.Cells["SessionID"].Value.ToString()));
                }
            }

            if (sessionIDList.Count > 0)
            {
                if (sessionIDList.Contains(new Guid(AppContext.Current.SessionID)))
                {
                    Utility.ShowMessageBox(Messages.Framework.FWE005,AppContext.Current.SessionID);
                    return;
                }

                if (Utility.ShowMessageBox(Messages.Framework.FWC001) == DialogResult.Yes)
                {
                    this._presenter.KillSessions(sessionIDList.ToArray());

                    foreach (Guid sessionID in sessionIDList)
                    {
                        this._sessionList.Remove(sessionID);
                    }
                    SessionInfo[] sessions = Array.CreateInstance(typeof(SessionInfo), this._sessionList.Count) as SessionInfo[];
// ReSharper disable AssignNullToNotNullAttribute
                    this._sessionList.Values.CopyTo(sessions, 0);
// ReSharper restore AssignNullToNotNullAttribute
                    this.ug_sessionlist.DataSource = sessions;
                    this.ug_sessionlist.DataBind();
                    this.lbl_totalsessionnumber.Text = this.ug_sessionlist.Rows.Count.ToString();
                }
            }
        }

        //public void ShowFilterSession()
        //{
        //    _presenter.ShowFilterSessionView();
        //}

        #endregion

        private void ug_sessionlist_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter || e.KeyData == Keys.Space)
                {
                    UltraGridRow row = ug_sessionlist.ActiveRow;

                    if (row != null)
                    {
                        if (row.Cells == null)
                        {
                            return;
                        }

                        string isSelected = row.Cells["Select"].Value.ToString();
                        if (isSelected == "True")
                        {
                            row.Cells["Select"].Value = false;
                        }
                        else
                        {
                            row.Cells["Select"].Value = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        public void AccessControls(bool hasKillRight)
        {
            this.killSessionButton.Visible = hasKillRight;
        }
    }
}
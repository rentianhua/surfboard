using System;
using HiiP.Infrastructure.Interface.Services;
using HiiP.Framework.Security.AccessControl.BusinessEntity;
using System.ComponentModel;
using HiiP.Framework.Common.ApplicationContexts;
using System.Runtime.CompilerServices;
using HiiP.Framework.Messaging;
using NCS.IConnect.ExceptionHandling;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Common.Client;
using NCS.IConnect.Common;
using NCS.IConnect.Messaging;
using System.Windows.Forms;
using HiiP.Framework.Security.AccessControl.Authentication;
using System.Threading;

namespace HiiP.Framework.Security.AccessControl.SessionRenew
{
    public class SessionRefreshService : ISessionRefresh
    {
        private readonly static BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private static System.Threading.Timer _autoRefreshScheduler;
        private static bool _isSessionKilled;
        private static bool _isSessionTimeout;

        static SessionRefreshService()
        {
            if (_isSessionKilled
                || ((AuthenticationMode.Forms == AppContext.Current.AuthenticationMode) && _isSessionTimeout))
            {
                return;
            }
            _backgroundWorker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
            {
                eDoWork.Result = SessionRenewManager.RefreshSession(false);
            };
            _backgroundWorker.RunWorkerCompleted += SessionRefreshCompleted;
        }

        internal static Exception ParseRawExceotion(Exception ex)
        {
            Exception realExcption = FaultExceptionHelper.Handle(ex);
            SessionRenewException sessionRenewException = realExcption as SessionRenewException;
            if (null != sessionRenewException)
            {
                switch (realExcption.HelpLink)
                {
                    case "FWE003":
                        {
                            sessionRenewException.SessionStatus = SessionStatus.Killed;
                            break;
                        }
                    case "FWE004":
                        {
                            sessionRenewException.SessionStatus = SessionStatus.Timeout;
                            break;
                        }
                    case "FWE006":
                        {
                            sessionRenewException.SessionStatus = SessionStatus.NotMatchUserName;
                            break;
                        }
                    case "FWE007":
                        {
                            sessionRenewException.SessionStatus = SessionStatus.InvalidOrScavenged;
                            break;
                        }
                }

                return sessionRenewException;
            }

            return realExcption;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void SessionRefreshCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Exception exception ;

            if (null != e.Error)
            {
                exception = ParseRawExceotion(e.Error);
            }
            else
            {
                SessionStatus status = (SessionStatus)e.Result;
                exception = CreateSessionRenewExceptionByStatus(status);
            }


            if (null != exception)
            {
                _isSessionKilled = ExceptionManager.IsSessionKilled(e.Error);
                _isSessionTimeout = ExceptionManager.IsSessionTimeOut(e.Error);

                ExceptionPolicy.HandleException(exception, "SessionRenew Policy");
                _isSessionTimeout = false;
            }
            else
            {
                if (e.Error!=null)
                {
                    ExceptionPolicy.HandleException(e.Error, "SessionRenew Policy");
                }
            }

        }

        private static SessionRenewException CreateSessionRenewExceptionByStatus(SessionStatus status)
        {
            switch (status)
            {
                case SessionStatus.Timeout:
                    {
                        return new SessionRenewException(Messages.Framework.FWE004.Format(), SessionStatus.Timeout);
                    }
                case SessionStatus.Killed:
                    {
                        return new SessionRenewException(Messages.Framework.FWE003.Format(), SessionStatus.Killed);
                    }
                case SessionStatus.NotMatchUserName:
                    {
                        return new SessionRenewException(Messages.Framework.FWE006.Format(), SessionStatus.NotMatchUserName);
                    }
                case SessionStatus.InvalidOrScavenged:
                    {
                        return new SessionRenewException(Messages.Framework.FWE007.Format(), SessionStatus.InvalidOrScavenged);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        /// <summary>
        /// Refresh session intervally
        /// </summary>
        public void KeepAlive()
        {
            if (_backgroundWorker.IsBusy)
            {
                return;
            }

            _backgroundWorker.RunWorkerAsync();
        }

        public void StartAutoRefreshScheduler()
        {
            if (null != _autoRefreshScheduler)
            {
                return;
            }

            //string timeoutVal = string.IsNullOrEmpty(AppContext.Current.SessionTimeoutInterval) ? "0" : AppContext.Current.SessionTimeoutInterval;
            //int timeoutMinSeconds = 1000 * int.Parse(timeoutVal);

            //if (AuthenticationMode.Forms == AppContext.Current.AuthenticationMode)
            //{
            //    _autoRefreshScheduler = new System.Threading.Timer(SessionCheckForFormsAuthentication, timeoutMinSeconds, 1000 * 60, timeoutMinSeconds);
            //}

            string intervalVal = string.IsNullOrEmpty(AppContext.Current.SessionRefreshInterval) ? "0" : AppContext.Current.SessionRefreshInterval;
            int intervalMinSeconds = 1000 * int.Parse(intervalVal);
            if (AuthenticationMode.Integration == AppContext.Current.AuthenticationMode)
            {
                _autoRefreshScheduler = new System.Threading.Timer(SessionRefreshForIntegrationAuthentication, null, 1000 * 60, intervalMinSeconds);
            }
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        //private static void SessionCheckForFormsAuthentication(object state)
        //{
        //    //var timeoutMinSeconds = (int)state;

        //    //if ((DateTime.Now - SessionRenewManager.LatestRefreshTime).TotalMilliseconds > timeoutMinSeconds)
        //    //{
        //    //    ExceptionManager.IsActive = false;
        //    //}
        //    //else
        //    //{
        //    //    ExceptionManager.IsActive = true;
        //    //}
        //    //System.Diagnostics.Trace.WriteLine(ExceptionManager.IsActive ? "Active" : "Idled");
        //}

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void SessionRefreshForIntegrationAuthentication(object state)
        {
            try
            {
                if (_isSessionKilled)
                {
                    //No need to take care of session timeout at here
                    return;
                }
                if (SessionRenewManager.RefreshSession(true) == SessionStatus.InvalidOrScavenged)
                {
                    AuthenticationManager.Login();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    SessionRenewException sessionException = ParseRawExceotion(ex) as SessionRenewException;
                    //if (null != sessionException && SessionStatus.InvalidOrScavenged == sessionException.SessionStatus)
                    //{
                    //    AuthenticationManager.Login();
                    //}

                    if (sessionException != null)
                    {
                        _isSessionKilled = ExceptionManager.IsSessionKilled(ex);
                        ExceptionPolicy.HandleException(sessionException, "SessionRenew Policy");
                    }
                    else
                    {
                        ExceptionPolicy.HandleException(ex, "SessionRenew Policy");
                    }
                }
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {
                    //swall exception
                }
            }
        }
    }
}

using System;
using System.Threading;
using System.Windows.Forms;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Security.AccessControl.BusinessEntity;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

using HiiP.Framework.Security.AccessControl.Authentication;
using HiiP.Framework.Common.Client;
using NCS.IConnect.Messaging;

namespace HiiP.Framework.Security.AccessControl.SessionRenew.ExceptionHandlers
{
    [ConfigurationElementType(typeof(SessionRenewHandlerData))]
    public class SessionRenewHandler : IExceptionHandler
    {
        private delegate DialogResult ShowMessageBoxWithSyncCallBack(string message, string caption);
        private static ReaderWriterLock _lock = new ReaderWriterLock();//Lock for ExceptionManager.IsActive
// ReSharper disable UnusedParameter.Local
        public SessionRenewHandler(string caption, Type formatterType, string template, bool includeInnerException)
// ReSharper restore UnusedParameter.Local
        {
        }

        #region IExceptionHandler Members

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
 
            SessionRenewException sessionException = SessionRefreshService.ParseRawExceotion(exception) as SessionRenewException;
            if (null == sessionException)
            {
                return exception;
            }

            if (sessionException.SessionStatus != SessionStatus.Timeout &&
                sessionException.SessionStatus != SessionStatus.Killed &&
                sessionException.SessionStatus != SessionStatus.NotMatchUserName &&
                sessionException.SessionStatus != SessionStatus.InvalidOrScavenged)
            {
                return exception;
            }

            try
            {
                //If another thread already set the value, then no need to prompt
                var originalValue = ExceptionManager.IsActive;
                if (!originalValue)
                {
                    return exception;
                }
                _lock.AcquireWriterLock(-1);
                //If two threads both to pass the previous check at the same time, then the first thread can pass the following check. 
                //But the second thread will have to wait for the first thread to complete. Then, when the second thread comes to here
                //The originalValue will be different with the current value of ExceptionManager.IsActive
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (originalValue != ExceptionManager.IsActive && !ExceptionManager.IsActive)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                {
                    return exception;
                }
                ExceptionManager.IsActive = false;
                switch (sessionException.SessionStatus)
                {
                    case SessionStatus.Timeout:
                        {
                            if (AuthenticationMode.Forms == AppContext.Current.AuthenticationMode)
                            {
                                if (DialogResult.OK == ShowErrorMessage(sessionException))
                                {
                                    if (AuthenticationManager.ShowLoginFormWhenSessionTimeout())
                                    {
                                        ExceptionManager.IsActive = true;
                                        return exception;
                                    }

                                    ShutDown();

                                }
                            }
                            break;
                        }
                    case SessionStatus.Killed:
                        {
                            if (DialogResult.OK == ShowErrorMessage(sessionException))
                            {
                                ShutDown();
                            }
                            break;
                        }
                    case SessionStatus.NotMatchUserName:
                        {
                            if (DialogResult.OK == ShowErrorMessage(sessionException))
                            {
                                ShutDown();
                            }
                            break;
                        }
                    case SessionStatus.InvalidOrScavenged:
                        {
                            if (AuthenticationMode.Forms == AppContext.Current.AuthenticationMode)
                            {
                                if (DialogResult.OK == ShowErrorMessage(sessionException))
                                {
                                    if (AuthenticationManager.ShowLoginFormWhenSessionTimeout())
                                    {
                                        ExceptionManager.IsActive = true;
                                        return exception;
                                    }

                                    ShutDown();

                                }
                            }
                            else
                            {
                                AuthenticationManager.Login();
                            }
                            break;
                        }
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.IsActive = true;
                ExceptionManager.HandleWithLogOnly(ex);
            }
            finally
            {
                if (ExceptionManager.IsActive)
                {
                    _lock.ReleaseLock();
                }
            }
            return exception;
        }

        private DialogResult ShowErrorMessage(Exception ex)
        {
            var callback = new ShowMessageBoxWithSyncCallBack(ShowMessageBoxWithSync);
            string caption = MessageSeverity.Error.ToString();
            var result = Utility.InvokeWithUIThread<DialogResult>(ExceptionManager.MainForm, callback, ex.Message, caption);
            
            return result;
        }

        private static DialogResult ShowMessageBoxWithSync( string message, string caption)
        {
            ExceptionManager.ShowingErrorMessageBox = true;
            var result = MessageBox.Show(ExceptionManager.MainForm, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            ExceptionManager.ShowingErrorMessageBox = false;
            return result;
        }

        #endregion

        private void ShutDown()
        {
            try
            {
                Application.Exit();
            }
            catch
            {
                ShutDown();
            }
        }
    }
}

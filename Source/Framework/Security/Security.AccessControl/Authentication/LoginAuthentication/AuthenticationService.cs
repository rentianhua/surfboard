using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.AccessControl.CallHandlers;
using HiiP.Framework.Security.AccessControl.Properties;

using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.Miscellaneous;
using HiiP.Infrastructure.Interface.Services;

using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.ObjectBuilder;

namespace HiiP.Framework.Security.AccessControl.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        static AuthenticationService()
        {
            AuthenticationManager.AuthenticationFailed += delegate {
            };           
        }

        private IUserSelectorService _userSelector;
        private WorkItem _rootWorkItem;
		
		[InjectionConstructor]
            public AuthenticationService([ServiceDependency]WorkItem workItem, [ServiceDependency] IUserSelectorService userSelector)
		{
			_userSelector = userSelector;
            if (workItem.RootWorkItem == null)
            {
                _rootWorkItem = workItem;
            }
            else
            {
                _rootWorkItem = workItem.RootWorkItem;
            }
		}

        #region IAuthenticationService Members

        public void Authenticate()
        {
            //********************Trace by Jin Nan********************
            Trace.Write("Authenticate User [Begin]");
            //********************Trace by Jin Nan********************
            SplashFormUtility splashUtility = null;

            try
            {
                #region Authentication
                //Integrated Authentication
                if (AuthenticationManager.AuthenticationMode == AuthenticationMode.Integration)
                {
                    // *** Shell start point *** 
                    // *** Write start tick for logging ***
                    // *** Integration authentication mode
                    // *** Write by Ya Ming ***
                    LoggingVariables.SetLoginStartTick();

                    splashUtility = new SplashFormUtility();
                    splashUtility.ShowSplashForm(_rootWorkItem);

                    try
                    {
                        splashUtility.SetStatusText("Authenticating, please wait...");
                        Login();

                    }
                    catch (ProtocolException)
                    {
                        splashUtility.SetStatusText("Authenticating, please wait...");
                        Login();
                    }
                }
                else
                {
                    //Forms Authentication
                    string userName = _userSelector.SelectUser();
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw new ApplicationExitException();
                    }
                }
                #endregion
                PopulateContextAndWriteLog();
                _rootWorkItem.Services.Get<ISessionRefresh>().StartAutoRefreshScheduler();
            }
            catch (Exception ex)
            {

                if (splashUtility != null)
                {
                    splashUtility.Close();
                }
                ExceptionManager.HandleForLogin(HandleWebSealException(ex));

                if (AuthenticationManager.AuthenticationMode == AuthenticationMode.Integration)
                {
                    throw new ApplicationExitException();
                }

                if (string.IsNullOrEmpty(AppContext.Current.ApplicationRestarting))
                {
                    AppContext.Current.ApplicationRestarting = "true";
                    Application.Restart();
                }
                else
                {
                    Thread.Sleep(1000);
                }

            }
            //********************Trace by Jin Nan********************
            Trace.Write("Authenticate User [End]");
            //********************Trace by Jin Nan********************
        }


       internal static Exception HandleWebSealException(Exception currentException)
        {
            if (HiiP.Framework.Security.AccessControl.Authentication.AuthenticationManager.AuthenticationMode == AuthenticationMode.Forms)
            {
                //Modify to resolve the password is expired by Qiu Ming on 05/12/2008
                string passwordExpiredMessage = "<meta name =\"DC.Title\" content=\"Expired Password,";
                if (currentException.Message.Contains(passwordExpiredMessage)) return (new UserPasswordExpiredException());
            }
            else
            {
                CommunicationException securityException = currentException as CommunicationException;
                if (securityException != null)
                {
                    //Application crashes when launched while Active Directory credentials are expired (often due to not logging off overnight)
                    string ADExpired = "Your credentials have expired";
                    if (securityException.Message.Contains(ADExpired))
                    {
                        return (new ADCredentialsExpiredException());
                    }
                }
            }
            return currentException;
        }

        private static void Login()
        {
            if (!AuthenticationManager.Login())
            {
                MessageBox.Show(Resources.NotAuthorizedUser, "Login failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ApplicationExitException();
            }
        }

        //private static string GetIPAddress()
        //{
        //    string hostName = System.Net.Dns.GetHostName();
        //     string ipAddress = string.Empty;
        //    var addressList = System.Net.Dns.GetHostEntry(hostName).AddressList;
        //    foreach (var address in addressList)
        //    {
        //        ipAddress += address + ",";
        //    }
        //    return ipAddress.TrimEnd(",".ToCharArray());
        //}

        private static void PopulateContextAndWriteLog()
        {
            long? endTick = Stopwatch.GetTimestamp();
            long elapsedMilliseconds = LoggingVariables.LoginStopwatch.ElapsedMilliseconds;
            SecurityLogger.WriteLogWhenLoginSuccessful(
                LoggingVariables.LoginStartTick,
                endTick,
                elapsedMilliseconds);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using HiiP.Framework.Common;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.AccessControl.Interface;
using HiiP.Framework.Security.AccessControl.Interface.Configuration;
using HiiP.Framework.Security.AccessControl.Interface.Constants;

namespace HiiP.Framework.Security.AccessControl.Authentication
{
    public static class AuthenticationManager
    {
        private delegate bool ShowLoginFormWithSyncCallBack(bool isCookieOut);
        #region Private Methods

        private static string GetCookieValue(string userName, string password)
        {
            userName = HttpUtility.UrlEncode(userName);
            password = HttpUtility.UrlEncode(password);
            string cookies ;

            WebSealAuthenticationSection webSealAuthenticationSection = WebSealSettings.GetWebSealAuthenticationSetting();
            Uri formLoginUri = new Uri(webSealAuthenticationSection.FormsLoginURL);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(formLoginUri);
            request.AllowAutoRedirect = false;

            //Prepare Post Data
            ASCIIEncoding encoding = new ASCIIEncoding();
            string authenticationDetails = WebSealSettings.GetUserNameFieldName() + "=" + userName + "&" + WebSealSettings.GetPasswordFieldName() + "=" + password;
            foreach (FormsFieldElement field in WebSealSettings.GetFormsFieldSetting())
            {
                //Only add the non username and password fields now.
                if ((string.Compare(field.FieldType, Constant.FormFiledType.UserName, StringComparison.OrdinalIgnoreCase) != 0)
                    && (string.Compare(field.FieldType, Constant.FormFiledType.Password, StringComparison.OrdinalIgnoreCase) != 0))
                {
                    authenticationDetails += "&" + field.Name + "=" + field.Value;
                }
            }

            byte[] data = encoding.GetBytes(authenticationDetails);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            if (formLoginUri.Scheme.Equals("https")) ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(DoCertificateValidation);

            //Post The Data
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
            }

            //Get The Response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    if (AuthenticationFailed != null)
                    {
                        AuthenticationFailed(null, new AuthenticationEventArgs { UserName = userName });
                    }
                }

                cookies = response.Headers["Set-Cookie"];
            }

            //Resovle form authencation's cookies on 25/07/208 by Qiu Ming
            //But changed by Maung2 for Workspace application at 12/08/2011
            if (cookies == null) return cookies;

            StringBuilder sb = new StringBuilder();
                            
            foreach (string s in cookies.Split(','))
            {
                sb.Append(s.Split(';')[0]);
                sb.Append("; ");
            }
            sb.Remove(sb.Length-2, 2);//remove last 2 char
            sb.Append(";");
            return sb.ToString();
        }

        private static X509Certificate _currentCertificate;
        public static bool DoCertificateValidation(object sender,X509Certificate certificate, X509Chain chain,SslPolicyErrors sslPolicyErrors)
        {
            if (_currentCertificate == null)   _currentCertificate = certificate;
            return true;
        }

        #endregion

        #region Static Properties       
        internal static WebSealAuthenticationSection WebSealAuthenticationSetting;

        [ThreadStatic]
        public static string LoginEndpointName;

        public static AuthenticationMode AuthenticationMode
        { get; set; }

        public static DateTime LastLoginDate
        { get; set; }

        #endregion

        #region Type constructor

        /// <summary>
        /// Type initialier.
        /// </summary>
        static AuthenticationManager()
        {
            WebSealAuthenticationSetting = WebSealSettings.GetWebSealAuthenticationSetting();
            LoginEndpointName = WebSealAuthenticationSetting.EndpointName;
            if (WebSealAuthenticationSetting.AuthenticationMode == Constant.AuthenticationMode.FormsAuthentication)
            {
                AuthenticationMode = AuthenticationMode.Forms;
                AppContext.Current.AuthenticationMode = AuthenticationMode.Forms;
            }
            else
            {
                AuthenticationMode = AuthenticationMode.Integration;
                AppContext.Current.AuthenticationMode = AuthenticationMode.Integration;
            }            
        }
        #endregion

        #region Public Events

        /// <summary>
        /// A event which will be fired when failed to authenticate.
        /// </summary>
        public static event EventHandler<AuthenticationEventArgs> AuthenticationFailed;

        public static event EventHandler CookieExpired;

        public static void OnCookieExpired()
        {
            if (CookieExpired != null)
            {
                CookieExpired(null, new EventArgs());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Forms authentication based login.
        /// </summary>
        /// <param name="userName">Ueser name</param>
        /// <param name="password">Password</param>
        /// <param name="loginWhenCookieExpired"></param>
        /// <returns>A bool value indicating whether to sucessfully login.</returns>
        public static bool Login(string userName, string password, params bool[] loginWhenCookieExpired)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            //Get security token in the form of cookie.
            string cookies = GetCookieValue(userName, password);
            if (string.IsNullOrEmpty(cookies))
            {
                //GetCookieValue() will send one request with the user name & password. When they are valid, it will return cookie.
                if (AuthenticationFailed != null)
                {
                    AuthenticationFailed(null, new AuthenticationEventArgs { UserName = userName });
                }
                return false;
            }
            AppContext.Current.CredentialCookie = cookies;

            if ((loginWhenCookieExpired.Length > 0) && (loginWhenCookieExpired[0])) return true;

            //When session time out, need to relogin
            ILoginService loginProxy = null;

            try
            {
                loginProxy = InstanceBuilder.Wrap<ILoginService>(new LoginProxy());
                string hostName = System.Net.Dns.GetHostName();
                string ipAddress = CultureUtility.GetIPAddress();
                IServiceProxy serviceProxy = loginProxy as IServiceProxy;

                if (serviceProxy==null)
                {
                    throw new BusinessException("LoginProxy must be the instance of IServiceProxy");
                }
                var channel = (serviceProxy).Channel as IContextChannel;
                if (channel == null)
                {
                    throw new BusinessException("LoginProxy's Channel must be the instance of IContextChannel");
                }
                using (new OperationContextScope(channel))
                {
                    //Attach security token into outgoing request message.
                    HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                    request.Headers.Add(HttpRequestHeader.Cookie, AppContext.Current.CredentialCookie);
                    OperationContext.Current.OutgoingMessageProperties.Add(HttpRequestMessageProperty.Name, request);

                    bool isAuthenticated ;
                    try
                    {
                        MembershipUserInfo userInfo;
                        SessionData sessionData;
                        isAuthenticated = loginProxy.Login(ipAddress, hostName, out userInfo, out sessionData);

                        if (!isAuthenticated)
                        {
                            if (AuthenticationFailed != null)
                            {
                                AuthenticationFailed(null, new AuthenticationEventArgs { UserName = userName });
                            }
                        }

                        SetContext(hostName, ipAddress, userInfo, sessionData);
                        return isAuthenticated;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "The remote server returned an error: (403) Forbidden.")
                        {
                            if (AuthenticationFailed != null)
                            {
                                AuthenticationFailed(null, new AuthenticationEventArgs { UserName = userName });
                            }
                            return false;
                        }

                        if (ex.InnerException == null)
                        {
                            throw;
                        }

                        if (ex.InnerException.Message == "The remote server returned an error: (403) Forbidden.")
                        {
                            if (AuthenticationFailed != null)
                            {
                                AuthenticationFailed(null, new AuthenticationEventArgs { UserName = userName });
                            }
                            return false;
                        }

                        throw;
                    }
                }
            }
            finally
            {
                IServiceProxy proxy = loginProxy as IServiceProxy;
                if (null != proxy)
                {
                    proxy.Dispose();
                }
            }

        }

        /// <summary>
        /// Login (Integrated Authentication)
        /// </summary>
        /// <returns></returns>
        public static bool Login()
        {
            var identity = WindowsIdentity.GetCurrent();

            if (identity!=null)
            {
                AppContext.Current.UserName = identity.Name.Split("\\".ToCharArray())[1];
            }
            ILoginService loginProxy = null;

            try
            {

                loginProxy = InstanceBuilder.Wrap<ILoginService>(new LoginProxy());
                IServiceProxy serviceProxy = loginProxy as IServiceProxy;

                if (serviceProxy == null)
                {
                    throw new BusinessException("LoginProxy must be the instance of IServiceProxy");
                }

                IContextChannel contextChannel = serviceProxy.Channel as IContextChannel;

                if (contextChannel==null)
                {
                    throw new BusinessException("serviceProxy.Channel must be the instance of IContextChannel.");
                }

                string hostName = System.Net.Dns.GetHostName();
                string ipAddress = CultureUtility.GetIPAddress();
                using (new OperationContextScope(contextChannel))
                {
                    MembershipUserInfo userInfo;
                    SessionData sessionData;

                    bool isAuthenticated = loginProxy.Login(ipAddress, hostName, out userInfo, out  sessionData);

                    if (!isAuthenticated)
                    {
                        if (AuthenticationFailed != null)
                        {
                            AuthenticationFailed(null, new AuthenticationEventArgs { UserName = string.Empty });
                        }

                        return false;
                    }

                    SetContext(hostName, ipAddress, userInfo, sessionData);

                    //Caching the credential cookie.
                    HttpResponseMessageProperty response = OperationContext.Current.IncomingMessageProperties[HttpResponseMessageProperty.Name]
                        as HttpResponseMessageProperty;

                    if (response==null)
                    {
                        throw new BusinessException("No response");
                    }
                    string cookies = response.Headers["Set-Cookie"];

                    AppContext.Current.CredentialCookie = ResolveCookiesForTAM(cookies);

                    //Without real TAM in Dev Environment.
                    if (string.IsNullOrEmpty(AppContext.Current.CredentialCookie))
                    {
                        WebSealAuthenticationSection webSealSetting = WebSealSettings.GetWebSealAuthenticationSetting();
                        AppContext.Current.CredentialCookie = string.Format("{0}={1}; path=/,ASP.NET_SessionId=zegytv2besun4h4524ucmfu1; path=/; HttpOnly", webSealSetting.FormsCookieName, AppContext.Current.UserName);
                    }
                    return true;
                }
            }
            finally
            {
                IServiceProxy proxy = loginProxy as IServiceProxy;
                if (null != proxy)
                {
                    proxy.Dispose();
                }
            }
        }

        private static void SetContext(string hostName, string ipAddress, MembershipUserInfo userInfo, SessionData sessionData)
        {
            //Set the contextual user name and Last Login Time.
            AppContext.Current.UserName = userInfo.UserName;
            AppContext.Current.UserID = userInfo.UserID;
            if (sessionData != null)
            {
                AppContext.Current.SessionID = sessionData.SessionID;
                AppContext.Current.SessionRefreshInterval = sessionData.RefreshInterval.TotalSeconds.ToString();
                AppContext.Current.SessionTimeoutInterval = sessionData.SessionTimeoutInterval.TotalSeconds.ToString();
            }
            else
            {
                AppContext.Current.SessionID = string.Empty;
                AppContext.Current.SessionRefreshInterval = string.Empty;
                AppContext.Current.SessionTimeoutInterval = string.Empty;
            }

            //Notice HiiPBatchJobBase has similiar codes.
            AppContext.Current.UserRoles = string.Join(",", userInfo.Roles);
            AppContext.Current.IPAddress = ipAddress??"";
            AppContext.Current.HostName = hostName??"";
            AppContext.Current.LastLoginDate = userInfo.LastLoginDate;
            // COMMENT: Migirate last login date into AppContext
            AuthenticationManager.LastLoginDate = userInfo.LastLoginDate;
            //AppContext.Current.Organization = userInfo.Organization??"";
            AppContext.Current.FullName = userInfo.FullName;
            AppContext.Current.Office = userInfo.Office??"";
            AppContext.Current.OfficeID = userInfo.OfficeID ?? "";
            //TODO, no confirmation for 'GraphicArea'
            AppContext.Current.GraphicArea = "Victoria";
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userInfo.UserName), userInfo.Roles);
        }


        private static string ResolveCookiesForTAM(string cookies)
        {
            string cookieData = string.Empty;
            string integratedStatefulCookieName = WebSealAuthenticationSetting.IntegratedStatefulCookieName;
            string integratedCookieName = WebSealAuthenticationSetting.IntegratedCookieName;

            const string splitCookieConst = "Path=/";

            cookies = cookies.Replace(splitCookieConst, string.Empty);
            string[] cookieList = cookies.Split(',');
            //Cookie:  PD_STATEFUL_db7c0b40-0fd8-11dc-9655-ac11607aaa77=%2Fhiipur; 
            //Path=/,PD-H-SESSION-ID=4_d-wouLJ6p+C0AGQun1WO5eFyCpsAZ25QnSl-bGbSyw5w0aHf; Path=/
            List<string> returnList = new List<string>();

            foreach (string cookie in cookieList)
            {
                if (cookie.IndexOf(integratedStatefulCookieName) > -1)
                {
                    returnList.Add(cookie.TrimEnd(';'));
                }
                if (cookie.IndexOf(integratedCookieName) > -1)
                {
                    returnList.Add(cookie.TrimStart(','));
                }
            }
            if ((returnList.Count > 1) && (returnList[0].IndexOf(integratedStatefulCookieName) > -1)) returnList.Reverse();
            int current = 0;
            foreach (string cookie in returnList)
            {
                if (current == 0)
                {
                    if (cookie.LastIndexOf(';') < 0)
                    {
                        cookieData = cookieData + cookie + ";";
                    }
                    else
                    {
                        cookieData = cookieData + cookie;
                    }
                }
                else
                {
                    cookieData = cookieData + cookie;
                }
                current = current + 1;
            }
            return cookieData;
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public static bool Logout()
        {
            if (WebSealAuthenticationSetting.AuthenticationMode != Constant.AuthenticationMode.FormsAuthentication)
            {
                return true;
            }

            string logoutUrl = WebSealAuthenticationSetting.FormsLogoutURL;

            if (string.IsNullOrEmpty(logoutUrl) || string.IsNullOrEmpty(AppContext.Current.CredentialCookie))
            {
                return true;
            }

            Uri formsLogoutURI = new Uri(logoutUrl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(formsLogoutURI);
            request.AllowAutoRedirect = false;

            request.CookieContainer = new CookieContainer();

            string cookieName = WebSealAuthenticationSetting.FormsCookieName;
            string cookieValue = AppContext.Current.CredentialCookie.Split(";".ToCharArray())[0].Split("=".ToCharArray())[1];

            Cookie authenticationCookie = new Cookie(cookieName, cookieValue, "/", formsLogoutURI.Host);

            request.CookieContainer.Add(authenticationCookie);

            //Get The Response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new AuthenticationException(Messages.Framework.FWE305.Format());
                }
            }

            return true;
        }
        #endregion

        public static Form ShellForm
        {
            get;
            set;
        }

        public static bool ShowLoginFormWhenSessionTimeout()
        {
            if (IsOpenLoginForm)
            {
                //When it is opening login, cannot return false. otherwise SessionRenewHandler will shutdown application
                return true;
            }
            //Suppress showing other message.
            ExceptionManager.ShowingErrorMessageBox = true;
            return Utility.InvokeWithUIThread<bool>(ExceptionManager.MainForm, new ShowLoginFormWithSyncCallBack(ShowLoginFormWithSync),false);
        }

        public static bool LoginFormOnCookieOut()
        {
            if (IsOpenLoginForm)
            {
                //When it is opening login, cannot return false. otherwise SessionRenewHandler will shutdown application
                return true;
            }
            //Suppress showing other message.
            ExceptionManager.ShowingErrorMessageBox = true;
            return Utility.InvokeWithUIThread<bool>(ExceptionManager.MainForm, new ShowLoginFormWithSyncCallBack(ShowLoginFormWithSync),true);
        }
        private static bool ShowLoginFormWithSync(bool isCookieOut)
        {
            using (LoginForm form = new LoginForm(isCookieOut))
            {
                form.SetUsernameReadonly();
                IsOpenLoginForm = true;
                form.TopMost = true;
                DialogResult result = form.ShowDialog();
                ExceptionManager.ShowingErrorMessageBox = false;
                IsOpenLoginForm = false;
                return result == DialogResult.OK;
            }
        }



        internal static bool IsOpenLoginForm
        {
            get;
            set;
        }
    }
}

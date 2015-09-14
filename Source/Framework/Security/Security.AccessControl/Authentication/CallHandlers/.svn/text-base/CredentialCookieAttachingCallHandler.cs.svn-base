using System.ServiceModel;
using System.ServiceModel.Channels;
using HiiP.Framework.Common;
using HiiP.Framework.Common.ApplicationContexts;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Net;
using System;
using HiiP.Framework.Security.AccessControl.Interface.Configuration;
using HiiP.Framework.Common.Client;
using System.Collections.Generic;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.AccessControl.SessionRenew;
using HiiP.Framework.Security.AccessControl.BusinessEntity;

namespace HiiP.Framework.Security.AccessControl.CallHandlers
{
    /// <summary>
    /// A call handler used to automatically attach credential cookie.
    /// </summary>
    [ConfigurationElementType(typeof (CredentialCookieAttachingCallHandlerData))]
    public class CredentialCookieAttachingCallHandler : CallHandlerBase
    {
        private static Dictionary<string,Exception> _eBusinessGatewayExceptions = new Dictionary<string,Exception>();
        private static object _sync = new object();
        /// <summary>
        /// Performs the operation of the handler.
        /// </summary>
        /// <param name="input">Input to the method call.</param>
        /// <param name="getNext">Delegate used to get the next delegate in the call handler pipeline.</param>
        /// <returns> Return value from the target.</returns>
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Client call: {0}.{1}", input.MethodBase.Module.Name, input.MethodBase.Name));
            OperationContextScope contextScope = null;
            IContextChannel contextChannel ;

            contextChannel = input.Target as IContextChannel;
            if (contextChannel == null)
            {
                IServiceProxy serviveProxy = input.Target as IServiceProxy;
                if (serviveProxy != null)
                {
                    contextChannel = serviveProxy.Channel as IContextChannel;
                }
            }

            if (contextChannel == null)
            {
                return getNext()(input, getNext);
            }

            if (OperationContext.Current == null)
            {
                contextScope = new OperationContextScope(contextChannel);
            }

            IMethodReturn methodReturn = this.AttachCredentialCookie(input, getNext);
            Exception currentException = methodReturn.Exception;
            if (HiiP.Framework.Security.AccessControl.Authentication.AuthenticationManager.AuthenticationMode == AuthenticationMode.Forms)
            {
                if (currentException != null)
                {
                    methodReturn = this.HandleCookieExpirationExceptionForForm(currentException, input, getNext);
                }
            }
            else
            {
                CommunicationException securityException = currentException as CommunicationException;
                if (securityException != null)
                {
                    methodReturn = this.HandleCookieExpirationExceptionForIntegration(securityException, input, getNext);
                }
            }

            if (contextScope != null)
            {
                contextScope.Dispose();
            }

            return methodReturn;
        }


        private IMethodReturn HandleCookieExpirationExceptionForForm(Exception exception, IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            //Modify to resolve the password is expired by Qiu Ming on 05/12/2008
            //string passwordExpiredMessage = "<meta name =\"DC.Title\" content=\"Expired Password,";
            string passwordExpiredMessage = WebSealSettings.GetPasswordExpiredExceptionKeySetting();
            if (!string.IsNullOrEmpty(passwordExpiredMessage) 
                && exception.Message.Contains(passwordExpiredMessage))
            {
                //It will immediately quit
                return input.CreateExceptionMethodReturn(new UserPasswordExpiredException());
            }

            //Sessino is expired
            //string sessionExpiredKey = "BEGIN Cookie check block";
            string sessionExpiredKey = WebSealSettings.GetSessionExpiredExceptionKeySetting();
            if (!string.IsNullOrEmpty(sessionExpiredKey)
                && exception.Message.Contains(sessionExpiredKey))
            {

                try
                {

                    if (!(input.MethodBase.Module.Name.Contains(".Logging.") && input.MethodBase.Name == "Write")
                       &&  (AppContext.Current.UserID.Length > 0)
                      && (!HiiP.Framework.Security.AccessControl.Authentication.AuthenticationManager.IsOpenLoginForm)
                        )
                    {
                        ExceptionManager.ShowingErrorMessageBox = true;

                        var friendlyExMessage = WebSealSettings.GetSessionExpiredExceptionMessageSetting();

                        if (string.IsNullOrEmpty(friendlyExMessage))
                        {
                            friendlyExMessage = "Your eBusiness Gateway session has expired. Please re-login.";
                        }

                        SessionCredentialsExpiredException sessionCredentialsExpiredException = new SessionCredentialsExpiredException(friendlyExMessage);
                        ExtendedMessageBoxExceptionHandler _messageHandler = new ExtendedMessageBoxExceptionHandler("Error", typeof(NCS.IConnect.ExceptionHandling.TextExceptionFormatter), "{message}", true);
                        _messageHandler.HandleException(sessionCredentialsExpiredException, Guid.NewGuid());

                        StoreExceptionInMemory(sessionCredentialsExpiredException,true);

                        if (HiiP.Framework.Security.AccessControl.Authentication.AuthenticationManager.LoginFormOnCookieOut())
                        {
                            StoreExceptionInMemory(null, false);
                            return this.Invoke(input, getNext);
                        }

                        //return input.CreateExceptionMethodReturn(new SessionCredentialsExpiredException());
                    }
                    return input.CreateExceptionMethodReturn(new SessionCredentialsExpiredException());
                }
                finally
                {
                    ExceptionManager.ShowingErrorMessageBox = false;
                }

            }
            return input.CreateExceptionMethodReturn(exception);
        }

        private static void StoreExceptionInMemory(Exception exception,bool toStore)
        {
            string key = (exception == null) ? "" : exception.GetType().FullName;
            lock (_sync)
            {
                if (toStore 
                    && !string.IsNullOrEmpty(key) 
                    && !_eBusinessGatewayExceptions.ContainsKey(key))
                {
                    _eBusinessGatewayExceptions.Add(key, exception);
                }
                else if (!toStore)
                {
                    foreach (var item in _eBusinessGatewayExceptions)
                    {
                        ExceptionManager.HandleWithLogOnly(item.Value);
                    }
                    _eBusinessGatewayExceptions.Clear();
                }
            }
        }

        private IMethodReturn HandleCookieExpirationExceptionForIntegration(CommunicationException exception, IMethodInvocation input, GetNextHandlerDelegate getNext)
        {                
            //Application crashes when launched while Active Directory credentials are expired (often due to not logging off overnight)
             //string ADExpired = "Your credentials have expired";
            string ADExpired = WebSealSettings.GetADAccountExpiredExceptionKeySetting();
             if (exception.Message.Contains(ADExpired))
             {
                 //It will immediately quit
                 return input.CreateExceptionMethodReturn(new ADCredentialsExpiredException());
             }

            WebException webException = exception.InnerException as WebException;
             if (webException == null)
             {
                 //Even if it is sessionRenewException, also will come to here.
                 return input.CreateExceptionMethodReturn(exception);
             }

             if ((!webException.Message.Contains("403")) && (!webException.Message.Contains("401")))
             {
                 return input.CreateExceptionMethodReturn(exception);
             }
             
             if (HiiP.Framework.Security.AccessControl.Authentication.AuthenticationManager.AuthenticationMode == AuthenticationMode.Integration)
             {
                 HiiP.Framework.Security.AccessControl.Authentication.AuthenticationManager.Login();
                 return this.Invoke(input, getNext);
             }

             return input.CreateExceptionMethodReturn(exception); 
        }

 

        private IMethodReturn AttachCredentialCookie(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            HttpRequestMessageProperty request;
            if (OperationContext.Current.OutgoingMessageProperties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                request = OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            }
            else
            {
                request = new HttpRequestMessageProperty();
            }

            if (string.IsNullOrEmpty(AppContext.Current.CredentialCookie))
            {
                return input.CreateExceptionMethodReturn(new CredentialCookieAttachingException("No credential cookie is attached in HTTP header."));
            }

            if (request==null)
            {
                throw new BusinessException("No valid request");
            }

            request.Headers[HttpRequestHeader.Cookie] = AppContext.Current.CredentialCookie;
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;

            return getNext()(input, getNext);
        }
    }
}
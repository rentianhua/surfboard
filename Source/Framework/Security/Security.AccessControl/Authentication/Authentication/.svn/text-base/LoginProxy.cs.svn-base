using System;
using System.ServiceModel;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Security.AccessControl.Interface;
using HiiP.Framework.Common.ApplicationContexts.CallHandlers;

namespace HiiP.Framework.Security.AccessControl.Authentication
{
    public sealed class LoginProxy : ILoginService, IDisposable, IServiceProxy
    {
        public ILoginService Channel
        { get; set; }

        string IServiceProxy.EndpointName
        {
            get
            {
                return AuthenticationManager.LoginEndpointName;
            }
            set
            {
                //ignore
            }
        }

        public LoginProxy()
        {
            var endPointName = string.IsNullOrEmpty(AuthenticationManager.LoginEndpointName) ? AuthenticationManager.WebSealAuthenticationSetting.EndpointName : AuthenticationManager.LoginEndpointName;
            ChannelFactory<ILoginService> channelFactory = new ChannelFactory<ILoginService>(endPointName);
            this.Channel = channelFactory.CreateChannel();
            (this as IServiceProxy).Channel = this.Channel;
        }

        #region ILoginService Members

        [ContextPropagationCallHandler(Location = ContextDomainScope.Client)]
        public bool Login(string ipAddress, string hostName, out MembershipUserInfo userInfo, out SessionData sessionData)
        {
            return this.Channel.Login(ipAddress, hostName, out userInfo, out sessionData);
        }
      
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            IDisposable disposable = this.Channel as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        #endregion

        #region IServiceProxy Members

        object IServiceProxy.Channel
        {
            get;
            set;
        }

        #endregion
    }
}

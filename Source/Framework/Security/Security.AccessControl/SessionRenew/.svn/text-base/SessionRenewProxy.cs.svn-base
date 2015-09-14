using System;
using System.ServiceModel;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Security.AccessControl.BusinessEntity;
using HiiP.Framework.Security.AccessControl.Interface;


namespace HiiP.Framework.Security.AccessControl.SessionRenew
{
    public sealed class SessionRenewProxy : IServiceProxy, ISessionRenewService
    {
        internal static string EndpointName = "SessionRenewService";

        private ISessionRenewService Channel
        { get; set; }

        public SessionRenewProxy()
        {
            ChannelFactory<ISessionRenewService> channelFactory = new ChannelFactory<ISessionRenewService>(EndpointName);
            this.Channel = channelFactory.CreateChannel();
            (this as IServiceProxy).Channel = this.Channel;

        }

        #region IServiceProxy Members

        object IServiceProxy.Channel
        { get; set; }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            IDisposable dispoable = this.Channel as IDisposable;
            if (dispoable != null)
            {
                dispoable.Dispose();
            }
        }

        #endregion

        #region ISessionRenewService Members

        public SessionStatus RefreshSession(string sessionID)
        {
            return this.Channel.RefreshSession(sessionID);
        }

        public void CloseSession(Guid sessionID)
        {
            this.Channel.CloseSession(sessionID);
        }

        #endregion

        #region IServiceProxy Members


        string IServiceProxy.EndpointName
        {
            get
            {
                return EndpointName;
            }
            set
            {
                //ignore
            }
        }

        #endregion
    }
}

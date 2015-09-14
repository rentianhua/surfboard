using System;
using System.Runtime.CompilerServices;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.AccessControl.BusinessEntity;
using HiiP.Framework.Security.AccessControl.Interface;
using HiiP.Framework.Security.AccessControl.Interface.Configuration;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Security.AccessControl.SessionRenew
{
   public static class SessionRenewManager
   {
       internal static DateTime LatestRefreshTime{get; private set;}
       private readonly static TimeSpan _refreshInterval;       

       static SessionRenewManager()
       {
           LatestRefreshTime = DateTime.Now;
           SessionClientSection clientSetting = SessionSettings.GetClientSetting();
           SessionRenewProxy.EndpointName = clientSetting.EndpointName;

           string interval = string.IsNullOrEmpty(AppContext.Current.SessionRefreshInterval) ? "0" : AppContext.Current.SessionRefreshInterval;

           _refreshInterval = new TimeSpan(0, 0, int.Parse(interval));
       }

       /// <summary>
       /// Close the session.
       /// </summary>
       public static void CloseSession()
       {
           if (string.IsNullOrEmpty(AppContext.Current.SessionID))
           {
               return;
           }

           using (SessionRenewProxy sessionRenewProxy = new SessionRenewProxy())
           {
               ISessionRenewService wrappedProxy = InstanceBuilder.Wrap<ISessionRenewService>(sessionRenewProxy);
               wrappedProxy.CloseSession(new Guid(AppContext.Current.SessionID));
           }           
       }

       [MethodImpl(MethodImplOptions.Synchronized)]
       public static SessionStatus RefreshSession(bool refreshImmediately)
       {          
           SessionStatus status;
           //try
           {
               if (!refreshImmediately
                   && ((DateTime.Now - LatestRefreshTime) < _refreshInterval))
               {
                   return SessionStatus.Active;
               }

               if (string.IsNullOrEmpty(AppContext.Current.SessionID))
               {
                   throw new SessionRenewException(Messages.Framework.FWE002.Format(), SessionStatus.UnKnown);
               }

               AppContext.SetToCallContext(AppContext.Current.ToDictionary());
               if (!refreshImmediately)
               {
                   LatestRefreshTime = DateTime.Now;
               }

               using (SessionRenewProxy sessionRenewProxy = new SessionRenewProxy())
               {
                   ISessionRenewService wrappedProxy = InstanceBuilder.Wrap<ISessionRenewService>(sessionRenewProxy);
                   status = wrappedProxy.RefreshSession(AppContext.Current.SessionID);
                   return status;
               }
           }
           //finally
           //{
           //    //ExceptionManager.IsActive = (status==SessionStatus.Active);
           //}

       }
   }
}
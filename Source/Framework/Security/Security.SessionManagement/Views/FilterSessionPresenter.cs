using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Security.SessionManagement.Constants;

namespace HiiP.Framework.Security.SessionManagement
{
    public partial class FilterSessionPresenter : Presenter<IFilterSessionView>
    {
        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            base.OnCloseView();
        }

        #region Event Publication

        [EventPublication(EventTopicNames.FilterSession, PublicationScope.Global)]
        public event EventHandler<EventArgs<SessionCriteria>> FilterSession;

        protected virtual void OnFilterSession(SessionCriteria sessionCriteria)
        {
            if (FilterSession != null)
            {
                FilterSession(this, new EventArgs<SessionCriteria>(sessionCriteria));
            }
        }

        #endregion

        #region Business Logic

        public void FilterSessionByCriteria(string userName,string ipAddress,  string host, DateTime logonTimeStart, DateTime logonTimeEnd, DateTime lastActiveTimeStart, DateTime lastActiveTimeEnd)
        {
            // contruct a session criteria

            SessionCriteria criteria = new SessionCriteria();
            if (!string.IsNullOrEmpty(userName))
            {
                criteria.UserName = userName;
            }

            if (!string.IsNullOrEmpty(ipAddress))
            {
                criteria.IPAddress = ipAddress;
            }

            if (!string.IsNullOrEmpty(host))
            {
                criteria.HostName = host;
            }

            criteria.LoginTimeFrom = logonTimeStart;
            criteria.LoginTimeTill = logonTimeEnd;
            criteria.LastActivityTimeFrom = lastActiveTimeStart;
            criteria.LastActivityTimeTill = lastActiveTimeEnd;
        
            OnFilterSession(criteria);
        }

        #endregion
    }
}


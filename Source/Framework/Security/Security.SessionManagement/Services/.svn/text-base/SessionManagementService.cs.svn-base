using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Framework.Security.SessionManagement.Interface.Services;
using HiiP.Framework.Security.SessionManagement.Interface.Constants;
using HiiP.Framework.Common;
using System.Windows.Forms;
using HiiP.Framework.Security.AccessControl.Interface;
using HiiP.Framework.Security.SessionManagement.ServiceProxy;
using HiiP.Framework.Common.CallHandlers;
using HiiP.Framework.Security.AccessControl.BusinessEntity;

namespace HiiP.Framework.Security.SessionManagement.Services
{
    [Service(typeof(ISessionManagementService))]
    public class SessionManagementService : ISessionManagementService
    {
        public SessionInfo[] GetActiveSessions(SessionCriteria criteria)
        {
            using (var proxy = new SessionProxy())
            {
                if (criteria == null)
                {
                    return proxy.GetActiveSessions();
                }

                return proxy.GetActiveSessions(criteria.UserName, criteria.IPAddress, criteria.HostName,
            criteria.LoginTimeFrom, criteria.LoginTimeTill, criteria.LastActivityTimeFrom, criteria.LastActivityTimeTill);
            }
        }

        public void KillSessions(Guid[] ids)
        {
            using (var proxy = new SessionProxy())
            {
                proxy.KillSessions(ids);
            }
        }
    }
}

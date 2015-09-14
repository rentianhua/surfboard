using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Security.AccessControl.DataAccess;
using HiiP.Framework.Security.AccessControl.BusinessEntity;

namespace HiiP.Framework.Security.AccessControl.BusinessComponent
{
    public class SessionRenewBC
    {
        private SessionRenewDA _da = new SessionRenewDA();

        /// <summary>
        /// Check the session status and renew it.
        /// </summary>
        /// <param name="checkSessionID">Indicate whether need to ignore session check</param>
        /// <param name="sessionID">The session ID unuiquely identify the session.</param>
        /// <param name="timeoutMinutes">The minutes of session timeout.</param>
        /// <param name="renewSession">True, renew session; false, not renew.</param>
        /// <param name="isActiveUser">Return true, ueser is active user; false, inactive.</param>
        /// <param name="sessionStatus">Return the current <see cref="SessionStatus"/> with session ID</param>
        public void AuthenticationCheck(bool checkSessionID, string sessionID, int timeoutMinutes, bool renewSession, out bool isActiveUser, out SessionStatus sessionStatus)
        {
            //it will call same SP of RefreshSession in _da.AuthenticationCheck's SP
            _da.AuthenticationCheck(checkSessionID, sessionID, timeoutMinutes, renewSession, out isActiveUser, out sessionStatus);
        }   


        /// <summary>
        /// Check the session status and renew it.
        /// </summary>
        /// <param name="sessionID">The session ID unuiquely identify the session.</param>
        /// <param name="timeoutMinutes">The minutes of session timeout.</param>
        /// <param name="renewSession">True, renew session; false, not renew.</param>
        /// <returns>The <see cref="SessionStatus"/>indicating the session status.</returns>
        public SessionStatus RefreshSession(string sessionID, int timeoutMinutes, bool renewSession)
        {
            return _da.RefreshSession(sessionID, timeoutMinutes, renewSession);
        }   

        /// <summary>
        /// Delete an existing session.
        /// </summary>
        /// <param name="sessionID">A GUID which uniquely identify a particular session.</param>
        /// <param name="userName"></param>
        /// <param name="isUserSessionMatched"></param>
        public void DeleteSession(Guid sessionID, string userName, out bool isUserSessionMatched)
        {
            _da.DeleteSession(sessionID, userName, out isUserSessionMatched);
        }

        /// <summary>
        /// Ensures the session user consistence.
        /// </summary>
        public void EnsureSessionUserConsistence()
        {
            _da.EnsureSessionUserConsistence();
        }
    }
}

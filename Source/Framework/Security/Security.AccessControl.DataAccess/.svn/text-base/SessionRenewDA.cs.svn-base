using System;
using System.Data.Common;


using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Security.AccessControl.BusinessEntity;

using NCS.IConnect.Helpers.Data;
using HiiP.Framework.Messaging;
using HiiP.Framework.Security.UserManagement.BusinessEntity;

namespace HiiP.Framework.Security.AccessControl.DataAccess
{
    public class SessionRenewDA
    {
        private DbHelper _helper = new DbHelper();

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
            DbCommand command = _helper.BuildDbCommand("P_IC_AUTHENTICATION_CHECK");

            //Init
            isActiveUser = false;
            sessionStatus = SessionStatus.Active;

            int checkSession = checkSessionID ? 1 : 0;
            int renewifActive = renewSession ? 1 : 0;
            int renewIfTimeout = (renewSession && AppContext.Current.AuthenticationMode == AuthenticationMode.Integration) ? 1 : 0;

            _helper.AssignParameterValues(command, checkSession,sessionID, DateTime.Now, timeoutMinutes, AppContext.Current.UserName, renewIfTimeout, renewifActive);
            _helper.ExecuteNonQuery(command);

            var userStatus = _helper.GetParameterValue(command, "@p_user_status");

            if (userStatus != null && userStatus != DBNull.Value)
            {
                isActiveUser = userStatus.ToString().Equals(UserStatus.Active,StringComparison.InvariantCultureIgnoreCase);
            }

            if (!checkSessionID)
            {
                return;
            }

            switch ((int)_helper.GetParameterValue(command, "p_status"))
            {
                case 4:
                    sessionStatus = SessionStatus.NotMatchUserName; 
                    break;
                case 3:
                    sessionStatus = SessionStatus.InvalidOrScavenged;
                    break;
                case 2:
                    sessionStatus = SessionStatus.Killed;
                    break;
                case 1:
                    sessionStatus = SessionStatus.Timeout;
                    break;
                default:
                    sessionStatus = SessionStatus.Active;
                    break;
            }

        }     

        /// <summary>
        /// Check the session status and renew it.
        /// </summary>
        /// <param name="sessionID">The session ID unuiquely identify the session.</param>
        /// <param name="timeoutMinutes">The minutes of session timeout.</param>
        /// <param name="renewSession">True, renew session; false, not renew.</param>
        /// <returns>The <see cref="SessionStatus"/>indicating the session status.</returns>
        public SessionStatus RefreshSession(string sessionID, int timeoutMinutes,bool renewSession)
        {            
            DbCommand command = _helper.BuildDbCommand("P_IC_SESSIONS_CHECK_STATUS");

            int renewifActive = renewSession ? 1 : 0;
            int renewIfTimeout = (renewSession && AppContext.Current.AuthenticationMode == AuthenticationMode.Integration) ? 1 : 0;
            _helper.AssignParameterValues(command, sessionID, DateTime.Now, timeoutMinutes, AppContext.Current.UserName, renewIfTimeout,renewifActive);
            _helper.ExecuteNonQuery(command);
            switch ((int)_helper.GetParameterValue(command, "p_status"))
            {
                case 4: return SessionStatus.NotMatchUserName;
                case 3: return SessionStatus.InvalidOrScavenged;
                case 2: return SessionStatus.Killed;
                case 1: return SessionStatus.Timeout;
                default: return SessionStatus.Active;
            }
        }     

        /// <summary>
        /// Delete an existing session.
        /// </summary>
        /// <param name="sessionID">A GUID which uniquely identify a particular session.</param>
        /// <param name="userName"></param>
        /// <param name="isUserSessionMatched"></param>
        public void DeleteSession(Guid sessionID, string userName, out bool isUserSessionMatched)
        {
            DbCommand command = this._helper.BuildDbCommand("P_IC_SESSIONS_DELETE");
            this._helper.AssignParameterValues(command, new object[] { sessionID.ToString(), userName});
            this._helper.ExecuteNonQuery(command);
            isUserSessionMatched = (bool)this._helper.GetParameterValue(command, "p_session_user_matched");
        }

        public void EnsureSessionUserConsistence()
        {
            DbHelper dbHelper = new DbHelper();
            DbCommand comannd = dbHelper.BuildDbCommand("P_IC_SESSIONS_CHECK_USER_NAME");
            dbHelper.AssignParameterValues(comannd, AppContext.Current.SessionID, AppContext.Current.UserName);
            dbHelper.ExecuteNonQuery(comannd);
            bool isKilled = (bool)dbHelper.GetParameterValue(comannd, "p_is_killed");
            bool userSessionMatched = (bool)dbHelper.GetParameterValue(comannd, "p_user_session_matched");

            if (isKilled)
            {
                throw new SessionRenewException(Messages.Framework.FWE003.Format(), SessionStatus.Killed);
            }

            if (!userSessionMatched)
            {

                throw new SessionRenewException(Messages.Framework.FWE006.Format(), SessionStatus.NotMatchUserName);
            }
        }
    }
}

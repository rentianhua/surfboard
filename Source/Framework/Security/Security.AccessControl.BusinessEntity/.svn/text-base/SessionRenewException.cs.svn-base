using System;
using System.Security.Permissions;
using System.Runtime.Serialization;


namespace HiiP.Framework.Security.AccessControl.BusinessEntity
{
    [global::System.Serializable]
    public class SessionRenewException : Exception
    {
        public SessionStatus SessionStatus{get;  set;}
        public SessionRenewException() { }
        public SessionRenewException(string message)
            : base(message)
        {
            SessionStatus = SessionStatus.UnKnown;
        }
        public SessionRenewException(string message, Exception inner)
            : base(message, inner)
        {
            SessionStatus = SessionStatus.UnKnown;
        }
        public SessionRenewException(string message, SessionStatus status)
            : base(message) 
        {
            SessionStatus = status; 
        }
        public SessionRenewException(string message, SessionStatus status, Exception inner)
            : base(message, inner)
        {
            SessionStatus = status; 
        }
        protected SessionRenewException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            base.GetObjectData(info,context);

            if (info.GetValue("SessionStatus",typeof(SessionStatus))==null)
            {
                info.AddValue("SessionStatus", this.SessionStatus);
            }
        }

    }
}

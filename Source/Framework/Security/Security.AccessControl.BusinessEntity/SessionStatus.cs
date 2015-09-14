using System;
using System.Collections.Generic;
using System.Text;

namespace HiiP.Framework.Security.AccessControl.BusinessEntity
{
    [global::System.Serializable]
    public enum SessionStatus
    {
        UnKnown,
        Active,
        Timeout,
        Killed,
        InvalidOrScavenged,
        NotMatchUserName
    }
}

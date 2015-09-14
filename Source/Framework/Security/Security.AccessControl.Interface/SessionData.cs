using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HiiP.Framework.Security.AccessControl.Interface
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class SessionData
    {
        [DataMember]
        public string SessionID
        { get; set; }

        [DataMember]
        public TimeSpan RefreshInterval
        { get; set; }

        [DataMember]
        public TimeSpan SessionTimeoutInterval
        { get; set; }

        [DataMember]
        public bool IsKilled
        {
            get;
            set;
        }

        [DataMember]
        public bool IsTimeoutOrInvalid
        {
            get;
            set;
        }
        [DataMember]
        public bool IsSessionMatched
        {
            get;
            set;
        }
    }
}

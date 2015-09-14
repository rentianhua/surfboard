using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HiiP.Framework.Security.AccessControl.Interface
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class MembershipUserInfo
    {
        [DataMember]
        public string UserID
        { get; set; }

        [DataMember]
        public string UserName
        { get; set; }

        [DataMember]
        public DateTime LastLoginDate
        { get; set; }

        [DataMember]
        public string Organization
        { get; set; }

        [DataMember]
        public string[] Roles
        {  get;set;}

        [DataMember]
        public string FullName
        { get; set; }
        
        [DataMember]
        public string Office
        { get; set; }

        [DataMember]
        public string OfficeID
        { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Threading;
using System.Net;

namespace HiiP.Framework.Security.AccessControl.BusinessEntity
{
    /// <summary>
    /// Session client information provided by the client application when session initialization.
    /// </summary>
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class SessionClientInfo
    {      
        /// <summary>
        /// User name.
        /// </summary>
        [DataMember]
        public string UserName
        { get; set; }

        [DataMember]
        public string FullName
        { get; set; }

                
        /// <summary>
        /// The IP address of client machine.
        /// </summary>
        [DataMember]
        public string IPAddress
        { get; set; }

        /// <summary>
        /// The host name of client machine.
        /// </summary>
        [DataMember]
        public string HostName
        { get; set; }       
    }
}

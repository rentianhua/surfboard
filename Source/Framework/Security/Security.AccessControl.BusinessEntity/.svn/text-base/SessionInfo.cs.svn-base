using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace HiiP.Framework.Security.AccessControl.BusinessEntity
{
    /// <summary>
    /// Session related information.
    /// </summary>
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class SessionInfo
    {
        /// <summary>
        /// A GUID which uniquely identifies a sesssion.
        /// </summary>
        [DataMember]
        public Guid SessionID
        { get; set; }

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
        /// Session start time.
        /// </summary>
        [DataMember]
        public DateTime StartTime
        { get; set; }

        /// <summary>
        /// Last activity time.
        /// </summary>
        [DataMember]
        public DateTime LastActivityTime
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cedar.Framwork.AuditTrail.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class AuditLogModel
    {
        /// <summary>
        /// Get or set ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Get or set AuditName
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// Get or set AuditType
        /// </summary>
        public string AuditType { get; set; }

        /// <summary>
        /// Get or set TransactionID
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// Get or set LogDateTime
        /// </summary>
        public DateTime LogDateTime { get; set; }

        /// <summary>
        /// Get or set Arguments
        /// </summary>
        public dynamic Arguments { get; set; }

        /// <summary>
        /// Get or set Target
        /// </summary>
        public dynamic Target { get; set; }

        /// <summary>
        /// Get or set Result
        /// </summary>
        public dynamic Result { get; set; }
    }
}

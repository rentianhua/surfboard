using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cedar.Framwork.AuditTrail
{
    /// <summary>
	/// The type of operation to be audited.
	/// </summary>
	public enum OperationType
    {
        /// <summary>
        /// Insert operation
        /// </summary>
        Insert = 1,
        /// <summary>
        /// Update operation
        /// </summary>
        Update,
        /// <summary>
        /// Delete operation
        /// </summary>
        Delete,
        /// <summary>
        /// ExecuteNonQuery/ExecuteScalar operation
        /// </summary>
        ExecuteCommand,
        /// <summary>
        /// The custom
        /// </summary>
        Custom
    }
}

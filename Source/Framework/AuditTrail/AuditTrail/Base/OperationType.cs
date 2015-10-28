namespace Cedar.Framework.AuditTrail.Base
{
    /// <summary>
    ///     The type of operation to be audited.
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        ///     Insert operation
        /// </summary>
        Insert = 1,

        /// <summary>
        ///     Update operation
        /// </summary>
        Update,

        /// <summary>
        ///     Delete operation
        /// </summary>
        Delete,

        /// <summary>
        ///     ExecuteNonQuery/ExecuteScalar operation
        /// </summary>
        ExecuteCommand,

        /// <summary>
        ///     The custom
        /// </summary>
        Custom
    }
}
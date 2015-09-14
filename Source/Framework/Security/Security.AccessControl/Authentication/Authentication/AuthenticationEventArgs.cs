using System;

namespace HiiP.Framework.Security.AccessControl.Authentication
{
    /// <summary>
    /// Event args for Authentication Exception.
    /// </summary>
    public class AuthenticationEventArgs: EventArgs
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace HiiP.Framework.Security.AccessControl.Interface.Configuration
{
    /// <summary>
    /// A ConfigurationSection representing the client side based session configuration.
    /// </summary>
    public class SessionClientSection:ConfigurationSection
    {
      

        /// <summary>
        /// Get or set the endpoint used for cummunication with session management service.
        /// </summary>
        [ConfigurationProperty("endpointName")]
        public string EndpointName
        {
            get
            {
                return (string)this["endpointName"];
            }
            set
            {
                this["endpointName"] = value;
            }
        }
    }
}

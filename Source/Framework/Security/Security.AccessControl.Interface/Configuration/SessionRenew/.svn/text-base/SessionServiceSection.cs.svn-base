using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace HiiP.Framework.Security.AccessControl.Interface.Configuration
{
    /// <summary>
    /// A ConfigurationSection representing the service side based session management configuration.
    /// </summary>
    public class SessionServiceSection : ConfigurationSection
    {
        /// <summary>
        /// Get or set the ducration of delay refresh.
        /// </summary>
        [ConfigurationProperty("refreshInterval", DefaultValue = 1.0)]
        public double RefreshInterval
        {
            get
            {
                return (double)this["refreshInterval"];
            }
            set
            {
                this["refreshInterval"] = value;
            }
        }
        /// <summary>
        /// Get or set he minutes of session timeout duration.
        /// </summary>
        [ConfigurationProperty("sessionTimeout", DefaultValue = 20)]
        public int SessionTimeout
        {
            get
            {
                return (int)this["sessionTimeout"];
            }
            set
            {
                this["sessionTimeout"] = value;
            }
        }

        /// <summary>
        /// Get or set the maxmum of simultaneous sessions per user.
        /// </summary>
        [ConfigurationProperty("maxSimultaneousSessions", DefaultValue = 10)]
        public int MaxSimultaneousSessions
        {
            get
            {
                return (int)this["maxSimultaneousSessions"];
            }
            set
            {
                this["maxSimultaneousSessions"] = value;
            }
        }      
    }
}

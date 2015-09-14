using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace HiiP.Framework.Security.AccessControl.Interface.Configuration
{
    /// <summary>
    /// TAM Web Seal authentication general configuration.
    /// </summary>
    public class WebSealAuthenticationSection: ConfigurationSection
    {
        /// <summary>
        /// Authentication mode: Integrated authentication or Forms authentication.
        /// </summary>
        [ConfigurationProperty("authenticationMode", IsRequired = true)]
        public string AuthenticationMode
        {
            get
            {
                return this["authenticationMode"] as string;
            }

            set
            {
                this["authenticationMode"] = value;
            }
        }

        /// <summary>
        /// Login URL for Forms authentication.
        /// </summary>
        [ConfigurationProperty("formsLoginURL", DefaultValue = "")]
        public string FormsLoginURL
        {
            get { return (string)this["formsLoginURL"]; }
            set { this["formsLoginURL"] = value; }
        }

        /// <summary>
        /// Logout URL for Forms authentication.
        /// </summary>
        [ConfigurationProperty("formsLogoutURL", DefaultValue = "")]
        public string FormsLogoutURL
        {
            get { return (string)this["formsLogoutURL"]; }
            set { this["formsLogoutURL"] = value; }
        }

        /// <summary>
        /// Cookie name for Forms authentication.
        /// </summary>
        [ConfigurationProperty("formsCookieName", DefaultValue = "")]
        public string FormsCookieName
        {
            get { return (string)this["formsCookieName"]; }
            set { this["formsCookieName"] = value; }
        }

        /// <summary>
        /// Cookie name for Integrated authentication.
        /// </summary>
        [ConfigurationProperty("integratedCookieName", DefaultValue = "")]
        public string IntegratedCookieName
        {
            get { return (string)this["integratedCookieName"]; }
            set { this["integratedCookieName"] = value; }
        }

        /// <summary>
        /// Stateful cookie name for Integrated authentication.
        /// </summary>
        [ConfigurationProperty("integratedStatefulCookieName", DefaultValue = "")]
        public string IntegratedStatefulCookieName
        {
            get { return (string)this["integratedStatefulCookieName"]; }
            set { this["integratedStatefulCookieName"] = value; }
        }

        /// <summary>
        /// The name of http header which the user name is acctached in.
        /// </summary>
        [ConfigurationProperty("userNameHeader", IsRequired = true)]
        public string UserNameHeader
        {
            get
            {
                return this["userNameHeader"] as string;
            }
            set
            {
                this["userNameHeader"] = value;
            }
        }

        [ConfigurationProperty("endpointName", IsRequired = true)]
        public string EndpointName
        {
            get
            {
                return this["endpointName"] as string;
            }
            set
            {
                this["endpointName"] = value;
            }
        }
    }
}

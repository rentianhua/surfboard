using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace HiiP.Framework.Security.AccessControl.Interface.Configuration
{
    /// <summary>
    /// Configuration section of form exception keys collecton.
    /// </summary>
    public class ExceptionKeysSection : ConfigurationSection
    {
        [ConfigurationProperty("sessionExpired", IsRequired = true)]
        public SessionExpiredExceptionKeyData SessionExpiredExceptionKey
        {
            get
            {
                return this["sessionExpired"] as SessionExpiredExceptionKeyData;
            }
            set
            {
                this["sessionExpired"] = value;
            }
        }

        [ConfigurationProperty("passwordExpired", IsRequired = true)]
        public PasswordExpiredExceptionKeyData PasswordExpiredExceptionKey
        {
            get
            {
                return this["passwordExpired"] as PasswordExpiredExceptionKeyData;
            }
            set
            {
                this["passwordExpired"] = value;
            }
        }

        [ConfigurationProperty("ADAccountExpired", IsRequired = true)]
        public ADAccountExpiredExceptionKeyData ADAccountExpiredExceptionKey
        {
            get
            {
                return this["ADAccountExpired"] as ADAccountExpiredExceptionKeyData;
            }
            set
            {
                this["ADAccountExpired"] = value;
            }
        }

    }

    /// <summary>
    /// Configuration section of session expired. Its value is retrieved from webSeal header.
    /// </summary>
    public class SessionExpiredExceptionKeyData :ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return this["key"] as string;
            }
            set
            {
                this["key"] = value;
            }
        }
        [ConfigurationProperty("message", IsRequired = false)]
        public string Message
        {
            get
            {
                return this["message"] as string;
            }
            set
            {
                this["message"] = value;
            }
        }
    }

    /// <summary>
    /// Configuration section of password expired. Its value is retrieved from webSeal header.
    /// </summary>
    public class PasswordExpiredExceptionKeyData : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return this["key"] as string;
            }
            set
            {
                this["key"] = value;
            }
        }
    }

    /// <summary>
    /// Configuration section of AD account expired. Its value is retrieved from webSeal header.
    /// </summary>
    public class ADAccountExpiredExceptionKeyData : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return this["key"] as string;
            }
            set
            {
                this["key"] = value;
            }
        }
    }

}

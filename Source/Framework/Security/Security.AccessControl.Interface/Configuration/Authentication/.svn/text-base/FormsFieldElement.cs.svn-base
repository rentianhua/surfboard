using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace HiiP.Framework.Security.AccessControl.Interface.Configuration
{
    /// <summary>
    /// Http form field configuration element used to attached to Http request to retrieve security token.
    /// </summary>
    public class FormsFieldElement:ConfigurationElement
    {
        /// <summary>
        /// Get or set the name of the field.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        /// get or set the value of the field.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string FieldType
        {
            get
            {
                return this["type"] as string;
            }
            set
            {
                this["type"] = value;
            }
        }

        /// <summary>
        /// Get or set the value of the field.
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return this["value"] as string;
            }
            set
            {
                this["value"] = value;
            }
        }       
    }
}
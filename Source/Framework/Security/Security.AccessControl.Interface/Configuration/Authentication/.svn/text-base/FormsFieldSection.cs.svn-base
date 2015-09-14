using System.Configuration;

namespace HiiP.Framework.Security.AccessControl.Interface.Configuration
{
    /// <summary>
    /// Configuration section of form field element collecton.
    /// </summary>
    public class FormsFieldSection: ConfigurationSection
    {
        /// <summary>
        /// Get or set the form field list.
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public FormsFieldCollection FormsFields
        {
            get
            {
                return this[""] as FormsFieldCollection;
            }
        }
    }
}

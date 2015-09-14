using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using HiiP.Framework.Security.AccessControl.Interface.Properties;

namespace HiiP.Framework.Security.AccessControl.Interface.Configuration
{
    /// <summary>
    /// A utility class used to get session related configuraiton information.
    /// </summary>
    public static class SessionSettings
    {
        private static SessionServiceSection _sessionServiceSettings;
        /// <summary>
        /// Get session configuration of clien side.
        /// </summary>
        /// <returns>SessionClientSection instance.</returns>
        public static SessionClientSection GetClientSetting()
        {
            SessionClientSection section = ConfigurationManager.GetSection(string.Format("{0}/{1}", Resources.ConfiguraionSectionGroupName, Resources.ClientConfigurationSectionName)) as SessionClientSection;
            return section;
        }

        /// <summary>
        /// Get session configuration of service side.
        /// </summary>
        /// <returns>SessionServiceSection instance.</returns>
        public static SessionServiceSection GetServiceSetting()
        {

            if (_sessionServiceSettings==null)
            {
                _sessionServiceSettings = ConfigurationManager.GetSection(string.Format("{0}/{1}", Resources.ConfiguraionSectionGroupName, Resources.ServiceConfigurationSectionName)) as SessionServiceSection;
            }

            return _sessionServiceSettings;
        }
    }
}

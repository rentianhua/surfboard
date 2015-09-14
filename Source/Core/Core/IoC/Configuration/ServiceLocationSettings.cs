#region

using System;
using System.Configuration;
using System.Linq;
using Cedar.Core.Configuration;
using Cedar.Core.Properties;

#endregion

namespace Cedar.Core.IoC.Configuration
{
    /// <summary>
    ///     The configuration setting for "iConnect.serviceLocation".
    /// </summary>
    [ConfigurationSectionName("sr.serviceLocation")]
    public class ServiceLocationSettings : ConfigurationSection
    {
        private const string DefaultServiceLocatorProperty = "defaultServiceLocator";
        private const string ServiceLocatorsProperty = "serviceLocators";
        private const string ResolvedAssembliesProperty = "resolvedAssemblies";

        /// <summary>
        ///     Gets the default service locator.
        /// </summary>
        /// <value>
        ///     The default service locator.
        /// </value>
        [ConfigurationProperty("defaultServiceLocator", IsRequired = false, DefaultValue = "")]
        public string DefaultServiceLocator
        {
            get { return (string)base["defaultServiceLocator"]; }
            set { base["defaultServiceLocator"] = value; }
        }

        /// <summary>
        ///     Gets the service locators.
        /// </summary>
        /// <value>
        ///     The service locators.
        /// </value>
        [ConfigurationProperty("serviceLocators", IsRequired = true)]
        public NameTypeConfigurationElementCollection<ServiceLocatorDataBase> ServiceLocators
        {
            get { return (NameTypeConfigurationElementCollection<ServiceLocatorDataBase>)base["serviceLocators"]; }
            set { base["serviceLocators"] = value; }
        }

        /// <summary>
        ///     Gets or sets the resolved assemblies.
        /// </summary>
        /// <value>
        ///     The resolved assemblies.
        /// </value>
        [ConfigurationProperty("resolvedAssemblies", IsRequired = false)]
        public ConfigurationElementCollection<AssemblyConfigurationElement> ResolvedAssemblies
        {
            get { return (ConfigurationElementCollection<AssemblyConfigurationElement>)base["resolvedAssemblies"]; }
            set { base["resolvedAssemblies"] = value; }
        }

        /// <summary>
        ///     Gets the service locator.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The service locator.</returns>
        public IServiceLocator GetServiceLocator(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(DefaultServiceLocator))
                {
                    return ServiceLocators.GetConfigurationElement(DefaultServiceLocator).CreateServiceLocator();
                }
                return null;
            }

            if (
                ServiceLocators.Cast<NameTypeConfigurationElement>()
                    .Any((NameTypeConfigurationElement element) => element.Name == name))
            {
                return ServiceLocators.GetConfigurationElement(name).CreateServiceLocator();
            }
            throw new ConfigurationErrorsException(Resources.ExceptionServiceLocatorNotExists.Format(new object[]
            {
                name
            }));
        }
    }
}
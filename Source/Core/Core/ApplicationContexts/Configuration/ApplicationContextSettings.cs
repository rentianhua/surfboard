using System;
using System.Configuration;
using Cedar.Core.Configuration;
using Cedar.Core.IoC;

namespace Cedar.Core.ApplicationContexts.Configuration
{
    /// <summary>
    /// Define the application context setting.
    /// </summary>
    [ConfigurationSectionName("sr.applicationContexts")]
    public class ApplicationContextSettings : ServiceLocatableSettings
    {
        private const string ContextLocatorsProperty = "contextLocators";
        private const string DefaultContextLocatorNameProperty = "defaultContextLocator";
        private const string ContextAttachBehaviorProperty = "contextAttachBehavior";

        /// <summary>
        /// Gets or sets the context attach behavior.
        /// </summary>
        /// <value>The context attach behavior.</value>
        [ConfigurationProperty("contextAttachBehavior", IsRequired = false, DefaultValue = ContextAttachBehavior.Clear)]
        public ContextAttachBehavior ContextAttachBehavior
        {
            get
            {
                return (ContextAttachBehavior)base["contextAttachBehavior"];
            }
            set
            {
                base["contextAttachBehavior"] = value;
            }
        }

        /// <summary>
        /// Gets the collection of defined ContextLocator objects.
        /// </summary>
        /// <value>
        /// The collection of defined ContextLocator objects.
        /// </value>
        [ConfigurationProperty("contextLocators", IsRequired = true)]
        public NameTypeConfigurationElementCollection<ContextLocatorDataBase> ContextLocators
        {
            get
            {
                return (NameTypeConfigurationElementCollection<ContextLocatorDataBase>)base["contextLocators"];
            }
            set
            {
                base["contextLocators"] = value;
            }
        }
        /// <summary>
        /// Gets or sets the default service locator.
        /// </summary>
        /// <value>The default service locator.</value>
        [ConfigurationProperty("defaultContextLocator", IsRequired = true)]
        public string DefaultContextLocator
        {
            get
            {
                return base["defaultContextLocator"] as string;
            }
            set
            {
                base["defaultContextLocator"] = value;
            }
        }
        /// <summary>
        /// Configures the specified service locator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public override void Configure(IServiceLocator serviceLocator)
        {
            foreach (ContextLocatorDataBase contextLocatorDataBase in this.ContextLocators)
            {
                Func<IContextLocator> providerCreator = contextLocatorDataBase.GetProviderCreator(this);
                if (providerCreator != null)
                {
                    serviceLocator.Register<IContextLocator>(providerCreator, contextLocatorDataBase.Name, contextLocatorDataBase.Name == this.DefaultContextLocator, contextLocatorDataBase.Lifetime);
                }
            }
        }
    }
}

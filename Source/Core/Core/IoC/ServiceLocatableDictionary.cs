using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cedar.Core.IoC
{
    /// <summary>
	/// A dictionary of object can be activated by service locator.
	/// </summary>
	/// <typeparam name="T">The type of element's value.</typeparam>
	public class ServiceLocatableDictionary<T>
    {
        /// <summary>
        /// Gets the service locator.
        /// </summary>
        /// <value>
        /// The service locator.
        /// </value>
        public IServiceLocator ServiceLocator
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public IEnumerable<string> Keys
        {
            get
            {
                return this.ServiceLocator.GetAllKeys<T>();
            }
        }
        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IEnumerable<T> Values
        {
            get
            {
                return this.ServiceLocator.GetAllServices<T>();
            }
        }
        /// <summary>
        /// Gets the object with the specified key.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public T this[string key]
        {
            get
            {
                return this.ServiceLocator.GetService<T>(key);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cedar.Core.IoC.ServiceLocatableDictionary`1" /> class.
        /// </summary>
        /// <param name="serviceLocatorName">Name of the service locator.</param>
        public ServiceLocatableDictionary(string serviceLocatorName = null)
        {
            this.ServiceLocator = ServiceLocatorFactory.GetServiceLocator(serviceLocatorName);
        }
    }
}

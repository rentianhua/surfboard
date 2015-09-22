#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity.Utility;
using Cedar.Core.IoC;

#endregion

namespace Cedar.Framework.Common.Client.WebAPI
{
    /// <summary>
	/// A custom DependencyResolver which uses ServiceLocator to acticvate service.
	/// </summary>
	public class ServiceLocatableDependencyResolver : IDependencyResolver, IDependencyScope, IDisposable
    {
        private List<IDisposable> disposableServices = new List<IDisposable>();
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
        /// Initializes a new instance of the ServiceLocatableDependencyResolver class.
        /// </summary>
        /// <param name="serviceLocatorName">Name of the service locator.</param>
        public ServiceLocatableDependencyResolver(string serviceLocatorName = null)
        {
            this.ServiceLocator = ServiceLocatorFactory.GetServiceLocator(serviceLocatorName);
        }

        /// <summary>
        /// Initializes a new instance of the ServiceLocatableDependencyResolver class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public ServiceLocatableDependencyResolver(IServiceLocator serviceLocator)
        {
            Guard.ArgumentNotNull(serviceLocator, "serviceLocator");
            this.ServiceLocator = serviceLocator;
        }

        /// <summary>
        /// Begins the scope.
        /// </summary>
        /// <returns>The created DependencyScope.</returns>
        public IDependencyScope BeginScope()
        {
            return new ServiceLocatableDependencyResolver(this.ServiceLocator);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The activated service instance.</returns>
        public object GetService(Type serviceType)
        {
            Guard.ArgumentNotNull(serviceType, "serviceType");
            object result;
            try
            {
                object service = this.ServiceLocator.GetService(serviceType, null);
                this.AddDisposableService(service);
                result = service;
            }
            catch (ResolutionException)
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The activated service instances.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            Guard.ArgumentNotNull(serviceType, "serviceType");
            IEnumerable<object> result;
            try
            {
                List<object> list = new List<object>();
                foreach (object current in this.ServiceLocator.GetAllServices(serviceType))
                {
                    this.AddDisposableService(current);
                    list.Add(current);
                }
                result = list;
            }
            catch (ResolutionException)
            {
                result = Enumerable.Empty<object>();
            }
            return result;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (IDisposable current in this.disposableServices)
            {
                current.Dispose();
            }
        }
        private void AddDisposableService(object servie)
        {
            IDisposable disposable = servie as IDisposable;
            if (disposable != null && !this.disposableServices.Contains(disposable))
            {
                this.disposableServices.Add(disposable);
            }
        }
    }
}
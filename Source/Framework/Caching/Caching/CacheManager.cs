using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.IoC;
using Cedar.Framwork.Caching;
using Cedar.Framwork.Caching.Redis;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Cedar.AuditTrail.Interception
{
    public class CacheManager : IDisposable
    {
        private static readonly object syncHelper = new object();
        private static CachingProviderBase provider;
        private bool disposed;

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>
        /// The providers.
        /// </value>
        public static ServiceLocatableDictionary<CachingProviderBase> Providers
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public static CachingProviderBase Provider
        {
            get
            {
                if (CacheManager.provider != null)
                {
                    return CacheManager.provider;
                }
                CachingProviderBase result;
                lock (CacheManager.syncHelper)
                {
                    if (CacheManager.provider != null)
                    {
                        result = CacheManager.provider;
                    }
                    else
                    {
                        result = (CacheManager.provider = ServiceLocatorFactory.GetServiceLocator(null).GetService<CachingProviderBase>(null));
                    }
                }
                return result;
            }
        }


        public object Get(string key)
        {
            return Provider.Get(key);
        }

        private CacheManager()
        {
            CacheManager.Providers = new ServiceLocatableDictionary<CachingProviderBase>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static CacheManager CreateCacheManager(string functionName)
        {
            Guard.ArgumentNotNullOrEmpty(functionName, "functionName");
            return new CacheManager();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.EnsureNotDisposed();
            this.disposed = true;
        }

        private void EnsureNotDisposed()
        {
            if (this.disposed)
            {
                throw new InvalidOperationException("ExceptionLoggerIsDisposed");
            }
        }
    }
}

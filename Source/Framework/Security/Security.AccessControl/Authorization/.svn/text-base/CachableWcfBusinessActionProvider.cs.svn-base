using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;

using HiiP.Infrastructure.Interface;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

using NCS.IConnect.Security;

namespace HiiP.Framework.Security.AccessControl.Authorization
{
    /// <summary>
    /// Manages storage of business action information for the applicationName in the data source.
    /// </summary>
    [ConfigurationElementType(typeof(CustomAuthorizationProviderData))]
    public class CacheableWcfBusinessActionProvider : WcfBusinessActionProvider
    {
        private const string ActionsCacheKeyPrefix = "__Actions";

        private CacheManager _cacheManager;
    
        /// <summary>
        /// Constructor will be called by AuthorizationFactory
        /// </summary>
        /// <param name="config">The configuration data from configuration file. It should contains applicationName and remoteAddress.</param>
        public CacheableWcfBusinessActionProvider(NameValueCollection config)
            : this(config["applicationName"], config["remoteAddress"], config["bindingName"], config["behaviorName"], config["cacheManager"])
        {}

        /// <summary>
        /// Construct and initialize a CachableWcfBusinessActionProvider.
        /// </summary>
        /// <param name="applicationName">Application name</param>
        /// <param name="remoteAddress">Remote address of WCF business action service.</param>
        /// <param name="bindingName">The binding name of endpoint communicating with business action service.</param>
        /// <param name="behaviorName">The behavior name of endpoint communicating with business action service.</param>
        /// <param name="cacheManager">The name of cache manager.</param>
        public CacheableWcfBusinessActionProvider(string applicationName, string remoteAddress, string bindingName, string behaviorName, string cacheManager)
            : base(applicationName, remoteAddress, bindingName, behaviorName)
        {
            if (string.IsNullOrEmpty(cacheManager))
            {
                this._cacheManager = CacheFactory.GetCacheManager();
            }
            else
            {
                this._cacheManager = CacheFactory.GetCacheManager(cacheManager);
            }
        }

        /// <summary>
        /// Get all action codes for a given user.
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <returns>Action Codes.</returns>
        public override string[] GetActionsForUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }
            if (_cacheManager == null)
            {
                throw new HiiP.Framework.Common.BusinessException("Uable to create cache storage.");
            }

            string cacheKey = ActionsCacheKeyPrefix + userName;
            var data = this._cacheManager.GetData(cacheKey) as string[];

            if (data == null)
            {
                using (var proxy = new AuthorizationProxy())
                {
                    data = proxy.GetActionsForUser(this.ApplicationName, userName);
                    this._cacheManager.Add(cacheKey,data ,CacheItemPriority.None,null,new AbsoluteTime(ParameterUtil.DefaultCacheTimeSpan));
                }
            }

            return data ;
        }
        
        /// <summary>
        /// Gets a value indicating whether the specified user is allowed to execute the business action.
        /// </summary>
        /// <param name="principal">Must be an System.Security.Principal.IPrincipal object.</param>
        /// <param name="actionCode">The action code to be evaluated.</param>
        /// <returns>true if the specified action code is authorized to the specified role; otherwise, false</returns>
        public override bool Authorize(IPrincipal principal, string actionCode)
        {
            return this.Authorize(principal.Identity.Name, actionCode);
        }

        /// <summary>
        ///  Gets a value indicating whether the specified user is allowed to execute the business action.
        /// </summary>
        /// <param name="userName">The user used to validate.</param>
        /// <param name="actionCode">The action code to be evaluated.</param>
        /// <returns>true if the specified action code is authorized to the specified role; otherwise, false</returns>
        public override bool Authorize(string userName, string actionCode)
        {
            return Array.IndexOf<string>(this.GetActionsForUser(userName), actionCode) > -1;
        }
    }
}
﻿using System;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using Cedar.AuditTrail.Interception;
using Cedar.Framwork.Caching.Interception.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Newtonsoft.Json;

namespace Cedar.Framwork.Caching.Interception
{
    [ConfigurationElementType(typeof(CachingCallHandlerData))]
    public class CachingCallHandler : ICallHandler
    {
        /// <summary>
		/// The default expiration time for the cached entries: 5 minutes
		/// </summary>
		public static readonly TimeSpan DefaultExpirationTime = new TimeSpan(0, 0, 0);

        /// <summary>
		/// Gets or sets the key generator.
		/// </summary>
		/// <value>The key generator.</value>
		public ICacheKeyGenerator KeyGenerator
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets or sets the expiration time.
        /// </summary>
        /// <value>The expiration time.</value>
        public TimeSpan ExpirationTime
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a <see cref="T:Cedar.Framwork.Caching.Interception.CachingCallHandler" /> that uses the default expiration time of 5 minutes.
        /// </summary>
        public CachingCallHandler() : this(CachingCallHandler.DefaultExpirationTime)
        {
        }

        /// <summary>
        /// Creates a <see cref="T:Cedar.Framwork.Caching.Interception.CachingCallHandler" /> that uses the given expiration time.
        /// </summary>
        /// <param name="expirationTime">Length of time the cached data goes unused before it is eligible for
        /// reclamation.</param>
        public CachingCallHandler(TimeSpan expirationTime)
        {
            this.KeyGenerator = new DefaultCacheKeyGenerator();
            this.ExpirationTime = expirationTime;
        }

        /// <summary>
        /// Creates a <see cref="T:Cedar.Framwork.Caching.Interception.CachingCallHandler" /> that uses the given expiration time.
        /// </summary>
        /// <param name="expirationTime">Length of time the cached data goes unused before it is eligible for
        ///             reclamation.</param>
        /// <param name="order">Order in which handler will be executed.</param>
        public CachingCallHandler(TimeSpan expirationTime, int order)
        {
            this.KeyGenerator = new DefaultCacheKeyGenerator();
            this.ExpirationTime = expirationTime;
            this.Order = order;
        }

        /// <summary>
        /// Implements the caching behavior of this handler.
        /// </summary>
        /// <param name="input"><see cref="T:Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation" /> object describing the current call.</param>
        /// <param name="getNext">delegate used to get the next handler in the current pipeline.</param>
        /// <returns>Return value from target method, or cached result if previous inputs have been seen.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (this.TargetMethodReturnsVoid(input))
            {
                return getNext()(input, getNext);
            }

            //object[] array = new object[input.Inputs.Count];
            //for (int i = 0; i < array.Length; i++)
            //{
            //    array[i] = input.Inputs[i];
            //}
            //string key = this.KeyGenerator.CreateCacheKey(input.MethodBase, array);
            //object[] array2 = (object[])HttpRuntime.Cache.Get(key);
            //if (array2 == null)
            //{
            //    IMethodReturn methodReturn = getNext()(input, getNext);
            //    if (methodReturn.Exception == null)
            //    {
            //        this.AddToCache(key, methodReturn.ReturnValue);
            //    }
            //    return methodReturn;
            //}
            //return input.CreateMethodReturn(array2[0], new object[]
            //{
            //    input.Arguments
            //});
            object[] array = new object[input.Inputs.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = input.Inputs[i];
            }
            string key = this.KeyGenerator.CreateCacheKey(input.MethodBase, array);

            //object[] array2 = (object[])CacheManager.Provider.Get(key);
            object[] array2 = (object[])HttpRuntime.Cache.Get(key);
            if (array2 == null)
            {
                IMethodReturn methodReturn = getNext()(input, getNext);

                if (methodReturn.Exception == null)
                {
                    this.AddToCache(key, methodReturn.ReturnValue, ExpirationTime);
                }
                return methodReturn;
            }

            return input.CreateMethodReturn(array2[0], new object[]
            {
                input.Arguments
            });
        }

        private bool TargetMethodReturnsVoid(IMethodInvocation input)
        {
            MethodInfo methodInfo = input.MethodBase as MethodInfo;
            return methodInfo != null && methodInfo.ReturnType == typeof(void);
        }

        private void AddToCache(string key, object value, TimeSpan expirationTime)
        {
            object[] value2 = new object[]
            {
                value
            };
            //CacheManager.Provider.Add(key, value2, expirationTime);
            HttpRuntime.Cache.Insert(key, value2, null, Cache.NoAbsoluteExpiration, this.ExpirationTime, CacheItemPriority.Normal, null);
        }
    }
}

﻿using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Newtonsoft.Json.Serialization;

namespace Cedar.Framwork.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CachingProviderBase
    {
        public CachingProviderBase(bool isenable)
        {
            Enabled = isenable;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Enabled { get; set; }

        public object Get(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            if (this.Enabled)
            {
                return this.GetCore(key);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        protected abstract void AddCore(string key, object value, TimeSpan expirationTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract object GetCore(string key);

        /// <summary>
        /// Remove all cache items added by the cache store itself.
        /// </summary>
        protected abstract void ClearCore();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        public void Add(string key, object value, TimeSpan expirationTime)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            if (this.Enabled)
            {
                this.AddCore(key, value, expirationTime);
            }
        }

        /// <summary>
        /// Remove all cache items added by the cache store itself.
        /// </summary>
        /// <remarks>Nothing will be done if not enabled.</remarks>
        public void Clear()
        {
            if (this.Enabled)
            {
                this.ClearCore();
            }
        }
    }
}
using System;
using Cedar.Core.Configuration;
using Cedar.Core.EntLib.Data;

namespace Cedar.Framwork.Caching.Redis
{
    /// <summary>
    /// 
    /// </summary>
    [ConfigurationElement(typeof(RedisCachingData))]
    public class RedisCachingProvider : CachingProviderBase
    {
        public RedisDatabaseWrapper RedisDatabaseWrapper
        {
            get;
            private set;
        }

        private TimeSpan ExpireSpan { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isenable"></param>
        /// <param name="expireSpan"></param>
        /// <param name="redisDatabaseWrapper"></param>
        public RedisCachingProvider(bool isenable, TimeSpan expireSpan, RedisDatabaseWrapper redisDatabaseWrapper) : base(isenable)
        {
            this.RedisDatabaseWrapper = redisDatabaseWrapper;
            ExpireSpan = expireSpan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected override void AddCore(string key, object value, TimeSpan expirationTime)
        {
            if (RedisDatabaseWrapper.KeyExists(key)) return;

            RedisDatabaseWrapper.StringSet(key, value);
            RedisDatabaseWrapper.KeyExpire(key,
                expirationTime == new TimeSpan(0, 0, 0) ? ExpireSpan : expirationTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override object GetCore(string key)
        {
            var keyExists = RedisDatabaseWrapper.KeyExists(key);

            return keyExists ? RedisDatabaseWrapper.StringGet(key) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ClearCore()
        {
            throw new NotImplementedException();
        }
    }
}

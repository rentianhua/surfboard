using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cedar.Core.Data;
using Cedar.Core.IoC;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Cedar.Core.EntLib.Data
{
    //[MapTo(typeof(RedisDatabaseWrapper), 0, Lifetime = Lifetime.Singleton)]
    public class RedisDatabaseWrapper
    {
        private static IConnectionMultiplexer _connectionMultiplexer;

        public RedisDatabaseWrapper()
        {
            var options = new ConfigurationOptions()
            {
                EndPoints =
                {
                    new DnsEndPoint("172.16.0.91",6379)
                },
                KeepAlive = 180,
                //Password = password,
                DefaultVersion = new Version("2.8.5"),
                // Needed for cache clear
                AllowAdmin = true
            };

            _connectionMultiplexer = ConnectionMultiplexer.Connect(options);
        }

        public RedisDatabaseWrapper(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public RedisDatabaseWrapper(dynamic configurationOptions)
        {
            //todo add configurationOptions
            _connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions());
        }

        public bool StringSet(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase(1);
            return db.StringSet(key, value);
        }
    }
}

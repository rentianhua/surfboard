using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using StackExchange.Redis;
using Redis.Cache;

namespace Cedar.Core.EntLib.Data
{
    //[MapTo(typeof(RedisDatabaseWrapper), 0, Lifetime = Lifetime.Singleton)]
    public class RedisDatabaseWrapper : IDisposable
    {
        private static IConnectionMultiplexer _connectionMultiplexer;
        private int database = 0;

        public RedisDatabaseWrapper(string ip, int database, string password = null, int port = 6379)
        {
            var options = new ConfigurationOptions()
            {
                EndPoints =
                {
                    new DnsEndPoint(ip,port)
                },
                KeepAlive = 180,
                Password = password,
                //DefaultVersion = new Version("2.8.5"),
                // Needed for cache clear
                AllowAdmin = true,
            };
            this.database = database;
            this.database = database;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(options);
        }

        public bool StringSet(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase(database);
            return db.StringSet(key, value);
        }

        public bool KeyExpire(string key, TimeSpan value)
        {
            var db = _connectionMultiplexer.GetDatabase(database);
            return db.KeyExpire(key, value);
        }

        public bool KeyExists(string key)
        {
            var db = _connectionMultiplexer.GetDatabase(database);
            return db.KeyExists(key);
        }

        public string StringGet(string key)
        {
            var db = _connectionMultiplexer.GetDatabase(database);
            return db.StringGet(key);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

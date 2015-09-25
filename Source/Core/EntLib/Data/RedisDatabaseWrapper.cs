using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using StackExchange.Redis;

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

        public bool StringSet(string key, object value)
        {
            var db = _connectionMultiplexer.GetDatabase(database);
            return db.StringSet(key, Serialize(value));
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

        public T StringGet<T>(string key)
        {
            var db = _connectionMultiplexer.GetDatabase(database);
            return Deserialize<T>(db.StringGet(key));
        }

        public object StringGet(string key)
        {
            var db = _connectionMultiplexer.GetDatabase(database);
            return db.StringGet(key);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

        static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
    }
}

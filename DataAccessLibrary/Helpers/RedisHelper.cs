using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Helpers
{
    public class RedisHelper
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisHelper(string connectionString)
        {
            ConfigurationOptions config = ConfigurationOptions.Parse(connectionString);
            _redis = ConnectionMultiplexer.Connect(config);
        }

        public bool IsRedisServerAvailable()
        {
            try
            {
                // Try to connect to Redis server
                var db = _redis.GetDatabase();
                var pingResult = db.Ping();
                return pingResult != TimeSpan.Zero;
            }
            catch (RedisConnectionException)
            {
                // Redis server is not available
                return false;
            }
        }
    }
}
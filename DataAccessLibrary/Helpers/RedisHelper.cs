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
            try
            {
                _redis = ConnectionMultiplexer.Connect(config);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis connection error: {ex.Message}");
            }
            
            }

        public bool IsRedisServerAvailable()
        {
            if (_redis is not null)
            {
                try
                {
                    // Try to connect to Redis server
                    var db = _redis.GetDatabase();
                    var pingResult = db.Ping();
                    return pingResult != TimeSpan.Zero;
                }
                catch (RedisConnectionException ex)
                {
                    // Redis server is not available
                    Console.WriteLine($"Redis connection error: {ex.Message}");
                    return false;
                }
            }
            else { return false; }
        }
    }
}
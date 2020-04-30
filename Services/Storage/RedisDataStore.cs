using Microsoft.Extensions.Configuration;
using sm_coding_challenge.Services.DataProvider;
using StackExchange.Redis;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace sm_coding_challenge.Services.DataSource
{
    public class RedisDataStore : IDataStore
    {
        private static TimeSpan Timeout => TimeSpan.FromSeconds(30);
        private readonly string _host;
        private readonly string DataKey = "data";

        private static ConnectionMultiplexer RedisConnection { get; set; }

        private TimeSpan DefaultKeyExpiration { get; }

        public RedisDataStore(IConfiguration config)
        {
            _host = config["redis:host"];
            if (double.TryParse(config["redis:expiryInSeconds"], out double expiryInSeconds))
            {
                DefaultKeyExpiration = TimeSpan.FromSeconds(expiryInSeconds);
            }
            else
            {
                throw new FormatException("expiryInSeconds does not have a valid double value.");
            }
        }

        public void Connect()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { _host }
            };

            RedisConnection = ConnectionMultiplexer.Connect(configurationOptions);
        }

        public async Task<string> GetDataAsync(bool forceInvalidate = false)
        {
            var redisCache = RedisConnection.GetDatabase();

            if (forceInvalidate)
            {
                await redisCache.KeyDeleteAsync(DataKey);
            }

            var data = await redisCache.StringGetAsync(DataKey);
            if (!string.IsNullOrWhiteSpace(data))
            {
                return data;
            }

            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var client = new HttpClient(handler))
            {
                client.Timeout = Timeout;
                var response = await client.GetAsync("https://gist.githubusercontent.com/RichardD012/a81e0d1730555bc0d8856d1be980c803/raw/3fe73fafadf7e5b699f056e55396282ff45a124b/basic.json");
                var stringData = await response.Content.ReadAsStringAsync();
                await redisCache.StringSetAsync(DataKey, stringData, DefaultKeyExpiration);

                return stringData;
            }
        }

    }
}

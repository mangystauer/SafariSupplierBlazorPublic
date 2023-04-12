using DataAccessLibrary.Models.Shatem.DataAccess;
using DataAccessLibrary.Models.Shatem.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using SuppliersBlazor.Extensions;
using DataAccessLibrary.Helpers;

namespace DataAccessLibrary.ApiDataAccess
{
    public class ShatemAccess : IShatemAccess
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ShatemConfig _shatemConfig;
        private readonly IDistributedCache _cache;
        private readonly RedisHelper _redisHelper;

        public ShatemAccess(IOptions<ShatemConfig> shatemConfig, IConfiguration configuration, IHttpClientFactory httpClientFactory, IDistributedCache cache, RedisHelper redisHelper)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _shatemConfig = shatemConfig.Value;
            _cache = cache;
            _redisHelper = redisHelper;
        }

        public async Task<ShatemAccessModel> GetAccessTokenAsync()
        {
            string recordKey = "shatemToken_" + DateTime.Now.ToString("yyyyMMdd");
            string loadLocation = "";
            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;

            if (_redisHelper.IsRedisServerAvailable())
            {
                var shatemAccessModel = await _cache.GetRecordAsync<ShatemAccessModel>(recordKey);

                if (shatemAccessModel is null)
                {
                    shatemAccessModel = await FetchAccessTokenFromServer(url, apiKey, recordKey);
                    if (shatemAccessModel != null)
                    {
                        return shatemAccessModel;
                    }
                }
                else
                {
                    loadLocation = $"Loaded from the cache at {DateTime.Now}";
                    return shatemAccessModel;
                }
            }
            else
            {
                var shatemAccessModel = await FetchAccessTokenFromServer(url, apiKey, recordKey);
                return shatemAccessModel;
            }

            return null;
        }

        public async Task<List<ShatemAgreement>> GetAgreementsAsync(string accessToken)
        {
            string url = _shatemConfig.Uri;
            string cacheKey = $"ShatemAgreements_"+ DateTime.Now.ToString("yyyyMM");


            // Try to get the result from cache
            List<ShatemAgreement> shatemAgreements = await _cache.GetRecordAsync<List<ShatemAgreement>>(cacheKey);

            if (shatemAgreements == null)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url + "/customer/agreements");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var _httpClient = _httpClientFactory.CreateClient())
                {
                    var response = await _httpClient.SendAsync(request);

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var serializerOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    shatemAgreements = JsonSerializer.Deserialize<List<ShatemAgreement>>(jsonResponse, serializerOptions);

                    // Store the result in cache for 24 hours
                    await _cache.SetRecordAsync(cacheKey, shatemAgreements, TimeSpan.FromHours(24));
                }
            }

            return shatemAgreements;
        }

        public async Task<ShatemLocationList> GetLocationListAsync(string accessToken)
        {

            string url = _shatemConfig.Uri;

            var request = new HttpRequestMessage(HttpMethod.Get, url + "/locations");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using (var _httpClient = _httpClientFactory.CreateClient())
            {

                var response = await _httpClient.SendAsync(request);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var shatemLocations = JsonSerializer.Deserialize<ShatemLocationList>(jsonResponse, serializerOptions);

                return shatemLocations;
            }
        }


        private async Task<ShatemAccessModel> FetchAccessTokenFromServer(string url, string apiKey, string recordKey)
        {
            var body = new Dictionary<string, string>
    {
        { "ApiKey", $"{apiKey}" }
    };

            var content = new FormUrlEncodedContent(body);

            url = url + "/auth/loginbyapikey";

            using (var _httpClient = _httpClientFactory.CreateClient())
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(30);

                try
                {
                    var response = await _httpClient.PostAsync(url, content);

                    string rmessage = response.StatusCode.ToString();
                    string rreason = response.ReasonPhrase;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        var serializerOptions = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var shatemAccessModel = JsonSerializer.Deserialize<ShatemAccessModel>(jsonResponse, serializerOptions);

                        try
                        {
                            await _cache.SetRecordAsync(recordKey, shatemAccessModel, TimeSpan.FromMinutes(55));
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception, e.g., log the error or take appropriate action
                            await Console.Out.WriteLineAsync($"Error storing data in Redis: {ex.Message}");
                        }

                        return shatemAccessModel;
                    }

                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync($"Error: {ex.Message}");
                }

            }

            return null;
        }
    }
}

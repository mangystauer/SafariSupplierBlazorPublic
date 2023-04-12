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

namespace DataAccessLibrary.ApiDataAccess
{
    public class ShatemAccess : IShatemAccess
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ShatemConfig _shatemConfig;

        public ShatemAccess(IOptions<ShatemConfig> shatemConfig, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _shatemConfig = shatemConfig.Value;
        }

        public async Task<ShatemAccessModel> GetAccessTokenAsync()
        {

            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;

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

                        return shatemAccessModel;
                    }

                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync($"Error: {ex.Message}");
                    return null;
                }



                return null;
            }
        }

        public async Task<List<ShatemAgreement>> GetAgreementsAsync(string accessToken)
        {
            string url = _shatemConfig.Uri;


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

                var shatemAgreements = JsonSerializer.Deserialize<List<ShatemAgreement>>(jsonResponse, serializerOptions);

                return shatemAgreements;
            }
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
    }
}

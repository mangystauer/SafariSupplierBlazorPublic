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

namespace DataAccessLibrary.ApiDataAccess
{
    public class ShatemAccess : IShatemAccess
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public ShatemAccess(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ShatemAccessModel> GetAccessTokenAsync(string apiKey)
        {

            var body = new Dictionary<string, string>
    {
        { "ApiKey", $"{apiKey}" }
    };

            var content = new FormUrlEncodedContent(body);

            using (var _httpClient = _httpClientFactory.CreateClient())
            {

                var response = await _httpClient.PostAsync(_configuration["ShatemUri"] + "/auth/loginbyapikey", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var shatemAccessModel = JsonSerializer.Deserialize<ShatemAccessModel>(jsonResponse, serializerOptions);

                return shatemAccessModel;

            }
        }

        public async Task<List<ShatemAgreement>> GetAgreementsAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _configuration["ShatemUri"] + "/customer/agreements");
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
            var request = new HttpRequestMessage(HttpMethod.Get, _configuration["ShatemUri"] + "/locations");
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

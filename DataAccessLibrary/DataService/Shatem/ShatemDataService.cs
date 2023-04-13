using DataAccessLibrary.ApiDataAccess;
using DataAccessLibrary.Helpers;
using DataAccessLibrary.Models.Shatem.DataAccess;
using DataAccessLibrary.Models.Shatem.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuppliersBlazor.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataService.Shatem
{
    public class ShatemDataService : IShatemDataService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IShatemAccess _shatemAccess;
        private readonly ShatemConfig _shatemConfig;
        private readonly RedisHelper _redisHelper;
        private readonly IDistributedCache _cache;

        public ShatemDataService(IOptions<ShatemConfig> shatemConfig, IConfiguration configuration, IHttpClientFactory httpClientFactory, IShatemAccess shatemAccess, RedisHelper redisHelper, IDistributedCache cache)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _shatemAccess = shatemAccess;
            _shatemConfig = shatemConfig.Value;
            _redisHelper = redisHelper;
            _cache = cache;


            //_apiKey = _configuration["ShatemApiKey"];
        }


        public async Task<List<ShatemFoundArticle>> SearchArticlesAsync(string lineToSearch, string tradeMarkName = null)
        {
            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;

            if (string.IsNullOrEmpty(tradeMarkName))
            {
                url += $"/articles/search?searchString={lineToSearch}";
            }
            else
            {
                url += $"/articles/search?searchString={lineToSearch}&tradeMarkNames={tradeMarkName}";

            }

            ShatemAccessModel shatemAccessModel = await _shatemAccess.GetAccessTokenAsync();
            string token = shatemAccessModel?.access_token;

            string recordKey = $"shatemPart_{lineToSearch.ToLower()}_" + DateTime.Now.ToString("yyyyMM");

            if (_redisHelper.IsRedisServerAvailable())
            {
                var shatemSearchResults = await _cache.GetRecordAsync<List<ShatemFoundArticle>>(recordKey);

                if (shatemSearchResults != null)
                {
                    return shatemSearchResults;
                }
            }

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        List<ShatemFoundArticleWrapper> foundArticleWrappers = JsonSerializer.Deserialize<List<ShatemFoundArticleWrapper>>(jsonResponse, jsonOptions);

                        if (foundArticleWrappers?.Count > 0)
                        {
                            List<ShatemFoundArticle> foundArticles = foundArticleWrappers.Select(a => a.Article).ToList();

                            if (_redisHelper.IsRedisServerAvailable())
                            {
                                await _cache.SetRecordAsync(recordKey, foundArticles, TimeSpan.FromHours(24));
                            }

                            return foundArticles;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Произошла ошибка при запросе. {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex) when (ex is HttpRequestException || ex is JsonException)
                {
                    Console.WriteLine($"Произошла ошибка. {ex.Message}");
                }
            }

            return null;
        }


        public async Task<ShatemFoundArticle> ArticleInfoAsync(string articleId, bool includeAnalogs = false)
        {
            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;

            url = url + $"/articles/{articleId}";

            if (includeAnalogs)
            {
                url += "?analogs=true";
            }

            ShatemAccessModel shatemAccessModel = await _shatemAccess.GetAccessTokenAsync();
            string token = shatemAccessModel.access_token; // Retrieve the access_token property from the ShatemAccessModel object


            ShatemFoundArticle foundArticle = new ShatemFoundArticle();

            string cacheKey = $"ShatemArticleInfo_{DateTime.Now.ToString("yyyyMM")}_{articleId}_{includeAnalogs}";

            if (_redisHelper.IsRedisServerAvailable())
            {
                // Try to get the result from cache


                foundArticle = await _cache.GetRecordAsync<ShatemFoundArticle>(cacheKey);

                if (foundArticle != null)
                {
                    // Return cached result if available
                    return foundArticle;
                }
            }

            using (var _httpClient = _httpClientFactory.CreateClient())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(jsonResponse))
                        {
                            JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            };

                            ShatemFoundArticleWrapper foundArticleWrapper = JsonSerializer.Deserialize<ShatemFoundArticleWrapper>(jsonResponse, jsonOptions);

                            if (foundArticleWrapper != null && foundArticleWrapper.Article != null)
                            {
                                foundArticle = foundArticleWrapper.Article;
                                if (_redisHelper.IsRedisServerAvailable())
                                {
                                    // Store the result in cache for future use
                                    await _cache.SetRecordAsync(cacheKey, foundArticle, TimeSpan.FromHours(24));
                                }
                                return foundArticle;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Произошла ошибка при запросе. Код ошибки: {response.StatusCode}");
                    }
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    Console.WriteLine($"Произошла ошибка при выполнении HTTP-запроса. {ex.Message}");
                }
                catch (System.Text.Json.JsonException ex)
                {
                    Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
                }
            }

            return null;
        }



        public async Task<ShatemFullArticle> FullArticleInfoAsync(int articleId)
        {
            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;
            string cacheKey = $"ShatemFullArticle_{articleId}";

            // Check if Redis server is available
            if (_redisHelper.IsRedisServerAvailable())
            {
                // Try to get the result from cache
                ShatemFullArticle shatemFullArticle = await _cache.GetRecordAsync<ShatemFullArticle>(cacheKey);

                if (shatemFullArticle != null)
                {
                    return shatemFullArticle;
                }
            }

            url = $"{url}/articles/{articleId}?include=trademark,contents,extended_info"; // Update URL to include query parameters

            ShatemAccessModel shatemAccessModel = await _shatemAccess.GetAccessTokenAsync();
            string token = shatemAccessModel.access_token; // Retrieve the access_token property from the ShatemAccessModel object

            using (var _httpClient = _httpClientFactory.CreateClient())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");

                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        ShatemFullArticle shatemFullArticle = JsonSerializer.Deserialize<ShatemFullArticle>(jsonResponse, jsonOptions);

                        if (shatemFullArticle != null)
                        {
                            // Store the result in cache if Redis server is available
                            if (_redisHelper.IsRedisServerAvailable())
                            {
                                await _cache.SetRecordAsync(cacheKey, shatemFullArticle, TimeSpan.FromDays(2));
                            }
                            return shatemFullArticle;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Произошла ошибка при запросе. {ex.Message}");
                    return null;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
                    return null;
                }
            }

            return null;
        }


        public async Task<List<ShatemArticlePrice>> SearchAvailableQuantityAsync(string articleId, bool includeAnalogs = false)
        {
            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;

            ShatemAccessModel shatemAccessModel = await _shatemAccess.GetAccessTokenAsync();
            string token = shatemAccessModel.access_token; // Retrieve the access_token property from the ShatemAccessModel object



            List<ShatemAgreement> shatemAgreements = await _shatemAccess.GetAgreementsAsync(token);
            string agreementCode = shatemAgreements.FirstOrDefault()?.code; // Use null conditional operator to prevent NullReferenceException

            url = $"{url}/prices/search?agreementCode={agreementCode}";

            string cacheKey = $"ShatemPrices_{articleId}_{includeAnalogs}"; // Generate a unique cache key based on articleId and includeAnalogs


            // Check if Redis server is available
            if (_redisHelper.IsRedisServerAvailable())
            {
                // Try to get the result from cache
                List<ShatemArticlePrice> cachedResults = await _cache.GetRecordAsync<List<ShatemArticlePrice>>(cacheKey);

                if (cachedResults != null)
                {
                    return cachedResults;
                }
            }

            using (var _httpClient = _httpClientFactory.CreateClient())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json"); // Set the Accept header

                try
                {
                    // Create the JSON body
                    string jsonBody = $"[{{ \"articleId\": {articleId}, \"includeAnalogs\": {includeAnalogs.ToString().ToLower()} }}]";

                    // Convert the JSON body to HttpContent
                    HttpContent httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _httpClient.PostAsync(url, httpContent);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Configure JsonSerializerOptions for case-insensitive property name handling
                    JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<ShatemArticlePrice> foundQty = JsonSerializer.Deserialize<List<ShatemArticlePrice>>(jsonResponse, jsonOptions);

                    // Store the result in cache if Redis server is available
                    if (_redisHelper.IsRedisServerAvailable())
                    {
                        await _cache.SetRecordAsync(cacheKey, foundQty, TimeSpan.FromMinutes(60)); // Cache the results for 60 minutes
                    }

                    return foundQty;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Произошла ошибка при запросе: {ex.Message}");
                    return null;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Произошла ошибка при десериализации JSON: {ex.Message}");
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    return null;
                }
            }
        }


        public async Task<List<ContentImage>> SearchContentsAsync(string contentId, int heightSize = 400, int widthSize = 400)
        {
            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;

            url = url + "/contents/search";


            string cacheKey = $"ShatemImages_{contentId}"; // Generate a unique cache key based on articleId and includeAnalogs

            // Check if Redis server is available
            if (_redisHelper.IsRedisServerAvailable())
            {
                // Try to get the result from cache
                List<ContentImage> cachedResults = await _cache.GetRecordAsync<List<ContentImage>>(cacheKey);

                if (cachedResults != null)
                {
                    return cachedResults;
                }
            }


            ShatemAccessModel shatemAccessModel = await _shatemAccess.GetAccessTokenAsync();
            string token = shatemAccessModel.access_token; // Retrieve the access_token property from the ShatemAccessModel object



            using (var _httpClient = _httpClientFactory.CreateClient())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                ContentKey contentKey = new ContentKey
                {
                    ContentId = contentId,
                    HeightSize = heightSize,
                    WidthSize = widthSize
                };

                SearchContentsRequest request = new SearchContentsRequest
                {
                    ContentKeys = new List<ContentKey> { contentKey }
                };

                string jsonRequest = JsonSerializer.Serialize(request);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        List<SearchResult> searchResults = JsonSerializer.Deserialize<List<SearchResult>>(jsonResponse);

                        if (searchResults != null && searchResults.Count > 0)
                        {
                            List<ContentImage> contentImages = new List<ContentImage>();

                            foreach (var result in searchResults)
                            {
                                // Decode base64 value and extract image data
                                if (!string.IsNullOrEmpty(result.Value) && result.Value.StartsWith("data:image/jpeg;base64,"))
                                {
                                    string base64Value = result.Value.Substring("data:image/jpeg;base64,".Length);
                                    byte[] imageBytes = Convert.FromBase64String(base64Value);
                                    ContentImage contentImage = new ContentImage
                                    {
                                        Id = result.Id,
                                        Value = imageBytes
                                    };
                                    contentImages.Add(contentImage);
                                }



                            }
                            // Store the result in cache if Redis server is available
                            if (_redisHelper.IsRedisServerAvailable())
                            {
                                await _cache.SetRecordAsync(cacheKey, contentImages, TimeSpan.FromDays(1));
                            }

                            return contentImages;
                        }
                    }

                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"An error occurred while making the request. {ex.Message}");
                    // Handle network-related error
                    return null;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"An error occurred while deserializing JSON. {ex.Message}");
                    // Handle JSON-related error
                    return null;
                }
            }
        }
    }

}
    


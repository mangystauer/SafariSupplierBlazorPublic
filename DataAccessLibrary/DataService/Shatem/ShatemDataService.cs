using DataAccessLibrary.ApiDataAccess;
using DataAccessLibrary.Models.Shatem.DataAccess;
using DataAccessLibrary.Models.Shatem.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        public ShatemDataService(IOptions<ShatemConfig> shatemConfig, IConfiguration configuration, IHttpClientFactory httpClientFactory, IShatemAccess shatemAccess)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _shatemAccess = shatemAccess;
            _shatemConfig = shatemConfig.Value;

            //_apiKey = _configuration["ShatemApiKey"];
        }


        public async Task<List<ShatemFoundArticle>> SearchArticlesAsync(string lineToSearch, string tradeMarkName = null)
        {
            //string url = _configuration["ShatemUri"];
            //string apiKey = _configuration["ShatemApiKey"];

            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;

            if (string.IsNullOrEmpty(tradeMarkName))
            {
                url = url + $"/articles/search?searchString={lineToSearch}";
            }
            else
            {
                url = url + $"/articles/search?searchString={lineToSearch}&tradeMarkNames={tradeMarkName}";
            }
            ShatemAccessModel shatemAccessModel = await _shatemAccess.GetAccessTokenAsync();
            string token = shatemAccessModel.access_token; // Retrieve the access_token property from the ShatemAccessModel object

            using (var httpClient = _httpClientFactory.CreateClient())
            {




                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        // Configure JsonSerializerOptions for case-insensitive property name handling
                        JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        List<ShatemFoundArticleWrapper> foundArticleWrappers = JsonSerializer.Deserialize<List<ShatemFoundArticleWrapper>>(jsonResponse, jsonOptions);

                        if (foundArticleWrappers != null && foundArticleWrappers.Count > 0)
                        {
                            // Extract the ShatemFoundArticle objects from the Article property of the wrapper class
                            List<ShatemFoundArticle> foundArticles = foundArticleWrappers.Select(a => a.Article).ToList();
                            return foundArticles;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Произошла ошибка при запросе. {ex.Message}");
                    // Handle web-related error
                    return null;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
                    // Handle JSON-related error
                    return null;
                }
            }
        }


        public async Task<ShatemFoundArticle> ArticleInfoAsync(string articleId, bool includeAnalogs = false)
        {
            //string url = _configuration["ShatemUri"];

            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;

            url = url + $"/articles/{articleId}";

            if (includeAnalogs)
            {
                url += "?analogs=true";
            }
            ShatemAccessModel shatemAccessModel = await _shatemAccess.GetAccessTokenAsync();
            string token = shatemAccessModel.access_token; // Retrieve the access_token property from the ShatemAccessModel object

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
                                return foundArticleWrapper.Article;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Произошла ошибка при запросе. Код ошибки: {response.StatusCode}");
                        return null;
                    }
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    Console.WriteLine($"Произошла ошибка при выполнении HTTP-запроса. {ex.Message}");
                    return null;
                }
                catch (System.Text.Json.JsonException ex)
                {
                    Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
                    return null;
                }
            }
        }



        public async Task<ShatemFullArticle> FullArticleInfoAsync(int articleId)
        {
            //string url = _configuration["ShatemUri"];

            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;

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
                            return shatemFullArticle;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
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
        }


        public async Task<List<ShatemArticlePrice>> SearchAvailableQuantityAsync(string articleId, bool includeAnalogs = false)
        {

            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;





            ShatemAccessModel shatemAccessModel = await _shatemAccess.GetAccessTokenAsync();
            string token = shatemAccessModel.access_token; // Retrieve the access_token property from the ShatemAccessModel object

            List<ShatemAgreement> shatemAgreements = await _shatemAccess.GetAgreementsAsync(token);

            string agreementCode = shatemAgreements.FirstOrDefault().code;


            url = $"{url}/prices/search?agreementCode={agreementCode}";

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
                    Console.WriteLine($"Exception: {ex.Message}");
                    return null;
                }
            }
        }

        public async Task<List<string>> SearchContentsAsync(string contentId, int heightSize = 400, int widthSize = 400)
        {

            string url = _shatemConfig.Uri;
            string apiKey = _shatemConfig.ApiKey;


            url = url + "/contents/search";

            ShatemAccessModel shatemAccessModel = await _shatemAccess.GetAccessTokenAsync();
            string token = shatemAccessModel.access_token; // Retrieve the access_token property from the ShatemAccessModel object

            using (var _httpClient = _httpClientFactory.CreateClient())
            {
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    _httpClient.DefaultRequestHeaders.Accept.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
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
                                List<string> imageUrls = new List<string>();

                                foreach (var result in searchResults)
                                {
                                    // Decode base64 value and extract image URL
                                    if (!string.IsNullOrEmpty(result.Value) && result.Value.StartsWith("data:image/jpeg;base64,"))
                                    {
                                        string base64Value = result.Value.Substring("data:image/jpeg;base64,".Length);
                                        byte[] imageBytes = Convert.FromBase64String(base64Value);
                                        string imageUrl = "data:image/jpeg;base64," + Encoding.UTF8.GetString(imageBytes);
                                        imageUrls.Add(imageUrl);
                                    }
                                }

                                return imageUrls;
                            }
                        }

                        return null;
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Произошла ошибка при запросе. {ex.Message}");
                        // Handle network-related error
                        return null;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
                        // Handle JSON-related error
                        return null;
                    }
                }
            }
        }
    }

}


using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration.Json;
using DataAccessLibrary.Models.Shatem.DataAccess;
using DataAccessLibrary.ApiDataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using DataAccessLibrary.Models.Shatem.Models;
using DataAccessLibrary.ApiDataAccess;
using System.Text;
using System.Net.Http.Headers;
using DataAccessLibrary.DataService.Shatem;

var builder = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory()) //<--You would need to set the path
.AddJsonFile("appsettings.json"); //or what ever file you have the settings

IConfiguration configuration = builder.Build();
string uriShatem = configuration["ShatemUri"];

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddHttpClient();
services.AddTransient<IShatemAccess, ShatemAccess>();
//services.AddHttpClient<ShatemDataService>();
services.AddScoped<ShatemDataService>();
var serviceProvider = services.BuildServiceProvider();


var client = serviceProvider.GetService<HttpClient>();

var shatemAccessDetails = new ShatemAccessModel();
var apiKey = configuration["ShatemApiKey"];


var _shatemAccess = serviceProvider.GetService<IShatemAccess>();

var _shatemDataService = serviceProvider.GetService<ShatemDataService>();


shatemAccessDetails = await _shatemAccess.GetAccessTokenAsync();


await Console.Out.WriteLineAsync($"The access token is {shatemAccessDetails.access_token}. Token type:{shatemAccessDetails.token_type}. Expires in {shatemAccessDetails.expires_in}. Refresh token: {shatemAccessDetails.refresh_token} ");


var token = shatemAccessDetails.access_token;

var shatemAgreementDetails = _shatemAccess.GetAgreementsAsync(token).Result.FirstOrDefault();

var agreementCode = shatemAgreementDetails.code;

Console.WriteLine($"The agreement code to use in queries is {agreementCode}");


string lineToSearch = "PL503";
int articleFoundId = 0;
List<ShatemFoundArticle> articles = new();

if (!string.IsNullOrEmpty(lineToSearch))
{
    // Get the bearer access token
    //string token = GetAccessToken();

    // Search the line and find the article id
    articles = await _shatemDataService.SearchArticlesAsync(lineToSearch);

    if (articles is null)
    {
        Console.WriteLine($"Line '{lineToSearch}' - Артикул не найден!");
    }
    else
    {
        foreach (var a in articles)
        {
            Console.WriteLine($"{a.Id} {a.Code} {a.TradeMarkName} {a.Name} {a.Description} {a.UnitOfMeasure}");
        }

        articleFoundId = articles.LastOrDefault().Id;

        // Search the available quantity by article id
        //int availableQuantity = SearchAvailableQuantity(token, articleId, uriShatem);
    }

}

if (articleFoundId > 0)
{
    List<ShatemArticlePrice> shatemArticlePrices = await _shatemDataService.SearchAvailableQuantityAsync(articleFoundId.ToString(), true);



    if (shatemArticlePrices is null)
    {
        Console.WriteLine($"Не найдено наличия по артикулу {articleFoundId}");
    }
    else
    {
        foreach (var sh in shatemArticlePrices)
        {

            if (sh.articleId == articleFoundId)
            {
                Console.WriteLine($"{sh.articleId} {sh.locationCodeReal} {sh.addInfo.city} {sh.quantity.available} {sh.price.value}");
                
                ShatemFullArticle fa = await _shatemDataService.FullArticleInfoAsync(sh.articleId);

                Console.WriteLine($"{fa.tradeMark.name}  {fa.contents.FirstOrDefault().contentId}");

                string imgurl = _shatemDataService.SearchContentsAsync(fa.contents.FirstOrDefault().contentId).Result.FirstOrDefault();

                Console.WriteLine(  $"{imgurl}");

            }
            else
            {

                ShatemFoundArticle zamena = await _shatemDataService.ArticleInfoAsync(sh.articleId.ToString());

                Console.WriteLine($"Замена! {zamena.Id} {zamena.Code} {zamena.TradeMarkName} {zamena.Name} {sh.locationCodeReal} {sh.addInfo.city} {sh.quantity.available} {sh.price.value}");
            }

        }
    }



}


Console.ReadLine();

//static async Task<List<ShatemFoundArticle>> SearchArticlesAsync(string token, string lineToSearch, IConfiguration configuration, string tradeMarkName = null)
//{
//    string url = configuration["ShatemUri"];

//    if (string.IsNullOrEmpty(tradeMarkName))
//    {
//        url = url + $"/articles/search?searchString={lineToSearch}";
//    }
//    else
//    {
//        url = url + $"/articles/search?searchString={lineToSearch}&tradeMarkNames={tradeMarkName}";
//    }

//    HttpClient httpClient = new HttpClient();
//    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

//    try
//    {
//        HttpResponseMessage response = await httpClient.GetAsync(url);

//        if (response.IsSuccessStatusCode)
//        {
//            string jsonResponse = await response.Content.ReadAsStringAsync();

//            // Configure JsonSerializerOptions for case-insensitive property name handling
//            JsonSerializerOptions jsonOptions = new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            };

//            List<ShatemFoundArticleWrapper> foundArticleWrappers = JsonSerializer.Deserialize<List<ShatemFoundArticleWrapper>>(jsonResponse, jsonOptions);

//            if (foundArticleWrappers != null && foundArticleWrappers.Count > 0)
//            {
//                // Extract the ShatemFoundArticle objects from the Article property of the wrapper class
//                List<ShatemFoundArticle> foundArticles = foundArticleWrappers.Select(a => a.Article).ToList();
//                return foundArticles;
//            }
//            else
//            {
//                return null;
//            }
//        }
//        else
//        {
//            return null;
//        }
//    }
//    catch (HttpRequestException ex)
//    {
//        Console.WriteLine($"Произошла ошибка при запросе. {ex.Message}");
//        // Handle web-related error
//        return null;
//    }
//    catch (JsonException ex)
//    {
//        Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
//        // Handle JSON-related error
//        return null;
//    }
//}


//static async Task<ShatemFoundArticle> ArticleInfoAsync(string token, string articleId, IConfiguration configuration, bool includeAnalogs = false)
//{
//    string url = configuration["ShatemUri"];

//    url = url + $"/articles/{articleId}";

//    if (includeAnalogs)
//    {
//        url += "?analogs=true";
//    }

//    HttpClient httpClient = new HttpClient();
//    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
//    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

//    try
//    {
//        HttpResponseMessage response = await httpClient.GetAsync(url);

//        if (response.IsSuccessStatusCode)
//        {
//            string jsonResponse = await response.Content.ReadAsStringAsync();

//            if (!string.IsNullOrEmpty(jsonResponse))
//            {
//                JsonSerializerOptions jsonOptions = new JsonSerializerOptions
//                {
//                    PropertyNameCaseInsensitive = true
//                };

//                ShatemFoundArticleWrapper foundArticleWrapper = JsonSerializer.Deserialize<ShatemFoundArticleWrapper>(jsonResponse, jsonOptions);

//                if (foundArticleWrapper != null && foundArticleWrapper.Article != null)
//                {
//                    return foundArticleWrapper.Article;
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }
//        else
//        {
//            Console.WriteLine($"Произошла ошибка при запросе. Код ошибки: {response.StatusCode}");
//            return null;
//        }
//    }
//    catch (System.Net.Http.HttpRequestException ex)
//    {
//        Console.WriteLine($"Произошла ошибка при выполнении HTTP-запроса. {ex.Message}");
//        return null;
//    }
//    catch (System.Text.Json.JsonException ex)
//    {
//        Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
//        return null;
//    }
//}



//static async Task<ShatemFullArticle> FullArticleInfoAsync(string token, int articleId, IConfiguration configuration)
//{
//    string url = configuration["ShatemUri"];

//    url = $"{url}/articles/{articleId}?include=trademark,contents,extended_info"; // Update URL to include query parameters

//    HttpClient httpClient = new HttpClient();
//    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
//    httpClient.DefaultRequestHeaders.Add("accept", "text/plain");

//    try
//    {
//        HttpResponseMessage response = await httpClient.GetAsync(url);
//        response.EnsureSuccessStatusCode();

//        string jsonResponse = await response.Content.ReadAsStringAsync();

//        if (!string.IsNullOrEmpty(jsonResponse))
//        {
//            JsonSerializerOptions jsonOptions = new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            };

//            ShatemFullArticle shatemFullArticle = JsonSerializer.Deserialize<ShatemFullArticle>(jsonResponse, jsonOptions);

//            if (shatemFullArticle != null)
//            {
//                return shatemFullArticle;
//            }
//            else
//            {
//                return null;
//            }
//        }
//        else
//        {
//            return null;
//        }
//    }
//    catch (HttpRequestException ex)
//    {
//        Console.WriteLine($"Произошла ошибка при запросе. {ex.Message}");
//        return null;
//    }
//    catch (JsonException ex)
//    {
//        Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
//        return null;
//    }
//}





//static async Task<List<ShatemArticlePrice>> SearchAvailableQuantityAsync(string token, string articleId, string uriShatem, string agreementCode, bool includeAnalogs = false)
//{
//    string url = $"{uriShatem}/prices/search?agreementCode={agreementCode}";

//    HttpClient httpClient = new HttpClient();
//    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
//    httpClient.DefaultRequestHeaders.Add("Accept", "application/json"); // Set the Accept header

//    try
//    {
//        // Create the JSON body
//        string jsonBody = $"[{{ \"articleId\": {articleId}, \"includeAnalogs\": {includeAnalogs.ToString().ToLower()} }}]";

//        // Convert the JSON body to HttpContent
//        HttpContent httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
//        HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
//        response.EnsureSuccessStatusCode();

//        string jsonResponse = await response.Content.ReadAsStringAsync();

//        // Configure JsonSerializerOptions for case-insensitive property name handling
//        JsonSerializerOptions jsonOptions = new JsonSerializerOptions
//        {
//            PropertyNameCaseInsensitive = true
//        };

//        List<ShatemArticlePrice> foundQty = JsonSerializer.Deserialize<List<ShatemArticlePrice>>(jsonResponse, jsonOptions);

//        return foundQty;
//    }
//    catch (HttpRequestException ex)
//    {
//        Console.WriteLine($"Произошла ошибка при запросе: {ex.Message}");
//        return null;
//    }
//    catch (JsonException ex)
//    {
//        Console.WriteLine($"Произошла ошибка при десериализации JSON: {ex.Message}");
//        return null;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Exception: {ex.Message}");
//        return null;
//    }
//}

//static async Task<List<string>> SearchContentsAsync(string token, string contentId, IConfiguration configuration, int heightSize = 400, int widthSize = 400)
//{
//    string url = configuration["ShatemUri"] + "/contents/search";

//    using (HttpClient httpClient = new HttpClient())
//    {
//        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
//        httpClient.DefaultRequestHeaders.Accept.Clear();
//        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
//        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//        ContentKey contentKey = new ContentKey
//        {
//            ContentId = contentId,
//            HeightSize = heightSize,
//            WidthSize = widthSize
//        };

//        SearchContentsRequest request = new SearchContentsRequest
//        {
//            ContentKeys = new List<ContentKey> { contentKey }
//        };

//        string jsonRequest = JsonSerializer.Serialize(request);
//        StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

//        try
//        {
//            HttpResponseMessage response = await httpClient.PostAsync(url, content);

//            if (response.IsSuccessStatusCode)
//            {
//                string jsonResponse = await response.Content.ReadAsStringAsync();
//                List<SearchResult> searchResults = JsonSerializer.Deserialize<List<SearchResult>>(jsonResponse);

//                if (searchResults != null && searchResults.Count > 0)
//                {
//                    List<string> imageUrls = new List<string>();

//                    foreach (var result in searchResults)
//                    {
//                        // Decode base64 value and extract image URL
//                        if (!string.IsNullOrEmpty(result.Value) && result.Value.StartsWith("data:image/jpeg;base64,"))
//                        {
//                            string base64Value = result.Value.Substring("data:image/jpeg;base64,".Length);
//                            byte[] imageBytes = Convert.FromBase64String(base64Value);
//                            string imageUrl = "data:image/jpeg;base64," + Encoding.UTF8.GetString(imageBytes);
//                            imageUrls.Add(imageUrl);
//                        }
//                    }

//                    return imageUrls;
//                }
//            }

//            return null;
//        }
//        catch (HttpRequestException ex)
//        {
//            Console.WriteLine($"Произошла ошибка при запросе. {ex.Message}");
//            // Handle network-related error
//            return null;
//        }
//        catch (JsonException ex)
//        {
//            Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
//            // Handle JSON-related error
//            return null;
//        }
//    }
//}

//static async Task<List<string>> SearchContentsAsync(string token, string contentId, IConfiguration configuration, int heightSize = 400, int widthSize = 400)
//{
//    string url = configuration["ShatemUri"] + "/contents/search";

//    using (HttpClient httpClient = new HttpClient())
//    {
//        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
//        httpClient.DefaultRequestHeaders.Accept.Clear();
//        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
//        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//        ContentKey contentKey = new ContentKey
//        {
//            ContentId = contentId,
//            HeightSize = heightSize,
//            WidthSize = widthSize
//        };

//        SearchContentsRequest request = new SearchContentsRequest
//        {
//            ContentKeys = new List<ContentKey> { contentKey }
//        };

//        string jsonRequest = JsonSerializer.Serialize(request);
//        StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

//        try
//        {
//            HttpResponseMessage response = await httpClient.PostAsync(url, content);

//            if (response.IsSuccessStatusCode)
//            {
//                string jsonResponse = await response.Content.ReadAsStringAsync();
//                List<SearchResult> searchResults = JsonSerializer.Deserialize<List<SearchResult>>(jsonResponse);

//                if (searchResults != null && searchResults.Count > 0)
//                {
//                    List<string> imageUrls = new List<string>();

//                    foreach (var result in searchResults)
//                    {
//                        // Decode base64 value and extract image URL
//                        if (!string.IsNullOrEmpty(result.Value) && result.Value.StartsWith("data:image/jpeg;base64,"))
//                        {
//                            string base64Value = result.Value.Substring("data:image/jpeg;base64,".Length);
//                            byte[] imageBytes = Convert.FromBase64String(base64Value);
//                            string imageUrl = "data:image/jpeg;base64," + Encoding.UTF8.GetString(imageBytes);
//                            imageUrls.Add(imageUrl);
//                        }
//                    }

//                    return imageUrls;
//                }
//            }

//            return null;
//        }
//        catch (HttpRequestException ex)
//        {
//            Console.WriteLine($"Произошла ошибка при запросе. {ex.Message}");
//            // Handle network-related error
//            return null;
//        }
//        catch (JsonException ex)
//        {
//            Console.WriteLine($"Произошла ошибка при десериализации JSON. {ex.Message}");
//            // Handle JSON-related error
//            return null;
//        }
//    }
//}





//    while (true)
//    {
//        // Check if the current time is between 8 am and 1 am next day
//        DateTime currentTime = DateTime.Now;
//        DateTime startTime = currentTime.Date.AddHours(8);
//        DateTime endTime = currentTime.AddDays(1).Date.AddHours(1);

//        if (currentTime >= startTime && currentTime < endTime)
//        {
//            // Get a random time interval between 5 and 80 minutes
//            int intervalInMinutes = new Random().Next(5, 81);

//            // Wait for the random time interval
//            Thread.Sleep(intervalInMinutes * 60 * 1000);

//            // Read the line to search from the database
//            string lineToSearch = ReadLineToSearchFromDatabase();

//            if (!string.IsNullOrEmpty(lineToSearch))
//            {
//                // Get the bearer access token
//                string token = GetAccessToken();

//                // Search the line and find the article id
//                string articleId = SearchArticleId(token, lineToSearch);

//                if (string.IsNullOrEmpty(articleId))
//                {
//                    Console.WriteLine($"Line '{lineToSearch}' - Article not found!");
//                }
//                else
//                {
//                    // Search the available quantity by article id
//                    int availableQuantity = SearchAvailableQuantity(token, articleId);

//                    Console.WriteLine($"Line '{lineToSearch}' - Available quantity for article {articleId}: {availableQuantity}");
//                }
//            }
//        }
//        else
//        {
//            // Wait for 10 minutes and check again
//            Thread.Sleep(10 * 60 * 1000);
//        }
//    }
//}

//static async Task<string> GetAccessTokenAsync()
//        {
//            var client = _httpClientFactory.CreateClient();

//            var body = new Dictionary<string, string>
//    {
//        { "grant_type", "client_credentials" },
//        { "client_id", "your-client-id" },
//        { "client_secret", "your-client-secret" }
//    };

//            var content = new FormUrlEncodedContent(body);

//            var response = await client.PostAsync("https://your-auth-server.com/token", content);

//            var jsonResponse = await response.Content.ReadAsStringAsync();
//            var jsonObject = JObject.Parse(jsonResponse);

//            return (string)jsonObject["access_token"];
//        }

//        static string SearchArticleId(string token, string lineToSearch)
//        {
//            string url = $"https://your-api-server.com/search?line={lineToSearch}";

//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//            request.Method = "GET";
//            request.Headers.Add("Authorization", $"Bearer {token}");

//            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

//            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
//            {
//                string jsonResponse = reader.ReadToEnd();
//                dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);

//                if (jsonObject.articles.Count > 0)
//                {
//                    return jsonObject.articles[0].id;
//                }
//                else
//                {
//                    return null;
//                }
//            }
//        }

//        static int SearchAvailableQuantity(string token, string articleId)
//        {
//            string url = $"https://your-api-server.com/article/{articleId}";

//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//            request.Method = "GET";
//            request.Headers.Add("Authorization", $"Bearer {token}");
//            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

//            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
//            {
//                string jsonResponse = reader.ReadToEnd();
//                dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);

//                return (int)jsonObject.availableQuantity;
//            }
//        }

//        static string ReadLineToSearchFromDatabase()
//        {
//            string connectionString = "your-connection-string";
//            string query = "SELECT TOP 1 line_to_search FROM lines_to_search WHERE is_searched = 0 ORDER BY created_at ASC";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                SqlCommand command = new SqlCommand(query, connection);
//                connection.Open();

//                using (SqlDataReader reader = command.ExecuteReader())
//                {
//                    if (reader.HasRows)
//                    {
//                        reader.Read();
//                        string lineToSearch = reader.GetString(0);

//                        // Mark the line as searched
//                        UpdateLineToSearch(lineToSearch);

//                        return lineToSearch;
//                    }
//                    else
//                    {
//                        return null;
//                    }
//                }
//            }
//        }

//        static void UpdateLineToSearch(string lineToSearch)
//        {
//            string connectionString = "your-connection-string";
//            string query = "UPDATE lines_to_search SET is_searched = 1 WHERE line_to_search = @lineToSearch";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                SqlCommand command = new SqlCommand(query, connection);
//                command.Parameters.AddWithValue("@lineToSearch", lineToSearch);
//                connection.Open();
//                command.ExecuteNonQuery();
//            }
//        }

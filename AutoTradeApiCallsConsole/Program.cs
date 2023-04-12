// See https://aka.ms/new-console-template for more information
using DataAccessLibrary.Models;
using System.Text.Json;
using Org.BouncyCastle.Crypto.Tls;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;


var root = new AutoTradeStockItemResult();


var data = new
{
    auth_key = "613c2c445caf04703e94f44f46de0dfd",
    method = "getStocksAndPrices",
    @params = new
           {
               storages = new[] { 0 },
               items = new Dictionary<string, Dictionary<string, int>>
        {
            { "KE100 LFW/X", new Dictionary<string, int> { { "XYG", 1 } } }
        }
           }
};

var content = new StringContent($"data={JsonConvert.SerializeObject(data)}", Encoding.UTF8, "application/x-www-form-urlencoded");

using (var _httpClient = HttpClientFactory.Create())
{
    var response = await _httpClient.PostAsync("https://api2.autotrade.su/?json", content);

    if (response.IsSuccessStatusCode)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        // Deserialize the JSON response into your C# class
        root = JsonConvert.DeserializeObject<AutoTradeStockItemResult>(responseString);
        // Use the deserialized object
    }
    else
    {
        return;
        // Handle the error response
    }
}


Console.WriteLine(root.code);
Console.WriteLine(root.message);
foreach (var item in root.items)
{
    Console.WriteLine(item.Key);
    Console.WriteLine(item.Value.name);

    foreach (var stock in item.Value.stocks)
    {
        Console.WriteLine(stock.Key);
        Console.WriteLine(stock.Value.name);
        Console.WriteLine($"Количество распаковано {stock.Value.quantity_unpacked}. Количество запаковано {stock.Value.quantity_packed}");
    }
}
Console.ReadLine();
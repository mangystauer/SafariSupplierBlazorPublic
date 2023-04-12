using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class StocksRequest
    {
        [JsonPropertyName("auth_key")]
        public string AuthKey { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("params")]
        public StocksParams Params { get; set; }
    }

    public class StocksParams
    {
        [JsonPropertyName("storages")]
        public int[] Storages { get; set; }

        [JsonPropertyName("items")]
        public Dictionary<string, Dictionary<string, int>> Items { get; set; }
    }

    public class StocksResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public StocksData Data { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    public class StocksData
    {
        [JsonPropertyName("items")]
        public Dictionary<string, StocksItem> Items { get; set; }
    }

    public class StocksItem
    {
        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }


}


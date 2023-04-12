using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class AutoTradeItemStockQuery
    {
        public string auth_key { get; set; }
        public string method { get; set; }
        [JsonPropertyName("params")]
        public Params _params { get; set; }
    }

    public class Params
    {
        public int[] storages { get; set; } = new int[0];
        [JsonPropertyName("items")]
        public ItemsQ items { get; set; }
        public int withDelivery { get; set; } = 0;
        public int withPriceUst { get; set; } = 0;
        public int strict { get; set; } = 1;

    }

    public class ItemsQ
    {
        [JsonPropertyName($"артикул")]
        public PartnumberQ artnumber { get; set; }
    }

    public class PartnumberQ
    {
        [JsonPropertyName("бренд")]
        public string Brand { get; set; }
        [JsonPropertyName("количество")]
        public int qty { get; set; } = 1;
    }

}

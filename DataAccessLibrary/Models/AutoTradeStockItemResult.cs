using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class Item
    {
        public string article { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string brand { get; set; }
        public int discounted { get; set; }
        public string inside_id_in { get; set; }
        public double price { get; set; }
        public double price_ust { get; set; }
        public string currency { get; set; }
        public string currency_ust { get; set; }
        public string unit { get; set; }
        public Dictionary<string, Stock> stocks { get; set; }
        public string part_type_name { get; set; }
    }

    public class Stock
    {
        public string id { get; set; }
        public string name { get; set; }
        public string legend { get; set; }
        public int quantity_unpacked { get; set; }
        public int quantity_packed { get; set; }
        public int delivery_period { get; set; }
        public int in_way { get; set; }
        public List<QuantityPackedDetail> quantity_packed_detail { get; set; }
    }

    public class QuantityPackedDetail
    {
        public int boxes_amount { get; set; }
        public int quantity_in_box { get; set; }
        public int total { get; set; }
    }

    public class AutoTradeStockItemResult
    {
        public Dictionary<string, Item> items { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }

}




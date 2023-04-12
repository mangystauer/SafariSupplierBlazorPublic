using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.Shatem.Models
{

    public class ShatemLocationList
    {
        public List<Stock> ShatemAvailableStocks { get; set; }
    }

    public class Stock
    {
        public string code { get; set; }
        public string name { get; set; }
        public string city { get; set; }
    }

}

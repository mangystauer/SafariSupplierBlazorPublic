using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.Shatem.Models
{
    public class ShatemFoundArticleWrapper
    {
        public ShatemFoundArticle Article { get; set; }
    }


    public class ShatemFoundArticle : IShatemFoundArticle
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string TradeMarkName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public int?  AvailableQty { get; set; }
        public decimal? Price { get; set; }
    }
}

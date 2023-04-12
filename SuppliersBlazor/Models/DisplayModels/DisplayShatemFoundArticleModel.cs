using DataAccessLibrary.Models.Shatem.Models;

namespace SuppliersBlazor.Models.DisplayModels
{
    public class DisplayShatemFoundArticleModel : IShatemFoundArticle
    {

        public int Id { get; set; }
        public string Code { get; set; }
        public string TradeMarkName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public int AvailableQty { get; set; }
        public int AvailableQtyAlmaty { get; set; }
    }
}

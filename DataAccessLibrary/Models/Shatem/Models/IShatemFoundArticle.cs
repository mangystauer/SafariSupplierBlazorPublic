namespace DataAccessLibrary.Models.Shatem.Models
{
    public interface IShatemFoundArticle
    {
        string Code { get; set; }
        string Description { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        string TradeMarkName { get; set; }
        string UnitOfMeasure { get; set; }
    }
}
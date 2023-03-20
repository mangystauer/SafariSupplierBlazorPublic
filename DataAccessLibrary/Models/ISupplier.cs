namespace DataAccessLibrary.Models
{
    public interface ISupplier
    {
        int avail { get; set; }
        int? avail1 { get; set; }
        int? avail2 { get; set; }
        bool avail2t { get; set; }
        int? avail3 { get; set; }
        bool avail3t { get; set; }
        int? avail4 { get; set; }
        bool avail4t { get; set; }
        int? avail5 { get; set; }
        bool avail5t { get; set; }
        int? avail6 { get; set; }
        bool avail6t { get; set; }
        string? brand { get; set; }
        int? brand_col { get; set; }
        decimal cost { get; set; }
        int? cross1col { get; set; }
        bool cross1t { get; set; }
        int? cross2col { get; set; }
        bool cross2t { get; set; }
        int? cross3col { get; set; }
        bool cross3t { get; set; }
        int? cross4col { get; set; }
        bool cross4t { get; set; }
        int? cross5col { get; set; }
        bool cross5t { get; set; }
        int? cross6col { get; set; }
        bool cross6t { get; set; }
        string? desc_manual { get; set; }
        int? descr { get; set; }
        bool hasnobrand { get; set; }
        bool hasnomodels { get; set; }
        bool hasnoqty { get; set; }
        int id { get; set; }
        bool manual_description { get; set; }
        decimal markupabove { get; set; }
        decimal markupbelow { get; set; }
        int markupthreshold { get; set; }
        bool massUpload { get; set; }
        int? models { get; set; }
        bool not_round_to_200 { get; set; }
        int? p_altnum1 { get; set; }
        string? p_time { get; set; }
        int partnum_col { get; set; }
        string prefix { get; set; }
        int? qty { get; set; }
        string supplier { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SuppliersBlazor.Models
{
    public class Supplier
    {
        [Required]
        [Key]
        public string id { get; set; }
        public string supplier { get; set; }
        public string prefix { get; set; }
        public string p_time { get; set; }
        public string massUpload { get; set; }
        public string hasnobrand { get; set; }
        public string brand { get; set; }
        public string brand_col { get; set; }
        public string partnum_col { get; set; }
        public string manual_description { get; set; }
        public string descr { get; set; }
        public string desc_manual { get; set; }
        public string avail { get; set; }
        public string hasnoqty { get; set; }
        public string qty { get; set; }
        public string cost { get; set; }
        public string hasnomodels { get; set; }
        public string models { get; set; }
        public string p_altnum1 { get; set; }
        public string avail1 { get; set; }
        public string avail2 { get; set; }
        public string avail3 { get; set; }
        public string avail4 { get; set; }
        public string avail5 { get; set; }
        public string avail6 { get; set; }
        public string avail2t { get; set; }
        public string avail3t { get; set; }
        public string avail4t { get; set; }
        public string avail5t { get; set; }
        public string avail6t { get; set; }
        public string not_round_to_200 { get; set; }
        public string markupthreshold { get; set; }
        public string markupbelow { get; set; }
        public string markupabove { get; set; }
        public string cross1t { get; set; }
        public string cross2t { get; set; }
        public string cross3t { get; set; }
        public string cross4t { get; set; }
        public string cross5t { get; set; }
        public string cross6t { get; set; }
        public string cross1col { get; set; }
        public string cross2col { get; set; }
        public string cross3col { get; set; }
        public string cross4col { get; set; }
        public string cross5col { get; set; }
        public string cross6col { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class Supplier
    {
        [Required]
        [Key]
        public int id { get; set; }
        [Required]
        public string supplier { get; set; }
        [Required]
        public string prefix { get; set; }
        public string? p_time { get; set; }
        public bool massUpload { get; set; }
        public bool hasnobrand { get; set; }
        public string? brand { get; set; }
        public int? brand_col { get; set; }
        [Required]
        public int partnum_col { get; set; }
        public string? manual_description { get; set; }
        public int? descr { get; set; }
        public string? desc_manual { get; set; }
        [Required]
        public int avail { get; set; }
        public bool hasnoqty { get; set; }
        public int? qty { get; set; }
        [Required]
        public decimal cost { get; set; }
        public bool hasnomodels { get; set; }
        public int? models { get; set; }
        public int? p_altnum1 { get; set; }
        public int? avail1 { get; set; }
        public int? avail2 { get; set; }
        public int? avail3 { get; set; }
        public int? avail4 { get; set; }
        public int? avail5 { get; set; }
        public int? avail6 { get; set; }
        public bool avail2t { get; set; }
        public bool avail3t { get; set; }
        public bool avail4t { get; set; }
        public bool avail5t { get; set; }
        public bool avail6t { get; set; }
        public bool not_round_to_200 { get; set; }
        [Required]
        public int markupthreshold { get; set; }
        [Required]
        public decimal markupbelow { get; set; }
        [Required]
        public decimal markupabove { get; set; }
        public bool cross1t { get; set; }
        public bool cross2t { get; set; }
        public bool cross3t { get; set; }
        public bool cross4t { get; set; }
        public bool cross5t { get; set; }
        public bool cross6t { get; set; }
        public int? cross1col { get; set; }
        public int? cross2col { get; set; }
        public int? cross3col { get; set; }
        public int? cross4col { get; set; }
        public int? cross5col { get; set; }
        public int? cross6col { get; set; }
    }


}

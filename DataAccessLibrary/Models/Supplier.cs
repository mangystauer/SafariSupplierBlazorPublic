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
        
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string supplier { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 2)]
        public string prefix { get; set; }
        public string? p_time { get; set; } = "2-5";
        public bool massUpload { get; set; } = false;
        public bool hasnobrand { get; set; } = false;
        public string? brand { get; set; } = null;
        public int? brand_col { get; set; } = null;
        [Required]
        public int partnum_col { get; set; }
        public bool manual_description { get; set; } = false;
        public int? descr { get; set; } = 0;
        public string? desc_manual { get; set; } = null;
        [Required]
        public int avail { get; set; }
        public bool hasnoqty { get; set; } = false;
        public int? qty { get; set; } = null;
        [Required]
        public decimal cost { get; set; }
        public bool hasnomodels { get; set; } = false;
        public int? models { get; set; } = null;
        public int? p_altnum1 { get; set; } = null;
        public int? avail1 { get; set; } = 0;
        public int? avail2 { get; set; } = 0;
        public int? avail3 { get; set; } = 0;
        public int? avail4 { get; set; } = 0;
        public int? avail5 { get; set; } = 0;
        public int? avail6 { get; set; } = 0;
        public bool avail2t { get; set; } = false;   
        public bool avail3t { get; set; } = false;
        public bool avail4t { get; set; } = false;
        public bool avail5t { get; set; } = false;
        public bool avail6t { get; set; } = false;
        public bool not_round_to_200 { get; set; } = false;
        [Required]
        public int markupthreshold { get; set; } = 150000;
        [Required]
        public decimal markupbelow { get; set; } = 0.3m;
        [Required]
        public decimal markupabove { get; set; } = 0.25m;
        public bool cross1t { get; set; } = false;
        public bool cross2t { get; set; } = false;
        public bool cross3t { get; set; } = false;
        public bool cross4t { get; set; } = false;
        public bool cross5t { get; set; } = false;
        public bool cross6t { get; set; } = false;
        public int? cross1col { get; set; } = null;
        public int? cross2col { get; set; } = null;
        public int? cross3col { get; set; } = null;
        public int? cross4col { get; set; } = null;
        public int? cross5col { get; set; } = null;
        public int? cross6col { get; set; } = null;
    }


}

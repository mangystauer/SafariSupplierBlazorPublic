using System.ComponentModel.DataAnnotations;

namespace SuppliersBlazor.Models.Search_Models
{
    public class ShatemPartsSearchModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Не меньше 3 символов")]
        public string PartNumber { get; set; }
        
        
        [MinLength(3, ErrorMessage = "Не меньше 3 символов")]
        public string TradeMarkName { get; set; }

        public bool IncludeAnalogs { get; set; } = true;

    }
}

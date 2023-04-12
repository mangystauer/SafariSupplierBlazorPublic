using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.Shatem.Models
{
    public class ShatemAgreement
    {
            public string code { get; set; }
            public string agreementGroup { get; set; }
            public string description { get; set; }
            public string locationCode { get; set; }
            public string currencyCode { get; set; }
            public bool isActive { get; set; }
            public bool isEnabledPickup { get; set; }
            public bool isEnabledDelivery { get; set; }

    }
}

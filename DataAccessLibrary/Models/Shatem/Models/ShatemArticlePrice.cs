using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.Shatem.Models
{
    public class ShatemArticlePrice
    {
            public string id { get; set; }
            public int articleId { get; set; }
            public string locationCode { get; set; }
            public string locationCodeReal { get; set; }
            public string agreementCode { get; set; }
            public string type { get; set; }
            public Price price { get; set; }
            public Quantity quantity { get; set; }
            public Supplyprobability supplyProbability { get; set; }
            public Addinfo addInfo { get; set; }
            public object priority { get; set; }
            public int hash { get; set; }
            public object[] deliveryDateTimes { get; set; }
            public DateTime shippingDateTime { get; set; }
            public object isImport { get; set; }
            public bool isFree { get; set; }
        }

        public class Price
        {
            public int value { get; set; }
            public int valueWithMargin { get; set; }
            public object valueRecommended { get; set; }
            public string currencyCode { get; set; }
        }

        public class Quantity
        {
            public int available { get; set; }
            public string availableType { get; set; }
            public int multiplicity { get; set; }
            public int minimum { get; set; }
            public object maximum { get; set; }
        }

        public class Supplyprobability
        {
            public object lastUpdateDateTime { get; set; }
            public object rating { get; set; }
        }

        public class Addinfo
        {
            public string city { get; set; }
            public bool isSale { get; set; }
            public string comment { get; set; }
            public bool isReturnAllowed { get; set; }
            public string warningText { get; set; }
        }

}

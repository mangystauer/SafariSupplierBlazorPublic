using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.Shatem.Models
{

    public class ShatemFullArticle
    {
        public Article article { get; set; }
        public Trademark tradeMark { get; set; }
        public ContentFromFullArticle[] contents { get; set; }
        public Extendedinfo extendedInfo { get; set; }
    }

    public class Article
    {
        public int id { get; set; }
        public string code { get; set; }
        public string tradeMarkName { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string unitOfMeasure { get; set; }
    }

    public class Trademark
    {
        public string name { get; set; }
        public string country { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string catalogUrl { get; set; }
        public bool extendedWarranty { get; set; }
    }

    public class Extendedinfo
    {
        public string extendedDescription { get; set; }
        public Original[] originals { get; set; }
        public Characteristic[] characteristics { get; set; }
        public Applicability[] applicability { get; set; }
    }

    public class Original
    {
        public string[] articleCodes { get; set; }
        public string tradeMarkName { get; set; }
    }

    public class Characteristic
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class Applicability
    {
        public string name { get; set; }
        public Model[] models { get; set; }
    }

    public class Model
    {
        public string name { get; set; }
        public Modification[] modifications { get; set; }
    }

    public class Modification
    {
        public string name { get; set; }
        public string mnEngineCode { get; set; }
        public string beginDate { get; set; }
        public string endDate { get; set; }
        public string kWt { get; set; }
        public string hPower { get; set; }
        public string cylinders { get; set; }
        public string vol { get; set; }
    }

    public class ContentFromFullArticle
    {
        public int sortOrder { get; set; }
        public string contentId { get; set; }
        public string contentType { get; set; }
    }

}

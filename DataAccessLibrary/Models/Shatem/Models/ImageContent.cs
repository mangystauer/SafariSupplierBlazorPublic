using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.Shatem.Models
{
    public class ImageContent
    {
        public string Id { get; set; }
        public List<Image> Images { get; set; }
    }

    public class Image
    {
        public string Value { get; set; }
    }

    public class ContentKey
    {
        public string ContentId { get; set; }
        public int HeightSize { get; set; }
        public int WidthSize { get; set; }
    }

    public class SearchContentsRequest
    {
        public List<ContentKey> ContentKeys { get; set; }
    }


    public class SearchResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

}

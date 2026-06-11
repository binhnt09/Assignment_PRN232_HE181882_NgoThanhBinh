using System.Text.Json.Serialization;

namespace Assignment_1_FE.Models
{
    public class ODataResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Value { get; set; }
        [JsonPropertyName("@odata.count")]
        public int? OdataCount { get; set; }
    }
}

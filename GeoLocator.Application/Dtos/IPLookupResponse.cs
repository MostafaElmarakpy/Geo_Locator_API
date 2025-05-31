using System.Text.Json.Serialization;
namespace GeoLocator.Application.Dtos
{
    public class IPLookupResponse
    {

        [JsonPropertyName("ip")]
        public string? IP { get; set; } = string.Empty;

        [JsonPropertyName("country_name")]
        public string? Country_Name { get; set; } = string.Empty;

        [JsonPropertyName("country_code2")]

        public string Country_Code { get; set; } = string.Empty;
        [JsonPropertyName("continent_code")]
        public string ContinentCode { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("zipcode")]
        public string Zipcode { get; set; } = string.Empty;
    }
}

using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    public record SpotifyExternalIdsDto
    {
        [JsonPropertyName("isrc")]
        public required string ISRC { get; init; }

        [JsonPropertyName("ean")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EAN { get; init; }

        [JsonPropertyName("upc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UPC { get; init; }
    }
}

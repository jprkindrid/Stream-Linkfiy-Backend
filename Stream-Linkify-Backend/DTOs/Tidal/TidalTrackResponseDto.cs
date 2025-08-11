using System.Text.Json.Serialization;

namespace Stream_Linkify_Backend.DTOs.Tidal
{
    public record TidalTrackResponseDto(
        [property: JsonPropertyName("data")] TidalTrackData Data,
        [property: JsonPropertyName("links")] TidalLinks Links,
        [property: JsonPropertyName("included")] List<TidalIncludedItem>? Included
    );

    public record TidalTrackData(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("attributes")] TidalTrackAttributes Attributes,
        [property: JsonPropertyName("relationships")] Dictionary<string, TidalLinks> Relationships
    );
}

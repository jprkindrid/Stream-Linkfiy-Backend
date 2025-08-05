using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    public record SpotifyArtistDto
    {
        [JsonPropertyName("external_urls")]
        public required SpotifyExternalUrlsDto ExternalUrls { get; init; }

        [JsonPropertyName("href")]
        public required string Href { get; init; }

        [JsonPropertyName("id")]
        public required string Id { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("type")]
        public required string Type { get; init; }

        [JsonPropertyName("uri")]
        public required string Uri { get; init; }
    }
}

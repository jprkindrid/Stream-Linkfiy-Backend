using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    public record SpotifyLinkedFromDto
    {
        [JsonPropertyName("external_urls")]
        public SpotifyExternalUrlsDto ExternalUrls { get; init; }

        [JsonPropertyName("href")]
        public string Href { get; init; }

        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("type")]
        public string Type { get; init; }

        [JsonPropertyName("uri")]
        public string Uri { get; init; }
    }
}

using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    public record SpotifyImageDto
    {
        [JsonPropertyName("url")]
        public required string Url { get; init; }

        [JsonPropertyName("height")]
        public required int Height { get; init; }

        [JsonPropertyName("width")]
        public required int Width { get; init; }
    }
}

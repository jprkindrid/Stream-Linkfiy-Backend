using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    public record SpotifyExternalUrlsDto
    {
        [JsonPropertyName("spotify")]
        public required string Spotify { get; init; }
    }
}

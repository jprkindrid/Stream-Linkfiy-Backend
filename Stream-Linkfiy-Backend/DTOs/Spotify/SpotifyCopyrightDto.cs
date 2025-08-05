using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    public record SpotifyCopyrightDto
    {
        [JsonPropertyName("text")]
        public string Text { get; init; }

        [JsonPropertyName("type")]
        public string Type { get; init; }
    }
}

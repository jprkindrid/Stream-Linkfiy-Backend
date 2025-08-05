using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    public record SpotifyAccessTokenDto
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; init; }

        [JsonPropertyName("token_type")]
        public required string TokenType { get; init; }

        [JsonPropertyName("expires_in")]
        public required int ExpiresIn { get; init; }

        [JsonPropertyName("expires_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? ExpiresAt { get; init; }
    }
}

using System.Text.Json.Serialization;

namespace Stream_Linkify_Backend.DTOs.Tidal
{
    public record TidalAccessTokenDto
    {
        [property: JsonPropertyName("scope")] public required string Scope { get; set; }
        [property: JsonPropertyName("token_type")] public required string TokenType { get; set; }
        [property: JsonPropertyName("access_token")] public required string AccessToken { get; set; }
        [property: JsonPropertyName("expires_in")] public required long ExpiresIn { get; set; }
        public long ExpiresAt { get; set; }
    }
}

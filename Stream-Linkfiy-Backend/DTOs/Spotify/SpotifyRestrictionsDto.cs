namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    using System.Text.Json.Serialization;

    public record SpotifyRestrictionsDto
    {
        [JsonPropertyName("reason")]
        public required string Reason { get; init; }
    }
}

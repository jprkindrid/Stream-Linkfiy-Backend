using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    public record SpotifyTrackDto
    {
        [JsonPropertyName("album")]
        public required SpotifyTrackAlbumDto Album { get; init; }

        [JsonPropertyName("artists")]
        public required List<SpotifyArtistDto> Artists { get; init; }

        [JsonPropertyName("available_markets")]
        public required List<string> AvailableMarkets { get; init; }

        [JsonPropertyName("disc_number")]
        public required int DiscNumber { get; init; }

        [JsonPropertyName("duration_ms")]
        public required int DurationMs { get; init; }

        [JsonPropertyName("explicit")]
        public required bool Explicit { get; init; }

        [JsonPropertyName("external_ids")]
        public required SpotifyExternalIdsDto ExternalIds { get; init; }

        [JsonPropertyName("external_urls")]
        public required SpotifyExternalUrlsDto ExternalUrls { get; init; }

        [JsonPropertyName("href")]
        public required string Href { get; init; }

        [JsonPropertyName("id")]
        public required string Id { get; init; }

        [JsonPropertyName("is_local")]
        public required bool IsLocal { get; init; }

        [JsonPropertyName("is_playable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsPlayable { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("popularity")]
        public required int Popularity { get; init; }

        [JsonPropertyName("preview_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PreviewUrl { get; init; }

        [JsonPropertyName("track_number")]
        public required int TrackNumber { get; init; }

        [JsonPropertyName("type")]
        public required string Type { get; init; }

        [JsonPropertyName("uri")]
        public required string Uri { get; init; }

        [JsonPropertyName("restrictions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SpotifyRestrictionsDto? Restrictions { get; init; }
    }
}

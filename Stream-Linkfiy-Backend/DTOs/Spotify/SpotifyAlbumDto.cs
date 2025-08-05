using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify;
public record SpotifyAlbumDto
{
    [JsonPropertyName("album_type")]
    public required string AlbumType { get; init; }

    [JsonPropertyName("total_tracks")]
    public required int TotalTracks { get; init; }

    [JsonPropertyName("available_markets")]
    public required List<string> AvailableMarkets { get; init; }

    [JsonPropertyName("external_urls")]
    public required SpotifyExternalUrlsDto ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public required string Href { get; init; }

    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("images")]
    public required List<SpotifyImageDto> Images { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("release_date")]
    public required string ReleaseDate { get; init; }

    [JsonPropertyName("release_date_precision")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReleaseDatePrecision { get; init; }

    [JsonPropertyName("restrictions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SpotifyRestrictionsDto? Restrictions { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    [JsonPropertyName("artists")]
    public required List<SpotifyArtistDto> Artists { get; init; }

    [JsonPropertyName("is_playable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsPlayable { get; init; }
}


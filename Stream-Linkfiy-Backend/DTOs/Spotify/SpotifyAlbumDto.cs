using System.Diagnostics;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    public record SpotifyAlbumDto
    {
        [JsonPropertyName("album_type")]
        public string AlbumType { get; init; }

        [JsonPropertyName("total_tracks")]
        public int TotalTracks { get; init; }

        [JsonPropertyName("available_markets")]
        public List<string> AvailableMarkets { get; init; }

        [JsonPropertyName("external_urls")]
        public SpotifyExternalUrlsDto ExternalUrls { get; init; }

        [JsonPropertyName("href")]
        public string Href { get; init; }

        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("images")]
        public List<SpotifyImageDto> Images { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; init; }

        [JsonPropertyName("release_date_precision")]
        public string ReleaseDatePrecision { get; init; }

        [JsonPropertyName("restrictions")]
        public SpotifyRestrictionsDto? Restrictions { get; init; }

        [JsonPropertyName("type")]
        public string Type { get; init; }

        [JsonPropertyName("uri")]
        public string Uri { get; init; }

        [JsonPropertyName("artists")]
        public List<SpotifyArtistDto> Artists { get; init; }

        [JsonPropertyName("tracks")]
        public SpotifyAlbumTrackDto Tracks { get; init; }

        [JsonPropertyName("copyrights")]
        public List<SpotifyCopyrightDto> Copyrights { get; init; }

        [JsonPropertyName("external_ids")]
        public SpotifyExternalIdsDto ExternalIds { get; init; }

        [JsonPropertyName("genres")]
        public List<string> Genres { get; init; }

        [JsonPropertyName("label")]
        public string Label { get; init; }

        [JsonPropertyName("popularity")]
        public int Popularity { get; init; }
    }
}
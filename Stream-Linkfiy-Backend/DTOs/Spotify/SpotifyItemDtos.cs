using System.Text.Json.Serialization;

namespace Stream_Linkfiy_Backend.DTOs.Spotify
{
    // ===== Common Types =====
    public record SpotifyExternalUrlsDto(
        [property: JsonPropertyName("spotify")] string Spotify
    );

    public record SpotifyImageDto(
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("height")] int? Height,
        [property: JsonPropertyName("width")] int? Width
    );

    public record SpotifyRestrictionsDto(
        [property: JsonPropertyName("reason")] string Reason
    );

    public record SpotifyExternalIdsDto(
        [property: JsonPropertyName("isrc")] string? Isrc,
        [property: JsonPropertyName("ean")] string? Ean,
        [property: JsonPropertyName("upc")] string? Upc
    );

    public record SpotifyCopyrightDto(
        [property: JsonPropertyName("text")] string Text,
        [property: JsonPropertyName("type")] string Type
    );

    public record SpotifyFollowersDto(
        [property: JsonPropertyName("href")] string? Href,
        [property: JsonPropertyName("total")] int Total
    );

    // ===== Artist =====
    public record SpotifyArtistBaseDto(
        [property: JsonPropertyName("external_urls")] SpotifyExternalUrlsDto ExternalUrls,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("uri")] string Uri
    );

    public record SpotifyArtistFullDto(
        SpotifyExternalUrlsDto ExternalUrls,
        string Href,
        string Id,
        string Name,
        string Type,
        string Uri,
        [property: JsonPropertyName("followers")] SpotifyFollowersDto Followers,
        [property: JsonPropertyName("genres")] List<string> Genres,
        [property: JsonPropertyName("images")] List<SpotifyImageDto> Images,
        [property: JsonPropertyName("popularity")] int Popularity
    ) : SpotifyArtistBaseDto(ExternalUrls, Href, Id, Name, Type, Uri);

    // ===== Album =====
    public record SpotifyAlbumBaseDto(
        [property: JsonPropertyName("album_type")] string AlbumType,
        [property: JsonPropertyName("total_tracks")] int TotalTracks,
        [property: JsonPropertyName("available_markets")] List<string> AvailableMarkets,
        [property: JsonPropertyName("external_urls")] SpotifyExternalUrlsDto ExternalUrls,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("images")] List<SpotifyImageDto> Images,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("release_date")] string ReleaseDate,
        [property: JsonPropertyName("release_date_precision")] string ReleaseDatePrecision,
        [property: JsonPropertyName("restrictions")] SpotifyRestrictionsDto? Restrictions,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("uri")] string Uri,
        [property: JsonPropertyName("artists")] List<SpotifyArtistBaseDto> Artists
    );

    public record SpotifyAlbumFullDto(
        string AlbumType,
        int TotalTracks,
        List<string> AvailableMarkets,
        SpotifyExternalUrlsDto ExternalUrls,
        string Href,
        string Id,
        List<SpotifyImageDto> Images,
        string Name,
        string ReleaseDate,
        string ReleaseDatePrecision,
        SpotifyRestrictionsDto? Restrictions,
        string Type,
        string Uri,
        List<SpotifyArtistBaseDto> Artists,
        [property: JsonPropertyName("tracks")] SpotifyPagingDto<SpotifyTrackSimplifiedDto> Tracks,
        [property: JsonPropertyName("copyrights")] List<SpotifyCopyrightDto> Copyrights,
        [property: JsonPropertyName("external_ids")] SpotifyExternalIdsDto ExternalIds,
        [property: JsonPropertyName("genres")] List<string> Genres,
        [property: JsonPropertyName("label")] string Label,
        [property: JsonPropertyName("popularity")] int Popularity
    ) : SpotifyAlbumBaseDto(AlbumType, TotalTracks, AvailableMarkets, ExternalUrls, Href, Id, Images, Name, ReleaseDate, ReleaseDatePrecision, Restrictions, Type, Uri, Artists);

    // ===== Track =====
    public record SpotifyTrackSimplifiedDto(
        [property: JsonPropertyName("artists")] List<SpotifyArtistBaseDto> Artists,
        [property: JsonPropertyName("available_markets")] List<string> AvailableMarkets,
        [property: JsonPropertyName("disc_number")] int DiscNumber,
        [property: JsonPropertyName("duration_ms")] int DurationMs,
        [property: JsonPropertyName("explicit")] bool Explicit,
        [property: JsonPropertyName("external_urls")] SpotifyExternalUrlsDto ExternalUrls,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("is_playable")] bool IsPlayable,
        [property: JsonPropertyName("linked_from")] object LinkedFrom,
        [property: JsonPropertyName("restrictions")] SpotifyRestrictionsDto? Restrictions,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("preview_url")] string? PreviewUrl,
        [property: JsonPropertyName("track_number")] int TrackNumber,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("uri")] string Uri,
        [property: JsonPropertyName("is_local")] bool IsLocal
    );

    public record SpotifyTrackFullDto(
        SpotifyAlbumBaseDto Album,
        List<SpotifyArtistBaseDto> Artists,
        List<string> AvailableMarkets,
        int DiscNumber,
        int DurationMs,
        bool Explicit,
        [property: JsonPropertyName("external_ids")] SpotifyExternalIdsDto ExternalIds,
        SpotifyExternalUrlsDto ExternalUrls,
        string Href,
        string Id,
        bool IsPlayable,
        object LinkedFrom,
        SpotifyRestrictionsDto? Restrictions,
        string Name,
        int Popularity,
        string? PreviewUrl,
        int TrackNumber,
        string Type,
        string Uri,
        bool IsLocal
    ) : SpotifyTrackSimplifiedDto(Artists, AvailableMarkets, DiscNumber, DurationMs, Explicit, ExternalUrls, Href, Id, IsPlayable, LinkedFrom, Restrictions, Name, PreviewUrl, TrackNumber, Type, Uri, IsLocal);

    // ===== Paging Wrapper =====
    public record SpotifyPagingDto<T>(
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("limit")] int Limit,
        [property: JsonPropertyName("next")] string? Next,
        [property: JsonPropertyName("offset")] int Offset,
        [property: JsonPropertyName("previous")] string? Previous,
        [property: JsonPropertyName("total")] int Total,
        [property: JsonPropertyName("items")] List<T> Items
    );

    // ===== Search Response =====
    public record SpotifySearchResponseDto(
        [property: JsonPropertyName("tracks")] SpotifyPagingDto<SpotifyTrackFullDto>? Tracks,
        [property: JsonPropertyName("artists")] SpotifyPagingDto<SpotifyArtistFullDto>? Artists,
        [property: JsonPropertyName("albums")] SpotifyPagingDto<SpotifyAlbumBaseDto>? Albums
    );
}
using System.Text.Json.Serialization;

namespace Stream_Linkify_Backend.DTOs.Apple
{
    public record AppleAlbumResponseDto(
        [property: JsonPropertyName("data")] List<AppleAlbumDataDto> Data
    );

    public record AppleAlbumDataDto(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("attributes")] AppleAlbumAttributes Attributes,
        [property: JsonPropertyName("relationships")] AppleAlbumRelationships Relationships
    );

    public record AppleAlbumAttributes(
        [property: JsonPropertyName("copyright")] string Copyright,
        [property: JsonPropertyName("genreNames")] List<string> GenreNames,
        [property: JsonPropertyName("releaseDate")] string ReleaseDate,
        [property: JsonPropertyName("upc")] string Upc,
        [property: JsonPropertyName("isMasteredForItunes")] bool IsMasteredForItunes,
        [property: JsonPropertyName("artwork")] AppleArtwork Artwork,
        [property: JsonPropertyName("playParams")] ApplePlayParams PlayParams,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("recordLabel")] string RecordLabel,
        [property: JsonPropertyName("isCompilation")] bool IsCompilation,
        [property: JsonPropertyName("trackCount")] int TrackCount,
        [property: JsonPropertyName("isSingle")] bool IsSingle,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("contentRating")] string? ContentRating,
        [property: JsonPropertyName("artistName")] string ArtistName,
        [property: JsonPropertyName("editorialNotes")] AppleEditorialNotes? EditorialNotes,
        [property: JsonPropertyName("isComplete")] bool IsComplete
    );

    public record AppleEditorialNotes(
        [property: JsonPropertyName("standard")] string Standard,
        [property: JsonPropertyName("short")] string Short
    );

    public record AppleAlbumRelationships(
        [property: JsonPropertyName("artists")] AppleRelationship Artists,
        [property: JsonPropertyName("tracks")] AppleAlbumTracksRelationship Tracks
    );

    public record AppleAlbumTracksRelationship(
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("data")] List<AppleAlbumTrackData> Data
    );

    public record AppleAlbumTrackData(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("attributes")] AppleSongAttributes Attributes
    );
}
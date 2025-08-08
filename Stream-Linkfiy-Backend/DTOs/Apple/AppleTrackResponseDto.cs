using System.Text.Json.Serialization;

namespace Stream_Linkify_Backend.DTOs.Apple
{
    public record AppleSongResponse(
        [property: JsonPropertyName("data")] List<AppleSongDataDto> Data,
        [property: JsonPropertyName("meta")] AppleMeta? Meta
    );

    public record AppleSongDataDto(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("attributes")] AppleSongAttributes Attributes,
        [property: JsonPropertyName("relationships")] AppleSongRelationships Relationships
    );

    public record AppleSongAttributes(
        [property: JsonPropertyName("albumName")] string AlbumName,
        [property: JsonPropertyName("genreNames")] List<string> GenreNames,
        [property: JsonPropertyName("trackNumber")] int TrackNumber,
        [property: JsonPropertyName("durationInMillis")] int DurationInMillis,
        [property: JsonPropertyName("releaseDate")] string ReleaseDate,
        [property: JsonPropertyName("isrc")] string Isrc,
        [property: JsonPropertyName("artwork")] AppleArtwork Artwork,
        [property: JsonPropertyName("composerName")] string? ComposerName,
        [property: JsonPropertyName("playParams")] ApplePlayParams PlayParams,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("discNumber")] int DiscNumber,
        [property: JsonPropertyName("hasLyrics")] bool HasLyrics,
        [property: JsonPropertyName("isAppleDigitalMaster")] bool IsAppleDigitalMaster,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("previews")] List<ApplePreview> Previews,
        [property: JsonPropertyName("artistName")] string ArtistName
    );

    public record AppleArtwork(
        [property: JsonPropertyName("width")] int Width,
        [property: JsonPropertyName("height")] int Height,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("bgColor")] string BgColor,
        [property: JsonPropertyName("textColor1")] string TextColor1,
        [property: JsonPropertyName("textColor2")] string TextColor2,
        [property: JsonPropertyName("textColor3")] string TextColor3,
        [property: JsonPropertyName("textColor4")] string TextColor4
    );

    public record ApplePlayParams(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("kind")] string Kind
    );

    public record ApplePreview(
        [property: JsonPropertyName("url")] string Url
    );

    public record AppleSongRelationships(
        [property: JsonPropertyName("artists")] AppleRelationship Artists,
        [property: JsonPropertyName("albums")] AppleRelationship Albums
    );

    public record AppleRelationship(
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("data")] List<AppleRelationshipData> Data
    );

    public record AppleRelationshipData(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("href")] string Href
    );
    public record AppleMeta(
    [property: JsonPropertyName("filters")] AppleMetaFilters Filters
    );

    public record AppleMetaFilters(
        [property: JsonPropertyName("isrc")] Dictionary<string, List<AppleMetaSong>> Isrc
    );

    public record AppleMetaSong(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("href")] string Href
    );

}
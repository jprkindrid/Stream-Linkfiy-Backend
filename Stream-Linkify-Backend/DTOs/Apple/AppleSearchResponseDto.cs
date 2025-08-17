using Stream_Linkify_Backend.DTOs.Apple;
using System.Text.Json.Serialization;

public record AppleSearchResponseDto(
    [property: JsonPropertyName("results")] AppleSearchResults Results,
    [property: JsonPropertyName("meta")] AppleSearchMeta? Meta
);

public record AppleSearchResults(
    [property: JsonPropertyName("artists")] AppleSearchResult<AppleArtistData>? Artists,
    [property: JsonPropertyName("songs")] AppleSearchResult<AppleSongDataDto>? Songs,
    [property: JsonPropertyName("albums")] AppleSearchResult<AppleAlbumDataDto>? Albums
);

public record AppleSearchResult<T>(
    [property: JsonPropertyName("href")] string Href,
    [property: JsonPropertyName("next")] string? Next,
    [property: JsonPropertyName("data")] List<T> Data
);

public record AppleArtistData(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("href")] string Href,
    [property: JsonPropertyName("attributes")] AppleArtistAttributes Attributes
);

public record AppleArtistAttributes(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("genreNames")] List<string> GenreNames,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("artwork")] AppleArtwork Artwork
);

public record AppleSearchMeta(
    [property: JsonPropertyName("results")] AppleSearchMetaResults Results
);

public record AppleSearchMetaResults(
    [property: JsonPropertyName("order")] List<string> Order,
    [property: JsonPropertyName("rawOrder")] List<string> RawOrder
);
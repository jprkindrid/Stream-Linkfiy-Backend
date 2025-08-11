using System.Text.Json;
using System.Text.Json.Serialization;

public record TidalArtistsResponseDto(
    [property: JsonPropertyName("data")] List<TidalArtistRef> Data,
    [property: JsonPropertyName("links")] TidalLinks Links,
    [property: JsonPropertyName("included")] List<TidalIncludedArtist> Included
);

public record TidalArtistRef(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type
);

public record TidalIncludedArtist(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("attributes")] TidalArtistAttributes Attributes,
    [property: JsonPropertyName("relationships")] TidalArtistRelationships Relationships
);

public record TidalArtistAttributes(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("popularity")] double Popularity,
    [property: JsonPropertyName("externalLinks")] List<TidalExternalLink> ExternalLinks
);

public record TidalArtistRelationships(
    [property: JsonPropertyName("similarArtists")] TidalLinksContainer SimilarArtists,
    [property: JsonPropertyName("albums")] TidalLinksContainer Albums,
    [property: JsonPropertyName("roles")] TidalLinksContainer Roles,
    [property: JsonPropertyName("videos")] TidalLinksContainer Videos,
    [property: JsonPropertyName("owners")] TidalLinksContainer Owners,
    [property: JsonPropertyName("biography")] TidalLinksContainer Biography,
    [property: JsonPropertyName("profileArt")] TidalLinksContainer ProfileArt,
    [property: JsonPropertyName("trackProviders")] TidalLinksContainer TrackProviders,
    [property: JsonPropertyName("tracks")] TidalLinksContainer Tracks,
    [property: JsonPropertyName("radio")] TidalLinksContainer Radio
);

public record TidalLinksContainer(
    [property: JsonPropertyName("links")] TidalLinks Links
);

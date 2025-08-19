using System.Text.Json.Serialization;

namespace Stream_Linkify_Backend.DTOs.Deezer
{
    namespace Stream_Linkify_Backend.DTOs.Deezer
    {
        // Track Search
        public record DeezerTrackSearchResponse(
            [property: JsonPropertyName("data")] List<DeezerTrackDto> Data,
            [property: JsonPropertyName("total")] int Total
        );
        // Album Search
        public record DeezerAlbumSearchResponse(
            [property: JsonPropertyName("data")] List<DeezerAlbumDto> Data,
            [property: JsonPropertyName("total")] int Total
        );
        // Track Full
        public record DeezerTrackFullDto(
            [property: JsonPropertyName("id")] long Id,
            [property: JsonPropertyName("readable")] bool Readable,
            [property: JsonPropertyName("title")] string Title,
            [property: JsonPropertyName("title_short")] string TitleShort,
            [property: JsonPropertyName("title_version")] string? TitleVersion,
            [property: JsonPropertyName("isrc")] string? Isrc,
            [property: JsonPropertyName("link")] string Link,
            [property: JsonPropertyName("duration")] int Duration,
            [property: JsonPropertyName("rank")] int Rank,
            [property: JsonPropertyName("release_date")] string? ReleaseDate,
            [property: JsonPropertyName("explicit_lyrics")] bool ExplicitLyrics,
            [property: JsonPropertyName("preview")] string Preview,
            [property: JsonPropertyName("md5_image")] string Md5Image,
            [property: JsonPropertyName("artist")] DeezerArtistDto Artist,
            [property: JsonPropertyName("album")] DeezerAlbumDto Album,
            [property: JsonPropertyName("contributors")] List<DeezerArtistDto>? Contributors
        );
        // Album Full
        public record DeezerAlbumFullDto(
            [property: JsonPropertyName("id")] long Id,
            [property: JsonPropertyName("title")] string Title,
            [property: JsonPropertyName("upc")] string? Upc,
            [property: JsonPropertyName("link")] string Link,
            [property: JsonPropertyName("cover")] string Cover,
            [property: JsonPropertyName("cover_small")] string CoverSmall,
            [property: JsonPropertyName("cover_medium")] string CoverMedium,
            [property: JsonPropertyName("cover_big")] string CoverBig,
            [property: JsonPropertyName("cover_xl")] string CoverXl,
            [property: JsonPropertyName("md5_image")] string Md5Image,
            [property: JsonPropertyName("label")] string? Label,
            [property: JsonPropertyName("nb_tracks")] int NbTracks,
            [property: JsonPropertyName("duration")] int Duration,
            [property: JsonPropertyName("release_date")] string ReleaseDate,
            [property: JsonPropertyName("record_type")] string RecordType,
            [property: JsonPropertyName("explicit_lyrics")] bool ExplicitLyrics,
            [property: JsonPropertyName("artist")] DeezerArtistDto Artist,
            [property: JsonPropertyName("contributors")] List<DeezerArtistDto>? Contributors,
            [property: JsonPropertyName("tracks")] DeezerAlbumTracks Tracks
        );
        public record DeezerAlbumTracks(
            [property: JsonPropertyName("data")] List<DeezerTrackDto> Data
        );
        // Shared DTOs
        public record DeezerTrackDto(
            [property: JsonPropertyName("id")] long Id,
            [property: JsonPropertyName("title")] string Title,
            [property: JsonPropertyName("title_short")] string TitleShort,
            [property: JsonPropertyName("title_version")] string? TitleVersion,
            [property: JsonPropertyName("link")] string Link,
            [property: JsonPropertyName("duration")] int Duration,
            [property: JsonPropertyName("rank")] int Rank,
            [property: JsonPropertyName("explicit_lyrics")] bool ExplicitLyrics,
            [property: JsonPropertyName("preview")] string Preview,
            [property: JsonPropertyName("md5_image")] string Md5Image,
            [property: JsonPropertyName("artist")] DeezerArtistDto Artist,
            [property: JsonPropertyName("album")] DeezerAlbumDto Album
        );
        public record DeezerAlbumDto(
            [property: JsonPropertyName("id")] long Id,
            [property: JsonPropertyName("title")] string Title,
            [property: JsonPropertyName("link")] string Link,
            [property: JsonPropertyName("cover")] string Cover,
            [property: JsonPropertyName("cover_small")] string CoverSmall,
            [property: JsonPropertyName("cover_medium")] string CoverMedium,
            [property: JsonPropertyName("cover_big")] string CoverBig,
            [property: JsonPropertyName("cover_xl")] string CoverXl,
            [property: JsonPropertyName("md5_image")] string Md5Image,
            [property: JsonPropertyName("tracklist")] string Tracklist,
            [property: JsonPropertyName("type")] string Type,
            [property: JsonPropertyName("artist")] DeezerArtistDto? Artist = null
        );
        public record DeezerArtistDto(
            [property: JsonPropertyName("id")] long Id,
            [property: JsonPropertyName("name")] string Name,
            [property: JsonPropertyName("link")] string? Link,
            [property: JsonPropertyName("picture")] string? Picture,
            [property: JsonPropertyName("picture_small")] string? PictureSmall,
            [property: JsonPropertyName("picture_medium")] string? PictureMedium,
            [property: JsonPropertyName("picture_big")] string? PictureBig,
            [property: JsonPropertyName("picture_xl")] string? PictureXl,
            [property: JsonPropertyName("tracklist")] string? Tracklist,
            [property: JsonPropertyName("type")] string? Type
        );
    }
}

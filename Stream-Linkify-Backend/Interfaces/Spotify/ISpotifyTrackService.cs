using Stream_Linkify_Backend.DTOs.Spotify;

namespace Stream_Linkify_Backend.Interfaces.Spotify
{
    public interface ISpotifyTrackService
    {
        Task<SpotifyTrackFullDto?> GetByUrlAsync(string url);
        Task<SpotifySearchResponseDto?> GetByIsrcAsync(string isrc);
    }
}

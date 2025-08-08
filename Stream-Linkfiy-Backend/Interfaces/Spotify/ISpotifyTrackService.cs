using Stream_Linkfiy_Backend.DTOs.Spotify;

namespace Stream_Linkfiy_Backend.Interfaces.Spotify
{
    public interface ISpotifyTrackService
    {
        Task<SpotifyTrackFullDto?> GetByUrlAsync(string url);
        Task<SpotifySearchResponseDto?> GetByIsrcAsync(string isrc);
    }
}

using Stream_Linkify_Backend.DTOs.Tidal;

namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalTrackService
    {
        Task<TidalTrackResponseDto?> GetTrackByUrlAsync(string url);
        Task<TidalSearchResponseDto?> GetTrackByNameAsync(string trackName, string artistName, string? albumName);
    }
}

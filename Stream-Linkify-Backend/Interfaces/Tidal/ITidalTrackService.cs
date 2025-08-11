using Stream_Linkify_Backend.DTOs.Tidal;

namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalTrackService
    {
        Task<TidalTrackResponseDto?> GetTrackByUrlAsync(string url);
        Task<string?> GetTrackUrlByNameAsync(string trackName, string artistName, string isrc);
    }
}

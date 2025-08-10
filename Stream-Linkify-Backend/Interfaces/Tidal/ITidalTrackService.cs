using Stream_Linkify_Backend.DTOs.Apple;

namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalTrackService
    {
        Task<TidalSongDataDto?> GetTrackByUrlAsync(string url);
        Task<TidalSongDataDto?> GetTrackByNameAsync(string trackName, string artistName, string? albumName);
    }
}

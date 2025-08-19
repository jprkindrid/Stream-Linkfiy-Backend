using Stream_Linkify_Backend.DTOs.Deezer.Stream_Linkify_Backend.DTOs.Deezer;

namespace Stream_Linkify_Backend.Interfaces.Deezer
{
    public interface IDeezerTrackService
    {
        Task<DeezerTrackFullDto?> GetByUrlAsync(string deezerUrl);
        Task<string?> GetByNameAsync(string trackName, string artistName);
    }
}

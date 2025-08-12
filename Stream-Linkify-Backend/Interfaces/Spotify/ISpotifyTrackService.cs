using Stream_Linkify_Backend.DTOs.Spotify;

namespace Stream_Linkify_Backend.Interfaces.Spotify
{
    public interface ISpotifyTrackService
    {
        Task<SpotifyTrackFullDto?> GetByUrlAsync(string url);
        Task<(string? url, string? albumName, List<string> artistNames)> GetByIsrcAsync(string isrc);
    }
}

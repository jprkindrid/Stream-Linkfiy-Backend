using Stream_Linkify_Backend.DTOs.Apple;

namespace Stream_Linkify_Backend.Interfaces.Apple
{
    public interface IAppleTrackService
    {
        Task<AppleSongDataDto?> GetTrackByUrlAsync(string url);
        Task<string?> GetTrackUrlByNameAsync(string isrc, string trackName, string artistName);
    }
}

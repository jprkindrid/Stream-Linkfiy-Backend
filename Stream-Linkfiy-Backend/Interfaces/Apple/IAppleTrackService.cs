using Stream_Linkify_Backend.DTOs.Apple;

namespace Stream_Linkify_Backend.Interfaces.Apple
{
    public interface IAppleTrackService
    {
        Task<AppleSongDataDto?> GetTrackByUrlAsync(string url);
        Task<AppleSongDataDto?> GetTrackByIsrcAsync(string isrc);
    }
}

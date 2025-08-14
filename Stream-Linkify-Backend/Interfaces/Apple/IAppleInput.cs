using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Interfaces.Apple
{
    public interface IAppleInput
    {
        Task<TrackReturnDto> GetTrackUrlsAsync(string appleUrl);
        Task<AlbumReturnDto> GetAlbumUrlsAsync(string appleUrl);
    }
}

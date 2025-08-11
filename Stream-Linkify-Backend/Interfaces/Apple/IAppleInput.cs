using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Interfaces.Apple
{
    public interface IAppleInput
    {
        Task<TrackReturnDto> GetUrlsAsync(string appleUrl);
    }
}

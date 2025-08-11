using Stream_Linkify_Backend.DTOs;

namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalInput
    {
        Task<TrackReturnDto> GetUrlsAsync(string tidalUrl);
    }
}

using Stream_Linkify_Backend.DTOs;

namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalInput
    {
        Task<TrackReturnDto> GetTrackUrlsAsync(string tidalUrl);
        Task<TrackReturnDto> GetAlbumUrlsAsync(string tidalUrl);
    }
}

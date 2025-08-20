using Stream_Linkify_Backend.DTOs;

namespace Stream_Linkify_Backend.Interfaces.Deezer
{
    public interface IDeezerInput
    {
        Task<TrackReturnDto> GetTrackUrlsAsync(string deezerUrl);
        Task<AlbumReturnDto> GetAlbumUrlsAsync(string deezerUrl);
    }
}

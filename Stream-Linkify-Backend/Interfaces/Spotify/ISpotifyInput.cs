using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Interfaces.Spotify
{
    public interface ISpotifyInput
    {
        Task<TrackReturnDto> GetTrackUrlsAsync(string spotifyUrl);
        Task<AlbumReturnDto> GetAlbumUrlsAsync(string spotifyUrl);
    }
}

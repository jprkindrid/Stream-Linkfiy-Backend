using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Interfaces.Spotify
{
    public interface ISpotifyInput
    {
        Task<TrackModel> getUrlsAsync(string spotifyUrl);
    }
}

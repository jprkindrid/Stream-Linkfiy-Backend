using Stream_Linkfiy_Backend.DTOs.Spotify;

namespace Stream_Linkfiy_Backend.Interfaces
{
    public interface ISpotifyTrackService
    {
        Task<SpotifyTrackDto?> GetTrackAsync(string url);
        string ExtractTrackId(string spotifyUrl);
    }
}

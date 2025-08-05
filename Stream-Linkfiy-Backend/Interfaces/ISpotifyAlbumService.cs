using Stream_Linkfiy_Backend.DTOs.Spotify;

namespace Stream_Linkfiy_Backend.Interfaces
{
    public interface ISpotifyAlbumService
    {
        Task<SpotifyAlbumDto> GetAlbumAsync(string url);
        string ExtractAlbumId(string spotifyUrl);
    }
}

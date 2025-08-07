using Stream_Linkfiy_Backend.DTOs.Spotify;

namespace Stream_Linkfiy_Backend.Interfaces
{
    public interface ISpotifyAlbumService
    {
        Task<SpotifyAlbumFullDto> GetByUrlAsync(string url);
    }
}

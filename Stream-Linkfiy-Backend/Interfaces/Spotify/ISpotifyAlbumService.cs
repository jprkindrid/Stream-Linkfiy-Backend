using Stream_Linkfiy_Backend.DTOs.Spotify;

namespace Stream_Linkfiy_Backend.Interfaces.Spotify
{
    public interface ISpotifyAlbumService
    {
        Task<SpotifyAlbumFullDto> GetByUrlAsync(string url);
    }
}

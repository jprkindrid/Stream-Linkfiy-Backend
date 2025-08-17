using Stream_Linkify_Backend.DTOs.Spotify;
using Stream_Linkify_Backend.DTOs.Tidal;

namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalAlbumService
    {
        Task<TidalAlbumResponseDto?> GetByUrlAsync(string url);
        Task<string?> GetUrlByNameAsync(string albumName, string firstArtistName, string upc);
    }
}

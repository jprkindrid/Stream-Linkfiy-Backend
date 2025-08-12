using Stream_Linkify_Backend.DTOs.Spotify;
using Stream_Linkify_Backend.DTOs.Tidal;

namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalAlbumService
    {
        Task<TidalAlbumData?> GetByUrlAsync(string url);
        Task<string?> GetUrlByUpcAsync(string upc);
    }
}

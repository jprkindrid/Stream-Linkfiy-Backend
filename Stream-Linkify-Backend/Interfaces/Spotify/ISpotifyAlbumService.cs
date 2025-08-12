using Stream_Linkify_Backend.DTOs.Spotify;

namespace Stream_Linkify_Backend.Interfaces.Spotify
{
    public interface ISpotifyAlbumService
    {
        Task<(string? UPC, string? albumName, List<string>? artistNames)?> GetByUrlAsync(string url);
        Task<string?> GetUrlByUpcAsync(string upc);
    }
}

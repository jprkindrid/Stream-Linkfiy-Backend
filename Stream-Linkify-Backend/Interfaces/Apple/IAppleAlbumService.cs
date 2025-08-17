using Stream_Linkify_Backend.DTOs.Apple;

namespace Stream_Linkify_Backend.Interfaces.Apple
{
    public interface IAppleAlbumService
    {
        Task<AppleAlbumDataDto?> GetByUrlAsync(string url);
        Task<string?> GetUrlByNameAsync(string upc, string albumName, string artistName);
    }
}

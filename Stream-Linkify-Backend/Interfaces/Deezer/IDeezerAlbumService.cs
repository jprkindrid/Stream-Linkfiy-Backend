using Stream_Linkify_Backend.DTOs.Deezer.Stream_Linkify_Backend.DTOs.Deezer;

namespace Stream_Linkify_Backend.Interfaces.Deezer
{
    public interface IDeezerAlbumService
    {
        Task<DeezerAlbumFullDto?> GetByUrlAsync(string deezerUrl);
        Task<string?> GetByNameAsync(string albumName, string artistName);
    }
}

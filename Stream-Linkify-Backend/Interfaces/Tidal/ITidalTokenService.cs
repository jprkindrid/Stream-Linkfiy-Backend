using Stream_Linkify_Backend.DTOs.Tidal;

namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalTokenService
    {
        Task<TidalAccessTokenDto> GetValidTokenAsync();
    }
}

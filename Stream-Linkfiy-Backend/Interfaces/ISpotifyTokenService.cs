using Stream_Linkfiy_Backend.DTOs.Spotify;

namespace Stream_Linkfiy_Backend.Interfaces
{
    public interface ISpotifyTokenService
    {
        Task<SpotifyAccessTokenDto?> GetValidTokenAsync();
        Task<SpotifyAccessTokenDto> RefreshTokenAsync();
        bool IsValidToken();
    }
}

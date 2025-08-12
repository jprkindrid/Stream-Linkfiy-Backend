using Stream_Linkify_Backend.DTOs.Spotify;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Spotify;

namespace Stream_Linkify_Backend.Services.Spotify
{
    public class SpotifyAlbumService : ISpotifyAlbumService
    {
        private const string spotifyApiUrl = "https://api.spotify.com/v1";
        private readonly ISpotifyApiClient spotifyApiClient;

        public SpotifyAlbumService(
           ISpotifyApiClient spotifyApiClient)
        {
            this.spotifyApiClient = spotifyApiClient;
        }
        public async Task<SpotifyAlbumFullDto?> GetByUrlAsync(string spotifyUrl)
        {
            var albumID = SpotifyUrlHelper.ExtractSpotifyId(spotifyUrl, "album");
            var reqUrl = $"{spotifyApiUrl}/albums/{albumID}";

            var result = await spotifyApiClient.SendSpotifyRequestAsync<SpotifyAlbumFullDto>(reqUrl);

            return result;
        }

        public async Task<string?> GetUrlByUpcAsync(string upc)
        {
            var query = $"upc:{upc}";
            var reqUrl = $"{spotifyApiUrl}/search/?q={Uri.EscapeDataString(query)}&type=album";

            SpotifySearchResponseDto? result = await spotifyApiClient.SendSpotifyRequestAsync<SpotifySearchResponseDto>(reqUrl);

            return result?.Albums?.Items?.FirstOrDefault()?.ExternalUrls.Spotify;
        }
    }
}

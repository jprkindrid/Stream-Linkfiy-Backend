using Stream_Linkify_Backend.DTOs.Spotify;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Spotify;

namespace Stream_Linkify_Backend.Services.Spotify
{
    public class SpotifyAlbumService : ISpotifyAlbumService
    {
        private const string spotifyApiUrl = "https://api.spotify.com/v1";
        private readonly ISpotifyApiClient spotifyApiClient;
        private readonly ILogger<SpotifyAlbumService> logger;

        public SpotifyAlbumService(
           ISpotifyApiClient spotifyApiClient,
            ILogger<SpotifyAlbumService> logger
            )
        {
            this.spotifyApiClient = spotifyApiClient;
            this.logger = logger;
        }
        public async Task<SpotifyAlbumFullDto?> GetByUrlAsync(string spotifyUrl)
        {
            var albumID = SpotifyUrlHelper.ExtractSpotifyId(spotifyUrl, "album");
            var reqUrl = $"{spotifyApiUrl}/albums/{albumID}";

            SpotifyAlbumFullDto? result = await spotifyApiClient.SendSpotifyRequestAsync<SpotifyAlbumFullDto>(reqUrl);
            if (result == null)
            {
                logger.LogWarning("Could not get Spotify album by url");
                return null;
            }

            return result;
        }

        public async Task<(string? url, List<string>? artistNames)> GetByUpcAsync(string upc)
        {
            var query = $"upc:{upc}";
            var reqUrl = $"{spotifyApiUrl}/search/?q={Uri.EscapeDataString(query)}&type=album";

            SpotifySearchResponseDto? result = await spotifyApiClient.SendSpotifyRequestAsync<SpotifySearchResponseDto>(reqUrl);

            if (result == null)
            {
                logger.LogWarning("Could not find Spotify Album with UPC: {upc}", upc);
                return (null, []);
            }

            var url = result?.Albums?.Items?.FirstOrDefault()?.ExternalUrls.Spotify;
            var artistNames = result!.Albums?.Items?.FirstOrDefault()?.Artists.Select(a => a.Name).ToList();
            if (artistNames == null)
                return (url, []);

            return (url, artistNames);
        }

    }
}

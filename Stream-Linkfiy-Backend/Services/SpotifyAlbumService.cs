using Stream_Linkfiy_Backend.DTOs.Spotify;
using Stream_Linkfiy_Backend.Helpers;
using Stream_Linkfiy_Backend.Interfaces.Spotify;

namespace Stream_Linkfiy_Backend.Services
{
    public class SpotifyAlbumService : ISpotifyAlbumService
    {
        private const string spotifyApiAlbumUrl = "https://api.spotify.com/v1/albums";
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<SpotifyTrackService> logger;
        private readonly ISpotifyTokenService spotifyTokenService;

        public SpotifyAlbumService(IHttpClientFactory httpClientFactory, ILogger<SpotifyTrackService> logger, ISpotifyTokenService spotifyTokenService)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.spotifyTokenService = spotifyTokenService;
        }
        public async Task<SpotifyAlbumFullDto> GetByUrlAsync(string spotifyUrl)
        {
            await SpotifyConcurrency.GlobalSemaphore.WaitAsync();
            try
            {
                var aToken = await spotifyTokenService.GetValidTokenAsync()
                    ?? throw new InvalidOperationException("Error getting spotify access token");

                var albumID = SpotifyUrlHelper.ExtractSpotifyId(spotifyUrl, "album");
                var reqUrl = $"{spotifyApiAlbumUrl}/{albumID}";

                var req = new HttpRequestMessage(HttpMethod.Get, reqUrl);
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", aToken.AccessToken);

                var client = httpClientFactory.CreateClient();
                string reqMessage = $"Making a spotify api request at the path '{reqUrl}'";
                logger.LogInformation(reqMessage);
                var resp = await client.SendAsync(req);
                resp.EnsureSuccessStatusCode();

                SpotifyAlbumFullDto album = await resp.Content.ReadFromJsonAsync < SpotifyAlbumFullDto>()
                    ?? throw new Exception("Error deseralizing spotify album response");

                return album;
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "error getting spotify album");
                throw new Exception("Error getting spotify album", ex);
            }
            finally
            {
                SpotifyConcurrency.GlobalSemaphore.Release();
            }

        }

    }
}

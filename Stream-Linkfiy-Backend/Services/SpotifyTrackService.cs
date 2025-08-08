using Stream_Linkfiy_Backend.DTOs.Spotify;
using Stream_Linkfiy_Backend.Helpers;
using Stream_Linkfiy_Backend.Interfaces.Spotify;

namespace Stream_Linkfiy_Backend.Services
{
    public class SpotifyTrackService : ISpotifyTrackService
    {
        private const string spotifyApiUrl = "https://api.spotify.com/v1";
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<SpotifyTrackService> logger;
        private readonly ISpotifyTokenService spotifyTokenService;

        public SpotifyTrackService(IHttpClientFactory httpClientFactory, ILogger<SpotifyTrackService> logger, ISpotifyTokenService spotifyTokenService)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.spotifyTokenService = spotifyTokenService;
        }
        public async Task<SpotifyTrackFullDto?> GetByUrlAsync(string spotifyUrl)
        {
            await SpotifyConcurrency.GlobalSemaphore.WaitAsync();
            try
            {
                var aToken = await spotifyTokenService.GetValidTokenAsync()
                    ?? throw new InvalidOperationException("Error getting spotify access token");

                var trackID = SpotifyUrlHelper.ExtractSpotifyId(spotifyUrl, "track");
                var reqUrl = $"{spotifyApiUrl}/tracks/{trackID}";

                var req = new HttpRequestMessage(HttpMethod.Get, reqUrl);
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", aToken.AccessToken);

                var client = httpClientFactory.CreateClient();
                string reqMessage = $"Making a spotify api request at the path '{reqUrl}'";
                logger.LogInformation(reqMessage);
                var resp = await client.SendAsync(req);
                resp.EnsureSuccessStatusCode();

                SpotifyTrackFullDto track = await resp.Content.ReadFromJsonAsync<SpotifyTrackFullDto>()
                    ?? throw new Exception("Error deseralizing spotify track response");

                return track;
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "error getting spotify track");
                return null;
            }
            finally
            {
                SpotifyConcurrency.GlobalSemaphore.Release();
            }

        }

        public async Task<SpotifySearchResponseDto?> GetByIsrcAsync(string isrc)
        {
            await SpotifyConcurrency.GlobalSemaphore.WaitAsync();
            try
            {
                var aToken = await spotifyTokenService.GetValidTokenAsync()
                        ?? throw new InvalidOperationException("Error getting spotify access token");

                var query = $"isrc:{isrc}";
                var reqUrl = $"{spotifyApiUrl}/search/?q={Uri.EscapeDataString(query)}&type=track";

                var req = new HttpRequestMessage(HttpMethod.Get, reqUrl);
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", aToken.AccessToken);

                var client = httpClientFactory.CreateClient();
                string reqMessage = $"Making a spotify api request at the path '{reqUrl}'";
                logger.LogInformation(reqMessage);
                var resp = await client.SendAsync(req);
                resp.EnsureSuccessStatusCode();

                SpotifySearchResponseDto track = await resp.Content.ReadFromJsonAsync<SpotifySearchResponseDto>()
                    ?? throw new Exception("Error deseralizing spotify track response");

                return track;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error getting spotify track by isrc");
                return null;
            }
            finally
            {
                SpotifyConcurrency.GlobalSemaphore.Release();
            }
        }

    }
}

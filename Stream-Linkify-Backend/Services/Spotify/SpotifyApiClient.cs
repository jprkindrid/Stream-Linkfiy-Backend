using Stream_Linkify_Backend.Interfaces.Spotify;

namespace Stream_Linkify_Backend.Services.Spotify
{
    public class SpotifyApiClient(
        IHttpClientFactory httpClientFactory,
        ILogger<SpotifyApiClient> logger,
        ISpotifyTokenService spotifyTokenService) : ISpotifyApiClient
    {
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly ILogger<SpotifyApiClient> logger = logger;
        private readonly ISpotifyTokenService spotifyTokenService = spotifyTokenService;
        private readonly SemaphoreSlim sem = new(10, 10);

        public async Task<T?> SendSpotifyRequestAsync<T>(string reqUrl)
        {
            await sem.WaitAsync();
            try
            {
                var aToken = await spotifyTokenService.GetValidTokenAsync()
                            ?? throw new InvalidOperationException("Error getting Spotify access token");

                var req = new HttpRequestMessage(HttpMethod.Get, reqUrl);
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", aToken.AccessToken);

                var client = httpClientFactory.CreateClient();
                string reqMessage = $"Making a Spotify api request at the path '{reqUrl}'";
                logger.LogInformation(reqMessage);

                var resp = await client.SendAsync(req);
                resp.EnsureSuccessStatusCode();

                var result = await resp.Content.ReadFromJsonAsync<T>();

                return result;
            }
            catch (Exception ex)
            {
                string exMessage = $"error getting making Spotify API request at {reqUrl}";
                logger.LogError(ex, exMessage);
                return default;
            }
            finally
            {
                sem.Release();
            }
        }
    }
}

using Stream_Linkfiy_Backend.DTOs.Spotify;
using Stream_Linkfiy_Backend.Helpers;
using Stream_Linkfiy_Backend.Interfaces;
using System.Text;
using System.Text.Json;

namespace Stream_Linkfiy_Backend.Services
{
    public class SpotifyTokenService : ISpotifyTokenService
    {
        private SpotifyAccessTokenDto? token;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly ILogger<SpotifyTokenService> logger;
        private readonly SemaphoreSlim sem = new(1, 1);

        public SpotifyTokenService(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<SpotifyTokenService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = config;
            this.logger = logger;
        }
        public async Task<SpotifyAccessTokenDto?> GetValidTokenAsync()
        {
            await sem.WaitAsync();
            try
            {
                if (token == null || !IsValidToken())
                {
                    logger.LogInformation("Getting new spotify access token");
                    token = await RefreshTokenAsync();
                }

                return token;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred while getting Spotify access token");
                throw new Exception($"error occured while getting spotify token: {ex.Message}");
            }
            finally
            {
                sem.Release();
            }
        }

        public async Task<SpotifyAccessTokenDto> RefreshTokenAsync()
        {
            var client = httpClientFactory.CreateClient();
            var clientId = RequiredConfig.Get(config, "Spotify:ClientId");
            var clientSecret = RequiredConfig.Get(config, "Spotify:ClientSecret");

            var url = "https://accounts.spotify.com/api/token";
            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"))
                );

            req.Content = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            ]);

            var resp = await client.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            var token = await resp.Content.ReadFromJsonAsync<SpotifyAccessTokenDto>() 
                ?? throw new Exception("Error deseralizing spotify token json");
            var expiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn).ToUnixTimeSeconds();

            logger.LogDebug("Retrieved new Spotify token expires at {ExpiresAt}",
                DateTimeOffset.FromUnixTimeSeconds(expiresAt));

            return token with { ExpiresAt = expiresAt };

        }

        public bool IsValidToken()
        {
            if (token == null)
                return false;

            if (token.ExpiresAt == null)
                return false;

            return DateTimeOffset.FromUnixTimeSeconds(token.ExpiresAt.Value) >
                DateTimeOffset.UtcNow.AddMinutes(5);
        }
    }
}

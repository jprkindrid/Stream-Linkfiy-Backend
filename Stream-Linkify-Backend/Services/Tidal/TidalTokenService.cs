using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Helpers;
using Stream_Linkify_Backend.Interfaces.Tidal;
using System.Text;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalTokenService : ITidalTokenService
    {
        private TidalAccessTokenDto? token;
        private readonly IConfiguration config;
        private readonly ILogger<TidalTokenService> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly SemaphoreSlim sem = new(1, 1);

        public TidalTokenService(
            IConfiguration config,
            ILogger<TidalTokenService> logger,
            IHttpClientFactory httpClientFactory
        )
        {
            this.config = config;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<TidalAccessTokenDto> GetValidTokenAsync()
        {
            await sem.WaitAsync();
            try
            {
                if (token == null || !IsValidToken())
                {
                    logger.LogInformation("Getting new TIDAL access token");
                    token = await FetchNewToken();
                }

                return token!;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error occurred while getting TIDAL access token");
                throw new Exception($"Error occurred while getting TIDAL token: {ex.Message}");
            }
            finally
            {
                sem.Release();
            }
        }

        private async Task<TidalAccessTokenDto> FetchNewToken()
        {
            var client = httpClientFactory.CreateClient();
            var clientId = RequiredConfig.Get(config, "Tidal:ClientId");
            var clientSecret = RequiredConfig.Get(config, "Tidal:ClientSecret");

            var url = "https://auth.tidal.com/v1/oauth2/token";

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"))
            );

            req.Content = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                }
            );

            var resp = await client.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            var token = await resp.Content.ReadFromJsonAsync<TidalAccessTokenDto>()
                ?? throw new Exception("Error deserializing TIDAL token JSON");

            token.ExpiresAt = DateTimeOffset.UtcNow
                .AddSeconds(token.ExpiresIn)
                .ToUnixTimeSeconds();

            return token;
        }

        private bool IsValidToken()
        {
            if (token?.ExpiresAt == null)
                return false;

            return DateTimeOffset.FromUnixTimeSeconds(token.ExpiresAt) >
                   DateTimeOffset.UtcNow.AddMinutes(5);
        }
    }
}
using Stream_Linkfiy_Backend.DTOs.Spotify;
using Stream_Linkfiy_Backend.Interfaces;
using System.Runtime.Intrinsics.X86;

namespace Stream_Linkfiy_Backend.Services
{
    public class SpotifyTrackService : ISpotifyTrackService
    {
        private const string spotifyApiTrackurl = "https://api.spotify.com/v1/tracks";
        private readonly SemaphoreSlim sem = new(1, 1);
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<SpotifyTrackService> logger;
        private readonly ISpotifyTokenService spotifyTokenService;

        public SpotifyTrackService(IHttpClientFactory httpClientFactory, ILogger<SpotifyTrackService> logger, ISpotifyTokenService spotifyTokenService)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.spotifyTokenService = spotifyTokenService;
        }
        public async Task<SpotifyTrackDto?> GetTrackAsync(string spotifyUrl)
        {
            await sem.WaitAsync();
            try
            {
                var aToken = await spotifyTokenService.GetValidTokenAsync()
                    ?? throw new InvalidOperationException("Error getting spotify access token");

                var trackID = ExtractTrackId(spotifyUrl);
                var reqUrl = $"{spotifyApiTrackurl}/{trackID}";

                var req = new HttpRequestMessage(HttpMethod.Get, reqUrl);
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", aToken.AccessToken);

                var client = httpClientFactory.CreateClient();
                string reqMessage = $"Making a spotify api request at the path '{reqUrl}'";
                logger.LogInformation(reqMessage);
                var resp = await client.SendAsync(req);
                resp.EnsureSuccessStatusCode();

                SpotifyTrackDto track = await resp.Content.ReadFromJsonAsync<SpotifyTrackDto>()
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
                sem.Release();
            }

        }

        public string ExtractTrackId(string spotifyUrl)
        {
            if (!Uri.TryCreate(spotifyUrl, UriKind.Absolute, out var uri))
                throw new ArgumentException("Invalid URL format");

            if (uri.Host == "open.spotify.com")
            {
                var pathParts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length >= 2 && pathParts[0] == "track")
                {
                    return pathParts[1];
                }
                
            }
            else if (uri.Scheme == "spotify")
                // if they enter a URI for some reason
            {
                var parts = uri.AbsolutePath.Split(":");
                if (parts.Length == 3 && parts[1] == "track")
                {
                    return parts[2];
                }
            }

            throw new ArgumentException("Not valid Spotify url");
        }
    }
}

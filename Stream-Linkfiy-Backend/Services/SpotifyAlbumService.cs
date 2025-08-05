using Stream_Linkfiy_Backend.DTOs.Spotify;
using Stream_Linkfiy_Backend.Interfaces;
using System.Runtime.Intrinsics.X86;

namespace Stream_Linkfiy_Backend.Services
{
    public class SpotifyAlbumService : ISpotifyAlbumService
    {
        private const string spotifyApiAlbumUrl = "https://api.spotify.com/v1/albums";
        private readonly SemaphoreSlim sem = new(1, 1);
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<SpotifyTrackService> logger;
        private readonly ISpotifyTokenService spotifyTokenService;

        public SpotifyAlbumService(IHttpClientFactory httpClientFactory, ILogger<SpotifyTrackService> logger, ISpotifyTokenService spotifyTokenService)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.spotifyTokenService = spotifyTokenService;
        }
        public async Task<SpotifyAlbumDto> GetAlbumAsync(string spotifyUrl)
        {
            await sem.WaitAsync();
            try
            {
                var aToken = await spotifyTokenService.GetValidTokenAsync()
                    ?? throw new InvalidOperationException("Error getting spotify access token");

                var albumID = ExtractAlbumId(spotifyUrl);
                var reqUrl = $"{spotifyApiAlbumUrl}/{albumID}";

                var req = new HttpRequestMessage(HttpMethod.Get, reqUrl);
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", aToken.AccessToken);

                var client = httpClientFactory.CreateClient();
                string reqMessage = $"Making a spotify api request at the path '{reqUrl}'";
                logger.LogInformation(reqMessage);
                var resp = await client.SendAsync(req);
                resp.EnsureSuccessStatusCode();

                SpotifyAlbumDto album = await resp.Content.ReadFromJsonAsync<SpotifyAlbumDto>()
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
                sem.Release();
            }

        }

        public string ExtractAlbumId(string spotifyUrl)
        {
            if (!Uri.TryCreate(spotifyUrl, UriKind.Absolute, out var uri))
                throw new ArgumentException("Invalid URL format");

            if (uri.Host == "open.spotify.com")
            {
                var pathParts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length >= 2 && pathParts[0] == "album")
                {
                    return pathParts[1];
                }
                
            }
            else if (uri.Scheme == "spotify")
                // if they enter a URI for some reason
            {
                var parts = uri.AbsolutePath.Split(":");
                if (parts.Length == 3 && parts[1] == "album")
                {
                    return parts[2];
                }
            }

            throw new ArgumentException("Not valid Spotify url");
        }
    }
}

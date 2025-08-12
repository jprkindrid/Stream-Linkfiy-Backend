using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Stream_Linkify_Backend.DTOs.Spotify;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Spotify;

namespace Stream_Linkify_Backend.Services.Spotify
{
    public class SpotifyTrackService : ISpotifyTrackService
    {
        private const string spotifyApiUrl = "https://api.Spotify.com/v1";
        private readonly ISpotifyApiClient spotifyApiClient;
        private readonly ILogger<SpotifyTrackService> logger;

        public SpotifyTrackService(
            ISpotifyApiClient spotifyApiClient,
            ILogger<SpotifyTrackService> logger
            )
        {

            this.spotifyApiClient = spotifyApiClient;
            this.logger = logger;
        }
        public async Task<SpotifyTrackFullDto?> GetByUrlAsync(string spotifyUrl)
        {

            var trackID = SpotifyUrlHelper.ExtractSpotifyId(spotifyUrl, "track");
            var reqUrl = $"{spotifyApiUrl}/tracks/{trackID}";

            var result = await spotifyApiClient.SendSpotifyRequestAsync<SpotifyTrackFullDto>(reqUrl);

            return result;
        }

        public async Task<(string? url, string? albumName, List<string> artistNames)> GetByIsrcAsync(string isrc)
        {
            var query = $"isrc:{isrc}";
            var reqUrl = $"{spotifyApiUrl}/search/?q={Uri.EscapeDataString(query)}&type=track%2Calbum";

            SpotifySearchResponseDto? result = await spotifyApiClient.SendSpotifyRequestAsync<SpotifySearchResponseDto>(reqUrl);

            if (result == null)
            {
                logger.LogWarning("Could not get Spotify track for isrc '{isrc}'", isrc);
                return (null, null, []);
            }

            var trackId = result.Tracks?.Items.FirstOrDefault()?.Id;
            var url = $"https://open.spotify.com/track/{trackId}";

            var albumName = result.Tracks?.Items?.FirstOrDefault()?.Album?.Name;

            var artistNames = result.Tracks?.Items?.FirstOrDefault()?.Artists.Select(a => a.Name).ToList();
            if (artistNames == null)
                return (url, albumName, []);

            return (url, albumName, artistNames);
        }
    }
}

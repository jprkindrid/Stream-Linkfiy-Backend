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

        public SpotifyTrackService(
            ISpotifyApiClient spotifyApiClient)
        {

            this.spotifyApiClient = spotifyApiClient;
        }
        public async Task<SpotifyTrackFullDto?> GetByUrlAsync(string spotifyUrl)
        {

            var trackID = SpotifyUrlHelper.ExtractSpotifyId(spotifyUrl, "track");
            var reqUrl = $"{spotifyApiUrl}/tracks/{trackID}";

            var result = await spotifyApiClient.SendSpotifyRequestAsync<SpotifyTrackFullDto>(reqUrl);

            return result;
        }

        public async Task<SpotifySearchResponseDto?> GetByIsrcAsync(string isrc)
        {
            var query = $"isrc:{isrc}";
            var reqUrl = $"{spotifyApiUrl}/search/?q={Uri.EscapeDataString(query)}&type=track%2Calbum";

            var result = await spotifyApiClient.SendSpotifyRequestAsync<SpotifySearchResponseDto>(reqUrl);

            return result;
        }
    }
}

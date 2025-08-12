using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalArtistService : ITidalArtistService

    {
        const string tidalApiUrl = "https://openapi.tidal.com/v2";
        private readonly ITidalApiClient tidalApiClient;
        private readonly ILogger<TidalArtistService> logger;

        public TidalArtistService(
            ITidalApiClient tidalApiClient,
            ILogger<TidalArtistService> logger
            )
        {
            this.tidalApiClient = tidalApiClient;
            this.logger = logger;
        }
        public async Task<List<string>?> GetTrackArtistNamesAsync(string tidalUrl)
        {
            var trackId = TidalUrlHelper.ExtractTidalId(tidalUrl, "track");
            var reqUrl = $"{tidalApiUrl}/tracks/{trackId}/relationships/artists?countryCode=US&include=tracks";

            var result = await tidalApiClient.SendTidalRequestAsync<TidalArtistsResponseDto>(reqUrl);

            if (result == null || result.Included == null || result.Included.Count == 0)
            {
                logger.LogWarning("Could not get artists for request {reqUrl}", reqUrl);
                return [];
            }

            return [.. result.Included.Select(a => a.Attributes.Name)];
        }
    }
}

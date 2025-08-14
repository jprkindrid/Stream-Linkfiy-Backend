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

        public async Task<List<string>?> GetArtistNamesAsync(string tidalUrl, string itemType)
        {
            itemType = itemType.ToLowerInvariant();
            var id = itemType switch
            {
                "track" => TidalUrlHelper.ExtractTidalId(tidalUrl, "track"),
                "album" => TidalUrlHelper.ExtractTidalId(tidalUrl, "album"),
                _ => throw new InvalidOperationException($"Invalid itemType for tidal.GetArtistNames: {itemType}")
            };

            var reqUrl = itemType switch
            {
                "track" => $"{tidalApiUrl}/tracks/{id}/relationships/artists?countryCode=US&include=tracks",
                "album" => $"{tidalApiUrl}/albums/{id}/relationships/artists?countryCode=US&include=albums",
                _ => throw new InvalidOperationException($"Invalid itemType for tidal.GetArtistNames: {itemType}")
            };

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

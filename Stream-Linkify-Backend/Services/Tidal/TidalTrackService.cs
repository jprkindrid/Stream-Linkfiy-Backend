using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalTrackService : ITidalTrackService

    {
        const string tidalApiUrl = "https://openapi.tidal.com/v2";
        private readonly ITidalApiClient tidalApiClient;

        public TidalTrackService(ITidalApiClient tidalApiClient)
        {
            this.tidalApiClient = tidalApiClient;
        }
        public async Task<TidalTrackResponseDto?> GetTrackByUrlAsync(string tidalUrl)
        {
            var trackId = TidalUrlHelper.ExtractTidalId(tidalUrl, "track");
            var reqUrl = $"{tidalApiUrl}/tracks/{trackId}?countryCode=US&include=tracks";

            var result = await tidalApiClient.SendTidalRequestAsync<TidalTrackResponseDto>(reqUrl);

            return result;
        }

        public async Task<string?> GetTrackUrlByNameAsync(string trackName, string artistName, string isrc, string countryCode = "US")
        {
            var query = $"{trackName}-{artistName}";
            var reqUrl = $"{tidalApiUrl}/searchResults/{Uri.EscapeDataString(query)}?countryCode={countryCode}&include=tracks";

            TidalSearchResponseDto? result = await tidalApiClient.SendTidalRequestAsync<TidalSearchResponseDto>(reqUrl);
            if (result == null || result.Included.Count == 0)
                return null;

            var browseUrl = result.Included
                .Select(i => i.DeserializeAttributes<TidalTrackAttributes>())
                .Where(a => a != null && string.Equals(a.Isrc?.Trim(), isrc?.Trim(), StringComparison.OrdinalIgnoreCase))
                .SelectMany(a => a?.ExternalLinks ?? Enumerable.Empty<TidalExternalLink>())
                .Select(l => l.Href)
                .FirstOrDefault(href => !string.IsNullOrWhiteSpace(href));

            if (browseUrl == null) return null;

            var trackId = TidalUrlHelper.ExtractTidalId(browseUrl, "track");
            return $"https://listen.tidal.com/track/{trackId}";
        }
    }
}

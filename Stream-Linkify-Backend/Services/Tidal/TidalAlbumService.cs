using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalAlbumService : ITidalAlbumService
    {
        const string tidalApiUrl = "https://openapi.tidal.com/v2";
        private readonly ITidalApiClient tidalApiClient;
        private readonly ILogger<TidalAlbumService> logger;

        public TidalAlbumService(
            ITidalApiClient tidalApiClient,
            ILogger<TidalAlbumService> logger
            )
        {
            this.tidalApiClient = tidalApiClient;
            this.logger = logger;
        }

        // TODO: IMPLEMENT THESE
        public async Task<TidalAlbumResponseDto?> GetByUrlAsync(string url)
        {
            var albumId = TidalUrlHelper.ExtractTidalId(url, "album");
            var reqUrl = $"{tidalApiUrl}/albums/{albumId}?countryCode=US&include=artists";
            var result = await tidalApiClient.SendTidalRequestAsync<TidalAlbumResponseDto>(reqUrl);

            return result;

        }

        public async Task<string?> GetUrlByNameAsync(string albumName, string firstArtistName, string upc, string countryCode = "US")
        {
            var query = $"{albumName}-{firstArtistName}";
            var reqUrl = $"{tidalApiUrl}/searchResults/{Uri.EscapeDataString(query)}?countryCode={countryCode}&include=albums";
            TidalSearchResponseDto? result = await tidalApiClient.SendTidalRequestAsync<TidalSearchResponseDto>(reqUrl);

            if (result == null || result.Included.Count == 0)
                return null;

           var browseUrl = result.Included
                .Select(i => i.DeserializeAttributes<TidalAlbumAttributes>())
                .Where(a => a != null && string.Equals(a.BarcodeId.Trim(), upc?.Trim(), StringComparison.OrdinalIgnoreCase))
                .SelectMany(a => a?.ExternalLinks ?? Enumerable.Empty<TidalExternalLink>())
                .Select(l => l.Href)
                .FirstOrDefault(href => !string.IsNullOrWhiteSpace(href));

            if (browseUrl == null) return null;

            var albumId = TidalUrlHelper.ExtractTidalId(browseUrl, "album");

            return $"https://listen.tidal.com/album/{albumId}";
        }
    }
}

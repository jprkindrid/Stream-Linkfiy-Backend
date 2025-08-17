using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalAlbumService(
        ITidalApiClient tidalApiClient,
        ILogger<TidalAlbumService> logger,
        ITidalArtistService tidalArtistService
            ) : ITidalAlbumService
    {
        const string tidalApiUrl = "https://openapi.tidal.com/v2";
        private readonly ITidalApiClient tidalApiClient = tidalApiClient;
        private readonly ILogger<TidalAlbumService> logger = logger;
        private readonly ITidalArtistService tidalArtistService = tidalArtistService;

        public async Task<TidalAlbumResponseDto?> GetByUrlAsync(string url)
        {
            var albumId = TidalUrlHelper.ExtractTidalId(url, "album");
            var reqUrl = $"{tidalApiUrl}/albums/{albumId}?countryCode=US&include=artists";
            var result = await tidalApiClient.SendTidalRequestAsync<TidalAlbumResponseDto>(reqUrl);

            return result;

        }

        public async Task<string?> GetUrlByNameAsync(string albumName, string artistName, string upc)
        {
            var query = $"{albumName}-{artistName}";
            var reqUrl = $"{tidalApiUrl}/searchResults/{Uri.EscapeDataString(query)}?countryCode=US&include=albums";
            TidalSearchResponseDto? result = await tidalApiClient.SendTidalRequestAsync<TidalSearchResponseDto>(reqUrl);

            if (result == null || result.Included.Count == 0)
            {
                logger.LogWarning("No TIDAL search results for album {albumName} with artist {artistName}", albumName, artistName);
                return null;
            }

            var browseUrl = result.Included
                 .Select(i => i.DeserializeAttributes<TidalAlbumAttributes>())
                 .Where(a => a != null && string.Equals(a.BarcodeId.Trim(), upc?.Trim(), StringComparison.OrdinalIgnoreCase))
                 .SelectMany(a => a?.ExternalLinks ?? Enumerable.Empty<TidalExternalLink>())
                 .Select(l => l.Href)
                 .FirstOrDefault(href => !string.IsNullOrWhiteSpace(href));

            if (browseUrl != null)
            {
                var albumId = TidalUrlHelper.ExtractTidalId(browseUrl, "album");
                return $"https://listen.tidal.com/album/{albumId}";
            }

            logger.LogWarning("Unable to get TIDAL album with upc '{upc}'", upc);

            // This isnt as accurate but I can't check for artists without calling a seperate
            // endpoint for the artists (in the TidalArtistService) which results in 20-50
            // extra calls and I will inevitably be rate limited so this is the solution
            // until TIDAL returns artist names with their track/album response.

            var firstAlbum = result.Included.FirstOrDefault();
            if (firstAlbum == null)
            {
                logger.LogWarning("No TIDAL search results for album {albumName} with artist {artistName}", albumName, artistName);
                return null;
            }

            return $"https://listen.tidal.com/album/{firstAlbum.Id}";
        }
    }
}

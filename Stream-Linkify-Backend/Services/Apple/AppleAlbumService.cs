using Stream_Linkify_Backend.DTOs.Apple;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Apple;

namespace Stream_Linkify_Backend.Services.Apple
{
    public class AppleAlbumService : IAppleAlbumService
    {
        private readonly IAppleApiClient appleApiClient;
        private readonly ILogger<IAppleAlbumService> logger;

        public AppleAlbumService(
            IAppleApiClient appleApiClient,
            ILogger<IAppleAlbumService> logger
            )
        {
            this.appleApiClient = appleApiClient;
            this.logger = logger;
        }
        public async Task<AppleAlbumDataDto?> GetByUrlAsync(string appleUrl)
        {
            var (region, albumId, _) = AppleUrlHelper.ExtractAppleAlbumIdAndRegion(appleUrl);
            var reqUrl = $"https://api.music.apple.com/v1/catalog/{region}/albums/{albumId}";
            var result = await appleApiClient.SendAppleRequestAsync<AppleAlbumResponseDto>(reqUrl);
            if (result == null || result.Data.Count == 0)
            {
                logger.LogWarning("Unable to get Apple Music album with url '{appleUrl}'", appleUrl);
                return null;
            }

            return result.Data[0];
        }

        public async Task<string?> GetUrlByUpcAsync(string upc)
        {
            var reqUrl = $"https://api.music.apple.com/v1/catalog/us/albums?filter[upc]={upc}";
            var result = await appleApiClient.SendAppleRequestAsync<AppleAlbumResponseDto>(reqUrl);
            if (result == null || result.Data.Count == 0)
            {
                logger.LogWarning("Unable to get Apple Music with UPC '{upc}'", upc);
                return null;
            }

            if (!(string.Equals(upc, result.Data[0].Attributes.Upc, StringComparison.OrdinalIgnoreCase)))
            {
                logger.LogWarning("Apple Music request for album with UPC '{upc}' returned incorrect album", upc);
                return null;
            }

            return result.Data[0].Attributes.Url;
        }
    }
}

using Stream_Linkify_Backend.DTOs.Apple;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Apple;

namespace Stream_Linkify_Backend.Services.Apple
{
    public class AppleAlbumService : IAppleAlbumService
    {
        private const string appleMusicApiUrl = "https://api.music.apple.com/v1";
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
            var reqUrl = $"{appleMusicApiUrl}/catalog/{region}/albums/{albumId}";
            var result = await appleApiClient.SendAppleRequestAsync<AppleAlbumResponseDto>(reqUrl);
            if (result == null || result.Data.Count == 0)
            {
                logger.LogWarning("Unable to get Apple Music album with url '{appleUrl}'", appleUrl);
                return null;
            }

            return result.Data[0];
        }

        public async Task<string?> GetUrlByNameAsync(string upc, string albumName, string artistName)
        {
            var reqUrl = $"{appleMusicApiUrl}/catalog/us/albums?filter[upc]={upc}";
            var result = await appleApiClient.SendAppleRequestAsync<AppleAlbumResponseDto>(reqUrl);
            if (result != null && result.Data.Count != 0)
            {
                if ((string.Equals(upc, result?.Data[0].Attributes.Upc, StringComparison.OrdinalIgnoreCase)))
                    return result?.Data[0].Attributes.Url;
            }
            logger.LogWarning("Unable to get Apple Music with UPC '{upc}'", upc);

            var query = $"{albumName} {artistName}";

            reqUrl = $"{appleMusicApiUrl}/catalog/us/search?types=albums&term={Uri.EscapeDataString(query)}";
            var searchResult = await appleApiClient.SendAppleRequestAsync<AppleSearchResponseDto>(reqUrl);

            if (searchResult == null || searchResult.Results.Albums == null || searchResult.Results.Albums.Data.Count == 0)
            {
                logger.LogWarning("No Apple Music search result for album {albumName} and artist {artistName}", albumName, artistName);
                return null;
            }

            foreach (var album in searchResult.Results.Albums.Data)
            {
                if (album.Attributes.Name == albumName && album.Attributes.ArtistName == artistName)
                    return album.Attributes.Url;
            }

            logger.LogWarning("No Apple Music search result for album {albumName} and artist {artistName}", albumName, artistName);


            return null;


        }
    }
}

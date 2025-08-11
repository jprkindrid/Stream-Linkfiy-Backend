using Newtonsoft.Json.Linq;
using Stream_Linkify_Backend.DTOs.Apple;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Services.Apple
{
    public class AppleTrackService : IAppleTrackService
    {
        private readonly IAppleApiClient appleApiClient;
        private readonly ILogger<AppleTrackService> logger;

        public AppleTrackService(IAppleApiClient appleApiClient,
            ILogger<AppleTrackService> logger)
        {
            this.appleApiClient = appleApiClient;
            this.logger = logger;
        }

        public async Task<AppleSongDataDto?> GetTrackByUrlAsync(string url)
        {
            var (region, _, trackId) = AppleUrlHelper.ExtractAppleTrackId(url);
            var reqUrl = $"https://api.music.apple.com/v1/catalog/{region}/songs/{trackId}";

            var result = await appleApiClient.SendAppleRequestAsync<AppleSongResponse>(reqUrl);

            if (result == null || result.Data == null || result.Data.Count == 0)
            {
                logger.LogWarning("Apple API returned no data for {Url}", reqUrl);
                logger.LogDebug("Apple Music Response: {@Result}", result);
                return null;
            }

            return result.Data.FirstOrDefault();

        }
        public async Task<AppleSongDataDto?> GetTrackByIsrcAsync(string isrc)
        {
            var reqUrl = $"https://api.music.apple.com/v1/catalog/us/songs?filter[isrc]={isrc}";

            var result = await appleApiClient.SendAppleRequestAsync<AppleSongResponse>(reqUrl);

            if (result == null || result.Data == null || result.Data.Count == 0)
            {
                logger.LogWarning("Apple API returned no data for {Url}", reqUrl);
                logger.LogDebug("Apple Music Response: {@Result}", result);
                return null;
            }

            return result.Data.FirstOrDefault();
        }


    }
}

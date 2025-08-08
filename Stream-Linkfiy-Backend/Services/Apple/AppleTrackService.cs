using Newtonsoft.Json.Linq;
using Stream_Linkify_Backend.DTOs.Apple;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Apple;

namespace Stream_Linkify_Backend.Services.Apple
{
    public class AppleTrackService : IAppleTrackService
    {
        private readonly IAppleApiClient appleApiClient;

        public AppleTrackService(IAppleApiClient appleApiClient)
        {
            this.appleApiClient = appleApiClient;
        }

        public async Task<AppleSongDataDto?> GetTrackByUrlAsync(string url)
        {
            var (region, _, trackId) = AppleUrlHelper.ExtractAppleTrackId(url);
            var reqUrl = $"https://api.music.apple.com/v1/catalog/{region}/songs/{trackId}";

            var result = await appleApiClient.SendAppleRequestAsync<AppleSongResponse>(reqUrl);
            
            return result.Data.FirstOrDefault();

        }
        public async Task<AppleSongDataDto?> GetTrackByIsrcAsync(string isrc)
        {
            var reqUrl = $"https://api.music.apple.com/v1/catalog/us/songs?filter[isrc]={isrc}";

            var result = await appleApiClient.SendAppleRequestAsync<AppleSongResponse>(reqUrl);

            return result.Data.FirstOrDefault();
        }


    }
}

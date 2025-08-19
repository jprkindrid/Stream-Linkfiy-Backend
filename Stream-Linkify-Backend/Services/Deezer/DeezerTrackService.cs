using Stream_Linkify_Backend.DTOs.Deezer.Stream_Linkify_Backend.DTOs.Deezer;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Deezer;

namespace Stream_Linkify_Backend.Services.Deezer
{
    public class DeezerTrackService(
        IDeezerApiClient deezerApiClient,
        ILogger<DeezerTrackService> logger
        ) : IDeezerTrackService
    {
        const string deezerApiUrl = "https://api.deezer.com";
        private readonly IDeezerApiClient deezerApiClient = deezerApiClient;
        private readonly ILogger<DeezerTrackService> logger = logger;

        public Task<string> GetByNameAsync(string trackName, string artistName)
        {
            throw new NotImplementedException();
        }

        public async Task<DeezerTrackFullDto?> GetByUrlAsync(string deezerUrl)
        {
            var trackId = await DeezerUrlHelper.ExtractDeezerId(deezerUrl);
            var reqUrl = $"{deezerApiUrl}/track/{trackId}";

            DeezerTrackFullDto? result = await deezerApiClient.SendDeezerRequestAsync<DeezerTrackFullDto>(reqUrl);

            if (result == null || string.IsNullOrWhiteSpace(result.Title) || string.IsNullOrWhiteSpace(result.Artist.Name))
            {
                logger.LogWarning("Could not get response for Deezer request {url}", reqUrl);
                return null;
            }

            return result;
        }
    }
}

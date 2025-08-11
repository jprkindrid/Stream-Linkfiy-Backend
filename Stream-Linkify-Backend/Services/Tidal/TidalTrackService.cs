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

        public Task<TidalSearchResponseDto?> GetTrackByNameAsync(string trackName, string artistName, string? albumName)
        {
            throw new NotImplementedException();
        }
    }
}

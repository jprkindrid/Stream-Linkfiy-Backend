using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalInput : ITidalInput
    {
        private readonly ILogger<TidalInput> logger;
        private readonly ITidalTrackService tidalTrackService;
        private readonly ISpotifyTrackService spotifyTrackService;
        private readonly IAppleTrackService appleTrackService;

        public TidalInput(
            ILogger<TidalInput> logger,
            ITidalTrackService tidalTrackService,
            ISpotifyTrackService spotifyTrackService,
            IAppleTrackService appleTrackService
            )
        {
            this.logger = logger;
            this.tidalTrackService = tidalTrackService;
            this.spotifyTrackService = spotifyTrackService;
            this.appleTrackService = appleTrackService;
        }

        public async Task<TrackReturnDto> GetUrlsAsync(string tidalUrl)
        {
            throw new NotImplementedException();
            //TidalTrackResponseDto tidalTrack = await tidalTrackService.GetTrackByUrlAsync(tidalUrl)
            //    ?? throw new InvalidOperationException("Tidal track not found");

            //var result = new TrackModel
            //{

            //}

        }
    }
}

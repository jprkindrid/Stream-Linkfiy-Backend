using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Services.Spotify
{
    public class SpotifyInput : ISpotifyInput
    {
        private readonly ILogger<SpotifyInput> logger;
        private readonly ISpotifyTrackService spotifyTrackService;
        private readonly IAppleTrackService appleTrackService;

        public SpotifyInput(
            ILogger<SpotifyInput> logger,
            ISpotifyTrackService spotifyTrackService,
            IAppleTrackService appleTrackService
            )
        {
            this.logger = logger;
            this.spotifyTrackService = spotifyTrackService;
            this.appleTrackService = appleTrackService;
        }

        public async Task<TrackModel> getUrlsAsync(string spotifyUrl)
        {
            var result = new TrackModel
            {
                SpotifyUrl = spotifyUrl
            };

            // Get Spotify track
            var spotifyTrack = await spotifyTrackService.GetByUrlAsync(spotifyUrl);
            if (spotifyTrack == null)
                throw new InvalidOperationException("Spotify track not found");

            result.ISRC = spotifyTrack.ExternalIds.Isrc;

            // Get AppleMusic track by ISRC
            var appleTrack = await appleTrackService.GetTrackByIsrcAsync(result.ISRC!);

            if (appleTrack == null)
            {
                logger.LogWarning("No Apple Music track found for ISRC {ISRC}", result.ISRC);
                result.AppleMusicUrl = null;
            }
            else
            {
                result.AppleMusicUrl = appleTrack.Attributes.Url;
            }

            return result;
        }
    }
}


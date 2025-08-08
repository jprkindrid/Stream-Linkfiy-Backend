using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Models;
using Stream_Linkify_Backend.Services.Spotify;

namespace Stream_Linkify_Backend.Services.Apple
{
    public class AppleInput : IAppleInput
    {
        private readonly ILogger<AppleInput> logger;
        private readonly ISpotifyTrackService spotifyTrackService;
        private readonly IAppleTrackService appleTrackService;

        public AppleInput(
            ILogger<AppleInput> logger,
            ISpotifyTrackService spotifyTrackService,
            IAppleTrackService appleTrackService
            )
        {
            this.logger = logger;
            this.spotifyTrackService = spotifyTrackService;
            this.appleTrackService = appleTrackService;
        }

        public async Task<TrackModel> getUrlsAsync(string appleUrl)
        {
            var result = new TrackModel
            {
                AppleMusicUrl = appleUrl,
            };

            var appleTrack = await appleTrackService.GetTrackByUrlAsync(appleUrl);
            if (appleTrack == null)
                throw new InvalidOperationException("apple track not found");

            result.ISRC = appleTrack.Attributes.Isrc;

            // Get Spoify Track from ISRC
            var spotifyTrack = await spotifyTrackService.GetByIsrcAsync(result.ISRC);

            if (spotifyTrack == null)
            {
                logger.LogWarning("No result found on Spotify for ISRC {ISRC}", result.ISRC);
                result.SpotifyUrl = null;
            }
            else
            {
                var firstTrack = spotifyTrack?.Tracks?.Items?
                    .FirstOrDefault(t => string.Equals(t.Type, "track", StringComparison.OrdinalIgnoreCase));

                if (firstTrack != null)
                {
                    result.SpotifyUrl = $"https://open.spotify.com/track/{firstTrack.Id}";
                }
                else
                {
                    logger.LogWarning("No track item found in Spotify search results for ISRC {ISRC}", result.ISRC);
                    result.SpotifyUrl = null;
                }
            }

            return result;
        }
    }
}

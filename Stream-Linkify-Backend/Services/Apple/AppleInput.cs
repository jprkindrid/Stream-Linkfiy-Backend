using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.DTOs.Apple;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Mappers;
using Stream_Linkify_Backend.Models;
using Stream_Linkify_Backend.Services.Spotify;

namespace Stream_Linkify_Backend.Services.Apple
{
    public class AppleInput : IAppleInput
    {
        private readonly ILogger<AppleInput> logger;
        private readonly ISpotifyTrackService spotifyTrackService;
        private readonly IAppleTrackService appleTrackService;
        private readonly ITidalTrackService tidalTrackService;

        public AppleInput(
            ILogger<AppleInput> logger,
            ISpotifyTrackService spotifyTrackService,
            IAppleTrackService appleTrackService,
            ITidalTrackService tidalTrackService
            )
        {
            this.logger = logger;
            this.spotifyTrackService = spotifyTrackService;
            this.appleTrackService = appleTrackService;
            this.tidalTrackService = tidalTrackService;
        }

        public async Task<TrackReturnDto> GetUrlsAsync(string appleUrl)
        {


            AppleSongDataDto? appleTrack = await appleTrackService.GetTrackByUrlAsync(appleUrl)
                ?? throw new InvalidOperationException("apple track not found");
            var result = new TrackModel
            {
                ISRC = appleTrack.Attributes.Isrc,
                AppleMusicUrl = appleTrack.Attributes.Url,
                SongName = appleTrack.Attributes.Name,
                AristNames = [appleTrack.Attributes.ArtistName],
                AlbumName = appleTrack.Attributes.AlbumName,
            };

            result.ISRC = appleTrack.Attributes.Isrc;

            // Get Spotify Track from ISRC
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
                    result.AristNames = [.. firstTrack.Artists.Select(a => a.Name)];
                    result.AlbumName = firstTrack.Album.Name;
                }
                else
                {
                    logger.LogWarning("No track item found in Spotify search results for ISRC {ISRC}", result.ISRC);
                    result.SpotifyUrl = null;
                }
            }

            // Get Tidal Track from artist, track title, isrc
            var tidalUrl = await tidalTrackService.GetTrackUrlByNameAsync(result.SongName, result.AristNames.First(), result.ISRC);
            if (tidalUrl != null)
            {
                result.TidalUrl = tidalUrl;
            }
            else
            {
                result.TidalUrl = null;
            }

            return result.ToReturnDo();
        }
    }
}

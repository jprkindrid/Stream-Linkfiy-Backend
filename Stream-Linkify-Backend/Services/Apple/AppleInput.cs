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

        public Task<TrackReturnDto> GetAlbumUrlsAsync(string appleUrl)
        {
            throw new NotImplementedException();
        }

        public async Task<TrackReturnDto> GetTrackUrlsAsync(string appleUrl)
        {


            AppleSongDataDto? appleTrack = await appleTrackService.GetTrackByUrlAsync(appleUrl)
                ?? throw new InvalidOperationException("apple track not found");
            var result = new TrackModel
            {
                ISRC = appleTrack.Attributes.Isrc,
                AppleMusicUrl = appleTrack.Attributes.Url,
                SongName = appleTrack.Attributes.Name,
                AritstNames = [appleTrack.Attributes.ArtistName],
                AlbumName = appleTrack.Attributes.AlbumName,
            };

            result.ISRC = appleTrack.Attributes.Isrc;

            // Get Spotify Track from ISRC
            var (spotifyTrackUrl, spotifyAlbumName, spotifyArtistNames) = await spotifyTrackService.GetByIsrcAsync(result.ISRC);
            result.SpotifyUrl = spotifyTrackUrl;
            if (spotifyAlbumName != null)
                result.AlbumName = spotifyAlbumName;

            if (spotifyArtistNames != null)
                result.AritstNames = spotifyArtistNames;

            // Get Tidal Track from artist, track title, isrc
            var tidalUrl = await tidalTrackService.GetTrackUrlByNameAsync(result.SongName, result.AritstNames.First(), result.ISRC);
            if (tidalUrl != null)
            {
                result.TidalUrl = tidalUrl;
            }
            else
            {
                result.TidalUrl = null;
            }

            return result.ToTrackReturnDo();
        }
    }
}

using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Mappers;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalInput : ITidalInput
    {
        private readonly ILogger<TidalInput> logger;
        private readonly ITidalTrackService tidalTrackService;
        private readonly ITidalArtistService tidalArtistService;
        private readonly ISpotifyTrackService spotifyTrackService;
        private readonly IAppleTrackService appleTrackService;

        public TidalInput(
            ILogger<TidalInput> logger,
            ITidalTrackService tidalTrackService,
            ITidalArtistService tidalArtistService,
            ISpotifyTrackService spotifyTrackService,
            IAppleTrackService appleTrackService
            )
        {
            this.logger = logger;
            this.tidalTrackService = tidalTrackService;
            this.tidalArtistService = tidalArtistService;
            this.spotifyTrackService = spotifyTrackService;
            this.appleTrackService = appleTrackService;
        }

        public async Task<TrackReturnDto> GetUrlsAsync(string tidalUrl)
        {
            TidalTrackResponseDto tidalTrack = await tidalTrackService.GetTrackByUrlAsync(tidalUrl)
                ?? throw new InvalidOperationException("Tidal track not found");

            var trueTidalUrl = tidalTrack.Data.Attributes.ExternalLinks.Select(l => l.Href).FirstOrDefault();

            var tidalArtists = await tidalArtistService.GetTrackArtistNamesAsync(tidalUrl) ?? null;
            var result = new TrackModel
            {
                ISRC = tidalTrack.Data.Attributes.Isrc,
                SongName = tidalTrack.Data.Attributes.Title,
                AritstNames = tidalArtists ?? [],
                TidalUrl = trueTidalUrl ?? tidalUrl
            };

            // Spotify
            var(spotifyTrackUrl, spotifyAlbumName, spotifyArtistNames) = await spotifyTrackService.GetByIsrcAsync(result.ISRC);
            result.SpotifyUrl = spotifyTrackUrl;
            if (spotifyAlbumName != null)
                result.AlbumName = spotifyAlbumName;

            if (spotifyArtistNames != null)
                result.AritstNames = spotifyArtistNames;
        
            // Apple
            var appleTrack = await appleTrackService.GetTrackByIsrcAsync(result.ISRC);
            result.AppleMusicUrl = appleTrack?.Attributes?.Url;
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Could not get Apple music track for ISRC: {result.ISRC}", result.ISRC);

            return result.ToReturnDo();

        }
    }
}

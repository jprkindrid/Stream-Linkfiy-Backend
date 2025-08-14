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
        private readonly ITidalAlbumService tidalAlbumService;
        private readonly ITidalArtistService tidalArtistService;
        private readonly ISpotifyTrackService spotifyTrackService;
        private readonly ISpotifyAlbumService spotifyAlbumService;
        private readonly IAppleTrackService appleTrackService;
        private readonly IAppleAlbumService appleAlbumService;

        public TidalInput(
            ILogger<TidalInput> logger,
            ITidalTrackService tidalTrackService,
            ITidalAlbumService tidalAlbumService,
            ITidalArtistService tidalArtistService,
            ISpotifyTrackService spotifyTrackService,
            ISpotifyAlbumService spotifyAlbumService,
            IAppleTrackService appleTrackService,
            IAppleAlbumService appleAlbumService
            )
        {
            this.logger = logger;
            this.tidalTrackService = tidalTrackService;
            this.tidalAlbumService = tidalAlbumService;
            this.tidalArtistService = tidalArtistService;
            this.spotifyTrackService = spotifyTrackService;
            this.spotifyAlbumService = spotifyAlbumService;
            this.appleTrackService = appleTrackService;
            this.appleAlbumService = appleAlbumService;
        }

        public async Task<AlbumReturnDto> GetAlbumUrlsAsync(string tidalUrl)
        {
            TidalAlbumResponseDto tidalAlbum = await tidalAlbumService.GetByUrlAsync(tidalUrl)
                ?? throw new InvalidOperationException($"Tidal album not found for {tidalUrl}");

            var trueTidalurl = tidalAlbum.Data.Attributes.ExternalLinks.Select(l => l.Href).FirstOrDefault();

            var tidalArtists = await tidalArtistService.GetArtistNamesAsync(tidalUrl, "album") ?? null;
            var result = new AlbumModel
            {
                UPC = tidalAlbum.Data.Attributes.BarcodeId,
                AlbumName = tidalAlbum.Data.Attributes.Title,
                AritstNames = tidalArtists ?? [],
                TidalUrl = trueTidalurl ?? tidalUrl
            };

            // Spotify
            var (spotifyUrl, spotifyArtistNames) = await spotifyAlbumService.GetByUpcAsync(result.UPC);
            if (spotifyArtistNames != null && result.AritstNames.Count == 0)
                result.AritstNames = spotifyArtistNames;

            // Apple
            var appleAlbumUrl = await appleAlbumService.GetUrlByUpcAsync(result.UPC);
            result.AppleMusicUrl = appleAlbumUrl;

            return result.ToAlbumReturnDto();
        }

        public async Task<TrackReturnDto> GetTrackUrlsAsync(string tidalUrl)
        {
            TidalTrackResponseDto tidalTrack = await tidalTrackService.GetTrackByUrlAsync(tidalUrl)
                ?? throw new InvalidOperationException($"Tidal track not found for {tidalUrl}");

            var trueTidalUrl = tidalTrack.Data.Attributes.ExternalLinks.Select(l => l.Href).FirstOrDefault();

            var tidalArtists = await tidalArtistService.GetArtistNamesAsync(tidalUrl, "track") ?? null;
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

            if (spotifyArtistNames != null && result.AritstNames.Count == 0)
                result.AritstNames = spotifyArtistNames;

            // Apple
            var appleTrackUrl = await appleTrackService.GetTrackUrlByIsrcAsync(result.ISRC);
            result.AppleMusicUrl = appleTrackUrl;

            return result.ToTrackReturnDto();

        }
    }
}

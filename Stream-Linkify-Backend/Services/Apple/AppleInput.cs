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
        private readonly ISpotifyAlbumService spotifyAlbumService;
        private readonly IAppleTrackService appleTrackService;
        private readonly IAppleAlbumService appleAlbumService;
        private readonly ITidalTrackService tidalTrackService;
        private readonly ITidalAlbumService tidalAlbumService;

        public AppleInput(
            ILogger<AppleInput> logger,
            ISpotifyTrackService spotifyTrackService,
            ISpotifyAlbumService spotifyAlbumService,
            IAppleTrackService appleTrackService,
            IAppleAlbumService appleAlbumService,
            ITidalTrackService tidalTrackService,
            ITidalAlbumService tidalAlbumService
            )
        {
            this.logger = logger;
            this.spotifyTrackService = spotifyTrackService;
            this.spotifyAlbumService = spotifyAlbumService;
            this.appleTrackService = appleTrackService;
            this.appleAlbumService = appleAlbumService;
            this.tidalTrackService = tidalTrackService;
            this.tidalAlbumService = tidalAlbumService;
        }

        public async Task<AlbumReturnDto> GetAlbumUrlsAsync(string appleUrl)
        {
            AppleAlbumDataDto? appleAlbum = await appleAlbumService.GetByUrlAsync(appleUrl)
                ?? throw new InvalidOperationException($"Apple Music album not found for {appleUrl}");

            var result = new AlbumModel
            {
                UPC = appleAlbum.Attributes.Upc,
                AppleMusicUrl = appleAlbum.Attributes.Url,
                AlbumName = appleAlbum.Attributes.Name,
                AritstNames = [appleAlbum.Attributes.Name],
            };

            // Spotify

            var (spotifyAlbumUrl, spotifyArtistNames) = await spotifyAlbumService.GetByUpcAsync(result.UPC);
            result.SpotifyUrl = spotifyAlbumUrl;
            if (result.SpotifyUrl == null)
                logger.LogWarning("Spotify URL not found for UPC {UPC}", result.UPC);

            if (spotifyArtistNames != null && (result.AritstNames[0] == null || spotifyArtistNames.Count > 1))
                result.AritstNames = spotifyArtistNames;

            // Tidal
            result.TidalUrl = await tidalAlbumService.GetUrlByNameAsync(result.AlbumName, result.AritstNames.First(), result.UPC);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URl not found for UPC {UPC}", result.UPC);

            return result.ToAlbumReturnDto();
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

            // Spotify
            var (spotifyTrackUrl, spotifyAlbumName, spotifyArtistNames) = await spotifyTrackService.GetByIsrcAsync(result.ISRC);
            result.SpotifyUrl = spotifyTrackUrl;
            if (result.SpotifyUrl == null)
                logger.LogWarning("Spotify URL not found for ISRC {ISRC}", result.ISRC);

            if (spotifyAlbumName != null && result.AlbumName == null)
                result.AlbumName = spotifyAlbumName;

            if (spotifyArtistNames != null && (result.AritstNames[0] == null || spotifyArtistNames.Count > 1))
                result.AritstNames = spotifyArtistNames;

            // Tidal
            result.TidalUrl = await tidalTrackService.GetTrackUrlByNameAsync(result.SongName, result.AritstNames.First(), result.ISRC);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URL not found for ISRC {ISRC}", result.ISRC);

            return result.ToTrackReturnDto();
        }
    }
}

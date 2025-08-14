using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.DTOs.Spotify;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Mappers;
using Stream_Linkify_Backend.Models;
using Stream_Linkify_Backend.Services.Tidal;

namespace Stream_Linkify_Backend.Services.Spotify
{
    public class SpotifyInput : ISpotifyInput
    {
        private readonly ILogger<SpotifyInput> logger;
        private readonly ISpotifyTrackService spotifyTrackService;
        private readonly ISpotifyAlbumService spotifyAlbumService;
        private readonly IAppleTrackService appleTrackService;
        private readonly IAppleAlbumService appleAlbumService;
        private readonly ITidalTrackService tidalTrackService;
        private readonly ITidalAlbumService tidalAlbumService;

        public SpotifyInput(
            ILogger<SpotifyInput> logger,
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

        public async Task<AlbumReturnDto> GetAlbumUrlsAsync(string spotifyUrl)
        {
            SpotifyAlbumFullDto spotifyAlbum = await spotifyAlbumService.GetByUrlAsync(spotifyUrl)
                ?? throw new InvalidOperationException($"Spotify album not found for {spotifyUrl}");

            var result = new AlbumModel
            {
                UPC = spotifyAlbum.ExternalIds.Upc,
                SpotifyUrl = spotifyUrl,
                AritstNames = [.. spotifyAlbum.Artists.Select(a => a.Name)],
                AlbumName = spotifyAlbum.Name
            };

            // Apple
            result.AppleMusicUrl = await appleAlbumService.GetUrlByUpcAsync(result.UPC!);
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Apple Music URL not found for UPC {UPC}", result.UPC);

            // Tidal
            result.TidalUrl = await tidalAlbumService.GetUrlByNameAsync(result.AlbumName, result.AritstNames.FirstOrDefault()!, result.UPC!);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URL not found for UPC {UPC}", result.UPC);

            return result.ToAlbumReturnDto();
        }

        public async Task<TrackReturnDto> GetTrackUrlsAsync(string spotifyUrl)
        {

            // Get Spotify track
            SpotifyTrackFullDto? spotifyTrack = await spotifyTrackService.GetByUrlAsync(spotifyUrl)
                ?? throw new InvalidOperationException($"Spotify track not found for {spotifyUrl}");

            var result = new TrackModel
            {
                ISRC = spotifyTrack.ExternalIds.Isrc,
                SpotifyUrl = spotifyUrl,
                AritstNames = [.. spotifyTrack.Artists.Select(a => a.Name)],
                SongName = spotifyTrack.Name,
                AlbumName = spotifyTrack.Album.Name
            };

            // Get AppleMusic url by ISRC
            result.AppleMusicUrl = await appleTrackService.GetTrackUrlByIsrcAsync(result.ISRC!);
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Apple Music URL not found for ISRC {ISRC}", result.ISRC);


            // Get Tidal url from artist, track title, isrc
            result.TidalUrl = await tidalTrackService.GetTrackUrlByNameAsync(result.SongName, result.AritstNames.FirstOrDefault()!, result.ISRC!);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URL not found for ISRC {ISRC}", result.ISRC);

            return result.ToTrackReturnDto();
        }
    }
}


using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Interfaces;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Mappers;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalInput(
        ILogger<TidalInput> logger,
        IMusicServiceFactory musicServices,
        ITidalArtistService tidalArtistService
            ) : ITidalInput
    {
        private readonly ILogger<TidalInput> logger = logger;
        private readonly IMusicServiceFactory musicServices = musicServices;
        private readonly ITidalArtistService tidalArtistService = tidalArtistService;

        public async Task<AlbumReturnDto> GetAlbumUrlsAsync(string tidalUrl)
        {
            TidalAlbumResponseDto tidalAlbum = await musicServices.TidalAlbum.GetByUrlAsync(tidalUrl)
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
            var (spotifyUrl, spotifyArtistNames) = await musicServices.SpotifyAlbum.GetByNameAsync(result.UPC, result.AlbumName, result.AritstNames[0]);
            result.SpotifyUrl = spotifyUrl;
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Spotify URL not found for UPC {UPC}", result.UPC);

            if (spotifyArtistNames != null && result.AritstNames.Count == 0)
                result.AritstNames = spotifyArtistNames;

            // Apple
            result.AppleMusicUrl = await musicServices.AppleAlbum.GetUrlByNameAsync(result.UPC, result.AlbumName, result.AritstNames[0]);
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Apple Music URL not found for UPC {UPC}", result.UPC);


            return result.ToAlbumReturnDto();
        }

        public async Task<TrackReturnDto> GetTrackUrlsAsync(string tidalUrl)
        {
            TidalTrackResponseDto tidalTrack = await musicServices.TidalTrack.GetTrackByUrlAsync(tidalUrl)
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
            var(spotifyTrackUrl, spotifyAlbumName, spotifyArtistNames) = await musicServices.SpotifyTrack.GetByNameAsync(result.ISRC, result.SongName, result.AritstNames[0]);
            result.SpotifyUrl = spotifyTrackUrl;
            if (result.SpotifyUrl == null)
                logger.LogWarning("Spotify URL not found for ISRC {ISRC}", result.ISRC);

            if (spotifyAlbumName != null)
                result.AlbumName = spotifyAlbumName;

            if (spotifyArtistNames != null && result.AritstNames.Count == 0)
                result.AritstNames = spotifyArtistNames;

            // Apple
            result.AppleMusicUrl = await musicServices.AppleTrack.GetTrackUrlByNameAsync(result.ISRC!, result.SongName, result.AritstNames[0]);
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Apple Music URL not found for ISRC {ISRC}", result.ISRC);

            return result.ToTrackReturnDto();
        }
    }
}

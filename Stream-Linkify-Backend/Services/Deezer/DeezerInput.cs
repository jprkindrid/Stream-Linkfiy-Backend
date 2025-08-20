using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.DTOs.Deezer.Stream_Linkify_Backend.DTOs.Deezer;
using Stream_Linkify_Backend.Interfaces;
using Stream_Linkify_Backend.Interfaces.Deezer;
using Stream_Linkify_Backend.Mappers;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Services.Deezer
{
    public class DeezerInput(
        IMusicServiceFactory musicServices,
        ILogger<DeezerInput> logger
        ) : IDeezerInput
    {
        private readonly IMusicServiceFactory musicServices = musicServices;
        private readonly ILogger<DeezerInput> logger = logger;

        public async Task<AlbumReturnDto> GetAlbumUrlsAsync(string deezerUrl)
        {
            DeezerAlbumFullDto? deezerAlbum = await musicServices.DeezerAlbum.GetByUrlAsync(deezerUrl)
                ?? throw new InvalidOperationException($"No deezer album found for {deezerUrl}");

            var result = new AlbumModel
            {
                UPC = deezerAlbum.Upc,
                AlbumName = deezerAlbum.Title,
                AritstNames = [.. deezerAlbum.Contributors!.Select(x => x.Name)],
                DeezerUrl = deezerAlbum.Link
            };

            // Spotify
            var (spotifyUrl, _) = await musicServices.SpotifyAlbum.GetByNameAsync(result.UPC!, result.AlbumName, result.AritstNames[0]);
            result.SpotifyUrl = spotifyUrl;
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Spotify URL not found for UPC {UPC}", result.UPC);

            // Apple
            result.AppleMusicUrl = await musicServices.AppleAlbum.GetUrlByNameAsync(result.UPC!, result.AlbumName, result.AritstNames[0]);
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Apple Music URL not found for UPC {UPC}", result.UPC);

            // Tidal
            result.TidalUrl = await musicServices.TidalAlbum.GetUrlByNameAsync(result.AlbumName, result.AritstNames.FirstOrDefault()!, result.UPC!);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URL not found for UPC {UPC}", result.UPC);

            return result.ToAlbumReturnDto();
        }

        public async Task<TrackReturnDto> GetTrackUrlsAsync(string deezerUrl)
        {
            DeezerTrackFullDto? deezerTrack = await musicServices.DeezerTrack.GetByUrlAsync(deezerUrl)
                ?? throw new InvalidOperationException($"No deezer track found for {deezerUrl}");

            var result = new TrackModel
            {
                ISRC = deezerTrack.Isrc,
                SongName = deezerTrack.Title,
                AlbumName = deezerTrack.Album.Title,
                AritstNames = [.. deezerTrack.Contributors!.Select(x => x.Name)],
                DeezerUrl = deezerTrack.Link,
            };

            // Spotify
            var (spotifyTrackUrl, _, _) = await musicServices.SpotifyTrack.GetByNameAsync(result.ISRC!, result.SongName, result.AritstNames[0]);
            result.SpotifyUrl = spotifyTrackUrl;
            if (result.SpotifyUrl == null)
                logger.LogWarning("Spotify URL not found for ISRC {ISRC}", result.ISRC);

            // AppleMusic
            result.AppleMusicUrl = await musicServices.AppleTrack.GetTrackUrlByNameAsync(result.ISRC!, result.SongName, result.AritstNames[0]);
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Apple Music URL not found for ISRC {ISRC}", result.ISRC);

            // TIDAL
            result.TidalUrl = await musicServices.TidalTrack.GetTrackUrlByNameAsync(result.SongName, result.AritstNames.FirstOrDefault()!, result.ISRC!);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URL not found for ISRC {ISRC}", result.ISRC);

            return result.ToTrackReturnDto();

        }
    }
}

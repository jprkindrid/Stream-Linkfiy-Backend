using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.DTOs.Spotify;
using Stream_Linkify_Backend.Interfaces;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Mappers;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Services.Spotify
{
    public class SpotifyInput(
        ILogger<SpotifyInput> logger,
        IMusicServiceFactory musicServices
            ) : ISpotifyInput
    {
        private readonly ILogger<SpotifyInput> logger = logger;
        private readonly IMusicServiceFactory musicServices = musicServices;

        public async Task<AlbumReturnDto> GetAlbumUrlsAsync(string spotifyUrl)
        {
            SpotifyAlbumFullDto spotifyAlbum = await musicServices.SpotifyAlbum.GetByUrlAsync(spotifyUrl)
                ?? throw new InvalidOperationException($"Spotify album not found for {spotifyUrl}");

            var result = new AlbumModel
            {
                UPC = spotifyAlbum.ExternalIds.Upc,
                SpotifyUrl = spotifyUrl,
                AritstNames = [.. spotifyAlbum.Artists.Select(a => a.Name)],
                AlbumName = spotifyAlbum.Name
            };

            // Apple
            result.AppleMusicUrl = await musicServices.AppleAlbum.GetUrlByUpcAsync(result.UPC!);
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Apple Music URL not found for UPC {UPC}", result.UPC);

            // Tidal
            result.TidalUrl = await musicServices.TidalAlbum.GetUrlByNameAsync(result.AlbumName, result.AritstNames.FirstOrDefault()!, result.UPC!);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URL not found for UPC {UPC}", result.UPC);

            return result.ToAlbumReturnDto();
        }

        public async Task<TrackReturnDto> GetTrackUrlsAsync(string spotifyUrl)
        {

            // Get Spotify track
            SpotifyTrackFullDto? spotifyTrack = await musicServices.SpotifyTrack.GetByUrlAsync(spotifyUrl)
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
            result.AppleMusicUrl = await musicServices.AppleTrack.GetTrackUrlByNameAsync(result.ISRC!, result.SongName, result.AritstNames[0]);
            if (result.AppleMusicUrl == null)
                logger.LogWarning("Apple Music URL not found for ISRC {ISRC}", result.ISRC);


            // Get Tidal url from artist, track title, isrc
            result.TidalUrl = await musicServices.TidalTrack.GetTrackUrlByNameAsync(result.SongName, result.AritstNames.FirstOrDefault()!, result.ISRC!);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URL not found for ISRC {ISRC}", result.ISRC);

            return result.ToTrackReturnDto();
        }
    }
}


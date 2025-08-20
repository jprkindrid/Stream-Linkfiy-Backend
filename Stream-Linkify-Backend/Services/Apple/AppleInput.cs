using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.DTOs.Apple;
using Stream_Linkify_Backend.Interfaces;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Mappers;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Services.Apple
{
    public class AppleInput(
        ILogger<AppleInput> logger,
        IMusicServiceFactory musicServices
            ) : IAppleInput
    {
        private readonly ILogger<AppleInput> logger = logger;
        private readonly IMusicServiceFactory musicServices = musicServices;

        public async Task<AlbumReturnDto> GetAlbumUrlsAsync(string appleUrl)
        {
            AppleAlbumDataDto? appleAlbum = await musicServices.AppleAlbum.GetByUrlAsync(appleUrl)
                ?? throw new InvalidOperationException($"Apple Music album not found for {appleUrl}");

            var result = new AlbumModel
            {
                UPC = appleAlbum.Attributes.Upc,
                AppleMusicUrl = appleAlbum.Attributes.Url,
                AlbumName = appleAlbum.Attributes.Name,
                AritstNames = [appleAlbum.Attributes.ArtistName],
            };

            // Spotify

            var (spotifyAlbumUrl, spotifyArtistNames) = await musicServices.SpotifyAlbum.GetByNameAsync(result.UPC, result.AlbumName, result.AritstNames[0]);
            result.SpotifyUrl = spotifyAlbumUrl;
            if (result.SpotifyUrl == null)
                logger.LogWarning("Spotify URL not found for UPC {UPC}", result.UPC);

            if (spotifyArtistNames != null && (result.AritstNames[0] == null || spotifyArtistNames.Count > 1))
                result.AritstNames = spotifyArtistNames;

            // Tidal
            result.TidalUrl = await musicServices.TidalAlbum.GetUrlByNameAsync(result.AlbumName, result.AritstNames.First(), result.UPC);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URl not found for UPC {UPC}", result.UPC);

            // Deezer
            result.DeezerUrl = await musicServices.DeezerAlbum.GetByNameAsync(result.AlbumName, result.AritstNames.First());
            if (result.DeezerUrl == null)
                logger.LogWarning("Deeer URL not found for album {albumName} and primary artist {artistName}", result.AlbumName, result.AritstNames.First());

            return result.ToAlbumReturnDto();
        }

        public async Task<TrackReturnDto> GetTrackUrlsAsync(string appleUrl)
        {
            AppleSongDataDto? appleTrack = await musicServices.AppleTrack.GetTrackByUrlAsync(appleUrl)
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
            var (spotifyTrackUrl, spotifyAlbumName, spotifyArtistNames) = await musicServices.SpotifyTrack.GetByNameAsync(result.ISRC, result.SongName, result.AritstNames[0]);
            result.SpotifyUrl = spotifyTrackUrl;
            if (result.SpotifyUrl == null)
                logger.LogWarning("Spotify URL not found for ISRC {ISRC}", result.ISRC);

            if (spotifyAlbumName != null && result.AlbumName == null)
                result.AlbumName = spotifyAlbumName;

            if (spotifyArtistNames != null && (result.AritstNames[0] == null || spotifyArtistNames.Count > 1))
                result.AritstNames = spotifyArtistNames;

            // Tidal
            result.TidalUrl = await musicServices.TidalTrack.GetTrackUrlByNameAsync(result.SongName, result.AritstNames.First(), result.ISRC);
            if (result.TidalUrl == null)
                logger.LogWarning("TIDAL URL not found for ISRC {ISRC}", result.ISRC);

            // Deezer
            result.DeezerUrl = await musicServices.DeezerTrack.GetByNameAsync(result.SongName, result.AritstNames.First());
            if (result.DeezerUrl == null)
                logger.LogWarning("Deeer URL not found for track {trackName} and primary artist {artistName}", result.SongName, result.AritstNames.First());
            

            return result.ToTrackReturnDto();
        }
    }
}

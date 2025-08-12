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
        private readonly IAppleTrackService appleTrackService;
        private readonly ITidalTrackService tidalTrackService;

        public SpotifyInput(
            ILogger<SpotifyInput> logger,
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

        public Task<TrackReturnDto> GetAlbumUrlsAsync(string spotifyUrl)
        {
            throw new NotImplementedException();
        }

        public async Task<TrackReturnDto> GetTrackUrlsAsync(string spotifyUrl)
        {
            
            // Get Spotify track
            SpotifyTrackFullDto? spotifyTrack = await spotifyTrackService.GetByUrlAsync(spotifyUrl)
                ?? throw new InvalidOperationException("Spotify track not found");
            
            var result = new TrackModel
            {
                ISRC = spotifyTrack.ExternalIds.Isrc,
                SpotifyUrl = spotifyUrl,
                AritstNames = [.. spotifyTrack.Artists.Select(a => a.Name)],
                SongName = spotifyTrack.Name,
                AlbumName = spotifyTrack.Album.Name
            };

            // Get AppleMusic url by ISRC
            var appleTrackUrl = await appleTrackService.GetTrackUrlByIsrcAsync(result.ISRC!);
            result.AppleMusicUrl = appleTrackUrl;

            // Get Tidal url from artist, track title, isrc
            var tidalUrl = await tidalTrackService.GetTrackUrlByNameAsync(result.SongName, result.AritstNames.FirstOrDefault()!, result.ISRC!);
            result.TidalUrl = tidalUrl;

            return result.ToTrackReturnDo();
        }
    }
}


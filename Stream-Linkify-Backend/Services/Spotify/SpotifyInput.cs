using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.DTOs.Spotify;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Mappers;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Services.Spotify
{
    public class SpotifyInput : ISpotifyInput
    {
        private readonly ILogger<SpotifyInput> logger;
        private readonly ISpotifyTrackService spotifyTrackService;
        private readonly IAppleTrackService appleTrackService;

        public SpotifyInput(
            ILogger<SpotifyInput> logger,
            ISpotifyTrackService spotifyTrackService,
            IAppleTrackService appleTrackService
            )
        {
            this.logger = logger;
            this.spotifyTrackService = spotifyTrackService;
            this.appleTrackService = appleTrackService;
        }

        public async Task<TrackReturnDto> GetUrlsAsync(string spotifyUrl)
        {
            
            // Get Spotify track
            SpotifyTrackFullDto? spotifyTrack = await spotifyTrackService.GetByUrlAsync(spotifyUrl)
                ?? throw new InvalidOperationException("Spotify track not found");
            
            var result = new TrackModel
            {
                ISRC = spotifyTrack.ExternalIds.Isrc,
                SpotifyUrl = spotifyUrl,
                AristNames = [.. spotifyTrack.Artists.Select(a => a.Name)],
                SongName = spotifyTrack.Name,
                AlbumName = spotifyTrack.Album.Name
            };

            // Get AppleMusic track by ISRC
            var appleTrack = await appleTrackService.GetTrackByIsrcAsync(result.ISRC!);

            if (appleTrack == null)
            {
                logger.LogWarning("No Apple Music track found for ISRC {ISRC}", result.ISRC);
                result.AppleMusicUrl = null;
            }
            else
            {
                result.AppleMusicUrl = appleTrack.Attributes.Url;
            }

            return result.ToReturnDo();
        }
    }
}


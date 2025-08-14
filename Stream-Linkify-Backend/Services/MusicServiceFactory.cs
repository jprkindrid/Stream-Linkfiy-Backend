using Stream_Linkify_Backend.Interfaces;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Services
{
    public class MusicServiceFactory : IMusicServiceFactory
    {
        public MusicServiceFactory(
            ISpotifyTrackService spotifyTrack,
            ISpotifyAlbumService spotifyAlbum,
            IAppleTrackService appleTrack,
            IAppleAlbumService appleAlbum,
            ITidalTrackService tidalTrack,
            ITidalAlbumService tidalAlbum
            )
        {
            SpotifyTrack = spotifyTrack;
            SpotifyAlbum = spotifyAlbum;
            AppleTrack = appleTrack;
            AppleAlbum = appleAlbum;
            TidalTrack = tidalTrack;
            TidalAlbum = tidalAlbum;
        }
        public ISpotifyTrackService SpotifyTrack { get; }
        public ISpotifyAlbumService SpotifyAlbum { get; }
        public IAppleTrackService AppleTrack { get; }
        public IAppleAlbumService AppleAlbum { get; }
        public ITidalTrackService TidalTrack { get; }
        public ITidalAlbumService TidalAlbum { get; }
    }
}

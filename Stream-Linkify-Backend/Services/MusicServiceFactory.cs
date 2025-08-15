using Stream_Linkify_Backend.Interfaces;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Services
{
    public class MusicServiceFactory(
        ISpotifyTrackService spotifyTrack,
        ISpotifyAlbumService spotifyAlbum,
        IAppleTrackService appleTrack,
        IAppleAlbumService appleAlbum,
        ITidalTrackService tidalTrack,
        ITidalAlbumService tidalAlbum
            ) : IMusicServiceFactory
    {
        public ISpotifyTrackService SpotifyTrack { get; } = spotifyTrack;
        public ISpotifyAlbumService SpotifyAlbum { get; } = spotifyAlbum;
        public IAppleTrackService AppleTrack { get; } = appleTrack;
        public IAppleAlbumService AppleAlbum { get; } = appleAlbum;
        public ITidalTrackService TidalTrack { get; } = tidalTrack;
        public ITidalAlbumService TidalAlbum { get; } = tidalAlbum;
    }
}

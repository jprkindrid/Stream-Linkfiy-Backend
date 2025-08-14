using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Interfaces
{
    public interface IMusicServiceFactory
    {
        // Spotify
        ISpotifyTrackService SpotifyTrack { get; }
        ISpotifyAlbumService SpotifyAlbum { get; }

        // Apple
        IAppleTrackService AppleTrack { get; }
        IAppleAlbumService AppleAlbum { get; }

        // TIDAL
        ITidalTrackService TidalTrack { get; }
        ITidalAlbumService TidalAlbum { get; }
    }
}

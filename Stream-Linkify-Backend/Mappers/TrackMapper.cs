using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Mappers
{
    public static class TrackMapper
    {
        public static TrackReturnDto ToReturnDo(this TrackModel track)
        {
            return new TrackReturnDto
            {
                ArtistNames = track.AritstNames,
                SongName = track.SongName,
                AlbumName = track.AlbumName,
                Spotify = track.SpotifyUrl,
                AppleMusic = track.AppleMusicUrl,
                Tidal = track.TidalUrl
            };
        }
    }
}

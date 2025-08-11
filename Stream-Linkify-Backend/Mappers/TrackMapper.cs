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
                ArtistNames = track.AristNames,
                SongName = track.SongName,
                AlbumName = track.AlbumName,
                SpotifyUrl = track.SpotifyUrl,
                AppleMusicUrl = track.AppleMusicUrl,
                TidalUrl = track.TidalUrl
            };
        }
    }
}

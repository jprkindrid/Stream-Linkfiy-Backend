using System.Text.Json;

namespace Stream_Linkify_Backend.Models
{
    public class TrackModel
    {
        public string? ISRC { get; set; }
        public required List<string> AritstNames { get; set; }
        public required string SongName { get; set; }

        public string? AlbumName { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? AppleMusicUrl { get; set; }
        public string? TidalUrl { get; set; }
    }
}

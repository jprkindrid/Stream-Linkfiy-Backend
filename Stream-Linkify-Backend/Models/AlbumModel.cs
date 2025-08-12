using System.Text.Json;

namespace Stream_Linkify_Backend.Models
{
    public class AlbumModel
    {
        public string? UPC { get; set; }
        public required List<string> AritstNames { get; set; }
        public string? AlbumName { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? AppleMusicUrl { get; set; }
        public string? TidalUrl { get; set; }
    }
}

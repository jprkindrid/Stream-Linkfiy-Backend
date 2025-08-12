namespace Stream_Linkify_Backend.DTOs
{
    public class AlbumReturnDto
    {
        public required List<string> ArtistNames { get; set; }
        public string? AlbumName { get; set; }
        public string? Spotify { get; set; }
        public string? AppleMusic { get; set; }
        public string? Tidal { get; set; }
    }
}

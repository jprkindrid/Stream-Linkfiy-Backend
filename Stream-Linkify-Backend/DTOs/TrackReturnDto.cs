namespace Stream_Linkify_Backend.DTOs
{
    public class TrackReturnDto
    {
        public required List<string> ArtistNames { get; set; }
        public required string SongName { get; set; }
        public string? AlbumName { get; set; }
        public string? Spotify { get; set; }
        public string? AppleMusic { get; set; }
        public string? Tidal { get; set; }
        public string? Deezer { get; set; }
    }
}

namespace Stream_Linkify_Backend.DTOs
{
    public class TrackReturnDto
    {
        public required List<string> ArtistNames { get; set; }
        public required string SongName { get; set; }
        public string? AlbumName { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? AppleMusicUrl { get; set; }
    }
}

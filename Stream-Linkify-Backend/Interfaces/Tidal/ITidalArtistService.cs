namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalArtistService
    {
        public Task<List<string?>> GetTrackArtistNamesAsync(string trackID);
    }
}

using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalTrackService : ITidalTrackService

    {
        public Task<TidalSongDataDto?> GetTrackByUrlAsync(string tidalUrl)
        {
            throw new NotImplementedException();
        }

        public Task<TidalSongDataDto?> GetTrackByNameAsync(string trackName, string artistName, string? albumName)
        {
            throw new NotImplementedException();
        }


    }
}

using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Interfaces.Tidal;

namespace Stream_Linkify_Backend.Services.Tidal
{
    public class TidalAlbumService : ITidalAlbumService
    {
        private readonly ITidalApiClient tidalApiClient;
        private readonly ILogger<TidalAlbumService> logger;

        public TidalAlbumService(
            ITidalApiClient tidalApiClient,
            ILogger<TidalAlbumService> logger
            )
        {
            this.tidalApiClient = tidalApiClient;
            this.logger = logger;
        }

        // TODO: IMPLEMENT THESE
        public Task<TidalAlbumData?> GetByUrlAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetUrlByUpcAsync(string upc)
        {
            throw new NotImplementedException();
        }
    }
}

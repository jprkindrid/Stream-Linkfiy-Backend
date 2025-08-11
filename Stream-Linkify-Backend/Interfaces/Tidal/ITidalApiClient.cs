namespace Stream_Linkify_Backend.Interfaces.Tidal
{
    public interface ITidalApiClient
    {
        Task<T?> SendTidalRequestAsync<T>(string reqUrl);
    }
}

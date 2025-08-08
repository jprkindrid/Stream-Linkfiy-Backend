namespace Stream_Linkify_Backend.Interfaces.Spotify
{
    public interface ISpotifyApiClient
    {
        Task<T?> SendSpotifyRequestAsync<T>(string reqUrl);
    }
}

namespace Stream_Linkify_Backend.Interfaces.Deezer
{
    public interface IDeezerApiClient
    {
        Task<T?> SendDeezerRequestAsync<T>(string reqUrl);
    }
}

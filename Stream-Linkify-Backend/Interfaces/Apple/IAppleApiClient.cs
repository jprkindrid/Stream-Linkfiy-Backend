namespace Stream_Linkify_Backend.Interfaces.Apple
{
    public interface IAppleApiClient
    {
        Task<T?> SendAppleRequestAsync<T>(string reqUrl);
    }
}

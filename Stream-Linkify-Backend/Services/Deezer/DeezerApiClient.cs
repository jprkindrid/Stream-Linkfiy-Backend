using Stream_Linkify_Backend.Interfaces.Deezer;
using Stream_Linkify_Backend.Services.Spotify;

namespace Stream_Linkify_Backend.Services.Deezer
{
    public class DeezerApiClient(
        IHttpClientFactory httpClientFactory,
        ILogger<DeezerApiClient> logger) : IDeezerApiClient
    {
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly ILogger<DeezerApiClient> logger = logger;
        private readonly SemaphoreSlim sem = new(10, 10);

        public async Task<T?> SendDeezerRequestAsync<T>(string reqUrl)
        {
            await sem.WaitAsync();
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, reqUrl);

                var client = httpClientFactory.CreateClient();
                string reqMessage = $"Making a Deezer api request at the path '{reqUrl}'";
                logger.LogInformation(reqMessage);

                var resp = await client.SendAsync(req);
                resp.EnsureSuccessStatusCode();

                var result = await resp.Content.ReadFromJsonAsync<T>();

                return result;
            }
            catch (Exception ex)
            {
                string exMessage = $"error getting making Deezer API request at {reqUrl}";
                logger.LogError(ex, exMessage);
                return default;
            }
            finally
            {
                sem.Release();
            }
        }
    }
}

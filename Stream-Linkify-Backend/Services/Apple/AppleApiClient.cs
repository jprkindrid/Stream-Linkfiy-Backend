using Microsoft.Extensions.Logging;
using Stream_Linkify_Backend.Interfaces.Apple;
using System.Net.Http;

namespace Stream_Linkify_Backend.Services.Apple
{
    public class AppleApiClient : IAppleApiClient
    {
        private readonly IAppleTokenService appleTokenService;
        private readonly ILogger<AppleApiClient> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly SemaphoreSlim sem = new(10, 10);

        public AppleApiClient(
            IAppleTokenService appleTokenService,
            ILogger<AppleApiClient> logger,
            IHttpClientFactory httpClientFactory)
        {
            this.appleTokenService = appleTokenService;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<T?> SendAppleRequestAsync<T>(string reqUrl)
        {
            await sem.WaitAsync();
            try
            {
                var aToken = appleTokenService.GetValidToken()
                    ?? throw new InvalidOperationException("error getting apple music jwt");

                var req = new HttpRequestMessage(HttpMethod.Get, reqUrl);
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", aToken);

                var client = httpClientFactory.CreateClient();
                logger.LogInformation("Making an Apple Music API request at '{ReqUrl}'", reqUrl);

                var resp = await client.SendAsync(req);
                resp.EnsureSuccessStatusCode();

                var result = await resp.Content.ReadFromJsonAsync<T>()
                    ?? throw new InvalidOperationException($"Error deserializing Apple Music response for {reqUrl}");



                return result;
            }
            catch (Exception ex)
            {
                string exMessage = $"error getting making Apple Music API request at {reqUrl}";
                logger.LogError(ex, exMessage);
                return default;

            }
            finally { sem.Release(); }
        }
    }
}

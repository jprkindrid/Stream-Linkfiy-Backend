namespace Stream_Linkify_Backend.Helpers.URLs
{
    public static class DeezerUrlHelper
    {
        public static async Task<string> ExtractDeezerId(string deezerUrl)
        {

            if (!Uri.TryCreate(deezerUrl, UriKind.Absolute, out var uri))
                throw new ArgumentException("Invalid URL format");

            if (uri.Host == "link.deezer.com")
            {
                // This is a share link we need to convert to a standard deezer link
                using var client = new HttpClient(new HttpClientHandler() { AllowAutoRedirect = false });
                var resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, deezerUrl))
                    ?? throw new InvalidOperationException($"Unexpected response from Deezer: null");

                if (resp.StatusCode == System.Net.HttpStatusCode.Found || 
                    resp.StatusCode == System.Net.HttpStatusCode.Redirect)
                {
                    var finalUrl = resp.Headers?.Location?.ToString();
                    if (!Uri.TryCreate(finalUrl, UriKind.Absolute, out uri))
                        throw new InvalidOperationException($"Unexpected response from Deezer: {resp.StatusCode}");
                } 
                else
                {
                    throw new InvalidOperationException($"Unexpected response from Deezer: {resp.StatusCode}");
                }
            }

            var pathParts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.ToLowerInvariant())
                .ToArray(); 
            // region, track/album, id
            var id = pathParts[2];

            if (!int.TryParse(id, out _))
                throw new ArgumentException("Invalid Deezer URL, id should be number");

            return id;
        }
    }
}

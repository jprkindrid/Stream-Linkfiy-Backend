namespace Stream_Linkify_Backend.Helpers.URLs
{
    public static class SpotifyUrlHelper
    {
        public static string ExtractSpotifyId(string spotifyUrl, string expectedType)
        {
            if (!Uri.TryCreate(spotifyUrl, UriKind.Absolute, out var uri))
                throw new ArgumentException("Invalid URL format");

            if (uri.Host == "open.spotify.com")
            {
                var pathParts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length >= 2 && pathParts[0] == expectedType)
                    return pathParts[1];
            }
            else if (uri.Scheme == "spotify")
            {
                var parts = spotifyUrl.Split(':');
                if (parts.Length == 3 && parts[1] == expectedType)
                    return parts[2];
            }

            throw new ArgumentException($"Not a valid Spotify {expectedType} URL");
        }
    }
}
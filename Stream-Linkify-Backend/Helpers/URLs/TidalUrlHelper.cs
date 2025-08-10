namespace Stream_Linkify_Backend.Helpers.URLs
{
    public static class TidalUrlHelper
    {
        public static string ExtractTidalId(string tidalUrl, string expectedType)
        {
            // examples
            // https://listen.tidal.com/album/430298609/track/430298614
            // https://tidal.com/browse/track/430298612?u
            // https://tidal.com/browse/album/430298609
            // https://tidal.com/browse/album/430298609?u

            if (!Uri.TryCreate(tidalUrl, UriKind.Absolute, out var uri))
                throw new ArgumentException("Invalid URL format");

            var host = uri.Host.ToLowerInvariant();
            if (host != "tidal.com" && host != "listen.tidal.com")
                throw new ArgumentException("Not a valid Tidal URL");

            var pathParts = uri.AbsolutePath
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.ToLowerInvariant())
                .ToArray();

            var result = "";

            if (pathParts.Length == 4 && pathParts[0] == "album" && pathParts[2] == "track")
            {
                result = expectedType.ToLowerInvariant() switch
                {
                    "album" => pathParts[1],
                    "track" => pathParts[3],
                    _ => throw new ArgumentException("Invalid expected type")
                };
            }
            else if (pathParts.Length == 3 && pathParts[0] == "browse")
            {
                if (!pathParts[1].Equals(expectedType, StringComparison.InvariantCultureIgnoreCase))
                    throw new ArgumentException($"Expected type '{expectedType}' but found '{pathParts[1]}'");

                result = pathParts[2];
            }
            else if (pathParts.Length == 2)
            {
                if (!pathParts[0].Equals(expectedType, StringComparison.InvariantCultureIgnoreCase))
                    throw new ArgumentException($"Expected type '{expectedType}' but found '{pathParts[0]}'");

                result = pathParts[1];
            }

            if (string.IsNullOrEmpty(result) || !int.TryParse(result, out var _))
                throw new ArgumentException($"Not a valid Tidal {expectedType} URL");

            return result;
        }
    }
}
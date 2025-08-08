using Stream_Linkify_Backend.DTOs.Spotify;
using Stream_Linkify_Backend.Helpers.URLs;
using Stream_Linkify_Backend.Interfaces.Spotify;

namespace Stream_Linkify_Backend.Services.Spotify
{
    public class SpotifyAlbumService : ISpotifyAlbumService
    {
        private const string spotifyApiAlbumUrl = "https://api.spotify.com/v1/albums";
        private readonly ISpotifyApiClient spotifyApiClient;

        public SpotifyAlbumService(
           ISpotifyApiClient spotifyApiClient)
        {
            this.spotifyApiClient = spotifyApiClient;
        }
        public async Task<SpotifyAlbumFullDto?> GetByUrlAsync(string spotifyUrl)
        {
            var albumID = SpotifyUrlHelper.ExtractSpotifyId(spotifyUrl, "album");
            var reqUrl = $"{spotifyApiAlbumUrl}/{albumID}";

            var result = await spotifyApiClient.SendSpotifyRequestAsync<SpotifyAlbumFullDto>(reqUrl);

            return result;
        }
    }
}

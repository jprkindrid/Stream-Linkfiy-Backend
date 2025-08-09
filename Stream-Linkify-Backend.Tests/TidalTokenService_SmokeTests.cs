using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Services.Tidal;
using System.Threading.Tasks;
using Xunit;

namespace Stream_Linkify_Backend.Tests
{
    public class TidalTokenService_SmokeTests
    {
        private readonly ServiceProvider _serviceProvider;

        public TidalTokenService_SmokeTests()
        {
            var services = new ServiceCollection();

            // Logging & HTTP
            services.AddLogging(b => b.AddConsole());
            services.AddHttpClient();

            // Config from user-secrets + env vars
            var config = new ConfigurationBuilder()
                .AddUserSecrets<TidalTokenService_SmokeTests>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(config);

            // Core Tidal services
            services.AddSingleton<ITidalTokenService, TidalTokenService>();


            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task GetValidTokenAsync_ReturnsToken()
        {
            var svc = _serviceProvider.GetRequiredService<ITidalTokenService>();

            var token = await svc.GetValidTokenAsync();

            Assert.NotNull(token);
            Assert.False(string.IsNullOrWhiteSpace(token!.AccessToken));
        }

        //[Fact]
        //public async Task GetValidTrackAsync_ReturnsTrack()
        //{
        //    var trackService = _serviceProvider.GetRequiredService<ISpotifyTrackService>();

        //    var exampleTrack = "https://open.spotify.com/track/43eLl2gwEr0fgbFgS11uOh?si=220d34b2d78b4ca7";

        //    var track = await trackService.GetByUrlAsync(exampleTrack);

        //    Assert.NotNull(track);
        //    Assert.False(string.IsNullOrWhiteSpace(track!.Name));
        //}

        //[Fact]
        //public async Task GetValidAlbumAsync_ReturnsAlbum()
        //{
        //    var albumService = _serviceProvider.GetRequiredService<ISpotifyAlbumService>();

        //    var exampleAlbum = "https://open.spotify.com/album/27teXombBxDGNa9f5jtOr2?si=R6dKhp2MSc-jKFirPkZzLA";

        //    var album = await albumService.GetByUrlAsync(exampleAlbum);

        //    Assert.NotNull(album);
        //    Assert.False(string.IsNullOrWhiteSpace(album!.Name));
        //}
    }
}
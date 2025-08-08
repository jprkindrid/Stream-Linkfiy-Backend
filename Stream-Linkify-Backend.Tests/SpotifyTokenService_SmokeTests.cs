using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stream_Linkfiy_Backend.Interfaces.Spotify;
using Stream_Linkfiy_Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stream_Linkify_Backend.Tests
{
    public class SpotifyTokenService_SmokeTests
    {
        [Fact]
        public async Task GetValidTokenAsync_ReturnsToken()
        {
            var services = new ServiceCollection();
            services.AddLogging(b => b.AddConsole());
            services.AddHttpClient();

            var config = new ConfigurationBuilder()
                .AddUserSecrets<SpotifyTokenService_SmokeTests>(optional: true).AddEnvironmentVariables().Build();

            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<ISpotifyTokenService, SpotifyTokenService>();

            var servicesProvider = services.BuildServiceProvider();
            var svc = servicesProvider.GetRequiredService<ISpotifyTokenService>();

            var token = await svc.GetValidTokenAsync();

            Assert.NotNull(token);
            Assert.False(string.IsNullOrWhiteSpace(token!.AccessToken));
        }

        [Fact]
        public async Task GetValidTrackAsync_ReturnsTrack()
        {
            var services = new ServiceCollection();
            services.AddLogging(b => b.AddConsole());
            services.AddHttpClient();

            var config = new ConfigurationBuilder()
                .AddUserSecrets<SpotifyTokenService_SmokeTests>(optional: true).AddEnvironmentVariables().Build();

            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<ISpotifyTokenService, SpotifyTokenService>();
            services.AddSingleton<ISpotifyTrackService, SpotifyTrackService>();

            var servicesProvider = services.BuildServiceProvider();
            var trackService = servicesProvider.GetRequiredService<ISpotifyTrackService>();

            var exampleTrack = "https://open.spotify.com/track/43eLl2gwEr0fgbFgS11uOh?si=220d34b2d78b4ca7";

            var track = await trackService.GetByUrlAsync(exampleTrack);

            Assert.NotNull(track);
            Assert.False(string.IsNullOrWhiteSpace(track!.Name));
        }
        [Fact]
        public async Task GetValidAlbumAsync_ReturnsAlbum()
        {
            var services = new ServiceCollection();
            services.AddLogging(b => b.AddConsole());
            services.AddHttpClient();

            var config = new ConfigurationBuilder()
                .AddUserSecrets<SpotifyTokenService_SmokeTests>(optional: true).AddEnvironmentVariables().Build();

            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<ISpotifyTokenService, SpotifyTokenService>();
            services.AddSingleton<ISpotifyAlbumService, SpotifyAlbumService>();

            var servicesProvider = services.BuildServiceProvider();
            var albumService = servicesProvider.GetRequiredService<ISpotifyAlbumService>();

            var exampleTrack = "https://open.spotify.com/album/27teXombBxDGNa9f5jtOr2?si=R6dKhp2MSc-jKFirPkZzLA";

            var album = await albumService.GetByUrlAsync(exampleTrack);

            Assert.NotNull(album);
            Assert.False(string.IsNullOrWhiteSpace(album!.Name));
        }
    }
}

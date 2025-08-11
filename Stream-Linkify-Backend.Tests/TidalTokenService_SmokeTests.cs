﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stream_Linkify_Backend.DTOs.Tidal;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Services.Tidal;
using System.Threading.Tasks;
using Xunit;

namespace Stream_Linkify_Backend.Tests
{
    public class TidalService_SmokeTests
    {
        private readonly ServiceProvider _serviceProvider;

        public TidalService_SmokeTests()
        {
            var services = new ServiceCollection();

            // Logging & HTTP
            services.AddLogging(b => b.AddConsole());
            services.AddHttpClient();

            // Config from user-secrets + env vars
            var config = new ConfigurationBuilder()
                .AddUserSecrets<TidalService_SmokeTests>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(config);

            // Core TIDAL services
            services.AddSingleton<ITidalTokenService, TidalTokenService>();
            services.AddSingleton<ITidalApiClient, TidalApiClient>();
            services.AddSingleton<ITidalTrackService, TidalTrackService>();
            //services.AddSingleton<ITidalAlbumService, TidalAlbumService>();

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

        [Fact]
        public async Task GetValidTrackAsync_ReturnsTrack()
        {
            var trackService = _serviceProvider.GetRequiredService<ITidalTrackService>();

            // Example TIDAL track URL
            var exampleTrack = "https://tidal.com/browse/track/430298612?u";

            TidalTrackResponseDto? track = await trackService.GetTrackByUrlAsync(exampleTrack);

            Assert.NotNull(track);
            Assert.False(string.IsNullOrWhiteSpace(track.Data.Attributes.Title));
        }

        [Fact]
        public async Task GetTrackByUrlAsync_ReturnsTrack()
        {
            var trackService = _serviceProvider.GetRequiredService<ITidalTrackService>();

            var testUrl = "https://tidal.com/browse/track/430298612?u";

            var track = await trackService.GetTrackByUrlAsync(testUrl);

            Assert.NotNull(track);
            Assert.False(string.IsNullOrWhiteSpace(track.Data.Attributes.Isrc));
            Assert.False(string.IsNullOrWhiteSpace(track.Data.Attributes.Title));
        }
        [Fact]
        public async Task GetTrackByNameAsync_ReturnsTrack()
        {
            var trackService = _serviceProvider.GetRequiredService<ITidalTrackService>();

            var testArtist = "Makay";
            var testTrack = "Drifting Dawn";
            var testIsrc = "GBWUL2540497";

            var trackUrl = await trackService.GetTrackUrlByNameAsync(testTrack, testArtist, testIsrc);

            Assert.False(string.IsNullOrWhiteSpace(trackUrl));
        }

        //[Fact]
        //public async Task GetValidAlbumAsync_ReturnsAlbum()
        //{
        //    var albumService = _serviceProvider.GetRequiredService<ITidalAlbumService>();

        //    // Example TIDAL album URL
        //    var exampleAlbum = "https://tidal.com/browse/album/445978727";

        //    var album = await albumService.GetAlbumByUrlAsync(exampleAlbum);

        //    Assert.NotNull(album);
        //    Assert.False(string.IsNullOrWhiteSpace(album!.Attributes.Title));
        //}
    }
}
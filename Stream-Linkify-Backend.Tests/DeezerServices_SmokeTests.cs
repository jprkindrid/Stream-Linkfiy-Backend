using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stream_Linkify_Backend.DTOs.Deezer;
using Stream_Linkify_Backend.DTOs.Deezer.Stream_Linkify_Backend.DTOs.Deezer;
using Stream_Linkify_Backend.Interfaces.Deezer;
using Stream_Linkify_Backend.Services.Deezer;
using System.Threading.Tasks;
using Xunit;

namespace Stream_Linkify_Backend.Tests
{
    public class DeezerService_SmokeTests
    {
        private readonly ServiceProvider _serviceProvider;

        public DeezerService_SmokeTests()
        {
            var services = new ServiceCollection();

            services.AddLogging(b => b.AddConsole());
            services.AddHttpClient();

            services.AddSingleton<IDeezerApiClient, DeezerApiClient>();
            services.AddSingleton<IDeezerTrackService, DeezerTrackService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task GetValidTrackByUrlAsync_ReturnsTrack()
        {
            var trackService = _serviceProvider.GetRequiredService<IDeezerTrackService>();

            var testUrl = "https://www.deezer.com/us/track/3135556";

            DeezerTrackFullDto? track = await trackService.GetByUrlAsync(testUrl);

            Assert.NotNull(track);
            Assert.Equal("Harder, Better, Faster, Stronger", track!.Title);
            Assert.Equal("Daft Punk", track.Artist.Name);
            Assert.False(string.IsNullOrWhiteSpace(track.Isrc));
        }
        [Fact]
        public async Task GetValidTrackByNameAsync_ReturnsTrack()
        {
            var trackService = _serviceProvider.GetRequiredService<IDeezerTrackService>();

            var artist = "Kindrid";
            var track = "No Pressure";

            string? result = await trackService.GetByNameAsync(track, artist);

            Assert.NotNull(result);
            Assert.Equal("https://www.deezer.com/track/3326228661", result);
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stream_Linkfiy_Backend.Interfaces;
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
    }
}

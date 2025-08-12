using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Services.Apple;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;

namespace Stream_Linkify_Backend.Tests
{
    public class AppleService_SmokeTests
    {
        private readonly IConfiguration _config;
        private readonly ServiceProvider _serviceProvider;

        public AppleService_SmokeTests()
        {
            var services = new ServiceCollection();

            _config = new ConfigurationBuilder()
                .AddUserSecrets<AppleService_SmokeTests>() // Make sure secrets contain AppleMusicKit keys
                .Build();

            services.AddSingleton<IConfiguration>(_config);

            // Logging & HTTP
            services.AddLogging(b => b.AddConsole());
            services.AddHttpClient();

            // Core Apple services
            services.AddSingleton<IAppleApiClient, AppleApiClient>();
            services.AddSingleton<IAppleTokenService, AppleTokenService>();
            services.AddScoped<IAppleTrackService, AppleTrackService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task GetValidTokenAsync_ReturnsToken()
        {
            var svc = _serviceProvider.GetRequiredService<IAppleTokenService>();

            var token = await Task.FromResult(svc.GetValidToken()); // Apple token is sync in your code

            Assert.NotNull(token);
            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public void GetValidToken_ShouldProduceValidJwt()
        {
            var service = new AppleTokenService(_config, NullLogger<AppleTokenService>.Instance);

            var token = service.GetValidToken();

            Assert.False(string.IsNullOrWhiteSpace(token));

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // Header checks
            Assert.Equal("ES256", jwt.Header["alg"]);
            Assert.Equal(_config["AppleMusicKit:KeyId"], jwt.Header["kid"]);

            // Payload checks
            Assert.Equal(_config["AppleMusicKit:TeamId"], jwt.Payload["iss"]);

            var iat = Convert.ToInt64(jwt.Payload["iat"]);
            var exp = Convert.ToInt64(jwt.Payload["exp"]);

            var issuedAt = DateTimeOffset.FromUnixTimeSeconds(iat);
            var expiresAt = DateTimeOffset.FromUnixTimeSeconds(exp);

            Assert.True(issuedAt <= DateTimeOffset.UtcNow.AddMinutes(1), "IssuedAt is too far in the future");
            Assert.True(expiresAt > issuedAt, "Expiration must be after IssuedAt");

            var maxLifetimeSeconds = 15777000; // Apple's max allowed
            var actualLifetimeSeconds = exp - iat;

            Assert.True(actualLifetimeSeconds <= maxLifetimeSeconds,
                $"Expiration must be <= {maxLifetimeSeconds} seconds (~6 months)");
        }

        [Fact]
        public async Task GetTrackByUrlAsync_ReturnsTrack()
        {
            var trackService = _serviceProvider.GetRequiredService<IAppleTrackService>();

            var testTrackUrl = "https://music.apple.com/us/album/wounded/1825854595?i=1825854596";

            var track = await trackService.GetTrackByUrlAsync(testTrackUrl);

            Assert.NotNull(track);
            Assert.False(string.IsNullOrWhiteSpace(track!.Attributes.Name));
            Assert.False(string.IsNullOrWhiteSpace(track.Attributes.ArtistName));
            Assert.False(string.IsNullOrWhiteSpace(track.Attributes.Isrc));
        }

        [Fact]
        public async Task GetTrackByIsrcAsync_ReturnsTrack()
        {
            var trackService = _serviceProvider.GetRequiredService<IAppleTrackService>();

            var testIsrc = "GBRKQ2482423";

            var track = await trackService.GetTrackByUrlAsync(testIsrc);

            Assert.NotNull(track);
        }
    }
}
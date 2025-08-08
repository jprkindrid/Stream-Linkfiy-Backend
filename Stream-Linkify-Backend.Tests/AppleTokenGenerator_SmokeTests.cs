using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Stream_Linkfiy_Backend.Services;
using Xunit;

namespace Stream_Linkify_Backend.Tests
{
    public class AppleTokenService_SmokeTests
    {
        private readonly IConfiguration _config;

        public AppleTokenService_SmokeTests()
        {
            // Load config from user-secrets for testing
            _config = new ConfigurationBuilder()
                .AddUserSecrets<AppleTokenService_SmokeTests>()
                .Build();
        }

        [Fact]
        public void GetValidToken_ShouldProduceValidJwt()
        {
            // Arrange
            var service = new AppleTokenService(_config, NullLogger<AppleTokenService>.Instance);

            // Act
            var token = service.GetValidToken();

            // Assert - token is not null or empty
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
    }
}
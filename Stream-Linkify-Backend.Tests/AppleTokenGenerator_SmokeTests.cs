using Microsoft.Extensions.Configuration;
using Stream_Linkfiy_Backend.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stream_Linkify_Backend.Tests
{
    public class AppleTokenGenerator_SmokeTests
    {
        private readonly IConfiguration _config;

        public AppleTokenGenerator_SmokeTests()
        {
            // Load config from user-secrets for testing
            _config = new ConfigurationBuilder()
                .AddUserSecrets<AppleTokenGenerator_SmokeTests>()
                .Build();
        }

        [Fact]
        public void GenerateDeveloperToken_ShouldProduceValidJwt()
        {
            var generator = new AppleTokenGenerator(_config);

            var token = generator.GenerateDeveloperToken();

            Assert.False(string.IsNullOrWhiteSpace(token));

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Assert.Equal("ES256", jwt.Header["alg"]);
            Assert.Equal(_config["AppleMusicKit:KeyId"], jwt.Header["kid"]);

            Assert.Equal(_config["AppleMusicKit:TeamId"], jwt.Payload["iss"]);

            var iat = Convert.ToInt64(jwt.Payload["iat"]);
            var exp = Convert.ToInt64(jwt.Payload["exp"]);

            var issuedAt = DateTimeOffset.FromUnixTimeSeconds(iat);
            var expiresAt = DateTimeOffset.FromUnixTimeSeconds(exp);

            Assert.True(issuedAt <= DateTimeOffset.UtcNow.AddMinutes(1), "IssuedAt is too far in the future");
            Assert.True(expiresAt > issuedAt, "Expiration must be after IssuedAt");
            var maxLifetimeSeconds = 15777000; // Apple's max allowed
            var actualLifetimeSeconds = (exp - iat);

            Assert.True(actualLifetimeSeconds <= maxLifetimeSeconds,
                $"Expiration must be <= {maxLifetimeSeconds} seconds (~6 months)");
        }
    }
}

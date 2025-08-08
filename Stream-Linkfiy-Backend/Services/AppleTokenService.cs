using Microsoft.IdentityModel.Tokens;
using Stream_Linkfiy_Backend.Helpers;
using Stream_Linkfiy_Backend.Interfaces.Apple;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Stream_Linkfiy_Backend.Services
{
    public class AppleTokenService : IAppleTokenService
    {
        private string? token;
        private long? expiresAt;
        private readonly IConfiguration config;
        private readonly ILogger<AppleTokenService> logger;
        private readonly Lock lockObj = new();

        public AppleTokenService(IConfiguration config, ILogger<AppleTokenService> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public string GetValidToken()
        {
            lock (lockObj)
            {
                if (token == null || !IsValidToken())
                {
                    logger.LogInformation("Generating new Apple Music developer token");
                    (token, expiresAt) = GenerateDeveloperToken();
                }

                return token!;
            }
        }

        private (string token, long expiresAt) GenerateDeveloperToken()
        {
            var teamId = RequiredConfig.Get(config, "AppleMusicKit:TeamId");
            var keyId = RequiredConfig.Get(config, "AppleMusicKit:KeyId");
            var privateKeyPem = RequiredConfig.Get(config, "AppleMusicKit:PrivateKey");

            privateKeyPem = privateKeyPem
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Trim();

            var privateKeyBytes = Convert.FromBase64String(privateKeyPem);

            using var ecdsa = ECDsa.Create();
            ecdsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

            var creds = new SigningCredentials(
                new ECDsaSecurityKey(ecdsa) { KeyId = keyId },
                SecurityAlgorithms.EcdsaSha256
            );

            var now = DateTimeOffset.UtcNow;
            var expires = now.AddSeconds(15777000); // Apple's max lifetime (~6 months)

            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = teamId,
                NotBefore = now.UtcDateTime,
                IssuedAt = now.UtcDateTime,
                Expires = expires.UtcDateTime,
                SigningCredentials = creds
            };

            var jwt = handler.CreateToken(descriptor);
            var token = handler.WriteToken(jwt);

            return (token, expires.ToUnixTimeSeconds());
        }
        private bool IsValidToken()
        {
            if (token == null || expiresAt == null)
                return false;

            return DateTimeOffset.FromUnixTimeSeconds(expiresAt.Value) >
                   DateTimeOffset.UtcNow.AddMinutes(5);
        }
    }
}

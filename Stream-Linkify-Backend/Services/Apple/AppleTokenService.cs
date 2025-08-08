using Microsoft.IdentityModel.Tokens;
using Stream_Linkify_Backend.Helpers;
using Stream_Linkify_Backend.Interfaces.Apple;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Stream_Linkify_Backend.Services.Apple
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
            string projectRoot = Path.GetFullPath(
                    Path.Combine(AppContext.BaseDirectory, @"..\..\..\..")
);
            string privateKeyPath = Path.Combine(projectRoot, "Stream-Linkify-Backend", "Keys", $"AuthKey_{keyId}.p8");
            if (!File.Exists(privateKeyPath))
            {
                throw new FileNotFoundException($"Apple Music private key not found at {privateKeyPath}");
            }
            string privateKey = File.ReadAllText(privateKeyPath);

            using var ecdsa = ECDsa.Create();
            ecdsa.ImportFromPem(privateKey);

            var securityKey = new ECDsaSecurityKey(ecdsa)
            {
                KeyId = keyId,
            };

            var creds = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.EcdsaSha256
                );

            var now = DateTimeOffset.UtcNow;
            var expires = now.AddDays(179);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = teamId,
                IssuedAt = now.UtcDateTime,
                Expires = expires.UtcDateTime,
                SigningCredentials = creds
            };

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(descriptor);
            string jwt = handler.WriteToken(securityToken);

            return (jwt, expires.ToUnixTimeSeconds());
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

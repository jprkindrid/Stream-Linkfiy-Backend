using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace Stream_Linkfiy_Backend.Helpers
{
    public class AppleTokenGenerator
    {
        private readonly IConfiguration config;

        public AppleTokenGenerator(IConfiguration config)
        {
            this.config = config;
        }
        public string GenerateDeveloperToken()
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

            var handler = new JwtSecurityTokenHandler();
            var description = new SecurityTokenDescriptor
            {
                Issuer = teamId,
                NotBefore = now.UtcDateTime,
                IssuedAt = now.UtcDateTime,
                Expires = now.AddSeconds(15777000).UtcDateTime, // about 6 months but we're being very specific
                SigningCredentials = creds
            };

            var token = handler.CreateToken(description);

            return handler.WriteToken(token);
        }
    }
}

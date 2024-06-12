using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SoundSphere.Core.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly string _securityKey;

        public SecurityService(IConfiguration config) => _securityKey = config["JWT:SecurityKey"];

        public string GetToken(User user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey)); // verify encoded signature and set it as security key in appsettings.Development.json
            // claims are used to store information about the user which are placed in the second part of the token
            var roleClaim = new Claim("role", role);
            var idClaim = new Claim("userId", user.Id.ToString());
            var infoClaim = new Claim("username", user.Email);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Backend",
                Audience = "Frontend",
                Subject = new ClaimsIdentity(new[] { roleClaim, idClaim, infoClaim }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256) // the token will be signed with this hashing algorithm
            };
            SecurityToken? securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string? token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public bool ValidateToken(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true
            };
            if (!tokenHandler.CanReadToken(tokenString.Replace("Bearer ", "")))
            {
                Console.WriteLine("Invalid token");
                return false;
            }
            tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out var validatedToken);
            return validatedToken != null;
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var randomNumberGenerator = RandomNumberGenerator.Create()) randomNumberGenerator.GetBytes(salt);
            return salt;
        }

        public string HashPassword(string password, byte[] salt) => Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000,
            numBytesRequested: 256 / 8));
    }
}
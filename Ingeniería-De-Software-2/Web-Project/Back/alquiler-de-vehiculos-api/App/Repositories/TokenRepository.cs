using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Entities;


namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        
        private readonly IConfiguration _configuration;
        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Generate(User user)
        {
            var userClaims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Mail),
                new Claim("role", user.Role.ToString())

            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32]; // 256 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        
    }
}
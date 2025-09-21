using Microsoft.IdentityModel.Tokens;
using Minimal.Api.Domain.Entity;
using Minimal.Api.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Minimal.Api.Domain.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetToken(Admin admin)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("Key");
            var issuer = jwtSettings.GetValue<string>("Issuer") ?? "Default-Issuer";
            var audience = jwtSettings.GetValue<string>("Audience") ?? "Default-Audience";
            var expirationMinutes = jwtSettings.GetValue<int>("ExpirationMinutes");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                {
                    new Claim("ID", admin.Id.ToString()),
                    new Claim("Email", admin.Email),
                    new Claim(ClaimTypes.Role, admin.Role.ToString()), 
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

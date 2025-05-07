using Dinner.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dinner.Api.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public string CreateToken(ApplicationUser user)
        {
            // toke key
            var tokenKey = configuration["TokenKey"] ?? throw new Exception("TokenKey not found in configuration");
            if(tokenKey.Length < 64) throw new Exception("TokenKey is too short, must be at least 64 characters");

            // Create key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            // Create claims
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName),
            };

            // Create credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Create token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

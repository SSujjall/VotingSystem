using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VotingSystem.Infrastructure.ExternalServices.JwtService.Config;
using VotingSystem.Infrastructure.Repositories.Interfaces;
using VotingSystem.Domain.Entities;
using VotingSystem.Common.Extensions;

namespace VotingSystem.Infrastructure.ExternalServices.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IAuthRepository _authRepository;

        public JwtService(IOptions<JwtConfig> jwtSettings, IAuthRepository authRepository)
        {
            _jwtConfig = jwtSettings.Value;
            _authRepository = authRepository;
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userRole = await _authRepository.GetUserRole(user);

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.Encrypt()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userRole)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.ValidIssuer,
                audience: _jwtConfig.ValidAudience,
                expires: DateTime.Now.AddHours(2),
                claims: claims,
                signingCredentials: credentials
            );

            return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}

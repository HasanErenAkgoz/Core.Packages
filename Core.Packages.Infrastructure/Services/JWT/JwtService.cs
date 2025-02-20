using Core.Packages.Application.Common.Models.NewFolder;
using Core.Packages.Application.Common.Services.JWT;
using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TokenOptions = Core.Packages.Infrastructure.Configurations.Token.TokenOptions;

namespace Core.Packages.Infrastructure.Services.JWT
{
    public class JwtService : ITokenService
    {
        private readonly TokenOptions _tokenOptions;
        private readonly UserManager<User> _userManager;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private DateTime _accessTokenExpiration;
        public JwtService(UserManager<User> userManager, Microsoft.Extensions.Configuration.IConfiguration configuration, IRolePermissionRepository rolePermissionRepository)
        {
            _userManager = userManager;
            _tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
            _rolePermissionRepository = rolePermissionRepository;

            if (_tokenOptions.SecurityKey.Length < 32)
                throw new InvalidOperationException("Security key must be at least 256 bits (32 characters) long.");
        }

        public async Task<TAccessToken> CreateToken<TAccessToken>(User user)
             where TAccessToken : IAccessToken, new()
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwt = await CreateJwtSecurityToken(_tokenOptions, user, credentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);
            Console.WriteLine($"Security Key is Null: {_tokenOptions.SecurityKey == null}");
            Console.WriteLine($"Security Key: {_tokenOptions.SecurityKey}");
            return new TAccessToken()
            {
                Token = token,
                Expiration = _accessTokenExpiration,
                RefreshToken = GenerateRefreshToken(),
            };
        }

        public async Task<JwtSecurityToken> CreateJwtSecurityToken(TokenOptions tokenOptions,User user,SigningCredentials signingCredentials)
        {
            var jwt = new JwtSecurityToken(
                tokenOptions.Issuer,
                tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: await SetClaims(user),
                signingCredentials: signingCredentials);
            return jwt;
        }

        private async Task<IEnumerable<Claim>> SetClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await GetUserPermissions(user);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email),
  
        };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(permissions.Select(permission => new Claim("Permission", permission)));
            return claims;
        }
        private async Task<List<string>> GetUserPermissions(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return await _rolePermissionRepository.Query()
                .Include(rp => rp.Permission)
                .Where(rp => userRoles.Contains(rp.Role.Name))
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync();
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    

        public static string DecodeToken(string input)
        {
            var handler = new JwtSecurityTokenHandler();
            if (input.StartsWith("Bearer "))
            {
                input = input["Bearer ".Length..];
            }

            return handler.ReadJwtToken(input).ToString();
        }

        public Task<(string Token, string RefreshToken)> GenerateToken(User user)
        {
            throw new NotImplementedException();
        }
    }
}

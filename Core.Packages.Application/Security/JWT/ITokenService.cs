using System.Security.Claims;
using Core.Packages.Domain.Identity;

namespace Core.Packages.Application.Security.JWT;

public interface ITokenService
{
    string GenerateAccessToken(User user, IList<string> roles);
    RefreshToken GenerateRefreshToken(string ipAddress);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    Task RevokeToken(string token);
    Task<bool> ValidateToken(string token);
} 
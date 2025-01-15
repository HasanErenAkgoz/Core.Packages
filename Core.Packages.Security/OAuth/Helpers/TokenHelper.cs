using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Packages.Domain.Security.Models.Configurations;
using Core.Packages.Security.OAuth.Constants;
using Core.Packages.Security.OAuth.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace Core.Packages.Security.OAuth.Helpers;

// Token işlemleri için yardımcı sınıf
public static class TokenHelper
{
    // Access token oluşturma
    public static string CreateAccessToken(
        OAuthOptions options,
        string userId,
        string clientId,
        IEnumerable<string> scopes,
        IEnumerable<Claim> additionalClaims = null)
    {
        var claims = new List<Claim>
        {
            new(OAuthClaimTypes.UserId, userId),
            new(OAuthClaimTypes.ClientId, clientId)
        };

        // Scope'ları ekle
        foreach (var scope in scopes)
        {
            claims.Add(new Claim(OAuthClaimTypes.Scope, scope));
        }

        // Ek claim'leri ekle
        if (additionalClaims != null)
        {
            claims.AddRange(additionalClaims);
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecurityKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(options.AccessTokenExpiration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Refresh token oluşturma
    public static string CreateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    // Token doğrulama
    public static ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch (Exception ex)
        {
            throw new OAuthValidationException("Token validation failed", ex.Message);
        }
    }
} 
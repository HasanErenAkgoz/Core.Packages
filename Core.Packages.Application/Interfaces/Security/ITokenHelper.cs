using System.Security.Claims;

namespace Core.Packages.Security.JWT;

// JWT token işlemleri için servis arayüzü
public interface ITokenHelper
{
    // Access token oluşturma
    string CreateAccessToken(
        int userId,
        string username,
        IEnumerable<string> roles,
        IEnumerable<Claim> additionalClaims = null);

    // Refresh token oluşturma
    string CreateRefreshToken();

    // Token doğrulama
    ClaimsPrincipal ValidateToken(string token);

    // Token'dan claim'leri alma
    IEnumerable<Claim> GetClaims(string token);

    // Token'ın geçerlilik süresini alma
    DateTime GetExpirationDate(string token);

    // Token'dan kullanıcı ID'sini alma
    int GetUserId(string token);
} 
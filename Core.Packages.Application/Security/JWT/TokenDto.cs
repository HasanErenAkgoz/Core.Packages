namespace Core.Packages.Application.Security.JWT;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime RefreshTokenExpirationDate { get; set; }
    public string RefreshToken { get; set; }
} 
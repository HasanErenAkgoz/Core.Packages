// OAuth yapılandırma ayarları
namespace Core.Packages.Domain.Security.Models.Configurations;

public class OAuthOptions
{
    // Token ayarları
    public int AccessTokenExpiration { get; set; } = 60; // Dakika
    public int RefreshTokenExpiration { get; set; } = 7; // Gün
    public string SecurityKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

    // Grant type ayarları
    public bool EnablePasswordFlow { get; set; } = true;
    public bool EnableClientCredentialsFlow { get; set; } = true;
    public bool EnableAuthorizationCodeFlow { get; set; } = true;
    public bool EnableRefreshToken { get; set; } = true;

    // Güvenlik ayarları
    public bool RequireHttps { get; set; } = true;
    public bool ValidateIssuer { get; set; } = true;
    public bool ValidateAudience { get; set; } = true;
    public bool ValidateLifetime { get; set; } = true;
    public bool ValidateIssuerSigningKey { get; set; } = true;

    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
    public string AuthorizationEndpoint { get; set; } = "https://accounts.google.com/o/oauth2/v2/auth";
    public string TokenEndpoint { get; set; } = "https://oauth2.googleapis.com/token";
    public string UserInformationEndpoint { get; set; } = "https://www.googleapis.com/oauth2/v3/userinfo";
    public List<string> Scopes { get; set; } = new();
}
using Core.Packages.Domain.Security.Models;

namespace Core.Packages.Application.Services.Auth;

// OAuth işlemlerini yöneten servis arayüzü
public interface IOAuthService
{
    // Authorization Code akışı için kod oluşturma
    Task<string> CreateAuthorizationCodeAsync(string clientId, string redirectUri, string scope, string state);
    
    // Authorization Code ile token alma
    Task<OAuthToken> ExchangeCodeForTokenAsync(string code, string clientId, string clientSecret, string redirectUri);
    
    // Resource Owner Password akışı ile token alma
    Task<OAuthToken> CreatePasswordTokenAsync(string clientId, string clientSecret, string username, string password, string scope);
    
    // Client Credentials akışı ile token alma
    Task<OAuthToken> CreateClientCredentialsTokenAsync(string clientId, string clientSecret, string scope);
    
    // Refresh token ile yeni token alma
    Task<OAuthToken> RefreshTokenAsync(string refreshToken, string clientId, string clientSecret);
    
    // Token geçerliliğini kontrol etme
    Task<bool> ValidateTokenAsync(string token);
    
    // Token'ı iptal etme
    Task RevokeTokenAsync(string token);
} 
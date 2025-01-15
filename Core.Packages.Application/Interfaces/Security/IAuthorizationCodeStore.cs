using Core.Packages.Domain.Security.Models;

namespace Core.Packages.Security.OAuth.Storage;

// Authorization Code'ları saklamak için kullanılan storage interface'i
public interface IAuthorizationCodeStore
{
    Task SaveAuthorizationCodeAsync(AuthorizationCode code);
    Task<AuthorizationCode?> GetAuthorizationCodeAsync(string code);
    Task MarkAuthorizationCodeAsUsedAsync(string code);
    Task<bool> ValidateAuthorizationCodeAsync(string code, string clientId, string redirectUri);
} 
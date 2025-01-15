namespace Core.Packages.Security.Authentication;

// Kullanıcı doğrulama sonucu
public class AuthenticationResult
{
    public bool IsSuccessful { get; set; }
    public int? UserId { get; set; }
    public string? Username { get; set; }
    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
    public string? ErrorMessage { get; set; }
}

// Kullanıcı doğrulama servisi arayüzü
public interface IAuthenticationService
{
    // Kullanıcı adı ve şifre ile doğrulama
    Task<AuthenticationResult> ValidateCredentialsAsync(string username, string password);

    // Refresh token ile doğrulama
    Task<AuthenticationResult> ValidateRefreshTokenAsync(string refreshToken);

    // Kullanıcının rollerini getir
    Task<IEnumerable<string>> GetUserRolesAsync(int userId);

    // Kullanıcının aktif olup olmadığını kontrol et
    Task<bool> IsUserActiveAsync(int userId);
} 
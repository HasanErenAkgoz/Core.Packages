namespace Core.Packages.Security.JWT.Models;

// JWT token isteği için model
public class TokenRequest
{
    // Kullanıcı bilgileri
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // İptal edilmiş token'ı yenilemek için
    public string? RefreshToken { get; set; }
} 
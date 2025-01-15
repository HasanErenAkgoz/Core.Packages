using Core.Packages.Domain.Enums;

namespace Core.Packages.Application.Services.Auth;

public interface ITwoFactorAuthService
{
    // TOTP (Time-based One-Time Password) işlemleri
    Task<(string SecretKey, string QrCodeUri)> GenerateTotpSecretAsync(string userId, string email);
    Task<bool> VerifyTotpCodeAsync(string userId, string code);
    Task<bool> EnableTotpAsync(string userId, string code);
    Task<bool> DisableTotpAsync(string userId, string code);
    
    // SMS/Email 2FA işlemleri
    Task<string> GenerateEmailCodeAsync(string userId, string email);
    Task<string> GenerateSmsCodeAsync(string userId, string phoneNumber);
    Task<bool> VerifyEmailCodeAsync(string userId, string code);
    Task<bool> VerifySmsCodeAsync(string userId, string code);
    Task<bool> EnableEmailTwoFactorAsync(string userId);
    Task<bool> EnableSmsTwoFactorAsync(string userId);
    Task<bool> DisableEmailTwoFactorAsync(string userId);
    Task<bool> DisableSmsTwoFactorAsync(string userId);
    
    // Backup ve Recovery kodları
    Task<List<string>> GenerateBackupCodesAsync(string userId);
    Task<List<string>> GenerateRecoveryCodesAsync(string userId);
    Task<bool> ValidateBackupCodeAsync(string userId, string code);
    Task<bool> ValidateRecoveryCodeAsync(string userId, string code);
    
    Task<bool> IsTwoFactorEnabledAsync(string userId);
    Task<bool> IsLockedOutAsync(string userId);
    Task ResetLockoutAsync(string userId);
    Task<TwoFactorType> GetEnabledTwoFactorTypeAsync(string userId);
} 
using Core.Packages.Domain.Identity;

namespace Core.Packages.Application.Security.MFA;

public interface ITwoFactorService
{
    Task<string> GenerateTwoFactorTokenAsync(User user);
    Task<bool> ValidateTwoFactorTokenAsync(User user, string token);
    Task<bool> IsTwoFactorEnabledAsync(User user);
    Task EnableTwoFactorAsync(User user);
    Task DisableTwoFactorAsync(User user);
} 
using Core.Packages.Domain.Enums;
using Core.Packages.Security.TwoFactorAuth.Models;
using Core.Packages.Security.TwoFactorAuth.Storage.Models;

namespace Core.Packages.Security.TwoFactorAuth.Storage;

public interface ITwoFactorStore
{
    // TwoFactorData işlemleri
    Task<TwoFactorData?> GetTwoFactorDataAsync(string userId);
    Task SaveTwoFactorDataAsync(TwoFactorData data);
    Task DeleteTwoFactorDataAsync(string userId);
    Task UpdateTwoFactorTypeAsync(string userId, TwoFactorType type);
    Task UpdateLockoutAsync(string userId, DateTime? lockoutEnd, int failedAttempts);
    
    // Code işlemleri
    Task<TwoFactorCode?> GetCodeAsync(string userId, string code, string purpose);
    Task<List<TwoFactorCode>> GetActiveCodesAsync(string userId, string purpose);
    Task SaveCodeAsync(TwoFactorCode code);
    Task MarkCodeAsUsedAsync(string userId, string code);
    Task DeleteExpiredCodesAsync(string userId);
    
    // Cleanup
    Task DeleteAllDataAsync(string userId);
} 
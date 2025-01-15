using Core.Packages.Domain.Enums;
using Core.Packages.Security.TwoFactorAuth.Models;

namespace Core.Packages.Security.TwoFactorAuth.Storage.Models;

public class TwoFactorCode
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public TwoFactorType Type { get; set; }
    public bool IsUsed { get; set; }
    public string Purpose { get; set; } = string.Empty; // "backup", "recovery", "email", "sms"
} 
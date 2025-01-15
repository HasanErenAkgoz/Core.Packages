using Core.Packages.Domain.Common;
using Core.Packages.Domain.Enums;
using Core.Packages.Security.TwoFactorAuth.Models;

namespace Core.Packages.Security.TwoFactorAuth.Storage.EntityFramework.Entities;

public class TwoFactorCodeEntity : Entity<int>
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public TwoFactorType Type { get; set; }
    public bool IsUsed { get; set; }
    public string Purpose { get; set; } = string.Empty;
} 
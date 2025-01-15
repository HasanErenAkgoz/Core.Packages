using Core.Packages.Domain.Common;
using Core.Packages.Domain.Enums;
using Core.Packages.Security.TwoFactorAuth.Models;

namespace Core.Packages.Security.TwoFactorAuth.Storage.EntityFramework.Entities;

public class TwoFactorDataEntity : Entity<int>
{
    public string UserId { get; set; } = string.Empty;
    public TwoFactorType Type { get; set; }
    public byte[]? SecretKey { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int FailedAttempts { get; set; }
} 
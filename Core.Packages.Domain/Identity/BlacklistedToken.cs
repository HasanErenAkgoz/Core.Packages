using Core.Packages.Domain.Common;

namespace Core.Packages.Domain.Identity;

public class BlacklistedToken : BaseEntity<int>, IEntity
{
    public string Token { get; set; }
    public DateTime BlacklistedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
} 
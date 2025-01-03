using Core.Packages.Domain.Common;

namespace Core.Packages.Domain.Security.Permissions.Entities;

public class Permission : BaseEntity<int>, IEntity
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string GroupName { get; set; }
    public string Description { get; set; }
} 
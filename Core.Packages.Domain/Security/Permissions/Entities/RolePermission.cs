using Core.Packages.Domain.Common;

namespace Core.Packages.Domain.Security.Permissions.Entities;

public class RolePermission : BaseEntity<int>, IEntity
{
    public string RoleName { get; set; }
    public string Permission { get; set; }
} 
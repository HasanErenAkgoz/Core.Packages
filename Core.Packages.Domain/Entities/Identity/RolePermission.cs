namespace Core.Packages.Domain.Entities.Identity;

public class RolePermission
{
    public Guid RoleId { get; set; }
    public string Permission { get; set; }
    public Role Role { get; set; }

    public RolePermission()
    {
        Permission = string.Empty;
    }

    public RolePermission(Guid roleId, string permission)
    {
        RoleId = roleId;
        Permission = permission;
    }
} 
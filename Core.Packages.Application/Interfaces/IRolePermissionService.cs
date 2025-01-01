namespace Core.Packages.Application.Interfaces;

public interface IRolePermissionService
{
    Task<bool> HasPermissionAsync(IEnumerable<string> roles, string permission);
    Task AddPermissionToRoleAsync(string roleName, string permission);
    Task RemovePermissionFromRoleAsync(string roleName, string permission);
    Task<IEnumerable<string>> GetRolePermissionsAsync(string roleName);
} 
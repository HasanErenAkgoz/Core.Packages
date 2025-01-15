namespace Core.Packages.Application.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string userId, string permission);
    Task<IList<string>> GetUserPermissionsAsync(string userId);
    Task<bool> AddPermissionToRoleAsync(string roleName, string permission);
    Task<bool> RemovePermissionFromRoleAsync(string roleName, string permission);
    Task<IList<string>> GetRolePermissionsAsync(string roleName);
    Task<bool> HasPermissionAsync(string userId, string[] permissions, bool requireAll = false);
} 
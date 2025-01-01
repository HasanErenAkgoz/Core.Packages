public interface IRolePermissionService
{
    Task<bool> HasPermissionAsync(IEnumerable<string> userRoles, string permission);
    Task<IEnumerable<string>> GetPermissionsAsync(string roleName);
    Task AddPermissionToRoleAsync(string roleName, string permission);
    Task RemovePermissionFromRoleAsync(string roleName, string permission);
} 
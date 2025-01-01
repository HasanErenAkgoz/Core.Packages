using Core.Packages.Domain.Security.Permissions.Models;

public interface IPermissionDefinitionService
{
    IEnumerable<PermissionInfo> GetAllPermissions();
    PermissionInfo GetPermissionInfo(string permissionKey);
} 
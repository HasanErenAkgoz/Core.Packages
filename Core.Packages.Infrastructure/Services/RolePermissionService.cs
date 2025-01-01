using Core.Packages.Application.Interfaces;
using Core.Packages.Application.Repositories;
using Core.Packages.Domain.Security;
using Core.Packages.Domain.Security.Permissions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Packages.Infrastructure.Services;

public class RolePermissionService : IRolePermissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public RolePermissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HasPermissionAsync(IEnumerable<string> roles, string permission)
    {
        var rolePermissions = await _unitOfWork.Repository<RolePermission>()
            .GetListAsync(rp => roles.Contains(rp.RoleName));

        return rolePermissions.Any(rp => rp.Permission == permission);
    }

    public async Task<IEnumerable<string>> GetPermissionsAsync(string roleName)
    {
        var rolePermissions = await _unitOfWork.Repository<RolePermission>()
            .GetListAsync(rp => rp.RoleName == roleName);

        return rolePermissions.Select(rp => rp.Permission);
    }

    public async Task AddPermissionToRoleAsync(string roleName, string permission)
    {
        var exists = await _unitOfWork.Repository<RolePermission>()
            .AnyAsync(rp => rp.RoleName == roleName && rp.Permission == permission);

        if (!exists)
        {
            var rolePermission = new RolePermission
            {
                RoleName = roleName,
                Permission = permission
            };

            await _unitOfWork.Repository<RolePermission>().AddAsync(rolePermission);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task RemovePermissionFromRoleAsync(string roleName, string permission)
    {
        var rolePermission = await _unitOfWork.Repository<RolePermission>()
            .GetAsync(rp => rp.RoleName == roleName && rp.Permission == permission);

        if (rolePermission != null)
        {
            await _unitOfWork.Repository<RolePermission>().DeleteAsync(rolePermission);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<string>> GetRolePermissionsAsync(string roleName)
    {
        var rolePermissions = await _unitOfWork.Repository<RolePermission>()
            .GetListAsync(rp => rp.RoleName == roleName);

        return rolePermissions.Select(rp => rp.Permission);
    }

    public async Task UpdateRolePermissionsAsync(string roleName, IEnumerable<string> permissions)
    {
        // Mevcut izinleri al
        var existingPermissions = await _unitOfWork.Repository<RolePermission>()
            .GetListAsync(rp => rp.RoleName == roleName);

        // Silinecek izinleri bul ve sil
        var permissionsToRemove = existingPermissions
            .Where(ep => !permissions.Contains(ep.Permission));

        foreach (var permission in permissionsToRemove)
        {
            await _unitOfWork.Repository<RolePermission>().DeleteAsync(permission);
        }

        // Yeni izinleri ekle
        var existingPermissionNames = existingPermissions.Select(ep => ep.Permission);
        var permissionsToAdd = permissions
            .Where(p => !existingPermissionNames.Contains(p));

        foreach (var permission in permissionsToAdd)
        {
            await _unitOfWork.Repository<RolePermission>().AddAsync(new RolePermission
            {
                RoleName = roleName,
                Permission = permission
            });
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
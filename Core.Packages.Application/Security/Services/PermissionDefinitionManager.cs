using Core.Packages.Domain.Security.Permissions.Models;

public class PermissionDefinitionManager : IPermissionDefinitionService
{
    private readonly Dictionary<string, PermissionInfo> _permissions;

    public PermissionDefinitionManager()
    {
        _permissions = new Dictionary<string, PermissionInfo>
        {
            ["Invoices.Create"] = new PermissionInfo 
            { 
                Key = "Invoices.Create",
                DisplayName = "Fatura Oluşturma",
                GroupName = "Fatura İşlemleri",
                Description = "Yeni fatura oluşturma yetkisi"
            },
            ["Invoices.Read"] = new PermissionInfo 
            { 
                Key = "Invoices.Read",
                DisplayName = "Fatura Görüntüleme",
                GroupName = "Fatura İşlemleri",
                Description = "Faturaları görüntüleme yetkisi"
            },
            // ... diğer permission'lar
        };
    }

    public IEnumerable<PermissionInfo> GetAllPermissions()
    {
        return _permissions.Values;
    }

    public PermissionInfo GetPermissionInfo(string permissionKey)
    {
        return _permissions.GetValueOrDefault(permissionKey);
    }
} 
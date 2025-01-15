namespace Core.Packages.Security.Authorization;

public static class Permissions
{
    public static class Accounting
    {
        public const string View = "Permissions.Accounting.View";
        public const string Create = "Permissions.Accounting.Create";
        public const string Update = "Permissions.Accounting.Update";
        public const string Delete = "Permissions.Accounting.Delete";
        public const string Approve = "Permissions.Accounting.Approve";
        public const string Export = "Permissions.Accounting.Export";
    }

    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Create = "Permissions.Users.Create";
        public const string Update = "Permissions.Users.Update";
        public const string Delete = "Permissions.Users.Delete";
        public const string ManageRoles = "Permissions.Users.ManageRoles";
    }

    public static class Roles
    {
        public const string View = "Permissions.Roles.View";
        public const string Create = "Permissions.Roles.Create";
        public const string Update = "Permissions.Roles.Update";
        public const string Delete = "Permissions.Roles.Delete";
        public const string ManagePermissions = "Permissions.Roles.ManagePermissions";
    }
} 
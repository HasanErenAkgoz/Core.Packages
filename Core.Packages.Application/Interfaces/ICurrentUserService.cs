namespace Core.Packages.Application.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }
    bool IsAuthenticated { get; }
    IEnumerable<string> Roles { get; }
    IEnumerable<string> Permissions { get; }
} 
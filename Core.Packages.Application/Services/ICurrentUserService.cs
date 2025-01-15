namespace Core.Packages.Application.Services;

public interface ICurrentUserService
{
    int Id { get; }
    string? Email { get; }
    string? Name { get; }
    bool IsAuthenticated { get; }
    IEnumerable<string> Roles { get; }
} 
namespace Core.Packages.Application.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? IpAddress { get; }
    string? Email { get; }
    string? Name { get; }
} 
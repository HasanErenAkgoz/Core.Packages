namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task RegisterAsync(string fullName, string email, string password);
}
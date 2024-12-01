using Infrastructure.Infrastructure.Identity;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task CreateUserAsync(ApplicationUser user, string password);
}
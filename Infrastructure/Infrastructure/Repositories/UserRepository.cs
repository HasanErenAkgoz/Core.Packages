using Application.Interfaces.Repositories;
using Infrastructure.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task CreateUserAsync(ApplicationUser user, string password)
    {
        await _userManager.CreateAsync(user, password);
    }
}

using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Infrastructure.Identity;

namespace Infrastructure.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userRepository.FindByEmailAsync(email);
        if (user == null || !await _userRepository.CheckPasswordAsync(user, password))
            throw new Exception("Invalid credentials");

        return _jwtService.GenerateToken(user);
    }

    public async Task RegisterAsync(string fullName, string email, string password)
    {
        var user = new ApplicationUser
        {
            FullName = fullName,
            Email = email,
            UserName = email
        };

        await _userRepository.CreateUserAsync(user, password);
    }
}
using Infrastructure.Infrastructure.Identity;

namespace Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);

}
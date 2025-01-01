using Core.Packages.Domain.Identity;

namespace Core.Packages.Application.Security.JWT;

public interface ITokenHelper
{
    string CreateToken(User user, IList<string> roles);
} 
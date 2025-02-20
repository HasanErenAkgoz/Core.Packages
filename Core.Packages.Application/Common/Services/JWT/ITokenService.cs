using Core.Packages.Application.Common.Models.NewFolder;
using Core.Packages.Domain.Entities;

namespace Core.Packages.Application.Common.Services.JWT
{
    public interface ITokenService
    {
        Task<TAccessToken> CreateToken<TAccessToken>(User user)

          where TAccessToken : IAccessToken, new();

        string GenerateRefreshToken();
        public Task<(string Token, string RefreshToken)> GenerateToken(User user);
    }
}

using Core.Packages.Domain.Identity;

namespace Core.Packages.Application.Security;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<IList<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);
    Task AddAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
}
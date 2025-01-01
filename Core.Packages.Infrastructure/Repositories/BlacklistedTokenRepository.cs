using Core.Packages.Application.Security;
using Core.Packages.Domain.Identity;
using Microsoft.EntityFrameworkCore;

public class BlacklistedTokenRepository : IBlacklistedTokenRepository
{
    private readonly DbContext _context;

    public BlacklistedTokenRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsTokenBlacklistedAsync(string token)
    {
        return await _context.Set<BlacklistedToken>()
            .AnyAsync(x => x.Token == token && x.ExpiryDate > DateTime.UtcNow);
    }

    public async Task BlacklistTokenAsync(string token, DateTime expiryDate)
    {
        var blacklistedToken = new BlacklistedToken
        {
            Token = token,
            BlacklistedDate = DateTime.UtcNow,
            ExpiryDate = expiryDate
        };

        await _context.Set<BlacklistedToken>().AddAsync(blacklistedToken);
        await _context.SaveChangesAsync();
    }
} 
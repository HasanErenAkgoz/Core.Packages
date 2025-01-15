using Core.Packages.Domain.Enums;
using Core.Packages.Security.TwoFactorAuth.Storage.EntityFramework.Entities;
using Core.Packages.Security.TwoFactorAuth.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Core.Packages.Security.TwoFactorAuth.Storage.EntityFramework;

public class EfTwoFactorStore<TContext> : ITwoFactorStore
{
    private readonly DbContext _context;
    private readonly DbSet<TwoFactorDataEntity> _twoFactorData;
    private readonly DbSet<TwoFactorCodeEntity> _twoFactorCodes;

    public EfTwoFactorStore(DbContext context)
    {
        _context = context;
        _twoFactorData = context.Set<TwoFactorDataEntity>();
        _twoFactorCodes = context.Set<TwoFactorCodeEntity>();
    }

    public async Task<TwoFactorData?> GetTwoFactorDataAsync(string userId)
    {
        var entity = await _twoFactorData.FirstOrDefaultAsync(x => x.UserId == userId);
        if (entity == null) return null;

        return new TwoFactorData
        {
            UserId = entity.UserId,
            Type = entity.Type,
            SecretKey = entity.SecretKey,
            IsEnabled = entity.IsEnabled,
            LockoutEnd = entity.LockoutEnd,
            FailedAttempts = entity.FailedAttempts
        };
    }

    public async Task SaveTwoFactorDataAsync(TwoFactorData data)
    {
        var entity = await _twoFactorData.FirstOrDefaultAsync(x => x.UserId == data.UserId);
        if (entity == null)
        {
            entity = new TwoFactorDataEntity
            {
                UserId = data.UserId
            };
            _twoFactorData.Add(entity);
        }

        entity.Type = data.Type;
        entity.SecretKey = data.SecretKey;
        entity.IsEnabled = data.IsEnabled;
        entity.LockoutEnd = data.LockoutEnd;
        entity.FailedAttempts = data.FailedAttempts;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteTwoFactorDataAsync(string userId)
    {
        var entity = await _twoFactorData.FirstOrDefaultAsync(x => x.UserId == userId);
        if (entity != null)
        {
            _twoFactorData.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateTwoFactorTypeAsync(string userId, TwoFactorType type)
    {
        var entity = await _twoFactorData.FirstOrDefaultAsync(x => x.UserId == userId);
        if (entity != null)
        {
            entity.Type = type;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateLockoutAsync(string userId, DateTime? lockoutEnd, int failedAttempts)
    {
        var entity = await _twoFactorData.FirstOrDefaultAsync(x => x.UserId == userId);
        if (entity != null)
        {
            entity.LockoutEnd = lockoutEnd;
            entity.FailedAttempts = failedAttempts;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<TwoFactorCode?> GetCodeAsync(string userId, string code, string purpose)
    {
        var entity = await _twoFactorCodes
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Code == code && x.Purpose == purpose);
        
        if (entity == null) return null;

        return new TwoFactorCode
        {
            UserId = entity.UserId,
            Code = entity.Code,
            ExpiresAt = entity.ExpiresAt,
            Type = entity.Type,
            IsUsed = entity.IsUsed,
            Purpose = entity.Purpose
        };
    }

    public async Task<List<TwoFactorCode>> GetActiveCodesAsync(string userId, string purpose)
    {
        var query = _twoFactorCodes.AsQueryable();
        
        var filteredQuery = await query.Where(x => x.UserId == userId && 
                                           x.Purpose == purpose && 
                                           !x.IsUsed && 
                                           x.ExpiresAt > DateTime.UtcNow).ToListAsync();

        var result = filteredQuery
            .Select(x => new TwoFactorCode
            {
                UserId = x.UserId,
                Code = x.Code,
                ExpiresAt = x.ExpiresAt,
                Type = x.Type,
                IsUsed = x.IsUsed,
                Purpose = x.Purpose
            }).ToList();

        return result;
    }

    public async Task SaveCodeAsync(TwoFactorCode code)
    {
        var entity = new TwoFactorCodeEntity
        {
            UserId = code.UserId,
            Code = code.Code,
            ExpiresAt = code.ExpiresAt,
            Type = code.Type,
            IsUsed = code.IsUsed,
            Purpose = code.Purpose
        };

        _twoFactorCodes.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task MarkCodeAsUsedAsync(string userId, string code)
    {
        var entity = await _twoFactorCodes.FirstOrDefaultAsync(x => x.UserId == userId && x.Code == code);
        if (entity != null)
        {
            entity.IsUsed = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteExpiredCodesAsync(string userId)
    {
        var expiredCodes = await _twoFactorCodes
            .Where(x => x.UserId == userId && x.ExpiresAt <= DateTime.UtcNow)
            .ToListAsync();

        if (expiredCodes.Any())
        {
            _twoFactorCodes.RemoveRange(expiredCodes);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllDataAsync(string userId)
    {
        var data = await _twoFactorData.FirstOrDefaultAsync(x => x.UserId == userId);
        if (data != null)
            _twoFactorData.Remove(data);

        var codes = await _twoFactorCodes.Where(x => x.UserId == userId).ToListAsync();
        if (codes.Any())
            _twoFactorCodes.RemoveRange(codes);

        await _context.SaveChangesAsync();
    }
} 
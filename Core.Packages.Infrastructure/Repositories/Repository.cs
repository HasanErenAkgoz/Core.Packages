using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Core.Packages.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(object id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, 
        Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        IQueryable<T> query = DbSet;
        
        if (include != null)
            query = include(query);

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<List<T>> GetListAsync(
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IQueryable<T>> include = null,
        int? pageIndex = null,
        int? pageSize = null)
    {
        IQueryable<T> query = DbSet;

        if (predicate != null)
            query = query.Where(predicate);

        if (include != null)
            query = include(query);

        if (pageIndex.HasValue && pageSize.HasValue)
            query = query.Skip(pageIndex.Value * pageSize.Value).Take(pageSize.Value);

        return await query.ToListAsync();
    }
} 
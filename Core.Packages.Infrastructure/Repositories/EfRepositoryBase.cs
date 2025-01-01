using System.Linq.Expressions;
using Core.Packages.Application.Repositories;
using Core.Packages.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Core.Packages.Infrastructure.Repositories;

public class EfRepositoryBase<T, TContext> : IAsyncRepository<T> where TContext : DbContext where T : class
{
    protected readonly TContext Context;
    protected readonly DbSet<T> DbSet;

    public EfRepositoryBase(TContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public IQueryable<T> Query() => DbSet;

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IList<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = DbSet;
        if (predicate != null)
            query = query.Where(predicate);
        return await query.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }
} 
using System.Linq.Expressions;
using Core.Packages.Application.Repositories;
using Core.Packages.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Core.Packages.Infrastructure.Repositories;

public class EfCoreRepositoryBase<TEntity, TContext> : IEfCoreRepository<TEntity>
    where TEntity : class, IEntity where TContext : DbContext
{
    public EfCoreRepositoryBase(TContext context)
    {
        Context = context;
    }

    protected TContext Context { get; }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var entityEntry = await Context.AddAsync(entity);
        return entityEntry.Entity;
    }

    public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await Context.Set<T>().AddRangeAsync(entities);
    }


    public TEntity Update(TEntity entity)
    {
        Context.Update(entity);
        return entity;
    }

    public async Task DeleteAsync(TEntity entity)
    {
        Context.Remove(entity);
        await Task.CompletedTask;
    }


    public TEntity Get(Expression<Func<TEntity, bool>> expression)
    {
        return Context.Set<TEntity>().FirstOrDefault(expression);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await Context.Set<TEntity>().AsQueryable().FirstOrDefaultAsync(expression);
    }

    public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression = null)
    {
        return expression == null
            ? Context.Set<TEntity>().AsNoTracking()
            : Context.Set<TEntity>().Where(expression).AsNoTracking();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Context.Set<TEntity>().AnyAsync(predicate);

    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression = null)
    {
        return expression == null
            ? await Context.Set<TEntity>().ToListAsync()
            : await Context.Set<TEntity>().Where(expression).ToListAsync();
    }

    public int SaveChanges()
    {
        return Context.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        return Context.SaveChangesAsync();
    }

    public IQueryable<TEntity> Query()
    {
        return Context.Set<TEntity>();
    }

    public Task<int> Execute(FormattableString interpolatedQueryString)
    {
        return Context.Database.ExecuteSqlInterpolatedAsync(interpolatedQueryString);
    }

    /// <summary>
    /// Transactional operations is prohibited when working with InMemoryDb!
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="action"></param>
    /// <param name="successAction"></param>
    /// <param name="exceptionAction"></param>
    /// <returns></returns>
    public TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null,
        Action<Exception> exceptionAction = null)
    {
        var result = default(TResult);
        try
        {
            if (Context.Database.ProviderName.EndsWith("InMemory"))
            {
                result = action();
                SaveChanges();
            }
            else
            {
                using var tx = Context.Database.BeginTransaction();
                try
                {
                    result = action();
                    SaveChanges();
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }

            successAction?.Invoke();
        }
        catch (Exception ex)
        {
            if (exceptionAction == null)
            {
                throw;
            }

            exceptionAction(ex);
        }

        return result;
    }

    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> expression = null)
    {
        if (expression == null)
        {
            return await Context.Set<TEntity>().CountAsync();
        }
        else
        {
            return await Context.Set<TEntity>().CountAsync(expression);
        }
    }

    public int GetCount(Expression<Func<TEntity, bool>> expression = null)
    {
        return expression == null ? Context.Set<TEntity>().Count() : Context.Set<TEntity>().Count(expression);
    }
}
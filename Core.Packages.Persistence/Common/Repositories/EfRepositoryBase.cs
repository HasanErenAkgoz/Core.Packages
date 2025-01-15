using Core.Packages.Application.Repositories;
using Core.Packages.Application.Services;
using Core.Packages.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Packages.Persistence.Common.Repositories;

public class EfRepositoryBase<TEntity, TId, TContext> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TContext : DbContext
{
    protected readonly TContext Context;
    protected readonly ICurrentUserService CurrentUserService;

    public EfRepositoryBase(TContext context, ICurrentUserService currentUserService)
    {
        Context = context;
        CurrentUserService = currentUserService;
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Context.Set<TEntity>();
        
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        
        if (include != null)
            queryable = include(queryable);
        
        if (!withDeleted)
            queryable = queryable.Where(x => x.Status != EntityStatus.Deleted);
        
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IList<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Context.Set<TEntity>();

        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (include != null)
            queryable = include(queryable);

        if (!withDeleted)
            queryable = queryable.Where(x => x.Status != EntityStatus.Deleted);

        if (predicate != null)
            queryable = queryable.Where(predicate);

        if (orderBy != null)
            queryable = orderBy(queryable);

        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Context.Set<TEntity>();

        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (!withDeleted)
            queryable = queryable.Where(x => x.Status != EntityStatus.Deleted);

        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.AnyAsync(cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.Status = EntityStatus.Active;
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IList<TEntity>> AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
            entity.Status = EntityStatus.Active;
        await Context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.ModifiedById = CurrentUserService.Id;
        entity.ModifiedDate = DateTime.UtcNow;
        Context.Set<TEntity>().Update(entity);
        return entity;
    }

    public async Task<IList<TEntity>> UpdateRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.ModifiedById = CurrentUserService.Id;
            entity.ModifiedDate = DateTime.UtcNow;
        }
        Context.Set<TEntity>().UpdateRange(entities);
        return entities;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, CancellationToken cancellationToken = default)
    {
        if (!permanent)
        {
            entity.DeletedById = CurrentUserService.Id;
            entity.DeletedDate = DateTime.UtcNow;
            entity.Status = EntityStatus.Deleted;
            Context.Set<TEntity>().Update(entity);
        }
        else
        {
            Context.Set<TEntity>().Remove(entity);
        }
        return entity;
    }

    public async Task<IList<TEntity>> DeleteRangeAsync(IList<TEntity> entities, bool permanent = false, CancellationToken cancellationToken = default)
    {
        if (!permanent)
        {
            foreach (var entity in entities)
            {
                entity.DeletedById = CurrentUserService.Id;
                entity.DeletedDate = DateTime.UtcNow;
                entity.Status = EntityStatus.Deleted;
            }
            Context.Set<TEntity>().UpdateRange(entities);
        }
        else
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
        return entities;
    }
} 
using Core.Packages.Application.Repositories;
using Core.Packages.CrossCuttingConcerns.Exceptions.Types;
using Core.Packages.Domain.Common;

namespace Core.Packages.Application.Services;

public abstract class BaseService<TEntity, TId> : IBaseService<TEntity, TId>
    where TEntity : Entity<TId>
{
    protected readonly IRepository<TEntity, TId> Repository;
    protected readonly IUnitOfWork UnitOfWork;

    protected BaseService(IRepository<TEntity, TId> repository, IUnitOfWork unitOfWork)
    {
        Repository = repository;
        UnitOfWork = unitOfWork;
    }

    public virtual async Task<TEntity?> GetAsync(TId id)
    {
        var entity = await Repository.GetAsync(x => x.Id.Equals(id));
        if (entity == null)
            throw new BusinessException($"{typeof(TEntity).Name} bulunamadı");
            
        return entity;
    }

    public virtual async Task<IList<TEntity>> GetListAsync()
    {
        return await Repository.GetListAsync();
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await Repository.AddAsync(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var existingEntity = await GetAsync(entity.Id);
        
        await Repository.UpdateAsync(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> DeleteAsync(TId id, bool permanent = false)
    {
        var entity = await GetAsync(id);
        
        await Repository.DeleteAsync(entity, permanent);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }
} 
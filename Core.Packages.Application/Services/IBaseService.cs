using Core.Packages.Domain.Common;

namespace Core.Packages.Application.Services;

public interface IBaseService<TEntity, TId> where TEntity : Entity<TId>
{
    Task<TEntity?> GetAsync(TId id);
    Task<IList<TEntity>> GetListAsync();
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> DeleteAsync(TId id, bool permanent = false);
} 
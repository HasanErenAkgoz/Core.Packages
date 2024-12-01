namespace Application.Interfaces.EntityFrameworkCore;

public interface IWriteRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    Task<TEntity> AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    Task DeleteByIdAsync(TId id);
}
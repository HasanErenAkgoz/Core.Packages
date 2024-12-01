using Application.Interfaces.EntityFrameworkCore;

namespace Infrastructure.Repositories.EntityFrameworkCore;

public class WriteRepository<TEntity, TId> : IWriteRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    public Task<TEntity> AddAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(TId id)
    {
        throw new NotImplementedException();
    }
}
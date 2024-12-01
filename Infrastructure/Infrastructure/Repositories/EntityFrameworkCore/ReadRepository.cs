using System.Linq.Expressions;
using Application.Interfaces.EntityFrameworkCore;
using Domain.Dtos.Pagination;

namespace Infrastructure.Repositories.EntityFrameworkCore;

public class ReadRepository<TEntity, TId> : IReadRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    public IQueryable<TEntity> GetAllAsync(bool tracking = true)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> GetByIdAsync(Guid id, bool tracking = true)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TField>> GetUniqueValuesAsync<TField>(Expression<Func<TEntity, TField>> selector)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<TEntity>> GetPagedAsync(PaginationQuery query,
        Expression<Func<TEntity, bool>>? filter = null)
    {
        throw new NotImplementedException();
    }
}
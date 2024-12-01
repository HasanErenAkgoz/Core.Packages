using System.Linq.Expressions;
using Domain.Dtos.Pagination;

namespace Application.Interfaces.EntityFrameworkCore;

public interface IReadRepository<TEntity,TId> where TEntity : BaseEntity<TId>
{
    IQueryable<TEntity> GetAllAsync(bool tracking = true);
    IQueryable<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true);
    Task<TEntity?> GetByIdAsync(Guid id, bool tracking = true);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? expression = null);
    Task<IReadOnlyList<TField>> GetUniqueValuesAsync<TField>(Expression<Func<TEntity, TField>> selector);
    Task<PagedResult<TEntity>> GetPagedAsync(PaginationQuery query, Expression<Func<TEntity, bool>>? filter = null);

}
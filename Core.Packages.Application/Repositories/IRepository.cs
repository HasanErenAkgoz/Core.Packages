using Core.Packages.Domain.Common;
using System.Linq.Expressions;

namespace Core.Packages.Application.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity<int>
{
    IQueryable<TEntity> Query();
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(int id);
    Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null);
    Task<TEntity> AddAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    void Update(TEntity entity);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
} 
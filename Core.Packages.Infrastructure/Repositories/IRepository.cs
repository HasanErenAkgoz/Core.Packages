using System.Linq.Expressions;

namespace Core.Packages.Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAsync(Expression<Func<T, bool>> predicate, 
        Func<IQueryable<T>, IQueryable<T>> include = null);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetListAsync(
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IQueryable<T>> include = null,
        int? pageIndex = null,
        int? pageSize = null);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
} 
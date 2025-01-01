using System.Linq.Expressions;

namespace Core.Packages.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(object id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    IQueryable<T> Query();
}
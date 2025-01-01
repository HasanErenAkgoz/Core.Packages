using System.Linq.Expressions;
using Core.Packages.Domain.Common;

namespace Core.Packages.Application.Repositories;

public interface IAsyncRepository<T>
{
    IQueryable<T> Query();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(int id);
    Task<IList<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null);
    Task<T> AddAsync(T entity);
    void Update(T entity);
    Task DeleteAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
} 
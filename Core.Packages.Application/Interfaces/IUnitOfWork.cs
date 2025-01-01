namespace Core.Packages.Application.Interfaces;
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    IGenericRepository<T> Repository<T>() where T : class;
    Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;
}
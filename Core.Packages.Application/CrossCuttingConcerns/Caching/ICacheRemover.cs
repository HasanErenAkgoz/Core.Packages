namespace Core.Packages.Application.Pipelines.Caching;

public interface ICacheRemover
{
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByGroupAsync(string groupKey, CancellationToken cancellationToken = default);
} 
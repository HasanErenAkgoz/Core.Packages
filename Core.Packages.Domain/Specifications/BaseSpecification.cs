using System.Linq.Expressions;

namespace Core.Packages.Domain.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }
} 
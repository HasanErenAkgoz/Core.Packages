using System.Linq.Expressions;
using Core.Packages.Domain.Specifications;

namespace Core.Packages.Domain.Specifications;

public class SpecificationBuilder<T>
{
    private readonly List<ISpecification<T>> _specifications = new();

    public SpecificationBuilder<T> AddSpecification(ISpecification<T> specification)
    {
        _specifications.Add(specification);
        return this;
    }

    public Expression<Func<T, bool>> BuildExpression()
    {
        if (!_specifications.Any())
            return x => true;

        var firstSpec = _specifications[0];
        var expression = firstSpec.ToExpression();

        for (var i = 1; i < _specifications.Count; i++)
        {
            var spec = _specifications[i];
            expression = expression.And(spec.ToExpression());
        }

        return expression;
    }
}
using System.Linq.Expressions;

namespace Core.Packages.Application.Common.Dynamic;

public class DynamicQuery<T>
{
    public IList<Expression<Func<T, bool>>> Filters { get; private set; }
    public IList<DynamicInclude> Includes { get; private set; }
    public IList<DynamicOrderBy> OrderBys { get; private set; }
    public int PageSize { get; private set; }
    public int PageNumber { get; private set; }

    public DynamicQuery()
    {
        Filters = new List<Expression<Func<T, bool>>>();
        Includes = new List<DynamicInclude>();
        OrderBys = new List<DynamicOrderBy>();
        PageSize = 10;  // Varsayılan sayfa boyutu
        PageNumber = 1; // Varsayılan sayfa numarası
    }

    public DynamicQuery<T> AddFilter(Expression<Func<T, bool>> filter)
    {
        Filters.Add(filter);
        return this;
    }

    public DynamicQuery<T> AddInclude(string propertyPath)
    {
        Includes.Add(new DynamicInclude(propertyPath));
        return this;
    }

    public DynamicQuery<T> AddOrderBy(string propertyName, OrderByType orderByType = OrderByType.Asc)
    {
        OrderBys.Add(new DynamicOrderBy(propertyName, orderByType));
        return this;
    }

    public DynamicQuery<T> SetPaging(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 ? pageSize : 10;
        return this;
    }
} 
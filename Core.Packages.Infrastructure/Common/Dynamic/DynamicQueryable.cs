using System.Linq.Expressions;
using Core.Packages.Application.Common.Dynamic;
using Core.Packages.Infrastructure.Common.Response;
using Microsoft.EntityFrameworkCore;

namespace Core.Packages.Infrastructure.Common.Dynamic;

public static class DynamicQueryable
{
    public static IQueryable<T> ToDynamic<T>(
        this IQueryable<T> query,
        DynamicQuery<T> dynamicQuery) where T : class
    {
        if (dynamicQuery.Filters != null && dynamicQuery.Filters.Any())
            query = dynamicQuery.Filters.Aggregate(query, (current, filter) => current.Where(filter));

        if (dynamicQuery.Includes != null && dynamicQuery.Includes.Any())
            query = dynamicQuery.Includes.Aggregate(query, (current, include) => 
                current.Include(include.PropertyPath));

        if (dynamicQuery.OrderBys != null && dynamicQuery.OrderBys.Any())
        {
            var ordered = ApplyOrder(query, dynamicQuery.OrderBys[0]);

            for (var i = 1; i < dynamicQuery.OrderBys.Count; i++)
                ordered = ApplyOrder(ordered, dynamicQuery.OrderBys[i]);

            query = ordered;
        }

        return query;
    }

    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> query,
        DynamicQuery<T> dynamicQuery,
        CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var dynamicResult = query.ToDynamic(dynamicQuery);
            
            var totalCount = await dynamicResult.CountAsync(cancellationToken);
            
            var items = await dynamicResult
                .Skip((dynamicQuery.PageNumber - 1) * dynamicQuery.PageSize)
                .Take(dynamicQuery.PageSize)
                .ToListAsync(cancellationToken);

            return PaginatedResult<T>.Success(
                data: items,
                currentPage: dynamicQuery.PageNumber,
                pageSize: dynamicQuery.PageSize,
                totalCount: totalCount);
        }
        catch (Exception ex)
        {
            return PaginatedResult<T>.Fail($"Sayfalama işlemi başarısız: {ex.Message}");
        }
    }

    private static IOrderedQueryable<T> ApplyOrder<T>(
        IQueryable<T> source,
        DynamicOrderBy orderBy)
    {
        var property = typeof(T).GetProperty(orderBy.PropertyName)
            ?? throw new ArgumentException($"Property {orderBy.PropertyName} not found on type {typeof(T).Name}");

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);
        var orderExpression = Expression.Lambda(propertyAccess, parameter);

        var methodName = orderBy.OrderByType == OrderByType.Asc ? "OrderBy" : "OrderByDescending";
        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), property.PropertyType },
            source.Expression,
            Expression.Quote(orderExpression));

        return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(resultExpression);
    }
} 
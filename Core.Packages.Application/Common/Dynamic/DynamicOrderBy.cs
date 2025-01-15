namespace Core.Packages.Application.Common.Dynamic;

public class DynamicOrderBy
{
    public string PropertyName { get; }
    public OrderByType OrderByType { get; }

    public DynamicOrderBy(string propertyName, OrderByType orderByType)
    {
        PropertyName = propertyName;
        OrderByType = orderByType;
    }
} 
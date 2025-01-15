namespace Core.Packages.Application.Common.Dynamic;

public class DynamicInclude
{
    public string PropertyPath { get; }

    public DynamicInclude(string propertyPath)
    {
        PropertyPath = propertyPath;
    }
} 
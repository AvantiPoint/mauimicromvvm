using System.Reflection;

namespace MauiMicroMvvm.Internals;

internal sealed class ReflectionQueryProperty : IQueryProperty
{
    public ReflectionQueryProperty(PropertyInfo propertyInfo)
    {
        Name = propertyInfo.Name;
        PropertyType = propertyInfo.PropertyType;
    }

    public string Name { get; }

    public Type PropertyType { get; }
}

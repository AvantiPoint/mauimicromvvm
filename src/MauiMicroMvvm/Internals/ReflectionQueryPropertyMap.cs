using System.Reflection;

namespace MauiMicroMvvm.Internals;

internal sealed class ReflectionQueryPropertyMap : IQueryPropertyMap
{
    private readonly IReadOnlyDictionary<string, IQueryProperty> _properties;

    public ReflectionQueryPropertyMap(Type viewModelType)
    {
        var properties = new Dictionary<string, IQueryProperty>(StringComparer.InvariantCultureIgnoreCase);
        foreach (var property in viewModelType.GetProperties())
        {
            properties.TryAdd(property.Name, new ReflectionQueryProperty(property));
        }

        _properties = properties;
    }

    public bool TryGetProperty(string key, out IQueryProperty property)
    {
        return _properties.TryGetValue(key, out property!);
    }
}

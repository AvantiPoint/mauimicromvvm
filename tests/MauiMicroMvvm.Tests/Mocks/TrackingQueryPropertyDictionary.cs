using System.Collections;
using System.Reflection;

namespace MauiMicroMvvm.Tests.Mocks;

internal class TrackingQueryPropertyDictionary : IReadOnlyDictionary<string, PropertyInfo>
{
    private readonly IReadOnlyDictionary<string, PropertyInfo> _properties;

    public TrackingQueryPropertyDictionary(IReadOnlyDictionary<string, PropertyInfo> properties)
    {
        _properties = properties;
    }

    public int TryGetValueCount { get; private set; }

    public IEnumerable<string> Keys => throw new InvalidOperationException("Property metadata should not be enumerated during query application.");

    public IEnumerable<PropertyInfo> Values => throw new InvalidOperationException("Property metadata should not be enumerated during query application.");

    public int Count => _properties.Count;

    public PropertyInfo this[string key] => _properties[key];

    public bool ContainsKey(string key)
    {
        return _properties.ContainsKey(key);
    }

    public bool TryGetValue(string key, out PropertyInfo value)
    {
        TryGetValueCount++;
        return _properties.TryGetValue(key, out value!);
    }

    public IEnumerator<KeyValuePair<string, PropertyInfo>> GetEnumerator()
    {
        throw new InvalidOperationException("Property metadata should not be enumerated during query application.");
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

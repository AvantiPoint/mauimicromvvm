using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal sealed class TrackingQueryPropertyMap : IQueryPropertyMap
{
    private readonly IReadOnlyDictionary<string, IQueryProperty> _properties;

    public TrackingQueryPropertyMap(IReadOnlyDictionary<string, IQueryProperty> properties)
    {
        _properties = properties;
    }

    public int TryGetPropertyCount { get; private set; }

    public bool TryGetProperty(string key, out IQueryProperty property)
    {
        TryGetPropertyCount++;
        return _properties.TryGetValue(key, out property!);
    }
}

using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal sealed class TrackingQueryParameterMap : IQueryParameterMap
{
    private readonly IReadOnlyDictionary<string, IQueryParameterSetter> _setters;

    public TrackingQueryParameterMap(IReadOnlyDictionary<string, IQueryParameterSetter> setters)
    {
        _setters = setters;
    }

    public int TryGetSetterCount { get; private set; }

    public bool TryGetSetter(string key, out IQueryParameterSetter setter)
    {
        TryGetSetterCount++;
        return _setters.TryGetValue(key, out setter!);
    }

    public bool TrySet(object target, string key, object? value)
    {
        return TryGetSetter(key, out var setter) && setter.TrySet(target, value);
    }
}

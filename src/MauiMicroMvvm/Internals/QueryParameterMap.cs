namespace MauiMicroMvvm.Internals;

internal sealed class QueryParameterMap : IQueryParameterMap
{
    private IReadOnlyDictionary<string, IQueryParameterSetter> _setters = new Dictionary<string, IQueryParameterSetter>(StringComparer.InvariantCultureIgnoreCase);

    public QueryParameterMap()
    {
    }

    public QueryParameterMap(IEnumerable<IQueryParameterSetter> setters)
    {
        LoadProperties(setters);
    }

    public void LoadProperties(IEnumerable<IQueryParameterSetter> setters)
    {
        ArgumentNullException.ThrowIfNull(setters);

        var updatedSetters = new Dictionary<string, IQueryParameterSetter>(StringComparer.InvariantCultureIgnoreCase);
        foreach (var setter in setters)
        {
            updatedSetters.TryAdd(setter.Name, setter);
        }

        _setters = updatedSetters;
    }

    public bool TryGetSetter(string key, out IQueryParameterSetter setter)
    {
        return _setters.TryGetValue(key, out setter!);
    }

    public bool TrySet(object target, string key, object? value)
    {
        return TryGetSetter(key, out var setter) && setter.TrySet(target, value);
    }
}
